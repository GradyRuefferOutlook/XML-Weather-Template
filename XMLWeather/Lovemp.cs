using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLWeather
{
    internal class Lovemp
    {
        public int w, h;
        public bool canJump = true, jumping = false;
        public float x, y, xS, yS, dXS, dYS, sX, sY;
        bool aDown = false, dDown = false, sDown = false, spDown = false, shDown = false, fDown = false, wDown = false;
        int jumpTime = 20;
        int jumpTimeSet = 20;
        int baseSpeed = 10;
        float storedSpeed = 0;
        int maxStoredSpeed = 5;
        float speedInc = (float)0.25;
        int aniCount = 0;
        public Image idle = Properties.Resources.LovempIdle;
        public Image[] rC = { Properties.Resources.LovempRightCycle__1_, Properties.Resources.LovempRightCycle__2_, Properties.Resources.LovempRightCycle__3_, Properties.Resources.LovempRightCycle__4_, Properties.Resources.LovempRightCycle__5_, Properties.Resources.LovempRightCycle__6_, Properties.Resources.LovempRightCycle__7_, Properties.Resources.LovempRightCycle__8_, Properties.Resources.LovempRightCycle__9_ };
        public Image[] lC = { Properties.Resources.LovempLeftCycle__1_, Properties.Resources.LovempLeftCycle__2_, Properties.Resources.LovempLeftCycle__3_, Properties.Resources.LovempLeftCycle__4_, Properties.Resources.LovempLeftCycle__5_, Properties.Resources.LovempLeftCycle__6_, Properties.Resources.LovempLeftCycle__7_, Properties.Resources.LovempLeftCycle__8_, Properties.Resources.LovempLeftCycle__9_ };
        public Image[] rH = { Properties.Resources.LovempRightHit__1_, Properties.Resources.LovempRightHit__2_, Properties.Resources.LovempRightHit__3_, Properties.Resources.LovempRightHit__4_, Properties.Resources.LovempRightHit__5_, Properties.Resources.LovempRightHit__6_, Properties.Resources.LovempRightHit__7_ };
        public Image[] lH = { Properties.Resources.LovempLeftHit__1_, Properties.Resources.LovempLeftHit__2_, Properties.Resources.LovempLeftHit__3_, Properties.Resources.LovempLeftHit__4_, Properties.Resources.LovempLeftHit__5_, Properties.Resources.LovempLeftHit__6_, Properties.Resources.LovempLeftHit__7_ };

        public RectangleF drawRectangle;
        public RectangleF prevArea;
        public Image drawImage = Properties.Resources.LovempIdle;

        public Lovemp(int x, int y)
        {
            this.x = x;
            this.y = y;
            sX = x;
            sY = y;
            dXS = 5;
            dYS = -1;
            w = 50;
            h = 50;
            drawRectangle = new Rectangle(x, y, w, h);
        }

        public void KeyInput(int k, bool r)
        {
            if (r)
            {
                switch (k)
                {
                    case 0:
                        aDown = false;
                        break;
                    case 1:
                        dDown = false;
                        break;
                    case 2:
                        sDown = false;
                        break;
                    case 3:
                        spDown = false;
                        break;
                    case 4:
                        shDown = false;
                        break;
                    case 5:
                        fDown = false;
                        break;
                    case 6:
                        wDown = false;
                        break;

                }
            }
            else
            {
                switch (k)
                {
                    case 0:
                        aDown = true;
                        break;
                    case 1:
                        dDown = true;
                        break;
                    case 2:
                        sDown = true;
                        break;
                    case 3:
                        spDown = true;
                        break;
                    case 4:
                        shDown = true;
                        break;
                    case 5:
                        fDown = true;
                        break;
                    case 6:
                        wDown = true;
                        break;

                }
            }

        }

        void Place()
        {
            prevArea = drawRectangle;
            drawRectangle.X = x;
            drawRectangle.Y = y;
        }

        public void Move(List<Rectangle> Floors, UserControl control)
        {
            if (canJump && spDown)
            {
                canJump = false;
                jumping = true;
                y++;
                yS = 25;
            }
            else if (!spDown)
            {
                jumping = false;
            }

            double jumpAdjust = 0.00005;
            if (jumping)
            {
                yS -= Convert.ToSingle(-(jumpAdjust / 1.5) * (jumpTime + 200) * (jumpTime - 200));
                jumpTime--;
            }
            else
            {
                yS -= Convert.ToSingle(-(jumpAdjust) * (jumpTime + 200) * (jumpTime - 200) * 2);
            }

            if (jumpTime == 0)
            {
                jumping = false;
                spDown = false;
            }

            y -= yS;

            Place();

            foreach (Rectangle floor in Floors)
            {
                if (drawRectangle.IntersectsWith(floor))
                {
                    //Hitting your Head or Feet?
                    if (Math.Abs((prevArea.Y + h) - floor.Y) > Math.Abs(prevArea.Y - (floor.Y + floor.Height)))
                    {
                        //More your Head or Side
                        if (Math.Abs(prevArea.Y - (floor.Y + floor.Height)) < Math.Abs((prevArea.X + w) - floor.X) || Math.Abs(prevArea.Y - (floor.Y + floor.Height)) < Math.Abs(prevArea.X - (floor.X + floor.Width)))
                        {
                            jumping = false;
                            y = (floor.Y + floor.Height);
                            yS = 0;
                            if (floor == Floors[0])
                            {
                                CurrentWeatherScreen.chosenDay++;
                                CurrentWeatherScreen.currentTempD = 0;
                                CurrentWeatherScreen.highTempD = 0;
                                CurrentWeatherScreen.lowTempD = 0;

                                if (CurrentWeatherScreen.chosenDay > Form1.days.Count - 1)
                                {
                                    CurrentWeatherScreen.chosenDay = Form1.days.Count - 1;
                                }
                            } 
                            else if (floor == Floors[1])
                            {
                                CurrentWeatherScreen.chosenDay--;
                                CurrentWeatherScreen.currentTempD = 0;
                                CurrentWeatherScreen.highTempD = 0;
                                CurrentWeatherScreen.lowTempD = 0;

                                if (CurrentWeatherScreen.chosenDay < 0)
                                {
                                    CurrentWeatherScreen.chosenDay = 0;
                                }
                            }
                        } 
                        else
                        {
                            if (Math.Abs((prevArea.X + w) - floor.X) > Math.Abs(prevArea.X - (floor.X + floor.Width)))
                            {
                                x = (floor.X + floor.Width);
                                if (spDown)
                                {
                                    xS = 25;
                                    yS = 25;
                                }
                            }
                            else
                            {
                                x = (floor.X - w);
                                if (spDown)
                                {
                                    xS = -25;
                                    yS = 25;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Math.Abs((prevArea.Y + h) - floor.Y) < Math.Abs((prevArea.X + w) - floor.X) || Math.Abs((prevArea.Y + h) - floor.Y) < Math.Abs(prevArea.X - (floor.X + floor.Width)))
                        {
                            canJump = true;
                            jumpTime = jumpTimeSet;
                            y = floor.Y - h;
                            yS = 0;
                        }
                        else
                        {
                            if (Math.Abs((prevArea.X + w) - floor.X) > Math.Abs(prevArea.X - (floor.X + floor.Width)))
                            {
                                x = (floor.X + floor.Width);
                                if (spDown)
                                {
                                    xS = 25;
                                    yS = 25;
                                }
                            }
                            else
                            {
                                x = (floor.X - w);
                                if (spDown)
                                {
                                    xS = -25;
                                    yS = 25;
                                }
                            }
                        }
                    }

                    Place();
                }
            }

            if (!jumping && yS == 0)
            {
                if (aDown && !dDown)
                {
                    drawImage = lC[aniCount];
                    xS = -baseSpeed + storedSpeed;
                    if (shDown)
                    {
                        storedSpeed -= speedInc;
                        if (storedSpeed < -maxStoredSpeed)
                        {
                            storedSpeed = -maxStoredSpeed;
                        }
                    }
                }
                else if (!aDown && dDown)
                {
                    drawImage = rC[aniCount];
                    xS = baseSpeed + storedSpeed;
                    if (shDown)
                    {
                        storedSpeed += speedInc;
                        if (storedSpeed > maxStoredSpeed)
                        {
                            storedSpeed = maxStoredSpeed;
                        }
                    }
                }
                else
                {
                    if (storedSpeed > 0)
                    {
                        storedSpeed -= speedInc * 2;
                        drawImage = rC[aniCount];
                    }
                    else if (storedSpeed < 0)
                    {
                        storedSpeed += speedInc * 2;
                        drawImage = lC[aniCount];
                    }

                    if (storedSpeed > -1 && storedSpeed < 1)
                    {
                        storedSpeed = 0;
                    }

                    if (storedSpeed == 0)
                    {
                        drawImage = idle;
                    }

                    xS = storedSpeed;
                }

                x += xS;
            }
            else
            {
                if (aDown && !dDown)
                {
                    xS -= speedInc * 8;
                }
                else if (!aDown && dDown)
                {
                    xS += speedInc * 8;
                }

                if (xS > baseSpeed + maxStoredSpeed)
                {
                    xS = baseSpeed + maxStoredSpeed;
                }
                else if (xS < -baseSpeed - maxStoredSpeed)
                {
                    xS = -baseSpeed - maxStoredSpeed;
                }

                x += xS;
            }

            Place();

            aniCount++;
            if (aniCount > 8)
            {
                aniCount = 0;
            }

            if( y > control.Height * 1.25)
            {
                x = sX;
                y = sY;
                Place();
            }
        }
    }
}
