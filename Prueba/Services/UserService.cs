using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Prueba.DTOs;
using Prueba.Facade;
using Prueba.Models;
using Prueba.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Prueba.Services
{
    public class UserService
    {
        private readonly UnitOfWorkRepositories _unitOfWorkRepositories;
        public UserService(UnitOfWorkRepositories unitOfWorkRepositories)
        {
            _unitOfWorkRepositories = unitOfWorkRepositories;
        }

        /// <summary>
        /// Método encargado de consumir el servicio que obtiene las transacciones y las almacena en base de datos junto con los usuarios y consumidores
        /// </summary>
        public async Task cargarDatosAsync()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5);
                StringContent content = new StringContent("", Encoding.UTF8, "application/json");
                string url = ConfigurationHelper.GetByName("apiPrueba");
                HttpResponseMessage responseMessage = await httpClient.PostAsync(url, content);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<TransaccionDTO> data = JsonConvert.DeserializeObject<IEnumerable<TransaccionDTO>>(await responseMessage.Content.ReadAsStringAsync()).ToList();

                    foreach (var item in data)
                    {
                        try
                        {
                            Comercio comercio = new Comercio
                            {
                                ComercioCodigo = item.comercio_codigo,
                                ComercioDireccion = item.comercio_direccion,
                                ComercioNit = item.comercio_nit,
                                ComercioNombre = item.comercio_nombre
                            };

                            //Si el comercio no existe se agrega
                            if ((await _unitOfWorkRepositories.ComercioRepository.Find(c => c.ComercioCodigo == comercio.ComercioCodigo)).FirstOrDefault() == null)
                            {
                                _unitOfWorkRepositories.ComercioRepository.Add(comercio);
                                try
                                {
                                    await _unitOfWorkRepositories.Save();
                                }
                                catch { }
                            }

                            Usuario usuario = new Usuario
                            {
                                UsuarioIdentificacion = item.usuario_identificacion,
                                UsuarioEmail = item.usuario_email,
                                UsuarioNombre = item.usuario_nombre
                            };

                            //Si el usuario no existe se agrega
                            if ((await _unitOfWorkRepositories.UsuarioRepository.Find(u => u.UsuarioIdentificacion == usuario.UsuarioIdentificacion)).FirstOrDefault() == null)
                            {
                                _unitOfWorkRepositories.UsuarioRepository.Add(usuario);
                                try
                                {
                                    await _unitOfWorkRepositories.Save();
                                }
                                catch { }
                            }


                            Trans trans = new Trans
                            {
                                TransCodigo = item.Trans_codigo,
                                TransConcepto = item.Trans_concepto,
                                TransEstadoId = item.Trans_estado,
                                TransMediopId = item.Trans_medio_pago == 0 ? 32 : item.Trans_medio_pago,
                                TransFecha = item.Trans_fecha,
                                TransTotal = item.Trans_total,
                                ComercioCodigo = item.comercio_codigo,
                                UsuarioIdentificacion = item.usuario_identificacion
                            };

                            //Si las transacciones no existen se agregan
                            if ((await _unitOfWorkRepositories.TransRepository.Find(t => t.TransCodigo.Equals(trans.TransCodigo))).FirstOrDefault() == null)
                            {
                                _unitOfWorkRepositories.TransRepository.Add(trans);
                                try
                                {
                                    await _unitOfWorkRepositories.Save();
                                }
                                catch { }
                            }
                        }catch(Exception ex)
                        {

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método encargado de validar las credenciales de acceso de un usuario o comercio
        /// </summary>
        /// <param name="user">Objeto con las credenciales del usuario</param>
        /// <returns>Retorna una tupla con true si las credenciales son correctas, de lo contrario retorna false con un mensaje de error</returns>
        public async Task<Tuple<bool,string>> validarLoginAsync(UserDTO user)
        {
            try
            {
                if (user.tipo == "PERSONA")
                {
                    var usuario = (await _unitOfWorkRepositories.UsuarioRepository.Find(u => u.UsuarioIdentificacion.Equals(user.identificacion))).FirstOrDefault();
                    if (usuario == null)
                    {
                        return new Tuple<bool, string>(false, "El usuario no existe");
                    }
                    else if (string.IsNullOrEmpty(usuario.UsuarioPassword))
                    {
                        return new Tuple<bool, string>(false, "Debe registrar una contraseña");
                    }
                    else
                    {
                        if (!Crypto.GetSHA256(user.password).Equals(usuario.UsuarioPassword))
                        {
                            return new Tuple<bool, string>(false, "El usuario o contraseña no es valido");
                        }
                        else
                        {
                            return new Tuple<bool, string>(true, "");
                        }
                    }
                }
                else if (user.tipo == "COMERCIO")
                {
                    var comercio = (await _unitOfWorkRepositories.ComercioRepository.Find(u => u.ComercioCodigo.Equals(int.Parse(user.identificacion)))).FirstOrDefault();
                    if (comercio == null)
                    {
                        return new Tuple<bool, string>(false, "El usuario no existe");
                    }
                    else if (string.IsNullOrEmpty(comercio.ComercioPassword))
                    {
                        return new Tuple<bool, string>(false, "Debe registrar una contraseña");
                    }
                    else
                    {
                        if (!Crypto.GetSHA256(user.password).Equals(comercio.ComercioPassword))
                        {
                            return new Tuple<bool, string>(false, "El usuario o contraseña no es valido");
                        }
                        else
                        {
                            return new Tuple<bool, string>(true, "");
                        }
                    }
                }
                else
                {
                    return new Tuple<bool, string>(false, "Tipo de usuario no es valido");
                }
            }catch(Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Actualiza la contraseña de un usuario o comercio que ya este registrado en base de datos
        /// </summary>
        /// <param name="user">Recibe un objeto con los datos del usuario o comercio</param>
        /// <returns>Retorna una tupla con true si el usuario o comercio se registro correctamente, de lo contrario retorna false junto con un mensaje</returns>
        public async Task<Tuple<bool,string>> registrarPasswordAsync(UserRegisterDTO user) 
        {
            try
            {
                if (user.tipo == "PERSONA")
                {
                    var usuario = (await _unitOfWorkRepositories.UsuarioRepository.Find(u => u.UsuarioIdentificacion.Equals(user.identificacion))).FirstOrDefault();
                    if (usuario == null)
                    {
                        return new Tuple<bool, string>(false, "El usuario no existe en la base de datos");
                    }
                    else if (!string.IsNullOrEmpty(usuario.UsuarioPassword))
                    {
                        return new Tuple<bool, string>(false, "El usuario ya esta registrado con una contraseña");
                    }
                    else
                    {
                        usuario.UsuarioPassword = Crypto.GetSHA256(user.password);
                        _unitOfWorkRepositories.UsuarioRepository.Update(usuario);
                        await _unitOfWorkRepositories.Save();

                        return new Tuple<bool, string>(true, "");
                    }
                }
                else if (user.tipo == "COMERCIO")
                {
                    var comercio = (await _unitOfWorkRepositories.ComercioRepository.Find(u => u.ComercioCodigo.Equals(int.Parse(user.identificacion)))).FirstOrDefault();
                    if (comercio == null)
                    {
                        return new Tuple<bool, string>(false, "El usuario no existe en la base de datos");
                    }
                    else if (!string.IsNullOrEmpty(comercio.ComercioPassword))
                    {
                        return new Tuple<bool, string>(false, "El usuario ya esta registrado con una contraseña");
                    }
                    else
                    {
                        comercio.ComercioPassword = Crypto.GetSHA256(user.password);
                        _unitOfWorkRepositories.ComercioRepository.Update(comercio);
                        await _unitOfWorkRepositories.Save();

                        return new Tuple<bool, string>(true, "");
                    }
                }
                else
                {
                    return new Tuple<bool, string>(false, "Tipo de usuario no es valido");
                }
            }
            catch(Exception ex)
            {
                throw ex;   
            }
        }
    }
}
