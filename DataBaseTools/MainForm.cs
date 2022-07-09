using DataBaseTools.Model;
using DataBaseTools.Common;
using DataBaseTools.SQLMethod;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DataBaseTools
{
    public partial class MainForm : Form
    {
        //#region 界面使用


        ///// <summary>
        ///// 界面使用
        ///// </summary>
        //[StructLayout(LayoutKind.Sequential)]
        //public struct MARGINS
        //{
        //    public int Left;
        //    public int Right;
        //    public int Top;
        //    public int Bottom;
        //}

        //[DllImport("dwmapi.dll", PreserveSig = false)]
        //static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        //[DllImport("dwmapi.dll", PreserveSig = false)]
        //static extern bool DwmIsCompositionEnabled(); //Dll 导入 DwmApi

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);
        //    if (DwmIsCompositionEnabled())
        //    {
        //        e.Graphics.Clear(Color.Silver); //将窗体用黑色填充（Dwm 会把黑色视为透明区域）
        //    }
        //}

        //#endregion
        private bool _haveGetSQLlog=false;
        private bool _haveGetPerformanceCounters = false;
        private bool _haveGetProcesses = false;
        private bool _haveGetConfig = false;

       
        //private bool _haveClusterInfo=false;
        //private bool _haveJob=false;
        //private bool _haveDataBaseInfo=false;
        //private bool _haveDataBaseLogs = false;
        //private bool _haveIndex = false;
        //private bool _havePerCheck=false;

 
        public static bool _haveFreeR = false;
        public static bool _haveReg = true;
        public static bool _haveConnect = false;


        private DataSet _PerformanceCountersDataSet;

        //创建代理。
        private delegate bool IncreaseHandle(int nValue, string vinfo);//代理创建
        private IncreaseHandle _Increase = null;//声明代理，用于后面的实例化代理
        private int _Max = 100;//用于实例化进度条，可以根据自己的需要，自己改变

        public MainForm()
        {

           /* if (!CommHelper.ValidationRegKey())
            {
                CommHelper.MsgBoxInfo("未有注册码或者注册码错误，请申请注册码！");
                RegForm regForm = new RegForm();
                regForm.ShowDialog();

            }
            else
            {
                _haveReg = true;
            }*/

            InitializeComponent();

            this.toolStripStatusLabel1.Text = "数据库工具！";

            textBox_AboutUS.AppendText(IntroInfo.Connectinfo + "\r\n\r\n");
            textBox_AboutUS.AppendText("调优服务：\r\n");
            textBox_AboutUS.AppendText(IntroInfo.PerformInfo + "\r\n\r\n");

            textBox_AboutUS.AppendText("健康检查：\r\n");
            textBox_AboutUS.AppendText(IntroInfo.HealthCheck + "\r\n\r\n");

            textBox_AboutUS.AppendText("运维服务：\r\n");
            textBox_AboutUS.AppendText(IntroInfo.OMInfo + "\r\n\r\n");

            textBox_AboutUS.AppendText("升级服务：\r\n");
            textBox_AboutUS.AppendText(IntroInfo.DBMigration + "\r\n\r\n");

            if (_haveReg)
            {
                SQLConfigClass sQLConfig = CommHelper.GetSQLConfig();
               // CommHelper.WriteLogs(sQLConfig.UserName, "Err");
                textBox_ServerName.Text = sQLConfig.ServerName;
                textBox_UserName.Text = sQLConfig.UserName;
                textBox_Ports.Text = sQLConfig.Ports;
                textBox_PassWord.Text = CommHelper.DecryptDES(sQLConfig.Password, CommHelper._encryptKey);
                //CommHelper.WriteLogs("start", "info");

                if (sQLConfig.ServerName != "" && sQLConfig.UserName != "" && sQLConfig.Ports != "" && sQLConfig.Password != "")
                {
                    info info = new info();
                    info.ShowDialog();
                    //  CommHelper.MsgBoxInfo("尝试连接数据库！");

                    if (_haveConnect)                 
                    {    

                        GetALLSQLInfo();
                     

                    }
                   
                }
                else
                {
                    _haveConnect=false;
                }
            }

            if (_haveFreeR)
            {
                ShowPayForm();
                timer_showPayForm.Enabled = true;
                timer_showPayForm.Start();
            }
            else
            { 
                timer_showPayForm.Enabled=false;
                timer_showPayForm.Stop();
            }
       

        }




      
         private void button1_Click(object sender, EventArgs e)
        {


            SqlConnection conn = null;
            try
            {          

                string ServerName = textBox_ServerName.Text.Trim();
                string DBName = "master";
                string DBUser = textBox_UserName.Text.Trim();
                string DBPWD = textBox_PassWord.Text;
                string Ports = textBox_Ports.Text.Trim();

                var constr = string.Format("Data Source={0},{1};Initial Catalog={2};Persist Security Info=True;User ID={3};Password={4};Connect Timeout=2;", ServerName, Ports, DBName, DBUser, DBPWD);
                conn = new SqlConnection(constr);
                conn.Open();
                conn.Close();
                CommHelper.MsgBoxInfo("连接成功");
              
                SQLConfigClass sQLConfig = CommHelper.GetSQLConfig();
                sQLConfig.ServerName = textBox_ServerName.Text;
                sQLConfig.UserName = textBox_UserName.Text;
                sQLConfig.Ports = textBox_Ports.Text;
                sQLConfig.Password = CommHelper.EncryptDES(textBox_PassWord.Text, CommHelper._encryptKey);
                CommHelper.WriteConfigClass(sQLConfig);
                GetALLSQLInfo();
               _haveConnect = true;




            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                CommHelper.MsgBoxERR("连接失败");
            }
            finally
            {
                if (conn != null)
                {
                    //关闭数据库连接
                    conn.Close();
                }

            }


             
        }

       

         
       

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_haveConnect)
            {
                CommHelper.MsgBoxInfo("无数据库连接，或者连接有错，请配置数据库连接！");
                return;
            }

            this.toolStripStatusLabel1.Text = "数据库工具！";
            string _TabName = tabControl1.SelectedTab.Name.Trim();
            switch (_TabName)
            {
                
                case "tabPage_Info"://基础信息
                    
                    break;
                case "tabPage_Log":  ///日志查询
                    if (!_haveGetSQLlog)
                    {
                        try
                        {
                            this.toolStripStatusLabel1.Text = "SQL日志信息！";

                            dataGridView_sqllog.DataSource = SQLCommon.GetSQLLogs(-30, "m", "sqlserver").Tables[0].DefaultView;
                            dataGridView_sqllog.Columns[0].Width = 150;
                            dataGridView_sqllog.Columns[1].Width = 100;
                            dataGridView_sqllog.Columns[2].Width = 1000;
                            _haveGetSQLlog = true;
                        }
                        catch (Exception ex)
                        {
                            CommHelper.WriteLogs(ex.StackTrace, "Err");

                            CommHelper.MsgBoxInfo("暂无SQL Server日志。");


                        }
                    }
                    break;
                case "tabPage_Perf": //性能计数器数据
                    if (!_haveGetPerformanceCounters)
                    {
                        try
                        {

                            this.toolStripStatusLabel1.Text = "性能计数器信息！";
                            _PerformanceCountersDataSet = SQLCommon.Getperformance_counters();

                            InitComboBoxObjectName();

                            // counters.Where(x=>x.)

                            //dataGridView_performance_counters.DataSource = SQLCommon.Getperformance_counters().Tables[0].DefaultView;
                            //dataGridView_performance_counters.Columns[0].Width = 300;
                            //dataGridView_performance_counters.Columns[1].Width = 300;
                            //dataGridView_performance_counters.Columns[2].Width = 200;
                            _haveGetPerformanceCounters = true;
                        }
                        catch (Exception ex)
                        {
                            CommHelper.WriteLogs(ex.StackTrace, "Err");
                            CommHelper.MsgBoxInfo("程序错误。");


                        }
                    }
                    break;
                case "tabPage_Processes": //数据库进程
                    if (!_haveGetProcesses)
                    {
                        try
                        {
                            this.toolStripStatusLabel1.Text = "进程信息，点击列表可以查看进程详细信息！！";

                            dataGridView_Processes.DataSource = SQLCommon.GetSysProcesses().Tables[0].DefaultView;
                            _haveGetProcesses = true;
                        }
                        catch (Exception ex)
                        {
                            CommHelper.WriteLogs(ex.StackTrace, "Err");
                            CommHelper.MsgBoxInfo("程序错误。");


                        }

                    }

                    break;
                case "tabPage_ConfigInfo"://配置设置信息
                    if (!_haveGetConfig)
                    {
                        try
                        {
                            this.toolStripStatusLabel1.Text = "获取系统配置信息，若需要看高级信息，需要show advanced options设置为1";

                            dataGridView_Config.DataSource = SQLCommon.GetSQLConfig().Tables[0].DefaultView;
                            dataGridView_Config.Columns[0].Width = 300;
                            _haveGetConfig = true;
                        }
                        catch (Exception ex)
                        {
                            CommHelper.WriteLogs(ex.StackTrace, "Err");
                            CommHelper.MsgBoxInfo("程序错误。");


                        }
                    }
                    break;
                case "tabPage_ClusterInfo": //群集信息
                    this.toolStripStatusLabel1.Text = "群集信息,可以选择详细信息查看！！";

                    break;
                case "tabPage_Job": //job信息
                    this.toolStripStatusLabel1.Text = "Job信息！！";
                    break;
                case "tabPage_DataBaseInfo": //数据库信息
                    this.toolStripStatusLabel1.Text = "数据库信息！！";

                    break;              
 
                case "tabPage_Index":   ///索引
                    this.toolStripStatusLabel1.Text = "索引信息！！";

                    break;
                case "tabPage_Per_Check":  //性能检查
                    this.toolStripStatusLabel1.Text = "性能检查！！";

                    break;
                case "tabPage_Security":  //安全检查
                    this.toolStripStatusLabel1.Text = "安全检查！！";
                  


                    break;
                case "tabPage_Other":  //其他
                    this.toolStripStatusLabel1.Text = "其他！！";

                    break;
                default:
                    break;

            }             

             
        }

 

        /// <summary>
        /// 获取基础信息
        /// </summary>
        private void GetALLSQLInfo()
        {

            //CommHelper.WriteLogs(SQLCommon.GetSqlServerProperties(), "info");

            textBox_SQLInfo.Text = "";            
            textBox_SQLInfo.AppendText(SQLCommon.GetSqlServerProperties());

            //textBox_SQLInfo.AppendText(SQLCommon.GetServerInfo());
            //textBox_SQLInfo.AppendText(SQLCommon.GetServerCPUMemoryInfo());
            //textBox_SQLInfo.AppendText(SQLCommon.GetSQLVerInfo());
           


        }

 
  

        private void radioButton_Click(object sender, EventArgs e)
        {
            string logtype = "sqlserver";
            int timespan = -30;
            string spantype = "day";

            if (radioButton_Serlogs.Checked)
            {
                logtype = "sqlserver";
            }
            if (radioButton_agentlog.Checked)
            {
                logtype = "agentlog";
            }

            if (radioButton_halfhour.Checked)
            {
                timespan = -30;
                spantype = "m";
            }
            if (radioButton_hour.Checked)
            {
                timespan = -60;
                spantype = "m";

            }
            if (radioButton_1day.Checked)
            {
                timespan = -1;
                spantype = "day";
            }
            if (radioButton_3days.Checked)
            {
                timespan = -3;
                spantype = "day";
            }
            if (radioButton_7days.Checked)
            {
                timespan = -7;
                spantype = "day";

            }


            try
            {
                dataGridView_sqllog.DataSource = SQLCommon.GetSQLLogs(timespan,spantype,logtype).Tables[0].DefaultView;
                dataGridView_sqllog.Columns[0].Width = 150;
                dataGridView_sqllog.Columns[1].Width = 100;
                dataGridView_sqllog.Columns[2].Width = 1000;
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");
                if (logtype == "sqlserver")
                {
                    CommHelper.MsgBoxInfo("暂无SQL Server日志。");
                }
                else
                {
                    CommHelper.MsgBoxInfo("暂无Agent日志。");
                }
                
            }



        }

        private void dataGridView_Processes_MouseClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string dbid = this.dataGridView_Processes.Rows[e.RowIndex].Cells["dbid"].Value.ToString();
                ProcessesInfo processesInfo = new ProcessesInfo(this.dataGridView_Processes.Rows[e.RowIndex].Cells[0].Value.ToString(), dbid);
                processesInfo.ShowDialog();
            }
        }

        private void comboBox_OBJECT_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
           // var listobject = SQLCommon.GetPerformanceType();
            comboBox_counter_name.Items.Clear();
            comboBox_instance_name.Items.Clear();

            DataTable dt = new DataTable();
          //  dt.ImportRow(_PerformanceCountersDataSet.Tables[0].Select($"OBJECT_NAME='{comboBox_OBJECT_NAME.SelectedItem}'"));

            DataRow[] dataRows = _PerformanceCountersDataSet.Tables[0].Select($"OBJECT_NAME='{comboBox_OBJECT_NAME.SelectedItem}'");
            
            List<string> list = new List<string>();
            foreach (DataRow row in dataRows)
            { 
                list.Add(row[1].ToString());
            }

            list=list.Distinct().ToList();


            List<string> list1 = new List<string>();
            foreach (DataRow row in dataRows)
            {
                list1.Add(row[2].ToString());
            }

            list1 = list1.Distinct().ToList();


            foreach (var item in list)
            {
                
                comboBox_counter_name.Items.Add(item);
                 
            }


            foreach (var item in list1)
            {

                comboBox_instance_name.Items.Add(item);

            }


         }


        /// <summary>
        /// 
        /// </summary>
        private void InitComboBoxObjectName()
        {

            DataTable _dt = _PerformanceCountersDataSet.Tables[0].DefaultView.ToTable(true, new string[] { "OBJECT_NAME" });

            comboBox_OBJECT_NAME.Items.Clear();
            foreach (DataRow c in _dt.Rows)
            {
                comboBox_OBJECT_NAME.Items.Add(c["OBJECT_NAME"]);
            }
            comboBox_counter_name.Items.Clear();
            comboBox_instance_name.Items.Clear();
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            DataRow[] dataRows;
            if (comboBox_OBJECT_NAME.SelectedItem!=null&& comboBox_OBJECT_NAME.SelectedItem.ToString().Trim() != "" )
            {
                if ( comboBox_counter_name.SelectedItem != null  && comboBox_counter_name.SelectedItem.ToString().Trim() != "")
                {
                    //dataRows = _PerformanceCountersDataSet.Tables[0].Select($"OBJECT_NAME='{comboBox_OBJECT_NAME.SelectedItem}' and counter_name='{comboBox_counter_name.SelectedItem}'  ");

                    if (comboBox_instance_name.SelectedItem != null && comboBox_instance_name.SelectedItem.ToString().Trim() != "")
                    {

                        dataRows = _PerformanceCountersDataSet.Tables[0].Select($"OBJECT_NAME='{comboBox_OBJECT_NAME.SelectedItem}' and counter_name='{comboBox_counter_name.SelectedItem}'  and instance_name='{comboBox_instance_name.SelectedItem}' ");


                    }
                    else
                    {
                        dataRows = _PerformanceCountersDataSet.Tables[0].Select($"OBJECT_NAME='{comboBox_OBJECT_NAME.SelectedItem}' and counter_name='{comboBox_counter_name.SelectedItem}' ");


                    }
                }
                else
                {
                    dataRows = _PerformanceCountersDataSet.Tables[0].Select($"OBJECT_NAME='{comboBox_OBJECT_NAME.SelectedItem}' ");


                }

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("object_name", typeof(string));
                dataTable.Columns.Add("counter_name", typeof(string));
                dataTable.Columns.Add("instance_name", typeof(string));
                dataTable.Columns.Add("cntr_value", typeof(string));
                dataTable.Columns.Add("cntr_type", typeof(string));

               

                 foreach (DataRow row in dataRows)
                {
                    dataTable.ImportRow(row);
                }
 
                dataGridView_performance_counters.DataSource =   dataTable.DefaultView;
                dataGridView_performance_counters.Columns[0].Width = 300;
                dataGridView_performance_counters.Columns[1].Width = 300;
                dataGridView_performance_counters.Columns[2].Width = 200;

            }
            else
            {
                CommHelper.MsgBoxInfo("请选择OBJECT_NAME");
            }

        }

        private void button_advance_Click(object sender, EventArgs e)
        {
            SQLCommon.SetSQLConfig(1);
            dataGridView_Config.DataSource = SQLCommon.GetSQLConfig().Tables[0].DefaultView;
            dataGridView_Config.Columns[0].Width = 300;
         }

        private void button_comon_Click(object sender, EventArgs e)
        {
            SQLCommon.SetSQLConfig(0);
            dataGridView_Config.DataSource = SQLCommon.GetSQLConfig().Tables[0].DefaultView;
            dataGridView_Config.Columns[0].Width = 300;
         }

        /// <summary>
        /// 群集报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                switch (comboBox_clusterinfo.SelectedItem.ToString().Trim())
                {

                    case "数据库镜像端点会话（AlwaysON主副本会话）":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Mirroring_Endpoints).Tables[0].DefaultView;
                        break;
                    case "故障转移群集信息":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_Cluster_Info).Tables[0].DefaultView;
                        break;
                    case "故障转移群集诊断日志配置":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrserver_diagnostics_log).Tables[0].DefaultView;
                        break;
                    case "AlwasyON数据库状态配置":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_DB_State_Config).Tables[0].DefaultView;
                        break;
                    case "AlwasyON状态识别":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_State_Identification).Tables[0].DefaultView;
                        break;
                    case "AlwasyON状态配置":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_State_Config).Tables[0].DefaultView;
                        break;
                    case "群集仲裁网络":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrWC_Quorum_Network).Tables[0].DefaultView;
                        break;
                    case "数据库镜像权限":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Mirroring_Permission).Tables[0].DefaultView;
                        break;
                    case "AlwasyON数据库故障转移信息":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDatabaseHADR).Tables[0].DefaultView;
                        break;

                    case "AG监听IP":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_Listener_IP).Tables[0].DefaultView;
                        break;

                    case "路由信息":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrRouting_list_Info).Tables[0].DefaultView;
                        break;

                    case "数据库镜像信息":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Mirroring_Info).Tables[0].DefaultView;
                        break;

                    case "AG群集状态":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_Cluster_Status).Tables[0].DefaultView;
                        break;


                    case "AG群集健康状态":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_Health_Status).Tables[0].DefaultView;
                        break;


                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }
        }

        /// <summary>
        /// 其他报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_other_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_other.SelectedItem.ToString().Trim())
                {

                    case "复制错误":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrReplicationErrors).Tables[0].DefaultView;
                        break;
                    case "服务状态":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSQLSvc_Status).Tables[0].DefaultView;
                        break;
                    case "数据库邮件":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDatabaseMail).Tables[0].DefaultView;
                        break;

                    case "资源池信息":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrResourceGroupPools).Tables[0].DefaultView;
                        break;
                    case "端点信息":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrEndpoints).Tables[0].DefaultView;
                        break;
                    case "工作负荷组":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrResourceGovernorGroups).Tables[0].DefaultView;
                        break;

                    case "服务器触发器":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrServerTriggers).Tables[0].DefaultView;
                        break;

                    case "加载到服务器的模块":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLoadedModules).Tables[0].DefaultView;
                        dataGridView_other.Columns[2].Width = 300;
                        break;


                    case "弃用的功能":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDeprecated_Features).Tables[0].DefaultView;
                        break;

                    case "备份设备":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrBackupDevices).Tables[0].DefaultView;
                        break;

                    case "链接服务器":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLinkedServers).Tables[0].DefaultView;
                        break;
                 
                 
                    case "SQLTraces配置":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSqlTraces).Tables[0].DefaultView;
                        break;

                    case "代理操作员":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrOperators).Tables[0].DefaultView;
                        break;


                    case "查询优化器SQL Server的详细统计信息":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrOptimizer_information).Tables[0].DefaultView;
                        break;

                    case "MaterDB内对象":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrObject_In_Master_DB).Tables[0].DefaultView;
                        break;

                    case "启动自动执行存储过程":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrStartup_Procedures).Tables[0].DefaultView;
                        break;

                    case "内联函数":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrinline_function).Tables[0].DefaultView;
                        break;

                    case "磁盘信息":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDisk_LUN_Info).Tables[0].DefaultView;
                        break;



                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }
        }

        private void radioButton_AgentJobs_Click(object sender, EventArgs e)
        {
            dataGridView_Log.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAgentJobs).Tables[0].DefaultView;

        }

        private void radioButtonAgentJobSteps_Click(object sender, EventArgs e)
        {
            dataGridView_Log.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAgentJobSteps).Tables[0].DefaultView;

        }

        private void radioButtonAgentJob_Run_History_Click(object sender, EventArgs e)
        {
            dataGridView_Log.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAgentJob_Run_History).Tables[0].DefaultView;

        }

        /// <summary>
        /// index comboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_Index_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                switch (comboBox_Index.SelectedItem.ToString().Trim())
                {

                    case "索引状态统计":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrStatistics_Information).Tables[0].DefaultView;
                        break;
                    case "索引状态统计1":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrStatistics_Information).Tables[0].DefaultView;
                        break;
                    case "内存索引统计":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIN_MEMORY_Index_Stats).Tables[0].DefaultView;
                        break;

                    case "列存储索引统计":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrColumnStore_Index_Stats).Tables[0].DefaultView;
                        break;

                    case "索引缺失信息":
                       // CommHelper.WriteLogs(SQLSTR._sqlstrMissing_Index_Information, "err");
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMissing_Index_Information).Tables[0].DefaultView;
                        
                        break;
                    case "索引使用统计":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIndex_Usage).Tables[0].DefaultView;
                        break;
                    case "索引碎片报告":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIndex_Fragmentation).Tables[0].DefaultView;
                        break;

                    case "冗余索引":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrRedundant_Indexes).Tables[0].DefaultView;
                        break;
                    case "重复索引":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDuplicate_Indexes).Tables[0].DefaultView;
                        break;
                    case "无索引表":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTable_WT_Indexes).Tables[0].DefaultView;
                        break;
                    case "无聚集索引表":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTable_WT_CL_Indexes).Tables[0].DefaultView;
                        break;

                    case "列总计超过900Bytes索引":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIndex_GT_900_Bytes).Tables[0].DefaultView;
                        break;
                    case "可恢复的索引重建":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrResumable_index_rebuild).Tables[0].DefaultView;
                        break;
                    case "索引大小报告":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIndex_Size_Info).Tables[0].DefaultView;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }

        }

        /// <summary>
        /// 安全combobox
        ///  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_Security_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_Security.SelectedItem.ToString().Trim())
                {
                    case "登录信息":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLogin_Info).Tables[0].DefaultView;
                        break;
                    case "服务器审计":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrServerAudits).Tables[0].DefaultView;
                        break;
                    case "服务器审计规则":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrServerAuditSpecifications).Tables[0].DefaultView;
                        break;
                    case "警告":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAlerts).Tables[0].DefaultView;
                        break;
                  
                    case "策略管理":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPolicyBasedManagement).Tables[0].DefaultView;
                        break;

                    case "数据库CheckDB检查时间":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLastCheckDBDate).Tables[0].DefaultView;
                        break;

                    case "数据库级别权限":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Level_Permission).Tables[0].DefaultView;
                        break;

                    case "数据库角色成员":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_role_members).Tables[0].DefaultView;
                        break;

                    case "数据库对象权限":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_object_permission).Tables[0].DefaultView;
                        break;
                    case "服务账户报告":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrService_Account_Information).Tables[0].DefaultView;
                        break;

                    case "SPN检查":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSPN_Check).Tables[0].DefaultView;
                        break;

                    case "备份报告":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrBackup_info).Tables[0].DefaultView;
                        break;
                    case "最后备份信息":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLast_Backupup_Info).Tables[0].DefaultView;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }
        }

        #region  数据库报告
        private void comboBox_DatabaseInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_DatabaseInfo.SelectedItem.ToString().Trim())
                {
                    case "数据库大小报告":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDatabaseSize).Tables[0].DefaultView;
                        break;
                    case "数据库大表报告":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDatabase_Table_Size_Info).Tables[0].DefaultView;
                        break;
                    case "数据库文件报告":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_File_Size_Info).Tables[0].DefaultView;
                        break;
                    case "数据库外键报告":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrFK_WT_Index).Tables[0].DefaultView;
                        break;
                    case "日志空间使用":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLog_Space_Usage).Tables[0].DefaultView;
                        break;
                    case "不受信任的约束":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrUntrusted_Constraints).Tables[0].DefaultView;
                        break;
                    case "虚拟日志文件":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrVLF_Count).Tables[0].DefaultView;
                        break;
                    case "分区信息":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPartitioning_Info).Tables[0].DefaultView;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }

           

        }


    #endregion
        private void radioButton_failjob_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView_Log.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrFailed_Jobs).Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }

        }


        #region ///性能选择combobox
        private void comboBox_perinfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_Memory.SelectedItem.ToString().Trim())
                {

                    
                    case "内存状态":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemory_Pressure).Tables[0].DefaultView;
                        break;
                    
                    case "内存性能指标":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPerfMemory).Tables[0].DefaultView;
                        break;
                    case "内存延迟":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemory_Grants_Pending).Tables[0].DefaultView;
                        break;

                    case "页生命周期":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPage_Life_Expectancy).Tables[0].DefaultView;
                        break;

                    case "活动状态的内存分配TOP100":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemoryClerks_Info).Tables[0].DefaultView;
                        break;
                    case "内存管理器SQL Server内部分配":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemoryBrokers_Info).Tables[0].DefaultView;
                        break;

                    case "内存不足异常":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrOOM_Exception).Tables[0].DefaultView;
                        break;
                    case "环形缓冲区内存使用报告":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrRing_Buffer_Memory_Usage).Tables[0].DefaultView;
                        break;
                    case "内存Dump":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemory_dump).Tables[0].DefaultView;
                        break;









                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }

        }

        private void comboBox_TempDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_TempDB.SelectedItem.ToString().Trim())
                {

                    case "tempdb冲突信息":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTemp_db_Contention_Info).Tables[0].DefaultView;
                        break;
                    case "TempDB使用状况":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTemp_DB_Usage).Tables[0].DefaultView;

                        break;
                    case "TempDB详细使用情况":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTempDB_Usage_Info).Tables[0].DefaultView;

                        break;
                    case "TempDB打开的交易":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTempDB_Open_Tran).Tables[0].DefaultView;

                        break;
                    case "数据库使用tempdb中的总空间":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTempDB_VersionStore_space).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }

        }

        private void comboBox_Waiting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_Waiting.SelectedItem.ToString().Trim())
                {

                    case "旋转锁等待信息":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSpinlocks_information).Tables[0].DefaultView;

                        break;
                    case "闩锁等待统计信息":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrlatch_wait_stats).Tables[0].DefaultView;

                        break;
                    case "等待信息":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrWaits_information).Tables[0].DefaultView;

                        break;
                    case "数据库锁等待":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Lock_Wait).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }

        }

        private void comboBox_CPU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_CPU.SelectedItem.ToString().Trim())
                {
                    case "一小时内CPU指标":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPerfCPU1h).Tables[0].DefaultView;
                        break;

                   


                    case "CPU_TOP50查询":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTop_50_CPU_Exp_Query).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }

        }

        private void comboBox_Disk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_Disk.SelectedItem.ToString().Trim())
                {
                    case "磁盘延时":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDrive_Latency).Tables[0].DefaultView;
                        break;
                    case "文件延时":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrFile_Level_Latency).Tables[0].DefaultView;
                        break;
                    case "延迟的IO请求":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPending_IO_Requests).Tables[0].DefaultView;
                        break;
 
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }

        }

        private void comboBox_Session_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_Session.SelectedItem.ToString().Trim())
                {
                    case "当前运行查询":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrCurrent_running_queries).Tables[0].DefaultView;
                        break;

                    case "休眠的会话":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSleeping_sessions).Tables[0].DefaultView;
                        break;

                    case "休眠会话跟踪":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSleeping_sessions_tran).Tables[0].DefaultView;
                        break;



                    case "阻止的会话":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrBlocking_Sessions).Tables[0].DefaultView;
                        break;

                    case "工作进程数":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrWorker_Count).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }

        }

        private void comboBox_QuryStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_QuryStore.SelectedItem.ToString().Trim())
                {

                    case "查询存储基本信息":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrQuery_Store_information).Tables[0].DefaultView;

                        break;

                    case "查询存储统计信息":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrQuery_Store_Stats).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }


        }
      
        private void comboBox_ALLOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_ALLOther.SelectedItem.ToString().Trim())
                {
                    case "执行计划统计":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPlan_Count).Tables[0].DefaultView;
                        break;
                    case "性能数据收集":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPerfDataCollection).Tables[0].DefaultView;
                        break;
                    case "扩展事件":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrExtendedEvents).Tables[0].DefaultView;
                        break;
                    case "存储过程计划性能统计":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrExpensive_Procedure_Stats).Tables[0].DefaultView;
                        break;
                    case "触发器执行性能统计":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrExpensive_Trigger_Stats).Tables[0].DefaultView;
                        break;


                    case "质疑的页":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSuspectpages).Tables[0].DefaultView;

                        break;

                    case "代价过高的功能":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrExpensive_Functions).Tables[0].DefaultView;
                        break;
                    case "TOP50重编译查询":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTop_50_Recompile_Qry).Tables[0].DefaultView;
                        break;
                    case "使用频率Top50查询":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrFreq_Exec_Query).Tables[0].DefaultView;
                        break;

                    case "TOP50读查询":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTop_50_Reads_Query).Tables[0].DefaultView;
                        break;


                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("获取数据错误。");


            }


        }
        #endregion

        private void MainForm_Move(object sender, EventArgs e)
        {
            #region ///获取winform 起始位置 
            int x = (SystemInformation.WorkingArea.Width - this.Size.Width) / 2;
            int y = (SystemInformation.WorkingArea.Height - this.Size.Height) / 2;
            this.StartPosition = FormStartPosition.CenterScreen; //窗体的位置由Location属性决定
            this.Location = (Point)new Size(x, y);         //窗体的起始位置为(x,y)
            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            #region ///获取winform 起始位置 
            int x = (SystemInformation.WorkingArea.Width - this.Size.Width) / 2;
            int y = (SystemInformation.WorkingArea.Height - this.Size.Height) / 2;
            this.StartPosition = FormStartPosition.CenterScreen; //窗体的位置由Location属性决定
            this.Location = (Point)new Size(x, y);         //窗体的起始位置为(x,y)
             #endregion

        }

        private void timer_showPayForm_Tick(object sender, EventArgs e)
        {
          ShowPayForm();

        }


        /// <summary>
        /// 未注册提示信息
        /// </summary>
        public void ShowPayForm()
        {


            PayForm payForm = new PayForm();
            payForm.TopMost = true;
            payForm.ShowDialog(this);

        }
    }
}

