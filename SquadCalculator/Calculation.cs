using System;
using System.Drawing;
using System.Globalization;
using SquadCalculator.ViewModels;
using SquadCalculator.Weapons;

namespace SquadCalculator
{
    public class Calculation
    {
        private const double DegToMilFactor = (double) 360 / 6400;
        
        private Map _selectedMap;
        
        public Map SelectedMap
        {
            get => MainViewModel.selectedItem;
            set => _selectedMap = value;
        }
        
        public string Distance { get; set; } = "";

        public string Azimuth { get; set; } = "";

        public string Elevation { get; set; } = "";

        private double MilToDeg(double mil)
        {
            return mil * DegToMilFactor;
        }

        private double MilToRad(double mil)
        {
            return DegToRad(MilToDeg(mil));
        }

        private double RadToMil(double rad)
        {
            return DegToMil(RadToDeg(rad));
        }

        private double DegToRad(double deg)
        {
            return (deg * Math.PI) / 180;
        }

        private double RadToDeg(double rad)
        {
            return (rad * 180) / Math.PI;
        }

        private double DegToMil(double deg)
        {
            return deg / DegToMilFactor;
        }

        private double GetDist((double X, double Y) mortar, (double X, double Y) target)
        {
            double mapScale = CalculateMapScale();
            
            var dLat = (mortar.Y - target.Y) * mapScale;
            var dLng = (mortar.X - target.X) * mapScale;
           
            return Double.Hypot(dLat, dLng);
        }

        private double CalculateMapScale()
        {
            double scale = SelectedMap.Size / 4096.0;
            return scale;
        }
        
        private double GetBearing((double X, double Y) a, (double X, double Y) b)
        {
            var bearing = Math.Atan2(b.X - a.X, b.Y - a.Y) * 180 / Math.PI;
            bearing = (180 - bearing) % 360;

            if (bearing < 0)
            {
                bearing += 360;
            }

            return bearing;
        }

        private double GetElevation(double x, double y, Weapon weapon)
        {
            double v = weapon.Velocity;
            double g = weapon.Gravity;
            var displacement = Math.Sqrt(Math.Pow(v, 4) - g * (g * Math.Pow(x, 2) + 2 * y * Math.Pow(v, 2)));
            var trajectoryAngle = Math.Atan((Math.Pow(v, 2) + displacement) / (g * x));

            if (weapon.AngleUnit == AngleUnit.Milliradians)
            {
                return RadToMil(trajectoryAngle);
            }

            return RadToDeg(trajectoryAngle);
        }

        private double GetHeight((double, double) a, (double, double) b, Bitmap heightMapImage)
        {
            double mortarHeight = default;
            double targetHeight = default;
            try
            { 
                var mortarColor = heightMapImage.GetPixel((int)a.Item1, (int)a.Item2); 
                var targetColor = heightMapImage.GetPixel((int)b.Item1, (int)b.Item2); 
                mortarHeight = (255 + mortarColor.R - mortarColor.B) * SelectedMap.Scale; 
                targetHeight = (255 + targetColor.R - targetColor.B) * SelectedMap.Scale;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
          
            return targetHeight - mortarHeight;
        }

        public FireResult Shoot(Bitmap heightMap, Weapon weapon, FireCoordinates fireCoordinates)
        {
            var height = GetHeight(fireCoordinates.MortarPositionOnHeightMap, fireCoordinates.TargetPositionOnHeightMap, heightMap);
            var distance = GetDist(fireCoordinates.MortarPositionOnMap, fireCoordinates.TargetPositionOnMap);
            var bearing = GetBearing(fireCoordinates.MortarPositionOnMap, fireCoordinates.TargetPositionOnMap);
            var elevation = GetElevation(distance, height, weapon);
            
            Distance = Math.Round(distance, 2).ToString(CultureInfo.InvariantCulture);;
            Elevation = Math.Round(elevation, 2).ToString(CultureInfo.InvariantCulture);
            Azimuth = Math.Round(bearing, 2).ToString(CultureInfo.InvariantCulture);
            
            FireResult fireResult = new FireResult(Math.Round(distance,2).ToString() + " m", Math.Round(bearing,2).ToString() + " °", Math.Round(elevation,2).ToString());
            return fireResult;
        }
      
    }

  
    
}