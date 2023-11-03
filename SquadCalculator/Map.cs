using System;

namespace SquadCalculator
{
    [Serializable]
    public class Map
    {
        public string Name { get; set; }
        public double BLevel { get; set; }
        public double WLevel { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double Scale { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int Size { get; set; }
        [NonSerialized]
        private string _heightMapPath;
        [NonSerialized]
        private string _mapPath;

        public String HeightMapPath
        {
            get
            {
                return _heightMapPath;
            }
            set
            {
                _heightMapPath = value;
            }
        }
        
        public String MapPath
        {
            get
            {
                return _mapPath;
            }
            set
            {
                _mapPath = value;
            }
        }
        
    }
}