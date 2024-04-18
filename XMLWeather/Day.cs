using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMLWeather
{
    public class Day
    {
        public string sunRise, sunSet, conditionNumber, conditionName, date, currentTemp, currentTime, condition, location, tempHigh, tempLow, 
            windSpeed, windDirection, precipitation, visibility, feelsLike;

        public Day()
        {
            sunRise = sunSet = conditionNumber = conditionName = date = currentTemp = currentTime = condition = location = tempHigh = tempLow
                = windSpeed = windDirection = precipitation = visibility = feelsLike = "";
        }
    }
}
