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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ViDi2;
using System.Windows.Media.Effects;
using System.Windows.Controls;
using static projSampaleViewer.frmEndmillNewTab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.ServiceModel.Channels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics; // where the Stopwatch class is defined 

namespace projSampaleViewer
{
    public partial class frmEndmillNewTab : Form
    {
        public frmEndmillNewTab()
        {
            InitializeComponent();
        }

        /*Structures*/
        public struct EndmillParameters
        {
            public int CatalogNo;
            public int BladesNo;
            public int Diameter;
            public int Length;
            public float FracLowerThreshold;
            public float FracUpperThreshold;
            public float PeelLowerThreshold;
            public float PeelUpperThreshold;
            public int roiPosX1;
            public int roiPosY1;
            public int roiWidth1;
            public int roiHeight1;
            public float roiAngle1;
            public float roiRatio1;
            public int roiPosX2;
            public int roiPosY2;
            public int roiWidth2;
            public int roiHeight2;
            public float roiAngle2;
            public float roiRatio2;
            public int roiPosX3;
            public int roiPosY3;
            public int roiWidth3;
            public int roiHeight3;
            public float roiAngle3;
            public float roiRatio3;
            public string ImagePath;
            public string IsFractions1;
            public string IsPeels1;
            public string IsFractions2;
            public string IsPeels2;
            public string IsFractions3;
            public string IsPeels3;
            public string Roi1;
            public string Roi2;
            public string Roi3;

            public float FracLowerArea;
            public float FracUpperArea;
            public float PeelLowerArea;
            public float PeelUpperArea;
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

        public struct RatioXratioY
        {
            public float ratioX;
            public float ratioY;
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
            public System.Drawing.Image imgActiveImage;
            public string LocalLastImagePath;
            public string ImageFileType;

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

        public struct ROI
        {
            public System.Windows.Rect rect;
            public double angle;
            public bool xROIused;
        }

        public struct RunTimeWorkapace
        {
            public int gpuId01;
            public int gpuId02;
            public Dictionary<string, ViDi2.Runtime.IStream> StreamDict;
            public bool xNoError;
        }

        public struct ResInfo
        {
            public string[] ShowRes;
            public int DefectId;
        }

        public struct ResArr
        {
            public ResInfo resInfo;
            public int resIndex;
        }

        /*Parameters*/
        static string JassonPath =System.Windows.Forms.Application.StartupPath +@"\Data\DataBase\EndmillsData.Jason";
        JassonClass JassonDataClass = new JassonClass();
        private bool isDragging;
        private System.Drawing.Point lastMousePosition;
        private bool isDrawing;
        private System.Drawing.Point startPoint;
        private Rectangle rectangle;
        private Rectangle rectangleToProc;
        private Rectangle[] RoiRects = new Rectangle[3];
        private bool isEditing;
        ResArr resArr = new ResArr();
        RunTimeWorkapace grunTimeWorkapace = new RunTimeWorkapace();
        string Model1Name = "Brake";
        string Model2Name = "Peel";
        RegionFound[] BreakeRegions;
        RegionFound[] PealRegions;
        ViDi2.ISample[] samplesROI = new ViDi2.ISample[2];
        private int PicBoxWidth;
        private int PicBoxHeight;
        public static frmStartUpWindow sform;
        ViDi2.Runtime.IControl control;
        ViDi2.Runtime.IWorkspace workspace;
        ViDi2.Runtime.IStream stream;
        private string sversion = "Sample Viewer 07-12-2023";
        private ucSampleViewerClass sampleviewr = new ucSampleViewerClass();
        private string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
        private ViDi2.Runtime.IWorkspaceList rwsl;
        private System.Windows.Rect currentROI = new System.Windows.Rect();
        private bool batchFirst;
        private bool xNoAction;
        PictureBox org;
        private CurrentImage gcurrentImage = new CurrentImage();
        public static RectangleResizeAndRotateAdd StaticROI = new RectangleResizeAndRotateAdd();
        private const string stdmodeCaption = "Edit ROI";
        private const string editmodeCaption = "ROI MODE";
        private int iCountPain;
        
        /*Events*/
        public event PropertyChangedEventHandler PropertyChanged;
        public IList<ViDi2.Runtime.IWorkspace> Workspaces => Control.Workspaces.ToList();
        public ISampleViewerViewModel SampleViewerViewModel => sampleviewr.sampleViewer.ViewModel;
        public DependencyObject Var { get; private set; }

        /*Functions*/

        public static void UpdateJson(string endmillName, Endmill origEnd)
        {
            // Read the JSON file
            string json = File.ReadAllText(JassonPath);

            // Parse the JSON into a JArray
            JArray jsonArray = JArray.Parse(json);

            // Find the object with the matching "EndmillName"
            JObject endmillObject = jsonArray.Children<JObject>()
                .FirstOrDefault(o => o["EndmillName"].ToString() == endmillName);

            if (endmillObject != null)
            {
                // Update the desired properties
                endmillObject["CatalogNo"] = origEnd.CatalogNo;
                endmillObject["BladesNo"] = origEnd.BladesNo;
                endmillObject["Diameter"] = origEnd.Diameter;
                endmillObject["Length"] = origEnd.Length;
                endmillObject["FracLowerThreshold"] = origEnd.FracLowerThreshold;
                endmillObject["FracUpperThreshold"] = origEnd.FracUpperThreshold;
                endmillObject["PeelLowerThreshold"] = origEnd.PeelLowerThreshold;
                endmillObject["PeelUpperThreshold"] = origEnd.PeelUpperThreshold;
                endmillObject["roiPosX1"] = origEnd.roiPosX1;
                endmillObject["roiPosY1"] = origEnd.roiPosY1;
                endmillObject["roiWidth1"] = origEnd.roiWidth1;
                endmillObject["roiHeight1"] = origEnd.roiHeight1;
                endmillObject["roiAngle1"] = origEnd.roiAngle1;
                endmillObject["roiRatio1"] = origEnd.roiRatio1;
                endmillObject["roiPosX2"] = origEnd.roiPosX2;
                endmillObject["roiPosY2"] = origEnd.roiPosY2;
                endmillObject["roiWidth2"] = origEnd.roiWidth2;
                endmillObject["roiHeight2"] = origEnd.roiHeight2;
                endmillObject["roiAngle2"] = origEnd.roiAngle2;
                endmillObject["roiRatio2"] = origEnd.roiRatio2;
                endmillObject["roiPosX3"] = origEnd.roiPosX3;
                endmillObject["roiPosY3"] = origEnd.roiPosY3;
                endmillObject["roiWidth3"] = origEnd.roiWidth3;
                endmillObject["roiHeight3"] = origEnd.roiHeight3;
                endmillObject["roiAngle3"] = origEnd.roiAngle3;
                endmillObject["roiRatio3"] = origEnd.roiRatio3;
                endmillObject["ImagePath"] = origEnd.ImagePath;
                endmillObject["IsFractions1"] = origEnd.IsFractions1;
                endmillObject["IsPeels1"] = origEnd.IsPeels1;
                endmillObject["IsFractions2"] = origEnd.IsFractions2;
                endmillObject["IsPeels2"] = origEnd.IsPeels2;
                endmillObject["IsFractions3"] = origEnd.IsFractions3;
                endmillObject["IsPeels3"] = origEnd.IsPeels3;
                endmillObject["Roi1"] = origEnd.Roi1;
                endmillObject["Roi2"] = origEnd.Roi2;
                endmillObject["Roi3"] = origEnd.Roi3;

                endmillObject["FracLowerArea"] = origEnd.FracLowerArea;
                endmillObject["FracUpperArea"] = origEnd.FracUpperArea;
                endmillObject["PeelLowerArea"] = origEnd.PeelLowerArea;
                endmillObject["PeelUpperArea"] = origEnd.PeelUpperArea;

                // Save the updated JSON back to the file
                File.WriteAllText(JassonPath, jsonArray.ToString());
            }
            else
            {
                Console.WriteLine("Endmill not found");
            }
        }

        public void saveToJason()
        {
            if (CmbCatNum.Text != "")
            {
                Endmill newEndmill = new Endmill();
                newEndmill.EndmillName = CmbCatNum.Text;
                newEndmill.CatalogNo = Int32.Parse(txtAddNum.Text);
                newEndmill.CatalogNo = Int32.Parse(txtBlades.Text);
                newEndmill.Diameter = Int32.Parse(txtDiameter.Text);
                newEndmill.Length = Int32.Parse(txtLength.Text);
                newEndmill.FracLowerThreshold = float.Parse(txtThresholdsLower.Text);
                newEndmill.FracUpperThreshold = float.Parse(txtThresholdsUpper.Text);
                newEndmill.PeelLowerThreshold = float.Parse(txtPlsThresoldLower.Text);
                newEndmill.PeelUpperThreshold = float.Parse(txtPlsThresoldUpper.Text);
                newEndmill.roiPosX1 = Int32.Parse(txtPProiPosX.Text);
                newEndmill.roiPosY1 = Int32.Parse(txtPProiPosY.Text);
                newEndmill.roiWidth1 = Int32.Parse(txtPProiWidth.Text);
                newEndmill.roiHeight1 = Int32.Parse(txtPProiHeight.Text);
                newEndmill.roiAngle1 = float.Parse(txtROIangle.Text);
                newEndmill.roiRatio1 = float.Parse(txtRatioImage2ROI.Text);
                newEndmill.roiPosX2 = Int32.Parse(txtPosX2.Text);
                newEndmill.roiPosY2 = Int32.Parse(txtPosY2.Text);
                newEndmill.roiWidth2 = Int32.Parse(txtWidth2.Text);
                newEndmill.roiHeight2 = Int32.Parse(txtHeight2.Text);
                newEndmill.roiAngle2 = float.Parse(txtAngle2.Text);
                newEndmill.roiRatio2 = float.Parse(txtRatio2.Text);
                newEndmill.roiPosX3 = Int32.Parse(txtPosX3.Text);
                newEndmill.roiPosY3 = Int32.Parse(txtPosY3.Text);
                newEndmill.roiWidth3 = Int32.Parse(txtWidth3.Text);
                newEndmill.roiHeight3 = Int32.Parse(txtHeight3.Text);
                newEndmill.roiAngle3 = float.Parse(txtAngle3.Text);
                newEndmill.roiRatio3 = float.Parse(txtRatio3.Text);

                newEndmill.FracLowerArea = float.Parse(txtAreaLower.Text);
                newEndmill.FracUpperArea = float.Parse(txtAreaUpper.Text);
                newEndmill.PeelLowerArea = float.Parse(txtPlsAreaLower.Text);
                newEndmill.PeelUpperArea = float.Parse(txtPlsAreaUpper.Text);

                newEndmill.ImagePath = txtImgPathLoad.Text;

                if (chkFractions1.Checked == true)
                    newEndmill.IsFractions1 = "True";
                else newEndmill.IsFractions1 = "False";

                if (chkPeels1.Checked == true)
                    newEndmill.IsPeels1 = "True";
                else newEndmill.IsPeels1 = "False";

                if (chkFractions2.Checked == true)
                    newEndmill.IsFractions2 = "True";
                else newEndmill.IsFractions2 = "False";

                if (chkPeels2.Checked == true)
                    newEndmill.IsPeels2 = "True";
                else newEndmill.IsPeels2 = "False";

                if (chkROI1.Checked == true)
                    newEndmill.Roi1 = "True";
                else newEndmill.Roi1 = "False";
                if (chkROI2.Checked == true)
                    newEndmill.Roi2 = "True";
                else newEndmill.Roi2 = "False";
                if (chkROI3.Checked == true)
                    newEndmill.Roi3 = "True";
                else newEndmill.Roi3 = "False";
                JassonDataClass.setNewValue(newEndmill);
            }
        }
        public void LoadJasonParameters()
        {
            if (CmbCatNum.Text != "")
            {
                Endmill EndData = new Endmill();
                EndData = JassonDataClass.getJassonParameters(CmbCatNum.Text);
                if (EndData != null)
                {
                    txtBlades.Text = EndData.BladesNo.ToString();
                    txtDiameter.Text = EndData.Diameter.ToString();
                    txtLength.Text = EndData.Length.ToString();
                    txtThresholdsLower.Text = EndData.FracLowerThreshold.ToString();
                    txtThresholdsUpper.Text = EndData.FracUpperThreshold.ToString();
                    txtPlsThresoldLower.Text = EndData.PeelLowerThreshold.ToString();
                    txtPlsThresoldUpper.Text = EndData.PeelUpperThreshold.ToString();
                    txtPProiPosX.Text = EndData.roiPosX1.ToString();
                    txtPProiPosY.Text = EndData.roiPosY1.ToString();
                    txtPProiWidth.Text = EndData.roiWidth1.ToString();
                    txtPProiHeight.Text = EndData.roiHeight1.ToString();
                    txtROIangle.Text = EndData.roiAngle1.ToString();
                    txtRatioImage2ROI.Text = EndData.roiRatio1.ToString();
                    txtPosX2.Text = EndData.roiPosX2.ToString();
                    txtPosY2.Text = EndData.roiPosY2.ToString();
                    txtWidth2.Text = EndData.roiWidth2.ToString();
                    txtHeight2.Text = EndData.roiHeight2.ToString();
                    txtAngle2.Text = EndData.roiAngle2.ToString();
                    txtRatio2.Text = EndData.roiRatio2.ToString();
                    txtPosX3.Text = EndData.roiPosX3.ToString();
                    txtPosY3.Text = EndData.roiPosY3.ToString();
                    txtWidth3.Text = EndData.roiWidth3.ToString();
                    txtHeight3.Text = EndData.roiHeight3.ToString();
                    txtAngle3.Text = EndData.roiAngle3.ToString();
                    txtRatio3.Text = EndData.roiRatio3.ToString();
                    txtAddNum.Text = EndData.CatalogNo.ToString();
                    txtImgPathLoad.Text = EndData.ImagePath.ToString();

                    txtAreaLower.Text = EndData.FracLowerArea.ToString();
                    txtAreaUpper.Text = EndData.FracUpperArea.ToString();
                    txtPlsAreaLower.Text = EndData.PeelLowerArea.ToString();
                    txtPlsAreaUpper.Text = EndData.PeelUpperArea.ToString();

                    if (EndData.IsFractions1 == "true")
                        chkFractions1.Checked = true;
                    else chkFractions1.Checked = false;
                    if (EndData.IsPeels1 == "true")
                        chkPeels1.Checked = true;
                    else chkPeels1.Checked = false;
                    if (EndData.IsFractions2 == "true")
                        chkFractions2.Checked = true;
                    else chkFractions2.Checked = false;
                    if (EndData.IsPeels2 == "true")
                        chkPeels2.Checked = true;
                    else chkPeels2.Checked = false;
                    if (EndData.IsFractions3 == "true")
                        chkFractions3.Checked = true;
                    else chkFractions3.Checked = false;
                    if (EndData.IsPeels3 == "true")
                        chkPeels3.Checked = true;
                    else chkPeels3.Checked = false;

                    if (EndData.Roi1 == "true")
                        chkROI1.Checked = true;
                    else chkROI1.Checked = false;
                    if (EndData.Roi2 == "true")
                        chkROI2.Checked = true;
                    else chkROI2.Checked = false;
                    if (EndData.Roi3 == "true")
                        chkROI3.Checked = true;
                    else chkROI3.Checked = false;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Check Endmill Name");
                CmbCatNum.Text = "";
            }
        }
        public void LoadINIParameters ()
        {   
            string iniFileName = CmbCatNum.Text + ".ini";
            string iniFilePath = System.Windows.Forms.Application.StartupPath + @"\Data\DataBase\" + iniFileName;
            if (File.Exists(iniFilePath))
            {
            EndmillParameters parameters = new EndmillParameters();
            parameters.BladesNo = GetIniInfo(iniFilePath, CmbCatNum.Text).BladesNo;
            parameters.Diameter = GetIniInfo(iniFilePath, CmbCatNum.Text).Diameter;
            parameters.Length = GetIniInfo(iniFilePath, CmbCatNum.Text).Length;
            parameters.FracLowerThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).FracLowerThreshold;
            parameters.FracUpperThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).FracUpperThreshold;
            parameters.PeelLowerThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).PeelLowerThreshold;
            parameters.PeelUpperThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).PeelUpperThreshold;
            parameters.roiPosX1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX1;
            parameters.roiPosY1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY1;
            parameters.roiWidth1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth1;
            parameters.roiHeight1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight1;
            parameters.roiAngle1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle1;
            parameters.roiRatio1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio1;
            parameters.roiPosX2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX2;
            parameters.roiPosY2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY2;
            parameters.roiWidth2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth2;
            parameters.roiHeight2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight2;
            parameters.roiAngle2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle2;
            parameters.roiRatio2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio2;
            parameters.roiPosX3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX3;
            parameters.roiPosY3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY3;
            parameters.roiWidth3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth3;
            parameters.roiHeight3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight3;
            parameters.roiAngle3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle3;
            parameters.roiRatio3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio3;
            parameters.CatalogNo = GetIniInfo(iniFilePath, CmbCatNum.Text).CatalogNo;
            parameters.ImagePath = GetIniInfo(iniFilePath, CmbCatNum.Text).ImagePath;
            parameters.IsFractions1 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions1;
            parameters.IsPeels1 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels1;
            parameters.IsFractions2 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions2;
            parameters.IsPeels2 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels2;
            parameters.IsFractions3 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions3;
            parameters.IsPeels3 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels3;
            parameters.Roi1 = GetIniInfo(iniFilePath, CmbCatNum.Text).Roi1;
            parameters.Roi2 = GetIniInfo(iniFilePath, CmbCatNum.Text).Roi2;
            parameters.Roi3 = GetIniInfo(iniFilePath, CmbCatNum.Text).Roi3;
            txtBlades.Text = parameters.BladesNo.ToString();
            txtDiameter.Text = parameters.Diameter.ToString();
            txtLength.Text = parameters.Length.ToString();
            txtThresholdsLower.Text = parameters.FracLowerThreshold.ToString();
            txtThresholdsUpper.Text = parameters.FracUpperThreshold.ToString();
            txtPlsThresoldLower.Text = parameters.PeelLowerThreshold.ToString();
            txtPlsThresoldUpper.Text = parameters.PeelUpperThreshold.ToString();
            txtPProiPosX.Text = parameters.roiPosX1.ToString();
            txtPProiPosY.Text = parameters.roiPosY1.ToString();
            txtPProiWidth.Text = parameters.roiWidth1.ToString();
            txtPProiHeight.Text = parameters.roiHeight1.ToString();
            txtROIangle.Text = parameters.roiAngle1.ToString();
            txtRatioImage2ROI.Text = parameters.roiRatio1.ToString();
            txtPosX2.Text = parameters.roiPosX2.ToString();
            txtPosY2.Text = parameters.roiPosY2.ToString();
            txtWidth2.Text = parameters.roiWidth2.ToString();
            txtHeight2.Text = parameters.roiHeight2.ToString();
            txtAngle2.Text = parameters.roiAngle2.ToString();
            txtRatio2.Text = parameters.roiRatio2.ToString();
            txtPosX3.Text = parameters.roiPosX3.ToString();
            txtPosY3.Text = parameters.roiPosY3.ToString();
            txtWidth3.Text = parameters.roiWidth3.ToString();
            txtHeight3.Text = parameters.roiHeight3.ToString();
            txtAngle3.Text = parameters.roiAngle3.ToString();
            txtRatio3.Text = parameters.roiRatio3.ToString();
            txtAddNum.Text = parameters.CatalogNo.ToString();
            txtImgPathLoad.Text = parameters.ImagePath.ToString();

                txtAreaLower.Text = parameters.FracLowerArea.ToString();
                txtAreaUpper.Text = parameters.FracUpperArea.ToString();
                txtPlsAreaLower.Text = parameters.PeelLowerArea.ToString();
                txtPlsAreaUpper.Text = parameters.PeelUpperArea.ToString();

                if (parameters.IsFractions1 == "True")
                chkFractions1.Checked = true;
            else chkFractions1.Checked = false;
            if (parameters.IsPeels1 == "True")
                chkPeels1.Checked = true;
            else chkPeels1.Checked = false;
            if (parameters.IsFractions2 == "True")
                chkFractions2.Checked = true;
            else chkFractions2.Checked = false;
            if (parameters.IsPeels2 == "True")
                chkPeels2.Checked = true;
            else chkPeels2.Checked = false;
            if (parameters.IsFractions3 == "True")
                chkFractions3.Checked = true;
            else chkFractions3.Checked = false;
            if (parameters.IsPeels3 == "True")
                chkPeels3.Checked = true;
            else chkPeels3.Checked = false;

            if (parameters.Roi1 == "True")
                chkROI1.Checked = true;
            else chkROI1.Checked = false;
            if (parameters.Roi2 == "True")
                chkROI2.Checked = true;
            else chkROI2.Checked = false;
            if (parameters.Roi3 == "True")
                chkROI3.Checked = true;
            else chkROI3.Checked = false;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Check Endmill Name");
                CmbCatNum.Text = "";
            }
        }
        public static System.Drawing.Image CropImage(System.Drawing.Image sourceImage, Rectangle cropRectangle)
        {
            Bitmap croppedImage = new Bitmap(cropRectangle.Width, cropRectangle.Height);

            using (Graphics graphics = Graphics.FromImage(croppedImage))
            {
                graphics.DrawImage(sourceImage, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height),
                    cropRectangle, GraphicsUnit.Pixel);
            }

            return croppedImage;
        }

        public void modelToRun (int modelIndex)
        {
            Workspace = rwsl[rwsl.Names[modelIndex]];

            txtRunTimeWSName.Text = Workspace.DisplayName;
            stream = Workspace.Streams.First();
            string streamName = stream.Name;  // Workspace.Streams.First().Name;
            ViDi2.Runtime.ITool tool = Workspace.Streams[streamName].Tools.First();
            txtToolName.Text = tool.Name;   // Workspace.Streams[streamName].Tools.First().Name;            
            txtToolType.Text = tool.Type.ToString();
            txtLastModified.Text = Workspace.LastModified.ToShortDateString();

            //class name display
            ViDi2.Runtime.IRedTool tool1 = (ViDi2.Runtime.IRedTool)stream.Tools.First();
            var knownClasses = tool1.KnownClasses;
            if (knownClasses.Count > 0)
            {
                string className = knownClasses[0];
                lblClassName.Text = className;
            }

            //get thresholds tool1                
            IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;

            ViDi2.Interval interval = hdRedParams.Threshold;

        }

        public void SetThreshold(ViDi2.Runtime.IRedHighDetailParameters hdRedParams, double lower1, double upper1)
        {
            bool xNotRedTool = false;
            if (hdRedParams == null) { xNotRedTool = true; }

            if (!xNotRedTool)
            {
                ViDi2.Interval interval = new ViDi2.Interval(lower1, upper1);
                hdRedParams.Threshold = interval;
            }

            exitProcedure:;
        }

        public void InitializeWorkspaces()
        {
            List<int> GPUList = new List<int>();
            control = new ViDi2.Runtime.Local.Control(GpuMode.Deferred, GPUList);
            //Gpu Cards
            control.InitializeComputeDevices(GpuMode.SingleDevicePerTool, GPUList);
            //how many cards
            var computeDevices = control.ComputeDevices;
            //create stream dictionary
            var StreamDict = new Dictionary<string, ViDi2.Runtime.IStream>();

            string gpuID = "default/red_HDM_20M_5472x3648/0";
            string wsName = Model1Name;
            string wsPath = System.Windows.Forms.Application.StartupPath + @"\final models\Proj_021_201223_104500_21122023_104445.vrws";
            string wsName2 = Model2Name;
            string wsPath2 = System.Windows.Forms.Application.StartupPath + @"\final models\WS_Proj_022_261223_111400_261223_183645.vrws";
            StreamDict.Add(wsName, control.Workspaces.Add(wsName, wsPath, gpuID).Streams["default"]);
            StreamDict.Add(wsName2, control.Workspaces.Add(wsName2, wsPath2, gpuID).Streams["default"]);
            grunTimeWorkapace.gpuId01 = 0;
            grunTimeWorkapace.StreamDict = StreamDict;
        }

        public void AddFieldToIniFile(string iniFilePath, string sectionName, string fieldName, string fieldValue)
        {
            // Read the existing INI file
            var iniLines = File.ReadAllLines(iniFilePath, Encoding.UTF8);

            // Find the section in which you want to add the field
            int sectionIndex = -1;
            for (int i = 0; i < iniLines.Length; i++)
            {
                if (iniLines[i].TrimStart().StartsWith($"[{sectionName}]"))
                {
                    sectionIndex = i;
                    break;
                }
            }

            if (sectionIndex == -1)
            {
                // Section not found, handle the error or create a new section if desired
                return;
            }

            // Add the new field to the section
            var newLine = $"{fieldName}={fieldValue}";
            iniLines[sectionIndex] += "\n" + newLine;

            // Write the modified INI file back
            File.WriteAllLines(iniFilePath, iniLines, Encoding.UTF8);
        }

        public bool DoesFieldExistInIniFile(string iniFilePath, string sectionName, string fieldName)
        {
            // Read the existing INI file
            var iniLines = File.ReadAllLines(iniFilePath);

            // Find the section in which to check for the field
            int sectionIndex = -1;
            for (int i = 0; i < iniLines.Length; i++)
            {
                if (iniLines[i].TrimStart().StartsWith($"[{sectionName}]"))
                {
                    sectionIndex = i;
                    break;
                }
            }

            if (sectionIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Section Not Found");
                return false;
            }

            // Check if the field exists in the section
            string fieldLine = $"{fieldName}=";
            for (int i = sectionIndex + 1; i < iniLines.Length; i++)
            {
                if (iniLines[i].StartsWith("["))
                {
                    // Reached the next section, field not found
                    return false;
                }

                if (iniLines[i].StartsWith(fieldLine))
                {
                    // Field found
                    return true;
                }
            }

            // Field not found
            return false;
        }

        public void DrawRectangleOnElementHost(ElementHost elementHost, Rectangle rectangle)
        {
            elementHost.Paint += (sender, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, rectangle, Color.Red, ButtonBorderStyle.Solid);
            };
            elementHost.Invalidate();
        }
        

        private static DialogResult ShowInputDialog(ref string input, System.Windows.Forms.ComboBox cmbList, string sHeader, Color color, int iSize, ref ContextMenuStrip cms, bool xNoCancelButton)
        {
            System.Drawing.Size size = new System.Drawing.Size(iSize, 240);  //original = 200, 70

            Form inputBox = new Form();

            inputBox.ControlBox = false;

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = sHeader; // "Select Number Of Rules Areas";

            bool xUseTextBox = false;
            if (xUseTextBox)
            {
                System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();

                int textBoxHeight = 40;
                textBox.Size = new System.Drawing.Size(size.Width - 10, textBoxHeight);
                textBox.Location = new System.Drawing.Point(5, 5);
                textBox.Text = input;
                var font = textBox.Font;

                System.Drawing.Font f = new System.Drawing.Font(font.Name.ToString(), 10.0f);
                textBox.Font = f;

                inputBox.Controls.Add(textBox);
            }
            else
            {
                System.Windows.Forms.Label textBox = new System.Windows.Forms.Label();

                int textBoxHeight = 40;
                textBox.Size = new System.Drawing.Size(size.Width - 10, textBoxHeight);
                textBox.Location = new System.Drawing.Point(5, 5);
                textBox.Text = input;
                var font = textBox.Font;

                System.Drawing.Font f = new System.Drawing.Font(font.Name.ToString(), 10.0f);
                textBox.Font = f;
                textBox.Text = sHeader; // "Select Number Of Rules Areas";
                textBox.TextAlign = ContentAlignment.MiddleCenter;
                textBox.BackColor = color;  //Color.Green;
                inputBox.Controls.Add(textBox);
            }

            //add listbox
            System.Windows.Forms.ComboBox lst = new System.Windows.Forms.ComboBox();
            object[] obj = new object[cmbList.Items.Count];
            cmbList.Items.CopyTo(obj, 0);
            lst.Items.AddRange(obj);
            lst.Text = sHeader; // "Select Number Of Rules Areas";

            lst.Size = new System.Drawing.Size(size.Width - 200, size.Height - 100);
            lst.Location = new System.Drawing.Point(10, 70);
            lst.DropDownStyle = ComboBoxStyle.DropDownList;
            var font1 = lst.Font;
            System.Drawing.Font f1 = new System.Drawing.Font(font1.Name.ToString(), 10.0f);
            lst.Font = f1;
            lst.ContextMenuStrip = cms;

            inputBox.Controls.Add(lst);

            System.Windows.Forms.Button okButton = new System.Windows.Forms.Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";

            int btnLocY = size.Height - 40;
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, btnLocY);
            inputBox.Controls.Add(okButton);

            System.Windows.Forms.Button cancelButton = new System.Windows.Forms.Button();
            if (!xNoCancelButton)
            {
                cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                cancelButton.Name = "cancelButton";
                cancelButton.Size = new System.Drawing.Size(75, 23);
                cancelButton.Text = "&Cancel";
                cancelButton.Location = new System.Drawing.Point(size.Width - 80, btnLocY);
                inputBox.Controls.Add(cancelButton);
            }

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();

            input = lst.Text;
            return result;
        }

        public bool ProcessImg(IImage image, ISample samp, RunTimeWorkapace runTimeWorkapace, string modelName, IRedHighDetailParameters hdRedParams)
        {
            if (stream != null)
            {
                SampleViewerViewModel.Sample = stream.Process(image);
                IMarkingOverlayViewModel mm = SampleViewerViewModel.MarkingModel;
                int numFrames = SampleViewerViewModel.Sample.Frames.Count;
                string sampleName = SampleViewerViewModel.Sample.Name;
                ViDi2.IMarking marking = SampleViewerViewModel.Marking;
                IMarkingOverlayViewModel markingmodel = SampleViewerViewModel.MarkingModel;
                //textBox1.Text = stream.Workspace.UniqueName.ToString();
                System.Collections.ObjectModel.ReadOnlyCollection<ViDi2.IView> views = marking.Views;

                double duration = marking.Duration; //process time
                double durationPostProcess = marking.DurationPostProcess;
                double durationProcessOnly = marking.DurationProcessOnly;
                ViDi2.IImageInfo imageinfo = marking.ImageInfo;
                IEnumerable<ViDi2.IImageInfo> imageinfos = marking.ImageInfos;
                IEnumerable<ViDi2.ISetInfo> setinfo = marking.Sets;

                lblDuration.Text = duration.ToString("0.0000");
                lblProcessedSize.Text = marking.ImageInfo.Width.ToString() + "x" + marking.ImageInfo.Height;

                var views01 = SampleViewerViewModel.Sample.Markings[SampleViewerViewModel.ToolName].Views;
                RegionFound[] regions = RecordDefectsInfo(views01);

                ResultsFound resultsFound = new ResultsFound();
                resultsFound.regionFounds = regions;
                //resultsFound.imageName = cmbImageName.Text.Trim();
                resultsFound.modelName = modelName.Substring(0, modelName.Length - 5);
                resultsFound.thresholdLow = hdRedParams.Threshold.Lower.ToString("0.00");
                resultsFound.thresholdUpper = hdRedParams.Threshold.Upper.ToString("0.00");

                //roi
                resultsFound.roi.angle = Convert.ToDouble(this.txtROIangle.Text.Trim());
                resultsFound.roi.rect.X = Convert.ToDouble(this.txtPProiPosX.Text.Trim());
                resultsFound.roi.rect.Y = Convert.ToDouble(this.txtPProiPosY.Text.Trim());
                resultsFound.roi.rect.Width = Convert.ToDouble(this.txtPProiWidth.Text.Trim());
                resultsFound.roi.rect.Height = Convert.ToDouble(this.txtPProiHeight.Text.Trim());
                resultsFound.roi.xROIused = Convert.ToBoolean(this.chkUsePPevaluationROI.Checked);
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
                RaisePropertyChanged(nameof(ViewIndices));
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Load Runtime Model First, stream = null");
                goto exitProcedure;
            }
            exitProcedure:;
            return true;
        }

        public bool ProcessImg2(IImage image, ISample samp, RunTimeWorkapace runTimeWorkapace, string modelName, IRedHighDetailParameters hdRedParams)
        {
            if (stream != null)
            {
                SampleViewerViewModel.Sample = stream.Process(image);
                IMarkingOverlayViewModel mm = SampleViewerViewModel.MarkingModel;
                int numFrames = SampleViewerViewModel.Sample.Frames.Count;
                string sampleName = SampleViewerViewModel.Sample.Name;
                ViDi2.IMarking marking = SampleViewerViewModel.Marking;
                IMarkingOverlayViewModel markingmodel = SampleViewerViewModel.MarkingModel;
                //textBox1.Text = stream.Workspace.UniqueName.ToString();
                System.Collections.ObjectModel.ReadOnlyCollection<ViDi2.IView> views = marking.Views;

                double duration = marking.Duration; //process time
                double durationPostProcess = marking.DurationPostProcess;
                double durationProcessOnly = marking.DurationProcessOnly;
                ViDi2.IImageInfo imageinfo = marking.ImageInfo;
                IEnumerable<ViDi2.IImageInfo> imageinfos = marking.ImageInfos;
                IEnumerable<ViDi2.ISetInfo> setinfo = marking.Sets;

                lblDuration.Text = duration.ToString("0.0000");
                lblProcessedSize.Text = marking.ImageInfo.Width.ToString() + "x" + marking.ImageInfo.Height;

                var views01 = SampleViewerViewModel.Sample.Markings[SampleViewerViewModel.ToolName].Views;
                RegionFound[] regions = RecordDefectsInfo(views01);

                ResultsFound resultsFound = new ResultsFound();
                resultsFound.regionFounds = regions;
                //resultsFound.imageName = cmbImageName.Text.Trim();
                resultsFound.modelName = modelName.Substring(0, modelName.Length - 5);
                resultsFound.thresholdLow = hdRedParams.Threshold.Lower.ToString("0.00");
                resultsFound.thresholdUpper = hdRedParams.Threshold.Upper.ToString("0.00");

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
                RaisePropertyChanged(nameof(ViewIndices));

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Load Runtime Model First, stream = null");
                goto exitProcedure;
            }
            exitProcedure:;
            return true;
        }

        public bool ProcessImg1(IImage image, ISample[] samples, RunTimeWorkapace runTimeWorkapace)
        {
            bool defect = false;
            if (stream != null)
            {
                ISample samp1 = runTimeWorkapace.StreamDict[Model1Name].Process(image);
                ISample samp2 = runTimeWorkapace.StreamDict[Model2Name].Process(image);

                int numFrames1 = samp1.Frames.Count;
                int numFrames2 = samp2.Frames.Count;

                string sample1Name = samp1.Name;
                string sample2Name = samp2.Name;

                double ActualHeight = sampleviewr.sampleViewer.ActualHeight;
                double ActualWidth = sampleviewr.sampleViewer.ActualWidth;

                Dictionary<string, IMarking> mark = samp1.Markings;
                Dictionary<string, IMarking> mark2 = samp2.Markings;

                Dictionary<string, IMarking>.KeyCollection MarkKey = mark.Keys;
                Dictionary<string, IMarking>.KeyCollection MarkKey2 = mark2.Keys;
                IMarking TryM = mark["red_HDM_20M_5472x3648"];
                IMarking TryM2 = mark2["red_HDM_20M_5472x3648"];
                ViDi2.IView View = TryM.Views[0];// mm.Marking.Views[0];
                ViDi2.IView View2 = TryM2.Views[0];
                ViDi2.IRedView redview = (ViDi2.IRedView)View;
                ViDi2.IRedView redview2 = (ViDi2.IRedView)View2;

                double duration = TryM.Duration; //process time
                double durationPostProcess = TryM.DurationPostProcess;
                double durationProcessOnly = TryM.DurationProcessOnly;
                ViDi2.IImageInfo imageinfo = TryM.ImageInfo;
                IEnumerable<ViDi2.IImageInfo> imageinfos = TryM.ImageInfos;
                IEnumerable<ViDi2.ISetInfo> setinfo = TryM.Sets;

                double duration2 = TryM2.Duration; //process time
                double durationPostProcess2 = TryM2.DurationPostProcess;
                double durationProcessOnly2 = TryM2.DurationProcessOnly;
                ViDi2.IImageInfo imageinfo2 = TryM2.ImageInfo;
                IEnumerable<ViDi2.IImageInfo> imageinfos2 = TryM2.ImageInfos;
                IEnumerable<ViDi2.ISetInfo> setinfo2 = TryM2.Sets;

                var views01 = samp1.Markings[SampleViewerViewModel.ToolName].Views;
                var views02 = samp2.Markings[SampleViewerViewModel.ToolName].Views;

                RegionFound[] regionFound = new RegionFound[redview.Regions.Count];
                RegionFound[] regionFound2 = new RegionFound[redview2.Regions.Count];
                if (regionFound.Length > 0 || regionFound2.Length > 0)
                {
                    defect = true;
                }

                ViDi2.Runtime.IRedTool tool = (ViDi2.Runtime.IRedTool)runTimeWorkapace.StreamDict[Model1Name].Tools.First();
                ViDi2.Runtime.IRedTool tool2 = (ViDi2.Runtime.IRedTool)runTimeWorkapace.StreamDict[Model2Name].Tools.First();

                var knownClasses = tool.KnownClasses;
                var knownClasses2 = tool2.KnownClasses;

                string className = knownClasses[0];
                string className2 = knownClasses2[0];
                string[] s = className.Split('_');
                string[] s2 = className2.Split('_');
                string cn = "";
                string cn2 = "";
                if (s.GetLength(0) > 0)
                {
                    if (s.GetLength(0) > 1)
                        cn = s[1];
                    else
                        cn = s[0];
                }

                if (s2.GetLength(0) > 0)
                {
                    if (s2.GetLength(0) > 1)
                        cn2 = s2[1];
                    else
                        cn2 = s2[0];
                }
                string result;
                string result2;
                int index2 = 0;
                int Iindex = 0;
                resArr.resIndex = 0;
                resArr.resInfo.ShowRes = new string[20];
                int index = 0;

                if (redview.Regions.Count != 0)
                {
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

                        Iindex++;
                        index++;
                    }
                    BreakeRegions = regionFound;
                }
                if (redview2.Regions.Count != 0)
                {
                    foreach (ViDi2.IRegion item in redview2.Regions)
                    {
                        regionFound2[index2].area = item.Area;
                        regionFound2[index2].width = item.Width;
                        regionFound2[index2].height = item.Height;
                        regionFound2[index2].center = item.Center;
                        regionFound2[index2].score = item.Score;
                        regionFound2[index2].className = cn;  // item.Name; region name
                        regionFound2[index2].classColor = item.Color;
                        regionFound2[index2].compactness = item.Compactness;
                        regionFound2[index2].covers = item.Covers;
                        regionFound2[index2].outer = item.Outer;
                        regionFound2[index2].perimeter = item.Perimeter;
                        Iindex++;
                        index2++;
                    }
                    PealRegions = regionFound2;
                }
                ResultsFound resultsFound1 = new ResultsFound();
                ResultsFound resultsFound2 = new ResultsFound();

                bool xNotWorking = true;
                if (!xNotWorking)
                {
                    ViDi2.IImage I = samp1.Image;
                    ViDi2.IImage I2 = samp2.Image;

                    using (Graphics g = Graphics.FromImage(samp1.Image.Bitmap))  //using (Graphics g = Graphics.FromImage(I.Bitmap))
                    {

                        string s1 = "1";
                        string sDisp = "MN " + s1;
                        float fontsize = 10.0f;

                        Brush bb = Brushes.Red;

                        float x = 500.0f;
                        float y = 500.0f;
                        g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Regular), bb, x, y);

                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Load Runtime Model First, stream = null");
                goto exitProcedure;

            }
            exitProcedure:;
             return defect;
        }

        string[] GetFile(string path)
        {
            string[] s = new string[59];
            int iNumOfParams = 0;
            int iNumOfParams01 = 0;
            int totalNumLines = 0;

            try
            {
                //get number of lines
                using (FileStream fs = File.Open(@path, FileMode.Open))
                {
                    //get total number of lines
                    StreamReader sr = new StreamReader(fs, System.Text.UTF8Encoding.UTF8);
                    while (!sr.EndOfStream)
                    {
                        string ss = sr.ReadLine();
                        iNumOfParams = iNumOfParams + 1;
                    }

                    fs.Close();
                }

                totalNumLines = iNumOfParams + 3;
                s = new string[totalNumLines];

                using (FileStream fs = File.Open(@path, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs, System.Text.UTF8Encoding.UTF8);
                    while (!sr.EndOfStream)
                    {
                        string ss = sr.ReadLine();
                        s[iNumOfParams01] = ss;
                        iNumOfParams01 = iNumOfParams01 + 1;
                    }

                    fs.Close();
                    s[totalNumLines - 2] = "OK";
                    s[totalNumLines - 1] = iNumOfParams.ToString();
                }

            }
            catch (System.Exception e)
            {
                if (totalNumLines > 2)
                {
                    s[totalNumLines - 2] = "ERROR";
                }
                else
                {
                    totalNumLines = 3;
                    s = new string[totalNumLines];
                    s[totalNumLines - 2] = "ERROR";
                }

                System.Windows.Forms.MessageBox.Show("GetFile(), Error Getting File : " + e.Message, "Error!");
            }

            return s;
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

        private System.Windows.Rect GetDeticatedRoiFromList(int ImageIndex)
        {
            System.Windows.Rect rect = new System.Windows.Rect();

            string roiFile = System.Windows.Forms.Application.StartupPath + @"\Data\ROI Batch\ROIbatch.dat";
            string[] ddROIs = GetFile(roiFile);
            string[] ddROIsDataOnly = new string[ddROIs.GetLength(0) - 3];
            Array.Copy(ddROIs, ddROIsDataOnly, ddROIs.GetLength(0) - 3);

            List<string> lst = new List<string>();
            lst.AddRange(ddROIsDataOnly);

            string result = "";
            List<string> result01 = new List<string>();

            if (ImageIndex < 10)
            {
                result = lst.FirstOrDefault(s => s.Substring(0, 2) == (ImageIndex.ToString() + ","));

                result01 = lst.FindAll(s => s.Substring(0, 2) == (ImageIndex.ToString() + ","));
            }
            else if (ImageIndex > 9 && ImageIndex < 100)
            {
                result = lst.FirstOrDefault(s => s.Substring(0, 3) == (ImageIndex.ToString() + ","));

                result01 = lst.FindAll(s => s.Substring(0, 3) == (ImageIndex.ToString() + ","));

            }
            else if (ImageIndex > 99 && ImageIndex < 1000)
            {
                result = lst.FirstOrDefault(s => s.Substring(0, 4) == (ImageIndex.ToString() + ","));
                result01 = lst.FindAll(s => s.Substring(0, 4) == (ImageIndex.ToString() + ","));
            }
            if (result01.Count > 1)
            {
                //display input dialog for selection of VP to copy as
               System.Windows.Forms.ComboBox list01 = new System.Windows.Forms.ComboBox();
                list01.Items.AddRange(result01.ToArray());

                int iSize = 400;
                string selectedROI = " ";
                ContextMenuStrip contextMenuStrip13 = new ContextMenuStrip();
                bool xNoCancelButton = true;
                DialogResult dr1 = ShowInputDialog(ref selectedROI, list01, "Select roi to use", Color.GreenYellow, iSize, ref contextMenuStrip13, xNoCancelButton);

                if (dr1 == DialogResult.Cancel) { goto exitProcedure; }
                else
                {
                    //continue
                    result = selectedROI;
                }

            }

            string[] sRect = result.Split(',');

            rect.X = Convert.ToDouble(sRect[1]);
            rect.Y = Convert.ToDouble(sRect[2]);
            rect.Width = Convert.ToDouble(sRect[3]);
            rect.Height = Convert.ToDouble(sRect[4]);

            exitProcedure:;
            return rect;

        }

        EndmillParameters GetIniInfo(string iniFilePath, string endmillName)
            {
                EndmillParameters myParam = new EndmillParameters();
                using (StreamReader reader = new StreamReader(iniFilePath))
                {
                    Dictionary<string, Dictionary<string, string>> iniData = new Dictionary<string, Dictionary<string, string>>();
                    string line;
                    string currentSection = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();

                        if (line.StartsWith("[") && line.EndsWith("]"))
                        {
                            currentSection = line.Substring(1, line.Length - 2);
                            iniData[currentSection] = new Dictionary<string, string>();
                        }
                        if (!string.IsNullOrEmpty(currentSection) && line.Contains("="))
                        {
                            string[] parts = line.Split(new char[] { '=' }, 2);
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();
                            iniData[currentSection][key] = value;
                        }
                    }

                    // Retrieve the BladesNo of the specified Endmill
                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("CatalogNO"))
                    {
                        if (int.TryParse(iniData[endmillName]["CatalogNO"], out int Catalog))
                        {
                            myParam.CatalogNo = Catalog;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("BladesNo"))
                    {
                        if (int.TryParse(iniData[endmillName]["BladesNo"], out int bladesNo))
                        {
                            myParam.BladesNo = bladesNo;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("Diameter"))
                    {
                        if (int.TryParse(iniData[endmillName]["Diameter"], out int diam))
                        {
                            myParam.Diameter = diam;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("Length"))
                    {
                        if (int.TryParse(iniData[endmillName]["Length"], out int Len))
                        {
                            myParam.Length = Len;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("FracLowerThreshold"))
                    {
                        if (float.TryParse(iniData[endmillName]["FracLowerThreshold"], out float low))
                        {
                            myParam.FracLowerThreshold = low;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("FracUpperThreshold"))
                    {
                        if (float.TryParse(iniData[endmillName]["FracUpperThreshold"], out float upp))
                        {
                            myParam.FracUpperThreshold = upp;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("PeelLowerThreshold"))
                    {
                        if (float.TryParse(iniData[endmillName]["PeelLowerThreshold"], out float low))
                        {
                            myParam.PeelLowerThreshold = low;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("PeelUpperThreshold"))
                    {
                        if (float.TryParse(iniData[endmillName]["PeelUpperThreshold"], out float upp))
                        {
                            myParam.PeelUpperThreshold = upp;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiPosX1"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiPosX1"], out int posX1))
                        {
                            myParam.roiPosX1 = posX1;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiPosY1"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiPosY1"], out int posY1))
                        {
                            myParam.roiPosY1 = posY1;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiWidth1"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiWidth1"], out int width1))
                        {
                            myParam.roiWidth1 = width1;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiHeight1"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiHeight1"], out int height1))
                        {
                            myParam.roiHeight1 = height1;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiAngle1"))
                    {
                        if (float.TryParse(iniData[endmillName]["roiAngle1"], out float angle1))
                        {
                            myParam.roiAngle1 = angle1;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiRatio1"))
                    {
                        if (float.TryParse(iniData[endmillName]["roiRatio1"], out float ratio1))
                        {
                            myParam.roiRatio1 = ratio1;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiPosX2"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiPosX2"], out int posX2))
                        {
                            myParam.roiPosX2 = posX2;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiPosY2"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiPosY2"], out int posY2))
                        {
                            myParam.roiPosY2 = posY2;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiWidth2"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiWidth2"], out int width2))
                        {
                            myParam.roiWidth2 = width2;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiHeight2"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiHeight2"], out int height2))
                        {
                            myParam.roiHeight2 = height2;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiAngle2"))
                    {
                        if (float.TryParse(iniData[endmillName]["roiAngle2"], out float angle2))
                        {
                            myParam.roiAngle2 = angle2;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiRatio2"))
                    {
                        if (float.TryParse(iniData[endmillName]["roiRatio2"], out float ratio2))
                        {
                            myParam.roiRatio2 = ratio2;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiPosX3"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiPosX3"], out int posX3))
                        {
                            myParam.roiPosX3 = posX3;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiPosY3"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiPosY3"], out int posY3))
                        {
                            myParam.roiPosY3 = posY3;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiWidth3"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiWidth3"], out int width3))
                        {
                            myParam.roiWidth3 = width3;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiHeight3"))
                    {
                        if (int.TryParse(iniData[endmillName]["roiHeight3"], out int height3))
                        {
                            myParam.roiHeight3 = height3;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiAngle3"))
                    {
                        if (float.TryParse(iniData[endmillName]["roiAngle3"], out float angle3))
                        {
                            myParam.roiAngle3 = angle3;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("roiRatio3"))
                    {
                        if (float.TryParse(iniData[endmillName]["roiRatio3"], out float ratio3))
                        {
                            myParam.roiRatio3 = ratio3;
                        }
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("ImagePath"))
                    {
                        string ImagePath = iniData[endmillName]["ImagePath"];
                        myParam.ImagePath = ImagePath;
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("IsFractions1"))
                    {
                        string IsFractions1 = iniData[endmillName]["IsFractions1"];
                        myParam.IsFractions1 = IsFractions1;
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("IsPeels1"))
                    {
                        string IsPeels1 = iniData[endmillName]["IsPeels1"];
                        myParam.IsPeels1 = IsPeels1;
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("IsFractions2"))
                    {
                        string IsFractions2 = iniData[endmillName]["IsFractions2"];
                        myParam.IsFractions2 = IsFractions2;
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("IsPeels2"))
                    {
                        string IsPeels2 = iniData[endmillName]["IsPeels2"];
                        myParam.IsPeels2 = IsPeels2;
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("IsFractions3"))
                    {
                        string IsFractions3 = iniData[endmillName]["IsFractions3"];
                        myParam.IsFractions3 = IsFractions3;
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("IsPeels3"))
                    {
                        string IsPeels3 = iniData[endmillName]["IsPeels3"];
                        myParam.IsPeels3 = IsPeels3;
                    }

                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("Roi1"))
                    {
                        string roi1 = iniData[endmillName]["Roi1"];
                        myParam.Roi1 = roi1;
                    }
                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("Roi2"))
                    {
                        string roi2 = iniData[endmillName]["Roi2"];
                        myParam.Roi2 = roi2;
                    }
                    if (iniData.ContainsKey(endmillName) && iniData[endmillName].ContainsKey("Roi3"))
                    {
                        string roi3 = iniData[endmillName]["Roi3"];
                        myParam.Roi3 = roi3;
                    }
                }

                return myParam;
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

        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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

            var to = sampleviewr.sampleViewer.TouchesOver;
            var trig = sampleviewr.sampleViewer.Triggers;
            System.Windows.Size size01 = sampleviewr.sampleViewer.RenderSize;
            var cm = sampleviewr.sampleViewer.ContextMenu;
        }

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

        private void initProporties()
        {
            ViDi2.Runtime.IStream stream = Stream;
            ViDi2.Runtime.IWorkspace workspace = Workspace;
            ViDi2.Runtime.IControl control = Control;
            IList<ViDi2.Runtime.IWorkspace> ws = Workspaces;
            Dictionary<int, string> dic = ViewIndices;
        }

        private void elementHost2_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            RoutedEvent mb = e.RoutedEvent;
        }

        private frmEndmillNewTab.RatioXratioY GetRatioXandRetioY(PictureBox pbxShowImage, CurrentImage currentImage)
        {
            frmEndmillNewTab.RatioXratioY ratioXY = new frmEndmillNewTab.RatioXratioY();
            if (pbxShowImage.Image != null)
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

        private void ROIeditMode(int x, int y, int width, int height)
        {
            //btnGetRecInfo.Enabled = true;

            RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxEditRoi, gcurrentImage);

            float ratioX = ratioxy.ratioX;
            float ratioY = ratioxy.ratioY;

            //if roi stored in ini use it for new roi
            string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
            IniFile AppliIni = new IniFile(INIpath);
            System.Windows.Rect rect = new System.Windows.Rect();

            rect.X = x;//Convert.ToDouble(AppliIni.ReadValue("roi", "x", "0"));
            rect.Y = y;// Convert.ToDouble(AppliIni.ReadValue("roi", "y", "0"));
            rect.Width = width;// Convert.ToDouble(AppliIni.ReadValue("roi", "width", "0"));
            rect.Height = height;// Convert.ToDouble(AppliIni.ReadValue("roi", "height", "0"));

            if (StaticROI != null)
            {
                StaticROI = null;
            }

            StaticROI = new RectangleResizeAndRotateAdd(Convert.ToSingle(rect.X * (1 / ratioX)), Convert.ToSingle(rect.Y * (1 / ratioY)), Convert.ToSingle(rect.Width * (1 / ratioX)), Convert.ToSingle(rect.Height * (1 / ratioY)));

            //recAdd[bboxIndex].useAsROI = "roi";
            StaticROI.SetPictureBox(this.pbxEditRoi);

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

        private void GetROIinfo1()
        {
            int iRectangleNumber = 0;

            if (pbxEditRoi.Image != null)
            {
                RectangleResizeAndRotateAdd.RectangleFullDetails srtaticROI = StaticROI.GetRectF();

                RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxEditRoi, gcurrentImage);

                float ratioX = ratioxy.ratioX;
                float ratioY = ratioxy.ratioY;


                txtPosX2.Text = (Math.Round((srtaticROI.translation.X + srtaticROI.rectangleF.Left) * ratioX, 0)).ToString("0");
                txtPosY2.Text = (Math.Round((srtaticROI.translation.Y + srtaticROI.rectangleF.Top) * ratioY, 0)).ToString("0");
                txtWidth2.Text = Math.Round(srtaticROI.rectangleF.Width * ratioX, 0).ToString();
                txtHeight2.Text = Math.Round(srtaticROI.rectangleF.Height * ratioY, 0).ToString();
                txtAngle2.Text = Math.Round(srtaticROI.angle, 3).ToString();
                //this.txtAngle.Text = recAll.angle.ToString("0.00");

                int imgX = this.pbxEditRoi.Image.Width;
                int imgY = this.pbxEditRoi.Image.Height;

                int ImageArea = imgX * imgY; //pixels^2
                float roiArea = srtaticROI.rectangleF.Width * srtaticROI.rectangleF.Height;
                txtRatio2.Text = (roiArea / ImageArea).ToString("0.00");
            }
        }

        private void GetROIinfo2()
        {
            int iRectangleNumber = 0;

            if (pbxEditRoi.Image != null)
            {
                RectangleResizeAndRotateAdd.RectangleFullDetails srtaticROI = StaticROI.GetRectF();

                RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxEditRoi, gcurrentImage);

                float ratioX = ratioxy.ratioX;
                float ratioY = ratioxy.ratioY;


                txtPosX3.Text = (Math.Round((srtaticROI.translation.X + srtaticROI.rectangleF.Left) * ratioX, 0)).ToString("0");
                txtPosY3.Text = (Math.Round((srtaticROI.translation.Y + srtaticROI.rectangleF.Top) * ratioY, 0)).ToString("0");
                txtWidth3.Text = Math.Round(srtaticROI.rectangleF.Width * ratioX, 0).ToString();
                txtHeight3.Text = Math.Round(srtaticROI.rectangleF.Height * ratioY, 0).ToString();
                txtAngle3.Text = Math.Round(srtaticROI.angle, 3).ToString();
                //this.txtAngle.Text = recAll.angle.ToString("0.00");

                int imgX = this.pbxEditRoi.Image.Width;
                int imgY = this.pbxEditRoi.Image.Height;

                int ImageArea = imgX * imgY; //pixels^2
                float roiArea = srtaticROI.rectangleF.Width * srtaticROI.rectangleF.Height;
                txtRatio3.Text = (roiArea / ImageArea).ToString("0.00");
            }
        }

        private void loadModels()
        {
            try
            {
                string modelPath = "";
                IniFile AppliIni = new IniFile(INIpath);
                modelPath = AppliIni.ReadValue("Last Model", "Full path", "");

                if (true)
                {
                    using (var fs = new System.IO.FileStream(modelPath, System.IO.FileMode.Open, FileAccess.Read))
                    {
                        System.Windows.Forms.Application.DoEvents();

                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
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
                            //foreach (var item in lstModels.Items)
                            //{
                            //    System.Windows.Forms.Application.DoEvents();

                            //    string modelPath1 = txtModelsPath.Text.Trim() + @"\" + item.ToString().Trim();
                            //    using (var fs1 = new System.IO.FileStream(modelPath1, System.IO.FileMode.Open, FileAccess.Read))
                            //    {
                            //        control.Workspaces.Add(Path.GetFileNameWithoutExtension(modelPath1), fs1);
                            //    }
                            //}
                        }

                        rwsl = control.Workspaces;
                        Workspace = rwsl[rwsl.Names[0]];

                        string[] s = new string[0];
                        if (Workspace.Parameters.Description != "")
                            s = Workspace.Parameters.Description.Split(' ');
                        if (s.GetLength(0) > 0)
                        {
                            float f = System.Convert.ToSingle(s[s.GetLength(0) - 1]);
                        }

                        Mouse.OverrideCursor = null;
                    }
                }

                RaisePropertyChanged(nameof(Workspaces));

                stream = Workspace.Streams.First();

                string streamName = stream.Name; 
                ViDi2.Runtime.ITool tool = Workspace.Streams[streamName].Tools.First();

                //class name display
                ViDi2.Runtime.IRedTool tool1 = (ViDi2.Runtime.IRedTool)stream.Tools.First();
                var knownClasses = tool1.KnownClasses;
                if (knownClasses.Count > 0)
                {
                    string className = knownClasses[0];
                }

                //will activate event on lst
                string fn = Path.GetFileName(modelPath);
                //int index = this.lstModels.Items.IndexOf(fn);
                //this.lstModels.SelectedIndex = index;

            }
            catch (ViDi2.Exception e)
            {
                System.Windows.Forms.MessageBox.Show("loadModel(), Error: " + e.Message);
            }

            exitProcedure:;

        }

        private Dictionary<string, string> ReadIniFile(string filePath)
        {
            var iniData = new Dictionary<string, string>();

            foreach (var line in File.ReadLines(filePath))
            {
                if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith(";"))
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        iniData[key] = value;
                    }
                }
            }

            return iniData;
        }

        private void LoadComboBoxFromIniFile(string filePath, System.Windows.Forms.ComboBox comboBox)
        {
            var iniData = ReadIniFile(filePath);

            comboBox.Items.Clear();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith("[E") && line.EndsWith("]"))
                    {
                        string l = line.Substring(1, line.Length - 2);
                        comboBox.Items.Add(l);
                    }
                }
            }

        }

        private void ApplyROI1(bool xNoSave, Rectangle ImageDimensions, bool xAutoROIU, System.Windows.Rect rect, string posX, string posY, string width, string height, string angle, string ratio)
        {
            string iniFileC = System.Windows.Forms.Application.StartupPath + @"/Data/DataBase/" + CmbCatNum.Text + ".ini";
            ViDi2.IManualRegionOfInterest redROI01 = (ViDi2.IManualRegionOfInterest)Stream.Tools.First().RegionOfInterest;
            redROI01.Parameters.Units = ViDi2.UnitsMode.Pixel;
            if (!chkFullImg.Checked)
            { 
                if (chkROI1.Checked)
                {
                    double ROIXpos = 0;
                    double ROIYpos = 0;
                    double ROIwidth = 0;
                    double ROIheight = 0;
                    double ROIangle = 0;

                    if (!xAutoROIU)
                    {
                        ROIXpos = Convert.ToDouble(posX);
                        ROIYpos = Convert.ToDouble(posY);
                        ROIwidth = Convert.ToDouble(width);
                        ROIheight = Convert.ToDouble(height);
                        ROIangle = Convert.ToDouble(angle);
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
                        IniFile AppliIni = new IniFile(iniFileC);
                        AppliIni.WriteValue(CmbCatNum.Text, "xroiPosX1", currentROI.X.ToString());
                        AppliIni.WriteValue(CmbCatNum.Text, "roiPosY1", currentROI.Y.ToString());
                        AppliIni.WriteValue(CmbCatNum.Text, "roiWidth1", currentROI.Width.ToString());
                        AppliIni.WriteValue(CmbCatNum.Text, "roiHeight1", currentROI.Height.ToString());

                        //AppliIni.WriteValue(CmbCatNum.Text, "used", chkUsePPevaluationROI.Checked.ToString());
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
                        ratio = ((arearoi / areaimg)).ToString("0.000");
                    else
                        ratio = "***";

                }
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
                ratio = ((areaimg / areaimg)).ToString(("0.000"));

                if (!(currentROI.Height == 1 && currentROI.Width == 1))
                {
                    IniFile AppliIni = new IniFile(INIpath);
                    //AppliIni.WriteValue("roi", "x", currentROI.X.ToString());
                    //AppliIni.WriteValue("roi", "y", currentROI.Y.ToString());
                    //AppliIni.WriteValue("roi", "width", currentROI.Width.ToString());
                    //AppliIni.WriteValue("roi", "height", currentROI.Height.ToString());

                   // AppliIni.WriteValue("roi", "used", chkUsePPevaluationROI.Checked.ToString());
                }
            }
        }


        private void ApplyROISimple(Rectangle ImageDimensions)
        {
            ViDi2.IManualRegionOfInterest redROI01 = (ViDi2.IManualRegionOfInterest)Stream.Tools.First().RegionOfInterest;
            redROI01.Parameters.Units = ViDi2.UnitsMode.Pixel;

            redROI01.Parameters.Offset = new ViDi2.Point(0, 0);
            redROI01.Parameters.Size = new ViDi2.Size(ImageDimensions.Width, ImageDimensions.Height);


        }



        private void ApplyROI(bool xNoSave, Rectangle ImageDimensions, bool xAutoROIU, System.Windows.Rect rect)
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

        private void ApplyROIRect(bool xNoSave, Rectangle ImageDimensions, bool xAutoROIU, System.Windows.Rect rect)
        {
            ViDi2.IManualRegionOfInterest redROI01 = (ViDi2.IManualRegionOfInterest)Stream.Tools.First().RegionOfInterest;
            redROI01.Parameters.Units = ViDi2.UnitsMode.Pixel;
            if (!chkFullImg.Checked)
            {
                if (chkROI1.Checked)
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

                if (chkROI2.Checked)
                {
                    double ROIXpos1 = 0;
                    double ROIYpos1 = 0;
                    double ROIwidth1 = 0;
                    double ROIheight1 = 0;
                    double ROIangle1 = 0;

                    if (!xAutoROIU)
                    {
                        ROIXpos1 = Convert.ToDouble(txtPosX2.Text.Trim());
                        ROIYpos1 = Convert.ToDouble(txtPosY2.Text.Trim());
                        ROIwidth1 = Convert.ToDouble(txtWidth2.Text.Trim());
                        ROIheight1 = Convert.ToDouble(txtHeight2.Text.Trim());
                        ROIangle1 = Convert.ToDouble(txtAngle2.Text.Trim());
                    }
                    else //auto-mode
                    {
                        ROIXpos1 = rect.X;
                        ROIYpos1 = rect.Y;
                        ROIwidth1 = rect.Width;
                        ROIheight1 = rect.Height;
                        ROIangle1 = 0;
                    }

                    redROI01.Parameters.Offset = new ViDi2.Point(ROIXpos1, ROIYpos1);
                    redROI01.Parameters.Size = new ViDi2.Size(ROIwidth1, ROIheight1);

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
                        txtRatio2.Text = ((arearoi / areaimg)).ToString("0.000");
                    else
                        txtRatio2.Text = "***";

                }

                if (chkROI3.Checked)
                {
                    double ROIXpos = 0;
                    double ROIYpos = 0;
                    double ROIwidth = 0;
                    double ROIheight = 0;
                    double ROIangle = 0;

                    if (!xAutoROIU)
                    {
                        ROIXpos = Convert.ToDouble(txtPosX3.Text.Trim());
                        ROIYpos = Convert.ToDouble(txtPosY3.Text.Trim());
                        ROIwidth = Convert.ToDouble(txtWidth3.Text.Trim());
                        ROIheight = Convert.ToDouble(txtHeight3.Text.Trim());
                        ROIangle = Convert.ToDouble(txtAngle3.Text.Trim());
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
                        txtRatio3.Text = ((arearoi / areaimg)).ToString("0.000");
                    else
                        txtRatio3.Text = "***";

                }
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
                txtRatio2.Text = ((areaimg / areaimg)).ToString(("0.000"));

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

        private bool LoadImage()
        {

            bool xLoadOK = false;

            if (true) //File.Exists(pathandbname))
            {
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
         
                this.pbxEditRoi.Image = (System.Drawing.Image)img.Clone();

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

        private bool LoadImage1(Bitmap bmp)
        {
            bool xLoadOK = false;
            if (true)
            {
                gcurrentImage.imgWidth = bmp.Width;
                gcurrentImage.imgHeight = bmp.Height;

                float imgWidth = Convert.ToSingle(bmp.Width);
                float imgHeight = Convert.ToSingle(bmp.Height);
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

                int height = this.pbxEditRoi.Height;
                float fHeight = Convert.ToSingle(width) / fAspectRatio;

                Bitmap img = new Bitmap(bmp, width, Convert.ToInt32(fHeight));

                this.pbxEditRoi.Image = (System.Drawing.Image)img.Clone();
                this.pbxEditRoi.SizeMode =  PictureBoxSizeMode.AutoSize;

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

        /*Listeners*/

        private void frmEndmillNewTab_Load(object sender, EventArgs e)
        {
            string filePath = System.Windows.Forms.Application.StartupPath + @"\Data\DataBase\FilesNames.ini";
            string jsonContent = File.ReadAllText(JassonPath);
            List<Dictionary<string, object>> endmillData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonContent);
            List<string> endmillNames = new List<string>();

            foreach (var data in endmillData)
            {
                if (data.ContainsKey("EndmillName"))
                {
                    string endmillName = data["EndmillName"].ToString();
                    endmillNames.Add(endmillName);
                }
            }
            CmbCatNum.DataSource = endmillNames;
            //LoadComboBoxFromIniFile(filePath, CmbCatNum);
            List<int> GPUList = new List<int>();
            try
            {
                string[] sMode = System.Windows.Forms.Application.StartupPath.Split('\\');
                lblRunMode.Visible = false;
                lblRunMode.Text = "Run mode: " + sMode[sMode.Length - 1];

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
                var computeDevices = control.ComputeDevices;
                control.OptimizedGPUMemory(0);
                this.Control = control;

                var StreamDict = new Dictionary<string, ViDi2.Runtime.IStream>();
                string gpuID = "default/red_HDM_20M_5472x3648/0";
                string wsName = Model1Name;
                string wsPath = System.Windows.Forms.Application.StartupPath + @"\Data\final models\Proj_021_201223_104500_21122023_104445.vrws";
                string wsName2 = Model2Name;
                string wsPath2 = System.Windows.Forms.Application.StartupPath + @"\Data\final models\WS_Proj_022_261223_111400_261223_183645.vrws";
                StreamDict.Add(wsName, control.Workspaces.Add(wsName, wsPath, gpuID).Streams["default"]);
                StreamDict.Add(wsName2, control.Workspaces.Add(wsName2, wsPath2, gpuID).Streams["default"]);
                grunTimeWorkapace.gpuId01 = 0;
                grunTimeWorkapace.StreamDict = StreamDict;

                //license
                string licenseType = "License: " + control.License.Type.ToString() + " " + control.License.PerformanceLevel.ToString() + ", Expires Days: " + control.License.Expires.Days.ToString();
                sversion = licenseType + ", " + sversion;
                this.Text = sversion;
                textBox1.Text = sversion;
                //
                sampleviewr.sampleViewer.Drop += btnDefect_Click;
                SampleViewerViewModel.ToolSelected += sampleViewer_ToolSelected;
                sampleviewr.sampleViewer.MouseDown += sampleViewer_MouseDown;
                sampleviewr.Visibility = System.Windows.Visibility.Visible;
                Var = sampleviewr.MainWindow.Parent;

                elementHost1.Child = sampleviewr;

                initProporties();


                //necessary for auto marking on image in non-WPF application
                MarkingOverlayExtensions.SetupOverlayEnvironment(1000, false, false);

                elementHost1.Child.MouseWheel += elementHost2_MouseWheel;

                string ImagePath = AppliIni.ReadValue("Last Image", "path", "");
                txtImgPathLoad.Text = ImagePath;

                string ImageName = AppliIni.ReadValue("Last Image", "name", "");

                string modelsPath = AppliIni.ReadValue("Models", "path", "");

                txtModelsPath.Text = modelsPath;
                string[] dirModels = Directory.GetFiles(modelsPath, "*.vrws");


                string[] dirImages = Directory.GetFiles(ImagePath);


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
                txtROIangle.Text = 0.ToString();

                xNoAction = true;
                chkUsePPevaluationROI.Checked = Convert.ToBoolean(AppliIni.ReadValue("roi", "used", "False"));

                xNoAction = false;

                Rectangle ImageDimensions = new Rectangle();
                System.Windows.Rect rect = new System.Windows.Rect();
                if (chkUsePPevaluationROI.Checked)
                    ApplyROI(true, ImageDimensions, false, rect);

                txtAreaFilterWidth.Text = AppliIni.ReadValue("OUTPUT FILTER", "Area Width", "");
                txtAreaFilterHeight.Text = AppliIni.ReadValue("OUTPUT FILTER", "Area Height", "");
                txtAreaFilterScore.Text = AppliIni.ReadValue("OUTPUT FILTER", "Score", "");
                chkFilterActive.Checked = Convert.ToBoolean(AppliIni.ReadValue("OUTPUT FILTER", "Used", ""));


            }
            catch (System.Exception e1)
            {
                System.Windows.Forms.MessageBox.Show("frmEndmillNewTab, Error: " + "\n\r\n\r" + e1.Message + "\n\r\n\rClosing Application");
                // this.Close();
                System.Windows.Forms.Application.Exit();
            }


        }


        private void Infer4()
        {
            {
                Stopwatch sw = new Stopwatch();
                sw.Restart();

                string ImagePath = Path.GetDirectoryName(txtImgPathLoad.Text.Trim());//txtImgPathLoad.Text.Trim();

                string ImageName = Path.GetFileName(txtImgPathLoad.Text.Trim());
                string fullPath = ImagePath + @"\" + ImageName;
                if (!File.Exists(fullPath)) { System.Windows.Forms.MessageBox.Show("Image or Path Don't Exist!,\n\r" + fullPath); goto exitProcedure; }

                //peels
                string modelFileName = "Proj_021_201223_104500_21122023_104445";
                string modelName = modelFileName.Trim() + ".vrws";  // this.lstModels.SelectedItem.ToString();
                var image = new ViDi2.UI.WpfImage(fullPath);   // imagePath);


                Rectangle ImageDimensions = new Rectangle(0, 0, image.Width, image.Height);
                ApplyROISimple(ImageDimensions);

                //--------------------------set thresholds--------------------------------
                IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;

                ViDi2.Runtime.IRedTool hdRedTool = (ViDi2.Runtime.IRedTool)Stream.Tools.First();


                ViDi2.Interval interval = new ViDi2.Interval(0.2, 0.9);
                hdRedParams.Threshold = interval;

                //output filter                
                IRedHighDetailParameters outputrFilterRed01 = hdRedTool.ParametersBase as IRedHighDetailParameters;  // ViDi2.IRedRegionOfInterestParameters;
                string filter = outputrFilterRed01.RegionFilter;

                filter = "area>= 64 and score> 0.15";
                outputrFilterRed01.RegionFilter = filter;

                //-----------------------------------process image----------------------------------
                SampleViewerViewModel.Sample = stream.Process(image);

                ViDi2.IMarking marking = SampleViewerViewModel.Marking;

                System.Collections.ObjectModel.ReadOnlyCollection<ViDi2.IView> views = marking.Views;

                double duration = marking.Duration; //process time
                double durationPostProcess = marking.DurationPostProcess;
                double durationProcessOnly = marking.DurationProcessOnly;
                ViDi2.IImageInfo imageinfo = marking.ImageInfo;
                IEnumerable<ViDi2.IImageInfo> imageinfos = marking.ImageInfos;

                IEnumerable<ViDi2.ISetInfo> setinfo = marking.Sets;

                duration = sw.ElapsedMilliseconds / 1000.0;
                lblDuration.Text = duration.ToString("0.0000");
                sw.Stop();
            }

        exitProcedure:;
        }


        private void Infer3()
        {
            {
                Stopwatch sw = new Stopwatch();
                sw.Restart();

                string ImagePath = Path.GetDirectoryName(txtImgPathLoad.Text.Trim());//txtImgPathLoad.Text.Trim();

                string ImageName = Path.GetFileName(txtImgPathLoad.Text.Trim());
                string fullPath = ImagePath + @"\" + ImageName;
                if (!File.Exists(fullPath)) { System.Windows.Forms.MessageBox.Show("Image or Path Don't Exist!,\n\r" + fullPath); goto exitProcedure; }

                //peels
                string modelFileName = "Proj_021_201223_104500_21122023_104445";
                //fractions
                //else modelFileName = "WS_Proj_022_261223_111400_261223_183645";
                string modelName = modelFileName.Trim() + ".vrws";  // this.lstModels.SelectedItem.ToString();
                var image = new ViDi2.UI.WpfImage(fullPath);   // imagePath);

                System.Windows.Rect ROIrect = new System.Windows.Rect();

                Rectangle ImageDimensions = new Rectangle(0, 0, image.Width, image.Height);
                ApplyROI(true, ImageDimensions, chkAutoROI.Checked, ROIrect);

                //--------------------------set thresholds--------------------------------
                IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;

                ViDi2.Runtime.IRedTool hdRedTool = (ViDi2.Runtime.IRedTool)Stream.Tools.First();


                bool xNotRedTool = false;
                if (hdRedParams == null) { xNotRedTool = true; }

                if (!xNotRedTool)
                {
                    ViDi2.Interval interval = new ViDi2.Interval(0.2, 0.9);
                    hdRedParams.Threshold = interval;
                }

                //output filter                
                IRedHighDetailParameters outputrFilterRed01 = hdRedTool.ParametersBase as IRedHighDetailParameters;  // ViDi2.IRedRegionOfInterestParameters;
                string filter = outputrFilterRed01.RegionFilter;

                string area = (Convert.ToInt32(txtAreaFilterWidth.Text.Trim()) * Convert.ToInt32(txtAreaFilterHeight.Text.Trim())).ToString();
                filter = "area>= 64 and score> 0.15";
                outputrFilterRed01.RegionFilter = filter;


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

                duration = sw.ElapsedMilliseconds / 1000.0;
                lblDuration.Text = duration.ToString("0.0000");
                sw.Stop();
            }

        exitProcedure:;
        }



        private void Infer2()
        {
            {
                Stopwatch sw = new Stopwatch();
                sw.Restart();

                string ImagePath = Path.GetDirectoryName(txtImgPathLoad.Text.Trim());//txtImgPathLoad.Text.Trim();

                string ImageName = Path.GetFileName(txtImgPathLoad.Text.Trim());
                string fullPath = ImagePath + @"\" + ImageName;
                if (!File.Exists(fullPath)) { System.Windows.Forms.MessageBox.Show("Image or Path Don't Exist!,\n\r" + fullPath); goto exitProcedure; }

                //peels
                string modelFileName= "Proj_021_201223_104500_21122023_104445";
                //fractions
                //else modelFileName = "WS_Proj_022_261223_111400_261223_183645";
                string modelName = modelFileName.Trim() + ".vrws";  // this.lstModels.SelectedItem.ToString();
                var image = new ViDi2.UI.WpfImage(fullPath);   // imagePath);

                System.Windows.Rect ROIrect = new System.Windows.Rect();

                Rectangle ImageDimensions = new Rectangle(0, 0, image.Width, image.Height);
                ApplyROI(true, ImageDimensions, chkAutoROI.Checked, ROIrect);

                //--------------------------set thresholds--------------------------------
                IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;

                ViDi2.Runtime.IRedTool hdRedTool = (ViDi2.Runtime.IRedTool)Stream.Tools.First();

                //double lower = 0.0;
                //double upper = 0.0;
                //if (IsNumeric(txtThresholdsLower.Text.Trim()))
                //{
                //    lower = System.Convert.ToDouble(txtThresholdsLower.Text.Trim());
                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("Check lower threshold, not a number!");
                //    goto exitProcedure;
                //}

                //if (IsNumeric(txtThresholdsUpper.Text.Trim()))
                //{
                //    upper = System.Convert.ToDouble(txtThresholdsUpper.Text.Trim());
                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("Check upper threshold, not a number!");
                //    goto exitProcedure;
                //}

                bool xNotRedTool = false;
                if (hdRedParams == null) { xNotRedTool = true; }

                if (!xNotRedTool)
                {
                    ViDi2.Interval interval = new ViDi2.Interval(0.2, 0.9);
                    hdRedParams.Threshold = interval;

                    //ViDi2.IParameters pb = Stream.Tools.First().ParametersBase;
                }

                //output filter                
                IRedHighDetailParameters outputrFilterRed01 = hdRedTool.ParametersBase as IRedHighDetailParameters;  // ViDi2.IRedRegionOfInterestParameters;
                string filter = outputrFilterRed01.RegionFilter;

                //if (!IsNumeric(txtAreaFilterWidth.Text.Trim())) { System.Windows.Forms.MessageBox.Show("Area Width, Not Numeric!"); goto exitProcedure; }
                //if (!IsNumeric(txtAreaFilterHeight.Text.Trim())) { System.Windows.Forms.MessageBox.Show("Area Height, Not Numeric!"); goto exitProcedure; }
                //if (IsNumeric(txtAreaFilterScore.Text.Trim())) { bool xscore = Convert.ToSingle(txtAreaFilterScore.Text.Trim()) < 1.0f; if (!xscore) { System.Windows.Forms.MessageBox.Show("Score >1.0"); goto exitProcedure; } }
                //else
                //{ System.Windows.Forms.MessageBox.Show("Score, Not Numeric!"); goto exitProcedure; }

                //bool xFilterActive = chkFilterActive.Checked;
                //if (xFilterActive)
                //{
                    //AppliIni.WriteValue("OUTPUT FILTER", "Area Width", txtAreaFilterWidth.Text.Trim());
                    //AppliIni.WriteValue("OUTPUT FILTER", "Area Height", txtAreaFilterHeight.Text.Trim());
                    //AppliIni.WriteValue("OUTPUT FILTER", "Score", txtAreaFilterScore.Text.Trim());
                    //AppliIni.WriteValue("OUTPUT FILTER", "Used", chkFilterActive.Checked.ToString());

                    string area = (Convert.ToInt32(txtAreaFilterWidth.Text.Trim()) * Convert.ToInt32(txtAreaFilterHeight.Text.Trim())).ToString();
                    filter = "area>= 64 and score> 0.15";
                    outputrFilterRed01.RegionFilter = filter;
                //}
                //else
                //{
                //    filter = "";
                //    outputrFilterRed01.RegionFilter = filter;

                //    //AppliIni.WriteValue("OUTPUT FILTER", "Area Width", txtAreaFilterWidth.Text.Trim());
                //    //AppliIni.WriteValue("OUTPUT FILTER", "Area Height", txtAreaFilterHeight.Text.Trim());
                //    //AppliIni.WriteValue("OUTPUT FILTER", "Score", txtAreaFilterScore.Text.Trim());
                //    //AppliIni.WriteValue("OUTPUT FILTER", "Used", chkFilterActive.Checked.ToString());
                //}


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

                duration = sw.ElapsedMilliseconds / 1000.0;
                lblDuration.Text = duration.ToString("0.0000");
                sw.Stop();

                //lblProcessedSize.Text = marking.ImageInfo.Width.ToString() + "x" + marking.ImageInfo.Height;

                //bool xForDebug = false;
                //if (xForDebug)
                //{
                //    saveMarkedImage();
                //}

                //var views01 = SampleViewerViewModel.Sample.Markings[SampleViewerViewModel.ToolName].Views;
                //RegionFound[] regions = RecordDefectsInfo(views01);

                //ResultsFound resultsFound = new ResultsFound();
                //resultsFound.regionFounds = regions;
                ////resultsFound.imageName = cmbImageName.Text.Trim();
                //resultsFound.modelName = modelName.Substring(0, modelName.Length - 5);
                //resultsFound.thresholdLow = hdRedParams.Threshold.Lower.ToString("0.00");
                //resultsFound.thresholdUpper = hdRedParams.Threshold.Upper.ToString("0.00");

                ////roi
                //resultsFound.roi.angle = Convert.ToDouble(this.txtROIangle.Text.Trim());
                //resultsFound.roi.rect.X = Convert.ToDouble(this.txtPProiPosX.Text.Trim());
                //resultsFound.roi.rect.Y = Convert.ToDouble(this.txtPProiPosY.Text.Trim());
                //resultsFound.roi.rect.Width = Convert.ToDouble(this.txtPProiWidth.Text.Trim());
                //resultsFound.roi.rect.Height = Convert.ToDouble(this.txtPProiHeight.Text.Trim());
                //resultsFound.roi.xROIused = Convert.ToBoolean(this.chkUsePPevaluationROI.Checked);

                ////try to write text on image
                //bool xNotWorking = true;
                //if (!xNotWorking)
                //{
                //    ViDi2.IImage I = SampleViewerViewModel.Sample.Image;

                //    using (Graphics g = Graphics.FromImage(SampleViewerViewModel.Sample.Image.Bitmap))  //using (Graphics g = Graphics.FromImage(I.Bitmap))
                //    {

                //        string s = "1";
                //        string sDisp = "MN " + s;
                //        float fontsize = 10.0f;

                //        Brush bb = Brushes.Red;

                //        float x = 500.0f;
                //        float y = 500.0f;
                //        g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Regular), bb, x, y);


                //    }
                //}

                //RaisePropertyChanged(nameof(ViewIndices));

            }
        //else
        //{
        //    System.Windows.Forms.MessageBox.Show("Load Runtime Model First, stream = null");
        //    goto exitProcedure;
        //}

        exitProcedure:;
        }



        private void Infer()
        {
            //if (stream != null)
            {
                //if (this.btnEditROI.Text == editmodeCaption)
                //{
                //    System.Windows.Forms.MessageBox.Show("ROI edit mode is active. Cancel roi Edit-Mode!");
                //    goto exitProcedure;
                //}
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                //double ActualHeight = sampleviewr.sampleViewer.ActualHeight;
                //double ActualWidth = sampleviewr.sampleViewer.ActualWidth;

                string ImagePath = Path.GetDirectoryName(txtImgPathLoad.Text.Trim());//txtImgPathLoad.Text.Trim();

                string ImageName = "";
                //if (cmbImageName.SelectedIndex > -1)
                //    ImageName = cmbImageName.Text;      //txtImageName.Text.Trim();
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("Select an Image");
                //    goto exitProcedure;
                //}
                ImageName = Path.GetFileName(txtImgPathLoad.Text.Trim());
                string fullPath = ImagePath + @"\" + ImageName;
                if (!File.Exists(fullPath)) { System.Windows.Forms.MessageBox.Show("Image or Path Don't Exist!,\n\r" + fullPath); goto exitProcedure; }

                this.Text = sversion + ", Current Image: " + fullPath;

                //string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";

                //IniFile AppliIni = new IniFile(INIpath);
                //AppliIni.WriteValue("Last Image", "path", ImagePath);

                ////AppliIni.WriteValue("Last Image", "name", cmbImageName.Text.Trim());  // ImageName);

                //AppliIni.WriteValue("Models", "path", txtModelsPath.Text.Trim());
                string modelFileName;
                if (txtRunTimeWSName.Text == "Peels")
                    modelFileName = "Proj_021_201223_104500_21122023_104445";
                else modelFileName = "WS_Proj_022_261223_111400_261223_183645";
                string modelName = modelFileName.Trim() + ".vrws";  // this.lstModels.SelectedItem.ToString();

                //AppliIni.WriteValue("Models", "model", modelName);

                //AppliIni.WriteValue("Last Model", "Full path", txtModelsPath.Text.Trim() + @"\" + modelName);

                var image = new ViDi2.UI.WpfImage(fullPath);   // imagePath);

                //
                System.Windows.Rect ROIrect = new System.Windows.Rect();
                //if (chkAutoROI.Checked)
                //    ROIrect = GetDeticatedRoiFromList(Convert.ToInt32(imgLstIndex.Text));

                Rectangle ImageDimensions = new Rectangle(0, 0, image.Width, image.Height);
                ApplyROI(true, ImageDimensions, chkAutoROI.Checked, ROIrect);

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

                //output filter                
                IRedHighDetailParameters outputrFilterRed01 = hdRedTool.ParametersBase as IRedHighDetailParameters;  // ViDi2.IRedRegionOfInterestParameters;
                string filter = outputrFilterRed01.RegionFilter;

                //if (!IsNumeric(txtAreaFilterWidth.Text.Trim())) { System.Windows.Forms.MessageBox.Show("Area Width, Not Numeric!"); goto exitProcedure; }
                //if (!IsNumeric(txtAreaFilterHeight.Text.Trim())) { System.Windows.Forms.MessageBox.Show("Area Height, Not Numeric!"); goto exitProcedure; }
                //if (IsNumeric(txtAreaFilterScore.Text.Trim())) { bool xscore = Convert.ToSingle(txtAreaFilterScore.Text.Trim()) < 1.0f; if (!xscore) { System.Windows.Forms.MessageBox.Show("Score >1.0"); goto exitProcedure; } }
                //else
                //{ System.Windows.Forms.MessageBox.Show("Score, Not Numeric!"); goto exitProcedure; }

                bool xFilterActive = chkFilterActive.Checked;
                if (xFilterActive)
                {
                    //AppliIni.WriteValue("OUTPUT FILTER", "Area Width", txtAreaFilterWidth.Text.Trim());
                    //AppliIni.WriteValue("OUTPUT FILTER", "Area Height", txtAreaFilterHeight.Text.Trim());
                    //AppliIni.WriteValue("OUTPUT FILTER", "Score", txtAreaFilterScore.Text.Trim());
                    //AppliIni.WriteValue("OUTPUT FILTER", "Used", chkFilterActive.Checked.ToString());

                    string area = (Convert.ToInt32(txtAreaFilterWidth.Text.Trim()) * Convert.ToInt32(txtAreaFilterHeight.Text.Trim())).ToString();
                    filter = "area>= " + area.ToString() + " and score>" + txtAreaFilterScore.Text.Trim();
                    outputrFilterRed01.RegionFilter = filter;
                }
                else
                {
                    filter = "";
                    outputrFilterRed01.RegionFilter = filter;

                    //AppliIni.WriteValue("OUTPUT FILTER", "Area Width", txtAreaFilterWidth.Text.Trim());
                    //AppliIni.WriteValue("OUTPUT FILTER", "Area Height", txtAreaFilterHeight.Text.Trim());
                    //AppliIni.WriteValue("OUTPUT FILTER", "Score", txtAreaFilterScore.Text.Trim());
                    //AppliIni.WriteValue("OUTPUT FILTER", "Used", chkFilterActive.Checked.ToString());
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

                duration = sw.ElapsedMilliseconds / 1000.0;
                lblDuration.Text = duration.ToString("0.0000");
                sw.Stop();

                //lblProcessedSize.Text = marking.ImageInfo.Width.ToString() + "x" + marking.ImageInfo.Height;

                //bool xForDebug = false;
                //if (xForDebug)
                //{
                //    saveMarkedImage();
                //}

                //var views01 = SampleViewerViewModel.Sample.Markings[SampleViewerViewModel.ToolName].Views;
                //RegionFound[] regions = RecordDefectsInfo(views01);

                //ResultsFound resultsFound = new ResultsFound();
                //resultsFound.regionFounds = regions;
                ////resultsFound.imageName = cmbImageName.Text.Trim();
                //resultsFound.modelName = modelName.Substring(0, modelName.Length - 5);
                //resultsFound.thresholdLow = hdRedParams.Threshold.Lower.ToString("0.00");
                //resultsFound.thresholdUpper = hdRedParams.Threshold.Upper.ToString("0.00");

                ////roi
                //resultsFound.roi.angle = Convert.ToDouble(this.txtROIangle.Text.Trim());
                //resultsFound.roi.rect.X = Convert.ToDouble(this.txtPProiPosX.Text.Trim());
                //resultsFound.roi.rect.Y = Convert.ToDouble(this.txtPProiPosY.Text.Trim());
                //resultsFound.roi.rect.Width = Convert.ToDouble(this.txtPProiWidth.Text.Trim());
                //resultsFound.roi.rect.Height = Convert.ToDouble(this.txtPProiHeight.Text.Trim());
                //resultsFound.roi.xROIused = Convert.ToBoolean(this.chkUsePPevaluationROI.Checked);

                ////try to write text on image
                //bool xNotWorking = true;
                //if (!xNotWorking)
                //{
                //    ViDi2.IImage I = SampleViewerViewModel.Sample.Image;

                //    using (Graphics g = Graphics.FromImage(SampleViewerViewModel.Sample.Image.Bitmap))  //using (Graphics g = Graphics.FromImage(I.Bitmap))
                //    {

                //        string s = "1";
                //        string sDisp = "MN " + s;
                //        float fontsize = 10.0f;

                //        Brush bb = Brushes.Red;

                //        float x = 500.0f;
                //        float y = 500.0f;
                //        g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Regular), bb, x, y);


                //    }
                //}

                //RaisePropertyChanged(nameof(ViewIndices));

            }
            //else
            //{
            //    System.Windows.Forms.MessageBox.Show("Load Runtime Model First, stream = null");
            //    goto exitProcedure;
            //}

        exitProcedure:;
        }



        private void btnDefect_Click(object sender, EventArgs e)
        {
            //NPNP
            Infer4();
            return;
            if (stream != null)
            {
                if (this.btnEditROI.Text == editmodeCaption)
                {
                    System.Windows.Forms.MessageBox.Show("ROI edit mode is active. Cancel roi Edit-Mode!");
                    goto exitProcedure;
                }
                Stopwatch sw = new Stopwatch();
                sw.Restart();
                double ActualHeight = sampleviewr.sampleViewer.ActualHeight;
                double ActualWidth = sampleviewr.sampleViewer.ActualWidth;

                string ImagePath = Path.GetDirectoryName(txtImgPathLoad.Text.Trim());//txtImgPathLoad.Text.Trim();

                string ImageName = "";
                //if (cmbImageName.SelectedIndex > -1)
                //    ImageName = cmbImageName.Text;      //txtImageName.Text.Trim();
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("Select an Image");
                //    goto exitProcedure;
                //}
                ImageName = Path.GetFileName(txtImgPathLoad.Text.Trim());
                string fullPath = ImagePath + @"\" + ImageName;
                if (!File.Exists(fullPath)) { System.Windows.Forms.MessageBox.Show("Image or Path Don't Exist!,\n\r" + fullPath); goto exitProcedure; }

                this.Text = sversion + ", Current Image: " + fullPath;

                string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";

                IniFile AppliIni = new IniFile(INIpath);
                AppliIni.WriteValue("Last Image", "path", ImagePath);

                //AppliIni.WriteValue("Last Image", "name", cmbImageName.Text.Trim());  // ImageName);

                AppliIni.WriteValue("Models", "path", txtModelsPath.Text.Trim());
                string modelFileName;
                if (txtRunTimeWSName.Text == "Peels")
                    modelFileName = "Proj_021_201223_104500_21122023_104445";
                else modelFileName = "WS_Proj_022_261223_111400_261223_183645";
                string modelName = modelFileName.Trim() + ".vrws";  // this.lstModels.SelectedItem.ToString();

                AppliIni.WriteValue("Models", "model", modelName);

                AppliIni.WriteValue("Last Model", "Full path", txtModelsPath.Text.Trim() + @"\" + modelName);

                var image = new ViDi2.UI.WpfImage(fullPath);   // imagePath);

                //
                System.Windows.Rect ROIrect = new System.Windows.Rect();
                //if (chkAutoROI.Checked)
                //    ROIrect = GetDeticatedRoiFromList(Convert.ToInt32(imgLstIndex.Text));

                Rectangle ImageDimensions = new Rectangle(0, 0, image.Width, image.Height);
                ApplyROI(true, ImageDimensions, chkAutoROI.Checked, ROIrect);

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

                //output filter                
                IRedHighDetailParameters outputrFilterRed01 = hdRedTool.ParametersBase as IRedHighDetailParameters;  // ViDi2.IRedRegionOfInterestParameters;
                string filter = outputrFilterRed01.RegionFilter;

                if (!IsNumeric(txtAreaFilterWidth.Text.Trim())) { System.Windows.Forms.MessageBox.Show("Area Width, Not Numeric!"); goto exitProcedure; }
                if (!IsNumeric(txtAreaFilterHeight.Text.Trim())) { System.Windows.Forms.MessageBox.Show("Area Height, Not Numeric!"); goto exitProcedure; }
                if (IsNumeric(txtAreaFilterScore.Text.Trim())) { bool xscore = Convert.ToSingle(txtAreaFilterScore.Text.Trim()) < 1.0f; if (!xscore) { System.Windows.Forms.MessageBox.Show("Score >1.0"); goto exitProcedure; } }
                else
                { System.Windows.Forms.MessageBox.Show("Score, Not Numeric!"); goto exitProcedure; }

                bool xFilterActive = chkFilterActive.Checked;
                if (xFilterActive)
                {
                    AppliIni.WriteValue("OUTPUT FILTER", "Area Width", txtAreaFilterWidth.Text.Trim());
                    AppliIni.WriteValue("OUTPUT FILTER", "Area Height", txtAreaFilterHeight.Text.Trim());
                    AppliIni.WriteValue("OUTPUT FILTER", "Score", txtAreaFilterScore.Text.Trim());
                    AppliIni.WriteValue("OUTPUT FILTER", "Used", chkFilterActive.Checked.ToString());

                    string area = (Convert.ToInt32(txtAreaFilterWidth.Text.Trim()) * Convert.ToInt32(txtAreaFilterHeight.Text.Trim())).ToString();
                    filter = "area>= " + area.ToString() + " and score>" + txtAreaFilterScore.Text.Trim();
                    outputrFilterRed01.RegionFilter = filter;
                }
                else
                {
                    filter = "";
                    outputrFilterRed01.RegionFilter = filter;

                    AppliIni.WriteValue("OUTPUT FILTER", "Area Width", txtAreaFilterWidth.Text.Trim());
                    AppliIni.WriteValue("OUTPUT FILTER", "Area Height", txtAreaFilterHeight.Text.Trim());
                    AppliIni.WriteValue("OUTPUT FILTER", "Score", txtAreaFilterScore.Text.Trim());
                    AppliIni.WriteValue("OUTPUT FILTER", "Used", chkFilterActive.Checked.ToString());
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

                duration = sw.ElapsedMilliseconds/1000.0;
                lblDuration.Text = duration.ToString("0.0000");
                sw.Stop();

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
                //resultsFound.imageName = cmbImageName.Text.Trim();
                resultsFound.modelName = modelName.Substring(0, modelName.Length - 5);
                resultsFound.thresholdLow = hdRedParams.Threshold.Lower.ToString("0.00");
                resultsFound.thresholdUpper = hdRedParams.Threshold.Upper.ToString("0.00");

                //roi
                resultsFound.roi.angle = Convert.ToDouble(this.txtROIangle.Text.Trim());
                resultsFound.roi.rect.X = Convert.ToDouble(this.txtPProiPosX.Text.Trim());
                resultsFound.roi.rect.Y = Convert.ToDouble(this.txtPProiPosY.Text.Trim());
                resultsFound.roi.rect.Width = Convert.ToDouble(this.txtPProiWidth.Text.Trim());
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

                RaisePropertyChanged(nameof(ViewIndices));

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Load Runtime Model First, stream = null");
                goto exitProcedure;
            }

            exitProcedure:;
        }

        private void txtImagePath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtImgPathLoad.Text != "")
                {
                    //cmbImageName.Items.Clear();
                    string ImagePath = txtImgPathLoad.Text;
                    string[] dirImages = Directory.GetFiles(ImagePath, "*.jpg");
                    //only image name
                    //foreach (string item in dirImages)
                    //{
                    //    cmbImageName.Items.Add(Path.GetFileName(item));
                    //}
                }
            }
            catch { }
        }

        private void btnAddROI_Click(object sender, EventArgs e)
        {
            //btnGetRecInfo.Enabled = true;

            RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxEditRoi, gcurrentImage);

            float ratioX = ratioxy.ratioX;
            float ratioY = ratioxy.ratioY;

            //if roi stored in ini use it for new roi
            string INIpath = System.Windows.Forms.Application.StartupPath + @"\Data\INI.ini";
            IniFile AppliIni = new IniFile(INIpath);
            System.Windows.Rect rect = new System.Windows.Rect();

            rect.X = Convert.ToDouble(AppliIni.ReadValue("roi", "x", "0"));
            rect.Y = Convert.ToDouble(AppliIni.ReadValue("roi", "y", "0"));
            rect.Width = Convert.ToDouble(AppliIni.ReadValue("roi", "width", "0"));
            rect.Height = Convert.ToDouble(AppliIni.ReadValue("roi", "height", "0"));

            StaticROI = new RectangleResizeAndRotateAdd(Convert.ToSingle(rect.X * (1 / ratioX)), Convert.ToSingle(rect.Y * (1 / ratioY)), Convert.ToSingle(rect.Width * (1 / ratioX)), Convert.ToSingle(rect.Height * (1 / ratioY)));

            StaticROI.SetPictureBox(this.pbxEditRoi);

        }

        private void btnEditROI_Click(object sender, EventArgs e)
        {
            try
            {
                rectangle = new Rectangle(0,0,0,0);
                if (SampleViewerViewModel.Sample != null)
                {
                    if (this.btnEditROI.Text == stdmodeCaption)
                    {
                        //---------------------edit mode---------------

                        StaticROI.DisEngage();
                        StaticROI.ClearGraphics(true);

                        LoadImage();
                        this.btnEditROI.Text = editmodeCaption;
                        btnDefect.Enabled = false;
                        btnEditROI.BackColor = Color.LightBlue;
                        elementHost1.Visible = false;
                        this.pbxEditRoi.Visible = true;
                        this.pbxEditRoi.BringToFront();
                        //
                        btnAddROI.Enabled = true;

                        //debug
                        //Rectangle rect = new Rectangle(60, 70, 300, 200);

                        using (Graphics g = Graphics.FromImage(this.pbxEditRoi.Image))
                        {
                            string sDisp = "";
                            sDisp = "ROI EDIT MODE";

                            float fontsize = 24.0f;

                            Brush bb = Brushes.Red;

                            float x = 40.0f;
                            float y = 25.0f;
                            g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Bold), bb, x, y);
                        }

                        //ROIeditMode();
                    }
                    //else //standard mode
                    //{
                    //    this.btnEditROI.Text = stdmodeCaption;
                    //    btnDefect.Enabled = true;
                    //    btnEditROI.BackColor = Color.Transparent;
                    //    this.pbxEditRoi.Visible = false;
                    //    elementHost1.Visible = true;
                    //    this.pbxEditRoi.SendToBack();

                    //    this.pbxEditRoi.Image.Dispose();
                    //    this.pbxEditRoi.Image = null;

                    //    //
                    //    btnAddROI.Enabled = false;

                    //    //
                    //    GetROIinfo();

                    //}
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Needs to Evaluate an Image Before roi Edit-Mode is Activated!");
                }


            }
            catch (NullReferenceException e1)
            {
                string message = e1.Message;
                System.Windows.Forms.MessageBox.Show("btnEditROI_Click(), Error: " + message);
            }
        }

        private void pbxEditRoi_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnApplyROI_Click(object sender, EventArgs e)
        {
            chkAutoROI.Checked = false;

            GetROIinfo();

            Rectangle ImageDimensions = new Rectangle();
            System.Windows.Rect rect = new System.Windows.Rect();
            ApplyROI(true, ImageDimensions, false, rect);
            btnApplyROI.BackColor = Color.LightBlue;
        }

        //private void lstModels_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //int ii = lstModels.SelectedIndex;
        //    //string item = sender.ToString();
        //    //int selectedindex = lstModels.Items.IndexOf("fgh");

        //    if (ii > -1)
        //    {
        //        Workspace = rwsl[rwsl.Names[ii]];

        //        txtRunTimeWSName.Text = Workspace.DisplayName;
        //        stream = Workspace.Streams.First();
        //        string streamName = stream.Name;  // Workspace.Streams.First().Name;
        //        ViDi2.Runtime.ITool tool = Workspace.Streams[streamName].Tools.First();
        //        txtToolName.Text = tool.Name;   // Workspace.Streams[streamName].Tools.First().Name;            
        //        txtToolType.Text = tool.Type.ToString();
        //        txtLastModified.Text = Workspace.LastModified.ToShortDateString();

        //        //class name display
        //        ViDi2.Runtime.IRedTool tool1 = (ViDi2.Runtime.IRedTool)stream.Tools.First();
        //        var knownClasses = tool1.KnownClasses;
        //        if (knownClasses.Count > 0)
        //        {
        //            string className = knownClasses[0];
        //            lblClassName.Text = className;
        //        }

        //        //get thresholds tool1                
        //        IRedHighDetailParameters hdRedParams = Stream.Tools.First().ParametersBase as IRedHighDetailParameters;

        //        ViDi2.Interval interval = hdRedParams.Threshold;

        //        //txtThresholdsLower.Text = interval.Lower.ToString("0.00");
        //        //txtThresholdsUpper.Text = interval.Upper.ToString("0.00");

        //    }

        //}

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            if (CmbCatNum.Text != "")
            {
                LoadJasonParameters();
               // LoadINIParameters();
                txtBlades.Enabled = true;
                txtDiameter.Enabled = true;
                txtLength.Enabled = true;
                txtThresholdsLower.Enabled = true;
                txtThresholdsUpper.Enabled = true;
                txtPProiPosX.Enabled = true;
                txtPProiPosY.Enabled = true;
                txtPProiWidth.Enabled = true;
                txtPProiHeight.Enabled = true;
                txtROIangle.Enabled = true;
                txtRatioImage2ROI.Enabled = true;
                txtAddNum.Enabled = true;
                txtImgPathLoad.Enabled = true;
                chkFractions1.Enabled = true;
                chkPeels1.Enabled = true;
                chkFractions2.Enabled = true;
                chkPeels2.Enabled = true;
                chkFractions3.Enabled = true;
                chkPeels3.Enabled = true;
                txtPlsThresoldLower.Enabled = true;
                txtPlsThresoldUpper.Enabled = true;
                txtPosX2.Enabled = true;
                txtPosY2.Enabled = true;
                txtWidth2.Enabled = true;
                txtHeight2.Enabled = true;
                txtAngle2.Enabled = true;
                txtRatio2.Enabled = true;
                txtPosX3.Enabled = true;
                txtPosY3.Enabled = true;
                txtWidth3.Enabled = true;
                txtHeight3.Enabled = true;
                txtAngle3.Enabled = true;
                txtRatio3.Enabled = true;

                txtPlsAreaLower.Enabled = true;
                txtPlsAreaUpper.Enabled = true;
                txtAreaLower.Enabled = true;
                txtAreaUpper.Enabled = true;
                Bitmap bmp = new Bitmap(txtImgPathLoad.Text);
                try
                {
                    //if (this.btnEditROI.Text == stdmodeCaption)
                    //{
                    //---------------------edit mode---------------

                    StaticROI.DisEngage();
                    StaticROI.ClearGraphics(true);

                    LoadImage1(bmp);
                    this.btnEditROI.Text = editmodeCaption;
                    btnDefect.Enabled = false;
                    btnEditROI.BackColor = Color.LightBlue;
                    elementHost1.Visible = false;
                    this.pbxEditRoi.Visible = true;
                    this.pbxEditRoi.BringToFront();
                    //
                    btnAddROI.Enabled = true;
                    using (Graphics g = Graphics.FromImage(this.pbxEditRoi.Image))
                    {
                        string sDisp = "";
                        sDisp = "ROI EDIT MODE";

                        float fontsize = 24.0f;

                        Brush bb = Brushes.Red;

                        float x = 40.0f;
                        float y = 25.0f;
                        g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Bold), bb, x, y);
                    }

                    ROIeditMode(Int32.Parse(txtPProiPosX.Text), Int32.Parse(txtPProiPosY.Text), Int32.Parse(txtPProiWidth.Text), Int32.Parse(txtPProiHeight.Text));
                    //}

                }
                catch (NullReferenceException e1)
                {
                    string message = e1.Message;
                    System.Windows.Forms.MessageBox.Show("btnEditROI_Click(), Error: " + message);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("button Edit Item(), Error: " + "add Endmilll Name");
            }
            }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            string isFrac, isPeel;
            int saved = 0;
            string iniFileName = CmbCatNum.Text + ".ini";
            string filePath = System.Windows.Forms.Application.StartupPath + @"\Data\DataBase\" + iniFileName;
            string section = CmbCatNum.Text;
            string[] newValues = {txtAddNum.Text, txtBlades.Text, txtDiameter.Text, txtLength.Text, txtThresholdsLower.Text, txtThresholdsUpper.Text, txtPlsThresoldLower.Text, txtPlsThresoldUpper.Text, txtPProiPosX.Text,
                txtPProiPosY.Text, txtPProiWidth.Text, txtPProiHeight.Text, txtROIangle.Text, txtRatioImage2ROI.Text,txtPosX2.Text, txtPosY2.Text, txtWidth2.Text, txtHeight2.Text, txtAngle2.Text, txtRatio2.Text,txtPosX3.Text, txtPosY3.Text, txtWidth3.Text, txtHeight3.Text, txtAngle3.Text, txtRatio3.Text, txtImgPathLoad.Text, chkFractions1.Checked.ToString(), chkPeels1.Checked.ToString(), chkROI1.Checked.ToString(), chkROI2.Checked.ToString(), chkROI3.Checked.ToString()};
            string[] keys = {"CatalogNO", "BladesNo", "Diameter", "Length", "FracLowerThreshold", "FracUpperThreshold","PeelLowerThreshold","PeelUpperThreshold", "roiPosX1",
            "roiPosY1", "roiWidth1", "roiHeight1", "roiAngle1", "roiRatio1", "roiPosX2","roiPosY2", "roiWidth2", "roiHeight2", "roiAngle2", "roiRatio2","roiPosX3",
            "roiPosY3", "roiWidth3", "roiHeight3", "roiAngle3", "roiRatio3", "ImagePath", "IsFractions", "IsPeels", "Roi1", "Roi2", "Roi3, txtAreaLower.Text, txtAreaUpper.Text, txtPlsAreaLower.Text, txtPlsAreaUpper.Text"};

            string[] iniLines = File.ReadAllLines(filePath);

            int sectionStartIndex = Array.IndexOf(iniLines, "[" + section + "]");
            if (sectionStartIndex == -1)
            {
                Console.WriteLine("Section not found in INI file.");
                goto ExitProc;
            }
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                string newValue = newValues[i];
                // Find the key-value pair in the section

                    for (int j = sectionStartIndex + 1; j < iniLines.Length; j++)
                    {
                        bool fieldExists = DoesFieldExistInIniFile(filePath, section, key);
                        if (fieldExists)
                        {
                            string line = iniLines[j];
                            if (line.StartsWith(key + "="))
                            {
                                // Modify the value of the key
                                string modifiedLine = key + "=" + newValue;
                                iniLines[j] = modifiedLine;
                                break;
                            }
                        }
                        else
                        {
                            AddFieldToIniFile(filePath, section, key, newValue);
                        }

                    }
                }
            
            File.WriteAllLines(filePath, iniLines);
            saved = 1;

            ExitProc:
            txtBlades.Enabled = false;
            txtDiameter.Enabled = false;
            txtLength.Enabled = false;
            txtThresholdsLower.Enabled = false;
            txtThresholdsUpper.Enabled = false;
            txtPProiPosX.Enabled = false;
            txtPProiPosY.Enabled = false;
            txtPProiWidth.Enabled = false;
            txtPProiHeight.Enabled = false;
            txtROIangle.Enabled = false;
            txtRatioImage2ROI.Enabled = false;
            txtAddNum.Enabled = false;
            txtImgPathLoad.Enabled = false;
            chkFractions1.Enabled = false;
            chkPeels1.Enabled = false;
            chkFractions2.Enabled = false;
            chkPeels2.Enabled = false;
            chkFractions3.Enabled = false;
            chkPeels3.Enabled = false;
            txtPlsThresoldLower.Enabled = false;
            txtPlsThresoldUpper.Enabled = false;
            txtPosX2.Enabled = false;
            txtPosY2.Enabled = false;
            txtWidth2.Enabled = false;
            txtHeight2.Enabled = false;
            txtAngle2.Enabled = false;
            txtRatio2.Enabled = false;
            txtPosX3.Enabled = false;
            txtPosY3.Enabled = false;
            txtWidth3.Enabled = false;
            txtHeight3.Enabled = false;
            txtAngle3.Enabled = false;
            txtRatio3.Enabled = false;

            txtPlsAreaLower.Enabled = false;
            txtPlsAreaUpper.Enabled = false;
            txtAreaLower.Enabled = false;
            txtAreaUpper.Enabled = false;

            if (saved == 0)
                System.Windows.Forms.MessageBox.Show("Item didn't saved, Pleas try again.");
            else System.Windows.Forms.MessageBox.Show("Item Saved Succesfully");
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            if (CmbCatNum.Text!="")
            {
                txtBlades.Enabled = false;
                txtDiameter.Enabled = false;
                txtLength.Enabled = false;
                txtThresholdsLower.Enabled = false;
                txtThresholdsUpper.Enabled = false;
                txtPProiPosX.Enabled = false;
                txtPProiPosY.Enabled = false;
                txtPProiWidth.Enabled = false;
                txtPProiHeight.Enabled = false;
                txtROIangle.Enabled = false;
                txtRatioImage2ROI.Enabled = false;
                txtAddNum.Enabled = false;
                txtImgPathLoad.Enabled = false;
                chkFractions1.Enabled = false;
                chkPeels1.Enabled = false;
                chkFractions2.Enabled = false;
                chkPeels2.Enabled = false;
                chkFractions3.Enabled = false;
                chkPeels3.Enabled = false;
                txtPlsThresoldLower.Enabled = false;
                txtPlsThresoldUpper.Enabled = false;
                txtPosX2.Enabled = false;
                txtPosY2.Enabled = false;
                txtWidth2.Enabled = false;
                txtHeight2.Enabled = false;
                txtAngle2.Enabled = false;
                txtRatio2.Enabled = false;
                txtPosX3.Enabled = false;
                txtPosY3.Enabled = false;
                txtWidth3.Enabled = false;
                txtHeight3.Enabled = false;
                txtAngle3.Enabled = false;
                txtRatio3.Enabled = false;

                txtPlsAreaLower.Enabled = false;
                txtPlsAreaUpper.Enabled = false;
                txtAreaLower.Enabled = false;
                txtAreaUpper.Enabled = false;
                // LoadINIParameters();
                LoadJasonParameters();
                this.pbxEditRoi.Visible = false;
                elementHost1.Visible = false;
                this.pbxEditRoi.SendToBack();

                this.pbxEditRoi.Image.Dispose();
                this.pbxEditRoi.Image = null;
                pbxRectangle.Visible = true;

                Bitmap bmp = new Bitmap(txtImgPathLoad.Text);
                float imgWidth = Convert.ToSingle(bmp.Width);
                float imgHeight = Convert.ToSingle(bmp.Height);
                float fAspectRatio = imgWidth / imgHeight;

                int width = 0;
                bool chckZoomChecked = false;
                if (!chckZoomChecked)
                {
                    width = this.pbxRectangle.Width;  //pbxShowImage.Width;  //this.currentImage.picBoxDefaultWidth;
                }
                else
                {
                    width = (int)imgWidth;
                }

                int height = this.pbxRectangle.Height;
                float fHeight = Convert.ToSingle(width) / fAspectRatio;

                Bitmap img = new Bitmap(bmp, width, Convert.ToInt32(fHeight));

                this.pbxRectangle.Image = (System.Drawing.Image)img.Clone();
                this.pbxRectangle.SizeMode = PictureBoxSizeMode.AutoSize;


                ////bmp.Save(@"C:\Users\hadars\Desktop\New folder (2)");

                gcurrentImage.imgWidth = bmp.Width;
                gcurrentImage.imgHeight = bmp.Height;

                gcurrentImage.picBoxDefaultWidth = this.pbxRectangle.Width;
                gcurrentImage.picBoxDefaultHeight = this.pbxRectangle.Height;

                if (img != null)
                    img.Dispose();

                RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxRectangle, gcurrentImage);

                float ratioX = ratioxy.ratioX;
                float ratioY = ratioxy.ratioY;
                pbxRectangle.Visible = true;
                pbxRectangle.BringToFront();
                int pX = Convert.ToInt32((Int32.Parse(txtPProiPosX.Text) * 1 / ratioX));
                int pY = Convert.ToInt32((Int32.Parse(txtPProiPosY.Text) * (1 / ratioY)));
                int pWidth = Convert.ToInt32((Int32.Parse(txtPProiWidth.Text) * (1 / ratioX)));
                int pHeight = Convert.ToInt32((Int32.Parse(txtPProiHeight.Text) * (1 / ratioY)));
                rectangle = new Rectangle(pX, pY, pWidth, pHeight);
                rectangleToProc = rectangle;
                pbxRectangle.Invalidate();

                GetROIinfo();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("button Cancel Edit(), Error: " + "add Endmilll Name");
            }
        }

        private void btnAddNewItem_Click(object sender, EventArgs e)
        {
            txtBlades.Enabled = true;
            txtDiameter.Enabled = true;
            txtLength.Enabled = true;
            txtThresholdsLower.Enabled = true;
            txtThresholdsUpper.Enabled = true;
            txtPProiPosX.Enabled = true;
            txtPProiPosY.Enabled = true;
            txtPProiWidth.Enabled = true;
            txtPProiHeight.Enabled = true;
            txtROIangle.Enabled = true;
            txtRatioImage2ROI.Enabled = true;
            txtAddNum.Enabled = true;
            txtImgPathLoad.Enabled = true;
            chkFractions1.Enabled = true;
            chkPeels1.Enabled = true;
            chkFractions2.Enabled = true;
            chkPeels2.Enabled = true;
            chkFractions3.Enabled = true;
            chkPeels3.Enabled = true;
            txtPlsThresoldLower.Enabled = true;
            txtPlsThresoldUpper.Enabled = true;
            txtPosX2.Enabled = true;
            txtPosY2.Enabled = true;
            txtWidth2.Enabled = true;
            txtHeight2.Enabled = true;
            txtAngle2.Enabled = true;
            txtRatio2.Enabled = true;
            txtPosX3.Enabled = true;
            txtPosY3.Enabled = true;
            txtWidth3.Enabled = true;
            txtHeight3.Enabled = true;
            txtAngle3.Enabled = true;
            txtRatio3.Enabled = true;

            txtPlsAreaLower.Enabled = true;
            txtPlsAreaUpper.Enabled = true;
            txtAreaLower.Enabled = true;
            txtAreaUpper.Enabled = true;
        }

        private void btnSaveItem_Click(object sender, EventArgs e)
        {
            string iniFileName = CmbCatNum.Text + ".ini";
            string folderPath = System.Windows.Forms.Application.StartupPath + @"\Data\DataBase\";
            string iniFilePath = System.Windows.Forms.Application.StartupPath + @"\Data\DataBase\" + iniFileName;
            string namePath = System.Windows.Forms.Application.StartupPath +@"\Data\DataBase\FilesNames.ini";
            int saved = 0;
            if (CmbCatNum.Text != "")
            {
               // string iniFilePath = @"C:\Users\hadars\Desktop\EndmillProject 15.07.24\InspectEndmill_All\InspectEndmill 24.07.24\Data.ini";
                string section = CmbCatNum.Text;
                //string[] iniLines = File.ReadAllLines(iniPath);
                string CatalogNum = txtAddNum.Text;
                string BladesNum = txtBlades.Text;
                string Diameter = txtDiameter.Text;
                string EndMillLength = txtLength.Text;
                string addFrLowerThreshold = txtThresholdsLower.Text;
                string addFrUpperThreshold = txtThresholdsUpper.Text;
                string addPeelLowerThreshold = txtPlsThresoldLower.Text;
                string addPeelUpperThreshold = txtPlsThresoldUpper.Text;
                string roiPosX = txtPProiPosX.Text;
                string roiPosY = txtPProiPosY.Text;
                string roiWidth = txtPProiWidth.Text;
                string roiHeight = txtPProiHeight.Text;
                string roiAangle = txtROIangle.Text;
                string roiRatio = txtRatioImage2ROI.Text;
                string roiPosX2 = txtPosX2.Text;
                string roiPosY2 = txtPosY2.Text;
                string roiWidth2 = txtWidth2.Text;
                string roiHeight2 = txtHeight2.Text;
                string roiAangle2 = txtAngle2.Text;
                string roiRatio2 = txtRatio2.Text;
                string roiPosX3 = txtPosX3.Text;
                string roiPosY3 = txtPosY3.Text;
                string roiWidth3 = txtWidth3.Text;
                string roiHeight3 = txtHeight3.Text;
                string roiAangle3 = txtAngle3.Text;
                string roiRatio3 = txtRatio3.Text;

                string addFrLowerArea = txtAreaLower.Text;
                string addFrUpperArea = txtAreaUpper.Text;
                string addPeelLowerArea = txtPlsAreaLower.Text;
                string addPeelUpperArea = txtPlsAreaUpper.Text;

                string ImagePath = txtImgPathLoad.Text;
                string isFractions;
                string isPeels;
                string Roi1;
                string Roi2;
                string Roi3;

                if (chkFractions1.Checked == true)
                    isFractions = "True";
                else isFractions = "False";

                if (chkPeels1.Checked == true)
                    isPeels = "True";
                else isPeels = "False";

                if (chkROI1.Checked == true)
                    Roi1 = "True";
                else Roi1 = "False";
                if (chkROI2.Checked == true)
                    Roi2 = "True";
                else Roi2 = "False";
                if (chkROI3.Checked == true)
                    Roi3 = "True";
                else Roi3 = "False";

                // int sectionStartIndex = Array.IndexOf(iniLines, "[" + section + "]");
                if (Directory.GetFiles(folderPath).Contains(iniFilePath))
                {
                    saved = 1;
                    System.Windows.Forms.MessageBox.Show("Endmill Name Already Exist");
                    goto exitProc;
                }

                using (StreamWriter writer = new StreamWriter(iniFilePath, true))
                {
                    writer.WriteLine("");
                    writer.WriteLine("[" + section + "]");
                    writer.WriteLine("CatalogNO" + "=" + CatalogNum);
                    writer.WriteLine("BladesNo" + "=" + BladesNum);
                    writer.WriteLine("Diameter" + "=" + Diameter);
                    writer.WriteLine("Length" + "=" + EndMillLength);
                    writer.WriteLine("LowerThreshold" + "=" + addFrLowerThreshold);
                    writer.WriteLine("UpperThreshold" + "=" + addFrUpperThreshold);
                    writer.WriteLine("LowerThreshold" + "=" + addPeelLowerThreshold);
                    writer.WriteLine("UpperThreshold" + "=" + addPeelUpperThreshold);
                    writer.WriteLine("roiPosX1" + "=" + roiPosX);
                    writer.WriteLine("roiPosY1" + "=" + roiPosY);
                    writer.WriteLine("roiWidth1" + "=" + roiWidth);
                    writer.WriteLine("roiHeight1" + "=" + roiHeight);
                    writer.WriteLine("roiAngle1" + "=" + roiAangle);
                    writer.WriteLine("roiRatio1" + "=" + roiRatio);
                    writer.WriteLine("roiPosX2" + "=" + roiPosX2);
                    writer.WriteLine("roiPosY2" + "=" + roiPosY2);
                    writer.WriteLine("roiWidth2" + "=" + roiWidth2);
                    writer.WriteLine("roiHeight2" + "=" + roiHeight2);
                    writer.WriteLine("roiAngle2" + "=" + roiAangle2);
                    writer.WriteLine("roiRatio2" + "=" + roiRatio2);
                    writer.WriteLine("roiPosX3" + "=" + roiPosX3);
                    writer.WriteLine("roiPosY3" + "=" + roiPosY3);
                    writer.WriteLine("roiWidth3" + "=" + roiWidth3);
                    writer.WriteLine("roiHeight3" + "=" + roiHeight3);
                    writer.WriteLine("roiAngle3" + "=" + roiAangle3);
                    writer.WriteLine("roiRatio3" + "=" + roiRatio3);
                    writer.WriteLine("ImagePath" + "=" + ImagePath);
                    writer.WriteLine("IsFractions" + "=" + isFractions);
                    writer.WriteLine("IsPeels" + "=" + isPeels);
                    writer.WriteLine("Roi1" + "=" + Roi1);
                    writer.WriteLine("Roi2" + "=" + Roi2);
                    writer.WriteLine("Roi3" + "=" + Roi3);

                    writer.WriteLine("LowerArea" + "=" + addFrLowerThreshold);
                    writer.WriteLine("UpperArea" + "=" + addFrUpperThreshold);
                    writer.WriteLine("LowerPlArea" + "=" + addPeelLowerThreshold);
                    writer.WriteLine("UpperPlArea" + "=" + addPeelUpperThreshold);

                    CmbCatNum.Items.Add(section);
                    writer.Close();
                }

                using (StreamWriter writer = new StreamWriter(namePath, true))
                {
                    writer.WriteLine("[" + section + "]");
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Enter Endmill Name!");
                saved = 1;
            }

            exitProc:
            txtBlades.Enabled = false;
            txtDiameter.Enabled = false;
            txtLength.Enabled = false;
            txtThresholdsLower.Enabled = false;
            txtThresholdsUpper.Enabled = false;
            txtPProiPosX.Enabled = false;
            txtPProiPosY.Enabled = false;
            txtPProiWidth.Enabled = false;
            txtPProiHeight.Enabled = false;
            txtROIangle.Enabled = false;
            txtRatioImage2ROI.Enabled = false;
            txtImgPathLoad.Enabled = false;
            chkFractions1.Enabled = false;
            chkPeels1.Enabled = false;
            chkFractions2.Enabled = false;
            chkPeels2.Enabled = false;
            chkFractions3.Enabled = false;
            chkPeels3.Enabled = false;
            txtPlsThresoldLower.Enabled = false;
            txtPlsThresoldUpper.Enabled = false;
            txtPosX2.Enabled = false;
            txtPosY2.Enabled = false;
            txtWidth2.Enabled = false;
            txtHeight2.Enabled = false;
            txtAngle2.Enabled = false;
            txtRatio2.Enabled = false;
            txtPosX3.Enabled = false;
            txtPosY3.Enabled = false;
            txtWidth3.Enabled = false;
            txtHeight3.Enabled = false;
            txtAngle3.Enabled = false;
            txtRatio3.Enabled = false;

            txtPlsAreaLower.Enabled = false;
            txtPlsAreaUpper.Enabled = false;
            txtPlsAreaLower.Enabled = false;
            txtPlsAreaUpper.Enabled = false;

            if (saved == 1) { System.Windows.Forms.MessageBox.Show("Endmill Name Already Exist"); }
            else System.Windows.Forms.MessageBox.Show("Endmill Data Saved Succesfully");
        }

        private void chkFractions_CheckStateChanged(object sender, EventArgs e)
        {
            //if (chkFractions.Checked==true)
            //    btnSearch.Enabled = true;
            //else if (chkPeels.Checked==false)
            //    btnSearch.Enabled = false;
        }

        private void chkPeels_CheckStateChanged(object sender, EventArgs e)
        {
            //if (chkPeels.Checked == true)
            //    btnSearch.Enabled = true;
            //else if (chkFractions.Checked == false)
            //    btnSearch.Enabled = false;
        }

        private void btnStopROIMode_Click(object sender, EventArgs e)
        {
            if (txtImgPathLoad.Text != "")
            {
                btnEditRoi1.BackColor = Color.Transparent;
                btnEditRoi2.BackColor = Color.Transparent;
                btnEditRoi3.BackColor = Color.Transparent;

                this.btnEditROI.Text = stdmodeCaption;
                btnDefect.Enabled = true;
                button1.Enabled = true;
                btnShoePeel.Enabled = true;
                btnEditROI.BackColor = Color.Transparent;
                button1.BackColor = Color.Transparent;
                btnShoePeel.BackColor = Color.Transparent;
                this.pbxEditRoi.Visible = false;
                elementHost1.Visible = true;
                this.pbxEditRoi.SendToBack();

                this.pbxEditRoi.Image.Dispose();
                this.pbxEditRoi.Image = null;

                //
                btnAddROI.Enabled = false;
                btnApplyROI.BackColor = Color.Transparent;
                //
                GetROIinfo();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("button Edit Roi(), Error: " + "Load Image First");
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ViDi2.IManualRegionOfInterest redROI01 = (ViDi2.IManualRegionOfInterest)Stream.Tools.First().RegionOfInterest;
            redROI01.Parameters.Units = ViDi2.UnitsMode.Pixel;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (i != e.Index)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
                if (checkedListBox1.SelectedItem == "Use Full Picture")
                {
                    chkROI1.Enabled = false;
                    chkROI2.Enabled = false;
                    chkROI3.Enabled = false;
                    chkROI1.Checked = false;
                    chkROI2.Checked = false;
                    chkROI3.Checked = false;
                    ISampleViewerView vs = sampleviewr.sampleViewer.ViewModel.View;

                    double imageW = vs.ViewModel.ViewWidth;
                    double imageH = vs.ViewModel.ViewHeight;

                    redROI01.Parameters.Offset = new ViDi2.Point(0, 0);
                    redROI01.Parameters.Size = new ViDi2.Size(imageW, imageH);

                    currentROI.X = redROI01.Parameters.Offset.X;
                    currentROI.Y = redROI01.Parameters.Offset.Y;

                    currentROI.Width = imageW;
                    currentROI.Height = imageH;
                }
                else
                {
                    chkROI1.Enabled = true;
                    chkROI2.Enabled = true;
                    chkROI3.Enabled = true;
                    string iniFileName = CmbCatNum.Text + ".ini";
                    string iniFilePath = System.Windows.Forms.Application.StartupPath + @"\Data\DataBase\" + iniFileName;
                    EndmillParameters parameters = new EndmillParameters();
                    parameters.Roi1 = GetIniInfo(iniFilePath, CmbCatNum.Text).Roi1;
                    parameters.Roi2 = GetIniInfo(iniFilePath, CmbCatNum.Text).Roi2;
                    parameters.Roi3 = GetIniInfo(iniFilePath, CmbCatNum.Text).Roi3;
                    if (parameters.Roi1 == "True")
                        chkROI1.Checked = true;
                    else chkROI1.Checked = false;
                    if (parameters.Roi2 == "True")
                        chkROI2.Checked = true;
                    else chkROI2.Checked = false;
                    if (parameters.Roi3 == "True")
                        chkROI3.Checked = true;
                    else chkROI3.Checked = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pbxRectangle.Visible = false;
            pbxEditRoi.Visible = false;
            elementHost1.Visible= true;
            if (CmbCatNum.Text=="")
            {
                System.Windows.Forms.MessageBox.Show("Please Choose Endmill");
                goto exitProcedure;
            }
            if (checkedListBox1.CheckedItems==null)
            {
                System.Windows.Forms.MessageBox.Show("Check if Use ROI Or Evaluate Full Image!");
            }
            modelToRun(0);
            string iniFileName = CmbCatNum.Text + ".ini";
            string iniFilePath = System.Windows.Forms.Application.StartupPath + @"\Data\DataBase\" + iniFileName;
            EndmillParameters parameters = new EndmillParameters();
            parameters.BladesNo = GetIniInfo(iniFilePath, CmbCatNum.Text).BladesNo;
            parameters.Diameter = GetIniInfo(iniFilePath, CmbCatNum.Text).Diameter;
            parameters.Length = GetIniInfo(iniFilePath, CmbCatNum.Text).Length;
            parameters.FracLowerThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).FracLowerThreshold;
            parameters.FracUpperThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).FracUpperThreshold;
            parameters.PeelLowerThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).PeelLowerThreshold;
            parameters.PeelUpperThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).PeelUpperThreshold;
            parameters.roiPosX1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX1;
            parameters.roiPosY1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY1;
            parameters.roiWidth1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth1;
            parameters.roiHeight1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight1;
            parameters.roiAngle1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle1;
            parameters.roiRatio1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio1;
            parameters.roiPosX2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX2;
            parameters.roiPosY2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY2;
            parameters.roiWidth2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth2;
            parameters.roiHeight2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight2;
            parameters.roiAngle2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle2;
            parameters.roiRatio2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio2;
            parameters.roiPosX3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX3;
            parameters.roiPosY3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY3;
            parameters.roiWidth3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth3;
            parameters.roiHeight3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight3;
            parameters.roiAngle3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle3;
            parameters.roiRatio3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio3;
            parameters.CatalogNo = GetIniInfo(iniFilePath, CmbCatNum.Text).CatalogNo;
            parameters.ImagePath = GetIniInfo(iniFilePath, CmbCatNum.Text).ImagePath;
            parameters.IsFractions1 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions1;
            parameters.IsPeels1 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels1;
            parameters.IsFractions2 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions2;
            parameters.IsPeels2 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels2;
            parameters.IsFractions3 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions3;
            parameters.IsPeels3 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels3;
            ViDi2.Runtime.IRedHighDetailParameters hdRedParamsBreake = grunTimeWorkapace.StreamDict[Model1Name].Tools.First().ParametersBase as ViDi2.Runtime.IRedHighDetailParameters;
            System.Windows.Rect ROIrect = new System.Windows.Rect();

            if (chkAutoROI.Checked)
                ROIrect = GetDeticatedRoiFromList(Convert.ToInt32(0));

            var image = new ViDi2.UI.WpfImage(parameters.ImagePath);   // imagePath);
            Rectangle ImageDimensions = new Rectangle(0, 0, image.Width, image.Height);
            if (checkedListBox1.SelectedItem == "Use ROI")
            {
                if (!chkROI1.Checked && !chkROI2.Checked && !chkROI3.Checked)
                {
                    System.Windows.Forms.MessageBox.Show("Click At Least One ROI Area ROI#1, ROI#2 OR ROI#3 Or check Full Image");
                    goto exitProcedure;
                }
                ApplyROIRect(true, ImageDimensions, chkAutoROI.Checked, ROIrect);
            }
            /*set the wanred threshold in the tool properties*/
            SetThreshold(hdRedParamsBreake, parameters.FracLowerThreshold, parameters.FracLowerThreshold);
            string modelName = "Proj_021_201223_104500_21122023_104445.vrws";
            Bitmap bmp = new Bitmap(parameters.ImagePath);
            IImage imageToProc = new ViDi2.Local.LibraryImage(parameters.ImagePath);
            ISample samples1 = ImageEvaloation(grunTimeWorkapace, imageToProc, "Brake");
            ProcessImg(image, samples1, grunTimeWorkapace, modelName, hdRedParamsBreake);

            exitProcedure:;
        }

        public ISample ImageEvaloation(RunTimeWorkapace runTimeWorkapace, IImage iimage, string BrakeOrPeel)
        {
            ISample sample = runTimeWorkapace.StreamDict[BrakeOrPeel].CreateSample(iimage);           
            return sample;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (CmbCatNum.Text != "")
            {
                //elementHost1.Visible = false;
                txtBlades.Enabled = false;
                txtDiameter.Enabled = false;
                txtLength.Enabled = false;
                txtThresholdsLower.Enabled = false;
                txtThresholdsUpper.Enabled = false;
                txtPProiPosX.Enabled = false;
                txtPProiPosY.Enabled = false;
                txtPProiWidth.Enabled = false;
                txtPProiHeight.Enabled = false;
                txtROIangle.Enabled = false;
                txtRatioImage2ROI.Enabled = false;
                txtAddNum.Enabled = false;
                txtImgPathLoad.Enabled = false;
                chkFractions1.Enabled = false;
                chkPeels1.Enabled = false;
                chkFractions2.Enabled = false;
                chkPeels2.Enabled = false;
                chkFractions3.Enabled = false;
                chkPeels3.Enabled = false;
                txtPlsThresoldLower.Enabled = false;
                txtPlsThresoldUpper.Enabled = false;
                txtPosX2.Enabled = false;
                txtPosY2.Enabled = false;
                txtWidth2.Enabled = false;
                txtHeight2.Enabled = false;
                txtAngle2.Enabled = false;
                txtRatio2.Enabled = false;
                txtPosX3.Enabled = false;
                txtPosY3.Enabled = false;
                txtWidth3.Enabled = false;
                txtHeight3.Enabled = false;
                txtAngle3.Enabled = false;
                txtRatio3.Enabled = false;

                txtPlsAreaLower.Enabled = false;
                txtPlsAreaUpper.Enabled = false;
                txtAreaLower.Enabled = false;
                txtAreaUpper.Enabled = false;

                LoadJasonParameters();
                Bitmap bmp = new Bitmap(txtImgPathLoad.Text);
                float imgWidth = Convert.ToSingle(bmp.Width);
                float imgHeight = Convert.ToSingle(bmp.Height);
                float fAspectRatio = imgWidth / imgHeight;

                int width = 0;
                bool chckZoomChecked = false;
                if (!chckZoomChecked)
                {
                    width = this.pbxRectangle.Width;  //pbxShowImage.Width;  //this.currentImage.picBoxDefaultWidth;
                }
                else
                {
                    width = (int)imgWidth;
                }

                int height = this.pbxRectangle.Height;
                float fHeight = Convert.ToSingle(width) / fAspectRatio;

                Bitmap img = new Bitmap(bmp, width, Convert.ToInt32(fHeight));

                this.pbxRectangle.Image = (System.Drawing.Image)img.Clone();
                this.pbxRectangle.SizeMode = PictureBoxSizeMode.AutoSize;
                

                gcurrentImage.imgWidth = bmp.Width;
                gcurrentImage.imgHeight = bmp.Height;

                gcurrentImage.picBoxDefaultWidth = this.pbxRectangle.Width;
                gcurrentImage.picBoxDefaultHeight = this.pbxRectangle.Height;

                if (img != null)
                    img.Dispose();

                RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxRectangle, gcurrentImage);

                float ratioX = ratioxy.ratioX;
                float ratioY = ratioxy.ratioY;

                pbxRectangle.Visible = true;
                pbxRectangle.BringToFront();

                int pX1 = Convert.ToInt32((Int32.Parse(txtPProiPosX.Text) * 1 / ratioX));
                int pY1 = Convert.ToInt32((Int32.Parse(txtPProiPosY.Text) * (1 / ratioY)));
                int pWidth1 = Convert.ToInt32((Int32.Parse(txtPProiWidth.Text) * (1 / ratioX)));
                int pHeight1 = Convert.ToInt32((Int32.Parse(txtPProiHeight.Text) * (1 / ratioY)));

                int pX2 = Convert.ToInt32((Int32.Parse(txtPosX2.Text) * 1 / ratioX));
                int pY2 = Convert.ToInt32((Int32.Parse(txtPosY2.Text) * (1 / ratioY)));
                int pWidth2 = Convert.ToInt32((Int32.Parse(txtWidth2.Text) * (1 / ratioX)));
                int pHeight2 = Convert.ToInt32((Int32.Parse(txtHeight2.Text) * (1 / ratioY)));


                int pX3 = Convert.ToInt32((Int32.Parse(txtPosX3.Text) * 1 / ratioX));
                int pY3 = Convert.ToInt32((Int32.Parse(txtPosY3.Text) * (1 / ratioY)));
                int pWidth3 = Convert.ToInt32((Int32.Parse(txtWidth3.Text) * (1 / ratioX)));
                int pHeight3 = Convert.ToInt32((Int32.Parse(txtHeight3.Text) * (1 / ratioY)));

                RoiRects[0] = new Rectangle(pX1, pY1, pWidth1, pHeight1);
                RoiRects[1] = new Rectangle(pX2, pY2, pWidth2, pHeight2);
                RoiRects[2] = new Rectangle(pX3, pY3, pWidth3, pHeight3);

                rectangle = new Rectangle(pX1, pY1, pWidth1, pHeight1);
                rectangleToProc = rectangle;
                pbxRectangle.Invalidate();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("button Load Item(), Error: " + "add Endmilll Name");
            }
        }

        private void btnShoePeel_Click(object sender, EventArgs e)
        {
            pbxRectangle.Visible = false;
            pbxEditRoi.Visible = false;
            elementHost1.Visible = true;
            if (CmbCatNum.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Please Choose Endmill");
                goto exitProcedure;
            }
            modelToRun(1);
            string iniFileName = CmbCatNum.Text + ".ini";
            string iniFilePath = System.Windows.Forms.Application.StartupPath + @"\Data\DataBase\" + iniFileName;
            EndmillParameters parameters = new EndmillParameters();
            parameters.BladesNo = GetIniInfo(iniFilePath, CmbCatNum.Text).BladesNo;
            parameters.Diameter = GetIniInfo(iniFilePath, CmbCatNum.Text).Diameter;
            parameters.Length = GetIniInfo(iniFilePath, CmbCatNum.Text).Length;
            parameters.FracLowerThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).FracLowerThreshold;
            parameters.FracUpperThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).FracUpperThreshold;
            parameters.PeelLowerThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).PeelLowerThreshold;
            parameters.PeelUpperThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).PeelUpperThreshold;
            parameters.roiPosX1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX1;
            parameters.roiPosY1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY1;
            parameters.roiWidth1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth1;
            parameters.roiHeight1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight1;
            parameters.roiAngle1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle1;
            parameters.roiRatio1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio1;
            parameters.roiPosX2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX2;
            parameters.roiPosY2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY2;
            parameters.roiWidth2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth2;
            parameters.roiHeight2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight2;
            parameters.roiAngle2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle2;
            parameters.roiRatio2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio2;
            parameters.roiPosX3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX3;
            parameters.roiPosY3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY3;
            parameters.roiWidth3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth3;
            parameters.roiHeight3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight3;
            parameters.roiAngle3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle3;
            parameters.roiRatio3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio3;
            parameters.CatalogNo = GetIniInfo(iniFilePath, CmbCatNum.Text).CatalogNo;
            parameters.ImagePath = GetIniInfo(iniFilePath, CmbCatNum.Text).ImagePath;
            parameters.IsFractions1 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions1;
            parameters.IsPeels1 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels1;
            parameters.IsFractions2 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions2;
            parameters.IsPeels2 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels2;
            parameters.IsFractions3 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions3;
            parameters.IsPeels3 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels3;

            ViDi2.Runtime.IRedHighDetailParameters hdRedParamPeels = grunTimeWorkapace.StreamDict[Model2Name].Tools.First().ParametersBase as ViDi2.Runtime.IRedHighDetailParameters;
            System.Windows.Rect ROIrect = new System.Windows.Rect();
            if (chkAutoROI.Checked)
                ROIrect = GetDeticatedRoiFromList(Convert.ToInt32(0));
            var image = new ViDi2.UI.WpfImage(parameters.ImagePath);   // imagePath);
            Rectangle ImageDimensions = new Rectangle(0, 0, image.Width, image.Height);
            if (checkedListBox1.SelectedItem == "Use ROI")
            {
                if (!chkROI1.Checked && !chkROI2.Checked && !chkROI3.Checked)
                {
                    System.Windows.Forms.MessageBox.Show("Click At Least One ROI Area ROI#1, ROI#2 OR ROI#3 Or check Full Image");
                    goto exitProcedure;
                }
                ApplyROIRect(true, ImageDimensions, chkAutoROI.Checked, ROIrect);
            }
            /*set the wanred threshold in the tool properties*/
            SetThreshold(hdRedParamPeels, parameters.PeelLowerThreshold, parameters.PeelUpperThreshold);
            string modelName = "WS_Proj_022_261223_111400_261223_183645.vrws";
            Bitmap bmp = new Bitmap(parameters.ImagePath);
            IImage imageToProc = new ViDi2.Local.LibraryImage(parameters.ImagePath);
            ISample samples1 = ImageEvaloation(grunTimeWorkapace, imageToProc, "Peel");
            ProcessImg(image, samples1, grunTimeWorkapace, modelName, hdRedParamPeels);

            exitProcedure:;
        }

        private void pbxEditRoi_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        private void pbxEditRoi_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
           
        }

        private void pbxEditRoi_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void pbxRectangle_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Red, RoiRects[0]);
            e.Graphics.DrawRectangle(Pens.Blue, RoiRects[1]);
            e.Graphics.DrawRectangle(Pens.Green, RoiRects[2]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pbxRectangle.Visible = false;
            pbxEditRoi.Visible = false;
            elementHost1.Visible = true;
            if (CmbCatNum.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Please Choose Endmill");
                goto exitProcedure;
            }
            modelToRun(0);
            string iniFileName = CmbCatNum.Text + ".ini";
            string iniFilePath = System.Windows.Forms.Application.StartupPath + @"\Data\DataBase\" + iniFileName;
            EndmillParameters parameters = new EndmillParameters();
            parameters.BladesNo = GetIniInfo(iniFilePath, CmbCatNum.Text).BladesNo;
            parameters.Diameter = GetIniInfo(iniFilePath, CmbCatNum.Text).Diameter;
            parameters.Length = GetIniInfo(iniFilePath, CmbCatNum.Text).Length;
            parameters.FracLowerThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).FracLowerThreshold;
            parameters.FracUpperThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).FracUpperThreshold;
            parameters.PeelLowerThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).PeelLowerThreshold;
            parameters.PeelUpperThreshold = GetIniInfo(iniFilePath, CmbCatNum.Text).PeelUpperThreshold;
            parameters.roiPosX1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX1;
            parameters.roiPosY1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY1;
            parameters.roiWidth1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth1;
            parameters.roiHeight1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight1;
            parameters.roiAngle1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle1;
            parameters.roiRatio1 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio1;
            parameters.roiPosX2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX2;
            parameters.roiPosY2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY2;
            parameters.roiWidth2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth2;
            parameters.roiHeight2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight2;
            parameters.roiAngle2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle2;
            parameters.roiRatio2 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio2;
            parameters.roiPosX3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosX3;
            parameters.roiPosY3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiPosY3;
            parameters.roiWidth3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiWidth3;
            parameters.roiHeight3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiHeight3;
            parameters.roiAngle3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiAngle3;
            parameters.roiRatio3 = GetIniInfo(iniFilePath, CmbCatNum.Text).roiRatio3;
            parameters.CatalogNo = GetIniInfo(iniFilePath, CmbCatNum.Text).CatalogNo;
            parameters.ImagePath = GetIniInfo(iniFilePath, CmbCatNum.Text).ImagePath;
            parameters.IsFractions1 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions1;
            parameters.IsPeels1 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels1;
            parameters.IsFractions2 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions2;
            parameters.IsPeels2 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels2;
            parameters.IsFractions3 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsFractions3;
            parameters.IsPeels3 = GetIniInfo(iniFilePath, CmbCatNum.Text).IsPeels3;

            ViDi2.Runtime.IRedHighDetailParameters hdRedParamsBreake = grunTimeWorkapace.StreamDict[Model1Name].Tools.First().ParametersBase as ViDi2.Runtime.IRedHighDetailParameters;
            /*set the wanred threshold in the tool properties*/
            SetThreshold(hdRedParamsBreake, parameters.FracLowerThreshold, parameters.FracLowerThreshold);
            string modelName = "Proj_021_201223_104500_21122023_104445.vrws";

            Bitmap bmp = new Bitmap(parameters.ImagePath);
            float imgWidth = Convert.ToSingle(bmp.Width);
            float imgHeight = Convert.ToSingle(bmp.Height);
            float fAspectRatio = imgWidth / imgHeight;

            int width = 0;
            bool chckZoomChecked = false;
            if (!chckZoomChecked)
            {
                width = this.pbxRectangle.Width;  //pbxShowImage.Width;  //this.currentImage.picBoxDefaultWidth;
            }
            else
            {
                width = (int)imgWidth;
            }

            int height = this.pbxRectangle.Height;
            float fHeight = Convert.ToSingle(width) / fAspectRatio;

            Bitmap img = new Bitmap(bmp, width, Convert.ToInt32(fHeight));

            ////bmp.Save(@"C:\Users\hadars\Desktop\New folder (2)");

            gcurrentImage.imgWidth = bmp.Width;
            gcurrentImage.imgHeight = bmp.Height;

            gcurrentImage.picBoxDefaultWidth = this.pbxRectangle.Width;
            gcurrentImage.picBoxDefaultHeight = this.pbxRectangle.Height;

            if (img != null)
                img.Dispose();

            RatioXratioY ratioxy = GetRatioXandRetioY(this.pbxRectangle, gcurrentImage);

            float ratioX = ratioxy.ratioX;
            float ratioY = ratioxy.ratioY;

            int pX = Convert.ToInt32((parameters.roiPosX1 * 1 / ratioX));
            int pY = Convert.ToInt32((parameters.roiPosY1 * (1 / ratioY)));
            int pWidth = Convert.ToInt32((parameters.roiWidth1 * (1 / ratioX)));
            int pHeight = Convert.ToInt32((parameters.roiHeight1 * (1 / ratioY)));
            rectangle = new Rectangle(pX, pY, pWidth, pHeight);

            System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(parameters.ImagePath);
            Rectangle cropRectangle = rectangle;

            System.Drawing.Image croppedImage = CropImage(sourceImage, cropRectangle);

            // Save the cropped image to a file
            croppedImage.Save(System.Windows.Forms.Application.StartupPath + @"\Data\croppedImages\" + "crop1.jpg" );
            // Dispose the images
            sourceImage.Dispose();
            croppedImage.Dispose();
            IImage imageToProc = new ViDi2.Local.LibraryImage(System.Windows.Forms.Application.StartupPath + @"\Data\croppedImages\" + "crop1.jpg");
            ISample samples1 = ImageEvaloation(grunTimeWorkapace, imageToProc, "Brake");
            var image = new ViDi2.UI.WpfImage(System.Windows.Forms.Application.StartupPath + @"\Data\croppedImages\" + "crop1.jpg");
            ProcessImg2(image, samples1, grunTimeWorkapace, modelName, hdRedParamsBreake);
            exitProcedure:;
        }

        private void btnEditRoi1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(txtImgPathLoad.Text);
            try
            {
                //if (this.btnEditROI.Text == stdmodeCaption)
                //{
                //---------------------edit mode---------------

                StaticROI.DisEngage();
                StaticROI.ClearGraphics(true);

                LoadImage1(bmp);
                //this.btnEditROI.Text = editmodeCaption;
                button1.Enabled = false;
                btnShoePeel.Enabled = false;

                btnEditRoi1.BackColor = Color.LightBlue;
                elementHost1.Visible = false;
                this.pbxEditRoi.Visible = true;
                this.pbxEditRoi.BringToFront();
                //
                btnAddROI.Enabled = true;
                using (Graphics g = Graphics.FromImage(this.pbxEditRoi.Image))
                {
                    string sDisp = "";
                    sDisp = "ROI EDIT MODE";

                    float fontsize = 24.0f;

                    Brush bb = Brushes.Red;

                    float x = 40.0f;
                    float y = 25.0f;
                    g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Bold), bb, x, y);
                }

                ROIeditMode(Int32.Parse(txtPProiPosX.Text), Int32.Parse(txtPProiPosY.Text), Int32.Parse(txtPProiWidth.Text), Int32.Parse(txtPProiHeight.Text));
                //}

            }
            catch (NullReferenceException e1)
            {
                string message = e1.Message;
                System.Windows.Forms.MessageBox.Show("btnEditROI_Click(), Error: " + message);
            }
        }

        private void btnEditRoi2_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(txtImgPathLoad.Text);

            try
            {
                //if (this.btnEditROI.Text == stdmodeCaption)
                //{
                //---------------------edit mode---------------
                btnEditRoi2.BackColor = Color.LightBlue;
                StaticROI.DisEngage();
                StaticROI.ClearGraphics(true);

                LoadImage1(bmp);
                //this.btnEditROI.Text = editmodeCaption;
                button1.Enabled = false;
                btnShoePeel.Enabled = false;

                btnEditRoi1.BackColor = Color.LightBlue;
                elementHost1.Visible = false;
                this.pbxEditRoi.Visible = true;
                this.pbxEditRoi.BringToFront();
                //
                btnAddROI.Enabled = true;
                using (Graphics g = Graphics.FromImage(this.pbxEditRoi.Image))
                {
                    string sDisp = "";
                    sDisp = "ROI EDIT MODE";

                    float fontsize = 24.0f;

                    Brush bb = Brushes.Red;

                    float x = 40.0f;
                    float y = 25.0f;
                    g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Bold), bb, x, y);
                }

                ROIeditMode(Int32.Parse(txtPosX2.Text), Int32.Parse(txtPosY2.Text), Int32.Parse(txtWidth2.Text), Int32.Parse(txtHeight2.Text));
                //}

            }
            catch (NullReferenceException e1)
            {
                string message = e1.Message;
                System.Windows.Forms.MessageBox.Show("btnEditROI_Click(), Error: " + message);
            }
        }

        private void btnEditRoi3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(txtImgPathLoad.Text);
            try
            {
                //if (this.btnEditROI.Text == stdmodeCaption)
                //{
                //---------------------edit mode---------------
                btnEditRoi3.BackColor = Color.LightBlue;
                StaticROI.DisEngage();
                StaticROI.ClearGraphics(true);

                LoadImage1(bmp);
                //this.btnEditROI.Text = editmodeCaption;
                button1.Enabled = false;
                btnShoePeel.Enabled = false;

                btnEditRoi1.BackColor = Color.LightBlue;
                elementHost1.Visible = false;
                this.pbxEditRoi.Visible = true;
                this.pbxEditRoi.BringToFront();
                //
                btnAddROI.Enabled = true;
                using (Graphics g = Graphics.FromImage(this.pbxEditRoi.Image))
                {
                    string sDisp = "";
                    sDisp = "ROI EDIT MODE";

                    float fontsize = 24.0f;

                    Brush bb = Brushes.Red;

                    float x = 40.0f;
                    float y = 25.0f;
                    g.DrawString(sDisp, new Font("Tahoma", fontsize, System.Drawing.FontStyle.Bold), bb, x, y);
                }

                ROIeditMode(Int32.Parse(txtPosX3.Text), Int32.Parse(txtPosY3.Text), Int32.Parse(txtWidth3.Text), Int32.Parse(txtHeight3.Text));
                //}

            }
            catch (NullReferenceException e1)
            {
                string message = e1.Message;
                System.Windows.Forms.MessageBox.Show("btnEditROI_Click(), Error: " + message);
            }
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            GetROIinfo();

            Rectangle ImageDimensions = new Rectangle();
            System.Windows.Rect rect = new System.Windows.Rect();
            ApplyROI1(true, ImageDimensions, false, rect, txtPProiPosX.Text.Trim(), txtPProiPosY.Text.Trim(),txtPProiWidth.Text.Trim(), txtPProiHeight.Text.Trim(), txtROIangle.Text.Trim(), txtRatioImage2ROI.Text.Trim());
            btnApplyROI.BackColor = Color.LightBlue;
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            GetROIinfo1();

            Rectangle ImageDimensions = new Rectangle();
            System.Windows.Rect rect = new System.Windows.Rect();
            ApplyROI1(true, ImageDimensions, false, rect, txtPosX2.Text.Trim(), txtPosY2.Text.Trim(), txtWidth2.Text.Trim(), txtHeight2.Text.Trim(), txtAngle2.Text.Trim(), txtRatio2.Text.Trim());
            btnApplyROI.BackColor = Color.LightBlue;
        }

        private void btnSave3_Click(object sender, EventArgs e)
        {
            GetROIinfo2();

            Rectangle ImageDimensions = new Rectangle();
            System.Windows.Rect rect = new System.Windows.Rect();
            ApplyROI1(true, ImageDimensions, false, rect, txtPosX3.Text.Trim(), txtPosY3.Text.Trim(), txtWidth3.Text.Trim(), txtHeight3.Text.Trim(), txtAngle3.Text.Trim(), txtRatio3.Text.Trim());
            btnApplyROI.BackColor = Color.LightBlue;
        }

        private void chkROI1_CheckedChanged(object sender, EventArgs e)
        {
            chkROI2.Checked = false;
            chkROI3.Checked = false;
        }

        private void chkROI2_CheckedChanged(object sender, EventArgs e)
        {
            chkROI1.Checked = false;
            chkROI3.Checked = false;
        }

        private void chkROI3_CheckedChanged(object sender, EventArgs e)
        {
            chkROI2.Checked = false;
            chkROI1.Checked = false;
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            txtBlades.Enabled = false;
            txtDiameter.Enabled = false;
            txtLength.Enabled = false;
            txtThresholdsLower.Enabled = false;
            txtThresholdsUpper.Enabled = false;
            txtPProiPosX.Enabled = false;
            txtPProiPosY.Enabled = false;
            txtPProiWidth.Enabled = false;
            txtPProiHeight.Enabled = false;
            txtROIangle.Enabled = false;
            txtRatioImage2ROI.Enabled = false;
            txtAddNum.Enabled = false;
            txtImgPathLoad.Enabled = false;
            chkFractions1.Enabled = false;
            chkPeels1.Enabled = false;
            chkFractions2.Enabled = false;
            chkPeels2.Enabled = false;
            chkFractions3.Enabled = false;
            chkPeels3.Enabled = false;
            txtPlsThresoldLower.Enabled = false;
            txtPlsThresoldUpper.Enabled = false;
            txtPosX2.Enabled = false;
            txtPosY2.Enabled = false;
            txtWidth2.Enabled = false;
            txtHeight2.Enabled = false;
            txtAngle2.Enabled = false;
            txtRatio2.Enabled = false;
            txtPosX3.Enabled = false;
            txtPosY3.Enabled = false;
            txtWidth3.Enabled = false;
            txtHeight3.Enabled = false;
            txtAngle3.Enabled = false;
            txtRatio3.Enabled = false;

            txtBlades.Text = "";
            txtDiameter.Text = "";
            txtLength.Text = "";
            txtThresholdsLower.Text = "";
            txtThresholdsUpper.Text = "";
            txtPProiPosX.Text = "";
            txtPProiPosY.Text = "";
            txtPProiWidth.Text = "";
            txtPProiHeight.Text = "";
            txtROIangle.Text = "";
            txtRatioImage2ROI.Text = "";
            txtAddNum.Text = "";
            txtImgPathLoad.Text = "";
            chkFractions1.Text = "";
            chkPeels1.Text = "";
            chkFractions2.Text = "";
            chkPeels2.Text = "";
            chkFractions3.Text = "";
            chkPeels3.Text = "";
            txtPlsThresoldLower.Text = "";
            txtPlsThresoldUpper.Text = "";
            txtPosX2.Text = "";
            txtPosY2.Text = "";
            txtWidth2.Text = "";
            txtHeight2.Text = "";
            txtAngle2.Text = "";
            txtRatio2.Text = "";
            txtPosX3.Text = "";
            txtPosY3.Text = "";
            txtWidth3.Text = "";
            txtHeight3.Text = "";
            txtAngle3.Text = "";
            txtRatio3.Text = "";

            txtPlsAreaLower.Enabled = false;
            txtPlsAreaUpper.Enabled = false;
            txtAreaLower.Enabled = false;
            txtAreaUpper.Enabled = false;
        }

        private void btnStopROIMode2_Click(object sender, EventArgs e)
        {
            if (txtImgPathLoad.Text != "")
            {
                btnEditRoi1.BackColor = Color.Transparent;
                btnEditRoi2.BackColor = Color.Transparent;
                btnEditRoi3.BackColor = Color.Transparent;

                this.btnEditROI.Text = stdmodeCaption;
                btnDefect.Enabled = true;
                button1.Enabled = true;
                btnShoePeel.Enabled = true;
                btnEditROI.BackColor = Color.Transparent;
                button1.BackColor = Color.Transparent;
                btnShoePeel.BackColor = Color.Transparent;
                this.pbxEditRoi.Visible = false;
                elementHost1.Visible = true;
                this.pbxEditRoi.SendToBack();

                this.pbxEditRoi.Image.Dispose();
                this.pbxEditRoi.Image = null;

                //
                btnAddROI.Enabled = false;
                btnApplyROI.BackColor = Color.Transparent;
                //
                GetROIinfo();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("button Edit Roi(), Error: " + "Load Image First");
            }
        }

        private void btnStopROIMode3_Click(object sender, EventArgs e)
        {
            if (txtImgPathLoad.Text != "")
            {
                btnEditRoi1.BackColor = Color.Transparent;
                btnEditRoi2.BackColor = Color.Transparent;
                btnEditRoi3.BackColor = Color.Transparent;

                this.btnEditROI.Text = stdmodeCaption;
                btnDefect.Enabled = true;
                button1.Enabled = true;
                btnShoePeel.Enabled = true;
                btnEditROI.BackColor = Color.Transparent;
                button1.BackColor = Color.Transparent;
                btnShoePeel.BackColor = Color.Transparent;
                this.pbxEditRoi.Visible = false;
                elementHost1.Visible = true;
                this.pbxEditRoi.SendToBack();

                this.pbxEditRoi.Image.Dispose();
                this.pbxEditRoi.Image = null;

                //
                btnAddROI.Enabled = false;
                btnApplyROI.BackColor = Color.Transparent;
                //
                GetROIinfo();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("button Edit Roi(), Error: " + "Load Image First");
            }
        }

        private void btnCncelEdit1_Click(object sender, EventArgs e)
        {
            if (txtImgPathLoad.Text != "")
            {
                btnEditRoi1.BackColor = Color.Transparent;
                btnEditRoi2.BackColor = Color.Transparent;
                btnEditRoi3.BackColor = Color.Transparent;

                this.btnEditROI.Text = stdmodeCaption;
                btnDefect.Enabled = true;
                button1.Enabled = true;
                btnShoePeel.Enabled = true;
                btnEditROI.BackColor = Color.Transparent;
                button1.BackColor = Color.Transparent;
                btnShoePeel.BackColor = Color.Transparent;
                this.pbxEditRoi.Visible = false;
                elementHost1.Visible = true;
                this.pbxEditRoi.SendToBack();

                this.pbxEditRoi.Image.Dispose();
                this.pbxEditRoi.Image = null;

                //
                btnAddROI.Enabled = false;
                btnApplyROI.BackColor = Color.Transparent;
                //
                GetROIinfo();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("button Edit Roi(), Error: " + "Load Image First");
            }
        }

        private void btnCncelEdit2_Click(object sender, EventArgs e)
        {
            if (txtImgPathLoad.Text != "")
            {
                btnEditRoi1.BackColor = Color.Transparent;
                btnEditRoi2.BackColor = Color.Transparent;
                btnEditRoi3.BackColor = Color.Transparent;

                this.btnEditROI.Text = stdmodeCaption;
                btnDefect.Enabled = true;
                button1.Enabled = true;
                btnShoePeel.Enabled = true;
                btnEditROI.BackColor = Color.Transparent;
                button1.BackColor = Color.Transparent;
                btnShoePeel.BackColor = Color.Transparent;
                this.pbxEditRoi.Visible = false;
                elementHost1.Visible = true;
                this.pbxEditRoi.SendToBack();

                this.pbxEditRoi.Image.Dispose();
                this.pbxEditRoi.Image = null;

                //
                btnAddROI.Enabled = false;
                btnApplyROI.BackColor = Color.Transparent;
                //
                GetROIinfo();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("button Edit Roi(), Error: " + "Load Image First");
            }
        }

        private void btnCncelEdit3_Click(object sender, EventArgs e)
        {
            if (txtImgPathLoad.Text != "")
            {
                btnEditRoi1.BackColor = Color.Transparent;
                btnEditRoi2.BackColor = Color.Transparent;
                btnEditRoi3.BackColor = Color.Transparent;

                this.btnEditROI.Text = stdmodeCaption;
                btnDefect.Enabled = true;
                button1.Enabled = true;
                btnShoePeel.Enabled = true;
                btnEditROI.BackColor = Color.Transparent;
                button1.BackColor = Color.Transparent;
                btnShoePeel.BackColor = Color.Transparent;
                this.pbxEditRoi.Visible = false;
                elementHost1.Visible = true;
                this.pbxEditRoi.SendToBack();

                this.pbxEditRoi.Image.Dispose();
                this.pbxEditRoi.Image = null;

                //
                btnAddROI.Enabled = false;
                btnApplyROI.BackColor = Color.Transparent;
                //
                GetROIinfo();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("button Edit Roi(), Error: " + "Load Image First");
            }
        }

        private void btnSave2Jasson_Click(object sender, EventArgs e)
        {
            saveToJason();
            string name = CmbCatNum.Text;
            CmbCatNum.Items.Add(name);
        }

        private void btnEditJason_Click(object sender, EventArgs e)
        {
            Endmill endmill = new Endmill();
            endmill.EndmillName = CmbCatNum.Text;
            endmill.CatalogNo =Int32.Parse(txtAddNum.Text);
            endmill.BladesNo = Int32.Parse(txtBlades.Text);
            endmill.Diameter = Int32.Parse(txtDiameter.Text);
            endmill.Length = Int32.Parse(txtLength.Text);
            endmill.FracLowerThreshold = float.Parse(txtThresholdsLower.Text);
            endmill.FracUpperThreshold = float.Parse(txtThresholdsUpper.Text);
            endmill.PeelLowerThreshold = float.Parse(txtPlsThresoldLower.Text);
            endmill.PeelUpperThreshold = float.Parse(txtPlsThresoldUpper.Text);
            endmill.roiPosX1 = Int32.Parse(txtPProiPosX.Text);
            endmill.roiPosY1 = Int32.Parse (txtPProiPosY.Text);
            endmill.roiWidth1 = Int32.Parse(txtPProiWidth.Text);
            endmill.roiHeight1 = Int32.Parse(txtPProiHeight.Text);
            endmill.roiAngle1 = float.Parse(txtROIangle.Text);
            endmill.roiRatio1 = float.Parse(txtRatioImage2ROI.Text);
            endmill.roiPosX2 = Int32.Parse(txtPosX2.Text);
            endmill.roiPosY2 = Int32.Parse(txtPosY2.Text);
            endmill.roiWidth2 = Int32.Parse(txtWidth2.Text);
            endmill.roiHeight2 = Int32.Parse(txtHeight2.Text);
            endmill.roiAngle2 = float.Parse(txtAngle2.Text);
            endmill.roiRatio2 = float.Parse(txtRatio2.Text);
            endmill.roiPosX3 = Int32.Parse(txtPosX3.Text);
            endmill.roiPosY3 = Int32.Parse(txtPosY3.Text);
            endmill.roiWidth3 = Int32.Parse(txtWidth3.Text);
            endmill.roiHeight3 = Int32.Parse(txtHeight3.Text);
            endmill.roiAngle3 = float.Parse(txtAngle3.Text);
            endmill.roiRatio3 = float.Parse(txtRatio3.Text);

            endmill.FracLowerArea = float.Parse(txtAreaLower.Text);
            endmill.FracUpperArea = float.Parse(txtAreaUpper.Text);
            endmill.PeelLowerArea = float.Parse(txtPlsAreaLower.Text);
            endmill.PeelUpperArea = float.Parse(txtPlsAreaUpper.Text);

            endmill.ImagePath = txtImgPathLoad.Text;
            if (chkFractions1.Checked == true)
            {
                endmill.IsFractions1 = "true";
            }
            else endmill.IsFractions1 = "false";
            //endmill.IsFractions1 = chkFractions1.Checked.ToString();
            if (chkPeels1.Checked == true)
            {
                endmill.IsPeels1 = "true";
            }
            else endmill.IsPeels1 = "false";
            //endmill.IsPeels1 = chkPeels1.Checked.ToString();
            if (chkFractions2.Checked == true)
            {
                endmill.IsFractions2 = "true";
            }
            else endmill.IsFractions2 = "false";
            //endmill.IsFractions2 = chkFractions2.Checked.ToString();
            if (chkPeels2.Checked == true)
            {
                endmill.IsPeels2 = "true";
            }
            else endmill.IsPeels2 = "false";
            //endmill.IsPeels2 = chkPeels2.Checked.ToString();
            if (chkFractions3.Checked == true)
            {
                endmill.IsFractions3 = "true";
            }
            else endmill.IsFractions3 = "false";
            //endmill.IsFractions3 = chkFractions3.Checked.ToString();
            if (chkPeels3.Checked == true)
            {
                endmill.IsPeels3 = "true";
            }
            else endmill.IsPeels3 = "false";
            //endmill.IsPeels3 = chkPeels3.Checked.ToString();
            if (chkROI1.Checked == true)
            {
                endmill.Roi1 = "true";
            }
            else endmill.Roi1 = "false";
            //endmill.Roi1 = chkROI1.Checked.ToString();
            if (chkROI2.Checked == true)
            {
                endmill.Roi2 = "true";
            }
            else endmill.Roi2 = "false";
            //endmill.Roi2 = chkROI2.Checked.ToString();
            if (chkROI3.Checked == true)
            {
                endmill.Roi3 = "true";
            }
            else endmill.Roi3 = "false";
            //endmill.Roi3 = chkROI3.Checked.ToString();
            UpdateJson(endmill.EndmillName, endmill);
        }
    }
}
