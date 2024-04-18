using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLWeather
{
    public partial class CurrentWeatherScreen : UserControl
    {
        int chosenDay = 0;
        Rectangle backdropHolder = new Rectangle();
        Image[] weather = new Image[8];
        Bitmap[] backdrop = new Bitmap[8];
        double[] opacities = new double[8];
        DateTime time;
        Rectangle groundRect = new Rectangle();
        Lovemp user = new Lovemp(0,0);
        Image startButton = Properties.Resources.Start;
        Image arrowButton = Properties.Resources.ArrowButton;
        Image barImage = Properties.Resources.Bar;
        Image timeBarImage = Properties.Resources.TimeBar;
        bool isNight = false;
        public static Font timeFont = new Font(new FontFamily("Perpetua"), 15, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font dayFont = new Font(new FontFamily("Perpetua"), 17, FontStyle.Bold, GraphicsUnit.Pixel);
        SolidBrush textBrush = new SolidBrush(Color.White);
        List<Rectangle> Floors = new List<Rectangle>();



        public CurrentWeatherScreen()
        {
            InitializeComponent();

            weather[0] = Properties.Resources.StormBackdrop;
            weather[1] = Properties.Resources.DrizzleBackdrop;
            weather[2] = Properties.Resources.RainBackdrop;
            weather[3] = Properties.Resources.SnowyBackdrop;
            weather[4] = Properties.Resources.WindyBackdrop;
            weather[5] = Properties.Resources.CloudyBackdrop;
            weather[6] = Properties.Resources.DayClearBackdrop;
            weather[7] = Properties.Resources.NightBackdrop;

            for (int i = 0; i < opacities.Length; i++)
            {
                opacities[i] = 0;
            }

            int barSize = 35;

            backdropHolder = new Rectangle(0, 0, this.Width, this.Height - barSize);

            groundRect = new Rectangle(0, this.Height - barSize, this.Width, barSize);

            user = new Lovemp((groundRect.Width/ 2) - (user.w / 2), groundRect.Y - user.h - 5);


            for (int i = 0; i < weather.Length; i++)
            {
                backdrop[i] = ChangeOpacity(weather[i], Convert.ToSingle(opacities[i]));
            }

            Floors.Add(groundRect);

            timeOp.Enabled = true;
        }

        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }

        private void CurrentWeatherScreen_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < weather.Length; i++)
            {
                e.Graphics.DrawImage(backdrop[i], backdropHolder);
            }

            e.Graphics.DrawImage(startButton, new Rectangle(groundRect.X, groundRect.Y - 2, groundRect.Width / 8, groundRect.Height + 2));

            e.Graphics.DrawImage(barImage, new Rectangle(groundRect.X + groundRect.Width / 8, groundRect.Y, Convert.ToInt16(groundRect.Width * 17 / 24), groundRect.Height));

            e.Graphics.DrawImage(arrowButton, new Rectangle(groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) - 10, groundRect.Y, 20, groundRect.Height));

            e.Graphics.DrawImage(timeBarImage, new Rectangle(groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 10, groundRect.Y, this.Width - (groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 10), groundRect.Height));

            e.Graphics.DrawString(time.DayOfWeek.ToString(), dayFont, textBrush, groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 5, groundRect.Y);
            e.Graphics.DrawString(time.TimeOfDay.ToString().Substring(0, 8), timeFont, textBrush, groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 93, groundRect.Y + 4);
            e.Graphics.DrawString(time.Date.ToString().Substring(0, 10), timeFont, textBrush, groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 75, groundRect.Y + 19);

            e.Graphics.DrawImage(user.drawImage, user.drawRectangle);
        }

        int DetermineConditions()
        {
            float opacityMod = Convert.ToSingle(0.05);

            for (int i = 0; i < opacities.Length; i++)
            {
                opacities[i] -= opacityMod;
                if (opacities[i] < 0)
                {
                    opacities[i] = 0;
                }
            }

            if (!isNight)
            {
                if (Convert.ToInt16(Form1.days[chosenDay].conditionNumber) >= 200 && Convert.ToInt16(Form1.days[chosenDay].conditionNumber) < 300)
                {
                    opacities[0] += opacityMod * 2;
                    if (opacities[0] > 1)
                    {
                        opacities[0] = 1;
                    }
                    backdrop[0] = ChangeOpacity(weather[0], Convert.ToSingle(opacities[0]));
                    return 0;
                }
                else if (Convert.ToInt16(Form1.days[chosenDay].conditionNumber) >= 300 && Convert.ToInt16(Form1.days[chosenDay].conditionNumber) < 400)
                {
                    opacities[1] += opacityMod * 2;
                    if (opacities[1] > 1)
                    {
                        opacities[1] = 1;
                    }
                    backdrop[1] = ChangeOpacity(weather[1], Convert.ToSingle(opacities[1]));
                    return 1;
                }
                else if (Convert.ToInt16(Form1.days[chosenDay].conditionNumber) >= 500 && Convert.ToInt16(Form1.days[chosenDay].conditionNumber) < 600)
                {
                    opacities[2] += opacityMod * 2;
                    if (opacities[2] > 1)
                    {
                        opacities[2] = 1;
                    }
                    backdrop[2] = ChangeOpacity(weather[2], Convert.ToSingle(opacities[2]));
                    return 2;
                }
                else if (Convert.ToInt16(Form1.days[chosenDay].conditionNumber) >= 600 && Convert.ToInt16(Form1.days[chosenDay].conditionNumber) < 700)
                {
                    opacities[3] += opacityMod * 2;
                    if (opacities[3] > 1)
                    {
                        opacities[3] = 1;
                    }
                    backdrop[3] = ChangeOpacity(weather[3], Convert.ToSingle(opacities[3]));
                    return 3;
                }
                else if (Convert.ToInt16(Form1.days[chosenDay].conditionNumber) >= 700 && Convert.ToInt16(Form1.days[chosenDay].conditionNumber) < 800)
                {
                    opacities[4] += opacityMod * 2;
                    if (opacities[4] > 1)
                    {
                        opacities[4] = 1;
                    }
                    backdrop[4] = ChangeOpacity(weather[4], Convert.ToSingle(opacities[4]));
                    return 4;
                }
                else if (Convert.ToInt16(Form1.days[chosenDay].conditionNumber) == 803 || Convert.ToInt16(Form1.days[chosenDay].conditionNumber) == 804)
                {
                    opacities[5] += opacityMod * 2;
                    if (opacities[5] > 1)
                    {
                        opacities[5] = 1;
                    }
                    backdrop[5] = ChangeOpacity(weather[5], Convert.ToSingle(opacities[5]));
                    return 5;
                }
                else
                {
                    opacities[6] += opacityMod * 2;
                    if (opacities[6] > 1)
                    {
                        opacities[6] = 1;
                    }
                    backdrop[6] = ChangeOpacity(weather[6], Convert.ToSingle(opacities[6]));
                    return 6;
                }
            }
            else
            {
                opacities[7] += opacityMod * 2;
                if (opacities[7] > 1)
                {
                    opacities[7] = 1;
                }
                backdrop[7] = ChangeOpacity(weather[7], Convert.ToSingle(opacities[7]));
                return 7;
            }
        }

        private void timeOp_Tick(object sender, EventArgs e)
        {
            time = DateTime.Now;

            DetermineNight();

            int w = DetermineConditions();

            for (int i = 0; i < opacities.Length; i++)
            {
                if (opacities[i] > 0 && i != w)
                {
                    backdrop[i] = ChangeOpacity(weather[i], Convert.ToSingle(opacities[i]));
                }
            }

            user.Move(Floors, this);

            Refresh();

        }

        void DetermineNight()
        {
            int rTime = (Convert.ToInt16(Form1.days[0].sunRise.Substring(11, 2)) * 3600) + (Convert.ToInt16(Form1.days[0].sunRise.Substring(14, 2)) * 60) + Convert.ToInt16(Form1.days[0].sunRise.Substring(17, 2)) - 14400;

            int sTime = (Convert.ToInt16(Form1.days[0].sunSet.Substring(11, 2)) * 3600) + (Convert.ToInt16(Form1.days[0].sunSet.Substring(14, 2)) * 60) + Convert.ToInt16(Form1.days[0].sunSet.Substring(17, 2)) - 14400;
            if (Convert.ToInt16(Form1.days[0].sunSet.Substring(11, 2)) < 12)
            {
                sTime = ((Convert.ToInt16(Form1.days[0].sunSet.Substring(11, 2)) + 24) * 3600) + (Convert.ToInt16(Form1.days[0].sunSet.Substring(14, 2)) * 60) + Convert.ToInt16(Form1.days[0].sunSet.Substring(17, 2)) - 14400;
            }

            int timeTime = (time.Hour * 3600) + (time.Minute * 60) + time.Second;

            if (timeTime > rTime && timeTime < sTime)
            {
                isNight = false;
            }
            else
            {
                isNight = true;
            }
        }

        private void CurrentWeatherScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.A:
                    user.KeyInput(0, false);
                    break;
                case Keys.D:
                    user.KeyInput(1, false);
                    break;
                case Keys.S:
                    user.KeyInput(2, false);
                    break;
                case Keys.Space:
                    user.KeyInput(3, false);
                    break;
                case Keys.ShiftKey:
                    user.KeyInput(4, false);
                    break;
                case Keys.F:
                    user.KeyInput(5, false);
                    break;
                case Keys.W:
                    user.KeyInput(6, false);
                    break;
            }
        }

        private void CurrentWeatherScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    user.KeyInput(0, true);
                    break;
                case Keys.D:
                    user.KeyInput(1, true);
                    break;
                case Keys.S:
                    user.KeyInput(2, true);
                    break;
                case Keys.Space:
                    user.KeyInput(3, true);
                    break;
                case Keys.ShiftKey:
                    user.KeyInput(4, true);
                    break;
                case Keys.F:
                    user.KeyInput(5, true);
                    break;
                case Keys.W:
                    user.KeyInput(6, true);
                    break;
            }
        }
    }
}
