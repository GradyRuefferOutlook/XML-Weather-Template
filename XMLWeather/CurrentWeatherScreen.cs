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
using Xamarin.Forms.Xaml;


//Doubleclick on element to switch between active character and text box
namespace XMLWeather
{
    public partial class CurrentWeatherScreen : UserControl
    {
        public static int chosenDay = 0;
        Rectangle backdropHolder = new Rectangle();
        Image[] weather = new Image[8];
        Bitmap[] backdrop = new Bitmap[8];
        double[] opacities = new double[8];
        DateTime time;
        String lastUpdated;
        Rectangle groundRect = new Rectangle();
        Lovemp user = new Lovemp(0, 0);
        Image startButton = Properties.Resources.Start;
        Image arrowButton = Properties.Resources.ArrowButton;
        Image barImage = Properties.Resources.Bar;
        Image timeBarImage = Properties.Resources.TimeBar;
        bool isNight = false;
        public static Font timeFont = new Font(new FontFamily("Perpetua"), 15, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font dayFont = new Font(new FontFamily("Perpetua"), 17, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font tempFont = new Font(new FontFamily("Magneto"), 15, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font explainFont = new Font(new FontFamily("Magneto"), 10, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font uiFont = new Font(new FontFamily("Microsoft YaHei UI"), 18, FontStyle.Bold, GraphicsUnit.Pixel);
        SolidBrush textBrush = new SolidBrush(Color.White);
        List<Rectangle> Floors = new List<Rectangle>();
        Rectangle startRect, arrowRect, hotBRect, coldBRect, hotTRect, coldTRect, currentBRect, currentTRect;
        Image thermBH = Properties.Resources.HotBase, thermCH = Properties.Resources.ColdBase, thermCuH = Properties.Resources.CurrentBase;
        SolidBrush hotBrush = new SolidBrush(Color.Red), coldBrush = new SolidBrush(Color.LightBlue), currentBrush = new SolidBrush(Color.BlueViolet);
        public static int highTemp, lowTemp, currentTemp, highTempC, lowTempC, currentTempC, highTempD = 0, lowTempD = 0, currentTempD = 0;
        Rectangle hotDisplay, coldDisplay, currentDisplay;
        Image thermCapL = Properties.Resources.ThermBarLeft, thermCapR = Properties.Resources.ThermBarRight, thermCapC = Properties.Resources.CurrentBar;
        Rectangle thermBarL, thermBarR, thermBarC;
        int thermWidth = 20;
        Rectangle sunRiseD, sunSetD, sunRiseT, sunSetT;
        Rectangle DayDisplay, weatherDisplay;
        Image sun = Properties.Resources.Sunrise, moon = Properties.Resources.Sunset;
        Rectangle dayUp, dayDown;
        Image upB = Properties.Resources.UpBlock, downB = Properties.Resources.DownBlock, blockB = Properties.Resources.BlockBack;
        Image[] daysOfWeek = { Properties.Resources.Monday, Properties.Resources.Tuesday, Properties.Resources.Wednesday, Properties.Resources.Thursday, Properties.Resources.Friday, Properties.Resources.Saturday, Properties.Resources.Sunday };
        Image displayDate;
        int dOW;
        public CurrentWeatherScreen()
        {
            InitializeComponent();

            ConvertTemp();

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

            user = new Lovemp((groundRect.Width / 2) - (user.w / 2), groundRect.Y - user.h - 5);

            dayUp = new Rectangle((this.Width / 2) - 100 - 25, groundRect.Y - (user.h * 2) - 50, 50, 50);
            dayDown = new Rectangle((this.Width / 2) + 100 - 25, groundRect.Y - (user.h * 2) - 50, 50, 50);
            Floors.Add(dayUp);
            Floors.Add(dayDown);

            for (int i = 0; i < weather.Length; i++)
            {
                backdrop[i] = ChangeOpacity(weather[i], Convert.ToSingle(opacities[i]));
            }

            Floors.Add(groundRect);

            startRect = new Rectangle(groundRect.X, groundRect.Y - 2, groundRect.Width / 8, groundRect.Height + 2);
            Floors.Add(startRect);

            arrowRect = new Rectangle(groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) - 10, groundRect.Y, 20, groundRect.Height);
            Floors.Add(arrowRect);

            hotBRect = new Rectangle(10, groundRect.Y - (user.h * 3) - 100, 100, 100);
            coldBRect = new Rectangle(this.Width - 110, groundRect.Y - (user.h * 3) - 100, 100, 100);
            currentBRect = new Rectangle((this.Width / 2) - 131, groundRect.Y - (user.h * 3) - 100, 100, 100);

            Floors.Add(new Rectangle(hotBRect.X + 30, hotBRect.Y, hotBRect.Width - 60, hotBRect.Height));
            Floors.Add(new Rectangle(coldBRect.X + 30, coldBRect.Y, coldBRect.Width - 60, coldBRect.Height));
            Floors.Add(new Rectangle(currentBRect.X, currentBRect.Y, currentBRect.Width, currentBRect.Height));

            hotTRect = new Rectangle(hotBRect.X + 21, hotBRect.Y + 40, hotBRect.Width, hotBRect.Height - 40);
            coldTRect = new Rectangle(coldBRect.X + 21, coldBRect.Y + 40, coldBRect.Width, coldBRect.Height - 40);
            currentTRect = new Rectangle(currentBRect.X + 21, currentBRect.Y + 35, currentBRect.Width, currentBRect.Height - 40);

            hotDisplay = new Rectangle(0, 0, 0, 0);
            coldDisplay = new Rectangle(0, 0, 0, 0);
            currentDisplay = new Rectangle(0, 0, 0, 0);

            thermBarL = new Rectangle(hotBRect.X + (hotBRect.Width / 2) - (thermWidth / 2) - 2, hotBRect.Y - (400 / 2), thermWidth + 4, 400 / 2);
            thermBarR = new Rectangle(coldBRect.X + (coldBRect.Width / 2) - (thermWidth / 2) - 2, coldBRect.Y - (400 / 2), thermWidth + 4, 400 / 2);
            thermBarC = new Rectangle((this.Width / 2) - 31, currentBRect.Y + (currentBRect.Height / 2) - 2 - (thermWidth / 2), 324 / 2, thermWidth + 4);
            Floors.Add(new Rectangle(thermBarL.X, thermBarL.Y, thermBarL.Width, thermBarL.Height + 75));
            Floors.Add(new Rectangle(thermBarR.X, thermBarR.Y, thermBarR.Width, thermBarR.Height + 75));
            Floors.Add(new Rectangle(thermBarC.X - 75, thermBarC.Y, thermBarC.Width + 75, thermBarC.Height));

            sunRiseD = new Rectangle(-300, -300, 600, 600);
            sunSetD = new Rectangle(this.Width - 300, -300, 600, 600);

            LocationBox.Size = new Size(150, 40);
            LocationBox.Location = new Point(startRect.X + startRect.Width + 100, groundRect.Y + 2 + 5);

            LonBox.Size = new Size(100, 40);
            LonBox.Location = new Point(LocationBox.Location.X + LocationBox.Width + 60 + 15 + 5, groundRect.Y + 2 + 5);

            LatBox.Size = new Size(100, 40);
            LatBox.Location = new Point(LonBox.Location.X + LonBox.Width + 60 + 10, groundRect.Y + 2 + 5);

            setTextBox();

            lastUpdated = time.TimeOfDay.ToString().Substring(0, 8);

            DayDisplay = new Rectangle((this.Width / 2) - ((sunSetD.X - (sunRiseD.X + sunRiseD.Width)) / 2), 0, sunSetD.X - (sunRiseD.X + sunRiseD.Width), 50);
            weatherDisplay = new Rectangle((DayDisplay.X + DayDisplay.Width / 2) - (((DayDisplay.Width / 3) * 4) / 2), DayDisplay.Y + DayDisplay.Height, (DayDisplay.Width / 3) * 4, 25);

            DetermineDayOfWeek();

            timeOp.Enabled = true;
        }

        void setTextBox()
        {
            LocationBox.Text = Form1.city + "," + Form1.state + "," + Form1.countryCode;
            LonBox.Text = Form1.lon;
            LatBox.Text = Form1.lat;
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

            e.Graphics.DrawImage(blockB, dayUp);
            e.Graphics.DrawImage(blockB, dayDown);

            e.Graphics.DrawImage(upB, dayUp);
            e.Graphics.DrawImage(downB, dayDown);

            e.Graphics.DrawImage(sun, sunRiseD);
            e.Graphics.DrawImage(moon, sunSetD);

            e.Graphics.DrawImage(startButton, startRect);

            e.Graphics.DrawImage(barImage, new Rectangle(groundRect.X + groundRect.Width / 8, groundRect.Y, Convert.ToInt16(groundRect.Width * 17 / 24), groundRect.Height));

            e.Graphics.DrawImage(arrowButton, arrowRect);

            e.Graphics.DrawImage(timeBarImage, new Rectangle(groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 10, groundRect.Y, this.Width - (groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 10), groundRect.Height));

            e.Graphics.DrawString(time.DayOfWeek.ToString(), dayFont, textBrush, groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 5, groundRect.Y);
            e.Graphics.DrawString(time.TimeOfDay.ToString().Substring(0, 8), timeFont, textBrush, groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 93, groundRect.Y + 4);
            e.Graphics.DrawString(time.Date.ToString().Substring(0, 10), timeFont, textBrush, groundRect.X + groundRect.Width / 8 + Convert.ToInt16(groundRect.Width * 17 / 24) + 75, groundRect.Y + 19);

            e.Graphics.DrawImage(user.drawImage, user.drawRectangle);

            e.Graphics.DrawImage(thermBH, hotBRect);
            e.Graphics.DrawImage(thermCH, coldBRect);
            e.Graphics.DrawImage(thermCuH, currentBRect);

            e.Graphics.DrawString(highTemp + "°K", tempFont, textBrush, hotTRect);
            e.Graphics.DrawString(lowTemp + "°K", tempFont, textBrush, coldTRect);
            e.Graphics.DrawString(currentTemp + "°K", tempFont, textBrush, currentTRect);
            e.Graphics.DrawString("Current", tempFont, textBrush, new Rectangle(currentTRect.X, currentTRect.Y + 15, currentTRect.Width, currentTRect.Height));

            e.Graphics.FillRectangle(hotBrush, hotDisplay);
            e.Graphics.FillRectangle(coldBrush, coldDisplay);
            e.Graphics.FillRectangle(currentBrush, currentDisplay);

            e.Graphics.DrawImage(thermCapL, thermBarL);
            e.Graphics.DrawImage(thermCapR, thermBarR);
            e.Graphics.DrawImage(thermCapC, thermBarC);

            e.Graphics.DrawString("Location:", uiFont, textBrush, new Point(startRect.X + startRect.Width + 5, groundRect.Y + 2 + 5));

            e.Graphics.DrawLine(new Pen(Color.DarkBlue, 2), new Point(LocationBox.Location.X + LocationBox.Width + 5, groundRect.Y), new Point(LocationBox.Location.X + LocationBox.Width + 5, groundRect.Y + groundRect.Height));

            e.Graphics.DrawString("L.O.N.:", uiFont, textBrush, new Point(LocationBox.Location.X + LocationBox.Width + 10, groundRect.Y + 2 + 5));

            e.Graphics.DrawString("L.A.T.:", uiFont, textBrush, new Point(LonBox.Location.X + LonBox.Width + 5, groundRect.Y + 2 + 5));

            e.Graphics.DrawImage(displayDate, DayDisplay);
            e.Graphics.DrawImage(barImage, weatherDisplay);
            e.Graphics.DrawString(Form1.days[chosenDay].date + ": " + Form1.days[chosenDay].conditionName.Substring(0, 1).ToUpper() + Form1.days[chosenDay].conditionName.Substring(1, Form1.days[chosenDay].conditionName.Length - 1), uiFont, textBrush, weatherDisplay);
        }

        void ConvertTemp()
        {
            highTemp = (int)(Convert.ToDouble(Form1.days[chosenDay].tempHigh) + 273);
            lowTemp = (int)(Convert.ToDouble(Form1.days[chosenDay].tempLow) + 273);
            try
            {
                currentTemp = (int)(Convert.ToDouble(Form1.days[chosenDay].currentTemp) + 273);
            }
            catch
            {

            }

            int diff = Math.Abs(highTemp - lowTemp);

            highTempC = highTemp - (diff);
            currentTempC = currentTemp;
            lowTempC = lowTemp - (10 * diff);

            currentTempD = 0;
            highTempD = 0;
            lowTempD = 0;
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

            AdjustTemps();

            int currentDay = chosenDay;

            user.Move(Floors, this);

            if (currentDay != chosenDay)
            {
                ConvertTemp();
            }

            DetermineDayOfWeek();

            Refresh();

        }

        void DetermineDayOfWeek()
        {
            if (dOW == null)
            {
                switch (time.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        {
                            dOW = 0;
                            break;
                        }
                    case DayOfWeek.Tuesday:
                        {
                            dOW = 1;
                            break;
                        }
                    case DayOfWeek.Wednesday:
                        {
                            dOW = 2;
                            break;
                        }
                    case DayOfWeek.Thursday:
                        {
                            dOW = 3;
                            break;
                        }
                    case DayOfWeek.Friday:
                        {
                            dOW = 4;
                            break;
                        }
                    case DayOfWeek.Saturday:
                        {
                            dOW = 5;
                            break;
                        }
                    case DayOfWeek.Sunday:
                        {
                            dOW = 6;
                            break;
                        }
                }
                displayDate = daysOfWeek[dOW];
            }
            else
            {
                displayDate = daysOfWeek[(dOW + chosenDay) % 7];
            }
        }

        void AdjustTemps()
        {
            if (highTempD < highTempC / 2)
            {
                highTempD++;
            }
            else if (highTempD > highTempC / 2)
            {
                highTempD--;
            }

            if (lowTempD < lowTempC / 2)
            {
                lowTempD++;
            }
            else if (lowTempD > lowTempC / 2)
            {
                lowTempD--;
            }

            if (currentTempD < currentTempC / 2)
            {
                currentTempD++;
            }
            else if (currentTempD > currentTempC / 2)
            {
                currentTempD--;
            }

            hotDisplay = new Rectangle(hotBRect.X + (hotBRect.Width / 2) - (thermWidth / 2), hotBRect.Y - highTempD, thermWidth, highTempD);
            coldDisplay = new Rectangle(coldBRect.X + (coldBRect.Width / 2) - (thermWidth / 2), coldBRect.Y - lowTempD, thermWidth, lowTempD);
            currentDisplay = new Rectangle(currentBRect.X + currentBRect.Width, currentBRect.Y + (currentBRect.Height / 2) - (thermWidth / 2), currentTempD, thermWidth);
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
            switch (e.KeyCode)
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

        private void CurrentWeatherScreen_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle mousePoint = new Rectangle(Cursor.Position.X - Form1.cursorAdjX, Cursor.Position.Y - Form1.cursorAdjY, 1, 1);

            if (mousePoint.IntersectsWith(groundRect))
            {
                LocationBox.Enabled = true;
                LonBox.Enabled = true;
                LatBox.Enabled = true;
            }
            else
            {
                LocationBox.Enabled = false;
                LonBox.Enabled = false;
                LatBox.Enabled = false;
                this.Focus();
            }

            if (mousePoint.IntersectsWith(startRect))
            {
                if (LonBox.Text != Form1.lon || LatBox.Text != Form1.lat)
                {
                    try
                    {
                        Form1.days.Clear();

                        Form1.lon = LonBox.Text;
                        Form1.lat = LatBox.Text;

                        Form1.ReverseGeocoding();

                        Form1.ExtractForecast();
                        Form1.ExtractCurrent();

                        setTextBox();
                        ConvertTemp();
                        chosenDay = 0;
                        Form1.days.RemoveAt(Form1.days.Count - 1);
                    }
                    catch
                    {
                        chosenDay = 0;
                        setTextBox();
                        ConvertTemp();
                        LonBox.Text = "INVALID";
                        LatBox.Text = "INVALID";
                    }
                }
                else
                {
                    try
                    {
                        Form1.days.Clear();

                        Form1.area = LocationBox.Text;

                        Form1.ForwardGeocoding();

                        Form1.ExtractForecast();
                        Form1.ExtractCurrent();
                        
                        chosenDay = 0;

                        setTextBox();
                        ConvertTemp();
                        Form1.days.RemoveAt(Form1.days.Count - 1);
                    }
                    catch
                    {
                        chosenDay = 0;
                        setTextBox();
                        ConvertTemp();
                        LocationBox.Text = "INVALID";
                    }
                }
            }
            else if (mousePoint.IntersectsWith(arrowRect))
            {

            }
        }
    }
}
