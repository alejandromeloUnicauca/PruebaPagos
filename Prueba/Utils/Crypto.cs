using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Prueba.Utils
{

    public static class Crypto
    {
        /// <summary>
        /// Método encargado de obtener el hash de una cadena de caracteres
        /// </summary>
        /// <param name="str">string para convertir en hash</param>
        /// <returns>Retorna un string con el hash del parametro recibido</returns>
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
