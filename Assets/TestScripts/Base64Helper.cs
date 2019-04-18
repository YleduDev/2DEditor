using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Drawing;

    /// <summary>
    /// Base64编码字符串与文件转换帮助类
    /// </summary>
    public class Base64Helper
    {
        /// <summary>
        /// Base64编码字符串转文件
        /// </summary>
        /// <param name="base64">Base64编码字符串</param>
        /// <param name="filename">文件完整路径</param>
        //public static void ConvertBase64ToFile(string base64, string filename)
        //{
        //    #region base64
        //    //base64 = "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"; 
        //    #endregion
        //    string dummyData = base64.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
        //    if (dummyData.Length % 4 > 0)
        //    {
        //        dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
        //    }
        //    byte[] t = Convert.FromBase64String(dummyData);//byte类型数据
        //    MemoryStream stream = new MemoryStream(Convert.FromBase64String(dummyData));
        //    //filename = @"C:\Users\admin\Desktop\Tongji\test.jpg";
        //    Bitmap img = new Bitmap(stream);
        //    img.Save(filename);
        //}

        /// <summary>
        /// 文件转Base64编码字符串
        /// </summary>
        /// <param name="fileName">文件完整路径</param>
        /// <returns></returns>
        public static string ConvertFileToBase64(string fileName)
        {
            //fileName = @"C:\Users\admin\Desktop\Tongji\test.jpg";
            FileStream fs = File.OpenRead(fileName);
            BinaryReader br = new BinaryReader(fs);
            byte[] bt = br.ReadBytes(Convert.ToInt32(fs.Length));
            string base64String = Convert.ToBase64String(bt);
            return base64String;
        }


    public static byte[] ConvertFileToBase64Bytes(string fileName)
    {
        //fileName = @"C:\Users\admin\Desktop\Tongji\test.jpg";
        FileStream fs = File.OpenRead(fileName);
        BinaryReader br = new BinaryReader(fs);
        byte[] bt = br.ReadBytes(Convert.ToInt32(fs.Length));
        fs.Close();
        //string base64String = Convert.ToBase64String(bt);
        return bt;
    }


    /// <summary>
    /// Base64编码字符串转(过滤特殊字符即可)
    /// </summary>
    /// <param name="base64"></param>
    /// <returns></returns>
    public static byte[] ConvertBase64(string base64)
        {
            try
            {
                string dummyData = base64.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
                if (dummyData.Length % 4 > 0)
                {
                    dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
                }
                return Convert.FromBase64String(dummyData);
            }

            catch (Exception ex)
            {
                // Error creating stream or reading from it.

                //ex.Message

                return null;

            }

        }
    }
