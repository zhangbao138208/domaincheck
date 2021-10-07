using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FK域名检测工具管理器
{
    public class DomainStruct
    {
        public string Company { get; set; }
        public string Domain { get; set; }
        public bool IsNeedToCheck { get; set; }
        public string CheckPointName { get; set; }
        public string Creater { get; set; }
        public string CreateTime { get; set; }
        public string Remark { get; set; }

        public DomainStruct() {
        }
        public DomainStruct(string company, string Domain, bool IsNeedToCheck,
            string CheckPointName, string Creater, string CreateTime,
            string Remark)
        {
            this.Company = company;
            this.Domain = Domain;
            this.IsNeedToCheck = IsNeedToCheck;
            this.CheckPointName = CheckPointName;
            this.Creater = Creater;
            this.CreateTime = CreateTime;
            this.Remark = Remark;
        }
    }
}
