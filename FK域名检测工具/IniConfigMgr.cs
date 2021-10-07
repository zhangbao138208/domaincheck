using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FK域名检测工具
{
    public class IniConfigMgr
    {
        private string filePath; // 文件路径
        private static IniConfigMgr m_IniConfigMgr; // 静态单例对象

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        static IniConfigMgr() {
            m_IniConfigMgr = new IniConfigMgr();
        }

        private IniConfigMgr() {
            filePath = Environment.CurrentDirectory + "\\config.ini";
        }

        public static IniConfigMgr IniInstance {
            get { return m_IniConfigMgr; }
        }

        public void WriteConfig(string key, string value, bool isUseDes = false) {
            if (!this.ExistINIFile()) {
                createINIFile();
            }

            if (isUseDes) {
                string temp = DesHelper.Encode(value);
                WritePrivateProfileString("default", key, temp, this.filePath);
                return;
            }
            WritePrivateProfileString("default", key, value, this.filePath);
        }

        public string LoadConfig(string key, bool isUseDes = false) {
            StringBuilder temp = new StringBuilder(512);
            int i = GetPrivateProfileString("default", key, "", temp, 512, this.filePath);
            if (isUseDes) {
                string temp2 = DesHelper.Decode(temp.ToString());
                return temp2;
            }
            return temp.ToString();
        }

        public bool ExistINIFile(){
            return File.Exists(this.filePath);
        }

        private void createINIFile() {
            File.Create(this.filePath);
        }
    }
}
