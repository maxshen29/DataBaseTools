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
        //#region ����ʹ��


        ///// <summary>
        ///// ����ʹ��
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
        //static extern bool DwmIsCompositionEnabled(); //Dll ���� DwmApi

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);
        //    if (DwmIsCompositionEnabled())
        //    {
        //        e.Graphics.Clear(Color.Silver); //�������ú�ɫ��䣨Dwm ��Ѻ�ɫ��Ϊ͸������
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

        //��������
        private delegate bool IncreaseHandle(int nValue, string vinfo);//������
        private IncreaseHandle _Increase = null;//�����������ں����ʵ��������
        private int _Max = 100;//����ʵ���������������Ը����Լ�����Ҫ���Լ��ı�

        public MainForm()
        {

           /* if (!CommHelper.ValidationRegKey())
            {
                CommHelper.MsgBoxInfo("δ��ע�������ע�������������ע���룡");
                RegForm regForm = new RegForm();
                regForm.ShowDialog();

            }
            else
            {
                _haveReg = true;
            }*/

            InitializeComponent();

            this.toolStripStatusLabel1.Text = "���ݿ⹤�ߣ�";

            textBox_AboutUS.AppendText(IntroInfo.Connectinfo + "\r\n\r\n");
            textBox_AboutUS.AppendText("���ŷ���\r\n");
            textBox_AboutUS.AppendText(IntroInfo.PerformInfo + "\r\n\r\n");

            textBox_AboutUS.AppendText("������飺\r\n");
            textBox_AboutUS.AppendText(IntroInfo.HealthCheck + "\r\n\r\n");

            textBox_AboutUS.AppendText("��ά����\r\n");
            textBox_AboutUS.AppendText(IntroInfo.OMInfo + "\r\n\r\n");

            textBox_AboutUS.AppendText("��������\r\n");
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
                    //  CommHelper.MsgBoxInfo("�����������ݿ⣡");

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
                CommHelper.MsgBoxInfo("���ӳɹ�");
              
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
                CommHelper.MsgBoxERR("����ʧ��");
            }
            finally
            {
                if (conn != null)
                {
                    //�ر����ݿ�����
                    conn.Close();
                }

            }


             
        }

       

         
       

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_haveConnect)
            {
                CommHelper.MsgBoxInfo("�����ݿ����ӣ����������д����������ݿ����ӣ�");
                return;
            }

            this.toolStripStatusLabel1.Text = "���ݿ⹤�ߣ�";
            string _TabName = tabControl1.SelectedTab.Name.Trim();
            switch (_TabName)
            {
                
                case "tabPage_Info"://������Ϣ
                    
                    break;
                case "tabPage_Log":  ///��־��ѯ
                    if (!_haveGetSQLlog)
                    {
                        try
                        {
                            this.toolStripStatusLabel1.Text = "SQL��־��Ϣ��";

                            dataGridView_sqllog.DataSource = SQLCommon.GetSQLLogs(-30, "m", "sqlserver").Tables[0].DefaultView;
                            dataGridView_sqllog.Columns[0].Width = 150;
                            dataGridView_sqllog.Columns[1].Width = 100;
                            dataGridView_sqllog.Columns[2].Width = 1000;
                            _haveGetSQLlog = true;
                        }
                        catch (Exception ex)
                        {
                            CommHelper.WriteLogs(ex.StackTrace, "Err");

                            CommHelper.MsgBoxInfo("����SQL Server��־��");


                        }
                    }
                    break;
                case "tabPage_Perf": //���ܼ���������
                    if (!_haveGetPerformanceCounters)
                    {
                        try
                        {

                            this.toolStripStatusLabel1.Text = "���ܼ�������Ϣ��";
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
                            CommHelper.MsgBoxInfo("�������");


                        }
                    }
                    break;
                case "tabPage_Processes": //���ݿ����
                    if (!_haveGetProcesses)
                    {
                        try
                        {
                            this.toolStripStatusLabel1.Text = "������Ϣ������б���Բ鿴������ϸ��Ϣ����";

                            dataGridView_Processes.DataSource = SQLCommon.GetSysProcesses().Tables[0].DefaultView;
                            _haveGetProcesses = true;
                        }
                        catch (Exception ex)
                        {
                            CommHelper.WriteLogs(ex.StackTrace, "Err");
                            CommHelper.MsgBoxInfo("�������");


                        }

                    }

                    break;
                case "tabPage_ConfigInfo"://����������Ϣ
                    if (!_haveGetConfig)
                    {
                        try
                        {
                            this.toolStripStatusLabel1.Text = "��ȡϵͳ������Ϣ������Ҫ���߼���Ϣ����Ҫshow advanced options����Ϊ1";

                            dataGridView_Config.DataSource = SQLCommon.GetSQLConfig().Tables[0].DefaultView;
                            dataGridView_Config.Columns[0].Width = 300;
                            _haveGetConfig = true;
                        }
                        catch (Exception ex)
                        {
                            CommHelper.WriteLogs(ex.StackTrace, "Err");
                            CommHelper.MsgBoxInfo("�������");


                        }
                    }
                    break;
                case "tabPage_ClusterInfo": //Ⱥ����Ϣ
                    this.toolStripStatusLabel1.Text = "Ⱥ����Ϣ,����ѡ����ϸ��Ϣ�鿴����";

                    break;
                case "tabPage_Job": //job��Ϣ
                    this.toolStripStatusLabel1.Text = "Job��Ϣ����";
                    break;
                case "tabPage_DataBaseInfo": //���ݿ���Ϣ
                    this.toolStripStatusLabel1.Text = "���ݿ���Ϣ����";

                    break;              
 
                case "tabPage_Index":   ///����
                    this.toolStripStatusLabel1.Text = "������Ϣ����";

                    break;
                case "tabPage_Per_Check":  //���ܼ��
                    this.toolStripStatusLabel1.Text = "���ܼ�飡��";

                    break;
                case "tabPage_Security":  //��ȫ���
                    this.toolStripStatusLabel1.Text = "��ȫ��飡��";
                  


                    break;
                case "tabPage_Other":  //����
                    this.toolStripStatusLabel1.Text = "��������";

                    break;
                default:
                    break;

            }             

             
        }

 

        /// <summary>
        /// ��ȡ������Ϣ
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
                    CommHelper.MsgBoxInfo("����SQL Server��־��");
                }
                else
                {
                    CommHelper.MsgBoxInfo("����Agent��־��");
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
                CommHelper.MsgBoxInfo("��ѡ��OBJECT_NAME");
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
        /// Ⱥ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                switch (comboBox_clusterinfo.SelectedItem.ToString().Trim())
                {

                    case "���ݿ⾵��˵�Ự��AlwaysON�������Ự��":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Mirroring_Endpoints).Tables[0].DefaultView;
                        break;
                    case "����ת��Ⱥ����Ϣ":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_Cluster_Info).Tables[0].DefaultView;
                        break;
                    case "����ת��Ⱥ�������־����":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrserver_diagnostics_log).Tables[0].DefaultView;
                        break;
                    case "AlwasyON���ݿ�״̬����":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_DB_State_Config).Tables[0].DefaultView;
                        break;
                    case "AlwasyON״̬ʶ��":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_State_Identification).Tables[0].DefaultView;
                        break;
                    case "AlwasyON״̬����":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_State_Config).Tables[0].DefaultView;
                        break;
                    case "Ⱥ���ٲ�����":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrWC_Quorum_Network).Tables[0].DefaultView;
                        break;
                    case "���ݿ⾵��Ȩ��":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Mirroring_Permission).Tables[0].DefaultView;
                        break;
                    case "AlwasyON���ݿ����ת����Ϣ":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDatabaseHADR).Tables[0].DefaultView;
                        break;

                    case "AG����IP":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_Listener_IP).Tables[0].DefaultView;
                        break;

                    case "·����Ϣ":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrRouting_list_Info).Tables[0].DefaultView;
                        break;

                    case "���ݿ⾵����Ϣ":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Mirroring_Info).Tables[0].DefaultView;
                        break;

                    case "AGȺ��״̬":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_Cluster_Status).Tables[0].DefaultView;
                        break;


                    case "AGȺ������״̬":
                        dataGridView_Cluster.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAG_Health_Status).Tables[0].DefaultView;
                        break;


                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_other_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_other.SelectedItem.ToString().Trim())
                {

                    case "���ƴ���":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrReplicationErrors).Tables[0].DefaultView;
                        break;
                    case "����״̬":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSQLSvc_Status).Tables[0].DefaultView;
                        break;
                    case "���ݿ��ʼ�":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDatabaseMail).Tables[0].DefaultView;
                        break;

                    case "��Դ����Ϣ":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrResourceGroupPools).Tables[0].DefaultView;
                        break;
                    case "�˵���Ϣ":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrEndpoints).Tables[0].DefaultView;
                        break;
                    case "����������":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrResourceGovernorGroups).Tables[0].DefaultView;
                        break;

                    case "������������":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrServerTriggers).Tables[0].DefaultView;
                        break;

                    case "���ص���������ģ��":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLoadedModules).Tables[0].DefaultView;
                        dataGridView_other.Columns[2].Width = 300;
                        break;


                    case "���õĹ���":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDeprecated_Features).Tables[0].DefaultView;
                        break;

                    case "�����豸":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrBackupDevices).Tables[0].DefaultView;
                        break;

                    case "���ӷ�����":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLinkedServers).Tables[0].DefaultView;
                        break;
                 
                 
                    case "SQLTraces����":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSqlTraces).Tables[0].DefaultView;
                        break;

                    case "�������Ա":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrOperators).Tables[0].DefaultView;
                        break;


                    case "��ѯ�Ż���SQL Server����ϸͳ����Ϣ":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrOptimizer_information).Tables[0].DefaultView;
                        break;

                    case "MaterDB�ڶ���":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrObject_In_Master_DB).Tables[0].DefaultView;
                        break;

                    case "�����Զ�ִ�д洢����":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrStartup_Procedures).Tables[0].DefaultView;
                        break;

                    case "��������":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrinline_function).Tables[0].DefaultView;
                        break;

                    case "������Ϣ":
                        dataGridView_other.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDisk_LUN_Info).Tables[0].DefaultView;
                        break;



                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


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

                    case "����״̬ͳ��":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrStatistics_Information).Tables[0].DefaultView;
                        break;
                    case "����״̬ͳ��1":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrStatistics_Information).Tables[0].DefaultView;
                        break;
                    case "�ڴ�����ͳ��":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIN_MEMORY_Index_Stats).Tables[0].DefaultView;
                        break;

                    case "�д洢����ͳ��":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrColumnStore_Index_Stats).Tables[0].DefaultView;
                        break;

                    case "����ȱʧ��Ϣ":
                       // CommHelper.WriteLogs(SQLSTR._sqlstrMissing_Index_Information, "err");
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMissing_Index_Information).Tables[0].DefaultView;
                        
                        break;
                    case "����ʹ��ͳ��":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIndex_Usage).Tables[0].DefaultView;
                        break;
                    case "������Ƭ����":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIndex_Fragmentation).Tables[0].DefaultView;
                        break;

                    case "��������":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrRedundant_Indexes).Tables[0].DefaultView;
                        break;
                    case "�ظ�����":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDuplicate_Indexes).Tables[0].DefaultView;
                        break;
                    case "��������":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTable_WT_Indexes).Tables[0].DefaultView;
                        break;
                    case "�޾ۼ�������":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTable_WT_CL_Indexes).Tables[0].DefaultView;
                        break;

                    case "���ܼƳ���900Bytes����":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIndex_GT_900_Bytes).Tables[0].DefaultView;
                        break;
                    case "�ɻָ��������ؽ�":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrResumable_index_rebuild).Tables[0].DefaultView;
                        break;
                    case "������С����":
                        dataGridView_Index.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrIndex_Size_Info).Tables[0].DefaultView;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }

        }

        /// <summary>
        /// ��ȫcombobox
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
                    case "��¼��Ϣ":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLogin_Info).Tables[0].DefaultView;
                        break;
                    case "���������":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrServerAudits).Tables[0].DefaultView;
                        break;
                    case "��������ƹ���":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrServerAuditSpecifications).Tables[0].DefaultView;
                        break;
                    case "����":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrAlerts).Tables[0].DefaultView;
                        break;
                  
                    case "���Թ���":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPolicyBasedManagement).Tables[0].DefaultView;
                        break;

                    case "���ݿ�CheckDB���ʱ��":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLastCheckDBDate).Tables[0].DefaultView;
                        break;

                    case "���ݿ⼶��Ȩ��":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Level_Permission).Tables[0].DefaultView;
                        break;

                    case "���ݿ��ɫ��Ա":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_role_members).Tables[0].DefaultView;
                        break;

                    case "���ݿ����Ȩ��":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_object_permission).Tables[0].DefaultView;
                        break;
                    case "�����˻�����":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrService_Account_Information).Tables[0].DefaultView;
                        break;

                    case "SPN���":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSPN_Check).Tables[0].DefaultView;
                        break;

                    case "���ݱ���":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrBackup_info).Tables[0].DefaultView;
                        break;
                    case "��󱸷���Ϣ":
                        dataGridView_Security.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLast_Backupup_Info).Tables[0].DefaultView;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }
        }

        #region  ���ݿⱨ��
        private void comboBox_DatabaseInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_DatabaseInfo.SelectedItem.ToString().Trim())
                {
                    case "���ݿ��С����":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDatabaseSize).Tables[0].DefaultView;
                        break;
                    case "���ݿ�����":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDatabase_Table_Size_Info).Tables[0].DefaultView;
                        break;
                    case "���ݿ��ļ�����":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_File_Size_Info).Tables[0].DefaultView;
                        break;
                    case "���ݿ��������":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrFK_WT_Index).Tables[0].DefaultView;
                        break;
                    case "��־�ռ�ʹ��":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrLog_Space_Usage).Tables[0].DefaultView;
                        break;
                    case "�������ε�Լ��":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrUntrusted_Constraints).Tables[0].DefaultView;
                        break;
                    case "������־�ļ�":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrVLF_Count).Tables[0].DefaultView;
                        break;
                    case "������Ϣ":
                        dataGridView_Databaseinfo.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPartitioning_Info).Tables[0].DefaultView;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


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

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }

        }


        #region ///����ѡ��combobox
        private void comboBox_perinfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_Memory.SelectedItem.ToString().Trim())
                {

                    
                    case "�ڴ�״̬":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemory_Pressure).Tables[0].DefaultView;
                        break;
                    
                    case "�ڴ�����ָ��":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPerfMemory).Tables[0].DefaultView;
                        break;
                    case "�ڴ��ӳ�":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemory_Grants_Pending).Tables[0].DefaultView;
                        break;

                    case "ҳ��������":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPage_Life_Expectancy).Tables[0].DefaultView;
                        break;

                    case "�״̬���ڴ����TOP100":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemoryClerks_Info).Tables[0].DefaultView;
                        break;
                    case "�ڴ������SQL Server�ڲ�����":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemoryBrokers_Info).Tables[0].DefaultView;
                        break;

                    case "�ڴ治���쳣":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrOOM_Exception).Tables[0].DefaultView;
                        break;
                    case "���λ������ڴ�ʹ�ñ���":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrRing_Buffer_Memory_Usage).Tables[0].DefaultView;
                        break;
                    case "�ڴ�Dump":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrMemory_dump).Tables[0].DefaultView;
                        break;









                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }

        }

        private void comboBox_TempDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_TempDB.SelectedItem.ToString().Trim())
                {

                    case "tempdb��ͻ��Ϣ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTemp_db_Contention_Info).Tables[0].DefaultView;
                        break;
                    case "TempDBʹ��״��":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTemp_DB_Usage).Tables[0].DefaultView;

                        break;
                    case "TempDB��ϸʹ�����":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTempDB_Usage_Info).Tables[0].DefaultView;

                        break;
                    case "TempDB�򿪵Ľ���":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTempDB_Open_Tran).Tables[0].DefaultView;

                        break;
                    case "���ݿ�ʹ��tempdb�е��ܿռ�":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTempDB_VersionStore_space).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }

        }

        private void comboBox_Waiting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_Waiting.SelectedItem.ToString().Trim())
                {

                    case "��ת���ȴ���Ϣ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSpinlocks_information).Tables[0].DefaultView;

                        break;
                    case "�����ȴ�ͳ����Ϣ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrlatch_wait_stats).Tables[0].DefaultView;

                        break;
                    case "�ȴ���Ϣ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrWaits_information).Tables[0].DefaultView;

                        break;
                    case "���ݿ����ȴ�":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDB_Lock_Wait).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }

        }

        private void comboBox_CPU_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_CPU.SelectedItem.ToString().Trim())
                {
                    case "һСʱ��CPUָ��":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPerfCPU1h).Tables[0].DefaultView;
                        break;

                   


                    case "CPU_TOP50��ѯ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTop_50_CPU_Exp_Query).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }

        }

        private void comboBox_Disk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_Disk.SelectedItem.ToString().Trim())
                {
                    case "������ʱ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrDrive_Latency).Tables[0].DefaultView;
                        break;
                    case "�ļ���ʱ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrFile_Level_Latency).Tables[0].DefaultView;
                        break;
                    case "�ӳٵ�IO����":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPending_IO_Requests).Tables[0].DefaultView;
                        break;
 
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }

        }

        private void comboBox_Session_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_Session.SelectedItem.ToString().Trim())
                {
                    case "��ǰ���в�ѯ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrCurrent_running_queries).Tables[0].DefaultView;
                        break;

                    case "���ߵĻỰ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSleeping_sessions).Tables[0].DefaultView;
                        break;

                    case "���߻Ự����":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSleeping_sessions_tran).Tables[0].DefaultView;
                        break;



                    case "��ֹ�ĻỰ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrBlocking_Sessions).Tables[0].DefaultView;
                        break;

                    case "����������":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrWorker_Count).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }

        }

        private void comboBox_QuryStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_QuryStore.SelectedItem.ToString().Trim())
                {

                    case "��ѯ�洢������Ϣ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrQuery_Store_information).Tables[0].DefaultView;

                        break;

                    case "��ѯ�洢ͳ����Ϣ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrQuery_Store_Stats).Tables[0].DefaultView;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }


        }
      
        private void comboBox_ALLOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox_ALLOther.SelectedItem.ToString().Trim())
                {
                    case "ִ�мƻ�ͳ��":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPlan_Count).Tables[0].DefaultView;
                        break;
                    case "���������ռ�":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrPerfDataCollection).Tables[0].DefaultView;
                        break;
                    case "��չ�¼�":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrExtendedEvents).Tables[0].DefaultView;
                        break;
                    case "�洢���̼ƻ�����ͳ��":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrExpensive_Procedure_Stats).Tables[0].DefaultView;
                        break;
                    case "������ִ������ͳ��":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrExpensive_Trigger_Stats).Tables[0].DefaultView;
                        break;


                    case "���ɵ�ҳ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrSuspectpages).Tables[0].DefaultView;

                        break;

                    case "���۹��ߵĹ���":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrExpensive_Functions).Tables[0].DefaultView;
                        break;
                    case "TOP50�ر����ѯ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTop_50_Recompile_Qry).Tables[0].DefaultView;
                        break;
                    case "ʹ��Ƶ��Top50��ѯ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrFreq_Exec_Query).Tables[0].DefaultView;
                        break;

                    case "TOP50����ѯ":
                        dataGridView_percheck.DataSource = SQLCommon.GetCommonDataSet(SQLSTR._sqlstrTop_50_Reads_Query).Tables[0].DefaultView;
                        break;


                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommHelper.WriteLogs(ex.StackTrace, "Err");

                CommHelper.MsgBoxInfo("��ȡ���ݴ���");


            }


        }
        #endregion

        private void MainForm_Move(object sender, EventArgs e)
        {
            #region ///��ȡwinform ��ʼλ�� 
            int x = (SystemInformation.WorkingArea.Width - this.Size.Width) / 2;
            int y = (SystemInformation.WorkingArea.Height - this.Size.Height) / 2;
            this.StartPosition = FormStartPosition.CenterScreen; //�����λ����Location���Ծ���
            this.Location = (Point)new Size(x, y);         //�������ʼλ��Ϊ(x,y)
            #endregion
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            #region ///��ȡwinform ��ʼλ�� 
            int x = (SystemInformation.WorkingArea.Width - this.Size.Width) / 2;
            int y = (SystemInformation.WorkingArea.Height - this.Size.Height) / 2;
            this.StartPosition = FormStartPosition.CenterScreen; //�����λ����Location���Ծ���
            this.Location = (Point)new Size(x, y);         //�������ʼλ��Ϊ(x,y)
             #endregion

        }

        private void timer_showPayForm_Tick(object sender, EventArgs e)
        {
          ShowPayForm();

        }


        /// <summary>
        /// δע����ʾ��Ϣ
        /// </summary>
        public void ShowPayForm()
        {


            PayForm payForm = new PayForm();
            payForm.TopMost = true;
            payForm.ShowDialog(this);

        }
    }
}

