using DataBaseTools.Common;
using DataBaseTools.SQLMethod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBaseTools
{
    public partial class info : Form
    {
        public info()
        {
            InitializeComponent();


        }

        private void info_Load(object sender, EventArgs e)
        {

        }

        private void info_Activated(object sender, EventArgs e)
        {

            if (SQLCommon.TryConnect())
            {
                MainForm._haveConnect = true;
                this.Close();
            }
            else
            {
                MainForm._haveConnect = false;
                CommHelper.MsgBoxInfo("无数据库连接，或者连接有错，请配置数据库连接！");
                this.Close();
            }

        }
    }
}
