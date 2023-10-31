using System.Threading.Tasks;
using Xamarin.Essentials;
using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    internal class PropertiesDict
    {
        internal static string GetUserAgent()
        {
            // EXAMPLE AGENT: Phone/Apple/iOS/13.3 (Sean's iPhone) 0584707f-0f7a-4a9e-8106-f7b01c6354cd/1.1.0
            string guid = "";

            if (App.Current.Properties.ContainsKey("guid"))
            {
                guid = App.Current.Properties["guid"] as string;
            }
            else
            {
                guid = System.Guid.NewGuid().ToString();
                App.Current.Properties.Add("guid", guid);
                App.Current.SavePropertiesAsync();
            }

            string agent = DeviceInfo.Idiom + "/" + DeviceInfo.Manufacturer + "/" + DeviceInfo.Platform + "/" + DeviceInfo.VersionString + " (" + DeviceInfo.Name + ")";
            agent += " " + guid + "/" + BaseViewModel.APP_VERSION;
            return agent;
        }

        internal static string GetSteamID()
        {
            string id = "";

            if (App.Current.Properties.ContainsKey("steamid"))
            {
                id = App.Current.Properties["steamid"] as string;
            }
            // don't set a default

            return id;
        }

        internal static GameEnum GetGame()
        {
            GameEnum game = GameEnum.CSS;

            if (App.Current.Properties.ContainsKey("game"))
            {
                string gameString = App.Current.Properties["game"] as string;
                switch (gameString)
                {
                    case "css": game = GameEnum.CSS; break;
                    case "css100t": game = GameEnum.CSS100T; break;
                    case "csgo": game = GameEnum.CSGO; break;
                    default: goto case "css";
                }
            }
            else
            {
                App.Current.Properties.Add("game", EnumToString.APIString(game));
                App.Current.SavePropertiesAsync();
            }

            return game;
        }

        internal static ModeEnum GetMode()
        {
            ModeEnum mode = ModeEnum.FW;

            if (App.Current.Properties.ContainsKey("mode"))
            {
                string modeString = App.Current.Properties["mode"] as string;
                switch (modeString)
                {
                    case "fw": mode = ModeEnum.FW; break;
                    case "hsw": mode = ModeEnum.HSW; break;
                    case "sw": mode = ModeEnum.SW; break;
                    case "bw": mode = ModeEnum.BW; break;
                    default: goto case "fw";
                }
            }
            else
            {
                App.Current.Properties.Add("mode", EnumToString.APIString(mode));
                App.Current.SavePropertiesAsync();
            }

            return mode;
        }

        internal static async Task SetAll(string steamID, GameEnum game, ModeEnum mode)
        {
            App.Current.Properties["steamid"] = steamID;
            App.Current.Properties["game"] = EnumToString.APIString(game);
            App.Current.Properties["mode"] = EnumToString.APIString(mode);
            await App.Current.SavePropertiesAsync();
        }
    }
}
