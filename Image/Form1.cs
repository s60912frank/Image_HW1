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
        private Capture webCam = null;//擷取攝影機影像
        private CascadeClassifier _cascadeClassifier;//人臉辨識用
       
        private int width;  //圖片的寬度
        private int height; //圖片的長度

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
            Mirror
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
                string test = openFileDialog.FileName;
                Source_frame = new Image<Bgr, byte>(openFileDialog.FileName);     //將圖片資訊存到Source_frame
                width = Source_frame.Width;   //給予寬的數值
                height = Source_frame.Height; //給予高的數值
                Temp_Frame = new Image<Bgr, Byte>(width, height, new Bgr(0, 0, 0));  //新增一個RGB Image 用來複製Source_frame的資訊
                Result_frame = new Image<Bgr, Byte>(width, height, new Bgr(0, 0, 0));   //新增一個RGB Image ，存結果圖片
                grayImage = new Image<Gray, byte>(width, height, new Gray(0));   //新增一個Gray Image ，存灰階圖片
                
                Temp_Frame = Source_frame.Clone();  //將Source_frame圖片資訊複製給Temp_Frame
                SourcePictureBox.Image = Source_frame.ToBitmap();   //將Source_frame轉成Bitmap格式呈現到PictureBox
                saveButton.Enabled=grayButton.Enabled = reliefButton.Enabled = negativeButton.Enabled = mirrorButton.Enabled = binarizationButton.Enabled = MedianButton.Enabled = true;
            }
        }

        //灰階
        private void grayButton_Click(object sender, EventArgs e)  
        {
            nowLiveprocess = LiveProcess.GrayScale;
            //還要做一下讀圖的灰階
        }

        private void GrayScale(Image<Bgr, Byte> source)
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
                    byte grayPixel = (byte)((pixelB + pixelG + pixelR) / 3);
                    result.Data[y, x, 0] = grayPixel;
                    result.Data[y, x, 1] = grayPixel;
                    result.Data[y, x, 2] = grayPixel;
                }
            }
            OutputPictureBox.Image = result.ToBitmap();
        }

        //二值化
        private void binarizationButton_Click(object sender, EventArgs e)
        {
            thresholdBar.Enabled = true;
            nowProcess = Process.Binarization;
            nowLiveprocess = LiveProcess.Binarization;
            thresholdBar.Maximum = Byte.MaxValue;
            thresholdBar.Minimum = Byte.MinValue;
            thresholdBar.Value = 127;
            Binarization(new Image<Bgr, Byte>((Bitmap)SourcePictureBox.Image));
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

        //負片
        private void negativeButton_Click(object sender, EventArgs e)
        {
            nowLiveprocess = LiveProcess.Negtive;
        }
     
        //鏡射
        private void mirrorButton_Click(object sender, EventArgs e)
        {
            nowLiveprocess = LiveProcess.Mirror;
        }

        private void Mirror(Image<Bgr, Byte> source)
        {
            //會盪掉不知為何
            thresholdBar.Enabled = false;
            Image<Bgr, Byte> result = new Image<Bgr, byte>(source.Size);
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    byte pixelB = source.Data[y, x, 0];
                    byte pixelG = source.Data[y, x, 1];
                    byte pixelR = source.Data[y, x, 2];
                    result.Data[y, width - x - 1, 0] = pixelB;
                    result.Data[y, width - x - 1, 1] = pixelG;
                    result.Data[y, width - x - 1, 2] = pixelR;
                }
            }
            OutputPictureBox.Image = result.ToBitmap();
        }
   

        // 浮雕
        private void reliefButton_Click(object sender, EventArgs e) 
        {
            thresholdBar.Enabled = false;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width - 1; x++)
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
            InitializeComponent();
        }

        //儲存圖片
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (OutputPictureBox.Image != null)   /////圖片會存到 Image\Image\bin 裡
                OutputPictureBox.Image.Save("../" + Temp_Frame.ToBitmap() + ".jpeg", ImageFormat.Jpeg);
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
                MedianFilter();
            }
        }

        private void Binarization(Image<Bgr,Byte> source) //二值化
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(source.Size);
            for (int y = 0; y < source.Height /2; y++)
            {
                for (int x = 0; x < source.Width / 2; x++)
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

        private void MedianFilter() //中值濾波
        {
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

        private void webButton_Click(object sender, EventArgs e)
        {
            webCam = new Capture();
            Application.Idle += new EventHandler(Application_Idle);
            saveButton.Enabled = grayButton.Enabled = reliefButton.Enabled = negativeButton.Enabled = mirrorButton.Enabled = binarizationButton.Enabled = MedianButton.Enabled = true;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            Image<Bgr, Byte> camImage = webCam.QueryFrame().ToImage<Bgr, Byte>();
            SourcePictureBox.Image = camImage.ToBitmap();
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
            }
        }

        private void faceButton_Click(object sender, EventArgs e)//因為我系統找不到Camera, 無法實測功能是否正確
        {
            nowLiveprocess = LiveProcess.FaceDetection;
        }

        private void FaceDetection(Image<Bgr,Byte> source)
        {
            _cascadeClassifier = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_default.xml");//路徑不知對不對
            if (source != null)
            {
                var grayframe = source.Convert<Gray, byte>();
                var faces = _cascadeClassifier.DetectMultiScale(grayframe); //the actual face detection happens here
                foreach (var face in faces)
                {
                    source.Draw(face, new Bgr(Color.Red), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them
                }
            }
            OutputPictureBox.Image = source.ToBitmap();
        }
    }
}
