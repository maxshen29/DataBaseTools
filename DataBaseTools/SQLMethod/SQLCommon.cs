using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseTools.Common;
using DataBaseTools.Model;
using System.Data;
using System.Data.SqlClient;


namespace DataBaseTools.SQLMethod
{
    public class SQLCommon
    {
         

        /// <summary>
        /// 测试连接返回bool
        /// </summary>
        /// <param name="sQLConfig"></param>
        /// <returns></returns>
        public static bool TryConnect()
        {
            

            bool result = false;
            SqlConnection conn = null;
            try
            {
 
                SQLConfigClass sQLConfig = CommHelper.GetSQLConfig();
                string ServerName = sQLConfig.ServerName.Trim();
                string DBName = "master";
                string DBUser = sQLConfig.UserName.Trim();
                string DBPWD = CommHelper.DecryptDES(sQLConfig.Password.Trim(), CommHelper._encryptKey);
                string Ports = sQLConfig.Ports.Trim();

               var  constr = string.Format("Data Source={0},{1};Initial Catalog={2};Persist Security Info=True;User ID={3};Password={4};Connect Timeout=2;", ServerName, Ports, DBName, DBUser, DBPWD);


                conn = new SqlConnection(constr);
                conn.Open();
                conn.Close(); 
             
                result = true;
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
             
                result = false;
            }
            finally
            {
                if (conn != null)
                {
                    //关闭数据库连接
                    conn.Close();
                }
          
            }
            return result;
         
        
        }
        /// <summary>
        /// 获取数据库版本信息
        /// </summary>
        /// <returns></returns>
        public static string GetSQLVerInfo()
        {
            string result="";
            string sqlstr = "select @@version";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);

                var dr = sqlCommand.ExecuteReader();
                
                if (dr.Read())
                {
                    result = $" 版本信息: \r\n {dr[0].ToString().Replace("	", "  \r\n")} \r\n";
                }
                
                dr.Close();
                sqlConnection.Close();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = "ex.StackTrace";
            }
            return result;
        }




        /// <summary>
        /// 获取CPU,内存信息
        /// </summary>
        /// <returns></returns>
        public static string GetServerCPUMemoryInfo()
        {
            string result = "";
            string sqlstr = "SELECT cpu_count,hyperthread_ratio,cpu_count/hyperthread_ratio  as [PhysicalCPUCount],physical_memory_kb/1024/1024,virtual_memory_kb/1024/1024,sqlserver_start_time FROM sys.dm_os_sys_info ";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
             
                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = $" CPU数量:   {dr[0]} \r\n 线程数:    {dr[1]} \r\n 物理CPU数:     {dr[2]} \r\n";
                    result += $" 物理内存:   {dr[3]}G\r\n 虚拟内存:    {dr[4]} G  \r\n";
                    result += $" 数据库启动时间:   {dr[5]} \r\n";

                }
                dr.Close();
                sqlConnection.Close();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = "ex.StackTrace";
            }
            return result;
        }


       

      /// <summary>
      /// 获取日志信息
      /// </summary>
      /// <param name="span">时间间隔</param>
      /// <param name="timetype">时间间隔类型</param>
      /// <param name="logtype">日字类型</param>
      /// <returns></returns>
        public static DataSet GetSQLLogs(int span, string timetype,string logtype)
        { 
            DataSet result = new DataSet();
            try
            {
                /// string starDate = DateTime.Now.AddDays(-3).ToString();
                string starDate = "";
                string sqlstr = "";

                if (timetype == "day")
                {
                    starDate = DateTime.Now.AddDays(span).ToString();
                }
                else
                {
                    starDate = DateTime.Now.AddMinutes(span).ToString();
                }
                if (logtype == "sqlserver")
                {
                    sqlstr = $"exec sys.xp_readerrorlog 0,1,null,null,'{starDate}','{DateTime.Now}','desc' ";
                }
                else
                {
                    sqlstr = $"exec sys.xp_readerrorlog 0,2,null,null,'{starDate}','{DateTime.Now}','desc' ";


                }
                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
            //    CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(result);
                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = null;
            }
            return result;

         
        }



        /// <summary>
        /// 获取数据集进程
        /// </summary>
        /// <returns></returns>
 
        public static DataSet GetSysProcesses()
        {
            DataSet result = new DataSet();
            try
            {
               
                string sqlstr = @"SELECT [spid]
                                  ,[kpid]
                                 
                                    ,[login_time]
                                  ,[last_batch]
                                   ,[lastwaittype]
                                    ,[cmd],[dbid] ,[blocked]
                                  ,[waitresource]                                  
                                  ,[uid]
                                  ,[cpu]
                                  ,[physical_io]
                                  ,[memusage]                                
                                  ,[ecid]
                                  ,[open_tran]
                                  ,[status]
                                   ,[hostname]
                                  ,[program_name]
                                  ,[hostprocess]                                 
                                  ,[nt_domain]
                                  ,[nt_username]
                                  ,[net_address]
                                  ,[net_library]
                                  ,[loginame]
                                   ,[stmt_start]
                                  ,[stmt_end]
                                  ,[request_id]
                               FROM [sys].[sysprocesses] order by cpu desc";

 
                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
              //  CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(result);
                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = null;
            }
            return result;


        }

        /// <summary>
        /// 获取性能计数器配置类型
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPerformanceObjectName()
        {
            List<string> result = new List<string>();

            try
            {

                string sqlstr = $"select DISTINCT(OBJECT_NAME) from  sys.dm_os_performance_counters";

                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                //  CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                var dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(dr[0].ToString());
                }
                dr.Close();
                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = null;
            }
            return result;

        }
        /// <summary>
        /// 获取性能计数器配置类型
        /// </summary>
        /// <returns></returns>
        public static List<string[]>  GetPerformanceType()
        {
            List<string[]> result = new List<string[]>();

 
            try
            {

                string sqlstr = $"select DISTINCT(OBJECT_NAME),counter_name from  sys.dm_os_performance_counters";

                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                //  CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                var dr = sqlCommand.ExecuteReader();
                string[] tempstrs = new string[2];
                while (dr.Read())
                {
                    tempstrs[0] = dr[0].ToString();
                    tempstrs[1] = dr[1].ToString();
                    result.Add(tempstrs);
                }
                dr.Close();
                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = null;
            }
            return result;

        }
        /// <summary>
        /// 获取性能计数器
        /// </summary>
        /// <returns></returns>
        public static DataSet Getperformance_counters()
        {
            DataSet result = new DataSet();
            try
            {

                string sqlstr = "select * from  sys.dm_os_performance_counters";


                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                //  CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(result);
                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = null;
            }
            return result;


        }

        /// <summary>
        /// 获取SQL 配置
        /// </summary>
        /// <returns></returns>
        public static DataSet GetSQLConfig()
        {
            DataSet result = new DataSet();
            try
            {

                string sqlstr = " sp_configure";


                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                //  CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(result);
                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = null;
            }
            return result;


        }



        /// <summary>
        /// 获取SQL 高级 配置
        /// </summary>
        /// <returns></returns>
        public static void SetSQLConfig(int parameter)
        {
             try
            {

                string sqlstr = $" sp_configure 'show advanced options',{parameter}";


                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                //  CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                var r = sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandText = "RECONFIGURE";
                r = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
              
            }
 

        }


        /// <summary>
        /// 获取进程信息
        /// </summary>
        /// <param name="spid"></param>
        /// <returns></returns>
        public static string GetProcessesCMD(string spid)
        {
            string result = "";
            try
            {

                string sqlstr = $"dbcc inputbuffer({spid})";


                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                //  CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = $" EventType: {dr[0]}\r\n";
                    result += $" Parameters: {dr[1]}\r\n";
                    result += $" EventInfo: {dr[2]}\r\n";
                }
         
                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = null;
            }
            return result;


        }


        /// <summary>
        /// 获取数据名称信息
        /// </summary>
        /// <param name="dbid"></param>
        /// <returns></returns>
        public static string GetDatabaseName(string dbid)
        {
            string result = "";
            try
            {

                string sqlstr = $"select db_name({dbid})";


                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                //  CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = $" 数据库ID: {dbid}\r\n";
                    result += $" 数据库名称: {dr[0]}\r\n";
                 }

                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = null;
            }
            return result;


        }






        /// <summary>
        /// 获取服务器语言信息
        /// </summary>
        /// <returns></returns>
        public static string GetSqlServerProperties()
        {

         
            string result = "";
            string sqlstr = @"Declare @AuditLevel int,@AuditLevelDesc varchar(50) 
	exec master..xp_instance_regread @rootkey='HKEY_LOCAL_MACHINE', @key='SOFTWARE\Microsoft\MSSQLServer\MSSQLServer', @value_name='AuditLevel', @value=@AuditLevel output
	SET @AuditLevelDesc=CASE @AuditLevel WHEN 0 THEN 'None' WHEN 1 THEN 'Successful Logins Only' WHEN 2 THEN 'Failed Logins Only' WHEN 3 THEN 'Both Failed and Successful Logins' END
	SELECT @@SERVERNAME InstanceName, @@VERSION InstanceVersion,SERVERPROPERTY('ProductVersion') AS [ProductVersion],SERVERPROPERTY('ProductLevel') AS [ProductLevel],SERVERPROPERTY('Edition') AS [Edition],SERVERPROPERTY('BuildClrVersion') AS [ClrVersion],
	SERVERPROPERTY('Collation') AS [Collation],CASE SERVERPROPERTY('IsIntegratedSecurityOnly') WHEN 1 THEN 'Windows' ELSE 'Mixed' END [AuthenticationMode],SERVERPROPERTY('ComputerNamePhysicalNetBIOS') AS [SqlHost],COALESCE(SERVERPROPERTY('InstanceName'), 'MSSQLSERVER') AS [InstanceName],
	SERVERPROPERTY('IsClustered') AS [IsClustered],SERVERPROPERTY('IsFullTextInstalled') AS [IsFullTextInstalled],SERVERPROPERTY('IsSingleUser') AS [IsSingleUser],@AuditLevelDesc LoginAuditing,SERVERPROPERTY('ResourceLastUpdateDateTime') AS [LastUpdateDateTime],SERVERPROPERTY('ComparisonStyle') AS [ComparisonStyle],
	SERVERPROPERTY('LCID') AS [LCID],SERVERPROPERTY('SqlCharSet') AS [SqlCharSet],SERVERPROPERTY('SqlCharSetName') AS [SqlCharSetName], SERVERPROPERTY('SqlSortOrder') AS [SqlSortOrder]
 ";
            try
            {
              CommHelper.WriteLogs(CommHelper.GetSQLConnectStr(), "info");
                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);

                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    { 
                        result+= dr.GetName(i)+": "+ dr[i].ToString()+"\r\n";
                    
                    }
                 }
                dr.Close();
                sqlConnection.Close();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = "ex.StackTrace";
            }
            return result;
        }

        /// <summary>
        /// 获取DataSet 通用SQLStr
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static DataSet GetCommonDataSet(string sqlstr)
        {

            DataSet result = new DataSet();
            try
            {               
                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                //  CommHelper.WriteLogs(sqlstr, "Info");

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;                
                sqlDataAdapter.Fill(result);
                sqlConnection.Close();
                sqlCommand.Dispose();

            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = null;
            }
            return result;

        }


        /// <summary>
        /// 获取服务器语言信息
        /// </summary>
        /// <returns></returns>
        public static string GetServerInfo()
        {
            string result = "";
            string sqlstr = "SELECT @@SERVERNAME AS SERVERNAME,@@SERVICENAME AS SERVICENAME,@@LANGUAGE AS SQLLANGUAGE ,os_language_version FROM sys.dm_os_windows_info";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(CommHelper.GetSQLConnectStr());
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlstr, sqlConnection);

                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = $" 服务器名称:  {dr[0]}\r\n 服务名称:   {dr[1]}\r\n SQLServer语言:    {dr[2]}\r\n 操作系统语言: {GetOSLanguage(dr[3].ToString())}\r\n";
                }
                dr.Close();
                sqlConnection.Close();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                result = "ex.StackTrace";
            }
            return result;
        }

        private static string GetOSLanguage(string ver)
        {
            string language = "";
            switch (ver.Trim())
            {
                case "2052":
                    language = "简体中文";
                    break;

                case "1033":
                    language = "English - United States";
                    break;

                case "2057":
                    language = "English - United Kingdom";
                    break;
                
                case "4100":
                    language = "Chinese - Singapore";
                    break;

                case "1028":
                    language = "Chinese - Taiwan";
                    break;

                case "3076":
                    language = "Chinese - Hong Kong SAR";
                    break;

                case "5124":
                    language = "Chinese - Macao SAR";
                    break;

                default:
                    language = ver;
                    break;
                    
            }

            return language;
        }


    }
}
