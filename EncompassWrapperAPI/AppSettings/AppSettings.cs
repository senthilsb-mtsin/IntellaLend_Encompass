using Microsoft.Extensions.Configuration;
using System;

namespace AppSettings
{
    public static class AppSettingJson
    {
        private static IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

        #region Public Properties

        public static string GetDBConnectionString
        {
            get
            {
                return configuration.GetConnectionString("WrapperDBConnection");
            }
        }

        public static string EncompassAPIURL
        {
            get
            {
                return configuration["AppConfigurations:EncompassAPIURL"].ToString();
            }
        }

        #endregion

        #region Private Properties


        #endregion

    }
}
