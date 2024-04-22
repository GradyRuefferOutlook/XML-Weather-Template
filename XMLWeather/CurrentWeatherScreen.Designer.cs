namespace XMLWeather
{
    partial class CurrentWeatherScreen
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timeOp = new System.Windows.Forms.Timer(this.components);
            this.LocationBox = new System.Windows.Forms.TextBox();
            this.LonBox = new System.Windows.Forms.TextBox();
            this.LatBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // timeOp
            // 
            this.timeOp.Interval = 20;
            this.timeOp.Tick += new System.EventHandler(this.timeOp_Tick);
            // 
            // LocationBox
            // 
            this.LocationBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LocationBox.Location = new System.Drawing.Point(252, 114);
            this.LocationBox.Name = "LocationBox";
            this.LocationBox.Size = new System.Drawing.Size(100, 24);
            this.LocationBox.TabIndex = 0;
            // 
            // LonBox
            // 
            this.LonBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LonBox.Location = new System.Drawing.Point(400, 323);
            this.LonBox.Name = "LonBox";
            this.LonBox.Size = new System.Drawing.Size(100, 24);
            this.LonBox.TabIndex = 1;
            // 
            // LatBox
            // 
            this.LatBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LatBox.Location = new System.Drawing.Point(408, 331);
            this.LatBox.Name = "LatBox";
            this.LatBox.Size = new System.Drawing.Size(100, 24);
            this.LatBox.TabIndex = 2;
            // 
            // CurrentWeatherScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Controls.Add(this.LatBox);
            this.Controls.Add(this.LonBox);
            this.Controls.Add(this.LocationBox);
            this.DoubleBuffered = true;
            this.Name = "CurrentWeatherScreen";
            this.Size = new System.Drawing.Size(900, 670);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CurrentWeatherScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CurrentWeatherScreen_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CurrentWeatherScreen_MouseDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CurrentWeatherScreen_PreviewKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timeOp;
        private System.Windows.Forms.TextBox LocationBox;
        private System.Windows.Forms.TextBox LonBox;
        private System.Windows.Forms.TextBox LatBox;
    }
}
