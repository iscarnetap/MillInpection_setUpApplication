namespace projSampaleViewer
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.btnLoadModel = new System.Windows.Forms.Button();
            this.btnDoEvaluation = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRunTimeWSName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtToolName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtToolType = new System.Windows.Forms.TextBox();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showUnshowMarkingTSMI01 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageWithMarkingTSMI1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnViewResults = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblProcessedSize = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblClassName = new System.Windows.Forms.Label();
            this.cmbImageName = new System.Windows.Forms.ComboBox();
            this.txtModelsPath = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lstModels = new System.Windows.Forms.ListBox();
            this.btnIMGcomboRefresh = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateCurrentModelThresholdsTSMI2 = new System.Windows.Forms.ToolStripMenuItem();
            this.txtThresholdsLower = new System.Windows.Forms.TextBox();
            this.txtThresholdsUpper = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtLastModified = new System.Windows.Forms.TextBox();
            this.grbStaticROI = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyRoiToClipboardTSMI3 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTest1 = new System.Windows.Forms.Button();
            this.lblTest6 = new System.Windows.Forms.Label();
            this.lblTest5 = new System.Windows.Forms.Label();
            this.lblTest4 = new System.Windows.Forms.Label();
            this.lblTest3 = new System.Windows.Forms.Label();
            this.lblTest2 = new System.Windows.Forms.Label();
            this.lblTest1 = new System.Windows.Forms.Label();
            this.txtRatioImage2ROI = new System.Windows.Forms.TextBox();
            this.btnGetRecInfo = new System.Windows.Forms.Button();
            this.btnEditROI = new System.Windows.Forms.Button();
            this.btnClearROI = new System.Windows.Forms.Button();
            this.label147 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.btnApplyROI = new System.Windows.Forms.Button();
            this.btnAddROI = new System.Windows.Forms.Button();
            this.label43 = new System.Windows.Forms.Label();
            this.chkUsePPevaluationROI = new System.Windows.Forms.CheckBox();
            this.label42 = new System.Windows.Forms.Label();
            this.txtPProiPosX = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtPProiWidth = new System.Windows.Forms.TextBox();
            this.txtPProiPosY = new System.Windows.Forms.TextBox();
            this.txtROIangle = new System.Windows.Forms.TextBox();
            this.txtPProiHeight = new System.Windows.Forms.TextBox();
            this.btnRestZoom = new System.Windows.Forms.Button();
            this.pbxEditRoi = new System.Windows.Forms.PictureBox();
            this.lststdResults = new System.Windows.Forms.ListBox();
            this.imgLstIndex = new System.Windows.Forms.Label();
            this.lblRunMode = new System.Windows.Forms.Label();
            this.btnROIapplyBatch = new System.Windows.Forms.Button();
            this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetROIBatchFileTSMI4 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.grbStaticROI.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxEditRoi)).BeginInit();
            this.contextMenuStrip4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadModel
            // 
            this.btnLoadModel.BackColor = System.Drawing.Color.Chartreuse;
            this.btnLoadModel.Location = new System.Drawing.Point(26, 37);
            this.btnLoadModel.Name = "btnLoadModel";
            this.btnLoadModel.Size = new System.Drawing.Size(125, 23);
            this.btnLoadModel.TabIndex = 0;
            this.btnLoadModel.Text = "LOAD MODELS";
            this.btnLoadModel.UseVisualStyleBackColor = false;
            this.btnLoadModel.Click += new System.EventHandler(this.btnLoadModel_Click);
            // 
            // btnDoEvaluation
            // 
            this.btnDoEvaluation.BackColor = System.Drawing.Color.Goldenrod;
            this.btnDoEvaluation.Location = new System.Drawing.Point(26, 78);
            this.btnDoEvaluation.Name = "btnDoEvaluation";
            this.btnDoEvaluation.Size = new System.Drawing.Size(125, 31);
            this.btnDoEvaluation.TabIndex = 1;
            this.btnDoEvaluation.Text = "DO EVALUATION";
            this.btnDoEvaluation.UseVisualStyleBackColor = false;
            this.btnDoEvaluation.Click += new System.EventHandler(this.btnDoEvaluation_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(30, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Riuntime Workspace Name:";
            // 
            // txtRunTimeWSName
            // 
            this.txtRunTimeWSName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtRunTimeWSName.Location = new System.Drawing.Point(251, 145);
            this.txtRunTimeWSName.Name = "txtRunTimeWSName";
            this.txtRunTimeWSName.Size = new System.Drawing.Size(276, 22);
            this.txtRunTimeWSName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(30, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tool Name:";
            // 
            // txtToolName
            // 
            this.txtToolName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtToolName.Location = new System.Drawing.Point(251, 173);
            this.txtToolName.Name = "txtToolName";
            this.txtToolName.Size = new System.Drawing.Size(276, 22);
            this.txtToolName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Info;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(30, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tool Type:";
            // 
            // txtToolType
            // 
            this.txtToolType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtToolType.Location = new System.Drawing.Point(251, 201);
            this.txtToolType.Name = "txtToolType";
            this.txtToolType.Size = new System.Drawing.Size(276, 22);
            this.txtToolType.TabIndex = 3;
            // 
            // elementHost1
            // 
            this.elementHost1.ContextMenuStrip = this.contextMenuStrip1;
            this.elementHost1.Location = new System.Drawing.Point(533, 12);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(1186, 840);
            this.elementHost1.TabIndex = 5;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showUnshowMarkingTSMI01,
            this.saveImageWithMarkingTSMI1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(210, 48);
            // 
            // showUnshowMarkingTSMI01
            // 
            this.showUnshowMarkingTSMI01.Name = "showUnshowMarkingTSMI01";
            this.showUnshowMarkingTSMI01.Size = new System.Drawing.Size(209, 22);
            this.showUnshowMarkingTSMI01.Text = "Show/Unshow Marking";
            this.showUnshowMarkingTSMI01.Click += new System.EventHandler(this.showUnshowMarkingTSMI01_Click);
            // 
            // saveImageWithMarkingTSMI1
            // 
            this.saveImageWithMarkingTSMI1.Name = "saveImageWithMarkingTSMI1";
            this.saveImageWithMarkingTSMI1.Size = new System.Drawing.Size(209, 22);
            this.saveImageWithMarkingTSMI1.Text = "Save Image With Marking";
            this.saveImageWithMarkingTSMI1.Click += new System.EventHandler(this.saveImageWithMarkingTSMI1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnViewResults);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblProcessedSize);
            this.groupBox1.Controls.Add(this.lblDuration);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label41);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox1.Location = new System.Drawing.Point(26, 632);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 322);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Results";
            // 
            // btnViewResults
            // 
            this.btnViewResults.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnViewResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewResults.Location = new System.Drawing.Point(6, 291);
            this.btnViewResults.Name = "btnViewResults";
            this.btnViewResults.Size = new System.Drawing.Size(131, 23);
            this.btnViewResults.TabIndex = 67;
            this.btnViewResults.Text = "VIEW RESULTS";
            this.btnViewResults.UseVisualStyleBackColor = false;
            this.btnViewResults.Click += new System.EventHandler(this.btnViewResults_Click);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Lime;
            this.label10.Location = new System.Drawing.Point(6, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 18);
            this.label10.TabIndex = 66;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(6, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 18);
            this.label7.TabIndex = 66;
            // 
            // lblProcessedSize
            // 
            this.lblProcessedSize.AutoSize = true;
            this.lblProcessedSize.BackColor = System.Drawing.Color.White;
            this.lblProcessedSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessedSize.Location = new System.Drawing.Point(126, 66);
            this.lblProcessedSize.Name = "lblProcessedSize";
            this.lblProcessedSize.Size = new System.Drawing.Size(38, 16);
            this.lblProcessedSize.TabIndex = 65;
            this.lblProcessedSize.Text = "xxxxx";
            this.lblProcessedSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.BackColor = System.Drawing.Color.White;
            this.lblDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuration.Location = new System.Drawing.Point(126, 34);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(38, 16);
            this.lblDuration.TabIndex = 65;
            this.lblDuration.Text = "xxxxx";
            this.lblDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(64, 131);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(178, 16);
            this.label9.TabIndex = 29;
            this.label9.Text = "Good Insert, no defects found";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(64, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(152, 16);
            this.label8.TabIndex = 29;
            this.label8.Text = "Bad Insert, defects found";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(180, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 15);
            this.label4.TabIndex = 65;
            this.label4.Text = "seconds";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.SteelBlue;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 16);
            this.label5.TabIndex = 65;
            this.label5.Text = "Processed Size:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.BackColor = System.Drawing.Color.SteelBlue;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(6, 34);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(114, 16);
            this.label41.TabIndex = 65;
            this.label41.Text = "Process Duration:";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtImagePath
            // 
            this.txtImagePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImagePath.Location = new System.Drawing.Point(130, 336);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(384, 21);
            this.txtImagePath.TabIndex = 30;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(26, 338);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(87, 17);
            this.label46.TabIndex = 29;
            this.label46.Text = "Image Path:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(25, 369);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 17);
            this.label6.TabIndex = 29;
            this.label6.Text = "Image Name:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(28, 265);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 16);
            this.label11.TabIndex = 2;
            this.label11.Text = "Class Name:";
            // 
            // lblClassName
            // 
            this.lblClassName.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.lblClassName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClassName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClassName.Location = new System.Drawing.Point(251, 265);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(96, 23);
            this.lblClassName.TabIndex = 2;
            this.lblClassName.Text = "xxxx";
            // 
            // cmbImageName
            // 
            this.cmbImageName.FormattingEnabled = true;
            this.cmbImageName.Location = new System.Drawing.Point(130, 369);
            this.cmbImageName.Name = "cmbImageName";
            this.cmbImageName.Size = new System.Drawing.Size(214, 21);
            this.cmbImageName.TabIndex = 31;
            this.cmbImageName.SelectedIndexChanged += new System.EventHandler(this.cmbImageName_SelectedIndexChanged);
            // 
            // txtModelsPath
            // 
            this.txtModelsPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModelsPath.Location = new System.Drawing.Point(130, 406);
            this.txtModelsPath.Name = "txtModelsPath";
            this.txtModelsPath.Size = new System.Drawing.Size(384, 21);
            this.txtModelsPath.TabIndex = 30;
            this.txtModelsPath.Text = "D:\\EndMill Projrct\\Final Brains_test_180923";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(25, 409);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 17);
            this.label12.TabIndex = 29;
            this.label12.Text = "Models Path:";
            // 
            // lstModels
            // 
            this.lstModels.FormattingEnabled = true;
            this.lstModels.Location = new System.Drawing.Point(130, 441);
            this.lstModels.Name = "lstModels";
            this.lstModels.Size = new System.Drawing.Size(384, 95);
            this.lstModels.TabIndex = 32;
            this.lstModels.SelectedIndexChanged += new System.EventHandler(this.lstModels_SelectedIndexChanged);
            // 
            // btnIMGcomboRefresh
            // 
            this.btnIMGcomboRefresh.BackColor = System.Drawing.Color.LawnGreen;
            this.btnIMGcomboRefresh.Location = new System.Drawing.Point(397, 368);
            this.btnIMGcomboRefresh.Name = "btnIMGcomboRefresh";
            this.btnIMGcomboRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnIMGcomboRefresh.TabIndex = 33;
            this.btnIMGcomboRefresh.Text = "REFRESH";
            this.btnIMGcomboRefresh.UseVisualStyleBackColor = false;
            this.btnIMGcomboRefresh.Click += new System.EventHandler(this.btnIMGcomboRefresh_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Orange;
            this.label13.ContextMenuStrip = this.contextMenuStrip2;
            this.label13.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(25, 306);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 17);
            this.label13.TabIndex = 29;
            this.label13.Text = "Thrersholds:";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateCurrentModelThresholdsTSMI2});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(254, 26);
            // 
            // updateCurrentModelThresholdsTSMI2
            // 
            this.updateCurrentModelThresholdsTSMI2.Name = "updateCurrentModelThresholdsTSMI2";
            this.updateCurrentModelThresholdsTSMI2.Size = new System.Drawing.Size(253, 22);
            this.updateCurrentModelThresholdsTSMI2.Text = "Update Current Model Thresholds";
            this.updateCurrentModelThresholdsTSMI2.Click += new System.EventHandler(this.updateCurrentModelThresholdsTSMI2_Click);
            // 
            // txtThresholdsLower
            // 
            this.txtThresholdsLower.BackColor = System.Drawing.SystemColors.HotTrack;
            this.txtThresholdsLower.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThresholdsLower.Location = new System.Drawing.Point(129, 306);
            this.txtThresholdsLower.Name = "txtThresholdsLower";
            this.txtThresholdsLower.Size = new System.Drawing.Size(31, 21);
            this.txtThresholdsLower.TabIndex = 30;
            this.txtThresholdsLower.Text = "0.2";
            // 
            // txtThresholdsUpper
            // 
            this.txtThresholdsUpper.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.txtThresholdsUpper.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThresholdsUpper.Location = new System.Drawing.Point(173, 306);
            this.txtThresholdsUpper.Name = "txtThresholdsUpper";
            this.txtThresholdsUpper.Size = new System.Drawing.Size(31, 21);
            this.txtThresholdsUpper.TabIndex = 30;
            this.txtThresholdsUpper.Text = "1.0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label14.Location = new System.Drawing.Point(32, 232);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(109, 16);
            this.label14.TabIndex = 2;
            this.label14.Text = "Last Modified :";
            // 
            // txtLastModified
            // 
            this.txtLastModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtLastModified.Location = new System.Drawing.Point(251, 232);
            this.txtLastModified.Name = "txtLastModified";
            this.txtLastModified.Size = new System.Drawing.Size(276, 22);
            this.txtLastModified.TabIndex = 3;
            // 
            // grbStaticROI
            // 
            this.grbStaticROI.ContextMenuStrip = this.contextMenuStrip3;
            this.grbStaticROI.Controls.Add(this.btnROIapplyBatch);
            this.grbStaticROI.Controls.Add(this.btnTest1);
            this.grbStaticROI.Controls.Add(this.lblTest6);
            this.grbStaticROI.Controls.Add(this.lblTest5);
            this.grbStaticROI.Controls.Add(this.lblTest4);
            this.grbStaticROI.Controls.Add(this.lblTest3);
            this.grbStaticROI.Controls.Add(this.lblTest2);
            this.grbStaticROI.Controls.Add(this.lblTest1);
            this.grbStaticROI.Controls.Add(this.txtRatioImage2ROI);
            this.grbStaticROI.Controls.Add(this.btnGetRecInfo);
            this.grbStaticROI.Controls.Add(this.btnEditROI);
            this.grbStaticROI.Controls.Add(this.btnClearROI);
            this.grbStaticROI.Controls.Add(this.label147);
            this.grbStaticROI.Controls.Add(this.label44);
            this.grbStaticROI.Controls.Add(this.btnApplyROI);
            this.grbStaticROI.Controls.Add(this.btnAddROI);
            this.grbStaticROI.Controls.Add(this.label43);
            this.grbStaticROI.Controls.Add(this.chkUsePPevaluationROI);
            this.grbStaticROI.Controls.Add(this.label42);
            this.grbStaticROI.Controls.Add(this.txtPProiPosX);
            this.grbStaticROI.Controls.Add(this.label45);
            this.grbStaticROI.Controls.Add(this.label15);
            this.grbStaticROI.Controls.Add(this.txtPProiWidth);
            this.grbStaticROI.Controls.Add(this.txtPProiPosY);
            this.grbStaticROI.Controls.Add(this.txtROIangle);
            this.grbStaticROI.Controls.Add(this.txtPProiHeight);
            this.grbStaticROI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.grbStaticROI.Location = new System.Drawing.Point(287, 634);
            this.grbStaticROI.Name = "grbStaticROI";
            this.grbStaticROI.Size = new System.Drawing.Size(227, 320);
            this.grbStaticROI.TabIndex = 65;
            this.grbStaticROI.TabStop = false;
            this.grbStaticROI.Text = "Static ROI";
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyRoiToClipboardTSMI3});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(191, 26);
            // 
            // copyRoiToClipboardTSMI3
            // 
            this.copyRoiToClipboardTSMI3.Name = "copyRoiToClipboardTSMI3";
            this.copyRoiToClipboardTSMI3.Size = new System.Drawing.Size(190, 22);
            this.copyRoiToClipboardTSMI3.Text = "Copy roi To Clipboard";
            this.copyRoiToClipboardTSMI3.Click += new System.EventHandler(this.copyRoiToClipboardTSMI3_Click);
            // 
            // btnTest1
            // 
            this.btnTest1.BackColor = System.Drawing.Color.MediumTurquoise;
            this.btnTest1.Location = new System.Drawing.Point(151, 228);
            this.btnTest1.Name = "btnTest1";
            this.btnTest1.Size = new System.Drawing.Size(50, 23);
            this.btnTest1.TabIndex = 67;
            this.btnTest1.Text = "T1";
            this.btnTest1.UseVisualStyleBackColor = false;
            this.btnTest1.Visible = false;
            this.btnTest1.Click += new System.EventHandler(this.btnTest1_Click);
            // 
            // lblTest6
            // 
            this.lblTest6.AutoSize = true;
            this.lblTest6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTest6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTest6.Location = new System.Drawing.Point(151, 205);
            this.lblTest6.Name = "lblTest6";
            this.lblTest6.Size = new System.Drawing.Size(50, 15);
            this.lblTest6.TabIndex = 66;
            this.lblTest6.Text = "label16";
            this.lblTest6.Visible = false;
            // 
            // lblTest5
            // 
            this.lblTest5.AutoSize = true;
            this.lblTest5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTest5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTest5.Location = new System.Drawing.Point(151, 185);
            this.lblTest5.Name = "lblTest5";
            this.lblTest5.Size = new System.Drawing.Size(50, 15);
            this.lblTest5.TabIndex = 66;
            this.lblTest5.Text = "label16";
            this.lblTest5.Visible = false;
            // 
            // lblTest4
            // 
            this.lblTest4.AutoSize = true;
            this.lblTest4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTest4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTest4.Location = new System.Drawing.Point(151, 165);
            this.lblTest4.Name = "lblTest4";
            this.lblTest4.Size = new System.Drawing.Size(50, 15);
            this.lblTest4.TabIndex = 66;
            this.lblTest4.Text = "label16";
            this.lblTest4.Visible = false;
            // 
            // lblTest3
            // 
            this.lblTest3.AutoSize = true;
            this.lblTest3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTest3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTest3.Location = new System.Drawing.Point(151, 145);
            this.lblTest3.Name = "lblTest3";
            this.lblTest3.Size = new System.Drawing.Size(50, 15);
            this.lblTest3.TabIndex = 66;
            this.lblTest3.Text = "label16";
            this.lblTest3.Visible = false;
            // 
            // lblTest2
            // 
            this.lblTest2.AutoSize = true;
            this.lblTest2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTest2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTest2.Location = new System.Drawing.Point(151, 125);
            this.lblTest2.Name = "lblTest2";
            this.lblTest2.Size = new System.Drawing.Size(50, 15);
            this.lblTest2.TabIndex = 66;
            this.lblTest2.Text = "label16";
            this.lblTest2.Visible = false;
            // 
            // lblTest1
            // 
            this.lblTest1.AutoSize = true;
            this.lblTest1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTest1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTest1.Location = new System.Drawing.Point(151, 105);
            this.lblTest1.Name = "lblTest1";
            this.lblTest1.Size = new System.Drawing.Size(50, 15);
            this.lblTest1.TabIndex = 66;
            this.lblTest1.Text = "label16";
            this.lblTest1.Visible = false;
            // 
            // txtRatioImage2ROI
            // 
            this.txtRatioImage2ROI.Location = new System.Drawing.Point(128, 291);
            this.txtRatioImage2ROI.Name = "txtRatioImage2ROI";
            this.txtRatioImage2ROI.Size = new System.Drawing.Size(50, 20);
            this.txtRatioImage2ROI.TabIndex = 65;
            // 
            // btnGetRecInfo
            // 
            this.btnGetRecInfo.BackColor = System.Drawing.Color.Chocolate;
            this.btnGetRecInfo.Enabled = false;
            this.btnGetRecInfo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetRecInfo.Location = new System.Drawing.Point(19, 123);
            this.btnGetRecInfo.Name = "btnGetRecInfo";
            this.btnGetRecInfo.Size = new System.Drawing.Size(98, 24);
            this.btnGetRecInfo.TabIndex = 64;
            this.btnGetRecInfo.Text = "GET ROI INFO";
            this.btnGetRecInfo.UseVisualStyleBackColor = false;
            this.btnGetRecInfo.Click += new System.EventHandler(this.btnGetRecInfo_Click);
            // 
            // btnEditROI
            // 
            this.btnEditROI.BackColor = System.Drawing.Color.DarkKhaki;
            this.btnEditROI.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditROI.Location = new System.Drawing.Point(123, 19);
            this.btnEditROI.Name = "btnEditROI";
            this.btnEditROI.Size = new System.Drawing.Size(71, 25);
            this.btnEditROI.TabIndex = 28;
            this.btnEditROI.Text = "EDIT ROI";
            this.btnEditROI.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEditROI.UseVisualStyleBackColor = false;
            this.btnEditROI.Click += new System.EventHandler(this.btnEditROI_Click);
            // 
            // btnClearROI
            // 
            this.btnClearROI.BackColor = System.Drawing.Color.GreenYellow;
            this.btnClearROI.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearROI.Location = new System.Drawing.Point(20, 94);
            this.btnClearROI.Name = "btnClearROI";
            this.btnClearROI.Size = new System.Drawing.Size(98, 24);
            this.btnClearROI.TabIndex = 28;
            this.btnClearROI.Text = "CLEAR ROI";
            this.btnClearROI.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClearROI.UseVisualStyleBackColor = false;
            this.btnClearROI.Click += new System.EventHandler(this.btnClearROI_Click);
            // 
            // label147
            // 
            this.label147.AutoSize = true;
            this.label147.BackColor = System.Drawing.Color.DarkTurquoise;
            this.label147.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label147.Location = new System.Drawing.Point(19, 262);
            this.label147.Name = "label147";
            this.label147.Size = new System.Drawing.Size(41, 15);
            this.label147.TabIndex = 60;
            this.label147.Text = "angle:";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.BackColor = System.Drawing.Color.PaleGreen;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.Location = new System.Drawing.Point(19, 236);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(46, 15);
            this.label44.TabIndex = 60;
            this.label44.Text = "Height:";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnApplyROI
            // 
            this.btnApplyROI.BackColor = System.Drawing.Color.Yellow;
            this.btnApplyROI.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyROI.Location = new System.Drawing.Point(123, 64);
            this.btnApplyROI.Name = "btnApplyROI";
            this.btnApplyROI.Size = new System.Drawing.Size(71, 24);
            this.btnApplyROI.TabIndex = 28;
            this.btnApplyROI.Text = "APPLY ROI";
            this.btnApplyROI.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnApplyROI.UseVisualStyleBackColor = false;
            this.btnApplyROI.Click += new System.EventHandler(this.btnApplyROI_Click);
            // 
            // btnAddROI
            // 
            this.btnAddROI.BackColor = System.Drawing.Color.Chartreuse;
            this.btnAddROI.Enabled = false;
            this.btnAddROI.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddROI.Location = new System.Drawing.Point(20, 64);
            this.btnAddROI.Name = "btnAddROI";
            this.btnAddROI.Size = new System.Drawing.Size(98, 24);
            this.btnAddROI.TabIndex = 28;
            this.btnAddROI.Text = "ADD ROI";
            this.btnAddROI.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddROI.UseVisualStyleBackColor = false;
            this.btnAddROI.Click += new System.EventHandler(this.btnAddROI_Click);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.BackColor = System.Drawing.Color.SteelBlue;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(17, 182);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(44, 15);
            this.label43.TabIndex = 61;
            this.label43.Text = "Pos. Y:";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkUsePPevaluationROI
            // 
            this.chkUsePPevaluationROI.AutoSize = true;
            this.chkUsePPevaluationROI.BackColor = System.Drawing.Color.YellowGreen;
            this.chkUsePPevaluationROI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUsePPevaluationROI.Location = new System.Drawing.Point(31, 23);
            this.chkUsePPevaluationROI.Name = "chkUsePPevaluationROI";
            this.chkUsePPevaluationROI.Size = new System.Drawing.Size(77, 17);
            this.chkUsePPevaluationROI.TabIndex = 55;
            this.chkUsePPevaluationROI.Text = "USE ROI";
            this.chkUsePPevaluationROI.UseVisualStyleBackColor = false;
            this.chkUsePPevaluationROI.CheckedChanged += new System.EventHandler(this.chkUsePPevaluationROI_CheckedChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.BackColor = System.Drawing.Color.PaleGreen;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.Location = new System.Drawing.Point(19, 209);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(41, 15);
            this.label42.TabIndex = 62;
            this.label42.Text = "Widht:";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPProiPosX
            // 
            this.txtPProiPosX.BackColor = System.Drawing.Color.SteelBlue;
            this.txtPProiPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPProiPosX.Location = new System.Drawing.Point(68, 152);
            this.txtPProiPosX.Name = "txtPProiPosX";
            this.txtPProiPosX.Size = new System.Drawing.Size(49, 20);
            this.txtPProiPosX.TabIndex = 58;
            this.txtPProiPosX.Text = "1700";
            // 
            // label45
            // 
            this.label45.BackColor = System.Drawing.Color.SteelBlue;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(20, 291);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(102, 20);
            this.label45.TabIndex = 63;
            this.label45.Text = "roi to image ratio:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.SteelBlue;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(17, 155);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 15);
            this.label15.TabIndex = 63;
            this.label15.Text = "Pos. X:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPProiWidth
            // 
            this.txtPProiWidth.BackColor = System.Drawing.Color.PaleGreen;
            this.txtPProiWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPProiWidth.Location = new System.Drawing.Point(68, 206);
            this.txtPProiWidth.Name = "txtPProiWidth";
            this.txtPProiWidth.Size = new System.Drawing.Size(49, 20);
            this.txtPProiWidth.TabIndex = 57;
            this.txtPProiWidth.Text = "2493";
            // 
            // txtPProiPosY
            // 
            this.txtPProiPosY.BackColor = System.Drawing.Color.SteelBlue;
            this.txtPProiPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPProiPosY.Location = new System.Drawing.Point(68, 179);
            this.txtPProiPosY.Name = "txtPProiPosY";
            this.txtPProiPosY.Size = new System.Drawing.Size(49, 20);
            this.txtPProiPosY.TabIndex = 59;
            this.txtPProiPosY.Text = "676";
            // 
            // txtROIangle
            // 
            this.txtROIangle.BackColor = System.Drawing.Color.DarkTurquoise;
            this.txtROIangle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtROIangle.Location = new System.Drawing.Point(68, 259);
            this.txtROIangle.Name = "txtROIangle";
            this.txtROIangle.Size = new System.Drawing.Size(49, 20);
            this.txtROIangle.TabIndex = 56;
            this.txtROIangle.Text = "0";
            // 
            // txtPProiHeight
            // 
            this.txtPProiHeight.BackColor = System.Drawing.Color.PaleGreen;
            this.txtPProiHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPProiHeight.Location = new System.Drawing.Point(68, 233);
            this.txtPProiHeight.Name = "txtPProiHeight";
            this.txtPProiHeight.Size = new System.Drawing.Size(49, 20);
            this.txtPProiHeight.TabIndex = 56;
            this.txtPProiHeight.Text = "2240";
            // 
            // btnRestZoom
            // 
            this.btnRestZoom.BackColor = System.Drawing.Color.DarkOrange;
            this.btnRestZoom.Location = new System.Drawing.Point(165, 82);
            this.btnRestZoom.Name = "btnRestZoom";
            this.btnRestZoom.Size = new System.Drawing.Size(96, 23);
            this.btnRestZoom.TabIndex = 66;
            this.btnRestZoom.Text = "RESET ZOOM";
            this.btnRestZoom.UseVisualStyleBackColor = false;
            this.btnRestZoom.Click += new System.EventHandler(this.btnResetZoom_Click);
            // 
            // pbxEditRoi
            // 
            this.pbxEditRoi.Location = new System.Drawing.Point(539, 30);
            this.pbxEditRoi.Name = "pbxEditRoi";
            this.pbxEditRoi.Size = new System.Drawing.Size(1180, 822);
            this.pbxEditRoi.TabIndex = 67;
            this.pbxEditRoi.TabStop = false;
            this.pbxEditRoi.Visible = false;
            this.pbxEditRoi.Paint += new System.Windows.Forms.PaintEventHandler(this.pbxEditRoi_Paint);
            // 
            // lststdResults
            // 
            this.lststdResults.BackColor = System.Drawing.SystemColors.Info;
            this.lststdResults.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lststdResults.FormattingEnabled = true;
            this.lststdResults.ItemHeight = 16;
            this.lststdResults.Location = new System.Drawing.Point(555, 859);
            this.lststdResults.Name = "lststdResults";
            this.lststdResults.Size = new System.Drawing.Size(742, 84);
            this.lststdResults.TabIndex = 68;
            this.lststdResults.Visible = false;
            // 
            // imgLstIndex
            // 
            this.imgLstIndex.AutoSize = true;
            this.imgLstIndex.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.imgLstIndex.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imgLstIndex.Location = new System.Drawing.Point(350, 371);
            this.imgLstIndex.Name = "imgLstIndex";
            this.imgLstIndex.Size = new System.Drawing.Size(32, 16);
            this.imgLstIndex.TabIndex = 69;
            this.imgLstIndex.Text = "Index";
            // 
            // lblRunMode
            // 
            this.lblRunMode.AutoSize = true;
            this.lblRunMode.Location = new System.Drawing.Point(1671, 949);
            this.lblRunMode.Name = "lblRunMode";
            this.lblRunMode.Size = new System.Drawing.Size(41, 13);
            this.lblRunMode.TabIndex = 70;
            this.lblRunMode.Text = "label16";
            // 
            // btnROIapplyBatch
            // 
            this.btnROIapplyBatch.BackColor = System.Drawing.Color.Yellow;
            this.btnROIapplyBatch.ContextMenuStrip = this.contextMenuStrip4;
            this.btnROIapplyBatch.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnROIapplyBatch.Location = new System.Drawing.Point(130, 259);
            this.btnROIapplyBatch.Name = "btnROIapplyBatch";
            this.btnROIapplyBatch.Size = new System.Drawing.Size(91, 24);
            this.btnROIapplyBatch.TabIndex = 68;
            this.btnROIapplyBatch.Text = "APPLY ROI B";
            this.btnROIapplyBatch.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnROIapplyBatch.UseVisualStyleBackColor = false;
            this.btnROIapplyBatch.Click += new System.EventHandler(this.btnROIapplyBatch_Click);
            // 
            // contextMenuStrip4
            // 
            this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetROIBatchFileTSMI4});
            this.contextMenuStrip4.Name = "contextMenuStrip4";
            this.contextMenuStrip4.Size = new System.Drawing.Size(181, 48);
            // 
            // resetROIBatchFileTSMI4
            // 
            this.resetROIBatchFileTSMI4.Name = "resetROIBatchFileTSMI4";
            this.resetROIBatchFileTSMI4.Size = new System.Drawing.Size(180, 22);
            this.resetROIBatchFileTSMI4.Text = "Reset ROI Batch File";
            this.resetROIBatchFileTSMI4.Click += new System.EventHandler(this.resetROIBatchFileTSMI4_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1791, 971);
            this.Controls.Add(this.lblRunMode);
            this.Controls.Add(this.imgLstIndex);
            this.Controls.Add(this.lststdResults);
            this.Controls.Add(this.pbxEditRoi);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.btnRestZoom);
            this.Controls.Add(this.grbStaticROI);
            this.Controls.Add(this.btnIMGcomboRefresh);
            this.Controls.Add(this.lstModels);
            this.Controls.Add(this.cmbImageName);
            this.Controls.Add(this.txtThresholdsUpper);
            this.Controls.Add(this.txtThresholdsLower);
            this.Controls.Add(this.txtModelsPath);
            this.Controls.Add(this.txtImagePath);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label46);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtLastModified);
            this.Controls.Add(this.txtToolType);
            this.Controls.Add(this.lblClassName);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtToolName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRunTimeWSName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDoEvaluation);
            this.Controls.Add(this.btnLoadModel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Sample Viewer, 24-08-2023";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.grbStaticROI.ResumeLayout(false);
            this.grbStaticROI.PerformLayout();
            this.contextMenuStrip3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxEditRoi)).EndInit();
            this.contextMenuStrip4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadModel;
        private System.Windows.Forms.Button btnDoEvaluation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRunTimeWSName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtToolName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtToolType;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showUnshowMarkingTSMI01;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblProcessedSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.ComboBox cmbImageName;
        private System.Windows.Forms.TextBox txtModelsPath;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ListBox lstModels;
        private System.Windows.Forms.Button btnIMGcomboRefresh;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtThresholdsLower;
        private System.Windows.Forms.TextBox txtThresholdsUpper;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem updateCurrentModelThresholdsTSMI2;
        private System.Windows.Forms.Button btnViewResults;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtLastModified;
        private System.Windows.Forms.GroupBox grbStaticROI;
        private System.Windows.Forms.TextBox txtRatioImage2ROI;
        private System.Windows.Forms.Button btnGetRecInfo;
        private System.Windows.Forms.Button btnClearROI;
        private System.Windows.Forms.Label label147;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Button btnAddROI;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.CheckBox chkUsePPevaluationROI;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox txtPProiPosX;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtPProiWidth;
        private System.Windows.Forms.TextBox txtPProiPosY;
        private System.Windows.Forms.TextBox txtROIangle;
        private System.Windows.Forms.TextBox txtPProiHeight;
        private System.Windows.Forms.Button btnApplyROI;
        private System.Windows.Forms.Button btnRestZoom;
        private System.Windows.Forms.Button btnTest1;
        private System.Windows.Forms.Label lblTest6;
        private System.Windows.Forms.Label lblTest5;
        private System.Windows.Forms.Label lblTest4;
        private System.Windows.Forms.Label lblTest3;
        private System.Windows.Forms.Label lblTest2;
        private System.Windows.Forms.Label lblTest1;
        private System.Windows.Forms.ToolStripMenuItem saveImageWithMarkingTSMI1;
        private System.Windows.Forms.PictureBox pbxEditRoi;
        private System.Windows.Forms.Button btnEditROI;
        private System.Windows.Forms.ListBox lststdResults;
        private System.Windows.Forms.Label imgLstIndex;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem copyRoiToClipboardTSMI3;
        private System.Windows.Forms.Label lblRunMode;
        private System.Windows.Forms.Button btnROIapplyBatch;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip4;
        private System.Windows.Forms.ToolStripMenuItem resetROIBatchFileTSMI4;
    }
}

