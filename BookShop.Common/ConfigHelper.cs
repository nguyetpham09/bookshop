using System.Configuration;

namespace BookShop.Common
{
    public class ConfigHelper
    {
        public static string GetByKey(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }

        public static void SetByKey(string key, string value)
        {
            ConfigurationManager.AppSettings[key] = value;
        }
    }
}
