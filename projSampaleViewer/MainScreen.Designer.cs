namespace projSampaleViewer
{
    partial class MainScreen
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.MainTab = new System.Windows.Forms.TabPage();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.FrontTab = new System.Windows.Forms.TabPage();
            this.SideTab = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblCoor = new System.Windows.Forms.Label();
            this.chckZoom = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkFilterActive = new System.Windows.Forms.CheckBox();
            this.txtAreaFilterScore = new System.Windows.Forms.TextBox();
            this.txtAreaFilterHeight = new System.Windows.Forms.TextBox();
            this.txtAreaFilterWidth = new System.Windows.Forms.TextBox();
            this.txtRatioImage2ROI = new System.Windows.Forms.TextBox();
            this.lblTest6 = new System.Windows.Forms.Label();
            this.lblTest5 = new System.Windows.Forms.Label();
            this.lblTest4 = new System.Windows.Forms.Label();
            this.lblTest3 = new System.Windows.Forms.Label();
            this.lblTest2 = new System.Windows.Forms.Label();
            this.lblRunMode = new System.Windows.Forms.Label();
            this.lststdResults = new System.Windows.Forms.ListBox();
            this.chkUsePPevaluationROI = new System.Windows.Forms.CheckBox();
            this.txtPProiHeight = new System.Windows.Forms.TextBox();
            this.txtPProiWidth = new System.Windows.Forms.TextBox();
            this.txtPProiPosY = new System.Windows.Forms.TextBox();
            this.txtPProiPosX = new System.Windows.Forms.TextBox();
            this.txtROIangle = new System.Windows.Forms.TextBox();
            this.lblProcessedSize = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.txtThresholdsUpper = new System.Windows.Forms.TextBox();
            this.txtThresholdsLower = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRunTimeWSName = new System.Windows.Forms.TextBox();
            this.cmbImageName = new System.Windows.Forms.ComboBox();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.txtModelsPath = new System.Windows.Forms.TextBox();
            this.lstModels = new System.Windows.Forms.ListBox();
            this.btnPosition = new System.Windows.Forms.Button();
            this.btnDefect = new System.Windows.Forms.Button();
            this.ROITab = new System.Windows.Forms.TabPage();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.tabControl1.SuspendLayout();
            this.MainTab.SuspendLayout();
            this.SideTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.MainTab);
            this.tabControl1.Controls.Add(this.FrontTab);
            this.tabControl1.Controls.Add(this.SideTab);
            this.tabControl1.Controls.Add(this.ROITab);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1486, 1050);
            this.tabControl1.TabIndex = 0;
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.txtVersion);
            this.MainTab.Font = new System.Drawing.Font("Aharoni", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.MainTab.Location = new System.Drawing.Point(4, 22);
            this.MainTab.Name = "MainTab";
            this.MainTab.Padding = new System.Windows.Forms.Padding(3);
            this.MainTab.Size = new System.Drawing.Size(1478, 1024);
            this.MainTab.TabIndex = 0;
            this.MainTab.Text = "Main";
            this.MainTab.UseVisualStyleBackColor = true;
            // 
            // txtVersion
            // 
            this.txtVersion.Font = new System.Drawing.Font("Aharoni", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtVersion.Location = new System.Drawing.Point(34, 355);
            this.txtVersion.Multiline = true;
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(434, 106);
            this.txtVersion.TabIndex = 0;
            // 
            // FrontTab
            // 
            this.FrontTab.Font = new System.Drawing.Font("Aharoni", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FrontTab.Location = new System.Drawing.Point(4, 22);
            this.FrontTab.Name = "FrontTab";
            this.FrontTab.Padding = new System.Windows.Forms.Padding(3);
            this.FrontTab.Size = new System.Drawing.Size(1478, 1024);
            this.FrontTab.TabIndex = 1;
            this.FrontTab.Text = "Front";
            this.FrontTab.UseVisualStyleBackColor = true;
            // 
            // SideTab
            // 
            this.SideTab.Controls.Add(this.pictureBox2);
            this.SideTab.Controls.Add(this.trackBar1);
            this.SideTab.Controls.Add(this.panel1);
            this.SideTab.Controls.Add(this.chckZoom);
            this.SideTab.Controls.Add(this.button1);
            this.SideTab.Controls.Add(this.btnCheck);
            this.SideTab.Controls.Add(this.label14);
            this.SideTab.Controls.Add(this.label13);
            this.SideTab.Controls.Add(this.label12);
            this.SideTab.Controls.Add(this.label11);
            this.SideTab.Controls.Add(this.label10);
            this.SideTab.Controls.Add(this.label9);
            this.SideTab.Controls.Add(this.label8);
            this.SideTab.Controls.Add(this.label7);
            this.SideTab.Controls.Add(this.label6);
            this.SideTab.Controls.Add(this.elementHost1);
            this.SideTab.Controls.Add(this.chkFilterActive);
            this.SideTab.Controls.Add(this.txtAreaFilterScore);
            this.SideTab.Controls.Add(this.txtAreaFilterHeight);
            this.SideTab.Controls.Add(this.txtAreaFilterWidth);
            this.SideTab.Controls.Add(this.txtRatioImage2ROI);
            this.SideTab.Controls.Add(this.lblTest6);
            this.SideTab.Controls.Add(this.lblTest5);
            this.SideTab.Controls.Add(this.lblTest4);
            this.SideTab.Controls.Add(this.lblTest3);
            this.SideTab.Controls.Add(this.lblTest2);
            this.SideTab.Controls.Add(this.lblRunMode);
            this.SideTab.Controls.Add(this.lststdResults);
            this.SideTab.Controls.Add(this.chkUsePPevaluationROI);
            this.SideTab.Controls.Add(this.txtPProiHeight);
            this.SideTab.Controls.Add(this.txtPProiWidth);
            this.SideTab.Controls.Add(this.txtPProiPosY);
            this.SideTab.Controls.Add(this.txtPProiPosX);
            this.SideTab.Controls.Add(this.txtROIangle);
            this.SideTab.Controls.Add(this.lblProcessedSize);
            this.SideTab.Controls.Add(this.lblDuration);
            this.SideTab.Controls.Add(this.txtThresholdsUpper);
            this.SideTab.Controls.Add(this.txtThresholdsLower);
            this.SideTab.Controls.Add(this.label5);
            this.SideTab.Controls.Add(this.label4);
            this.SideTab.Controls.Add(this.label3);
            this.SideTab.Controls.Add(this.label2);
            this.SideTab.Controls.Add(this.label1);
            this.SideTab.Controls.Add(this.txtRunTimeWSName);
            this.SideTab.Controls.Add(this.cmbImageName);
            this.SideTab.Controls.Add(this.txtImagePath);
            this.SideTab.Controls.Add(this.txtModelsPath);
            this.SideTab.Controls.Add(this.lstModels);
            this.SideTab.Controls.Add(this.btnPosition);
            this.SideTab.Controls.Add(this.btnDefect);
            this.SideTab.Font = new System.Drawing.Font("Aharoni", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.SideTab.Location = new System.Drawing.Point(4, 22);
            this.SideTab.Name = "SideTab";
            this.SideTab.Padding = new System.Windows.Forms.Padding(3);
            this.SideTab.Size = new System.Drawing.Size(1478, 1024);
            this.SideTab.TabIndex = 2;
            this.SideTab.Text = "Side";
            this.SideTab.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(564, 80);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(276, 242);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 52;
            this.pictureBox2.TabStop = false;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(999, 373);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(243, 45);
            this.trackBar1.TabIndex = 51;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lblCoor);
            this.panel1.Location = new System.Drawing.Point(846, 425);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(563, 447);
            this.panel1.TabIndex = 50;
            this.panel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel1_Scroll);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(21, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(537, 425);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 46;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // lblCoor
            // 
            this.lblCoor.AutoSize = true;
            this.lblCoor.Location = new System.Drawing.Point(3, 431);
            this.lblCoor.Name = "lblCoor";
            this.lblCoor.Size = new System.Drawing.Size(41, 11);
            this.lblCoor.TabIndex = 47;
            this.lblCoor.Text = "label15";
            // 
            // chckZoom
            // 
            this.chckZoom.AutoSize = true;
            this.chckZoom.Location = new System.Drawing.Point(786, 440);
            this.chckZoom.Name = "chckZoom";
            this.chckZoom.Size = new System.Drawing.Size(54, 15);
            this.chckZoom.TabIndex = 49;
            this.chckZoom.Text = "zoom";
            this.chckZoom.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(533, 395);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 48;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.BackColor = System.Drawing.Color.RosyBrown;
            this.btnCheck.Font = new System.Drawing.Font("Aharoni", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnCheck.Location = new System.Drawing.Point(421, 286);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(114, 36);
            this.btnCheck.TabIndex = 45;
            this.btnCheck.Text = "Check Picture";
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 678);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(102, 11);
            this.label14.TabIndex = 44;
            this.label14.Text = "roi to image ratio:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 548);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 11);
            this.label13.TabIndex = 43;
            this.label13.Text = "Pos. X:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 575);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 11);
            this.label12.TabIndex = 42;
            this.label12.Text = "Pos. Y:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 599);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 11);
            this.label11.TabIndex = 41;
            this.label11.Text = "Widht:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 623);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 11);
            this.label10.TabIndex = 40;
            this.label10.Text = "Height:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 647);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 11);
            this.label9.TabIndex = 39;
            this.label9.Text = "angle:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(172, 504);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 11);
            this.label8.TabIndex = 38;
            this.label8.Text = "Score:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 504);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 11);
            this.label7.TabIndex = 37;
            this.label7.Text = "Area:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 251);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 11);
            this.label6.TabIndex = 36;
            this.label6.Text = "Models Path:";
            // 
            // chkFilterActive
            // 
            this.chkFilterActive.AutoSize = true;
            this.chkFilterActive.Location = new System.Drawing.Point(216, 472);
            this.chkFilterActive.Name = "chkFilterActive";
            this.chkFilterActive.Size = new System.Drawing.Size(85, 15);
            this.chkFilterActive.TabIndex = 34;
            this.chkFilterActive.Text = "Filter Active";
            this.chkFilterActive.UseVisualStyleBackColor = true;
            // 
            // txtAreaFilterScore
            // 
            this.txtAreaFilterScore.Location = new System.Drawing.Point(216, 501);
            this.txtAreaFilterScore.Name = "txtAreaFilterScore";
            this.txtAreaFilterScore.Size = new System.Drawing.Size(47, 18);
            this.txtAreaFilterScore.TabIndex = 33;
            // 
            // txtAreaFilterHeight
            // 
            this.txtAreaFilterHeight.Location = new System.Drawing.Point(114, 501);
            this.txtAreaFilterHeight.Name = "txtAreaFilterHeight";
            this.txtAreaFilterHeight.Size = new System.Drawing.Size(47, 18);
            this.txtAreaFilterHeight.TabIndex = 32;
            // 
            // txtAreaFilterWidth
            // 
            this.txtAreaFilterWidth.Location = new System.Drawing.Point(55, 501);
            this.txtAreaFilterWidth.Name = "txtAreaFilterWidth";
            this.txtAreaFilterWidth.Size = new System.Drawing.Size(47, 18);
            this.txtAreaFilterWidth.TabIndex = 31;
            // 
            // txtRatioImage2ROI
            // 
            this.txtRatioImage2ROI.Location = new System.Drawing.Point(119, 675);
            this.txtRatioImage2ROI.Name = "txtRatioImage2ROI";
            this.txtRatioImage2ROI.Size = new System.Drawing.Size(54, 18);
            this.txtRatioImage2ROI.TabIndex = 30;
            // 
            // lblTest6
            // 
            this.lblTest6.AutoSize = true;
            this.lblTest6.Location = new System.Drawing.Point(16, 120);
            this.lblTest6.Name = "lblTest6";
            this.lblTest6.Size = new System.Drawing.Size(36, 11);
            this.lblTest6.TabIndex = 29;
            this.lblTest6.Text = "label6";
            // 
            // lblTest5
            // 
            this.lblTest5.AutoSize = true;
            this.lblTest5.Location = new System.Drawing.Point(16, 96);
            this.lblTest5.Name = "lblTest5";
            this.lblTest5.Size = new System.Drawing.Size(36, 11);
            this.lblTest5.TabIndex = 28;
            this.lblTest5.Text = "label6";
            // 
            // lblTest4
            // 
            this.lblTest4.AutoSize = true;
            this.lblTest4.Location = new System.Drawing.Point(16, 63);
            this.lblTest4.Name = "lblTest4";
            this.lblTest4.Size = new System.Drawing.Size(36, 11);
            this.lblTest4.TabIndex = 27;
            this.lblTest4.Text = "label6";
            // 
            // lblTest3
            // 
            this.lblTest3.AutoSize = true;
            this.lblTest3.Location = new System.Drawing.Point(16, 37);
            this.lblTest3.Name = "lblTest3";
            this.lblTest3.Size = new System.Drawing.Size(36, 11);
            this.lblTest3.TabIndex = 26;
            this.lblTest3.Text = "label6";
            // 
            // lblTest2
            // 
            this.lblTest2.AutoSize = true;
            this.lblTest2.Location = new System.Drawing.Point(16, 16);
            this.lblTest2.Name = "lblTest2";
            this.lblTest2.Size = new System.Drawing.Size(36, 11);
            this.lblTest2.TabIndex = 25;
            this.lblTest2.Text = "label6";
            // 
            // lblRunMode
            // 
            this.lblRunMode.AutoSize = true;
            this.lblRunMode.Location = new System.Drawing.Point(11, 710);
            this.lblRunMode.Name = "lblRunMode";
            this.lblRunMode.Size = new System.Drawing.Size(58, 11);
            this.lblRunMode.TabIndex = 24;
            this.lblRunMode.Text = "Run Mode";
            // 
            // lststdResults
            // 
            this.lststdResults.FormattingEnabled = true;
            this.lststdResults.ItemHeight = 11;
            this.lststdResults.Location = new System.Drawing.Point(851, 878);
            this.lststdResults.Name = "lststdResults";
            this.lststdResults.Size = new System.Drawing.Size(558, 136);
            this.lststdResults.TabIndex = 23;
            // 
            // chkUsePPevaluationROI
            // 
            this.chkUsePPevaluationROI.AutoSize = true;
            this.chkUsePPevaluationROI.Location = new System.Drawing.Point(18, 472);
            this.chkUsePPevaluationROI.Name = "chkUsePPevaluationROI";
            this.chkUsePPevaluationROI.Size = new System.Drawing.Size(155, 15);
            this.chkUsePPevaluationROI.TabIndex = 22;
            this.chkUsePPevaluationROI.Text = "chk Use  Pevaluation ROI";
            this.chkUsePPevaluationROI.UseVisualStyleBackColor = true;
            // 
            // txtPProiHeight
            // 
            this.txtPProiHeight.Location = new System.Drawing.Point(55, 620);
            this.txtPProiHeight.Name = "txtPProiHeight";
            this.txtPProiHeight.Size = new System.Drawing.Size(47, 18);
            this.txtPProiHeight.TabIndex = 21;
            // 
            // txtPProiWidth
            // 
            this.txtPProiWidth.Location = new System.Drawing.Point(54, 596);
            this.txtPProiWidth.Name = "txtPProiWidth";
            this.txtPProiWidth.Size = new System.Drawing.Size(47, 18);
            this.txtPProiWidth.TabIndex = 20;
            // 
            // txtPProiPosY
            // 
            this.txtPProiPosY.Location = new System.Drawing.Point(55, 572);
            this.txtPProiPosY.Name = "txtPProiPosY";
            this.txtPProiPosY.Size = new System.Drawing.Size(47, 18);
            this.txtPProiPosY.TabIndex = 19;
            // 
            // txtPProiPosX
            // 
            this.txtPProiPosX.Location = new System.Drawing.Point(55, 545);
            this.txtPProiPosX.Name = "txtPProiPosX";
            this.txtPProiPosX.Size = new System.Drawing.Size(47, 18);
            this.txtPProiPosX.TabIndex = 18;
            // 
            // txtROIangle
            // 
            this.txtROIangle.Location = new System.Drawing.Point(54, 644);
            this.txtROIangle.Name = "txtROIangle";
            this.txtROIangle.Size = new System.Drawing.Size(47, 18);
            this.txtROIangle.TabIndex = 17;
            // 
            // lblProcessedSize
            // 
            this.lblProcessedSize.AutoSize = true;
            this.lblProcessedSize.Location = new System.Drawing.Point(77, 119);
            this.lblProcessedSize.Name = "lblProcessedSize";
            this.lblProcessedSize.Size = new System.Drawing.Size(82, 11);
            this.lblProcessedSize.TabIndex = 16;
            this.lblProcessedSize.Text = "Processed Size";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(77, 94);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(52, 11);
            this.lblDuration.TabIndex = 15;
            this.lblDuration.Text = "Duration";
            // 
            // txtThresholdsUpper
            // 
            this.txtThresholdsUpper.Location = new System.Drawing.Point(163, 48);
            this.txtThresholdsUpper.Name = "txtThresholdsUpper";
            this.txtThresholdsUpper.Size = new System.Drawing.Size(100, 18);
            this.txtThresholdsUpper.TabIndex = 14;
            // 
            // txtThresholdsLower
            // 
            this.txtThresholdsLower.Location = new System.Drawing.Point(163, 13);
            this.txtThresholdsLower.Name = "txtThresholdsLower";
            this.txtThresholdsLower.Size = new System.Drawing.Size(100, 18);
            this.txtThresholdsLower.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 11);
            this.label5.TabIndex = 12;
            this.label5.Text = "High Threshold";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(77, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 11);
            this.label4.TabIndex = 11;
            this.label4.Text = "Low Threshold";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 11);
            this.label3.TabIndex = 10;
            this.label3.Text = "Image Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 11);
            this.label2.TabIndex = 9;
            this.label2.Text = "Image Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 11);
            this.label1.TabIndex = 8;
            this.label1.Text = "Riuntime Workspace Name:";
            // 
            // txtRunTimeWSName
            // 
            this.txtRunTimeWSName.Location = new System.Drawing.Point(174, 157);
            this.txtRunTimeWSName.Name = "txtRunTimeWSName";
            this.txtRunTimeWSName.Size = new System.Drawing.Size(205, 18);
            this.txtRunTimeWSName.TabIndex = 7;
            this.txtRunTimeWSName.Text = "Proj_021_201223_104500_21122023_104445";
            // 
            // cmbImageName
            // 
            this.cmbImageName.FormattingEnabled = true;
            this.cmbImageName.Location = new System.Drawing.Point(174, 215);
            this.cmbImageName.Name = "cmbImageName";
            this.cmbImageName.Size = new System.Drawing.Size(187, 19);
            this.cmbImageName.TabIndex = 6;
            // 
            // txtImagePath
            // 
            this.txtImagePath.Location = new System.Drawing.Point(174, 191);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(205, 18);
            this.txtImagePath.TabIndex = 5;
            this.txtImagePath.TextChanged += new System.EventHandler(this.txtImagePath_TextChanged);
            // 
            // txtModelsPath
            // 
            this.txtModelsPath.Location = new System.Drawing.Point(174, 248);
            this.txtModelsPath.Name = "txtModelsPath";
            this.txtModelsPath.Size = new System.Drawing.Size(241, 18);
            this.txtModelsPath.TabIndex = 4;
            this.txtModelsPath.Text = @"C:\Users\inspmachha\Desktop\final models";
            // 
            // lstModels
            // 
            this.lstModels.FormattingEnabled = true;
            this.lstModels.ItemHeight = 11;
            this.lstModels.Location = new System.Drawing.Point(18, 286);
            this.lstModels.Name = "lstModels";
            this.lstModels.Size = new System.Drawing.Size(397, 158);
            this.lstModels.TabIndex = 3;
            // 
            // btnPosition
            // 
            this.btnPosition.BackColor = System.Drawing.Color.RosyBrown;
            this.btnPosition.Font = new System.Drawing.Font("Aharoni", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnPosition.Location = new System.Drawing.Point(348, 86);
            this.btnPosition.Name = "btnPosition";
            this.btnPosition.Size = new System.Drawing.Size(187, 31);
            this.btnPosition.TabIndex = 2;
            this.btnPosition.Text = "Fix Position";
            this.btnPosition.UseVisualStyleBackColor = false;
            // 
            // btnDefect
            // 
            this.btnDefect.BackColor = System.Drawing.Color.RosyBrown;
            this.btnDefect.Font = new System.Drawing.Font("Aharoni", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnDefect.Location = new System.Drawing.Point(348, 34);
            this.btnDefect.Name = "btnDefect";
            this.btnDefect.Size = new System.Drawing.Size(187, 31);
            this.btnDefect.TabIndex = 1;
            this.btnDefect.Text = "Search Defects";
            this.btnDefect.UseVisualStyleBackColor = false;
            this.btnDefect.Click += new System.EventHandler(this.btnDefect_Click);
            // 
            // ROITab
            // 
            this.ROITab.Font = new System.Drawing.Font("Aharoni", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ROITab.Location = new System.Drawing.Point(4, 22);
            this.ROITab.Name = "ROITab";
            this.ROITab.Padding = new System.Windows.Forms.Padding(3);
            this.ROITab.Size = new System.Drawing.Size(1478, 1024);
            this.ROITab.TabIndex = 3;
            this.ROITab.Text = "ROI";
            this.ROITab.UseVisualStyleBackColor = true;
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(867, 6);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(537, 326);
            this.elementHost1.TabIndex = 35;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1510, 1061);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainScreen";
            this.Text = "MainScreen";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainScreen_FormClosed);
            this.Load += new System.EventHandler(this.txtModelsPath_Load);
            this.tabControl1.ResumeLayout(false);
            this.MainTab.ResumeLayout(false);
            this.MainTab.PerformLayout();
            this.SideTab.ResumeLayout(false);
            this.SideTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage MainTab;
        private System.Windows.Forms.TabPage FrontTab;
        private System.Windows.Forms.TabPage SideTab;
        private System.Windows.Forms.TabPage ROITab;
        private System.Windows.Forms.Button btnPosition;
        private System.Windows.Forms.Button btnDefect;
        private System.Windows.Forms.ListBox lstModels;
        private System.Windows.Forms.TextBox txtModelsPath;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.ComboBox cmbImageName;
        private System.Windows.Forms.TextBox txtRunTimeWSName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtThresholdsUpper;
        private System.Windows.Forms.TextBox txtThresholdsLower;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblProcessedSize;
        private System.Windows.Forms.TextBox txtPProiHeight;
        private System.Windows.Forms.TextBox txtPProiWidth;
        private System.Windows.Forms.TextBox txtPProiPosY;
        private System.Windows.Forms.TextBox txtPProiPosX;
        private System.Windows.Forms.TextBox txtROIangle;
        private System.Windows.Forms.CheckBox chkUsePPevaluationROI;
        private System.Windows.Forms.ListBox lststdResults;
        private System.Windows.Forms.Label lblTest6;
        private System.Windows.Forms.Label lblTest5;
        private System.Windows.Forms.Label lblTest4;
        private System.Windows.Forms.Label lblTest3;
        private System.Windows.Forms.Label lblTest2;
        private System.Windows.Forms.TextBox txtRatioImage2ROI;
        private System.Windows.Forms.TextBox txtAreaFilterWidth;
        private System.Windows.Forms.TextBox txtAreaFilterScore;
        private System.Windows.Forms.TextBox txtAreaFilterHeight;
        private System.Windows.Forms.CheckBox chkFilterActive;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblRunMode;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblCoor;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chckZoom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
    }
}