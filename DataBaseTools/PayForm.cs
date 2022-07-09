using System;
using System.Collections.Generic;
using System.ComponentModel;
using DataBaseTools.Model;

namespace DataBaseTools
{
    public partial class PayForm : Form
    {

       // private bool payClose=false;
        public PayForm()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            string[] AboutInfo = new string[4];
            AboutInfo[0] = IntroInfo.OMInfo;
            AboutInfo[1] = IntroInfo.PerformInfo;
            AboutInfo[2] = IntroInfo.HealthCheck;
            AboutInfo[3] = IntroInfo.DBMigration;

            int i = 0;

            i= DateTime.Now.Minute%4;
            textBox_Info.AppendText(AboutInfo[i]);
            textBox_Info.AppendText("\r\n\r\n\r\n\r\n\r\n");
            textBox_Info.AppendText(IntroInfo.Connectinfo);
            textBox_Info.AppendText(IntroInfo.ClassInfo);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Close();
        }

        private void PayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        //    e.Cancel = true;
        }
        //重写OnClosing使点击关闭按键时窗体能够缩进托盘
        protected override void OnClosing(CancelEventArgs e)
        {
        //    this.ShowInTaskbar = false;       
       //     e.Cancel = true;
        }
 

    }
}
