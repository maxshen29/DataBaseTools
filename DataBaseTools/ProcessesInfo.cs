using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBaseTools.SQLMethod;

namespace DataBaseTools
{
    public partial class ProcessesInfo : Form
    {
        public ProcessesInfo(string spid, string dbid)
        {
            InitializeComponent();
            textBox_CMD.AppendText(SQLCommon.GetDatabaseName(dbid));
            textBox_CMD.AppendText(SQLCommon.GetProcessesCMD(spid));
        }
    }
}
