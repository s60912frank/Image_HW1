using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;


namespace Image
{
    public partial class Form1 : Form
    {
        private Image<Bgr, Byte> Source_frame = null;   //呈現Load圖片的畫面
        private Image<Bgr, Byte> Temp_Frame;  //複製Source_frame數據到Temp_Frame，方便進行影像處理
        private Image<Gray, Byte> grayImage = null; //new灰階畫面
        private Image<Bgr, Byte> Result_frame = null;  //最後呈現的畫面(RGB專用)
       
        private int width;  //圖片的寬度
        private int height; //圖片的長度
      
   
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
                width = Source_frame.Width;   //給予寬的數值
                height = Source_frame.Height; //給予高的數值
                Temp_Frame = new Image<Bgr, Byte>(width, height, new Bgr(0, 0, 0));  //新增一個RGB Image 用來複製Source_frame的資訊
                Result_frame = new Image<Bgr, Byte>(width, height, new Bgr(0, 0, 0));   //新增一個RGB Image ，存結果圖片
                grayImage = new Image<Gray, byte>(width, height, new Gray(0));   //新增一個Gray Image ，存灰階圖片
                
                Temp_Frame = Source_frame.Clone();  //將Source_frame圖片資訊複製給Temp_Frame
                SourcePictureBox.Image = Source_frame.ToBitmap();   //將Source_frame轉成Bitmap格式呈現到PictureBox
                saveButton.Enabled = grayButton.Enabled = reliefButton.Enabled = negativeButton.Enabled = mirrorButton.Enabled = binarizationButton.Enabled = MedianButton.Enabled = buttonCamshift.Enabled = true;
                ReSizePictureBox();
            }
        }

        //讀鳥圖.
        private void load_bird_Button_Click(object sender, EventArgs e)
        {
            Source_frame = new Image<Bgr, byte>("..\\..\\src\\IMG_2059.jpg");     //將圖片資訊存到Source_frame
            width = Source_frame.Width;   //給予寬的數值
            height = Source_frame.Height; //給予高的數值
            Temp_Frame = new Image<Bgr, Byte>(width, height, new Bgr(0, 0, 0));  //新增一個RGB Image 用來複製Source_frame的資訊
            Result_frame = new Image<Bgr, Byte>(width, height, new Bgr(0, 0, 0));   //新增一個RGB Image ，存結果圖片
            grayImage = new Image<Gray, byte>(width, height, new Gray(0));   //新增一個Gray Image ，存灰階圖片

            Temp_Frame = Source_frame.Clone();  //將Source_frame圖片資訊複製給Temp_Frame
            SourcePictureBox.Image = Source_frame.ToBitmap();   //將Source_frame轉成Bitmap格式呈現到PictureBox
            saveButton.Enabled = grayButton.Enabled = reliefButton.Enabled = negativeButton.Enabled = mirrorButton.Enabled = binarizationButton.Enabled = MedianButton.Enabled = buttonCamshift.Enabled = true;
            ReSizePictureBox();
        }

        //讀狗圖.
        private void load_dog_Button_Click(object sender, EventArgs e)
        {
            Source_frame = new Image<Bgr, byte>("..\\..\\src\\paper.jpg");     //將圖片資訊存到Source_frame
            width = Source_frame.Width;   //給予寬的數值
            height = Source_frame.Height; //給予高的數值
            Temp_Frame = new Image<Bgr, Byte>(width, height, new Bgr(0, 0, 0));  //新增一個RGB Image 用來複製Source_frame的資訊
            Result_frame = new Image<Bgr, Byte>(width, height, new Bgr(0, 0, 0));   //新增一個RGB Image ，存結果圖片
            grayImage = new Image<Gray, byte>(width, height, new Gray(0));   //新增一個Gray Image ，存灰階圖片

            Temp_Frame = Source_frame.Clone();  //將Source_frame圖片資訊複製給Temp_Frame
            SourcePictureBox.Image = Source_frame.ToBitmap();   //將Source_frame轉成Bitmap格式呈現到PictureBox
            saveButton.Enabled = grayButton.Enabled = reliefButton.Enabled = negativeButton.Enabled = mirrorButton.Enabled = binarizationButton.Enabled = MedianButton.Enabled = buttonCamshift.Enabled = true;
            ReSizePictureBox();
        }

        //灰階
        private void grayButton_Click(object sender, EventArgs e)  
        {
            for (int y = 0; y < height; y++)            
                for (int x = 0; x < width; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    Result_frame.Data[y, x, 0] = grayPixel;
                    Result_frame.Data[y, x, 1] = grayPixel;
                    Result_frame.Data[y, x, 2] = grayPixel;
                }            
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }
  
     

        //二值化
        private void binarizationButton_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < height; y++)            
                for (int x = 0; x < width; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    if (grayPixel >= 127)
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
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }



        //負片
        private void negativeButton_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < height; y++)            
                for (int x = 0; x < width; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    Result_frame.Data[y, x, 0] = (byte)(Byte.MaxValue - pixelB);
                    Result_frame.Data[y, x, 1] = (byte)(Byte.MaxValue - pixelG);
                    Result_frame.Data[y, x, 2] = (byte)(Byte.MaxValue - pixelR);
                }
            
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }
     
        //鏡射
        private void mirrorButton_Click(object sender, EventArgs e)
        {            
            for (int y = 0; y < height; y++)            
                for (int x = 0; x < width; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    Result_frame.Data[y, width - x - 1, 0] = pixelB;
                    Result_frame.Data[y, width - x - 1, 1] = pixelG;
                    Result_frame.Data[y, width - x - 1, 2] = pixelR;
                }
            
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }
   

        /// 浮雕
        private void reliefButton_Click(object sender, EventArgs e) 
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width-1; x++)
                {
                    byte pixelB = Source_frame.Data[y, x, 0];
                    byte pixelG = Source_frame.Data[y, x, 1];
                    byte pixelR = Source_frame.Data[y, x, 2];
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    byte pixelB2 = Source_frame.Data[y, x+1, 0];
                    byte pixelG2 = Source_frame.Data[y, x+1, 1];
                    byte pixelR2 = Source_frame.Data[y, x+1, 2];
                    byte grayPixel2 = (byte)((pixelB2 + pixelG2 + pixelR2) / 3);

                    int direction = (grayPixel - grayPixel2) > 0 ? 1 : -1;
                    int difference = Math.Abs(grayPixel - grayPixel2);
                    byte Result = (byte)(direction*Math.Sqrt(difference / 2) * Math.Sqrt(127) + 127);
                    Result = (byte)(direction*difference+ 127);
                    Result_frame.Data[y, x, 0] = Result;
                    Result_frame.Data[y, x, 1] = Result;
                    Result_frame.Data[y, x, 2] = Result;                    
                }
            }
            for (int y = 0; y < height; y++)
            {
                Result_frame.Data[y, width - 1, 0] = Result_frame.Data[y, width - 2, 0];
                Result_frame.Data[y, width - 1, 1] = Result_frame.Data[y, width - 2, 0];
                Result_frame.Data[y, width - 1, 2] = Result_frame.Data[y, width - 2, 0];
            }
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }              

        //中值濾波
        private void MedianButton_Click(object sender, EventArgs e)
        {            
            const int MASK_SIZE = 5;

            for (int y = 0; y < height - MASK_SIZE + 1; y++)
                for (int x = 0; x < width - MASK_SIZE + 1; x++)
                {
                    int k = 0;                    
                    int[] arrayR = new int[MASK_SIZE * MASK_SIZE];
                    int[] arrayG = new int[MASK_SIZE * MASK_SIZE];
                    int[] arrayB = new int[MASK_SIZE * MASK_SIZE];
                    for (int x2 = 0; x2 < MASK_SIZE; x2++)
                        for (int y2 = 0; y2 < MASK_SIZE; y2++)
                        {
                            arrayR[k] = Source_frame.Data[y + y2, x + x2, 0];
                            arrayG[k] = Source_frame.Data[y + y2, x + x2, 1];
                            arrayB[k++] = Source_frame.Data[y + y2, x + x2, 2];
                        }     
                    Array.Sort(arrayR);
                    Array.Sort(arrayG);
                    Array.Sort(arrayB);
                    Result_frame.Data[y, x, 0] = (byte)arrayR[MASK_SIZE * MASK_SIZE / 2];
                    Result_frame.Data[y, x, 1] = (byte)arrayG[MASK_SIZE * MASK_SIZE / 2];
                    Result_frame.Data[y, x, 2] = (byte)arrayB[MASK_SIZE * MASK_SIZE / 2];
                }
            for (int y = 0; y < height - MASK_SIZE + 1; y++)
                for (int x = width - MASK_SIZE + 1; x < width; x++)
                {
                    Result_frame.Data[y, x, 0] = Result_frame.Data[y, x - MASK_SIZE + 1, 0];
                    Result_frame.Data[y, x, 1] = Result_frame.Data[y, x - MASK_SIZE + 1, 1];
                    Result_frame.Data[y, x, 2] = Result_frame.Data[y, x - MASK_SIZE + 1, 2];
                }
            for (int y = height - MASK_SIZE + 1; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Result_frame.Data[y, x, 0] = Result_frame.Data[y - MASK_SIZE + 1, x, 0];
                    Result_frame.Data[y, x, 1] = Result_frame.Data[y - MASK_SIZE + 1, x, 1];
                    Result_frame.Data[y, x, 2] = Result_frame.Data[y - MASK_SIZE + 1, x, 2];
                }        
            OutputPictureBox.Image = Result_frame.ToBitmap();
        }
        
        //結束主程式
        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(Environment.ExitCode);
            InitializeComponent();
        }

        //儲存圖片
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (OutputPictureBox.Image != null)   /////圖片會存到 Image\Image\bin 裡
                OutputPictureBox.Image.Save("../" + Temp_Frame.ToBitmap() + ".jpeg", ImageFormat.Jpeg);
        }


        // new new new new new new new new new new new new new new new new new new new new new new new new new new new new new new new
        // new new new new new new new new new new new new new new new new new new new new new new new new new new new new new new new
        // new new new new new new new new new new new new new new new new new new new new new new new new new new new new new new new
        Rectangle _sourceRectangle;
        int _pressX;
        int _pressY;
        bool _isPress = false;
        //bool _isCamshiftMode = false;
        double _xScale;
        double _yScale;

        private void buttonCamshift_Click(object sender, EventArgs e)
        {
            
        }
        //調整PictureBox為Image大小.
        private void ReSizePictureBox()
        {
            if (Source_frame != null)
            {
                _yScale = (double)Source_frame.Height / SourcePictureBox.Height;
                SourcePictureBox.Size = new System.Drawing.Size((int)(Source_frame.Width / _yScale), SourcePictureBox.Height);
                OutputPictureBox.Image = Source_frame.ToBitmap();
            }
        }

        private void SourcePictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (Source_frame != null)
            {
                _pressX = e.X;
                _pressY = e.Y;
                _isPress = true;
                _xScale = (double)Source_frame.Width / SourcePictureBox.Width;
                _yScale = (double)Source_frame.Height / SourcePictureBox.Height;
            }
        }

        private void SourcePictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isPress)
                if (Source_frame != null)
                {
                    Image<Bgr, Byte> frame = Source_frame.Clone();
                    _sourceRectangle = new Rectangle((int)(_pressX * _xScale), (int)(_pressY * _xScale), (int)((e.X - _pressX) * _yScale), (int)((e.Y - _pressY) * _yScale));
                    frame.Draw(_sourceRectangle, new Bgr(Color.Red), 2);
                    SourcePictureBox.Image = frame.ToBitmap();                    
                }
        }

        // 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 
        // 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 
        // 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 觸發追蹤再這. 
        private void SourcePictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (Source_frame != null)
            {
                Image<Bgr, Byte> frame = Source_frame.Clone();
                _sourceRectangle = new Rectangle((int)(_pressX * _xScale), (int)(_pressY * _xScale), (int)((e.X - _pressX) * _yScale), (int)((e.Y - _pressY) * _yScale));
                frame.Draw(_sourceRectangle, new Bgr(Color.Red), 2);
                SourcePictureBox.Image = frame.ToBitmap();


                // Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here
                // Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here
                // Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here  Edit Here

                ObjectTracking(Source_frame, _sourceRectangle);  //更改Source_frame為input影像
                Result_frame = new Image<Bgr, byte>(Source_frame.ToBitmap());

                Rectangle resultRectanle = Tracking(Result_frame);  //更改Result_frame為output影像
                Result_frame.Draw(resultRectanle, new Bgr(Color.Red), 2);
                OutputPictureBox.Image = Result_frame.ToBitmap();
            }
            _isPress = false;
        }

        // Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift 
        // Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift Camshift 

        public Image<Hsv, byte> hsv;
        public Image<Gray, byte> hue;
        public Image<Gray, byte> mask;
        public Image<Gray, byte> backproject;
        public DenseHistogram hist;
        private Rectangle trackingWindow;
        private MCvConnectedComp trackcomp;
        private MCvBox2D trackbox;
        private void ObjectTracking(Image<Bgr, Byte> image, Rectangle ROI)
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
            trackingWindow = ROI;

            // Producing Object's hist
            CalObjectHist(image);
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
            hue.ROI = trackingWindow;
            mask.ROI = trackingWindow;
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
            hue.ROI = System.Drawing.Rectangle.Empty;
            mask.ROI = System.Drawing.Rectangle.Empty;

            // Now we have Object's Histogram, called hist.
        }
        public Rectangle Tracking(Image<Bgr, Byte> image)
        {
            UpdateHue(image);

            // Calucate BackProject
            backproject = hist.BackProject(new Image<Gray, byte>[] { hue });

            // Apply mask
            backproject._And(mask);

            // Tracking windows empty means camshift lost bounding-box last time
            // here we give camshift a new start window from 0,0 (you could change it)
            if (trackingWindow.IsEmpty || trackingWindow.Width == 0 || trackingWindow.Height == 0)
            {
                trackingWindow = new Rectangle(0, 0, 100, 100);
            }
            CvInvoke.cvCamShift(backproject, trackingWindow,
                new MCvTermCriteria(10, 1), out trackcomp, out trackbox);

            // update tracking window
            trackingWindow = trackcomp.rect;

            return trackingWindow;
        }
    }
}
