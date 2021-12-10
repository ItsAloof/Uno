
using Photon.Realtime;
using Photon.Chat;

namespace Un
{
    public static class AppSettingsExtensions
    {
        public static ChatAppSettings GetChatSettings(this AppSettings appSettings)
        {
            return new ChatAppSettings
            {
                AppIdChat = appSettings.AppIdChat,
                AppVersion = appSettings.AppVersion,
                FixedRegion = appSettings.IsBestRegion ? null : appSettings.FixedRegion,
                NetworkLogging = appSettings.NetworkLogging,
                Protocol = appSettings.Protocol,
                EnableProtocolFallback = appSettings.EnableProtocolFallback,
                Server = appSettings.IsDefaultNameServer ? null : appSettings.Server,
                Port = (ushort)appSettings.Port
            };
        }
    }
}