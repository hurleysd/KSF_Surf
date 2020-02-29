using System;
using System.Collections.Generic;

namespace KSF_Surf.Models
{
    // Classes for KSF API response JSON deserialization-----------------------------------
    public class KSFDatum
    {
        public string hostname { get; set; }
        public string surftimer_servername { get; set; }
        public string ipport { get; set; }
        public string timesinceupdate { get; set; }
        public string timestamp { get; set; }
        public string currentmap { get; set; }
        public string timeleft { get; set; }
        public string mp_timelimit { get; set; }
        public string playersonline { get; set; }
        public string playersalive { get; set; }
        public string playersspectate { get; set; }
        public List<object> players { get; set; }
    }

    public class KSFRootObject
    {
        public string status { get; set; }
        public List<KSFDatum> data { get; set; }
    }
}