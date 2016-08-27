using System.Configuration;

namespace OMInsurance.Configuration
{
    public static class ConfiguraionProvider
    {
        public static readonly string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["OMInsurance"].ConnectionString;
        public static readonly int AuthenticationCookieRefreshMargin = int.Parse(ConfigurationManager.AppSettings["AuthenticationCookieRefreshMargin"]);
        public static readonly int AuthenticationCookieDuration = int.Parse(ConfigurationManager.AppSettings["AuthenticationCookieDuration"]);
        public static readonly string AuthenticationCookieEncryptionKey = ConfigurationManager.AppSettings["AuthenticationCookieEncryptionKey"];
        public static readonly string LogPath = AppConfigurationHelper.GetConfiguration("LogPath");
        public static readonly string FileSizeInMb = ConfigurationManager.AppSettings["FileSizeInMb"];
        public static readonly string FileStorageFolder = ConfigurationManager.AppSettings["FileStorageFolder"];
    }
}
