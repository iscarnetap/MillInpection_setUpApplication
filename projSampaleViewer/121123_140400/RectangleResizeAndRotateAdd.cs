using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DefectsInspection
{
    
    #region -----------------Name space level Declarations---------------------


    #endregion



    public class RectangleResizeAndRotateAdd
    {
        //Author: Yoav Tamari
        //Project: PR55922
        //Created: 
        //Description: 
        //History: 16/02/2022, search mask working good when change to theta: -90< theta change < +90, theta = angle of individual
        //mask after layed on image
        //20/03/2022: check in mask in relation to center of rectangle (lines: 506, 610)


        //constructor
        
        public RectangleResizeAndRotateAdd()
        {
            //Center location of our item
            _rectPos = new PointF(200, 200);

            //The rectangle dimensions in _rectPos space
            _rect = new RectangleF(-40, -30, 80, 60);

            _rectRotation = 0;
        }
        public RectangleResizeAndRotateAdd(float width, float height)
        {
            //Center location of our item
            _rectPos = new PointF(200, 200);

            //The rectangle dimensions in _rectPos space
            _rect = new RectangleF(-width / 2, -height / 2, width, height);

            _rectRotation = 0;
        }
        public RectangleResizeAndRotateAdd(float posX,float posY,float width, float height)
        {
            //Center location of our item
            _rectPos = new PointF(posX + width / 2, posY + height/2);  //correct formula 17/10/2023, //good testing:  _rectPos = new PointF(posX + 200/2, posY);, _rectPos = new PointF(posX + width / 2, posY);
            
            //The rectangle dimensions in _rectPos space
            _rect = new RectangleF(-width / 2, -height / 2, width, height); //correct formula 17/10/2023



            //_rect = new RectangleF(-100, -50, 200, 100); //good testing

            _rectRotation = 0;
        }
        public RectangleResizeAndRotateAdd(float posX, float posY, float width, float height, float angle)
        {
            //Center location of our item
            _rectPos = new PointF(posX, posY);

            //The rectangle dimensions in _rectPos space
            _rect = new RectangleF(-width / 2, -height / 2, width, height);

            _rectRotation = angle;
        }
        #region -----------------Class level Declarations---------------------
        public struct RectangleFullDetails
        {
            public RectangleF rectangleF;
            public float angle;  //in degrees
            public PointF translation;
            public readonly string rect;  // = "rect";
            public Corners RotatedRecCorners;
            //public TransformMaster.LineEquation2D[] LineEquation;
            //public VisualizationMask.SearchLimits searchLimitsRect;
            public bool xLineEquaWScaleINIT;
            public int Index;
            public bool xInternal;    //internal search region subtructed from total external region, mast be with index following the external.
            public float masterAngleFromLocator;
        }
        public struct Corners
        {
            public PointF[] vertix;
            //public bool xNormal;
            //public bool xNormalized; // true = was normalized
            public RectNormal RectNormalStatus;
        }
        public struct RectNormal
        {
            public bool xNormal;
            public bool xNormalized; // true = was normalized

            public bool xYnormal;   //y normal upside up.
            public bool xMpositive;  //tiltied right
            public bool xX01Flip;
        }
        public struct RegionEvalResults
        {
            public double Ytop;
            public double Ybottom;

            public double Xright;
            public double Xleft;

            public bool Yin;
            public bool Xin;

            public bool xCoordinateInRegion;
            public int regionNumber;

            public float percentRectIn;

             
        }
        public struct RegionEvalResultsPerClass
        {
            public RegionEvalResults[] regionEvalResults;
            public RegionEvalResults[] InternalregionEvalResults;
        }

        public struct RegionsSearchResults
        {
            public RegionEvalResultsPerClass[] rgionEvalResultsPerClass;

            public string SearchTimeElapse;

        }

        //private PictureBox mPictureBox;
        [JsonIgnore]
        public PictureBox mPictureBox;
        public enum AnchorPoint
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Rotation,
            Center
        }
        public struct fPoint
        {
            public float x;
            public float y;
        }

        private bool _drag;

        private SizeF _dragSize;
        private PointF _dragStart;
        private PointF _dragStartOffset;
        private RectangleF _dragRect;
        private AnchorPoint _dragAnchor;
        private Single _dragRot;

        [JsonProperty("_rect")]
        private RectangleF _rect;

        [JsonProperty("_rectPos")]
        private PointF _rectPos;

        [JsonProperty("_rectRotation")]
        public Single _rectRotation;

        private bool xClearGraphics = false;

        public RectangleFullDetails rectangleFullDetails;
        public int gMaskIndex;

        [JsonProperty("gMaskMasterAngle")]
        private float gMaskMasterAngle;

        public string useAsROI;

        [JsonProperty("lastValrectRotation")]
        private Single lastValrectRotation;

        private int iSloawDown;
        public bool xSlowDownPain = false;
        #endregion

        #region-------------------------Methods -----------------------------        
        public void SetPictureBox(PictureBox p)
        {
            this.mPictureBox = p;
            mPictureBox.MouseDown += new MouseEventHandler(mPictureBox_MouseDown);
            mPictureBox.MouseUp += new MouseEventHandler(mPictureBox_MouseUp);
            mPictureBox.MouseMove += new MouseEventHandler(mPictureBox_MouseMove);
            mPictureBox.Paint += new PaintEventHandler(mPictureBox_Paint);

            if (mPictureBox != null) { mPictureBox.Refresh(); }
        }
        public void ConnectDiscPaint(PictureBox p,bool xDiscConnect)
        {
            this.mPictureBox = p;

            if (xDiscConnect)
                mPictureBox.Paint += new PaintEventHandler(mPictureBox_Paint);
            else
                this.mPictureBox.Paint -= new PaintEventHandler(mPictureBox_Paint);
        }
        public RectangleFullDetails GetRectF()
        {
            
            RectangleFullDetails rectangleFdetails = new RectangleFullDetails();
            rectangleFdetails = this.rectangleFullDetails;

            rectangleFdetails.rectangleF.X = _rect.X;                    // X location 1
            rectangleFdetails.rectangleF.Y = _rect.Y;                    // Y location 2

            rectangleFdetails.rectangleF.Width = _rect.Width;                 // Width 3 
            rectangleFdetails.rectangleF.Height = _rect.Height;                //Height 4 

            rectangleFdetails.angle = _rectRotation;              //in degrees  Rotation 5

            rectangleFdetails.translation.X = _rectPos.X;         //        trtanslation 6
            rectangleFdetails.translation.Y = _rectPos.Y;         //        trtanslation 7

            rectangleFdetails.masterAngleFromLocator = gMaskMasterAngle;  //locator angle 8

            //new calculate rotated rectangular corners
            RectangleResizeAndRotateAdd.RectangleFullDetails rectRegion = CalcSortRegion(rectangleFdetails);
            rectangleFdetails.RotatedRecCorners = rectRegion.RotatedRecCorners;

            return rectangleFdetails;
             
        }
        public RectangleFullDetails GetRectF(bool xFullScale, float[] scale)
        {
            //scale[0] - x scale
            //scale[1] - y scale

            RectangleFullDetails rectangleFdetails = new RectangleFullDetails();
            rectangleFdetails = this.rectangleFullDetails;

            rectangleFdetails.rectangleF.X = _rect.X;                    // X location 1
            rectangleFdetails.rectangleF.Y = _rect.Y;                    // Y location 2

            rectangleFdetails.rectangleF.Width = _rect.Width;                 // Width 3 
            rectangleFdetails.rectangleF.Height = _rect.Height;                //Height 4 

            rectangleFdetails.angle = _rectRotation;              //in degrees  Rotation 5

            rectangleFdetails.translation.X = _rectPos.X;         //        trtanslation 6
            rectangleFdetails.translation.Y = _rectPos.Y;         //        trtanslation 7

            //new calculate rotated rectangular corners
            if (!xFullScale)
            {
                RectangleResizeAndRotateAdd.RectangleFullDetails rectRegion = CalcSortRegion(rectangleFdetails);
                rectangleFdetails.RotatedRecCorners = rectRegion.RotatedRecCorners;
            }
            else
            {


                rectangleFdetails = new RectangleFullDetails();

                rectangleFdetails = this.rectangleFullDetails;

                rectangleFdetails.rectangleF.Width = _rect.Width;
                rectangleFdetails.rectangleF.Height = _rect.Height;

                rectangleFdetails.translation.X = _rectPos.X * scale[0];
                rectangleFdetails.translation.Y = _rectPos.Y * scale[1];

                rectangleFdetails.rectangleF.Width = rectangleFdetails.rectangleF.Width * scale[0];
                rectangleFdetails.rectangleF.Height = rectangleFdetails.rectangleF.Height * scale[1];
                               
                //working
                rectangleFdetails.rectangleF.X = _rect.X * scale[0];
                rectangleFdetails.rectangleF.Y = _rect.Y * scale[1];

                rectangleFdetails.angle = _rectRotation;


                RectangleResizeAndRotateAdd.RectangleFullDetails rectRegion = CalcSortRegion(rectangleFdetails);

                rectangleFdetails.RotatedRecCorners = rectRegion.RotatedRecCorners;
            }

            return rectangleFdetails;

        }
        public void SetRectFL(bool xFullScale, float[] scale)
        {

            //new calculate rotated rectangular corners
            if (!xFullScale)
            {
                //RectangleResizeAndRotateAdd.RectangleFullDetails rectRegion = CalcSortRegion(rectangleFdetails);
                //rectangleFdetails.RotatedRecCorners = rectRegion.RotatedRecCorners;
            }
            else
            {


                this.rectangleFullDetails.translation.X = _rectPos.X * scale[0];
                this.rectangleFullDetails.translation.Y = _rectPos.Y * scale[1];

                _rectPos.X = this.rectangleFullDetails.translation.X;
                _rectPos.Y = this.rectangleFullDetails.translation.Y;


                this.rectangleFullDetails.rectangleF.X = _rect.X * scale[0];
                this.rectangleFullDetails.rectangleF.Y = _rect.Y * scale[1];

                _rect.X = this.rectangleFullDetails.rectangleF.X;
                _rect.Y = this.rectangleFullDetails.rectangleF.Y;


                this.rectangleFullDetails.rectangleF.Width = _rect.Width * scale[0];
                this.rectangleFullDetails.rectangleF.Height = _rect.Height * scale[1];

                _rect.Width = this.rectangleFullDetails.rectangleF.Width;
                _rect.Height = this.rectangleFullDetails.rectangleF.Height;


                this.rectangleFullDetails.angle = _rectRotation;


                //RectangleResizeAndRotateAdd.RectangleFullDetails rectRegion = CalcSortRegion(rectangleFdetails);

                //rectangleFdetails.RotatedRecCorners = rectRegion.RotatedRecCorners;
            }

            //return rectangleFdetails;

        }
        public void DisEngage()
        {
            if (mPictureBox != null)
            {
                //uncouple delegates              
                mPictureBox.MouseDown -= new MouseEventHandler(mPictureBox_MouseDown);
                mPictureBox.MouseUp -= new MouseEventHandler(mPictureBox_MouseUp);
                mPictureBox.MouseMove -= new MouseEventHandler(mPictureBox_MouseMove);
                mPictureBox.Paint -= new PaintEventHandler(mPictureBox_Paint);
            }
            //mPictureBox.Dispose();
        }
        public void ClearGraphics(bool xClear)
        {
            xClearGraphics = xClear;
            if (mPictureBox != null) { mPictureBox.Refresh(); }
        }
        //public void LayRectFromFile(Masks.EvaluationMask mask, PictureBox p,bool xNoDisplay, float MaskMasterAngle)
        //{
        //    float x              = mask.X;
        //    float y              = mask.Y;
        //    float width          = mask.Width;
        //    float height         = mask.Height;
        //    float RoptationAngle = mask.Angle;

        //    float offsetX = mask.offsetX;
        //    float offsetY = mask.offsetY;

        //    gMaskMasterAngle = MaskMasterAngle;
        //    //

        //    //Center location of our item
        //    _rectPos = new PointF(offsetX, offsetY);

        //    //The rectangle dimensions in _rectPos space
        //    _rect = new RectangleF(x, y, width, height);

        //    _rectRotation = RoptationAngle;

        //    if (!xNoDisplay)
        //    {
        //        SetPictureBox(p);
        //    }

        //    this.rectangleFullDetails.Index     = mask.Index;
        //    this.rectangleFullDetails.xInternal = mask.xInternalMask;
        //}
        public RectangleFullDetails CalcSortRegion(RectangleResizeAndRotateAdd.RectangleFullDetails recFF)
        {
            //Yoav Tamari
            //Created: 12/08/2021
            //Description: rotate project standard resizeable rectanglurs around there anchor coordinates.
            //Works good

            //History:

            RectangleFullDetails Allout = new RectangleFullDetails();
            Allout = recFF;

            Corners CornersOut = new Corners();
            CornersOut.vertix = new PointF[4];

            Corners CornersOut00 = new Corners();
            CornersOut00.vertix = new PointF[4];


            float angleDeg     = recFF.angle;
            PointF translation = recFF.translation;
            RectangleF rectF   = recFF.rectangleF;

            //calculate all unrotated corners
            Corners corners = new Corners();
            corners.vertix = new PointF[4];

            //topleft
            corners.vertix[0].X = rectF.Left;
            corners.vertix[0].Y = rectF.Top;

            //topright
            corners.vertix[1].X = rectF.Right;
            corners.vertix[1].Y = rectF.Top;

            //bottom right
            corners.vertix[2].X = rectF.Right;
            corners.vertix[2].Y = rectF.Bottom;

            //bottom left
            corners.vertix[3].X = corners.vertix[0].X;
            corners.vertix[3].Y = corners.vertix[2].Y;

            //normalize cornerns
            Corners corners01 = CornersNormalizationOrth(corners);


            //calc center of rotation  coordinates
            PointF centerF = new PointF();
            centerF.X  = translation.X;
            centerF.Y  = translation.Y;


            //translate zero angle rectangle to image coordinates before rotating
            var mat00 = new Matrix();             
            mat00.Translate(translation.X, translation.Y);
            mat00.Rotate(0.0f); 
            
            CornersOut00.vertix[0] = mat00.TransformPoint(corners.vertix[0]);
            CornersOut00.vertix[1] = mat00.TransformPoint(corners.vertix[1]);
            CornersOut00.vertix[2] = mat00.TransformPoint(corners.vertix[2]);
            CornersOut00.vertix[3] = mat00.TransformPoint(corners.vertix[3]);


            //rotation
            var mat = new Matrix();
            mat.Translate(0.0f, 0.0f);
            //mat.Rotate(angleDeg); //good but not rotated around correct center. Need to rotate around anchor point
            mat.RotateAt(angleDeg, centerF);
            //mat.Invert();  

            CornersOut.vertix[0] = mat.TransformPoint(CornersOut00.vertix[0]);
            CornersOut.vertix[1] = mat.TransformPoint(CornersOut00.vertix[1]);
            CornersOut.vertix[2] = mat.TransformPoint(CornersOut00.vertix[2]);
            CornersOut.vertix[3] = mat.TransformPoint(CornersOut00.vertix[3]);

            //normalize cornerns
            //Corners corners02 = CornersNormalizationGnr(CornersOut);

            Allout.RotatedRecCorners = CornersOut;

            return Allout;
        }        
        public Corners CornersNormalizationOrth(Corners CornersIN)
        {
            //orthagonal

            Corners CornersOut = new Corners();

            float[] X = new float[CornersIN.vertix.GetLength(0)];
            float[] Y = new float[CornersIN.vertix.GetLength(0)];

            for (int i=0;i< CornersIN.vertix.GetLength(0);i++)
            {
                X[i] = CornersIN.vertix[i].X;
                Y[i] = CornersIN.vertix[i].Y;
            }

            //check if normal X
            bool X03Normal       = false;
            bool X12Normal       = false;
            bool Xrelation03to12 = false;
            bool Xnormal         = false;

            X03Normal       = X[0] == X[3];
            X12Normal       = X[1] == X[2];
            Xrelation03to12 = X[0] < X[1];

            Xnormal = X03Normal && X12Normal && Xrelation03to12;



            //check if normal y
            bool Y01Normal       = false;
            bool Y23Normal       = false;
            bool Yrelation01to23 = false;
            bool Ynormal         = false;

            Y01Normal       = Y[0] == Y[1];
            Y23Normal       = Y[2] == Y[3];
            Yrelation01to23 = Y[0] < Y[2]; 

            Ynormal = Y01Normal && Y23Normal && Yrelation01to23;

            CornersOut.RectNormalStatus.xNormal =  Xnormal && Ynormal;



            return CornersOut;

        }

        //public Corners CornersNormalizationGnr(Corners CornersIN)
        //{
        //    //general

        //    Corners CornersOut = new Corners();

        //    float[] X = new float[CornersIN.vertix.GetLength(0)];
        //    float[] Y = new float[CornersIN.vertix.GetLength(0)];

        //    for (int i = 0; i < CornersIN.vertix.GetLength(0); i++)
        //    {
        //        X[i] = CornersIN.vertix[i].X;
        //        Y[i] = CornersIN.vertix[i].Y;
        //    }

        //    //check if normal X
        //    bool X03Normal = false;
        //    bool X12Normal = false;
        //    bool Xrelation03to12 = false;
        //    bool Xnormal = false;

        //    X03Normal = X[0] >= X[3];
        //    X12Normal = X[1] == X[2];
        //    Xrelation03to12 = X[0] < X[1];

        //    Xnormal = X03Normal && X12Normal && Xrelation03to12;



        //    //check if normal y
        //    bool Y01Normal = false;
        //    bool Y23Normal = false;
        //    bool Yrelation01to23 = false;
        //    bool Ynormal = false;

        //    Y01Normal = Y[0] == Y[1];
        //    Y23Normal = Y[2] == Y[3];
        //    Yrelation01to23 = Y[0] < Y[2];

        //    Ynormal = Y01Normal && Y23Normal && Yrelation01to23;

        //    CornersOut.RectNormalStatus.xNormal = Xnormal && Ynormal;



        //    return CornersOut;

        //}

        private float sign(fPoint p1, fPoint p2, fPoint p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }
        private double dist2(fPoint p1, fPoint p2)
        {
            return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
        }
        private bool PointInTriangle(fPoint pt, fPoint v1, fPoint v2, fPoint v3)
        {
            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = sign(pt, v1, v2);
            d2 = sign(pt, v2, v3);
            d3 = sign(pt, v3, v1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }
        public bool IsIn(fPoint pt, fPoint v1, fPoint v2, fPoint v3, fPoint v4)
        {
            bool bres = PointInTriangle(pt, v1, v2, v3);
            if (!bres)
            {
                double d1 = dist2(v4, v1);
                double d2 = dist2(v4, v2);
                double d3 = dist2(v4, v3);
                if (d1 < d2)
                {
                    if (d2 < d3)
                        bres = PointInTriangle(pt, v4, v1, v2);
                    else
                        bres = PointInTriangle(pt, v4, v1, v3);
                }
                else
                {
                    if (d2 < d3)
                        bres = PointInTriangle(pt, v4, v1, v2);
                    else
                        bres = PointInTriangle(pt, v4, v2, v3);
                }
            }
            return bres;
        }
        #endregion

        #region---------------Methods For Events, Delegates------------------
        private void mPictureBox_Paint(object sender, PaintEventArgs e)
        {
            //if(xSlowDownPain)
            //{
            //    if (iSloawDown < 1)
            //    {
            //        iSloawDown++;
            //        goto exitProcedure;
            //    }
            //    else
            //    {
            //        iSloawDown = 0;
            //    }
            //}

            bool xUseProcedureExt = true;
            if (!xUseProcedureExt)
            {
                var gc = e.Graphics;

                // Move Graphics handler to Rectangle's space
                var mat = new Matrix();
                mat.Translate(_rectPos.X, _rectPos.Y);
                mat.Rotate(_rectRotation);
                gc.Transform = mat;

                bool xRectanleWithFill = false;

                if (xRectanleWithFill)
                {
                    // All out gizmo rectangles are defined in Rectangle Space
                    var rectTopLeft = new RectangleF(_rect.Left - 5f, _rect.Top - 5f, 10f, 10f);
                    var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top - 5f, 10f, 10f);
                    var rectBottomLeft = new RectangleF(_rect.Left - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
                    var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
                    var rectRotate = new RectangleF(-5, _rect.Top + -30, 10f, 10f);
                    var rectCenter = new RectangleF(-5, -5, 10f, 10f);

                    var backBrush = new SolidBrush(Color.CadetBlue);
                    var cornerBrush = new SolidBrush(Color.OrangeRed);

                    // Looks rotated because we've transformed the graphics context
                    bool xNoFill = false;
                    if (!xNoFill)
                    {
                        gc.FillRectangle(backBrush, _rect);
                        gc.FillRectangle(cornerBrush, rectTopLeft);
                        gc.FillRectangle(cornerBrush, rectTopRight);
                        gc.FillRectangle(cornerBrush, rectBottomLeft);
                        gc.FillRectangle(cornerBrush, rectBottomRight);
                        gc.FillRectangle(cornerBrush, rectRotate);
                        gc.FillRectangle(cornerBrush, rectCenter);
                    }
                }
                else
                {
                    // All out gizmo rectangles are defined in Rectangle Space

                    var rectTopLeft = new RectangleF(_rect.Left - 2.5f, _rect.Top - 2.5f, 5f, 5f);
                    var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 2.5f, _rect.Top - 2.5f, 5f, 5f);
                    var rectBottomLeft = new RectangleF(_rect.Left - 2.5f, _rect.Top + _rect.Height - 2.5f, 5f, 5f);
                    var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 2.5f, _rect.Top + _rect.Height - 2.5f, 5f, 5f);
                    var rectRotate = new RectangleF(-2.5f, _rect.Top + -30, 5f, 5f);
                    var rectCenter = new RectangleF(-2.5f, -2.5f, 5f, 5f);

                    var backBrush = new SolidBrush(Color.CadetBlue);
                    var cornerBrush = new SolidBrush(Color.OrangeRed);                  

                    // Looks rotated because we've transformed the graphics context
                    bool xNoFill = true;
                    if (!xNoFill)
                    {
                        gc.FillRectangle(backBrush, _rect);
                        gc.FillRectangle(cornerBrush, rectTopLeft);
                        gc.FillRectangle(cornerBrush, rectTopRight);
                        gc.FillRectangle(cornerBrush, rectBottomLeft);
                        gc.FillRectangle(cornerBrush, rectBottomRight);
                        gc.FillRectangle(cornerBrush, rectRotate);
                        gc.FillRectangle(cornerBrush, rectCenter);
                    }
                    else
                    {



                        //gc.FillRectangle(backBrush, _rect);
                        RectangleF[] recF = new RectangleF[1];
                        recF[0] = _rect;
                        gc.DrawRectangles(new Pen(Color.Green, 2f), recF);
                        gc.FillRectangle(cornerBrush, rectTopLeft);
                        gc.FillRectangle(cornerBrush, rectTopRight);
                        gc.FillRectangle(cornerBrush, rectBottomLeft);

                        gc.FillRectangle(cornerBrush, rectBottomRight);
                        gc.FillRectangle(cornerBrush, rectRotate);
                        gc.FillRectangle(cornerBrush, rectCenter);

                    }



                }
                // Reset Graphics state
                gc.ResetTransform();
            }
            else
            {
                if (!float.IsNaN(_rectRotation))
                {
                    lastValrectRotation = _rectRotation;
                    bool xPrintMaskID = true;
                    AddRec(e, xClearGraphics, this.rectangleFullDetails.Index + 1, xPrintMaskID); //zero base index
                }
                else 
                {
                    _rectRotation = lastValrectRotation;
                    System.Threading.Thread.Sleep(50);
                    bool xPrintMaskID = true;
                    AddRec(e, xClearGraphics, this.rectangleFullDetails.Index + 1, xPrintMaskID); //zero base index
                }
            }

        exitProcedure:;

        }
        private void AddRec(PaintEventArgs e,bool xclear,int maskIndex, bool xPrintMaskID)
        {
            var gc = e.Graphics;

            bool xClear = xclear;

            bool xInternal = rectangleFullDetails.xInternal;

            try
            {
               
                // Move Graphics handler to Rectangle's space
                if (float.IsNaN(_rectRotation)) { goto exitProcedure; }
                var mat = new Matrix();
                mat.Translate(_rectPos.X, _rectPos.Y);
                mat.Rotate(_rectRotation);
                gc.Transform = mat;
               

                bool xRectanleWithFill = false;
                if (!xClear)
                {
                    if (xRectanleWithFill)
                    {
                        // All out gizmo rectangles are defined in Rectangle Space
                        var rectTopLeft = new RectangleF(_rect.Left - 5f, _rect.Top - 5f, 10f, 10f);
                        var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top - 5f, 10f, 10f);
                        var rectBottomLeft = new RectangleF(_rect.Left - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
                        var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
                        var rectRotate = new RectangleF(-5, _rect.Top + -30, 10f, 10f);
                        var rectCenter = new RectangleF(-5, -5, 10f, 10f);

                        var backBrush = new SolidBrush(Color.CadetBlue);
                        var cornerBrush = new SolidBrush(Color.OrangeRed);

                        // Looks rotated because we've transformed the graphics context
                        bool xNoFill = false;
                        if (!xNoFill)
                        {
                            gc.FillRectangle(backBrush, _rect);
                            gc.FillRectangle(cornerBrush, rectTopLeft);
                            gc.FillRectangle(cornerBrush, rectTopRight);
                            gc.FillRectangle(cornerBrush, rectBottomLeft);
                            gc.FillRectangle(cornerBrush, rectBottomRight);
                            gc.FillRectangle(cornerBrush, rectRotate);
                            gc.FillRectangle(cornerBrush, rectCenter);
                        }
                    }
                    else
                    {
                        // All out gizmo rectangles are defined in Rectangle Space
                                              
                        var rectTopLeft = new RectangleF(_rect.Left - 2.5f, _rect.Top - 2.5f, 5f, 5f);
                        var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 2.5f, _rect.Top - 2.5f, 5f, 5f);
                        var rectBottomLeft = new RectangleF(_rect.Left - 2.5f, _rect.Top + _rect.Height - 2.5f, 5f, 5f);
                        var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 2.5f, _rect.Top + _rect.Height - 2.5f, 5f, 5f);
                        var rectRotate = new RectangleF(-2.5f, _rect.Top + -30, 5f, 5f);
                        var rectCenter = new RectangleF(-2.5f, -2.5f, 5f, 5f);
                       
                        var backBrush = new SolidBrush(Color.CadetBlue);
                        var cornerBrush = new SolidBrush(Color.OrangeRed);

                        // Looks rotated because we've transformed the graphics context
                        bool xNoFill = true;
                        if (!xNoFill)
                        {
                            gc.FillRectangle(backBrush, _rect);
                            gc.FillRectangle(cornerBrush, rectTopLeft);
                            gc.FillRectangle(cornerBrush, rectTopRight);
                            gc.FillRectangle(cornerBrush, rectBottomLeft);
                            gc.FillRectangle(cornerBrush, rectBottomRight);
                            gc.FillRectangle(cornerBrush, rectRotate);
                            gc.FillRectangle(cornerBrush, rectCenter);
                        }
                        else
                        {
                            
                            //gc.FillRectangle(backBrush, _rect);
                            RectangleF[] recF = new RectangleF[1];
                            recF[0] = _rect;

                            Color cc;

                            if (!xInternal)
                                cc = Color.Green;
                            else
                                cc = Color.Cyan;

                            gc.DrawRectangles(new Pen(cc, 2f), recF);
                            gc.FillRectangle(cornerBrush, rectTopLeft);
                            gc.FillRectangle(cornerBrush, rectTopRight);
                            gc.FillRectangle(cornerBrush, rectBottomLeft);
                            gc.FillRectangle(cornerBrush, rectBottomRight);
                            gc.FillRectangle(cornerBrush, rectRotate);
                            gc.FillRectangle(cornerBrush, rectCenter);
                           
                            // add mask index

                            //xPrintMaskID = true;
                            if (xPrintMaskID)
                            {
                                Brush bb;

                                string s = maskIndex.ToString();

                                string sDisp = "MN " + s;
                                int Xoffset = 50; // 50;
                                float fontsize = 9.0f;


                                if (!xInternal)
                                    bb = Brushes.Red;
                                else
                                    bb = Brushes.Cyan;

                                gc.DrawString(sDisp, new Font("Tahoma", fontsize, FontStyle.Regular), bb, rectTopLeft.X - Xoffset, rectTopLeft.Y);

                            }


                        }



                    }
                }
                else //clear
                {
                    xClearGraphics = false;
                    gc.Clear(Color.Transparent);
                }
                // Reset Graphics state
                gc.ResetTransform();
            }
            catch(Exception e1)
            {
                MessageBox.Show("AddRec(), Error: " + e1.Message);
            }

        exitProcedure:;

        }
        private void AddCircle(PaintEventArgs e)
        {
            var gc = e.Graphics;

            // Move Graphics handler to Rectangle's space
            var mat = new Matrix();
            mat.Translate(_rectPos.X, _rectPos.Y);
            mat.Rotate(_rectRotation);
            gc.Transform = mat;

            bool xRectanleWithFill = false;

            if (xRectanleWithFill)
            {
                // All out gizmo rectangles are defined in Rectangle Space
                var rectTopLeft = new RectangleF(_rect.Left - 5f, _rect.Top - 5f, 10f, 10f);
                var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top - 5f, 10f, 10f);
                var rectBottomLeft = new RectangleF(_rect.Left - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
                var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
                var rectRotate = new RectangleF(-5, _rect.Top + -30, 10f, 10f);
                var rectCenter = new RectangleF(-5, -5, 10f, 10f);

                var backBrush = new SolidBrush(Color.CadetBlue);
                var cornerBrush = new SolidBrush(Color.OrangeRed);

                // Looks rotated because we've transformed the graphics context
                bool xNoFill = false;
                if (!xNoFill)
                {
                    gc.FillRectangle(backBrush, _rect);
                    gc.FillRectangle(cornerBrush, rectTopLeft);
                    gc.FillRectangle(cornerBrush, rectTopRight);
                    gc.FillRectangle(cornerBrush, rectBottomLeft);
                    gc.FillRectangle(cornerBrush, rectBottomRight);
                    gc.FillRectangle(cornerBrush, rectRotate);
                    gc.FillRectangle(cornerBrush, rectCenter);
                }
            }
            else
            {
                // All out gizmo rectangles are defined in Rectangle Space

                var rectTopLeft = new RectangleF(_rect.Left - 2.5f, _rect.Top - 2.5f, 5f, 5f);
                var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 2.5f, _rect.Top - 2.5f, 5f, 5f);
                var rectBottomLeft = new RectangleF(_rect.Left - 2.5f, _rect.Top + _rect.Height - 2.5f, 5f, 5f);
                var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 2.5f, _rect.Top + _rect.Height - 2.5f, 5f, 5f);
                var rectRotate = new RectangleF(-2.5f, _rect.Top + -30, 5f, 5f);
                var rectCenter = new RectangleF(-2.5f, -2.5f, 5f, 5f);

                var backBrush = new SolidBrush(Color.CadetBlue);
                var cornerBrush = new SolidBrush(Color.OrangeRed);

                // Looks rotated because we've transformed the graphics context
                bool xNoFill = true;
                if (!xNoFill)
                {
                    gc.FillRectangle(backBrush, _rect);
                    gc.FillRectangle(cornerBrush, rectTopLeft);
                    gc.FillRectangle(cornerBrush, rectTopRight);
                    gc.FillRectangle(cornerBrush, rectBottomLeft);
                    gc.FillRectangle(cornerBrush, rectBottomRight);
                    gc.FillRectangle(cornerBrush, rectRotate);
                    gc.FillRectangle(cornerBrush, rectCenter);
                }
                else
                {

                                       
                    gc.DrawEllipse(new Pen(Color.Green, 2f), _rect);
                    gc.FillRectangle(cornerBrush, rectTopLeft);
                    gc.FillRectangle(cornerBrush, rectTopRight);
                    gc.FillRectangle(cornerBrush, rectBottomLeft);
                    gc.FillRectangle(cornerBrush, rectBottomRight);
                    gc.FillRectangle(cornerBrush, rectRotate);
                    gc.FillRectangle(cornerBrush, rectCenter);

                }



            }
            // Reset Graphics state
            gc.ResetTransform();
        }
        private void mPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Compute a Screen to Rectangle transform 

            var mat = new Matrix();
            mat.Translate(_rectPos.X, _rectPos.Y);
            mat.Rotate(_rectRotation);
            mat.Invert();

            // Mouse point in Rectangle's space. 
            var point = mat.TransformPoint(new PointF(e.X, e.Y));

            var rect = _rect;
            var rectTopLeft = new RectangleF(_rect.Left - 5f, _rect.Top - 5f, 10f, 10f);
            var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top - 5f, 10f, 10f);
            var rectBottomLeft = new RectangleF(_rect.Left - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
            var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
            var rectRotate = new RectangleF(-5, _rect.Top + -30, 10f, 10f);

            if (!_drag)
            {
                //We're in Rectangle space now, so its simple box-point intersection test
                if (rectTopLeft.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.TopLeft;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                }
                else if (rectTopRight.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.TopRight;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                }
                else if (rectBottomLeft.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.BottomLeft;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                }
                else if (rectBottomRight.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.BottomRight;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                }
                else if (rectRotate.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.Rotation;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                    _dragRot = _rectRotation;
                }
                else if (rect.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.Center;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                    _dragStartOffset = new PointF(e.X - _rectPos.X, e.Y - _rectPos.Y);
                }
            }
        }
        private void mPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _drag = false;
        }
        private void mPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {

                if (_drag)
                {
                    var mat = new Matrix();

                    mat.Translate(_rectPos.X, _rectPos.Y);
                    mat.Rotate(_rectRotation);
                    mat.Invert();

                    var point = mat.TransformPoint(new PointF(e.X, e.Y));

                    SizeF deltaSize;
                    PointF clamped;

                    switch (_dragAnchor)
                    {
                        case AnchorPoint.TopLeft:

                            clamped = new PointF(Math.Min(0, point.X), Math.Min(0, point.Y));
                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                            _rect = new RectangleF(
                                _dragRect.Left + deltaSize.Width,
                                _dragRect.Top + deltaSize.Height,
                                _dragRect.Width - deltaSize.Width,
                                _dragRect.Height - deltaSize.Height);
                            break;

                        case AnchorPoint.TopRight:
                            clamped = new PointF(Math.Max(0, point.X), Math.Min(0, point.Y));
                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                            _rect = new RectangleF(
                                _dragRect.Left,
                                _dragRect.Top + deltaSize.Height,
                                _dragRect.Width + deltaSize.Width,
                                _dragRect.Height - deltaSize.Height);
                            break;

                        case AnchorPoint.BottomLeft:
                            clamped = new PointF(Math.Min(0, point.X), Math.Max(0, point.Y));
                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                            _rect = new RectangleF(
                                _dragRect.Left + deltaSize.Width,
                                _dragRect.Top,
                                _dragRect.Width - deltaSize.Width,
                                _dragRect.Height + deltaSize.Height);
                            break;

                        case AnchorPoint.BottomRight:
                            clamped = new PointF(Math.Max(0, point.X), Math.Max(0, point.Y));
                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                            _rect = new RectangleF(
                                _dragRect.Left,
                                _dragRect.Top,
                                _dragRect.Width + deltaSize.Width,
                                _dragRect.Height + deltaSize.Height);
                            break;

                        case AnchorPoint.Rotation:
                            var vecX = point.X;
                            var vecY = -point.Y;

                            var len = Math.Sqrt(vecX * vecX + vecY * vecY);

                            var normX = vecX / len;
                            var normY = vecY / len;

                            //In rectangles's space, 
                            //compute dot product between, 
                            //Up and mouse-position vector
                            var dotProduct = (0 * normX + 1 * normY);
                            var angle = Math.Acos(dotProduct);

                            if (point.X < 0)
                                angle = -angle;

                            // Add (delta-radians) to rotation as degrees
                            _rectRotation += (float)((180 / Math.PI) * angle);

                            break;

                        case AnchorPoint.Center:
                            //move this in screen-space
                            _rectPos = new PointF(e.X - _dragStartOffset.X, e.Y - _dragStartOffset.Y);
                            break;
                    }

                    mPictureBox.Invalidate();
                }
            }
            catch(Exception e1)
            {
                DisEngage();
                MessageBox.Show("mPictureBox_MouseMove(), Error: " + e1.Message);
            }
        }
        public void mPictureBoxPaintManual(PaintEventArgs e)
        {
            bool xUseProcedureExt = true;
            if (!xUseProcedureExt)
            {
                var gc = e.Graphics;

                // Move Graphics handler to Rectangle's space
                var mat = new Matrix();
                mat.Translate(_rectPos.X, _rectPos.Y);
                mat.Rotate(_rectRotation);
                gc.Transform = mat;

                bool xRectanleWithFill = false;

                if (xRectanleWithFill)
                {
                    // All out gizmo rectangles are defined in Rectangle Space
                    var rectTopLeft = new RectangleF(_rect.Left - 5f, _rect.Top - 5f, 10f, 10f);
                    var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top - 5f, 10f, 10f);
                    var rectBottomLeft = new RectangleF(_rect.Left - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
                    var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
                    var rectRotate = new RectangleF(-5, _rect.Top + -30, 10f, 10f);
                    var rectCenter = new RectangleF(-5, -5, 10f, 10f);

                    var backBrush = new SolidBrush(Color.CadetBlue);
                    var cornerBrush = new SolidBrush(Color.OrangeRed);

                    // Looks rotated because we've transformed the graphics context
                    bool xNoFill = false;
                    if (!xNoFill)
                    {
                        gc.FillRectangle(backBrush, _rect);
                        gc.FillRectangle(cornerBrush, rectTopLeft);
                        gc.FillRectangle(cornerBrush, rectTopRight);
                        gc.FillRectangle(cornerBrush, rectBottomLeft);
                        gc.FillRectangle(cornerBrush, rectBottomRight);
                        gc.FillRectangle(cornerBrush, rectRotate);
                        gc.FillRectangle(cornerBrush, rectCenter);
                    }
                }
                else
                {
                    // All out gizmo rectangles are defined in Rectangle Space

                    var rectTopLeft = new RectangleF(_rect.Left - 2.5f, _rect.Top - 2.5f, 5f, 5f);
                    var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 2.5f, _rect.Top - 2.5f, 5f, 5f);
                    var rectBottomLeft = new RectangleF(_rect.Left - 2.5f, _rect.Top + _rect.Height - 2.5f, 5f, 5f);
                    var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 2.5f, _rect.Top + _rect.Height - 2.5f, 5f, 5f);
                    var rectRotate = new RectangleF(-2.5f, _rect.Top + -30, 5f, 5f);
                    var rectCenter = new RectangleF(-2.5f, -2.5f, 5f, 5f);

                    var backBrush = new SolidBrush(Color.CadetBlue);
                    var cornerBrush = new SolidBrush(Color.OrangeRed);

                    // Looks rotated because we've transformed the graphics context
                    bool xNoFill = true;
                    if (!xNoFill)
                    {
                        gc.FillRectangle(backBrush, _rect);
                        gc.FillRectangle(cornerBrush, rectTopLeft);
                        gc.FillRectangle(cornerBrush, rectTopRight);
                        gc.FillRectangle(cornerBrush, rectBottomLeft);
                        gc.FillRectangle(cornerBrush, rectBottomRight);
                        gc.FillRectangle(cornerBrush, rectRotate);
                        gc.FillRectangle(cornerBrush, rectCenter);
                    }
                    else
                    {



                        //gc.FillRectangle(backBrush, _rect);
                        RectangleF[] recF = new RectangleF[1];
                        recF[0] = _rect;
                        gc.DrawRectangles(new Pen(Color.Green, 2f), recF);
                        gc.FillRectangle(cornerBrush, rectTopLeft);
                        gc.FillRectangle(cornerBrush, rectTopRight);
                        gc.FillRectangle(cornerBrush, rectBottomLeft);

                        gc.FillRectangle(cornerBrush, rectBottomRight);
                        gc.FillRectangle(cornerBrush, rectRotate);
                        gc.FillRectangle(cornerBrush, rectCenter);

                    }



                }
                // Reset Graphics state
                gc.ResetTransform();
            }
            else
            {
                bool xPrintMaskID = true;
                AddRec(e, xClearGraphics, this.rectangleFullDetails.Index + 1, xPrintMaskID); //zero base index
            }
        }
        public void mPictureBox_MouseDownManual(MouseEventArgs e)
        {
            // Compute a Screen to Rectangle transform 

            var mat = new Matrix();
            mat.Translate(_rectPos.X, _rectPos.Y);
            mat.Rotate(_rectRotation);
            mat.Invert();

            // Mouse point in Rectangle's space. 
            var point = mat.TransformPoint(new PointF(e.X, e.Y));

            var rect = _rect;
            var rectTopLeft = new RectangleF(_rect.Left - 5f, _rect.Top - 5f, 10f, 10f);
            var rectTopRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top - 5f, 10f, 10f);
            var rectBottomLeft = new RectangleF(_rect.Left - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
            var rectBottomRight = new RectangleF(_rect.Left + _rect.Width - 5f, _rect.Top + _rect.Height - 5f, 10f, 10f);
            var rectRotate = new RectangleF(-5, _rect.Top + -30, 10f, 10f);

            if (!_drag)
            {
                //We're in Rectangle space now, so its simple box-point intersection test
                if (rectTopLeft.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.TopLeft;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                }
                else if (rectTopRight.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.TopRight;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                }
                else if (rectBottomLeft.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.BottomLeft;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                }
                else if (rectBottomRight.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.BottomRight;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                }
                else if (rectRotate.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.Rotation;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                    _dragRot = _rectRotation;
                }
                else if (rect.Contains(point))
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.Center;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                    _dragStartOffset = new PointF(e.X - _rectPos.X, e.Y - _rectPos.Y);
                }
                else
                {
                    _drag = true;
                    _dragStart = new PointF(point.X, point.Y);
                    _dragAnchor = AnchorPoint.Center;
                    _dragRect = new RectangleF(_rect.Left, _rect.Top, _rect.Width, _rect.Height);
                    _dragStartOffset = new PointF(e.X - _rectPos.X, e.Y - _rectPos.Y);
                }
            }
        }
        public void mPictureBoxMouseMoveManual(MouseEventArgs e, PictureBox p, bool xdrag)
        {
            try
            {
                this.mPictureBox = p;                

                if (_drag)
                {
                    var mat = new Matrix();

                    mat.Translate(_rectPos.X, _rectPos.Y);
                    mat.Rotate(_rectRotation);
                    mat.Invert();

                    var point = mat.TransformPoint(new PointF(e.X, e.Y));

                    SizeF deltaSize;
                    PointF clamped;

                    switch (_dragAnchor)
                    {
                        case AnchorPoint.TopLeft:

                            clamped = new PointF(Math.Min(0, point.X), Math.Min(0, point.Y));
                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                            _rect = new RectangleF(
                                _dragRect.Left + deltaSize.Width,
                                _dragRect.Top + deltaSize.Height,
                                _dragRect.Width - deltaSize.Width,
                                _dragRect.Height - deltaSize.Height);
                            break;

                        case AnchorPoint.TopRight:
                            clamped = new PointF(Math.Max(0, point.X), Math.Min(0, point.Y));
                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                            _rect = new RectangleF(
                                _dragRect.Left,
                                _dragRect.Top + deltaSize.Height,
                                _dragRect.Width + deltaSize.Width,
                                _dragRect.Height - deltaSize.Height);
                            break;

                        case AnchorPoint.BottomLeft:
                            clamped = new PointF(Math.Min(0, point.X), Math.Max(0, point.Y));
                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                            _rect = new RectangleF(
                                _dragRect.Left + deltaSize.Width,
                                _dragRect.Top,
                                _dragRect.Width - deltaSize.Width,
                                _dragRect.Height + deltaSize.Height);
                            break;

                        case AnchorPoint.BottomRight:
                            clamped = new PointF(Math.Max(0, point.X), Math.Max(0, point.Y));
                            deltaSize = new SizeF(clamped.X - _dragStart.X, clamped.Y - _dragStart.Y);
                            _rect = new RectangleF(
                                _dragRect.Left,
                                _dragRect.Top,
                                _dragRect.Width + deltaSize.Width,
                                _dragRect.Height + deltaSize.Height);
                            break;

                        case AnchorPoint.Rotation:
                            var vecX = point.X;
                            var vecY = -point.Y;

                            var len = Math.Sqrt(vecX * vecX + vecY * vecY);

                            var normX = vecX / len;
                            var normY = vecY / len;

                            //In rectangles's space, 
                            //compute dot product between, 
                            //Up and mouse-position vector
                            var dotProduct = (0 * normX + 1 * normY);
                            var angle = Math.Acos(dotProduct);

                            if (point.X < 0)
                                angle = -angle;

                            // Add (delta-radians) to rotation as degrees
                            _rectRotation += (float)((180 / Math.PI) * angle);

                            break;

                        case AnchorPoint.Center:
                            //move this in screen-space
                            //_rectPos = new PointF(e.X - _dragStartOffset.X, e.Y - _dragStartOffset.Y);
                            //_rectPos = new PointF( _dragStartOffset.X, _dragStartOffset.Y); //new 25/02/2022

                           _rectPos = new PointF(_rectPos.X + 50, _rectPos.Y + 0); //new 25/02/2022


                            break;
                    }

                    this.mPictureBox.Invalidate();
                    _drag = false; //new 25/02/2022
                    Rectangle recl = new Rectangle(3, 0, 1037, 657);
                    PaintEventArgs e2 = new PaintEventArgs(mPictureBox.CreateGraphics(),recl);

                    mPictureBoxPaintManual(e2);
                }
            }
            catch (Exception e1)
            {
                DisEngage();
                MessageBox.Show("mPictureBox_MouseMove(), Error: " + e1.Message);
            }
        }

        #endregion

        #region--------------------Utilities Methods ------------------------
        private PointF[] RotateRectANGLE(RectangleF rectangle,float angleRotate)
        {
            PointF[] translatedCorners = new PointF[4];

            var rect = rectangle;

            PointF TopLeft = new PointF();
            PointF TopRight = new PointF();
            PointF BottomRight = new PointF();
            PointF BottomLeft = new PointF();

            TopLeft.X = rect.X;
            TopLeft.Y = rect.Y;

            TopRight.X = rect.Right;
            TopRight.Y = rect.Y;

            BottomRight.X = rect.Right;
            BottomRight.Y = rect.Bottom;

            BottomLeft.X = rect.X;
            BottomLeft.Y = rect.Bottom;


            PointF[] cornerPoints = { TopLeft, TopRight, BottomRight, BottomLeft };

            var m = new Matrix();

            //define rotation around rect center
            //PointF origen = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

            PointF origen = new PointF(TopLeft.X, TopLeft.Y);

            m.RotateAt(angleRotate, origen);

            //transform corner points
            m.TransformPoints(cornerPoints);

            translatedCorners = cornerPoints;

            return translatedCorners;
        }
        private void RotateRectangle(Graphics g, RectangleF[] r, float angle, PointF translate, string shape)
        {

            using (Matrix m = new Matrix())
            {
                m.Translate(translate.X, translate.Y);
                m.Rotate(angle);

                g.Transform = m;
                if (shape == "rect")
                {
                    g.DrawRectangles(new Pen(Color.Green, 2.0f), r);
                }
                else if (shape == "circ")
                {
                    g.DrawEllipse(new Pen(Color.Green, 2.0f), r[0]);
                }

                //g.ResetTransform();
            }
        }

        #endregion

    }

    public static class MatrixExtension
    {
        //Yoav, needs to be in project, add additional functionallity to class Matrix. Don't delete!
        public static PointF TransformPoint(this Matrix @this, PointF point)
        {
            var points = new[] { point };

            @this.TransformPoints(points);

            return points[0];
        }
    }
}
