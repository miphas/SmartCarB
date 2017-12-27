namespace SmartCar {
    partial class Form_Path {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.boxWay = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.rioCX = new System.Windows.Forms.RadioButton();
            this.rioXZ = new System.Windows.Forms.RadioButton();
            this.rioSLJ = new System.Windows.Forms.RadioButton();
            this.rioDLJ = new System.Windows.Forms.RadioButton();
            this.rioLD = new System.Windows.Forms.RadioButton();
            this.rioPT = new System.Windows.Forms.RadioButton();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.boxModel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.picGenMap = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGenMap)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitContainer1.Panel1.Controls.Add(this.boxWay);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.rioCX);
            this.splitContainer1.Panel1.Controls.Add(this.rioXZ);
            this.splitContainer1.Panel1.Controls.Add(this.rioSLJ);
            this.splitContainer1.Panel1.Controls.Add(this.rioDLJ);
            this.splitContainer1.Panel1.Controls.Add(this.rioLD);
            this.splitContainer1.Panel1.Controls.Add(this.rioPT);
            this.splitContainer1.Panel1.Controls.Add(this.btnDel);
            this.splitContainer1.Panel1.Controls.Add(this.btnSaveAs);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.boxModel);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer1.Panel1.Controls.Add(this.btnOpen);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel2.Controls.Add(this.picGenMap);
            this.splitContainer1.Size = new System.Drawing.Size(784, 508);
            this.splitContainer1.SplitterDistance = 222;
            this.splitContainer1.TabIndex = 1;
            // 
            // boxWay
            // 
            this.boxWay.FormattingEnabled = true;
            this.boxWay.Items.AddRange(new object[] {
            "居中",
            "靠左",
            "靠右"});
            this.boxWay.Location = new System.Drawing.Point(19, 251);
            this.boxWay.Name = "boxWay";
            this.boxWay.Size = new System.Drawing.Size(156, 20);
            this.boxWay.TabIndex = 30;
            this.boxWay.Text = "居中";
            this.boxWay.SelectedIndexChanged += new System.EventHandler(this.boxWay_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(14, 240);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(189, 1);
            this.label8.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 221);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 28;
            this.label9.Text = "通道行进模式";
            // 
            // rioCX
            // 
            this.rioCX.Appearance = System.Windows.Forms.Appearance.Button;
            this.rioCX.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rioCX.Image = global::SmartCar.Properties.Resources.undo_48px_1170302_easyicon_net;
            this.rioCX.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rioCX.Location = new System.Drawing.Point(130, 400);
            this.rioCX.Name = "rioCX";
            this.rioCX.Size = new System.Drawing.Size(51, 66);
            this.rioCX.TabIndex = 27;
            this.rioCX.TabStop = true;
            this.rioCX.Text = "撤销";
            this.rioCX.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rioCX.UseVisualStyleBackColor = false;
            // 
            // rioXZ
            // 
            this.rioXZ.Appearance = System.Windows.Forms.Appearance.Button;
            this.rioXZ.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rioXZ.Image = global::SmartCar.Properties.Resources.cursor_48px_1170259_easyicon_net;
            this.rioXZ.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rioXZ.Location = new System.Drawing.Point(130, 328);
            this.rioXZ.Name = "rioXZ";
            this.rioXZ.Size = new System.Drawing.Size(51, 66);
            this.rioXZ.TabIndex = 26;
            this.rioXZ.TabStop = true;
            this.rioXZ.Text = "选择";
            this.rioXZ.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rioXZ.UseVisualStyleBackColor = false;
            // 
            // rioSLJ
            // 
            this.rioSLJ.Appearance = System.Windows.Forms.Appearance.Button;
            this.rioSLJ.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rioSLJ.Image = global::SmartCar.Properties.Resources.arrows_switch_vertical_29_88679245283px_1182588_easyicon_net;
            this.rioSLJ.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rioSLJ.Location = new System.Drawing.Point(73, 400);
            this.rioSLJ.Name = "rioSLJ";
            this.rioSLJ.Size = new System.Drawing.Size(51, 66);
            this.rioSLJ.TabIndex = 25;
            this.rioSLJ.TabStop = true;
            this.rioSLJ.Text = "双路径";
            this.rioSLJ.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rioSLJ.UseVisualStyleBackColor = false;
            this.rioSLJ.Click += new System.EventHandler(this.rioSLJ_Click);
            // 
            // rioDLJ
            // 
            this.rioDLJ.Appearance = System.Windows.Forms.Appearance.Button;
            this.rioDLJ.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rioDLJ.Image = global::SmartCar.Properties.Resources.arrow_29_828571428571px_1199165_easyicon_net;
            this.rioDLJ.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rioDLJ.Location = new System.Drawing.Point(73, 328);
            this.rioDLJ.Name = "rioDLJ";
            this.rioDLJ.Size = new System.Drawing.Size(51, 66);
            this.rioDLJ.TabIndex = 24;
            this.rioDLJ.TabStop = true;
            this.rioDLJ.Text = "单路径";
            this.rioDLJ.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rioDLJ.UseVisualStyleBackColor = false;
            this.rioDLJ.Click += new System.EventHandler(this.rioDLJ_Click);
            // 
            // rioLD
            // 
            this.rioLD.Appearance = System.Windows.Forms.Appearance.Button;
            this.rioLD.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rioLD.Image = global::SmartCar.Properties.Resources.circle_slelected_48px_1143358_easyicon_net;
            this.rioLD.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rioLD.Location = new System.Drawing.Point(16, 400);
            this.rioLD.Name = "rioLD";
            this.rioLD.Size = new System.Drawing.Size(51, 66);
            this.rioLD.TabIndex = 23;
            this.rioLD.TabStop = true;
            this.rioLD.Text = "雷达点";
            this.rioLD.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rioLD.UseVisualStyleBackColor = false;
            this.rioLD.Click += new System.EventHandler(this.rioLD_Click);
            // 
            // rioPT
            // 
            this.rioPT.Appearance = System.Windows.Forms.Appearance.Button;
            this.rioPT.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rioPT.Image = global::SmartCar.Properties.Resources.circle_empty_48px_1143357_easyicon_net;
            this.rioPT.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rioPT.Location = new System.Drawing.Point(16, 328);
            this.rioPT.Name = "rioPT";
            this.rioPT.Size = new System.Drawing.Size(51, 66);
            this.rioPT.TabIndex = 22;
            this.rioPT.TabStop = true;
            this.rioPT.Text = "普通点";
            this.rioPT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rioPT.UseVisualStyleBackColor = false;
            this.rioPT.Click += new System.EventHandler(this.rioPT_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(99, 159);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 21);
            this.btnDel.TabIndex = 20;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Location = new System.Drawing.Point(99, 186);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(75, 21);
            this.btnSaveAs.TabIndex = 19;
            this.btnSaveAs.Text = "另存";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(14, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(189, 1);
            this.label4.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "路径信息";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 186);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 21);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // boxModel
            // 
            this.boxModel.FormattingEnabled = true;
            this.boxModel.Items.AddRange(new object[] {
            "生成新的路径",
            "编辑已有路径"});
            this.boxModel.Location = new System.Drawing.Point(19, 43);
            this.boxModel.Name = "boxModel";
            this.boxModel.Size = new System.Drawing.Size(156, 20);
            this.boxModel.TabIndex = 10;
            this.boxModel.SelectedIndexChanged += new System.EventHandler(this.boxModel_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(14, 311);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(189, 1);
            this.label5.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 292);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "路径编辑";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.txtName);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(16, 105);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(158, 48);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "路径名称";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(3, 20);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(153, 21);
            this.txtName.TabIndex = 2;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(16, 159);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 21);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(14, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 1);
            this.label2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前模式";
            // 
            // picGenMap
            // 
            this.picGenMap.Location = new System.Drawing.Point(0, 0);
            this.picGenMap.Margin = new System.Windows.Forms.Padding(0);
            this.picGenMap.Name = "picGenMap";
            this.picGenMap.Size = new System.Drawing.Size(555, 506);
            this.picGenMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picGenMap.TabIndex = 0;
            this.picGenMap.TabStop = false;
            // 
            // Form_Path
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 508);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Path";
            this.Text = "Form_Path";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGenMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox boxModel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.RadioButton rioXZ;
        private System.Windows.Forms.RadioButton rioSLJ;
        private System.Windows.Forms.RadioButton rioDLJ;
        private System.Windows.Forms.RadioButton rioLD;
        private System.Windows.Forms.RadioButton rioPT;
        private System.Windows.Forms.RadioButton rioCX;
        private System.Windows.Forms.PictureBox picGenMap;
        private System.Windows.Forms.ComboBox boxWay;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}