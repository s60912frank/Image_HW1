namespace Image
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.SourcePictureBox = new System.Windows.Forms.PictureBox();
            this.OutputPictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.grayButton = new System.Windows.Forms.Button();
            this.binarizationButton = new System.Windows.Forms.Button();
            this.negativeButton = new System.Windows.Forms.Button();
            this.mirrorButton = new System.Windows.Forms.Button();
            this.MedianButton = new System.Windows.Forms.Button();
            this.reliefButton = new System.Windows.Forms.Button();
            this.load_Button = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.load_bird_Button = new System.Windows.Forms.Button();
            this.load_dog_Button = new System.Windows.Forms.Button();
            this.buttonCamshift = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SourcePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SourcePictureBox
            // 
            this.SourcePictureBox.Location = new System.Drawing.Point(12, 50);
            this.SourcePictureBox.Name = "SourcePictureBox";
            this.SourcePictureBox.Size = new System.Drawing.Size(434, 300);
            this.SourcePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.SourcePictureBox.TabIndex = 0;
            this.SourcePictureBox.TabStop = false;
            this.SourcePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SourcePictureBox_MouseDown);
            this.SourcePictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SourcePictureBox_MouseMove);
            this.SourcePictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SourcePictureBox_MouseUp);
            // 
            // OutputPictureBox
            // 
            this.OutputPictureBox.Location = new System.Drawing.Point(460, 50);
            this.OutputPictureBox.Name = "OutputPictureBox";
            this.OutputPictureBox.Size = new System.Drawing.Size(434, 300);
            this.OutputPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OutputPictureBox.TabIndex = 2;
            this.OutputPictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(39, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 29);
            this.label1.TabIndex = 3;
            this.label1.Text = "Source";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(486, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Output Image";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // grayButton
            // 
            this.grayButton.Enabled = false;
            this.grayButton.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.grayButton.Location = new System.Drawing.Point(460, 374);
            this.grayButton.Name = "grayButton";
            this.grayButton.Size = new System.Drawing.Size(100, 30);
            this.grayButton.TabIndex = 18;
            this.grayButton.Text = "灰階";
            this.grayButton.UseVisualStyleBackColor = true;
            this.grayButton.Click += new System.EventHandler(this.grayButton_Click);
            // 
            // binarizationButton
            // 
            this.binarizationButton.Enabled = false;
            this.binarizationButton.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.binarizationButton.Location = new System.Drawing.Point(566, 374);
            this.binarizationButton.Name = "binarizationButton";
            this.binarizationButton.Size = new System.Drawing.Size(100, 30);
            this.binarizationButton.TabIndex = 19;
            this.binarizationButton.Text = "二值化";
            this.binarizationButton.UseVisualStyleBackColor = true;
            this.binarizationButton.Click += new System.EventHandler(this.binarizationButton_Click);
            // 
            // negativeButton
            // 
            this.negativeButton.Enabled = false;
            this.negativeButton.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.negativeButton.Location = new System.Drawing.Point(672, 374);
            this.negativeButton.Name = "negativeButton";
            this.negativeButton.Size = new System.Drawing.Size(100, 30);
            this.negativeButton.TabIndex = 20;
            this.negativeButton.Text = "負片";
            this.negativeButton.UseVisualStyleBackColor = true;
            this.negativeButton.Click += new System.EventHandler(this.negativeButton_Click);
            // 
            // mirrorButton
            // 
            this.mirrorButton.Enabled = false;
            this.mirrorButton.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.mirrorButton.Location = new System.Drawing.Point(460, 410);
            this.mirrorButton.Name = "mirrorButton";
            this.mirrorButton.Size = new System.Drawing.Size(100, 30);
            this.mirrorButton.TabIndex = 21;
            this.mirrorButton.Text = "鏡射";
            this.mirrorButton.UseVisualStyleBackColor = true;
            this.mirrorButton.Click += new System.EventHandler(this.mirrorButton_Click);
            // 
            // MedianButton
            // 
            this.MedianButton.Enabled = false;
            this.MedianButton.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.MedianButton.Location = new System.Drawing.Point(672, 410);
            this.MedianButton.Name = "MedianButton";
            this.MedianButton.Size = new System.Drawing.Size(164, 30);
            this.MedianButton.TabIndex = 22;
            this.MedianButton.Text = "加分功能:中值濾波";
            this.MedianButton.UseVisualStyleBackColor = true;
            this.MedianButton.Click += new System.EventHandler(this.MedianButton_Click);
            // 
            // reliefButton
            // 
            this.reliefButton.Enabled = false;
            this.reliefButton.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.reliefButton.Location = new System.Drawing.Point(566, 410);
            this.reliefButton.Name = "reliefButton";
            this.reliefButton.Size = new System.Drawing.Size(100, 30);
            this.reliefButton.TabIndex = 23;
            this.reliefButton.Text = "浮雕";
            this.reliefButton.UseVisualStyleBackColor = true;
            this.reliefButton.Click += new System.EventHandler(this.reliefButton_Click);
            // 
            // load_Button
            // 
            this.load_Button.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.load_Button.Location = new System.Drawing.Point(46, 374);
            this.load_Button.Name = "load_Button";
            this.load_Button.Size = new System.Drawing.Size(100, 30);
            this.load_Button.TabIndex = 24;
            this.load_Button.Text = "讀圖";
            this.load_Button.UseVisualStyleBackColor = true;
            this.load_Button.Click += new System.EventHandler(this.load_Button_Click);
            // 
            // exitButton
            // 
            this.exitButton.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.exitButton.Location = new System.Drawing.Point(672, 446);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(100, 30);
            this.exitButton.TabIndex = 25;
            this.exitButton.Text = "EXIT";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.saveButton.Location = new System.Drawing.Point(46, 410);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 30);
            this.saveButton.TabIndex = 26;
            this.saveButton.Text = "存取圖像";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // load_bird_Button
            // 
            this.load_bird_Button.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.load_bird_Button.Location = new System.Drawing.Point(159, 374);
            this.load_bird_Button.Name = "load_bird_Button";
            this.load_bird_Button.Size = new System.Drawing.Size(100, 30);
            this.load_bird_Button.TabIndex = 27;
            this.load_bird_Button.Text = "load bird";
            this.load_bird_Button.UseVisualStyleBackColor = true;
            this.load_bird_Button.Click += new System.EventHandler(this.load_bird_Button_Click);
            // 
            // load_dog_Button
            // 
            this.load_dog_Button.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.load_dog_Button.Location = new System.Drawing.Point(159, 410);
            this.load_dog_Button.Name = "load_dog_Button";
            this.load_dog_Button.Size = new System.Drawing.Size(100, 30);
            this.load_dog_Button.TabIndex = 28;
            this.load_dog_Button.Text = "load dog";
            this.load_dog_Button.UseVisualStyleBackColor = true;
            this.load_dog_Button.Click += new System.EventHandler(this.load_dog_Button_Click);
            // 
            // buttonCamshift
            // 
            this.buttonCamshift.Enabled = false;
            this.buttonCamshift.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.buttonCamshift.Location = new System.Drawing.Point(778, 374);
            this.buttonCamshift.Name = "buttonCamshift";
            this.buttonCamshift.Size = new System.Drawing.Size(100, 30);
            this.buttonCamshift.TabIndex = 29;
            this.buttonCamshift.Text = "物件追蹤";
            this.buttonCamshift.UseVisualStyleBackColor = true;
            this.buttonCamshift.Click += new System.EventHandler(this.buttonCamshift_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 501);
            this.Controls.Add(this.buttonCamshift);
            this.Controls.Add(this.load_dog_Button);
            this.Controls.Add(this.load_bird_Button);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.load_Button);
            this.Controls.Add(this.reliefButton);
            this.Controls.Add(this.MedianButton);
            this.Controls.Add(this.mirrorButton);
            this.Controls.Add(this.negativeButton);
            this.Controls.Add(this.binarizationButton);
            this.Controls.Add(this.grayButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OutputPictureBox);
            this.Controls.Add(this.SourcePictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.SourcePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox SourcePictureBox;
        private System.Windows.Forms.PictureBox OutputPictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button grayButton;
        private System.Windows.Forms.Button binarizationButton;
        private System.Windows.Forms.Button negativeButton;
        private System.Windows.Forms.Button mirrorButton;
        private System.Windows.Forms.Button MedianButton;
        private System.Windows.Forms.Button reliefButton;
        private System.Windows.Forms.Button load_Button;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button load_bird_Button;
        private System.Windows.Forms.Button load_dog_Button;
        private System.Windows.Forms.Button buttonCamshift;
    }
}

