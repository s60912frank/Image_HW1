using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;


namespace Image
{
    public partial class Form1 : Form
    {
        private Capture webCam = null;//擷取攝影機影像
        private bool _isCamOpen = false;
        private Image<Bgr, Byte> Source_frame = null;   //呈現Load圖片的畫面
        private Image<Bgr, Byte> Result_frame = null;  //最後呈現的畫面(RGB專用)

        //FOR CAMSHIFT
        private Rectangle sourceRect = new Rectangle();

        //scale
        private double _xScale;
        private double _yScale;
        private int _pressX;
        private int _pressY;

        private Process nowProcess = Process.Binarization;
        private enum Process
        {
            Binarization,
            MedianFilter
        };

        private LiveProcess nowLiveprocess = LiveProcess.None;
        private enum LiveProcess
        {
            None,
            FaceDetection,
            GrayScale,
            Binarization,
            Negtive,
            Mirror,
            Relief,
            CamShift
        };
   
        public Form1()
        {
            InitializeComponent();
        }

        //讀取圖片
        private void load_Button_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Source_frame = new Image<Bgr, byte>(openFileDialog.FileName);     //將圖片資訊存到Source_frame
                //SourcePictureBox.Image = Source_frame.ToBitmap();   //將Source_frame轉成Bitmap格式呈現到PictureBox
                SourcePictureBox.Image = Source_frame.ToBitmap();
                ReSizePictureBox();
                saveButton.Enabled=grayButton.Enabled = reliefButton.Enabled = negativeButton.Enabled = mirrorButton.Enabled = binarizationButton.Enabled = MedianButton.Enabled = true;
            }
        }

        private void webButton_Click(object sender, EventArgs e)
        {
            if (_isCamOpen)
            {
                webButton.Text = "開啟攝影機";
                webCam = null;
                Application.Idle -= Application_Idle;
                saveButton.Enabled = grayButton.Enabled = reliefButton.Enabled = negativeButton.Enabled = mirrorButton.Enabled = binarizationButton.Enabled = MedianButton.Enabled = false;
                _isCamOpen = false;
            }
            else
            {
                webButton.Text = "關閉攝影機";
                webCam = new Capture();
                Application.Idle += new EventHandler(Application_Idle);
                saveButton.Enabled = grayButton.Enabled = reliefButton.Enabled = negativeButton.Enabled = mirrorButton.Enabled = binarizationButton.Enabled = MedianButton.Enabled = true;
                _isCamOpen = true;
            }
        }

        //private void SetFrameToBox(Image<Bgr,Byte> source, PictureBox box)
        //{
        //    _yScale = (double)source.Height / SourcePictureBox.Height;

        //    int fixedWidth = box.Width;
        //    double ratio = (double)source.Height / (double)source.Width;
        //    source.Resize(fixedWidth, (int)(fixedWidth * ratio), Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
        //    box.Size = new Size(fixedWidth, (int)(fixedWidth * ratio));
        //    box.Image = source.ToBitmap();
        //}

        //調整PictureBox為Image大小.
        private void ReSizePictureBox()
        {
            if (Source_frame != null)
            {
                _yScale = (double)Source_frame.Height / SourcePictureBox.Height;
                SourcePictureBox.Size = new Size((int)(Source_frame.Width / _yScale), SourcePictureBox.Height);
                OutputPictureBox.Image = Source_frame.ToBitmap();
            }
        }

        void Application_Idle(object sender, EventArgs e)
        {
            Source_frame = webCam.QueryFrame().Convert<Bgr, byte>();
            SourcePictureBox.Image = Source_frame.ToBitmap();
            //SetFrameToBox(Source_frame, SourcePictureBox);
            switch (nowLiveprocess)
            {
                case LiveProcess.FaceDetection:
                    FaceDetection();
                    break;
                case LiveProcess.Binarization:
                    Binarization();
                    break;
                case LiveProcess.GrayScale:
                    GrayScale();
                    break;
                case LiveProcess.Negtive:
                    Negtive();
                    break;
                case LiveProcess.Mirror:
                    Mirror();
                    break;
                case LiveProcess.Relief:
                    Relief();
                    break;
                case LiveProcess.CamShift:
                    //camShiftSource = camImage;
                    //ObjectTracking(camImage, sourceRect);
                    //ObjectTracking(Source_frame, sourceRect);
                    //camShiftSource = camImage;
                    KeepTracking();
                    break;
            }
        }

        private void faceButton_Click(object sender, EventArgs e)//因為我系統找不到Camera, 無法實測功能是否正確
        {
            nowLiveprocess = LiveProcess.FaceDetection;
        }

        //灰階
        private void grayButton_Click(object sender, EventArgs e)  
        {
            if (_isCamOpen)
            {
                nowLiveprocess = LiveProcess.GrayScale;
            }
            else
            {
                GrayScale();
            }
        }

        //二值化
        private void binarizationButton_Click(object sender, EventArgs e)
        {
            thresholdBar.Enabled = true;
            thresholdBar.Maximum = Byte.MaxValue;
            thresholdBar.Minimum = Byte.MinValue;
            thresholdBar.Value = 127;
            if (_isCamOpen)
            {
                nowLiveprocess = LiveProcess.Binarization;
            }
            else
            {
                nowProcess = Process.Binarization;
                Binarization();
            }
        }

        //負片
        private void negativeButton_Click(object sender, EventArgs e)
        {
            if (_isCamOpen)
            {
                nowLiveprocess = LiveProcess.Negtive;
            }
            else
            {
                Negtive();
            }
        }
     
        //鏡射
        private void mirrorButton_Click(object sender, EventArgs e)
        {
            if (_isCamOpen)
            {
                nowLiveprocess = LiveProcess.Mirror;
            }
            else
            {
                Mirror();
            }
        }

        // 浮雕
        private void reliefButton_Click(object sender, EventArgs e) 
        {
            if (_isCamOpen)
            {
                nowLiveprocess = LiveProcess.Relief;
            }
            else
            {
                Relief();
            }
        }

        //中值濾波
        private void MedianButton_Click(object sender, EventArgs e)
        {
            thresholdBar.Enabled = true;
            nowProcess = Process.MedianFilter;
            thresholdBar.Maximum = 10;
            thresholdBar.Minimum = 3;
            thresholdBar.Value = 3;
            MedianFilter();
        }

        //結束主程式
        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(Environment.ExitCode);
        }

        //儲存圖片
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (OutputPictureBox.Image != null)   /////圖片會存到 Image\Image\bin 裡
                OutputPictureBox.Image.Save("../output.jpeg", ImageFormat.Jpeg);
        }

        private void thresholdBar_Scroll(object sender, EventArgs e) //拉那個什麼的時候會觸發的事件
        {
            TrackBar bar = sender as TrackBar;
            if (nowProcess == Process.Binarization)
            {
                thresholdBar.Maximum = Byte.MaxValue;
                thresholdBar.Minimum = Byte.MinValue;
                Binarization();
            }
            else if (nowProcess == Process.MedianFilter)
            {
                MedianFilter();
            }
        }

        private void GrayScale()
        {
            thresholdBar.Enabled = false;
            Result_frame = new Image<Bgr, byte>(Source_frame.Size);
            //MessageBox.Show(source.Size.Height.ToString());
            for (int y = 0; y < Source_frame.Height; y++)
            {
                for (int x = 0; x < Source_frame.Width; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    Result_frame.Data[y, x, 0] = grayPixel;
                    Result_frame.Data[y, x, 1] = grayPixel;
                    Result_frame.Data[y, x, 2] = grayPixel;
                }
            }
            OutputPictureBox.Image = Result_frame.ToBitmap();
            //SetFrameToBox(Result_frame, OutputPictureBox);
        }

        private void Mirror()
        {
            thresholdBar.Enabled = false;
            Result_frame = new Image<Bgr, byte>(Source_frame.Size);
            for (int y = 0; y < Source_frame.Height; y++)
            {
                for (int x = 0; x < Source_frame.Width; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    Result_frame.Data[y, Source_frame.Width - x - 1, 0] = pixelB;
                    Result_frame.Data[y, Source_frame.Width - x - 1, 1] = pixelG;
                    Result_frame.Data[y, Source_frame.Width - x - 1, 2] = pixelR;
                }
            }
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }

        private void Negtive()
        {
            thresholdBar.Enabled = false;
            Result_frame = new Image<Bgr, byte>(Source_frame.Size);
            for (int y = 0; y < Source_frame.Height; y++)
            {
                for (int x = 0; x < Source_frame.Width; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    Result_frame.Data[y, x, 0] = (byte)(Byte.MaxValue - pixelB);
                    Result_frame.Data[y, x, 1] = (byte)(Byte.MaxValue - pixelG);
                    Result_frame.Data[y, x, 2] = (byte)(Byte.MaxValue - pixelR);
                }
            }
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }

        private void Binarization() //二值化
        {
            Result_frame = new Image<Bgr, byte>(Source_frame.Size);
            for (int y = 0; y < Source_frame.Height; y++)
            {
                for (int x = 0; x < Source_frame.Width; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    if (grayPixel >= thresholdBar.Value)
                    {
                        Result_frame.Data[y, x, 0] = Byte.MaxValue;
                        Result_frame.Data[y, x, 1] = Byte.MaxValue;
                        Result_frame.Data[y, x, 2] = Byte.MaxValue;
                    }
                    else
                    {
                        Result_frame.Data[y, x, 0] = Byte.MinValue;
                        Result_frame.Data[y, x, 1] = Byte.MinValue;
                        Result_frame.Data[y, x, 2] = Byte.MinValue;
                    }
                }
            }
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }

        private void Relief()
        {
            thresholdBar.Enabled = false;
            Result_frame = new Image<Bgr, byte>(Source_frame.Size);
            for (int y = 0; y < Source_frame.Height; y++)
            {
                for (int x = 0; x < Source_frame.Width - 1; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    byte pixelB2 = Source_frame.Data[y, x + 1, 0];
                    byte pixelG2 = Source_frame.Data[y, x + 1, 1];
                    byte pixelR2 = Source_frame.Data[y, x + 1, 2];
                    byte grayPixel2 = (byte)((pixelB2 + pixelG2 + pixelR2) / 3);

                    int direction = (grayPixel - grayPixel2) > 0 ? 1 : -1;
                    int difference = Math.Abs(grayPixel - grayPixel2);
                    byte Result = (byte)(direction * Math.Sqrt(difference / 2) * Math.Sqrt(127) + 127);

                    Result_frame.Data[y, x, 0] = Result;
                    Result_frame.Data[y, x, 1] = Result;
                    Result_frame.Data[y, x, 2] = Result;
                }
            }
            for (int y = 0; y < Source_frame.Height; y++)
            {
                Result_frame.Data[y, Source_frame.Width - 1, 0] = Result_frame.Data[y, Source_frame.Width - 2, 0];
                Result_frame.Data[y, Source_frame.Width - 1, 1] = Result_frame.Data[y, Source_frame.Width - 2, 0];
                Result_frame.Data[y, Source_frame.Width - 1, 2] = Result_frame.Data[y, Source_frame.Width - 2, 0];
            }
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }

        private void MedianFilter() //中值濾波
        {
            Result_frame = new Image<Bgr, byte>(Source_frame.Size);
            int height = Source_frame.Height;
            int width = Source_frame.Width;
            int MaskSize = thresholdBar.Value;
            int squrtMaskSize = MaskSize * MaskSize;
            for (int y = 0; y < height - MaskSize + 1; y++)
                for (int x = 0; x < width - MaskSize + 1; x++)
                {
                    int[] arrayB = new int[squrtMaskSize];
                    int[] arrayG = new int[squrtMaskSize];
                    int[] arrayR = new int[squrtMaskSize];
                    int k = 0;
                    for (int x2 = 0; x2 < MaskSize; x2++)
                    {
                        for (int y2 = 0; y2 < MaskSize; y2++)
                        {
                            arrayB[k] = Source_frame.Data[y + y2, x + x2, 0];
                            arrayG[k] = Source_frame.Data[y + y2, x + x2, 1];
                            arrayR[k] = Source_frame.Data[y + y2, x + x2, 2];
                            k++;
                        }
                    }
                    Array.Sort(arrayB);
                    Array.Sort(arrayG);
                    Array.Sort(arrayR);
                    if (MaskSize % 2 == 1)
                    {
                        Result_frame.Data[y, x, 0] = (byte)arrayB[squrtMaskSize / 2 + 1];
                        Result_frame.Data[y, x, 1] = (byte)arrayG[squrtMaskSize / 2 + 1];
                        Result_frame.Data[y, x, 2] = (byte)arrayR[squrtMaskSize / 2 + 1];
                    }
                    else
                    {
                        Result_frame.Data[y, x, 0] = (byte)((arrayB[squrtMaskSize / 2] + arrayB[squrtMaskSize / 2 + 1]) / 2);
                        Result_frame.Data[y, x, 1] = (byte)((arrayG[squrtMaskSize / 2] + arrayG[squrtMaskSize / 2 + 1]) / 2);
                        Result_frame.Data[y, x, 2] = (byte)((arrayR[squrtMaskSize / 2] + arrayR[squrtMaskSize / 2 + 1]) / 2);
                    }
                }
            for (int y = 0; y < height - MaskSize + 1; y++)
                for (int x = width - MaskSize + 1; x < width; x++)
                {
                    Result_frame.Data[y, x, 0] = Result_frame.Data[y, x - MaskSize + 1, 0];
                    Result_frame.Data[y, x, 1] = Result_frame.Data[y, x - MaskSize + 1, 1];
                    Result_frame.Data[y, x, 2] = Result_frame.Data[y, x - MaskSize + 1, 2];
                }
            for (int y = height - MaskSize + 1; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Result_frame.Data[y, x, 0] = Result_frame.Data[y - MaskSize + 1, x, 0];
                    Result_frame.Data[y, x, 1] = Result_frame.Data[y - MaskSize + 1, x, 1];
                    Result_frame.Data[y, x, 2] = Result_frame.Data[y - MaskSize + 1, x, 2];
                }
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }

        private void FaceDetection()
        {
            CascadeClassifier cascadeClassifier = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_default.xml");//路徑不知對不對
            if (Source_frame != null)
            {
                var grayframe = Source_frame.Convert<Gray, byte>();
                var faces = cascadeClassifier.DetectMultiScale(grayframe, 1.1, 3, Size.Empty, Size.Empty); //the actual face detection happens here
                foreach (var face in faces)
                {
                    Source_frame.Draw(face, new Bgr(Color.Red), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them
                }
            }
            OutputPictureBox.Image = Source_frame.ToBitmap();
        }

        private void camShiftButton_Click(object sender, EventArgs e)
        {
            //追蹤物體
            nowLiveprocess = LiveProcess.CamShift;
        }

        //以下是在picturebox中畫框框
        private bool drawing = false;
        private Rectangle tempRect = new Rectangle();

        private void SourcePictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (Source_frame != null && nowLiveprocess == LiveProcess.CamShift)
            {
                sourceRect = Rectangle.Empty;
                tempRect = Rectangle.Empty;
                _pressX = e.X;
                _pressY = e.Y;
                drawing = true;
                _xScale = (double)Source_frame.Width / SourcePictureBox.Width;
                _yScale = (double)Source_frame.Height / SourcePictureBox.Height;
            }
        }

        private void SourcePictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(drawing && Source_frame != null && nowLiveprocess == LiveProcess.CamShift)
            {
                Image<Bgr, Byte> frame = Source_frame.Clone();
                tempRect = new Rectangle((int)(_pressX * _xScale), (int)(_pressY * _yScale), (int)((e.X - _pressX) * _xScale), (int)((e.Y - _pressY) * _yScale));
                frame.Draw(tempRect, new Bgr(Color.Red), 2);
                SourcePictureBox.Image = frame.ToBitmap();
            }
        }

        private void SourcePictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing && nowLiveprocess == LiveProcess.CamShift)
            {
                drawing = false;
                sourceRect = tempRect;
                //do tracking
                ObjectTracking(Source_frame, sourceRect);
            }
        }

        public Image<Hsv, byte> hsv;
        public Image<Gray, byte> hue;
        public Image<Gray, byte> mask;
        public Image<Gray, byte> backproject;
        public DenseHistogram hist;
        //private Rectangle trackingWindow;
        private MCvConnectedComp trackcomp;
        private MCvBox2D trackbox;
        private void ObjectTracking(Image<Bgr, Byte> image, Rectangle ROI)
        {
            OutputPictureBox.Image = image.ToBitmap();
            if (ROI.Size != Size.Empty)
            {
                // Initialize parameters
                trackbox = new MCvBox2D();
                trackcomp = new MCvConnectedComp();
                hue = new Image<Gray, byte>(image.Width, image.Height);
                hue._EqualizeHist();
                mask = new Image<Gray, byte>(image.Width, image.Height);
                hist = new DenseHistogram(30, new RangeF(0, 180));
                backproject = new Image<Gray, byte>(image.Width, image.Height);

                // Assign Object's ROI from source image.
                sourceRect = ROI;

                //UpdateHue(image);
                // Producing Object's hist
                CalObjectHist(image);

                Result_frame = Source_frame.Clone();
                Rectangle resultRectanle = TrackingResult(Result_frame);  //更改Result_frame為output影像
                Result_frame.Draw(resultRectanle, new Bgr(Color.Red), 2);
                OutputPictureBox.Image = Result_frame.ToBitmap();
            }
        }

        private void KeepTracking()
        {
            if (sourceRect.Size != Size.Empty)
            {
                Result_frame = Source_frame.Clone();
                Rectangle resultRectanle = TrackingResult(Result_frame);  //更改Result_frame為output影像
                Result_frame.Draw(resultRectanle, new Bgr(Color.Red), 2);
                OutputPictureBox.Image = Result_frame.ToBitmap();
            }
            else
            {
                OutputPictureBox.Image = Source_frame.ToBitmap();
            }
        }

        private void UpdateHue(Image<Bgr, Byte> image)
        {
            // release previous image memory
            if (hsv != null) hsv.Dispose();
            hsv = image.Convert<Hsv, Byte>();

            // Drop low saturation pixels
            mask = hsv.Split()[1].ThresholdBinary(new Gray(60), new Gray(255));
            CvInvoke.cvInRangeS(hsv, new MCvScalar(0, 30, Math.Min(10, 255), 0),
                new MCvScalar(180, 256, Math.Max(10, 255), 0), mask);

            // Get Hue
            hue = hsv.Split()[0];
        }

        private void CalObjectHist(Image<Bgr, Byte> image)
        {
            UpdateHue(image);

            // Set tracking object's ROI
            hue.ROI = sourceRect;
            mask.ROI = sourceRect;
            hist.Calculate(new Image<Gray, byte>[] { hue }, false, mask);

            // Scale Historgram
            float max = 0, min = 0, scale = 0;
            int[] minLocations, maxLocations;
            hist.MinMax(out min, out max, out minLocations, out maxLocations);
            if (max != 0)
            {
                scale = 255 / max;
            }
            CvInvoke.cvConvertScale(hist.MCvHistogram.bins, hist.MCvHistogram.bins, scale, 0);

            // Clear ROI
            hue.ROI = Rectangle.Empty;
            mask.ROI = Rectangle.Empty;

            // Now we have Object's Histogram, called hist.
        }
        public Rectangle TrackingResult(Image<Bgr, Byte> image)
        {
            UpdateHue(image);
            Rectangle resultRect;
            // Calucate BackProject
            backproject = hist.BackProject(new Image<Gray, byte>[] { hue });

            // Apply mask
            backproject._And(mask);

            // Tracking windows empty means camshift lost bounding-box last time
            // here we give camshift a new start window from 0,0 (you could change it)
            if (sourceRect.IsEmpty || sourceRect.Width == 0 || sourceRect.Height == 0)
            {
                resultRect = new Rectangle(0, 0, 100, 100);
            }
            CvInvoke.cvCamShift(backproject, sourceRect,
                new MCvTermCriteria(10, 1), out trackcomp, out trackbox);

            // update tracking window
            resultRect = trackcomp.rect;

            return resultRect;
        }

    }
}