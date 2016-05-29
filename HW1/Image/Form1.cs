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

        //FOR CAMSHIFT
        private Rectangle sourceRect = new Rectangle();

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
                Image<Bgr, byte> Source_frame = new Image<Bgr, byte>(openFileDialog.FileName);     //將圖片資訊存到Source_frame
                //SourcePictureBox.Image = Source_frame.ToBitmap();   //將Source_frame轉成Bitmap格式呈現到PictureBox
                SetFrameToBox(Source_frame, SourcePictureBox);
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

        private void SetFrameToBox(Image<Bgr,Byte> source, PictureBox box)
        {
            int fixedWidth = box.Width;
            double ratio = (double)source.Height / (double)source.Width;
            source.Resize(fixedWidth, (int)(fixedWidth * ratio), Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            box.Size = new Size(fixedWidth, (int)(fixedWidth * ratio));
            box.Image = source.ToBitmap();
        }

        void Application_Idle(object sender, EventArgs e)
        {
            Image<Bgr, Byte> camImage = webCam.QueryFrame().Convert<Bgr, byte>();
            
            //SourcePictureBox.Image = camImage.ToBitmap();
            SetFrameToBox(camImage, SourcePictureBox);
            switch (nowLiveprocess)
            {
                case LiveProcess.FaceDetection:
                    FaceDetection(camImage);
                    break;
                case LiveProcess.Binarization:
                    Binarization(camImage);
                    break;
                case LiveProcess.GrayScale:
                    GrayScale(camImage);
                    break;
                case LiveProcess.Negtive:
                    Negtive(camImage);
                    break;
                case LiveProcess.Mirror:
                    Mirror(camImage);
                    break;
                case LiveProcess.Relief:
                    Relief(camImage);
                    break;
                case LiveProcess.CamShift:
                    //camShiftSource = camImage;
                    //ObjectTracking(camImage, sourceRect);
                    ObjectTracking(new Image<Bgr, byte>((Bitmap)SourcePictureBox.Image), sourceRect);
                    //camShiftSource = camImage;
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
                GrayScale(new Image<Bgr, Byte>((Bitmap)SourcePictureBox.Image));
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
                Binarization(new Image<Bgr, Byte>((Bitmap)SourcePictureBox.Image));
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
                Negtive(new Image<Bgr, Byte>((Bitmap)SourcePictureBox.Image));
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
                Mirror(new Image<Bgr, Byte>((Bitmap)SourcePictureBox.Image));
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
                Relief(new Image<Bgr, byte>((Bitmap)SourcePictureBox.Image));
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
            MedianFilter(new Image<Bgr, Byte>((Bitmap)SourcePictureBox.Image));
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
                Binarization(new Image<Bgr, Byte>((Bitmap)SourcePictureBox.Image));
            }
            else if (nowProcess == Process.MedianFilter)
            {
                MedianFilter(new Image<Bgr, Byte>((Bitmap)SourcePictureBox.Image));
            }
        }

        private void GrayScale(Image<Bgr, Byte> source)
        {
            thresholdBar.Enabled = false;
            Image<Bgr, Byte> result = new Image<Bgr, byte>(source.Size);
            //MessageBox.Show(source.Size.Height.ToString());
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    if(source.Data == null)
                    {
                        //source.
                        MessageBox.Show(source.Size.ToString());
                    }
                    byte pixelB = source.Data[y, x, 0];
                    byte pixelG = source.Data[y, x, 1];
                    byte pixelR = source.Data[y, x, 2];
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    result.Data[y, x, 0] = grayPixel;
                    result.Data[y, x, 1] = grayPixel;
                    result.Data[y, x, 2] = grayPixel;
                }
            }
            //OutputPictureBox.Image = result.ToBitmap();
            SetFrameToBox(result, OutputPictureBox);
        }

        private void Mirror(Image<Bgr, Byte> source)
        {
            thresholdBar.Enabled = false;
            Image<Bgr, Byte> result = new Image<Bgr, byte>(source.Size);
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    byte pixelB = source.Data[y, x, 0];
                    byte pixelG = source.Data[y, x, 1];
                    byte pixelR = source.Data[y, x, 2];
                    result.Data[y, source.Width - x - 1, 0] = pixelB;
                    result.Data[y, source.Width - x - 1, 1] = pixelG;
                    result.Data[y, source.Width - x - 1, 2] = pixelR;
                }
            }
            OutputPictureBox.Image = result.ToBitmap();
        }

        private void Negtive(Image<Bgr, byte> source)
        {
            thresholdBar.Enabled = false;
            Image<Bgr, Byte> result = new Image<Bgr, byte>(source.Size);
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    byte pixelB = source.Data[y, x, 0];
                    byte pixelG = source.Data[y, x, 1];
                    byte pixelR = source.Data[y, x, 2];
                    result.Data[y, x, 0] = (byte)(Byte.MaxValue - pixelB);
                    result.Data[y, x, 1] = (byte)(Byte.MaxValue - pixelG);
                    result.Data[y, x, 2] = (byte)(Byte.MaxValue - pixelR);
                }
            }
            OutputPictureBox.Image = result.ToBitmap();
        }

        private void Binarization(Image<Bgr,Byte> source) //二值化
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(source.Size);
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    byte pixelB = source.Data[y, x, 0];
                    byte pixelG = source.Data[y, x, 1];
                    byte pixelR = source.Data[y, x, 2];
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    if (grayPixel >= thresholdBar.Value)
                    {
                        result.Data[y, x, 0] = Byte.MaxValue;
                        result.Data[y, x, 1] = Byte.MaxValue;
                        result.Data[y, x, 2] = Byte.MaxValue;
                    }
                    else
                    {
                        result.Data[y, x, 0] = Byte.MinValue;
                        result.Data[y, x, 1] = Byte.MinValue;
                        result.Data[y, x, 2] = Byte.MinValue;
                    }
                }
            }
            OutputPictureBox.Image = result.ToBitmap();
        }

        private void Relief(Image<Bgr, Byte> source)
        {
            thresholdBar.Enabled = false;
            Image<Bgr, Byte> result = new Image<Bgr, byte>(source.Size);
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width - 1; x++)
                {
                    byte pixelB = source.Data[y, x, 0];
                    byte pixelG = source.Data[y, x, 1];
                    byte pixelR = source.Data[y, x, 2];
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    byte pixelB2 = source.Data[y, x + 1, 0];
                    byte pixelG2 = source.Data[y, x + 1, 1];
                    byte pixelR2 = source.Data[y, x + 1, 2];
                    byte grayPixel2 = (byte)((pixelB2 + pixelG2 + pixelR2) / 3);

                    int direction = (grayPixel - grayPixel2) > 0 ? 1 : -1;
                    int difference = Math.Abs(grayPixel - grayPixel2);
                    byte Result = (byte)(direction * Math.Sqrt(difference / 2) * Math.Sqrt(127) + 127);

                    result.Data[y, x, 0] = Result;
                    result.Data[y, x, 1] = Result;
                    result.Data[y, x, 2] = Result;
                }
            }
            for (int y = 0; y < source.Height; y++)
            {
                result.Data[y, source.Width - 1, 0] = result.Data[y, source.Width - 2, 0];
                result.Data[y, source.Width - 1, 1] = result.Data[y, source.Width - 2, 0];
                result.Data[y, source.Width - 1, 2] = result.Data[y, source.Width - 2, 0];
            }
            OutputPictureBox.Image = result.ToBitmap();
        }

        private void MedianFilter(Image<Bgr, Byte> source) //中值濾波
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(source.Size);
            int height = source.Height;
            int width = source.Width;
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
                            arrayB[k] = source.Data[y + y2, x + x2, 0];
                            arrayG[k] = source.Data[y + y2, x + x2, 1];
                            arrayR[k] = source.Data[y + y2, x + x2, 2];
                            k++;
                        }
                    }
                    Array.Sort(arrayB);
                    Array.Sort(arrayG);
                    Array.Sort(arrayR);
                    if (MaskSize % 2 == 1)
                    {
                        result.Data[y, x, 0] = (byte)arrayB[squrtMaskSize / 2 + 1];
                        result.Data[y, x, 1] = (byte)arrayG[squrtMaskSize / 2 + 1];
                        result.Data[y, x, 2] = (byte)arrayR[squrtMaskSize / 2 + 1];
                    }
                    else
                    {
                        result.Data[y, x, 0] = (byte)((arrayB[squrtMaskSize / 2] + arrayB[squrtMaskSize / 2 + 1]) / 2);
                        result.Data[y, x, 1] = (byte)((arrayG[squrtMaskSize / 2] + arrayG[squrtMaskSize / 2 + 1]) / 2);
                        result.Data[y, x, 2] = (byte)((arrayR[squrtMaskSize / 2] + arrayR[squrtMaskSize / 2 + 1]) / 2);
                    }
                }
            for (int y = 0; y < height - MaskSize + 1; y++)
                for (int x = width - MaskSize + 1; x < width; x++)
                {
                    result.Data[y, x, 0] = result.Data[y, x - MaskSize + 1, 0];
                    result.Data[y, x, 1] = result.Data[y, x - MaskSize + 1, 1];
                    result.Data[y, x, 2] = result.Data[y, x - MaskSize + 1, 2];
                }
            for (int y = height - MaskSize + 1; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    result.Data[y, x, 0] = result.Data[y - MaskSize + 1, x, 0];
                    result.Data[y, x, 1] = result.Data[y - MaskSize + 1, x, 1];
                    result.Data[y, x, 2] = result.Data[y - MaskSize + 1, x, 2];
                }
            OutputPictureBox.Image = result.ToBitmap();
        }

        private void FaceDetection(Image<Bgr,Byte> source)
        {
            CascadeClassifier cascadeClassifier = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_default.xml");//路徑不知對不對
            if (source != null)
            {
                var grayframe = source.Convert<Gray, byte>();
                var faces = cascadeClassifier.DetectMultiScale(grayframe, 1.1, 1, new Size(10, 10), Size.Empty); //the actual face detection happens here
                foreach (var face in faces)
                {
                    source.Draw(face, new Bgr(Color.Red), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them
                }
            }
            OutputPictureBox.Image = source.ToBitmap();
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
            sourceRect = Rectangle.Empty;
            tempRect.Location = e.Location;
            tempRect.Size = new Size(0, 0);
            drawing = true;
        }

        private void SourcePictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                tempRect.Size = new Size(e.Location.X - tempRect.Location.X, e.Location.Y - tempRect.Location.Y);
                //SourcePictureBox.Invalidate();
                TestDraw(new Image<Bgr, byte>((Bitmap)SourcePictureBox.Image));
            }
        }

        private void SourcePictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing && nowLiveprocess == LiveProcess.CamShift)
            {
                drawing = false;
                sourceRect = tempRect;
                //do tracking
                //ObjectTracking(camShiftSource, sourceRect);
            }
        }

        private void TestDraw(Image<Bgr, byte> image)
        {
            image.Draw(tempRect, new Bgr(Color.Red), 2);
            //SourcePictureBox.Image = image.ToBitmap();
            SetFrameToBox(image, SourcePictureBox);
        }

        private void SourcePictureBox_Paint(object sender, PaintEventArgs e)
        {
            //if (tempRect != null && nowLiveprocess == LiveProcess.CamShift)
            //{
            //    e.Graphics.DrawRectangle(Pens.Red, tempRect);
            //}
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
            SetFrameToBox(image, OutputPictureBox);
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

                //OutputPictureBox.Invalidate();
                Image<Bgr, byte> Result_frame = new Image<Bgr, byte>((Bitmap)OutputPictureBox.Image);
                Rectangle resultRectanle = TrackingResult(Result_frame);  //更改Result_frame為output影像
                Result_frame.Draw(resultRectanle, new Bgr(Color.Red), 2);
                //OutputPictureBox.Image = Result_frame.ToBitmap();
                SetFrameToBox(Result_frame, OutputPictureBox);
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
//}
