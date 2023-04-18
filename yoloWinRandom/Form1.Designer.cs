namespace yoloWinRandom
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_go = new Button();
            label1 = new Label();
            txt_fisrt = new TextBox();
            txt_second = new TextBox();
            txt_thrid = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txt_trainImgPath = new TextBox();
            label4 = new Label();
            label5 = new Label();
            txt_trainLabelPath = new TextBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            btn_createDir = new Button();
            groupBox1 = new GroupBox();
            label9 = new Label();
            txt_createDirPath = new TextBox();
            groupBox2 = new GroupBox();
            groupBox5 = new GroupBox();
            txt_testLabelPath = new TextBox();
            label12 = new Label();
            label13 = new Label();
            txt_testImgPath = new TextBox();
            groupBox4 = new GroupBox();
            txt_valLabelPath = new TextBox();
            label10 = new Label();
            label11 = new Label();
            txt_valImgPath = new TextBox();
            groupBox3 = new GroupBox();
            groupBox6 = new GroupBox();
            txt_logs = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox6.SuspendLayout();
            SuspendLayout();
            // 
            // btn_go
            // 
            btn_go.Location = new Point(581, 304);
            btn_go.Name = "btn_go";
            btn_go.Size = new Size(105, 42);
            btn_go.TabIndex = 0;
            btn_go.Text = "开始";
            btn_go.UseVisualStyleBackColor = true;
            btn_go.Click += btn_go_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(116, 323);
            label1.Name = "label1";
            label1.Size = new Size(42, 21);
            label1.TabIndex = 1;
            label1.Text = "比例";
            // 
            // txt_fisrt
            // 
            txt_fisrt.Location = new Point(193, 323);
            txt_fisrt.Name = "txt_fisrt";
            txt_fisrt.Size = new Size(67, 23);
            txt_fisrt.TabIndex = 2;
            txt_fisrt.Text = "7";
            txt_fisrt.TextAlign = HorizontalAlignment.Center;
            // 
            // txt_second
            // 
            txt_second.Location = new Point(293, 323);
            txt_second.Name = "txt_second";
            txt_second.Size = new Size(67, 23);
            txt_second.TabIndex = 2;
            txt_second.Text = "2";
            txt_second.TextAlign = HorizontalAlignment.Center;
            // 
            // txt_thrid
            // 
            txt_thrid.Location = new Point(391, 323);
            txt_thrid.Name = "txt_thrid";
            txt_thrid.Size = new Size(67, 23);
            txt_thrid.TabIndex = 2;
            txt_thrid.Text = "1";
            txt_thrid.TextAlign = HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(269, 323);
            label2.Name = "label2";
            label2.Size = new Size(26, 21);
            label2.TabIndex = 1;
            label2.Text = "：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(366, 323);
            label3.Name = "label3";
            label3.Size = new Size(26, 21);
            label3.TabIndex = 1;
            label3.Text = "：";
            // 
            // txt_trainImgPath
            // 
            txt_trainImgPath.Location = new Point(88, 23);
            txt_trainImgPath.Name = "txt_trainImgPath";
            txt_trainImgPath.Size = new Size(595, 23);
            txt_trainImgPath.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(3, 23);
            label4.Name = "label4";
            label4.Size = new Size(74, 21);
            label4.TabIndex = 1;
            label4.Text = "图片路径";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(3, 52);
            label5.Name = "label5";
            label5.Size = new Size(74, 21);
            label5.TabIndex = 1;
            label5.Text = "标签路径";
            // 
            // txt_trainLabelPath
            // 
            txt_trainLabelPath.Location = new Point(88, 52);
            txt_trainLabelPath.Name = "txt_trainLabelPath";
            txt_trainLabelPath.Size = new Size(595, 23);
            txt_trainLabelPath.TabIndex = 3;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(203, 290);
            label6.Name = "label6";
            label6.Size = new Size(42, 21);
            label6.TabIndex = 1;
            label6.Text = "训练";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(304, 290);
            label7.Name = "label7";
            label7.Size = new Size(42, 21);
            label7.TabIndex = 1;
            label7.Text = "验证";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(401, 290);
            label8.Name = "label8";
            label8.Size = new Size(42, 21);
            label8.TabIndex = 1;
            label8.Text = "测试";
            // 
            // btn_createDir
            // 
            btn_createDir.Location = new Point(625, 31);
            btn_createDir.Name = "btn_createDir";
            btn_createDir.Size = new Size(67, 25);
            btn_createDir.TabIndex = 4;
            btn_createDir.Text = "创建";
            btn_createDir.UseVisualStyleBackColor = true;
            btn_createDir.Click += btn_createDir_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(txt_createDirPath);
            groupBox1.Controls.Add(btn_createDir);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(716, 71);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "yolo标准文件夹";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(17, 35);
            label9.Name = "label9";
            label9.Size = new Size(68, 17);
            label9.TabIndex = 6;
            label9.Text = "文件夹路径";
            // 
            // txt_createDirPath
            // 
            txt_createDirPath.Location = new Point(91, 32);
            txt_createDirPath.Name = "txt_createDirPath";
            txt_createDirPath.Size = new Size(535, 23);
            txt_createDirPath.TabIndex = 5;
            txt_createDirPath.Click += txt_createDirPath_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox5);
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Controls.Add(btn_go);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(txt_thrid);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(txt_second);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(txt_fisrt);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 71);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(716, 372);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "比例划分，如果采用标准路径，选择最上面的文件夹即可";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(txt_testLabelPath);
            groupBox5.Controls.Add(label12);
            groupBox5.Controls.Add(label13);
            groupBox5.Controls.Add(txt_testImgPath);
            groupBox5.Dock = DockStyle.Top;
            groupBox5.Location = new Point(3, 191);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(710, 88);
            groupBox5.TabIndex = 6;
            groupBox5.TabStop = false;
            groupBox5.Text = "测试";
            // 
            // txt_testLabelPath
            // 
            txt_testLabelPath.Location = new Point(88, 52);
            txt_testLabelPath.Name = "txt_testLabelPath";
            txt_testLabelPath.Size = new Size(595, 23);
            txt_testLabelPath.TabIndex = 3;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label12.Location = new Point(3, 52);
            label12.Name = "label12";
            label12.Size = new Size(74, 21);
            label12.TabIndex = 1;
            label12.Text = "标签路径";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label13.Location = new Point(3, 23);
            label13.Name = "label13";
            label13.Size = new Size(74, 21);
            label13.TabIndex = 1;
            label13.Text = "图片路径";
            // 
            // txt_testImgPath
            // 
            txt_testImgPath.Location = new Point(88, 23);
            txt_testImgPath.Name = "txt_testImgPath";
            txt_testImgPath.Size = new Size(595, 23);
            txt_testImgPath.TabIndex = 3;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(txt_valLabelPath);
            groupBox4.Controls.Add(label10);
            groupBox4.Controls.Add(label11);
            groupBox4.Controls.Add(txt_valImgPath);
            groupBox4.Dock = DockStyle.Top;
            groupBox4.Location = new Point(3, 103);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(710, 88);
            groupBox4.TabIndex = 5;
            groupBox4.TabStop = false;
            groupBox4.Text = "验证";
            // 
            // txt_valLabelPath
            // 
            txt_valLabelPath.Location = new Point(88, 52);
            txt_valLabelPath.Name = "txt_valLabelPath";
            txt_valLabelPath.Size = new Size(595, 23);
            txt_valLabelPath.TabIndex = 3;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(3, 52);
            label10.Name = "label10";
            label10.Size = new Size(74, 21);
            label10.TabIndex = 1;
            label10.Text = "标签路径";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(3, 23);
            label11.Name = "label11";
            label11.Size = new Size(74, 21);
            label11.TabIndex = 1;
            label11.Text = "图片路径";
            // 
            // txt_valImgPath
            // 
            txt_valImgPath.Location = new Point(88, 23);
            txt_valImgPath.Name = "txt_valImgPath";
            txt_valImgPath.Size = new Size(595, 23);
            txt_valImgPath.TabIndex = 3;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(txt_trainLabelPath);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(txt_trainImgPath);
            groupBox3.Dock = DockStyle.Top;
            groupBox3.Location = new Point(3, 19);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(710, 84);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "训练";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(txt_logs);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(0, 443);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(716, 170);
            groupBox6.TabIndex = 7;
            groupBox6.TabStop = false;
            groupBox6.Text = "操作日志";
            // 
            // txt_logs
            // 
            txt_logs.Dock = DockStyle.Fill;
            txt_logs.Location = new Point(3, 19);
            txt_logs.Multiline = true;
            txt_logs.Name = "txt_logs";
            txt_logs.ScrollBars = ScrollBars.Both;
            txt_logs.Size = new Size(710, 148);
            txt_logs.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(716, 613);
            Controls.Add(groupBox6);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "yolo训练图片和标签随机分配";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btn_go;
        private Label label1;
        private TextBox txt_fisrt;
        private TextBox txt_second;
        private TextBox txt_thrid;
        private Label label2;
        private Label label3;
        private TextBox txt_trainImgPath;
        private Label label4;
        private Label label5;
        private TextBox txt_trainLabelPath;
        private Label label6;
        private Label label7;
        private Label label8;
        private Button btn_createDir;
        private GroupBox groupBox1;
        private Label label9;
        private TextBox txt_createDirPath;
        private GroupBox groupBox2;
        private GroupBox groupBox5;
        private TextBox txt_testLabelPath;
        private Label label12;
        private Label label13;
        private TextBox txt_testImgPath;
        private GroupBox groupBox4;
        private TextBox txt_valLabelPath;
        private Label label10;
        private Label label11;
        private TextBox txt_valImgPath;
        private GroupBox groupBox3;
        private GroupBox groupBox6;
        private TextBox txt_logs;
    }
}