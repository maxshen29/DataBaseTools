using DataBaseTools.Common;

namespace DataBaseTools
{
    public partial class RegForm : Form
    {
        public RegForm()
        {
            InitializeComponent();
           textBox_ApplicationCode.Text =CommHelper.GetBaseKeyInfo();

            

        }

        private void button1_Click(object sender, EventArgs e)
        {       



            if (CommHelper.EncryptDES(textBox_ApplicationCode.Text, CommHelper._encryptKey) == textBox_RegCode.Text)
            {
           
                CommHelper.MsgBoxInfo("注册成功！");
                CommHelper.WriteConfigClassKey(textBox_RegCode.Text);
                MainForm._haveReg = true;
                this.Close();

            }
            else
            {
                MainForm._haveReg = false;
                CommHelper.MsgBoxERR("注册码错误！");


            }
        }

        private void RegForm_FormClosed(object sender, FormClosedEventArgs e)
        {
           //  Application.Exit();
             //this.Close();
        }

        private void button_Free_Click(object sender, EventArgs e)
        {
            MainForm._haveFreeR=true;
            MainForm._haveReg = true;
            this.Close();
        }
    }
}
