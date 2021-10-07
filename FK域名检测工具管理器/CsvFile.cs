using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FK域名检测工具管理器
{

    /// <summary>
    /// 标记属性的别名Title
    /// </summary>
    public class AttrForCsvColumnLabel : Attribute
    {
        public string Title { get; set; }
    }

    public static class CsvFileUtility
    {

        /// <summary>
        /// Save the List data to CSV file
        /// </summary>
        /// <param name="dataList">data source</param>
        /// <param name="filePath">file path</param>
        /// <returns>success flag</returns>
        public static bool SaveDataToCSVFile<T>(List<T> dataList, string path, string filePath) where T : class
        {


            bool successFlag = true;

            StringBuilder sb_Text = new StringBuilder();
            StringBuilder strColumn = new StringBuilder();
            StringBuilder strValue = new StringBuilder();
            StreamWriter sw = null;
            var tp = typeof(T);
            PropertyInfo[] props = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            try
            {
                //sw = new StreamWriter(filePath);
                for (int i = 0; i < props.Length; i++)
                {
                    var itemPropery = props[i];
                    AttrForCsvColumnLabel labelAttr = itemPropery.GetCustomAttributes(typeof(AttrForCsvColumnLabel), true).FirstOrDefault() as AttrForCsvColumnLabel;
                    if (null != labelAttr)
                    {
                        strColumn.Append(labelAttr.Title);
                    }
                    else
                    {
                        if (String.Compare(props[i].Name, "LogIndex") == 0)
                        {
                            strColumn.Append("日志编号");
                        }
                        else if (String.Compare(props[i].Name, "Domain") == 0)
                        {
                            strColumn.Append("监测域名");
                        }
                        else if (String.Compare(props[i].Name, "CheckIP") == 0)
                        {
                            strColumn.Append("监测IP");
                        }
                        else if (String.Compare(props[i].Name, "Creator") == 0)
                        {
                            strColumn.Append("创建人");
                        }
                        else if (String.Compare(props[i].Name, "CheckDate") == 0)
                        {
                            strColumn.Append("最后监测日期");
                        }
                        else if (String.Compare(props[i].Name, "Result") == 0)
                        {
                            strColumn.Append("监测结果");
                        }
                        else if (String.Compare(props[i].Name, "PrintScreen") == 0)
                        {
                            strColumn.Append("截图");
                        }
                        else
                        {
                            strColumn.Append(props[i].Name);
                        }
                    }

                    strColumn.Append(",");
                }
                strColumn.Remove(strColumn.Length - 1, 1);
                //sw.WriteLine(strColumn);   
                //write the column name
                sb_Text.AppendLine(strColumn.ToString());

                for (int i = 0; i < dataList.Count; i++)
                {
                    var model = dataList[i];
                    //strValue.Remove(0, strValue.Length);
                    //clear the temp row value
                    strValue.Clear();
                    for (int m = 0; m < props.Length; m++)
                    {
                        var itemPropery = props[m];
                        var val = itemPropery.GetValue(model, null);
                        if (m == 0)
                        {
                            strValue.Append(val);
                        }
                        else
                        {
                            strValue.Append(",");
                            if (val is byte[]) // 截图
                            {
                                string picName = props[0].GetValue(model, null) + ".jpg";
                                bool b = PictureFileUtility.SavePicture(CommonFunc.ObjectToByteArray(val), path + "/" + picName);
                                if(b)
                                    strValue.Append(picName);
                            }
                            else
                            {
                                strValue.Append(val);
                            }
                        }
                    }


                    //sw.WriteLine(strValue);
                    //write the row value
                    sb_Text.AppendLine(strValue.ToString());
                }
            }
            catch (Exception)
            {
                successFlag = false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                }
            }

            File.WriteAllText(filePath, sb_Text.ToString(), Encoding.Default);

            return successFlag;
        }
    }
}