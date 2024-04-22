using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.Device.Location;


//Note: if your location permissions are off, comment out the geolocator (it is marked below)
namespace XMLWeather
{
    public partial class Form1 : Form
    {
        // TODO: create list to hold day objects
        public static List<Day> days = new List<Day>();
        public static string lat = "43.36679000";
        public static string lon = "-80.94972000";
        public static string area = "Stratford, ON, CA";
        public static string city = "";
        public static string state = "";
        public static string countryCode = "";
        public static int cursorAdjX, cursorAdjY;

        public Form1()
        {
            InitializeComponent();

            cursorAdjX = this.Location.X;
            cursorAdjY = this.Location.Y;
        }

        void GetLocationProperty()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.Start(); //started watcher
            GeoCoordinate coord = watcher.Position.Location;
            while (watcher.Position.Location.IsUnknown)
            {

            }
            lat = watcher.Position.Location.Latitude.ToString(); //latitude
            lon = watcher.Position.Location.Longitude.ToString();  //logitude
        }

        public static void ForwardGeocoding()
        {
            XmlReader reader = XmlReader.Create("https://api.geoapify.com/v1/geocode/search?text=" + area + "&apiKey=d77525cc39bd42a2bab3ba550875d05f&format=xml");
            while (reader.Read())
            {
                reader.ReadToFollowing("country_code");
                countryCode = reader.ReadString();

                reader.ReadToFollowing("state");
                state = reader.ReadString();

                reader.ReadToFollowing("city");
                city = reader.ReadString();

                reader.ReadToFollowing("lon");
                lon = reader.ReadString();

                reader.ReadToFollowing("lat");
                lat = reader.ReadString();

                reader.ReadToFollowing("formatted");
                area = reader.ReadString();
                break;
            }
        }

        public static void ReverseGeocoding()
        {
            XmlReader reader = XmlReader.Create("https://api.geoapify.com/v1/geocode/reverse?lat=" + lat + "&lon=" + lon + "&apiKey=d77525cc39bd42a2bab3ba550875d05f&format=xml");
            while (reader.Read())
            {
                reader.ReadToFollowing("country_code");
                countryCode = reader.ReadString();

                reader.ReadToFollowing("state");
                state = reader.ReadString();

                reader.ReadToFollowing("city");
                city = reader.ReadString();

                reader.ReadToFollowing("lon");
                lon = reader.ReadString();

                reader.ReadToFollowing("lat");
                lat = reader.ReadString();

                reader.ReadToFollowing("formatted");
                area = reader.ReadString();
                break;
            }
        }

        public static void ExtractForecast()
        {
            //Sorry Mr.T, Have to use yours, this is now a paid feature
            XmlReader reader;
            try
            {
                reader = XmlReader.Create("http://api.openweathermap.org/data/2.5/forecast/daily?q="  + city + ", " + state + ", " + countryCode + "&mode=xml&units=metric&cnt=7&appid=3f2e224b815c0ed45524322e145149f0");
            }
            catch
            {
                reader = XmlReader.Create("http://api.openweathermap.org/data/2.5/forecast/daily?q=" + area + "&mode=xml&units=metric&cnt=7&appid=3f2e224b815c0ed45524322e145149f0");
            }

            while (reader.Read())
            {
                Day d = new Day();

                reader.ReadToFollowing("time");
                d.date = reader.GetAttribute("day");

                reader.ReadToFollowing("sun");
                d.sunRise = reader.GetAttribute("rise");
                d.sunSet = reader.GetAttribute("set");

                reader.ReadToFollowing("symbol");
                d.conditionNumber = reader.GetAttribute("number");
                d.conditionName = reader.GetAttribute("name");

                reader.ReadToFollowing("temperature");
                d.tempLow = reader.GetAttribute("min");
                d.tempHigh = reader.GetAttribute("max");

                //TODO: if day object not null add to the days list
                days.Add(d);
            }
        }

        public static void ExtractCurrent()
        {
            if (days.Count == 0)
            {
                days.Add(new Day());
            }
            // current info is not included in forecast file so we need to use this file to get it
            XmlReader reader;
            try
            {
                reader = XmlReader.Create("http://api.openweathermap.org/data/2.5/weather?q=" + city + ", " + state + ", " + countryCode + "&mode=xml&units=metric&appid=cd7da378ac74c7d48d029dc694b801e3");
            }
            catch
            {
                reader = XmlReader.Create("http://api.openweathermap.org/data/2.5/weather?q=" + area + "&mode=xml&units=metric&appid=cd7da378ac74c7d48d029dc694b801e3");
            }

            //TODO: find the city and current temperature and add to appropriate item in days list
            reader.ReadToFollowing("city");
            days[0].location = reader.GetAttribute("name");
            reader.ReadToFollowing("sun");
            days[0].sunRise = reader.GetAttribute("rise");
            days[0].sunSet = reader.GetAttribute("set");
            reader.ReadToFollowing("temperature");
            days[0].currentTemp = reader.GetAttribute("value");
            days[0].tempLow = reader.GetAttribute("min");
            days[0].tempHigh = reader.GetAttribute("max");
            reader.ReadToFollowing("weather");
            days[0].conditionNumber = reader.GetAttribute("number");
            days[0].conditionName = reader.GetAttribute("value");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Comment this out if you have location permissions turned off
            while (lat == "")
            {
                GetLocationProperty();
            }

            ReverseGeocoding();
            ForwardGeocoding();

            ExtractForecast();
            ExtractCurrent();

            // open weather screen for todays weather
            this.Controls.Add(new CurrentWeatherScreen());
        }
    }
}
