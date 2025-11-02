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
using System.ServiceModel.Syndication;
using ViDi2;
using System.Collections.ObjectModel;
using System.Reflection;
using OxyPlot;
using System.Drawing.Imaging;
using SuaKIT_SegmentationEvaluator;
namespace projSampaleViewer
{
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
            InitializeComponent();
        }
        private Vector3 currentPosition;
        private float DPI
        {
            get
            {
                using (var g = CreateGraphics())
                    return g.DpiX;
            }
        }
        //private Vector3 PointToCartesian(System.Drawing.Point point)
        //{
        //    return new Vector3(Pixel_to_Mn(point.X), Pixel_to_Mn(pictureBox1.Height - point.Y));
        //}

        private float Pixel_to_Mn(float pixel)
        {
            return pixel * 25.4f / DPI;
        }

        private float Mm_to_Pixel(float Mm)
        {
            return Mm*DPI/25.4f;
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
            public Image zoomSnap1;
            public Image zoomSnap2;
            public Image zoomSnap3;
            public Image zoomSnap4;
            public Image zoomSnap5;
            public Image imgActiveImage;
            public string LocalLastImagePath;
            public string ImageFileType;

        }
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
            public ViDi2.Rect rect;
            public double angle;
            public bool xROIused;
        }
        public struct FilterEvalOutput
        {
            public string AreaWidth;
            public string AreaHeight;
            public string Score;
            public string FilterUsed;

        }
        public struct JsonLblData
        {
            public int[][,] ijCoordinates;
            public byte[] bClasses;

        }
        public struct Rect
        {
            public int ClassID;
            public int width;
            public int height;
            public int x;
            public int y;
            public double prob;
            public double uncert;
            public bool xPassedAllTests;

        }
        public struct Results
        {
            public JsonLblData[] Contours;
            public double[] fProb;
            public double[] fUncert;
            public int iClassID;
            public string sDefectID;
            public Rect[] rect;
            public bool[] xInSearchRegion;
            public int[] iRegionNumber;

            public bool[] xInternalInSearchRegion;
            public int[] iInternalRegionNumber;


        }
        public struct ResultsStats
        {
            public double[] ProbAll;
            public double[] UncertAll;

            public double ProbAverage;
            public double UncertAverage;
            public double ProbStdDev;
            public double UncertStdDev;

            public double ProbMedian;
            public double UncertMedian;

            public double ProbMin;
            public double UncertMin;
            public double ProbMax;
            public double UncertMax;

            public int iSampleSize;
        }
        public struct ResultsClass
        {
            public Results[] ResultsPerClass;
            public string ResultsPath;
            public string ImageName;
            public string CatalogNumber;
            public ResultsStats[] resultsStatsPerClass;
            public string[] searchRegions;
            public RectangleResizeAndRotateAdd.RegionEvalResultsPerClass[] RegionalSearchResults;
        }

        public struct DataForDisplayDefects
        {

            public List<int> indexesToDisplay;
            public List<string> sProbUncert;
            public int selectedContour;
            public List<double> dProbSort;
            public List<double> dUncertSort;
            public List<string> sInSearchRegion;

            public string[] sProbUncertList;
            public string[] sInSearchRegionList;
            public int[] OverLappedIndexes;
            public bool BeenUsed;
            public int[] NumContoursPerClass;


        }
        //private System.Windows.Forms.Integration.ElementHost elementHost1;
        ViDiSuiteServiceLocator viDiSuiteServiceLocator = new ViDiSuiteServiceLocator();
        System.Windows.Controls.UserControl cc = new System.Windows.Controls.UserControl();
        Func<int, int> square = x => x * x;
        ViDi2.Runtime.IControl control;
        ViDi2.Runtime.IWorkspace workspace;
        ViDi2.Runtime.IStream stream;
        ViDi2.Runtime.IStream[] streamArr;
        ViDi2.Runtime.IControl control1;
        ViDi2.Runtime.IWorkspace workspace1;
        ViDi2.Runtime.IStream stream1;
        ViDi2.Runtime.IStream[] streamArr1;
        private ucSampleViewerClass sampleviewr = new ucSampleViewerClass();
        private string sversion = "Sample Viewer 07-12-2023";
        private string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
        private ViDi2.Runtime.IWorkspaceList rwsl;
        public static RectangleResizeAndRotateAdd StaticROI = new RectangleResizeAndRotateAdd();
        private System.Windows.Rect currentROI = new System.Windows.Rect();
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
        private const string stdmodeCaption = "EDIT ROI";
        private const string editmodeCaption = "ROI MODE";
        private FilterEvalOutput gfilterEvalOutput = new FilterEvalOutput();
        public ISampleViewerViewModel SampleViewerViewModel => sampleviewr.sampleViewer.ViewModel;
        RegionFound[] CurrentRegion;
        int RegCnt;
        public static CurrentImage currentImage = new CurrentImage();
        private ImageCodecInfo jgpEncoder;
        private EncoderParameters myEncoderParameters;
        private string defaultImagePath;
        private int PicBoxWith;
        private int PicBoxHeight;
        ExportResults.ResultsAndInfo resultsEvluator = new ExportResults.ResultsAndInfo();
        Image regionPic;
        Image unsignImage;
        Image signImage;
        int currentZoom;
        private void initProporties()
        {
            ViDi2.Runtime.IStream stream = Stream;
            ViDi2.Runtime.IWorkspace workspace = Workspace;
            ViDi2.Runtime.IControl control = Control;
            IList<ViDi2.Runtime.IWorkspace> ws = Workspaces;
            Dictionary<int, string> dic = ViewIndices;

            //SampleViewerViewModel.

        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public ViDi2.Runtime.IStream Stream
        {
            get { return stream; }
            set
            {
                stream = value;
                SampleViewerViewModel.Sample = null;
                RaisePropertyChanged(nameof(Stream));
            }
        }
        public ViDi2.Runtime.IStream Stream1
        {
            get { return stream1; }
            set
            {
                stream1 = value;
                SampleViewerViewModel.Sample = null;
                RaisePropertyChanged(nameof(Stream1));
            }
        }
        public ViDi2.Runtime.IWorkspace Workspace
        {
            get { return workspace; }
            set
            {
                workspace = value;
                Stream = workspace.Streams.First();
                RaisePropertyChanged(nameof(Workspace));
            }
        }
        public ViDi2.Runtime.IWorkspace Workspace1
        {
            get { return workspace1; }
            set
            {
                workspace1 = value;
                Stream = workspace1.Streams.First();
                RaisePropertyChanged(nameof(Workspace1));
            }
        }
        public ViDi2.Runtime.IControl Control
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
        public ViDi2.Runtime.IControl Control1
        {
            get { return control1; }
            set
            {
                control1 = value;
                RaisePropertyChanged(nameof(Control1));
                RaisePropertyChanged(nameof(Workspaces1));  //;
                RaisePropertyChanged(nameof(Stream1));
            }
        }
        public IList<ViDi2.Runtime.IWorkspace> Workspaces => Control.Workspaces.ToList();
        public IList<ViDi2.Runtime.IWorkspace> Workspaces1 => Control1.Workspaces.ToList();
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

                        if (Workspace != null)
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
                //txtRunTimeWSName.Text = Workspace.DisplayName;
                stream = Workspace.Streams.First();
                //create new stream
               //ViDi2.IStream SS = Workspace.Streams["ModelName"];

                string streamName = stream.Name;  // Workspace.Streams.First().Name;
                ViDi2.Runtime.ITool tool = Workspace.Streams[streamName].Tools.First();
                //txtToolName.Text = tool.Name;   // Workspace.Streams[streamName].Tools.First().Name;            
                //txtToolType.Text = tool.Type.ToString();

                //class name display
                ViDi2.Runtime.IRedTool tool1 = (ViDi2.Runtime.IRedTool)stream.Tools.First();
                var knownClasses = tool1.KnownClasses;
                if (knownClasses.Count > 0)
                {
                    string className = knownClasses[0];
                   // lblClassName.Text = className;
                }

                //will activate event on lst
                string fn = Path.GetFileName(modelPath);
                int index = this.lstModels.Items.IndexOf(fn);
                this.lstModels.SelectedIndex = index;

                //var si = this.lstModels.SelectedItem;

            }
            catch (ViDi2.Exception e)
            {
                System.Windows.Forms.MessageBox.Show("loadModel(), Error: " + e.Message);
            }

            exitProcedure:;

        }


        private void OverLayLabelRecOnly(ResultsClass results, DataForDisplayDefects DataForObj, int[] NumContoursPerClass) //List<int> indexesToDisplay)
        {
            
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

        private void btnDefect_Click(object sender, EventArgs e)
        {
             if (stream != null)
            {
            string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
            string[] ModelsNames;
            string modelName = txtRunTimeWSName.Text.Trim() + ".vrws";  // this.lstModels.SelectedItem.ToString();
            double ActualHeight = sampleviewr.sampleViewer.ActualHeight;
            double ActualWidth = sampleviewr.sampleViewer.ActualWidth;
            string imagePath = @"S:\A03 - Tools\A03-HW-M70 Pressing.Systems\Public\SC End Mills Inspection\Data labeling\images\round light 28.06.23\002 defect";

            string ImagePath = txtImagePath.Text.Trim();
            string ImageName = "";

            if (cmbImageName.SelectedIndex > -1)
                ImageName = cmbImageName.Text;      //txtImageName.Text.Trim();
            else
            {
                System.Windows.Forms.MessageBox.Show("Select an Image");
                goto exitProcedure;
            }
            string fullPath = ImagePath + @"\" + ImageName;

            if (!File.Exists(fullPath)) { System.Windows.Forms.MessageBox.Show("Image or Path Don't Exist!,\n\r" + fullPath); goto exitProcedure; }
            this.Text = sversion + ", Current Image: " + fullPath;  /// imagePath;


            IniFile AppliIni = new IniFile(INIpath);
            AppliIni.WriteValue("Last Image", "path", ImagePath);

            AppliIni.WriteValue("Last Image", "name", cmbImageName.Text.Trim());  // ImageName);

            AppliIni.WriteValue("Models", "path", txtModelsPath.Text.Trim());


            AppliIni.WriteValue("Models", "model", modelName);

            AppliIni.WriteValue("Last Model", "Full path", txtModelsPath.Text.Trim() + @"\" + modelName);

            var image = new ViDi2.UI.WpfImage(fullPath);   // imagePath);

           //--------------------------set thresholds--------------------------------
            IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;

            ViDi2.Runtime.IRedTool hdRedTool = (ViDi2.Runtime.IRedTool)Stream.Tools.First();

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

            IMarkingOverlayViewModel mm = SampleViewerViewModel.MarkingModel;
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
            RegionFound[] regions = RecordDefectsInfo(views01);

            ResultsFound resultsFound = new ResultsFound();
            resultsFound.regionFounds = regions;
            resultsFound.imageName = cmbImageName.Text.Trim();
            resultsFound.modelName = modelName.Substring(0, modelName.Length - 5);
            resultsFound.thresholdLow = hdRedParams.Threshold.Lower.ToString("0.00");
            resultsFound.thresholdUpper = hdRedParams.Threshold.Upper.ToString("0.00");

            //roi
            //resultsFound.roi.angle = Convert.ToDouble(this.txtROIangle.Text.Trim());
            //resultsFound.roi.rect.X = Convert.ToDouble(this.txtPProiPosX.Text.Trim());
            //resultsFound.roi.rect.Y = Convert.ToDouble(this.txtPProiPosY.Text.Trim());
            //resultsFound.roi.rect.Width = Convert.ToDouble(this.txtPProiWidth.Text.Trim());
            //resultsFound.roi.rect.Height = Convert.ToDouble(this.txtPProiHeight.Text.Trim());
            //resultsFound.roi.xROIused = Convert.ToBoolean(this.chkUsePPevaluationROI.Checked);

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
                LogResults(resultsFound);
                //
                //Results2Lst(resultsFound,lstResults);

                Results2Lst(resultsFound, lststdResults);



                RaisePropertyChanged(nameof(ViewIndices));

            }


            exitProcedure:;
        }
        private void Results2Lst(ResultsFound resultsFound, ListBox lst)
        {
            lst.Items.Clear();

            //lst.Height = resultsFound.regionFounds.GetLength(0)*20;

            //System.Windows.Media.BrushConverter converter = new System.Windows.Media.BrushConverter();
            //System.Windows.Media.Brush brush = (System.Windows.Media.Brush)converter.ConvertFromString("#FFFFFF90");

            //lst.Background = brush;
            int i = 1;
            foreach (RegionFound item in resultsFound.regionFounds)
            {
                string sLoadList = i.ToString() + " "
                                   + "class name: " + item.className
                                   + ", score: " + item.score.ToString("0.00")
                                   + ", size: " + item.width.ToString() + ", " + item.height.ToString()
                                   + ", area: " + item.area.ToString("0")
                                   + ", perimeter: " + item.perimeter.ToString("0")
                                   + ", center: " + item.center.X.ToString("0") + ", " + item.center.Y.ToString("0")

                    ;

                lst.Items.Add(sLoadList);
                i++;
            }

            if (resultsFound.regionFounds.GetLength(0) == 0)
                lst.Visible = false;
            else
                lst.Visible = true;

        }
        private void LogResults(ResultsFound resultsFound)
        {
            //log: date, image name, score, size, width, height, area, defect name (class), model file name, roi, image size
            string timenow1 = string.Format("{0:dd-MM-yyy, HH:mm:ss}", DateTime.Now);

            string string1 = timenow1;
            string string2 = ",image name," + resultsFound.imageName + ",model name,"
                            + resultsFound.modelName + ".vrws"
                            + ", Thresholds," + resultsFound.thresholdLow + "," + resultsFound.thresholdUpper
                            + ", roi used," + resultsFound.roi.xROIused.ToString().ToLower()
                            + ", angle," + resultsFound.roi.angle.ToString("0.00")
                            + ", x," + resultsFound.roi.rect.X.ToString("0.00")
                            + ", y," + resultsFound.roi.rect.Y.ToString("0.00")
                            + ", width," + resultsFound.roi.rect.Width.ToString("0.00")
                            + ", height," + resultsFound.roi.rect.Height.ToString("0.00")

                            ;

            //resultsFound.regionFounds
            int i = 0;
            string[] saveString = new string[resultsFound.regionFounds.GetLength(0)];
            if (resultsFound.regionFounds.GetLength(0) == 0)
            {
                //saveString = new string[1]; saveString[0] =  "------------------------------no defects found-----------------------------"; i = 1;
                saveString = new string[1]; saveString[0] = string2;
            }


            foreach (RegionFound item in resultsFound.regionFounds)
            {

                saveString[i] = "class name,D" + item.className + ",score," + item.score.ToString("0.00") + ",cnter," + item.center.X.ToString("0") + "," + item.center.Y.ToString("0") + ",area," + item.area.ToString("0") + ",width," + item.width.ToString() + ",height," + item.height.ToString()

                     ;

                i++;
            }


            //compile answer
            string stringAnswer = string2 + "\n,,";
            for (int k = 0; k < i; k++)
            {
                if (i - k > 1) { stringAnswer = stringAnswer + saveString[k] + "\n,,"; }
                else { stringAnswer = stringAnswer + saveString[k]; }

            }

            string timenow2 = string.Format("{0:ddMMyy_HHmmss}", DateTime.Now); ;
            string fileName = "Defect Found Report_" + timenow2 + ".csv";


            string2 = stringAnswer;
            LogDefectsLines(string1, string2, fileName);
        }
        private static void LogDefectsLines(string string1, string string2, string fileName)
        {
            string path00 = System.Windows.Forms.Application.StartupPath + @"\Data\Defects Reports";

            string path = "";

            string[] dd = Directory.GetFiles(path00);
            foreach (string item in dd)
            {
                string[] s = item.Split('_');
                DateTime dt = new DateTime();

                DateTime dt1 = DateTime.Parse(s[1].Substring(0, 2) + "/" + s[1].Substring(2, 2) + "/" + s[1].Substring(4, 2));

                if (dt1.Date == DateTime.Now.Date)
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
            catch (ViDi2.Exception e)
            {
                xError = true;
                Console.WriteLine("SaveTextToFile(), Error saving barcode to file, " + e.Message, "Error!");
            }


            return xError;
        }
        private RegionFound[] RecordDefectsInfo(System.Collections.ObjectModel.ReadOnlyCollection<ViDi2.IView> views)
        {
            //var views01 = SampleViewerViewModel.Sample.Markings[SampleViewerViewModel.ToolName].Views;

            ViDi2.IRedView redview = (ViDi2.IRedView)views[0];

            RegionFound[] regionFound = new RegionFound[redview.Regions.Count];

            ViDi2.Runtime.IRedTool tool = (ViDi2.Runtime.IRedTool)Stream.Tools.First();

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
        private void saveMarkedImage()
        {

            ViDi2.IImage I = SampleViewerViewModel.Sample.OverlayImage();
            Bitmap bb = I.Bitmap;
            string savePath = @"C:\ProgramData\Cognex\VisionPro Deep Learning\2.1\Examples\c#\projSampaleViewer\bin\x64\Debug\Data\test1.jpg";
            bb.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            I.Dispose();
            bb.Dispose();
        }

        void sampleViewer_ToolSelected(ViDi2.ITool tool)
        {
            RaisePropertyChanged(nameof(SampleViewer));
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
        Image ZoomPicture(Image img, System.Drawing.Size size)
        {
            Bitmap bm = new Bitmap (regionPic, Convert.ToInt32(regionPic.Width * size.Width),Convert.ToInt32(regionPic.Height * size.Height));
            Graphics gpu = Graphics.FromImage (bm);
            gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bm;
        }
        PictureBox org;
        private void txtModelsPath_Load(object sender, EventArgs e)
        {
            cmbImageName.Items.Clear();
            string[] sMode = System.Windows.Forms.Application.StartupPath.Split('\\');
            lblRunMode.Visible = false;
            lblRunMode.Text = "Run mode: " + sMode[sMode.Length - 1];

            PicBoxWith = pictureBox1.Width;
            PicBoxHeight = pictureBox1.Height;

            string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
            IniFile AppliIni = new IniFile(INIpath);

            string smin = AppliIni.ReadValue("Thresholds", "min.", "0.4");
            string smax = AppliIni.ReadValue("Thresholds", "max.", "1.0");

            this.txtThresholdsLower.Text = smin.Trim();
            this.txtThresholdsUpper.Text = smax.Trim();
            string ss = AppliIni.ReadValue("Models", "path", "");
            int numFiles = Directory.GetFiles(ss, "*.vrws").GetLength(0);
            sform = new frmStartUpWindow();
            sform.Visible = true;
            sform.numCards = numFiles;

            sform.Show();
            sform.Refresh();

            var control = new ViDi2.Runtime.Local.Control(ViDi2.GpuMode.Deferred);

            // Initializes all CUDA devices
            control.InitializeComputeDevices(ViDi2.GpuMode.SingleDevicePerTool, new List<int>() { });
            // Turns off optimized GPU memory since high-detail mode doesn't support it
            control.OptimizedGPUMemory(0);

            this.Control = control;
            string licenseType = "License: " + control.License.Type.ToString() + " " + control.License.PerformanceLevel.ToString() + ", Expires Days: " + control.License.Expires.Days.ToString();
            sversion = licenseType + ", " + sversion;
            if (control.License.Expires.Days <= 30)
            {
                txtVersion.ForeColor = Color.Red;
            }
            txtVersion.Text = sversion;

            sampleviewr.sampleViewer.Drop += btnDefect_Click;
            SampleViewerViewModel.ToolSelected += sampleViewer_ToolSelected;
            sampleviewr.sampleViewer.MouseDown += sampleViewer_MouseDown;

            sampleviewr.Visibility = System.Windows.Visibility.Visible;
            Var = sampleviewr.MainWindow.Parent;

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
            Bitmap picture = new Bitmap(dirImages[0]);
            pictureBox1.Image = picture;
            //only image name
            //foreach (string item in dirImages)
            //{
            //    cmbImageName.Items.Add(Path.GetFileName(item));
            //}

            int index = cmbImageName.Items.IndexOf(ImageName);
            cmbImageName.SelectedIndex = index;
            Image img = cmbImageName.Items[0] as Image;
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
            batchFirst = Convert.ToBoolean(AppliIni.ReadValue("Batch roi", "First Done", "0"));


            txtPProiPosX.Text = currentROI.X.ToString();
            txtPProiPosY.Text = currentROI.Y.ToString();
            txtPProiWidth.Text = currentROI.Width.ToString();
            txtPProiHeight.Text = currentROI.Height.ToString();

            xNoAction = true;
            chkUsePPevaluationROI.Checked = Convert.ToBoolean(AppliIni.ReadValue("roi", "used", "False"));

            xNoAction = false;

            Rectangle ImageDimensions = new Rectangle();
           ViDi2.Rect rect = new ViDi2.Rect();
            if (chkUsePPevaluationROI.Checked)
                ApplyROI(true, ImageDimensions, false, rect);

            //
            //tryGridWrite();  

            //                      

            txtAreaFilterWidth.Text = AppliIni.ReadValue("OUTPUT FILTER", "Area Width", "");
            txtAreaFilterHeight.Text = AppliIni.ReadValue("OUTPUT FILTER", "Area Height", "");
            txtAreaFilterScore.Text = AppliIni.ReadValue("OUTPUT FILTER", "Score", "");
            chkFilterActive.Checked = Convert.ToBoolean(AppliIni.ReadValue("OUTPUT FILTER", "Used", ""));

            trackBar1.Minimum = 1;
            trackBar1.Maximum = 6;
            trackBar1.SmallChange = 1;
            trackBar1.LargeChange = 1;
            trackBar1.UseWaitCursor = false;
            this.DoubleBuffered = true;
            org = new PictureBox();
            org.Image = pictureBox1.Image;
        }

        private void elementHost2_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            RoutedEvent mb = e.RoutedEvent;

        }

        private void ApplyROI(bool xNoSave, Rectangle ImageDimensions, bool xAutoROIU, ViDi2.Rect rect)
        {

            ViDi2.IManualRegionOfInterest redROI01 = (ViDi2.IManualRegionOfInterest)Stream.Tools.First().RegionOfInterest;

            redROI01.Parameters.Units = ViDi2.UnitsMode.Pixel;


            if (chkUsePPevaluationROI.Checked)
            {
                double ROIXpos = 0;
                double ROIYpos = 0;
                double ROIwidth = 0;
                double ROIheight = 0;
                double ROIangle = 0;

                if (!xAutoROIU)
                {
                    ROIXpos = Convert.ToDouble(txtPProiPosX.Text.Trim());
                    ROIYpos = Convert.ToDouble(txtPProiPosY.Text.Trim());
                    ROIwidth = Convert.ToDouble(txtPProiWidth.Text.Trim());
                    ROIheight = Convert.ToDouble(txtPProiHeight.Text.Trim());
                    ROIangle = Convert.ToDouble(txtROIangle.Text.Trim());
                }
                else //auto-mode
                {
                    ROIXpos = rect.X;
                    ROIYpos = rect.Y;
                    ROIwidth = rect.Width;
                    ROIheight = rect.Height;
                    ROIangle = 0;
                }


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

                if (imageW == 1 && imageH == 1)
                {
                    imageW = ImageDimensions.Width;
                    imageH = ImageDimensions.Height;
                }


                double areaimg = imageW * imageH;
                int arearoi = (int)(redROI01.Parameters.Size.Width * redROI01.Parameters.Size.Height);
                if (areaimg != 1) // = 1, no image dimensions avialble
                    txtRatioImage2ROI.Text = ((arearoi / areaimg)).ToString("0.000");
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

        public void SetPictureBox()
        {
            lststdResults.Items.Clear();
            if (stream != null)
            {
                string[] ModelsNames = new string[2];
                string ss = txtModelsPath.Text;
                int numFiles = Directory.GetFiles(ss, "*.vrws").GetLength(0);

                string ImagePath = txtImagePath.Text;
                string ImageName = cmbImageName.Text;
                if (!File.Exists(ImagePath + @"\" + ImageName))
                {
                    System.Windows.Forms.MessageBox.Show("image path " + ImagePath + @"\" + ImageName + "don't exist");
                }
                string modelsPath = txtModelsPath.Text;
                string[] dirModels = Directory.GetFiles(modelsPath, "*.vrws");
                foreach (string item in dirModels)
                {
                    if (!File.Exists(item))
                    {
                        System.Windows.Forms.MessageBox.Show("model path " + item + "don't exist");
                    }
                }
                string[] dirImages = Directory.GetFiles(ImagePath, "*.jpg");
                Bitmap bmp = new Bitmap(dirImages[cmbImageName.SelectedIndex]);

                float imgWidth = Convert.ToSingle(bmp.Width);
                float imgHeight = Convert.ToSingle(bmp.Height);
                float fAspectRatio = imgWidth / imgHeight;
                int width1 = 0;
                if (true)
                {
                    width1 = bmp.Width;  //pbxShowImage.Width;  //this.currentImage.picBoxDefaultWidth;
                }
                else
                {
                    width1 = (int)imgWidth;
                }

                int height1 = pictureBox1.Height;
                float fHeight = Convert.ToSingle(width1) / fAspectRatio;

                Bitmap ImagSized = new Bitmap(bmp, width1, Convert.ToInt32(fHeight));

                panel1.AutoScroll = true;
                this.pictureBox1.Image = ImagSized;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 1;
            lststdResults.Items.Clear();
            if (stream != null)
            {
                string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";

                string[] ModelsNames = new string[2];
                IniFile AppliIni = new IniFile(INIpath);
                string ss = AppliIni.ReadValue("Models", "path", "");
                int numFiles = Directory.GetFiles(ss, "*.vrws").GetLength(0);

                //string ImagePath = AppliIni.ReadValue("Last Image", "path", "");

                // txtImagePath.Text = ImagePath;
                string ImagePath = txtImagePath.Text;
                string ImageName = cmbImageName.Text;
                if (!File.Exists(ImagePath + @"\" + ImageName))
                {
                    System.Windows.Forms.MessageBox.Show("image path " + ImagePath + @"\" + ImageName + "don't exist");
                }
                string modelsPath = AppliIni.ReadValue("Models", "path", "");
                string[] dirModels = Directory.GetFiles(modelsPath, "*.vrws");
                //int i = 0;
                foreach (string item in dirModels)
                {
                    if (!File.Exists(item))
                    {
                        System.Windows.Forms.MessageBox.Show("model path " + item + "don't exist");
                    }
                    // lstModels.Items.Add(Path.GetFileName(item));
                    //ModelsNames[i] = Path.GetFileName(item);
                    //i++;
                }
                string[] dirImages = Directory.GetFiles(ImagePath, "*.jpg");
                Bitmap bmp = new Bitmap(dirImages[cmbImageName.SelectedIndex]);
                
                //Bitmap bmp = new Bitmap(dirImages[0]);

                float imgWidth = Convert.ToSingle(bmp.Width);
                float imgHeight = Convert.ToSingle(bmp.Height);
                float fAspectRatio = imgWidth / imgHeight;
                int width1 = 0;
                if (true)
                {
                    width1 = bmp.Width;  //pbxShowImage.Width;  //this.currentImage.picBoxDefaultWidth;
                }
                else
                {
                    width1 = (int)imgWidth;
                }

                int height1 = pictureBox1.Height;
                float fHeight = Convert.ToSingle(width1) / fAspectRatio;

                Bitmap ImagSized = new Bitmap(bmp, width1, Convert.ToInt32(fHeight));
                unsignImage = ImagSized;
                pictureBox2.Image = unsignImage;
                panel1.AutoScroll = true;
                this.pictureBox1.Image = ImagSized;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

              //  SetPictureBox();
                // loadModels();

                //--------------------------set thresholds--------------------------------
                IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;
                ViDi2.Runtime.IRedTool hdRedTool = (ViDi2.Runtime.IRedTool)Stream.Tools.First();
                //ViDi2.Runtime.IRedTool hdRedToolFirst = (ViDi2.Runtime.IRedTool)Stream.Tools["Proj_021_201223_104500_21122023_104445.vrws"];
                //ViDi2.Runtime.IRedTool hdRedToolSecond = (ViDi2.Runtime.IRedTool)Stream.Tools["WS_Proj_022_261223_111400_261223_183645.vrws"];
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

                panel1.AutoScroll = true;

                int ind = cmbImageName.SelectedIndex;
                if (ind >= 0 && ind < cmbImageName.Items.Count)
                { 
                //Bitmap bitmap = new Bitmap(ImagSized);
               // Image img = bitmap;
                
                var image = new ViDi2.UI.WpfImage(dirImages[ind]);   // imagePath);
                SampleViewerViewModel.Sample = stream.Process(image);
                IMarkingOverlayViewModel mm = SampleViewerViewModel.MarkingModel;

                    ViDi2.IView View = mm.Marking.Views[0];

                double x = View.Pose.OffsetX;
                double y = View.Pose.OffsetY;
                double width = View.Size.Width;
                double height = View.Size.Height;
                bool isL = View.IsLabeled;

                ViDi2.IRedView redview = (ViDi2.IRedView)View;
                RegionFound[] regionFound = new RegionFound[redview.Regions.Count];
                ViDi2.Runtime.IRedTool tool = (ViDi2.Runtime.IRedTool)Stream.Tools.First();

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
                   
                string result;
                int index = 0;
                 if (redview.Regions.Count == 0)
                    {
                        DrawRegion1(ImagSized, dirImages[ind], null);
                    }
                 else
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
                    CurrentRegion = regionFound;
                    // DrawRegion(img, dirImages[index], regionFound[index]);
                    //this.pictureBox1.Image =
                    result = "Area = " + regionFound[index].area.ToString() + " Score = " + regionFound[index].score.ToString() + " Perimeter = " + regionFound[index].perimeter.ToString();
                    lststdResults.Items.Add(result);

                    DrawRegion1(ImagSized, dirImages[ind], regionFound);
                    index++;
                }
                // pictureBox1.Cursor = System.Windows.Forms.Cursors.Cross;
                //DrawRegion(pictureBox1.Image);
                //double ex1 = CurrentRegion[0].outer[0].X;
                //double ex2 = CurrentRegion[0].outer[0].Y;
                //bool OK = LoadImage01(dirImages[0]);
            }
                exitProcedure:;
            }
        }

        private void DrawRegion1(Image image, string dirImages, RegionFound[] reg)
        {
            Bitmap bmp = new Bitmap(image);
            int excludeIndex = -1;
            float imgWidth = Convert.ToSingle(image.Width);
            float imgHeight = Convert.ToSingle(image.Height);
            float fAspectRatio = imgWidth / imgHeight;
            int width = 0;
            width = (int)imgWidth;
            int height = pictureBox1.Height;


            //bitmap.Size = new Size(imgWidth, imgHeight);
            //Bitmap img = new Bitmap(image, width, Convert.ToInt32(fHeight));
            if (true)
            {
                width = PicBoxWith;
                //width = pictureBox1.Width;  //pbxShowImage.Width;  //this.currentImage.picBoxDefaultWidth;
            }
            else
            {
                width = (int)imgWidth;
            }
            float fHeight = Convert.ToSingle(width) / fAspectRatio;

            resultsEvluator.sClassesNames = "defect";  // cognexRedEvalOut.sClassesName;
            resultsEvluator.ModelBestLoss = 25;

            int numDetect = 1; //cognexRedEvalOut.regionOut.GetLength(0);
            resultsEvluator.resultsPerClass.ResultsPerClass = new ExportResults.Results[1]; //for now single class models from Cognex
            resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours = new ExportResults.JsonLblData[numDetect];
            resultsEvluator.resultsPerClass.ResultsPerClass[0].fProb = new double[numDetect];
            resultsEvluator.resultsPerClass.ResultsPerClass[0].fUncert = new double[numDetect];
            resultsEvluator.resultsPerClass.ResultsPerClass[0].rect = new ExportResults.Rect[numDetect];

            //if (reg == null)
            //{
                Bitmap bitmap1 = new Bitmap(bmp, width, Convert.ToInt32(fHeight));
                pictureBox1.Image = bitmap1;
                regionPic = bitmap1;
                //unsignImage = bitmap;
                signImage = bitmap1;
                unsignImage = bitmap1;   
            //}
            //else//countors
            foreach (RegionFound item in reg)
            {
                int icounterNumber = 0;
                //foreach (ViDi2.IRegion item in reg.outer)
                //{
                if (item.outer != null)
                { 
                resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours[icounterNumber].ijCoordinates = new int[numDetect][,];
                resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours[icounterNumber].ijCoordinates[icounterNumber] = new int[item.outer.Count, 2];
                resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours[icounterNumber].bClasses = new byte[1];
                resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours[icounterNumber].bClasses[0] = 0; //class id backwoard competability
                                                                                                             //convert countor points array to integer array
                for (int i = 0; i < item.outer.Count; i++)
                {
                    ViDi2.Point p = item.outer[i];
                    resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours[icounterNumber].ijCoordinates[icounterNumber][i, 0] = (int)p.X;
                    resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours[icounterNumber].ijCoordinates[icounterNumber][i, 1] = (int)p.Y;
                }

                for (int i = 0; i < item.outer.Count; i++)
                {
                    ViDi2.Point p = item.outer[i];
                    resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours[icounterNumber].ijCoordinates[icounterNumber][i, 0] = (int)p.X;
                    resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours[icounterNumber].ijCoordinates[icounterNumber][i, 1] = (int)p.Y;
                }
            }
            //List<ViDi2.Point> pointsl = item.Outer.ToList();                                                          
            //prob., score
            resultsEvluator.resultsPerClass.ResultsPerClass[0].fProb[icounterNumber] = item.score;
            resultsEvluator.resultsPerClass.ResultsPerClass[0].fUncert[icounterNumber] = 0;

            resultsEvluator.resultsPerClass.ResultsPerClass[0].rect[icounterNumber].width = item.width;
            resultsEvluator.resultsPerClass.ResultsPerClass[0].rect[icounterNumber].height = item.height;

            resultsEvluator.resultsPerClass.ResultsPerClass[0].rect[icounterNumber].x = (int)(item.center.X - item.width / 2);
            resultsEvluator.resultsPerClass.ResultsPerClass[0].rect[icounterNumber].y = (int)(item.center.Y - item.height / 2);

            icounterNumber++;
            //}

            JsonLblData jsonLbl = new JsonLblData();
            jsonLbl.ijCoordinates = resultsEvluator.resultsPerClass.ResultsPerClass[0].Contours[0].ijCoordinates;

            jsonLbl.bClasses = new byte[1] { 1 };
            //unsignImage = bmp;
            Bitmap Bmp1 = FillLabel(bmp, jsonLbl, 1);
            signImage = Bmp1;
            
            Bitmap bitmap = new Bitmap(Bmp1, width, Convert.ToInt32(fHeight));

            pictureBox1.Image = bitmap;
            regionPic = bitmap;

        }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //trackBar1.Value = 1;
            //lststdResults.Items.Clear();

            frmEndmillNewTab form2 = new frmEndmillNewTab();
            form2.Show();
        }

        //private void DrawRegion(Image image, string dirImages, RegionFound reg)
        //{
        //    //Bitmap bitmap = new Bitmap(dirImages);
        //    Bitmap bmp = new Bitmap(dirImages);
        //    //pictureBox1.Image = bmp;
        //    int excludeIndex = -1;
        //    float imgWidth = Convert.ToSingle(image.Width);
        //    float imgHeight = Convert.ToSingle(image.Height);
        //    float fAspectRatio = imgWidth / imgHeight;
        //    int width = 0;
        //    width = (int)imgWidth;
        //    int height = pictureBox1.Height;
        //    float fHeight = Convert.ToSingle(width) / fAspectRatio;
        //    Bitmap bitmap = new Bitmap(image, width, Convert.ToInt32(fHeight));
        //    //bitmap.Size = new Size(imgWidth, imgHeight);
        //    Bitmap img = new Bitmap(image, width, Convert.ToInt32(fHeight));

        //    foreach (ViDi2.Point o in reg.outer)
        //    {
        //        //Myouter[j] = new Vector3(Pixel_to_Mn((float)o.X), Pixel_to_Mn((float)o.Y), Pixel_to_Mn((float)o.X));
        //        //bitmap.SetPixel(10, 10, Color.Red);
        //        //bitmap.SetPixel(100, 100, Color.Red);
        //        //bmp.SetPixel(1000, 1000, Color.Red);
        //        //bmp.SetPixel((int)o.X/10, (int)o.Y/10, Color.Red);
                
        //        pictureBox1.Image = bmp;
              
        //        //e.Graphics.DrawPoint(new Pen(Color.Red, 0), Myouter[0]);
        //    }

        //}

        public void DrawRectangle ()
        {

        }

        private void MainScreen_FormClosed(object sender, FormClosedEventArgs e)
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
                for (int i = rwsl.Names.Count - 1; i > -1; i--)
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

        private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
           // currentPosition = PointToCartesian(e.Location);
            //lblCoor.Text = string.Format("{0}, {1}", e.Location.X, e.Location.Y);
            // lblCoor.Text = string.Format("{0,0:F3}, {1,0:F3}", currentPosition.X, currentPosition.Y);
            lblCoor.Text = string.Format("{0,0:F3}, {1,0:F3}", Mm_to_Pixel( e.Location.X), Mm_to_Pixel (e.Location.Y));

        }



        private static Bitmap LoadBitmapUnlocked(string file_name)
        {
            //using (Bitmap bm = new Bitmap(file_name))
            //{
            //    return new Bitmap(bm);
            //}

            //19/10/2021
            Image img = null;
            using (FileStream stream = new FileStream(file_name, FileMode.Open, FileAccess.Read))
            {
                img = Image.FromStream(stream);
                stream.Close();

                return (Bitmap)img;
            }
        }

        private Image SaveToStream(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            var ms = new MemoryStream();

           // bmp.Save(ms, jgpEncoder, myEncoderParameters);

            //Do whatever you need to do with the image
            //e.g.
            //img = Image.FromStream(ms);

            return img;
        }

        private bool LoadImage01(string pathandbname)
        {
            bool xLoadOK = false;
            if (File.Exists(pathandbname))
            {
                Image image = LoadBitmapUnlocked(pathandbname);

                currentImage.imgActiveImage = (Image)image.Clone();
                int ilenght = pathandbname.Length;
                string snapIndex = pathandbname.Substring(ilenght - 5, 1);

                switch (snapIndex)
                {
                    case "1":
                        currentImage.zoomSnap1 = SaveToStream(image);
                        break;

                    case "2":
                        currentImage.zoomSnap2 = SaveToStream(image);
                        break;

                    case "3":
                        currentImage.zoomSnap3 = SaveToStream(image);
                        break;
                    case "4":
                        currentImage.zoomSnap4 = SaveToStream(image);
                        break;
                    case "5":
                        currentImage.zoomSnap5 = SaveToStream(image);
                        break;
                }
                currentImage.imgWidth = image.Width;
                currentImage.imgHeight = image.Height;
                currentImage.path = pathandbname;

                defaultImagePath = pathandbname;

                float imgWidth = Convert.ToSingle(image.Width);
                float imgHeight = Convert.ToSingle(image.Height);
                float fAspectRatio = imgWidth / imgHeight;

                int width = 0;
                if (!chckZoom.Checked)
                {
                    width = currentImage.picBoxDefaultWidth;  //pbxShowImage.Width;  //this.currentImage.picBoxDefaultWidth;
                }
                else
                {
                    width = (int)imgWidth;
                }
                int height = pictureBox1.Height;
                float fHeight = Convert.ToSingle(width) / fAspectRatio;

                Bitmap img = new Bitmap(image, width, Convert.ToInt32(fHeight));
                currentImage.picBoxDefaultWidth = img.Width;
                currentImage.picBoxDefaultHeight = img.Height;
               // pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

                if (image != null)
                    image.Dispose();

                xLoadOK = true;
            }
            else
            {
                xLoadOK = false;
            }

            return xLoadOK;
        }



        private Bitmap FillLabel(Bitmap ImgBitmap, JsonLblData ijCoordinates, int excludeIndex)
        {

            Graphics g = null;
            using (g = Graphics.FromImage(ImgBitmap))
            {
                int iNumOfClasses = ijCoordinates.bClasses.GetLength(0);
                for (int c = 0; c < iNumOfClasses; c++)
                {
                    if (ijCoordinates.bClasses[c] > 0)
                    {
                        if (excludeIndex != c)
                        {

                            Color ClassColor = Color.Green;
                            Pen ContourPen = new Pen(Color.Green);
                            // Set the pen's width.
                            ContourPen.Width = 3.0F;

                            int iClassIndex = ijCoordinates.bClasses[c];

                            if (iClassIndex == 0)
                            {
                                ClassColor = Color.Green; ContourPen = new Pen(Color.Green);
                            }
                            else if (iClassIndex == 1)
                            {
                                ClassColor = Color.Red;
                                ContourPen = new Pen(Color.Red);
                            }
                            else if (iClassIndex == 2)
                            {
                                ClassColor = Color.Blue;
                                ContourPen = new Pen(Color.Blue);
                            }

                            //set class id for bitmap
                            //if (ijCoordinates.selectedContour == c)
                            //{
                            //    ClassColor = Color.Yellow;
                            //    ContourPen = new Pen(Color.Yellow);

                            //    ijCoordinates.selectedContour = -1;
                            //}


                            ContourPen.Width = 3.0F;
                            SolidBrush ClassBrush = new SolidBrush(ClassColor);

                            //get coordinates of label i
                            int iNumCoor = ijCoordinates.ijCoordinates[c].GetLength(0);
                           System.Drawing.Point[] LblCoordinates = new System.Drawing.Point[iNumCoor];
                            //load label's contuor coordinates 
                            for (int i = 0; i < iNumCoor; i++)
                            {
                                int iCorrX = ijCoordinates.ijCoordinates[c][i, 0];
                                int iCorrY = ijCoordinates.ijCoordinates[c][i, 1];
                                System.Drawing.Point pnt = new System.Drawing.Point(iCorrX, iCorrY);

                                LblCoordinates[i] = pnt;
                            }

                            bool xContourOnly = true;
                            if (!xContourOnly)
                            {
                                g.FillClosedCurve(ClassBrush, LblCoordinates);  //standard mode (default mode is Alternate)
                            }
                            else
                            {
                                //Pen p = Pens.Green;
                                // Create a new pen.
                                g.DrawPolygon(ContourPen, LblCoordinates);  //standard mode (default mode is Alternate)
                            }
                        }
                    }

                } //end for class index
            }

            return ImgBitmap;

        }
        private void ZoomInOut(bool zoom)
        {
            //Zoom ratio by which the images will be zoomed by default
            int zoomRatio = 10;
            //Set the zoomed width and height
            int widthZoom = pictureBox1.Image.Width * zoomRatio / 100;
            int heightZoom = pictureBox1.Image.Height * zoomRatio / 100;
            //zoom = true --> zoom in
            //zoom = false --> zoom out
            if (!zoom)
            {
                widthZoom *= -1;
                heightZoom *= -1;
            }
            //Add the width and height to the picture box dimensions
            pictureBox1.Width += widthZoom;
            pictureBox1.Height += heightZoom;
        }
        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            bool zoom;
            if (panel1.AutoScroll)
            {
                zoom = true;
            }
            else
            {
                 zoom = false;
            }
            ZoomInOut(zoom);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value!=0)
            {
                currentZoom = trackBar1.Value;
                pictureBox1.Image = null;
                unsignImage = ZoomPicture(unsignImage, new System.Drawing.Size(trackBar1.Value, trackBar1.Value));
                pictureBox1.Image = ZoomPicture(org.Image, new System.Drawing.Size(trackBar1.Value, trackBar1.Value));
            }
        }

        private void txtImagePath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtImagePath.Text != "")
                {
                    cmbImageName.Items.Clear();
                    string ImagePath = txtImagePath.Text;
                    string[] dirImages = Directory.GetFiles(ImagePath, "*.jpg");
                    //only image name
                    foreach (string item in dirImages)
                    {
                        cmbImageName.Items.Add(Path.GetFileName(item));
                    }
                }
            }
            catch { }
        }

        private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //signImage = pictureBox1.Image;
            pictureBox1.Image = null;
            pictureBox1.Image = ZoomPicture(unsignImage, new System.Drawing.Size(trackBar1.Value, trackBar1.Value));
        }

        private void pictureBox1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Image = ZoomPicture(signImage, new System.Drawing.Size(trackBar1.Value, trackBar1.Value));
        }

      
    }
}
