using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.Utils
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Método para acceder al archivo de configuración y obtener el valor de un parámetro
        /// </summary>
        /// <param name="key">Llave del valor que se quiere obtener</param>
        /// <returns></returns>
        public static string GetByName(string key)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IConfigurationSection section = config.GetSection(key);

            return section.Value;
        }
    }
}
