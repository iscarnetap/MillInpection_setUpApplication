using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuaKIT_SegmentationEvaluator
{
    public static class ExportResults
    {

        public struct JsonLblData
        {
            public int[][,] ijCoordinates;
            public byte[] bClasses;

        }
        public struct Results
        {           
            public JsonLblData[] Contours;
            public double[] fProb;
            public double[] fUncert;
            public int iClassID;
            public string sDefectID;
            public Rect[] rect;
        }
        public struct ResultsClass
        {
            public Results[] ResultsPerClass;
            public string ResultsPath;
            public string ImageName;
            public string CatalogNumber;
           
        }
        public struct ResultsAndInfo
        {
            public ResultsClass resultsPerClass;
            public string ModelName;
            public float ModelBestLoss;
            public string sEvaluationTime;
            public string sClassesNames;
        }
        public struct Rect
        {
            public int ClassID;
            public int width;
            public int height;
            public int x;
            public int y;
        }
        //public static JsonLblData ConvertOpenCVCountor2JsonLblData(OpenCvSharp.Point[] contour, int iClassID)
        //{

        //    int iNumCountors = contour.Length;
        //    JsonLblData JLBLdata = new JsonLblData();


        //    OpenCvSharp.Point[] countor = contour;
        //    int iCountor = countor.Length; //number of points in the countor
        //    JLBLdata.ijCoordinates = new int[1][,];
        //    JLBLdata.bClasses = new byte[1];
        //    JLBLdata.ijCoordinates[0] = new int[iCountor, 2];
        //    int iPointNum = 0;
        //    foreach (OpenCvSharp.Point item in countor)
        //    {
        //        JLBLdata.ijCoordinates[0][iPointNum, 0] = item.X;
        //        JLBLdata.ijCoordinates[0][iPointNum, 1] = item.Y;
        //        iPointNum = iPointNum + 1;
        //    }

        //    JLBLdata.bClasses[0] = Convert.ToByte(iClassID);



        //    return JLBLdata;
        //}
    }

   
}
