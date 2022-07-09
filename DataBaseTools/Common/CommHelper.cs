using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using DataBaseTools.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataBaseTools.Common
{
    public static class CommHelper
    {

        /// <summary>
        /// 获取基础 计算机key
        /// </summary>
        /// <returns></returns>
        public static string GetBaseKeyInfo()
        {
            ComputerInfo _computerInfo = new ComputerInfo();
            string basekeyinfo = $"{_computerInfo.DiskID}{_computerInfo.CpuID}{_computerInfo.MacAddress}";
            return basekeyinfo.Replace(" ", "").Replace(":", "");

        }
        /// <summary>
        /// 验证注册码
        /// </summary> 
        /// <returns></returns>
        public static bool ValidationRegKey()
        {
            bool result = false;
            try
            {
                if (EncryptDES(GetBaseKeyInfo(),_encryptKey)==GetQLConfigClassALL().key)
                {

                    result = true;
                }
                else
                {
                    result = false;

                }

                
            
            }
            catch (Exception ex)
            {
            
                result = false;
                CommHelper.WriteLogs(ex.StackTrace, "Err");
            
            }
           return result;

        }
        
        #region 加密解密内容
        /// <summary>
        /// 加密密钥
        /// </summary>
        public static string _encryptKey = "ABA9E2OPS2D94E43A3CA66002BP79FAY";

        /// <summary>
        /// 向量
        /// </summary>
        private static byte[] Keys = { 0x05, 0x06, 0x07, 0x08, 0x09, 0x00, 0x01, 0x02, 0x03, 0x04, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

 
        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为16位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 

        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var DCSP = Aes.Create();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception("数据库密码加密异常" + ex.Message);
            }

        }

        /// <summary> 
        /// DES解密字符串 
        /// </summary> 
        /// <param name="decryptString">待解密的字符串</param> 
        /// <param name="decryptKey">解密密钥,要求为16位,和加密密钥相同</param> 
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns> 

        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 16));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                var DCSP = Aes.Create();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                Byte[] inputByteArrays = new byte[inputByteArray.Length];
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                //  Logger.Error("解密异常：str:" + decryptString + ",Key:" + decryptKey + "---" + ex.Message);
                return null;
            }

        }
        #endregion

        public static void WriteLogs(string content,string loglevel)
        {
           // Console.WriteLine(content);
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                 path = path+ "/Logs/";
               
               // Console.WriteLine(path);
                if (!string.IsNullOrEmpty(path))
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = path +loglevel+"-"+ DateTime.Now.ToString("yyyy-MM-dd")+ ".txt";
                    if (!File.Exists(path))
                    {
                        FileStream fs = File.Create(path);
                        fs.Close();
                    }
                    if (File.Exists(path))
                    {
                        StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "----" + content);
                        sw.Close();
                    }
                }
               // Console.WriteLine(path);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
               
            }
        }

        /// <summary>
        /// 获取SQL连接字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSQLConnectStr()
        {
            string result = "";

            try
            {
                SQLConfigClass sQLConfig = GetSQLConfig();
                string ServerName = sQLConfig.ServerName.Trim();
                string DBName = "master";
                string DBUser = sQLConfig.UserName.Trim();
                string DBPWD = DecryptDES(sQLConfig.Password.Trim(),_encryptKey);
                string Ports= sQLConfig.Ports.Trim();

                result= string.Format("Data Source={0},{1};Initial Catalog={2};Persist Security Info=True;User ID={3};Password={4};Connect Timeout=500;", ServerName, Ports, DBName, DBUser, DBPWD);
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = "配置信息缺失！";
            }

            return result;

        }

        /// <summary>
        /// 获取数据库配置信息
        /// </summary>
        /// <returns></returns>
        public static SQLConfigClass GetSQLConfig()
        {
            SQLConfigClass sQLConfigClass = new SQLConfigClass();
            try
            {

                IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("Config/config.json").Build();
                configuration.GetSection("sqlconfig").Bind(sQLConfigClass);
                // DebugMsgBox(configuration.GetSection("sqlconfig").GetChildren();


            }
            catch (Exception e)
            {
                WriteLogs(e.StackTrace, "Err");
            }
            return sQLConfigClass;
        }

        /// <summary>
        /// 获取整体配置
        /// </summary>
        /// <returns></returns>
        public static SQLConfigClassALL GetQLConfigClassALL()
        {
            SQLConfigClassALL _sQLConfigClassall = new SQLConfigClassALL();
            try
            {


                StreamReader file = File.OpenText("Config/config.json");

                JsonTextReader reader = new JsonTextReader(file);

                JObject jsonObject = (JObject)JToken.ReadFrom(reader);
                _sQLConfigClassall = jsonObject.ToObject<SQLConfigClassALL>();

                reader.Close();
                file.Close();

            }
            catch (Exception e)
            {
                WriteLogs(e.StackTrace, "Err");
            }
            return _sQLConfigClassall;

        }


      /// <summary>
      /// 写入数据库配置
      /// </summary>
        public static void WriteConfigClass(SQLConfigClass sQLConfigClass)
        {
            SQLConfigClassALL _sQLConfigClassall = new SQLConfigClassALL();
            try
            {
                _sQLConfigClassall.sqlconfig = sQLConfigClass;
                _sQLConfigClassall.key=GetQLConfigClassALL().key;

 
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(_sQLConfigClassall, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("Config/config.json", output);

 
            }
            catch (Exception e)
            {
                WriteLogs(e.StackTrace, "Err");
            }
 
        }

        /// <summary>
        /// 写入注册码
        /// </summary>
        /// <param name="sQLConfigClass"></param>
        public static void WriteConfigClassKey(string RegCode)
        {
            SQLConfigClassALL _sQLConfigClassall = new SQLConfigClassALL();
            try
            {
                _sQLConfigClassall.sqlconfig =GetSQLConfig();
                _sQLConfigClassall.key = RegCode;


                string output = Newtonsoft.Json.JsonConvert.SerializeObject(_sQLConfigClassall, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText("Config/config.json", output);


            }
            catch (Exception e)
            {
                WriteLogs(e.StackTrace, "Err");
            }

        }



        public static void DebugMsgBox(string msg)
        {
            MessageBox.Show(msg);


        }

        /// <summary>
        /// 错误提示框，之后优化
        /// </summary>
        /// <param name="msg"></param>
        public static void MsgBoxERR(string msg)
        {
            MessageBox.Show(msg,"错误信息");


        }


        /// <summary>
        /// 普通提示框，之后优化
        /// </summary>
        /// <param name="msg"></param>
        public static void MsgBoxInfo(string msg)
        {
            MessageBox.Show(msg, "提示信息");


        }


        public static void DebugMsg(string message,int TaskIndex)
        {

            IConfiguration configuration  = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config/appsettings.json").Build();
            bool Debug =  Convert.ToBoolean(configuration.GetSection("Debug").Value);
            if (Debug)
            {
                Console.WriteLine(message);
               // WriteLogs(message,TaskIndex,"DebugMsg");
            }

        }
         

         

        /// <summary>
        /// 时间戳转换为时间
        /// </summary>
        /// <param name="longDateTime"></param>
        /// <returns></returns>
        public static DateTime LongDateTimeToDateTimeString(long longDateTime)
        {
            //用来格式化long类型时间的,声明的变量
            long unixDate;
            DateTime start;
            DateTime date;
            //ENd

           // unixDate = long.Parse(longDateTime);
            unixDate =longDateTime;
            start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            date = start.AddMilliseconds(unixDate).ToLocalTime();

            return date;

        }

    }
}


 