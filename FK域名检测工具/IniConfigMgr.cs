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
        private readonly string _filePath; // 文件路径

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        static IniConfigMgr() {
            IniInstance = new IniConfigMgr();
        }

        private IniConfigMgr() {
            _filePath = Environment.CurrentDirectory + "\\config.ini";
        }

        public static IniConfigMgr IniInstance { get; }

        public void WriteConfig(string key, string value, bool isUseDes = false) {
            if (!this.ExistIniFile()) {
                CreateIniFile();
            }

            if (isUseDes) {
                string temp = DesHelper.Encode(value);
                WritePrivateProfileString("default", key, temp, this._filePath);
                return;
            }
            WritePrivateProfileString("default", key, value, this._filePath);
        }

        public string LoadConfig(string key, bool isUseDes = false) {
            var temp = new StringBuilder(512);
            GetPrivateProfileString("default", key, "", temp, 512, this._filePath);
            if (!isUseDes) return temp.ToString();
            var temp2 = DesHelper.Decode(temp.ToString());
            return temp2;
        }

        private bool ExistIniFile(){
            return File.Exists(this._filePath);
        }

        private void CreateIniFile() {
            File.Create(this._filePath);
        }
    }
}
