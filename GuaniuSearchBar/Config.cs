using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuaniuSearchBar
{
    public class Config
    {
        public struct HotKeyConfig
        {
            public int hotkeyIndex;
            public bool enabled;
            public HotKeyConfig(int index, bool enabled)
            {
                this.enabled = enabled;
                this.hotkeyIndex = index;

            }
        }

       private static  HotKeyConfig hotkeyConfig;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static HotKeyConfig ReadHotkeyConfigFromFile()
        {
            var sr = new StreamReader(new FileStream(AppDomain.CurrentDomain.BaseDirectory + "cfg", FileMode.OpenOrCreate));
            var line1 = sr.ReadLine();
            sr.Close();
            hotkeyConfig.hotkeyIndex = 0;
            hotkeyConfig.enabled = false;
            if (line1 != null)
            {
                string[] strs = line1.Split(';');
                if (strs.Length==2)
                {
                    if (strs[1] == "0")
                    {
                        hotkeyConfig.hotkeyIndex = 0;
                    }
                    if (strs[1] == "1")
                    {
                        hotkeyConfig.hotkeyIndex = 1;
                    }
                    bool.TryParse(strs[0], out hotkeyConfig.enabled);
                }
            
            }
            return hotkeyConfig;



        }
        public static void SaveHotkeyConfig(HotKeyConfig cfg)
        {
            var sw = new StreamWriter(new FileStream(AppDomain.CurrentDomain.BaseDirectory + "cfg", FileMode.Truncate));
            sw.WriteLine(cfg.enabled.ToString()+";"+cfg.hotkeyIndex.ToString());
            sw.Close();
        }
    }
}
