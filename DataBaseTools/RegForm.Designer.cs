namespace DataBaseTools
{
    partial class RegForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegForm));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ApplicationCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_RegCode = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_Free = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "申请码：";
            // 
            // textBox_ApplicationCode
            // 
            this.textBox_ApplicationCode.Location = new System.Drawing.Point(145, 26);
            this.textBox_ApplicationCode.Name = "textBox_ApplicationCode";
            this.textBox_ApplicationCode.ReadOnly = true;
            this.textBox_ApplicationCode.Size = new System.Drawing.Size(589, 27);
            this.textBox_ApplicationCode.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "注册码：";
            // 
            // textBox_RegCode
            // 
            this.textBox_RegCode.Location = new System.Drawing.Point(145, 80);
            this.textBox_RegCode.Name = "textBox_RegCode";
            this.textBox_RegCode.Size = new System.Drawing.Size(589, 27);
            this.textBox_RegCode.TabIndex = 4;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(640, 130);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(94, 29);
            this.button_ok.TabIndex = 5;
            this.button_ok.Text = "注册";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBox_RegCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button_ok);
            this.panel1.Controls.Add(this.button_Free);
            this.panel1.Controls.Add(this.textBox_ApplicationCode);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(14, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 210);
            this.panel1.TabIndex = 6;
            // 
            // button_Free
            // 
            this.button_Free.Location = new System.Drawing.Point(640, 165);
            this.button_Free.Name = "button_Free";
            this.button_Free.Size = new System.Drawing.Size(94, 29);
            this.button_Free.TabIndex = 1;
            this.button_Free.Text = "免费试用";
            this.button_Free.UseVisualStyleBackColor = true;
            this.button_Free.Click += new System.EventHandler(this.button_Free_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(409, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "此软件为付费软件，申请注册码请联系：131364@qq.com";
            // 
            // RegForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 233);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "应用注册";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RegForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
        private TextBox textBox_ApplicationCode;
        private Label label2;
        private TextBox textBox_RegCode;
        private Button button_ok;
        private Panel panel1;
        private Label label3;
        private Button button_Free;
    }
}