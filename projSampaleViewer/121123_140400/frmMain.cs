using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using ViDi2.Runtime;
using ViDi2.UI;
using ViDi2.UI.ViewModels;
using ViDi.NET;
using System.IO;
using System.Windows.Input;
using System.Windows;
using INIgetset;
using DefectsInspection;
using SystemColors = System.Windows.SystemColors;



//using HostingWpfUserControlInWf;

namespace projSampaleViewer
{
    
    #region -----------------Name space level Declarations---------------------


    #endregion



    public partial class frmMain : Form
    {
        //Author: Yoav Tamari
        //Project: EndMill
        //Created: 15-08-2023 
        //Description: 

        

        //constructor
        public frmMain()
        {
            InitializeComponent();
        }


        #region -----------------Class level Declarations---------------------  
        //        [System.Security.SecurityCritical]
        //#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        //        public virtual System.Runtime.Remoting.ObjRef CreateObjRef(Graphics bmp);
        //#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        
        public struct RegionFound
        {
            public double area;
            public int width;
            public int height;
            public ViDi2.Point center;
            public double score;
            public string classColor;
            public double compactness;
            public System.Collections.ObjectModel.ReadOnlyCollection<ViDi2.IRegion> covers;
            public string className;
            public System.Collections.ObjectModel.ReadOnlyCollection<ViDi2.Point> outer;
            public double perimeter;

        }
        public struct ResultsFound
        {
            public RegionFound[] regionFounds;
            public string imageName;
            public string modelName;
            public string thresholdLow;
            public string thresholdUpper;
            public ROI roi;
        }
        public struct RatioXratioY
        {
            public float ratioX;
            public float ratioY;
        }
        public struct ROI
        {
            public Rect rect;
            public double angle;
            public bool xROIused;
        }
        public struct CurrentImage
        {
            public int imgWidth;
            public int imgHeight;
            public int picBoxDefaultWidth;
            public int picBoxDefaultHeight;
            public string path;
            public float ratioX; //ratio real image size to picture box size
            public float ratioY; //ratio real image size to picture box size
            public string imageName;
            public int InOrderIndex;
            public string CatalogNumber;
            public int iStationNumber;
            //public Image zoomSnap1;
            //public Image zoomSnap2;
            //public Image zoomSnap3;
            //public Image zoomSnap4;
            //public Image zoomSnap5;
            public Image imgActiveImage;
            public string LocalLastImagePath;
            public string ImageFileType;

        }

        ViDiSuiteServiceLocator viDiSuiteServiceLocator = new ViDiSuiteServiceLocator();     
        System.Windows.Controls.UserControl cc = new System.Windows.Controls.UserControl();        
        Func<int, int> square = x => x * x;        
        IControl control;
        IWorkspace workspace;
        IStream stream;
        private ucSampleViewerClass sampleviewr = new ucSampleViewerClass();
        private string sversion = "Sample Viewer 09-11-2023";
        private string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
        private ViDi2.Runtime.IWorkspaceList rwsl;
        public static RectangleResizeAndRotateAdd StaticROI = new RectangleResizeAndRotateAdd();
        private Rect currentROI = new Rect();
        public static frmStartUpWindow sform;
        private bool xNoAction;
        private System.Threading.Thread tnewthread;
        private System.Windows.Controls.TextBlock txt1 = new System.Windows.Controls.TextBlock();
        private System.Windows.Controls.TextBlock txt2 = new System.Windows.Controls.TextBlock();
        private System.Windows.Controls.ListBox lstResults = new System.Windows.Controls.ListBox();
        public System.Drawing.Drawing2D.GraphicsState aState;
        private CurrentImage gcurrentImage = new CurrentImage();
        private int iCountPain;
        private bool batchFirst;
        private string pathROIbatch = System.Windows.Forms.Application.StartupPath + @"\Data\ROI Batch\ROIbatch.dat";
        private int iImageIndex;

        public ISampleViewerViewModel SampleViewerViewModel => sampleviewr.sampleViewer.ViewModel;

        //public ViDi2.Training.UI.Viewer TRViewer => sampleviewr.viewer;


        #endregion

        #region-------------------------Methods -----------------------------
        private void loadModels()
        {
            try
            {


                string modelPath = "";
                IniFile AppliIni = new IniFile(INIpath);
                modelPath = AppliIni.ReadValue("Last Model", "Full path", "");

                //string fn = Path.GetFileName(modelPath);
                //int index = this.lstModels.Items.IndexOf(fn);               
                //this.lstModels.SelectedIndex = index;

                if (true)
                {
                    using (var fs = new System.IO.FileStream(modelPath, System.IO.FileMode.Open, FileAccess.Read))
                    {
                        System.Windows.Forms.Application.DoEvents();

                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        //var stopwatch = new Stopwatch();
                        //stopwatch.Start();
                        string[] s0 = modelPath.Split('\\');

                        if (Workspace != null)
                            if (Workspace.UniqueName == s0[s0.GetLength(0) - 1].Substring(0, s0[s0.GetLength(0) - 1].Length - 5)) { System.Windows.Forms.MessageBox.Show("This Model Is Already Loaded!"); goto exitProcedure; }

                        if (Workspace !=null)
                            if (Workspace.IsOpen)
                                    Workspace.Close();

                        bool xSingleLoad = false;
                        if (xSingleLoad)
                        {
                            Workspace = control.Workspaces.Add(Path.GetFileNameWithoutExtension(modelPath), fs);
                        }
                        else
                        {
                            foreach (var item in lstModels.Items)
                            {
                                System.Windows.Forms.Application.DoEvents();

                                string modelPath1 = txtModelsPath.Text.Trim() + @"\" + item.ToString().Trim();
                                using (var fs1 = new System.IO.FileStream(modelPath1, System.IO.FileMode.Open, FileAccess.Read))
                                {
                                    control.Workspaces.Add(Path.GetFileNameWithoutExtension(modelPath1), fs1);
                                }
                            }
                        }

                        rwsl = control.Workspaces;
                        Workspace = rwsl[rwsl.Names[0]];

                       string[] s = new string[0];
                        if (Workspace.Parameters.Description != "")
                            s = Workspace.Parameters.Description.Split(' ');
                        if (s.GetLength(0) > 0)
                        {
                            float f = System.Convert.ToSingle(s[s.GetLength(0) - 1]);
                            //lblToolBestLoss.Content = f.ToString("0.00");
                        }
                        else
                        {
                            //lblToolBestLoss.Content = "Unknown";
                        }

                        Mouse.OverrideCursor = null;
                    }
                }

                RaisePropertyChanged(nameof(Workspaces));

                //
                txtRunTimeWSName.Text = Workspace.DisplayName;
                stream = Workspace.Streams.First();
                string streamName = stream.Name;  // Workspace.Streams.First().Name;
                ITool tool = Workspace.Streams[streamName].Tools.First();
                txtToolName.Text = tool.Name;   // Workspace.Streams[streamName].Tools.First().Name;            
                txtToolType.Text = tool.Type.ToString();

                //class name display
                IRedTool tool1 = (IRedTool)stream.Tools.First();
                var knownClasses = tool1.KnownClasses;
                if (knownClasses.Count > 0)
                {
                    string className = knownClasses[0];
                    lblClassName.Text = className;
                }

                //will activate event on lst
                string fn = Path.GetFileName(modelPath);
                int index = this.lstModels.Items.IndexOf(fn);
                this.lstModels.SelectedIndex = index;

                //var si = this.lstModels.SelectedItem;

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("loadModel(), Error: " + e.Message);
            }

        exitProcedure:;

        }
        private static void LogDefectsLines(string string1, string string2,string fileName)
        {
            string path00 = System.Windows.Forms.Application.StartupPath + @"\Data\Defects Reports";

            string path = "";

            string[] dd = Directory.GetFiles(path00);
            foreach(string item in dd)
            {
                string[] s = item.Split('_');
                DateTime dt = new DateTime();

                DateTime dt1 = DateTime.Parse(s[1].Substring(0,2) + "/" + s[1].Substring(2, 2) + "/" + s[1].Substring(4, 2));

                if(dt1.Date == DateTime.Now.Date)
                {
                    
                    path = System.Windows.Forms.Application.StartupPath + @"\Data\Defects Reports\" + Path.GetFileName(item);
                    break;
                }
                else
                {
                    path = System.Windows.Forms.Application.StartupPath + @"\Data\Defects Reports\" + fileName;
                }
            }

            if (dd.GetLength(0) == 0) { path = System.Windows.Forms.Application.StartupPath + @"\Data\Defects Reports\" + fileName; }

            System.Console.WriteLine(string1 + string2);
            
            string[] stringLog = new string[1] { string1 + string2 };
            SaveTextToFile(stringLog, path, FileMode.Append);
        }
        private void ApplyROI(bool xNoSave,Rectangle ImageDimensions)
        {

            ViDi2.IManualRegionOfInterest redROI01 = (ViDi2.IManualRegionOfInterest)Stream.Tools.First().RegionOfInterest;

            redROI01.Parameters.Units = ViDi2.UnitsMode.Pixel;


            if (chkUsePPevaluationROI.Checked)
            {


                double ROIXpos = Convert.ToDouble(txtPProiPosX.Text.Trim());
                double ROIYpos = Convert.ToDouble(txtPProiPosY.Text.Trim());
                double ROIwidth = Convert.ToDouble(txtPProiWidth.Text.Trim());
                double ROIheight = Convert.ToDouble(txtPProiHeight.Text.Trim());
                double ROIangle = Convert.ToDouble(txtROIangle.Text.Trim());

                redROI01.Parameters.Offset = new ViDi2.Point(ROIXpos, ROIYpos);
                redROI01.Parameters.Size = new ViDi2.Size(ROIwidth, ROIheight);

                currentROI.X = redROI01.Parameters.Offset.X;
                currentROI.Y = redROI01.Parameters.Offset.Y;

                currentROI.Width = redROI01.Parameters.Size.Width;
                currentROI.Height = redROI01.Parameters.Size.Height;

                ViDi2.Size size = redROI01.Parameters.Scale;

                size.Height = 1;
                size.Width = 1;

                redROI01.Parameters.Scale = size; //tested, size = roi scale with respect to image;

                if (currentROI.Height != 1 && currentROI.Width != 1 && (xNoSave))
                {
                    IniFile AppliIni = new IniFile(INIpath);
                    AppliIni.WriteValue("roi", "x", currentROI.X.ToString());
                    AppliIni.WriteValue("roi", "y", currentROI.Y.ToString());
                    AppliIni.WriteValue("roi", "width", currentROI.Width.ToString());
                    AppliIni.WriteValue("roi", "height", currentROI.Height.ToString());

                    AppliIni.WriteValue("roi", "used", chkUsePPevaluationROI.Checked.ToString());
                }

                ISampleViewerView vs = sampleviewr.sampleViewer.ViewModel.View;

                double imageW = vs.ViewModel.ViewWidth;
                double imageH = vs.ViewModel.ViewHeight;
                
                if(imageW ==1 && imageH ==1)
                {
                    imageW = ImageDimensions.Width;
                    imageH = ImageDimensions.Height;
                }


                double areaimg = imageW * imageH;
                int arearoi = (int)(redROI01.Parameters.Size.Width * redROI01.Parameters.Size.Height);
                if(areaimg != 1) // = 1, no image dimensions avialble
                    txtRatioImage2ROI.Text = ((arearoi / areaimg )).ToString("0.000");
                else
                    txtRatioImage2ROI.Text = "***";

            }
            else
            {

                ISampleViewerView vs = sampleviewr.sampleViewer.ViewModel.View;

                double imageW = vs.ViewModel.ViewWidth;
                double imageH = vs.ViewModel.ViewHeight;

                redROI01.Parameters.Offset = new ViDi2.Point(0, 0);
                redROI01.Parameters.Size = new ViDi2.Size(imageW, imageH);

                currentROI.X = redROI01.Parameters.Offset.X;
                currentROI.Y = redROI01.Parameters.Offset.Y;

                currentROI.Width = imageW;
                currentROI.Height = imageH;

                double areaimg = (imageW * imageH);
                txtRatioImage2ROI.Text = ((areaimg / areaimg)).ToString(("0.000"));

                if (!(currentROI.Height == 1 && currentROI.Width == 1))
                {
                    IniFile AppliIni = new IniFile(INIpath);
                    //AppliIni.WriteValue("roi", "x", currentROI.X.ToString());
                    //AppliIni.WriteValue("roi", "y", currentROI.Y.ToString());
                    //AppliIni.WriteValue("roi", "width", currentROI.Width.ToString());
                    //AppliIni.WriteValue("roi", "height", currentROI.Height.ToString());

                    AppliIni.WriteValue("roi", "used", chkUsePPevaluationROI.Checked.ToString());
                }
            }




        }
        private void SaveROIBatch(bool xNoSave, Rectangle ImageDimensions)
        {

            ViDi2.IManualRegionOfInterest redROI01 = (ViDi2.IManualRegionOfInterest)Stream.Tools.First().RegionOfInterest;

            redROI01.Parameters.Units = ViDi2.UnitsMode.Pixel;


           


                double ROIXpos = Convert.ToDouble(txtPProiPosX.Text.Trim());
                double ROIYpos = Convert.ToDouble(txtPProiPosY.Text.Trim());
                double ROIwidth = Convert.ToDouble(txtPProiWidth.Text.Trim());
                double ROIheight = Convert.ToDouble(txtPProiHeight.Text.Trim());
                double ROIangle = Convert.ToDouble(txtROIangle.Text.Trim());

                redROI01.Parameters.Offset = new ViDi2.Point(ROIXpos, ROIYpos);
                redROI01.Parameters.Size = new ViDi2.Size(ROIwidth, ROIheight);

                currentROI.X = redROI01.Parameters.Offset.X;
                currentROI.Y = redROI01.Parameters.Offset.Y;

                currentROI.Width = redROI01.Parameters.Size.Width;
                currentROI.Height = redROI01.Parameters.Size.Height;

                ViDi2.Size size = redROI01.Parameters.Scale;

                size.Height = 1;
                size.Width = 1;

                redROI01.Parameters.Scale = size; //tested, size = roi scale with respect to image;

                if (currentROI.Height != 1 && currentROI.Width != 1 && (xNoSave))
                {
                //IniFile AppliIni = new IniFile(INIpath);
                //AppliIni.WriteValue("roi", "x", currentROI.X.ToString());
                //AppliIni.WriteValue("roi", "y", currentROI.Y.ToString());
                //AppliIni.WriteValue("roi", "width", currentROI.Width.ToString());
                //AppliIni.WriteValue("roi", "height", currentROI.Height.ToString());

                //AppliIni.WriteValue("roi", "used", chkUsePPevaluationROI.Checked.ToString());
                iImageIndex++;

                string saveString =     iImageIndex.ToString()         + "," 
                                        + currentROI.X.ToString()      + ","
                                        + currentROI.Y.ToString()      + ","
                                        + currentROI.Width.ToString()  + ","
                                        + currentROI.Height.ToString()

                        ;

                    //save to batch file
                    

                    string[] sString = new string[1] { saveString };
                    if (!batchFirst)
                    {
                        batchFirst = true;
                        SaveTextToFile(sString, pathROIbatch, FileMode.Create);
                    }
                    else
                    {
                        SaveTextToFile(sString, pathROIbatch, FileMode.Append);
                    }



                }

                ISampleViewerView vs = sampleviewr.sampleViewer.ViewModel.View;

                double imageW = vs.ViewModel.ViewWidth;
                double imageH = vs.ViewModel.ViewHeight;

                if (imageW == 1 && imageH == 1)
                {
                    imageW = ImageDimensions.Width;
                    imageH = ImageDimensions.Height;
                }


                //double areaimg = imageW * imageH;
                //int arearoi = (int)(redROI01.Parameters.Size.Width * redROI01.Parameters.Size.Height);
                //if (areaimg != 1) // = 1, no image dimensions avialble
                //    txtRatioImage2ROI.Text = ((arearoi / areaimg)).ToString("0.000");
                //else
                //    txtRatioImage2ROI.Text = "***";

           
           




        }
        private void ROIeditMode()
        {
            btnGetRecInfo.Enabled = true;

            RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxEditRoi, gcurrentImage);

            float ratioX = ratioxy.ratioX;
            float ratioY = ratioxy.ratioY;

            //if roi stored in ini use it for new roi
            string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
            IniFile AppliIni = new IniFile(INIpath);
            Rect rect = new Rect();

            rect.X = Convert.ToDouble(AppliIni.ReadValue("roi", "x", "0"));
            rect.Y = Convert.ToDouble(AppliIni.ReadValue("roi", "y", "0"));
            rect.Width = Convert.ToDouble(AppliIni.ReadValue("roi", "width", "0"));
            rect.Height = Convert.ToDouble(AppliIni.ReadValue("roi", "height", "0"));

            if(StaticROI != null)
            {
                StaticROI = null;
            }

            StaticROI = new RectangleResizeAndRotateAdd(Convert.ToSingle(rect.X * (1 / ratioX)), Convert.ToSingle(rect.Y * (1 / ratioY)), Convert.ToSingle(rect.Width * (1 / ratioX)), Convert.ToSingle(rect.Height * (1 / ratioY)));

            //recAdd[bboxIndex].useAsROI = "roi";
            StaticROI.SetPictureBox(this.pbxEditRoi);

            //this.pbxEditRoi.Refresh();
        }
        private void tryGridWrite()
        {
            //------------Yoav. 10-09-2023: try to find background writing to sampleViewer---------------
            System.Windows.Controls.Grid grid = (System.Windows.Controls.Grid)sampleviewr.sampleViewer.Content;

            string g11 = grid.FlowDirection.ToString();
            Type typ = grid.GetType();

            bool xCreateGrid = true;

            if (xCreateGrid)
            {
                grid.ShowGridLines = true;

                System.Windows.Controls.ColumnDefinition colDef1 = new System.Windows.Controls.ColumnDefinition();
                //System.Windows.Controls.ColumnDefinition colDef2 = new System.Windows.Controls.ColumnDefinition();
                //System.Windows.Controls.ColumnDefinition colDef3 = new System.Windows.Controls.ColumnDefinition();

                grid.ColumnDefinitions.Add(colDef1);
                //grid.ColumnDefinitions.Add(colDef2);
                //grid.ColumnDefinitions.Add(colDef3);

                // Define the Rows
                System.Windows.Controls.RowDefinition rowDef1 = new System.Windows.Controls.RowDefinition();
                //System.Windows.Controls.RowDefinition rowDef2 = new System.Windows.Controls.RowDefinition();
                //System.Windows.Controls.RowDefinition rowDef3 = new System.Windows.Controls.RowDefinition();
                //System.Windows.Controls.RowDefinition rowDef4 = new System.Windows.Controls.RowDefinition();

                grid.RowDefinitions.Add(rowDef1);
                //grid.RowDefinitions.Add(rowDef2);
                //grid.RowDefinitions.Add(rowDef3);
                //grid.RowDefinitions.Add(rowDef4);

                // Add the first text cell to the Grid
                //System.Windows.Controls.TextBlock txt1 = new System.Windows.Controls.TextBlock();

                //----------------------------------------------txt1-----------------------------------------
                txt1.Text = "Cognex API Runtime Evaluator";
                txt1.FontSize = 20;
                txt1.FontWeight = FontWeights.Bold;
                txt1.Width = 300;
                txt1.Height= 200;

                txt1.VerticalAlignment   = System.Windows.VerticalAlignment.Bottom;
                txt1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                Thickness th = new Thickness();
                th.Bottom = 200; //works good, 11-10-2023
                th.Top    = 200; //works good, 11-10-2023
                th.Right  = 200; //not tested
                th.Left   = 700; //works good, 11-10-2023
                txt1.Margin = th;
                txt1.TextAlignment = TextAlignment.Left;
                txt1.LineHeight = 300;
                txt1.Visibility = Visibility.Visible;
            
                txt1.Foreground = SystemColors.ControlLightLightBrush; // ActiveCaptionBrush;
                System.Windows.Controls.Grid.SetRow(txt1, 0); //0
                System.Windows.Controls.Grid.SetColumn(txt1, 0);
                System.Windows.Controls.Grid.SetColumnSpan(txt1, 1);  //3
                
                txt1.Name = "txt1";
                grid.Children.Add(txt1); //make it work, 11-10-2023

                //----------------------------------------------txt2-----------------------------------------
                
                txt2.Text = "Ready For Doing Evaluation";
                txt2.FontSize = 20;
                txt2.FontWeight = FontWeights.Bold;
                txt2.Width = 300;
                txt2.Height = 200;

                txt2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                txt2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                Thickness th2 = new Thickness();
                th2.Bottom = 200; //works good, 11-10-2023
                th2.Top = 200; //works good, 11-10-2023
                th2.Right = 200; //not tested
                th2.Left = 300; //works good, 11-10-2023
                txt2.Margin = th2;

                txt2.TextAlignment = TextAlignment.Left;
                txt2.LineHeight = 300;
                txt2.Visibility = Visibility.Visible;

                txt2.Foreground = SystemColors.ControlLightLightBrush; // ActiveCaptionBrush;
                System.Windows.Controls.Grid.SetRow(txt2, 0); //0
                System.Windows.Controls.Grid.SetColumn(txt2, 0);
                System.Windows.Controls.Grid.SetColumnSpan(txt2, 1);  //3

                txt2.Name = "txt2";
                grid.Children.Add(txt2); //make it work, 11-10-2023
               
                //----------------------------------------------lstResults---------------------------------------- -
                lstResults.Items.Add("1 lstResults");
                lstResults.Items.Add("2 lstResults");
                lstResults.Items.Add("3 lstResults");
                lstResults.Items.Add("4 lstResults");
                lstResults.Items.Add("5 lstResults");

                lstResults.SelectedIndex = 0;
                lstResults.FontSize = 12;
                lstResults.FontWeight = FontWeights.Normal;
                lstResults.Width = 500;
                lstResults.Height = 40;

                System.Windows.Media.BrushConverter converter = new System.Windows.Media.BrushConverter();
                System.Windows.Media.Brush brush = (System.Windows.Media.Brush)converter.ConvertFromString("#FFFFFF90");

                lstResults.Background = brush;

                lstResults.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                lstResults.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                Thickness th3 = new Thickness();
                th3.Bottom = 350; //works good, 11-10-2023
                th3.Top    = 200; //works good, 11-10-2023
                th3.Right  = 200; //not tested
                th3.Left   = 700; //works good, 11-10-2023
                lstResults.Margin = th3;

                //lstResults.TextAlignment = TextAlignment.Left;
                //lstResults.LineHeight = 300;
                lstResults.Visibility = Visibility.Visible;

                lstResults.Foreground = SystemColors.ControlDarkDarkBrush; // ActiveCaptionBrush;
                System.Windows.Controls.Grid.SetRow(lstResults, 0); //0
                System.Windows.Controls.Grid.SetColumn(lstResults, 0);
                System.Windows.Controls.Grid.SetColumnSpan(lstResults, 1);  //3

                lstResults.Name = "lstResults";
                grid.Children.Add(lstResults); //make it work, 11-10-2023
               
            }


            System.Windows.Controls.ControlTemplate ct = sampleviewr.sampleViewer.Template;

            System.Windows.Media.FontFamily ff = sampleviewr.sampleViewer.FontFamily;

            double fs = sampleviewr.sampleViewer.FontSize; //std = 12
            sampleviewr.sampleViewer.FontSize = 12;
            double h = sampleviewr.sampleViewer.ActualHeight;
            double w = sampleviewr.sampleViewer.ActualWidth;

            DataTemplate dt = sampleviewr.sampleViewer.ContentTemplate;
            System.Windows.Controls.DataTemplateSelector dts = sampleviewr.sampleViewer.ContentTemplateSelector;
            System.Windows.Controls.ContextMenu fre = sampleviewr.sampleViewer.ContextMenu;
            System.Windows.Input.Cursor crs = sampleviewr.sampleViewer.Cursor;
            object dcotex = sampleviewr.sampleViewer.DataContext;
            bool xContent = sampleviewr.sampleViewer.HasContent;
            //sampleViewer.InputBindings
            bool xinputen = sampleviewr.sampleViewer.IsInputMethodEnabled;
            string sname = sampleviewr.sampleViewer.Name;
            ResourceDictionary rd = sampleviewr.sampleViewer.Resources;

            //-------------------------------------------------------------------------------------------
        }
        private bool LoadImage()
        {
            //string pathandbname

            bool xLoadOK = false;

            if (true) //File.Exists(pathandbname))
            {
                //no lock
                //Image image = LoadBitmapUnlocked(pathandbname);

                //currentImage.imgActiveImage = (Image)image.Clone();






                //currentImage.path = pathandbname;

                //defaultImagePath = pathandbname;
                gcurrentImage.imgWidth = SampleViewerViewModel.Sample.Image.Bitmap.Width;
                gcurrentImage.imgHeight = SampleViewerViewModel.Sample.Image.Bitmap.Height;

                float imgWidth = Convert.ToSingle(SampleViewerViewModel.Sample.Image.Bitmap.Width);
                float imgHeight = Convert.ToSingle(SampleViewerViewModel.Sample.Image.Bitmap.Height);
                float fAspectRatio = imgWidth / imgHeight;

                int width = 0;
                bool chckZoomChecked = false;
                if (!chckZoomChecked)
                {
                    width = this.pbxEditRoi.Width;  //pbxShowImage.Width;  //this.currentImage.picBoxDefaultWidth;
                }
                else
                {
                    width = (int)imgWidth;
                }

               

                //int width = pbxShowImage.Width;
                int height = this.pbxEditRoi.Height;
                float fHeight = Convert.ToSingle(width) / fAspectRatio;

                Bitmap img = new Bitmap(SampleViewerViewModel.Sample.Image.Bitmap, width, Convert.ToInt32(fHeight));

                //inv.set(panel4, "AutoScroll", true);  

                //panel1.AutoScroll = true;               
                this.pbxEditRoi.Image  = (Image)img.Clone();

                //currentImage.picBoxDefaultWidth = img.Width;
                //currentImage.picBoxDefaultHeight = img.Height;

                this.pbxEditRoi.SizeMode = PictureBoxSizeMode.AutoSize;

                gcurrentImage.picBoxDefaultWidth = this.pbxEditRoi.Width;
                gcurrentImage.picBoxDefaultHeight = this.pbxEditRoi.Height;

                if (img != null)
                    img.Dispose();

                xLoadOK = true;
            }
            else
            {
                xLoadOK = false;
            }

            return xLoadOK;

        }
        private void GetROIinfo()
        {
            int iRectangleNumber = 0;

            if (pbxEditRoi.Image != null)
            {
                RectangleResizeAndRotateAdd.RectangleFullDetails srtaticROI = StaticROI.GetRectF();

                RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxEditRoi, gcurrentImage);

                float ratioX = ratioxy.ratioX;
                float ratioY = ratioxy.ratioY;


                txtPProiPosX.Text = (Math.Round((srtaticROI.translation.X + srtaticROI.rectangleF.Left) * ratioX, 0)).ToString("0");
                txtPProiPosY.Text = (Math.Round((srtaticROI.translation.Y + srtaticROI.rectangleF.Top) * ratioY, 0)).ToString("0");
                txtPProiWidth.Text = Math.Round(srtaticROI.rectangleF.Width * ratioX, 0).ToString();
                txtPProiHeight.Text = Math.Round(srtaticROI.rectangleF.Height * ratioY, 0).ToString();
                txtROIangle.Text = Math.Round(srtaticROI.angle, 3).ToString();
                //this.txtAngle.Text = recAll.angle.ToString("0.00");

                int imgX = this.pbxEditRoi.Image.Width;
                int imgY = this.pbxEditRoi.Image.Height;

                int ImageArea = imgX * imgY; //pixels^2
                float roiArea = srtaticROI.rectangleF.Width * srtaticROI.rectangleF.Height;
                txtRatioImage2ROI.Text = (roiArea / ImageArea).ToString("0.00");
            }
        }
        #endregion

        #region------------------------Properties----------------------------
        private void initProporties()
        {
            IStream stream = Stream;
            IWorkspace workspace = Workspace;
            IControl control = Control;
            IList<IWorkspace> ws = Workspaces;
            Dictionary<int, string> dic = ViewIndices;

            //SampleViewerViewModel.
           
        }
        public IStream Stream
        {
            get { return stream; }
            set
            {
                stream = value;
                SampleViewerViewModel.Sample = null;
                RaisePropertyChanged(nameof(Stream));
            }
        }
        public IWorkspace Workspace
        {
            get { return workspace; }
            set
            {
                workspace = value;
                Stream = workspace.Streams.First();
                RaisePropertyChanged(nameof(Workspace));
            }
        }
        public IControl Control
        {
            get { return control; }
            set
            {
                control = value;
                RaisePropertyChanged(nameof(Control));
                RaisePropertyChanged(nameof(Workspaces));  //;
                RaisePropertyChanged(nameof(Stream));
            }
        }
        public IList<IWorkspace> Workspaces => Control.Workspaces.ToList();
        public DependencyObject Var { get; private set; }
        public Dictionary<int, string> ViewIndices
        {
            get
            {
                var indices = new Dictionary<int, string>();

                if (SampleViewerViewModel.Sample != null && SampleViewerViewModel.ToolName != "")
                {
                    var views = SampleViewerViewModel.Sample.Markings[SampleViewerViewModel.ToolName].Views;
                    indices.Add(-1, "all");

                    for (int i = 0; i < views.Count; ++i)
                        indices.Add(i, i.ToString());
                }

                return indices;
            }
        }
        
        #endregion

        #region---------------Methods For Events, Delegates------------------
        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {

                string[] sMode = System.Windows.Forms.Application.StartupPath.Split('\\');
                lblRunMode.Visible = false;
                lblRunMode.Text = "Run mode: " + sMode[sMode.Length-1];

                string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
                IniFile AppliIni = new IniFile(INIpath);

                //
                string ss = AppliIni.ReadValue("Models", "path", "");
                int numFiles = Directory.GetFiles(ss, "*.vrws").GetLength(0);

                //
                string smin = AppliIni.ReadValue("Thresholds", "min.", "0.4");
                string smax = AppliIni.ReadValue("Thresholds", "max.", "1.0");

                this.txtThresholdsLower.Text = smin.Trim();
                this.txtThresholdsUpper.Text = smax.Trim();

                //load startup form
                sform = new frmStartUpWindow();
                sform.Visible = true;
                sform.numCards = numFiles;

                sform.Show();
                sform.Refresh();

                // Initalize the control
                var control = new ViDi2.Runtime.Local.Control(ViDi2.GpuMode.Deferred);

                // Initializes all CUDA devices
                control.InitializeComputeDevices(ViDi2.GpuMode.SingleDevicePerTool, new List<int>() { });
                // Turns off optimized GPU memory since high-detail mode doesn't support it
                control.OptimizedGPUMemory(0);

                this.Control = control;

                //license
                string licenseType = "License: " + control.License.Type.ToString() + " " + control.License.PerformanceLevel.ToString();
                sversion = licenseType + ", " + sversion;
                this.Text = sversion;

                //
                sampleviewr.sampleViewer.Drop += btnDoEvaluation_Click;
                SampleViewerViewModel.ToolSelected += sampleViewer_ToolSelected;
                sampleviewr.sampleViewer.MouseDown += sampleViewer_MouseDown;
                //sampleviewr.sampleViewer.LayoutUpdated += sampleViewer_LayOutUpdate;

                //sampleviewr.sampleViewer.MouseWheel += elementHost2_MouseWheel;

                //attaching user control sample viewer
                sampleviewr.Visibility = System.Windows.Visibility.Visible;
                Var = sampleviewr.MainWindow.Parent;

                //sampleviewr.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                //sampleviewr.VerticalAlignment   = System.Windows.VerticalAlignment.Center;

                //sampleviewr.sampleViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                //sampleviewr.sampleViewer.VerticalAlignment   = System.Windows.VerticalAlignment.Center;

                elementHost1.Child = sampleviewr; //.MainWindow;

                initProporties();


                //necessary for auto marking on image in non-WPF application
                MarkingOverlayExtensions.SetupOverlayEnvironment(1000, false, false);

                elementHost1.Child.MouseWheel += elementHost2_MouseWheel;

                string ImagePath = AppliIni.ReadValue("Last Image", "path", "");
                txtImagePath.Text = ImagePath;

                string ImageName = AppliIni.ReadValue("Last Image", "name", "");

                string modelsPath = AppliIni.ReadValue("Models", "path", "");

                txtModelsPath.Text = modelsPath;
                string[] dirModels = Directory.GetFiles(modelsPath, "*.vrws");

                //only model name
                foreach (string item in dirModels)
                {
                    lstModels.Items.Add(Path.GetFileName(item));
                }

                string[] dirImages = Directory.GetFiles(ImagePath);

                //only image name
                foreach (string item in dirImages)
                {
                    cmbImageName.Items.Add(Path.GetFileName(item));
                }

                int index = cmbImageName.Items.IndexOf(ImageName);
                cmbImageName.SelectedIndex = index;

                //
                loadModels();

                sform.Visible = false;

                sform.Dispose();
                sform = null;

                //
                currentROI.X = Convert.ToDouble(AppliIni.ReadValue("roi", "x", "0"));
                currentROI.Y = Convert.ToDouble(AppliIni.ReadValue("roi", "y", "0"));
                currentROI.Width = Convert.ToDouble(AppliIni.ReadValue("roi", "width", "0"));
                currentROI.Height = Convert.ToDouble(AppliIni.ReadValue("roi", "height", "0"));

                txtPProiPosX.Text = currentROI.X.ToString();
                txtPProiPosY.Text = currentROI.Y.ToString();
                txtPProiWidth.Text = currentROI.Width.ToString();
                txtPProiHeight.Text = currentROI.Height.ToString();

                xNoAction = true;
                chkUsePPevaluationROI.Checked = Convert.ToBoolean(AppliIni.ReadValue("roi", "used", "False"));

                xNoAction = false;

                Rectangle ImageDimensions = new Rectangle();
                if (chkUsePPevaluationROI.Checked)
                    ApplyROI(true, ImageDimensions);

                //
                //tryGridWrite();  

            }
            catch(Exception e1)
            {
                System.Windows.Forms.MessageBox.Show("frmMain_Load(), Error: " + "\n\r\n\r"  + e1.Message + "\n\r\n\rClosing Application");
                // this.Close();
                System.Windows.Forms.Application.Exit();
            }
        }

        private void elementHost2_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            RoutedEvent  mb = e.RoutedEvent;



        }        
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //must have this, otherwise application will not shutdown
            MarkingOverlayExtensions.TeardownOverlayEnvironment();

            sampleviewr.sampleViewer.ViewModel.Dispose();
            sampleviewr.sampleViewer.Dispose();
            sampleviewr = null;

            //if (Workspace != null)
            //    if (Workspace.IsOpen)
            //    {
            //        Workspace.Close();
            //        control.Workspaces.Remove(Workspace.DisplayName);
            //    }

            if (rwsl != null)
            {
                for (int i = rwsl.Names.Count-1; i >-1; i--)
                {                    
                    if (rwsl[rwsl.Names[i]].IsOpen)
                         rwsl[rwsl.Names[i]].Close();
                }
            }

            if (control != null)
            {
                control.Dispose();
                control = null;
            }


        }
        private void btnLoadModel_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    DefaultExt = ".vrws",
                    Filter = "Cognex Deep Learning Studio Runtime Workspaces (*.vrws)|*.vrws",
                    ValidateNames = false
                };

                string modelPath = "";
                IniFile AppliIni = new IniFile(INIpath);

                if ((bool)dialog.ShowDialog() == true)
                {
                    using (var fs = new System.IO.FileStream(dialog.FileName, System.IO.FileMode.Open, FileAccess.Read))
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        //var stopwatch = new Stopwatch();
                        //stopwatch.Start();
                        string[] s0 = dialog.FileName.Split('\\');

                        if (Workspace != null)
                            if (Workspace.UniqueName == s0[s0.GetLength(0) - 1].Substring(0, s0[s0.GetLength(0) - 1].Length - 5)) { System.Windows.Forms.MessageBox.Show("This Model Is Already Loaded!"); goto exitProcedure; }

                        if (Workspace != null)
                            if (Workspace.IsOpen)
                                Workspace.Close();

                        Workspace = control.Workspaces.Add(Path.GetFileNameWithoutExtension(dialog.FileName), fs);

                        //sampleviewr

                        modelPath = dialog.FileName;                        
                        AppliIni.WriteValue("Last Model", "Full path", modelPath);

                        //ViDi2.Training.Local.LibraryAccess libraryAccess = new ViDi2.Training.Local.LibraryAccess();
                        //libraryAccess.TrainingWorkspaceManager.NewWorkspacePath.




                        string[] s = new string[0];
                        if (Workspace.Parameters.Description != "")
                            s = Workspace.Parameters.Description.Split(' ');
                        if (s.GetLength(0) > 0)
                        {
                            float f = System.Convert.ToSingle(s[s.GetLength(0) - 1]);
                            //lblToolBestLoss.Content = f.ToString("0.00");
                        }
                        else
                        {
                            //lblToolBestLoss.Content = "Unknown";
                        }

                        Mouse.OverrideCursor = null;
                    }
                }

                RaisePropertyChanged(nameof(Workspaces));

                //
                txtRunTimeWSName.Text = Workspace.DisplayName;
                stream = Workspace.Streams.First();
                string streamName = stream.Name;  // Workspace.Streams.First().Name;
                ITool tool = Workspace.Streams[streamName].Tools.First();
                txtToolName.Text = tool.Name;   // Workspace.Streams[streamName].Tools.First().Name;            
                txtToolType.Text = tool.Type.ToString();

                //class name display
                IRedTool tool1 = (IRedTool)stream.Tools.First();
                var knownClasses = tool1.KnownClasses;
                if (knownClasses.Count > 0)
                {
                    string className = knownClasses[0];
                    lblClassName.Text = className;
                }


            }
            catch(Exception e1)
            {
                System.Windows.Forms.MessageBox.Show("btnLoadModel_Click(), Error:" + e1.Message);
            }

        exitProcedure:;

            Mouse.OverrideCursor = null; // System.Windows.Input.Cursors.Arrow;


        }   
        private void btnDoEvaluation_Click(object sender, EventArgs e)
        {
            if (stream != null)
            {
                txt2.Visibility = Visibility.Hidden;
                //
                //ApplyROI(false);

                double ActualHeight = sampleviewr.sampleViewer.ActualHeight;
                double ActualWidth = sampleviewr.sampleViewer.ActualWidth;

                string imagePath = @"D:\Cognex\Proj_403\Standard Cognex\Images\defect_0001.jpg";

                string ImagePath = txtImagePath.Text.Trim();

                string ImageName = "";
                if (cmbImageName.SelectedIndex>-1)
                    ImageName = cmbImageName.Text;      //txtImageName.Text.Trim();
                else
                {
                    System.Windows.Forms.MessageBox.Show("Select an Image");
                    goto exitProcedure;
                }

                string fullPath = ImagePath + @"\" + ImageName;

                if (!File.Exists(fullPath)) { System.Windows.Forms.MessageBox.Show("Image or Path Don't Exist!,\n\r" + fullPath);  goto exitProcedure; }

                //string imagePath = @"S:\A03 - Tools\A03-HW-M70 Pressing.Systems\Public\SC End Mills Inspection\Yoav\Prog_9\002_06.00_009_3.jpg";

                this.Text = sversion + ", Current Image: " + fullPath;  /// imagePath;

                string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";

                IniFile AppliIni = new IniFile(INIpath);
                AppliIni.WriteValue("Last Image", "path", ImagePath);

                AppliIni.WriteValue("Last Image", "name", cmbImageName.Text.Trim());  // ImageName);
                
                AppliIni.WriteValue("Models", "path", txtModelsPath.Text.Trim());

                string modelName = txtRunTimeWSName.Text.Trim() + ".vrws";  // this.lstModels.SelectedItem.ToString();

                AppliIni.WriteValue("Models", "model", modelName);

                AppliIni.WriteValue("Last Model", "Full path", txtModelsPath.Text.Trim() + @"\" + modelName);

                var image = new ViDi2.UI.WpfImage(fullPath);   // imagePath);

                //
                Rectangle ImageDimensions = new Rectangle(0,0,image.Width,image.Height);
                ApplyROI(true, ImageDimensions);

                //--------------------------set thresholds--------------------------------
                IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;

                IRedTool hdRedTool = (IRedTool)Stream.Tools.First();

                double lower = 0.0;
                double upper = 0.0;
                if (IsNumeric(txtThresholdsLower.Text.Trim()))
                {
                    lower = System.Convert.ToDouble(txtThresholdsLower.Text.Trim());
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Check lower threshold, not a number!");
                    goto exitProcedure;
                }

                if (IsNumeric(txtThresholdsUpper.Text.Trim()))
                {
                    upper = System.Convert.ToDouble(txtThresholdsUpper.Text.Trim());
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Check upper threshold, not a number!");
                    goto exitProcedure;
                }


                bool xNotRedTool = false;
                if (hdRedParams == null) { xNotRedTool = true; }

                if (!xNotRedTool)
                {
                    ViDi2.Interval interval = new ViDi2.Interval(lower, upper);
                    hdRedParams.Threshold = interval;

                    //ViDi2.IParameters pb = Stream.Tools.First().ParametersBase;
                }

                

                //-----------------------------------process image----------------------------------
                SampleViewerViewModel.Sample = stream.Process(image);                               
                                         
                IMarkingOverlayViewModel mm =SampleViewerViewModel.MarkingModel;
                
                //Yoav 28-05-2023, get more information after process
                int numFrames = SampleViewerViewModel.Sample.Frames.Count;
                string sampleName = SampleViewerViewModel.Sample.Name;
                ViDi2.IMarking marking = SampleViewerViewModel.Marking;
                IMarkingOverlayViewModel markingmodel = SampleViewerViewModel.MarkingModel;

                System.Collections.ObjectModel.ReadOnlyCollection<ViDi2.IView> views = marking.Views;

                double duration = marking.Duration; //process time
                double durationPostProcess = marking.DurationPostProcess;
                double durationProcessOnly = marking.DurationProcessOnly;
                ViDi2.IImageInfo imageinfo = marking.ImageInfo;
                IEnumerable<ViDi2.IImageInfo> imageinfos = marking.ImageInfos;

                IEnumerable<ViDi2.ISetInfo> setinfo = marking.Sets;


                lblDuration.Text = duration.ToString("0.0000");

                lblProcessedSize.Text = marking.ImageInfo.Width.ToString() + "x" + marking.ImageInfo.Height;               

                bool xForDebug = false;
                if (xForDebug)
                {
                    saveMarkedImage();
                }

                var views01 = SampleViewerViewModel.Sample.Markings[SampleViewerViewModel.ToolName].Views;
                RegionFound[]  regions = RecordDefectsInfo(views01);

                ResultsFound resultsFound = new ResultsFound();
                resultsFound.regionFounds = regions;
                resultsFound.imageName = cmbImageName.Text.Trim();
                resultsFound.modelName = modelName.Substring(0, modelName.Length - 5);
                resultsFound.thresholdLow   = hdRedParams.Threshold.Lower.ToString("0.00");
                resultsFound.thresholdUpper = hdRedParams.Threshold.Upper.ToString("0.00");

                //roi
                resultsFound.roi.angle  = Convert.ToDouble(this.txtROIangle.Text.Trim());
                resultsFound.roi.rect.X = Convert.ToDouble(this.txtPProiPosX.Text.Trim());
                resultsFound.roi.rect.Y = Convert.ToDouble(this.txtPProiPosY.Text.Trim());
                resultsFound.roi.rect.Width  = Convert.ToDouble(this.txtPProiWidth.Text.Trim());
                resultsFound.roi.rect.Height = Convert.ToDouble(this.txtPProiHeight.Text.Trim());
                resultsFound.roi.xROIused = Convert.ToBoolean(this.chkUsePPevaluationROI.Checked);

                //try to write text on image
                bool xNotWorking = true;
                if (!xNotWorking)
                {
                    ViDi2.IImage I = SampleViewerViewModel.Sample.Image;

                    using (Graphics g = Graphics.FromImage(SampleViewerViewModel.Sample.Image.Bitmap))  //using (Graphics g = Graphics.FromImage(I.Bitmap))
                    {

                        string s = "1";
                        string sDisp = "MN " + s;
                        float fontsize = 10.0f;

                        Brush bb = Brushes.Red;

                        float x = 500.0f;
                        float y = 500.0f;
                        g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Regular), bb, x, y);


                    }
                }

                //
                LogResults(resultsFound);
                //
                //Results2Lst(resultsFound,lstResults);

                Results2Lst(resultsFound, lststdResults);



                RaisePropertyChanged(nameof(ViewIndices));

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Load Runtime Model First, stream = null");
            }

         exitProcedure:;
        }
        private void Results2Lst(ResultsFound resultsFound,ListBox lst)
        {
            lst.Items.Clear();

            lst.Height = resultsFound.regionFounds.GetLength(0)*20;
            
            //System.Windows.Media.BrushConverter converter = new System.Windows.Media.BrushConverter();
            //System.Windows.Media.Brush brush = (System.Windows.Media.Brush)converter.ConvertFromString("#FFFFFF90");
            
            //lst.Background = brush;

            foreach (RegionFound item in resultsFound.regionFounds)
            {
                string sLoadList = "class name: " + item.className
                                   + ", score: "  + item.score.ToString("0.00")
                                   + ", size: " + item.width.ToString() + ", " + item.height.ToString()
                                   + ", area: " + item.area.ToString("0")
                                   + ", perimeter: " + item.perimeter.ToString("0")
                                   + ", center: " + item.center.X.ToString("0") + ", " + item.center.Y.ToString("0")

                    ;

                lst.Items.Add(sLoadList);
            }

            if (resultsFound.regionFounds.GetLength(0) == 0)
                lst.Visible = false;
            else
                lst.Visible = true;

        }        
        private void saveMarkedImage()
        {
            
            ViDi2.IImage I = SampleViewerViewModel.Sample.OverlayImage();
            Bitmap bb = I.Bitmap;
            string savePath = @"C:\ProgramData\Cognex\VisionPro Deep Learning\2.1\Examples\c#\projSampaleViewer\bin\x64\Debug\Data\test1.jpg";
            bb.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            I.Dispose();
            bb.Dispose();
        }
        private RegionFound[] RecordDefectsInfo(System.Collections.ObjectModel.ReadOnlyCollection<ViDi2.IView> views)
        {
            //var views01 = SampleViewerViewModel.Sample.Markings[SampleViewerViewModel.ToolName].Views;

            ViDi2.IRedView redview = (ViDi2.IRedView)views[0];

            RegionFound[] regionFound = new RegionFound[redview.Regions.Count];

            IRedTool tool = (IRedTool)Stream.Tools.First();

            var knownClasses = tool.KnownClasses;
            string className = knownClasses[0];
            string[] s = className.Split('_');
            string cn = "";
            if (s.GetLength(0) > 0)
            {
                if (s.GetLength(0) > 1)
                    cn = s[1];
                else
                    cn = s[0];
            }


            int index = 0;
            foreach (ViDi2.IRegion item in redview.Regions)
            {
                regionFound[index].area = item.Area;
                regionFound[index].width = item.Width;
                regionFound[index].height = item.Height;
                regionFound[index].center = item.Center;
                regionFound[index].score = item.Score;
                regionFound[index].className = cn;  // item.Name; region name
                regionFound[index].classColor = item.Color;
                regionFound[index].compactness = item.Compactness;
                regionFound[index].covers = item.Covers;
                regionFound[index].outer = item.Outer;
                regionFound[index].perimeter = item.Perimeter;                

                index++;
            }                        

            return regionFound;

        }
        private void LogResults(ResultsFound resultsFound)
        {
            //log: date, image name, score, size, width, height, area, defect name (class), model file name, roi, image size
            string timenow1 = string.Format("{0:dd-MM-yyy, HH:mm:ss}", DateTime.Now);

            string string1 = timenow1;
            string string2 = ",image name," + resultsFound.imageName            + ",model name,"
                            + resultsFound.modelName + ".vrws"
                            + ", Thresholds," + resultsFound.thresholdLow + "," + resultsFound.thresholdUpper
                            + ", roi used,"   + resultsFound.roi.xROIused.ToString().ToLower()
                            + ", angle,"      + resultsFound.roi.angle.ToString("0.00")
                            + ", x,"          + resultsFound.roi.rect.X.ToString("0.00")
                            + ", y,"          + resultsFound.roi.rect.Y.ToString("0.00")
                            + ", width,"      + resultsFound.roi.rect.Width.ToString("0.00")
                            + ", height,"     + resultsFound.roi.rect.Height.ToString("0.00")

                            ;

            //resultsFound.regionFounds
            int i = 0;
            string[] saveString = new string[resultsFound.regionFounds.GetLength(0)];
            if (resultsFound.regionFounds.GetLength(0) == 0)
            {
                //saveString = new string[1]; saveString[0] =  "------------------------------no defects found-----------------------------"; i = 1;
                saveString = new string[1]; saveString[0] =  string2;
            }

            
            foreach(RegionFound item in resultsFound.regionFounds)
            {
               
               saveString[i] =  "class name,D" + item.className +  ",score," + item.score.ToString("0.00")  + ",cnter," + item.center.X.ToString("0") + "," + item.center.Y.ToString("0") + ",area," + item.area.ToString("0") + ",width," + item.width.ToString() + ",height," + item.height.ToString()
                                
                    ;
                               
                i++;
            }
            

            //compile answer
            string stringAnswer = string2 + "\n,,";
            for(int k=0; k<i;k++)
            {
                if (i - k > 1) { stringAnswer = stringAnswer + saveString[k] + "\n,,"; }
                else { stringAnswer = stringAnswer + saveString[k]; }

            }

            string timenow2 = string.Format("{0:ddMMyy_HHmmss}", DateTime.Now); ;
            string fileName = "Defect Found Report_" + timenow2 + ".csv";


            string2 = stringAnswer;
            LogDefectsLines(string1, string2, fileName);
        }                
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        void sampleViewer_ToolSelected(ViDi2.ITool tool)
        {
            RaisePropertyChanged(nameof(SampleViewer));
        }
        private void showUnshowMarkingTSMI01_Click(object sender, EventArgs e)
        {
            if (SampleViewerViewModel.HideOverlay)
                SampleViewerViewModel.HideOverlay = false;
            else
                SampleViewerViewModel.HideOverlay = true;
        }
        private void btnIMGcomboRefresh_Click(object sender, EventArgs e)
        {

            string[] dirImages = Directory.GetFiles(txtImagePath.Text.Trim());

            string[] fileName = new string[dirImages.GetLength(0)];


            if(dirImages.GetLength(0)>0)
                cmbImageName.Items.Clear();
            else
            {
                System.Windows.Forms.MessageBox.Show("No Imagers In Path: \n\r" + txtImagePath.Text.Trim());
                goto exitProcerdure;
            }

            int i = 0;
            //only image name
            foreach (string item in dirImages)
            {
                cmbImageName.Items.Add(Path.GetFileName(item));
                fileName[i] = Path.GetFileName(item);
                i++;
            }

            //int index = cmbImageName.Items.IndexOf(0);
            if(cmbImageName.Items.Count>0)
                cmbImageName.SelectedIndex = 0;

            string path = System.Windows.Forms.Application.StartupPath + @"\Data\FilesName.csv";
            SaveTextToFile(fileName, path, FileMode.CreateNew);


            exitProcerdure:;
        }
        private void lstModels_SelectedIndexChanged(object sender, EventArgs e)
        {

            int ii = lstModels.SelectedIndex;
            //string item = sender.ToString();
            //int selectedindex = lstModels.Items.IndexOf("fgh");

            if (ii > -1)
            {
                Workspace = rwsl[rwsl.Names[ii]];
                
                txtRunTimeWSName.Text = Workspace.DisplayName;
                stream = Workspace.Streams.First();
                string streamName = stream.Name;  // Workspace.Streams.First().Name;
                ITool tool = Workspace.Streams[streamName].Tools.First();
                txtToolName.Text = tool.Name;   // Workspace.Streams[streamName].Tools.First().Name;            
                txtToolType.Text = tool.Type.ToString();
                txtLastModified.Text = Workspace.LastModified.ToShortDateString();

                //class name display
                IRedTool tool1 = (IRedTool)stream.Tools.First();
                var knownClasses = tool1.KnownClasses;
                if (knownClasses.Count > 0)
                {
                    string className = knownClasses[0];
                    lblClassName.Text = className;
                }

                //get thresholds tool1                
                IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;
               
                ViDi2.Interval interval = hdRedParams.Threshold;

                //txtThresholdsLower.Text = interval.Lower.ToString("0.00");
                //txtThresholdsUpper.Text = interval.Upper.ToString("0.00");

            } 

        }
        private void updateCurrentModelThresholdsTSMI2_Click(object sender, EventArgs e)
        {
            //update thresholds tool1                
            IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;

            IRedTool hdRedTool = (IRedTool)Stream.Tools.First();

            ViDi2.Interval interval = new ViDi2.Interval();

            double lower = 0.0;
            double upper = 0.0;

            if (IsNumeric(txtThresholdsLower.Text.Trim()))
            {
                lower = System.Convert.ToDouble(txtThresholdsLower.Text.Trim());
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Check lower threshold, not a number!");
                goto exitProcedure;
            }

            if (IsNumeric(txtThresholdsUpper.Text.Trim()))
            {
                upper = System.Convert.ToDouble(txtThresholdsUpper.Text.Trim());
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Check upper threshold, not a number!");
                goto exitProcedure;
            }


            bool xNotRedTool = false;
            if (hdRedParams == null) { xNotRedTool = true; }

            if (!xNotRedTool)
            {
                interval = new ViDi2.Interval(lower, upper);
                hdRedParams.Threshold = interval;

            }


            IniFile AppliIni = new IniFile(INIpath);
            AppliIni.WriteValue("Thresholds", "min.", interval.Lower.ToString());
            AppliIni.WriteValue("Thresholds", "max.", interval.Upper.ToString());


        exitProcedure:;

        }
        private void btnViewResults_Click(object sender, EventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".csv",
                Filter = "Cognex Deep Learning Studio Runtime Workspaces (*.csv)|*.CSV",
                ValidateNames = false,
                InitialDirectory = System.Windows.Forms.Application.StartupPath + @"\Data\Defects Reports"

            };

            if ((bool)dialog.ShowDialog() == true)
            {

                string path = dialog.FileName;
                if (File.Exists(path))
                    System.Diagnostics.Process.Start(path);
                else
                    System.Windows.Forms.MessageBox.Show("File Don't Exist!");

            }
        }
        private void btnAddROI_Click(object sender, EventArgs e)
        {
            btnGetRecInfo.Enabled = true;

            RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxEditRoi, gcurrentImage);

            float ratioX = ratioxy.ratioX;
            float ratioY = ratioxy.ratioY;

            //if roi stored in ini use it for new roi
            string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
            IniFile AppliIni = new IniFile(INIpath);
            Rect rect = new Rect();
            
            rect.X = Convert.ToDouble(AppliIni.ReadValue("roi", "x", "0"));
            rect.Y = Convert.ToDouble(AppliIni.ReadValue("roi", "y", "0"));
            rect.Width = Convert.ToDouble(AppliIni.ReadValue("roi", "width", "0"));
            rect.Height = Convert.ToDouble(AppliIni.ReadValue("roi", "height", "0"));

            StaticROI = new RectangleResizeAndRotateAdd(Convert.ToSingle(rect.X*(1/ratioX)), Convert.ToSingle(rect.Y * (1 / ratioY)), Convert.ToSingle(rect.Width * (1 / ratioX)), Convert.ToSingle(rect.Height * (1 / ratioY)));

            //recAdd[bboxIndex].useAsROI = "roi";
            StaticROI.SetPictureBox(this.pbxEditRoi);

            //this.pbxEditRoi.Refresh();
            

        }        
        private void btnApplyROI_Click(object sender, EventArgs e)
        {
            GetROIinfo();

            Rectangle ImageDimensions = new Rectangle();
            ApplyROI(true, ImageDimensions);
        }
        private void btnClearROI_Click(object sender, EventArgs e)
        {
            StaticROI.DisEngage();
            StaticROI.ClearGraphics(true);
            //pbxEditRoi.Refresh();

        }
        private void btnGetRecInfo_Click(object sender, EventArgs e)
        {            
            GetROIinfo();
        }
        private void SaveROI_OnClick(object sender, RoutedEventArgs e)
        {
        //    if (IsNumeric(txtRoiX.Text.Trim()))
        //        currentROI.X = Convert.ToDouble(txtRoiX.Text.Trim());
        //    else
        //    {
        //        MessageBox.Show("Error: Check ROI.X value Not Numeric! " + txtRoiX.Text.Trim());
        //        goto exitProcedure;
        //    }

        //    if (IsNumeric(txtRoiY.Text.Trim()))
        //        currentROI.Y = Convert.ToDouble(txtRoiY.Text.Trim());
        //    else
        //    {
        //        MessageBox.Show("Error: Check ROI.Y value Not Numeric! " + txtRoiY.Text.Trim());
        //        goto exitProcedure;
        //    }

        //    if (IsNumeric(txtRoiWidth.Text.Trim()))
        //        currentROI.Width = Convert.ToDouble(txtRoiWidth.Text.Trim());
        //    else
        //    {
        //        MessageBox.Show("Error: Check ROI.Width value Not Numeric! " + txtRoiWidth.Text.Trim());
        //        goto exitProcedure;
        //    }


        //    if (IsNumeric(txtRoiHeight.Text.Trim()))
        //        currentROI.Height = Convert.ToDouble(txtRoiHeight.Text.Trim());
        //    else
        //    {
        //        MessageBox.Show("Error: Check ROI.Height value Not Numeric! " + txtRoiHeight.Text.Trim());
        //        goto exitProcedure;
        //    }




        //    string INIpath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Data\ProjINI.ini";
        //    IniFile AppliIni = new IniFile(INIpath);


        //    AppliIni.WriteValue("roi", "x", currentROI.X.ToString());
        //    AppliIni.WriteValue("roi", "y", currentROI.Y.ToString());
        //    AppliIni.WriteValue("roi", "width", currentROI.Width.ToString());
        //    AppliIni.WriteValue("roi", "height", currentROI.Height.ToString());

        //exitProcedure:;

        }
        private void chkUsePPevaluationROI_CheckedChanged(object sender, EventArgs e)
        {
            Rectangle ImageDimensions = new Rectangle();
            if (chkUsePPevaluationROI.Checked && !xNoAction)
                ApplyROI(false, ImageDimensions);
        }
        private void sampleViewer_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int d = 1;

            ViDi2.UI.SampleViewer o = (ViDi2.UI.SampleViewer)e.Source;           

            System.Windows.Size DesiredSize = o.DesiredSize;

            lblTest5.Text = DesiredSize.Width.ToString();
            lblTest6.Text = DesiredSize.Height.ToString();

            if (lblTest4.Text != "1")
                lblTest4.Text = "1";
            else
                lblTest4.Text = "0";


            var to = sampleviewr.sampleViewer.TouchesOver;

            var trig = sampleviewr.sampleViewer.Triggers;

            System.Windows.Size size01 = sampleviewr.sampleViewer.RenderSize;

            lblTest2.Text = size01.Width.ToString();
            lblTest3.Text = size01.Height.ToString();

            var cm = sampleviewr.sampleViewer.ContextMenu;

            //var vm = sampleviewr.sampleViewer.ViewModel;


        }
        private void saveImageWithMarkingTSMI1_Click(object sender, EventArgs e)
        {
            //System.Windows.Media.Brush br = sampleviewr.sampleViewer.Background;

            //br.Freeze();
            //sampleviewr.sampleViewer.Background = br;

            //ISampleViewerViewModel sm = SampleViewerViewModel;

            //ViDi2.IImage I00 = sm.Sample.OverlayImage();


            ViDi2.IImage I = SampleViewerViewModel.Sample.OverlayImage();
            Bitmap bb = I.Bitmap;
            string savePath = System.Windows.Forms.Application.StartupPath + @"\Data\LastImageMarking.jpg";
            bb.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            I.Dispose();
            bb.Dispose();


            //works, image with no mrking
            bool xNotUsed = true;
            if (!xNotUsed)
            {
                ViDi2.IMarking marking = SampleViewerViewModel.Marking;


                System.Windows.Media.SolidColorBrush myBrush = new System.Windows.Media.SolidColorBrush();


                //marking.OverlayImage.BackColor = 

                System.IO.Stream stream = new System.IO.MemoryStream();
                marking.ViewImage(0).Save(stream, ViDi2.ImageFormat.JPEG);
                System.Drawing.Bitmap btm01 = new Bitmap(stream);
                btm01.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                btm01.Dispose();


                //System.Windows.Controls.ContentControl I00 = marking.OverlayDrawing();



                //not working freez error, try to do a work over a thread boundary
                ViDi2.IImage I01 = marking.OverlayImage();  // ViewImage(0);
                Bitmap btm = I01.Bitmap;
                btm.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                btm.Dispose();
            }


        }
        private void btnEditROI_Click(object sender, EventArgs e)
        {
            try
            {
                if (SampleViewerViewModel.Sample.Image != null)
                {

                }

                const string stdmodeCoption = "EDIT ROI";
                const string editmodeCoption = "ROI MODE";
                if (this.btnEditROI.Text == stdmodeCoption)
                {
                    //---------------------edit mode---------------

                    StaticROI.DisEngage();
                    StaticROI.ClearGraphics(true);

                    LoadImage();
                    this.btnEditROI.Text = editmodeCoption;
                    elementHost1.Visible = false;
                    this.pbxEditRoi.Visible = true;
                    this.pbxEditRoi.BringToFront();
                    //
                    btnAddROI.Enabled = true;

                    //debug
                    //Rectangle rect = new Rectangle(60, 70, 300, 200);

                    using (Graphics g = Graphics.FromImage(this.pbxEditRoi.Image))
                    {


                        //debug
                        //Color cc = Color.Green;
                        //g.DrawRectangle(new Pen(cc, 3.0f), rect);

                        //string s = "1";
                        string sDisp = "";
                        sDisp = "ROI EDIT MODE";

                        float fontsize = 24.0f;

                        Brush bb = Brushes.Red;

                        float x = 40.0f;
                        float y = 25.0f;
                        g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Bold), bb, x, y);
                    }

                    ROIeditMode();
                }
                else //standard mode
                {
                    this.btnEditROI.Text = stdmodeCoption;
                    this.pbxEditRoi.Visible = false;
                    elementHost1.Visible = true;
                    this.pbxEditRoi.SendToBack();

                    this.pbxEditRoi.Image.Dispose();
                    this.pbxEditRoi.Image = null;

                    //
                    btnAddROI.Enabled = false;

                    //
                    GetROIinfo();

                }

                                
            }
            catch (NullReferenceException e1)
            {
                string message = e1.Message;
                System.Windows.Forms.MessageBox.Show("Processed first image first");
            }
        }
        private void pbxEditRoi_Paint(object sender, PaintEventArgs e)
        {
            iCountPain++;
            System.Diagnostics.Debug.Print(iCountPain.ToString() + " Debug.Print: pbxShowImage_Paint()");
        }
        private void cmbImageName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox lst = (ComboBox)sender;

            imgLstIndex.Text = (lst.SelectedIndex + 1).ToString();
        }
        private void btnResetZoom_Click(object sender, EventArgs e)
        {
            sampleviewr.sampleViewer.ViewModel.ResetZoom();
        }
        private void copyRoiToClipboardTSMI3_Click(object sender, EventArgs e)
        {
            string PProiPosX   = txtPProiPosX.Text.Trim();
            string PProiPosY   = txtPProiPosY.Text.Trim();
            string PProiWidth  =  txtPProiWidth.Text.Trim();
            string PProiHeight = txtPProiHeight.Text.Trim();
            string ROIangle    = txtROIangle.Text.Trim();         
            string RatioImage2ROI = txtRatioImage2ROI.Text.Trim();

            System.Windows.Forms.Clipboard.Clear();
            System.Windows.Forms.Clipboard.SetText(  "PosX            = " + PProiPosX      + "\n\r"
                                                   + "PosY            = " + PProiPosY      + "\n\r"
                                                   + "Width           = " + PProiWidth     + "\n\r"
                                                   + "Height          = " + PProiHeight    + "\n\r"
                                                   + "Angle           = " + ROIangle       + "\n\r"
                                                   + "Ratio Image ROI = " + RatioImage2ROI + "\n\r"
                );

        }
        private void btnROIapplyBatch_Click(object sender, EventArgs e)
        {
            //1. get roi
            //2. save to batch file

            this.GetROIinfo();
            Rectangle ImageDimensions = new Rectangle();
            SaveROIBatch(true, ImageDimensions);
        }

        #endregion

        #region--------------------Utilities Methods ------------------------
        private static bool SaveTextToFile(string[] sString, string path, FileMode fileMode)
        {
            bool xError = true;
            int iItems = sString.Length;
            try
            {
                // Open the stream and write to it (overwrite old file). 
                using (FileStream fs = File.Open(path, fileMode))
                {
                    for (int i = 0; i < iItems; i++)
                    {
                        Byte[] info =
                        new UTF8Encoding(true).GetBytes(string.Concat(sString[i], "\r\n"));
                        // write information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                    fs.Close();
                }

                xError = false;
            }
            catch (Exception e)
            {
                xError = true;
                Console.WriteLine("SaveTextToFile(), Error saving barcode to file, " + e.Message, "Error!");
            }


            return xError;
        }
        private static bool IsNumeric(string InputString)
        {
            bool xreturn = false;
            int iNum;
            float fNum;

            ////try integer
            bool IsNumeric = int.TryParse(InputString, out iNum);
            //if integer not then try float
            if (!IsNumeric) { IsNumeric = float.TryParse(InputString, out fNum); }


            if (IsNumeric) { xreturn = true; } else { xreturn = false; }
            return xreturn;
        }
        private frmMain.RatioXratioY GetRatioXandRetioY(PictureBox pbxShowImage, CurrentImage currentImage)
        {
            frmMain.RatioXratioY ratioXY = new frmMain.RatioXratioY();
            if(pbxShowImage.Image !=null)
            {
                int imgX = pbxShowImage.Image.Width;
                int imgY = pbxShowImage.Image.Height;

                float ratioX = currentImage.imgWidth / Convert.ToSingle(imgX);
                float ratioY = currentImage.imgHeight / Convert.ToSingle(imgY);
                currentImage.ratioX = ratioX;
                currentImage.ratioY = ratioY;

                ratioXY.ratioX = ratioX;
                ratioXY.ratioY = ratioY;
            }
            return ratioXY;
        }

        #endregion
        
        private void btnTest1_Click(object sender, EventArgs e)
        {
            double ActualWidth = sampleviewr.sampleViewer.ActualWidth;
            double ActualHeight = sampleviewr.sampleViewer.ActualHeight;

            string ss = sampleviewr.sampleViewer.ViewModel.ToString();

            ISampleViewerView vs = sampleviewr.sampleViewer.ViewModel.View;

            lblTest1.Text = ActualWidth.ToString();
            lblTest2.Text = sampleviewr.sampleViewer.Width.ToString();
            lblTest3.Text = "";
            lblTest4.Text = "";
            lblTest5.Text = "";
            lblTest6.Text = "";
        }

        private void resetROIBatchFileTSMI4_Click(object sender, EventArgs e)
        {
            DialogResult dr = System.Windows.Forms.MessageBox.Show("Are you sure you want to reset roi batch file?","",MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string timenow12 = string.Format("{0:ddMMyy_HHmmss}", DateTime.Now);

                batchFirst = false;
                string sourceFile = pathROIbatch;
                string pp = timenow12 + "_" + Path.GetFileName(pathROIbatch);
                string pp00  = Path.GetDirectoryName(pathROIbatch); //path only                

                string destination = pp00 + @"\" + pp;
                File.Copy(sourceFile, destination);
                File.Delete(sourceFile);

                iImageIndex = 0;

            }
        }
    }
}
