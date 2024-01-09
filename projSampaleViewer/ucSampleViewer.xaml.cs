using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViDi2.UI;
using ViDi2.Runtime;
using ViDi2.UI.ViewModels;
using System.ComponentModel;

namespace projSampaleViewer
{
    //Author: Yoav Tamari
    //Project: EndMill
    //Created: 15-08-2023 
    //Description: 

    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ucSampleViewerClass : UserControl
    {

        public IControl control;
        public IWorkspace workspace;
        public IStream stream;


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public IControl Control
        {
            get { return control; }
            set
            {
                control = value;
                RaisePropertyChanged(nameof(Control));
                RaisePropertyChanged(nameof(Workspaces));
                RaisePropertyChanged(nameof(Stream));
            }
        }
        public IList<IWorkspace> Workspaces => Control.Workspaces.ToList();
        /// <summary>
        /// Gets or sets the current workspace
        /// </summary>
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
        /// <summary>
        /// Gets or sets the current stream
        /// </summary>
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
        public ISampleViewerViewModel SampleViewerViewModel => sampleViewer.ViewModel;
       

        public ucSampleViewerClass()
        {
            ViDiSuiteServiceLocator.Initialize();
            InitializeComponent();
        }
        
        public void saveMarkedImage()
        {
            ViDi2.IImage I = SampleViewerViewModel.Sample.OverlayImage();
            System.Drawing.Bitmap bb = I.Bitmap;
            string savePath = @"C:\ProgramData\Cognex\VisionPro Deep Learning\2.1\Examples\c#\projSampaleViewer\bin\x64\Debug\Data\test1.jpg";
            bb.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            I.Dispose();
            bb.Dispose();            
        }
    }
}
