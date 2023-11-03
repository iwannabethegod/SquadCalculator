using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace SquadCalculator
{
    public class Maps
    {
        public ObservableCollection<Map> ListMaps { get ; set; } = new ObservableCollection<Map>();

        //Only for saving new json list
        public void SaveMap()
        {
            using (FileStream fs = new FileStream("maps.json", FileMode.OpenOrCreate))
            {
                JsonSerializer.SerializeAsync(fs, ListMaps);
            }
        }

        public async Task LoadMapsAsync()
        {
            using (FileStream file = new FileStream("maps.json", FileMode.Open))
            {
                try
                {
                    List<Map>? mapsCollection = 
                        await JsonSerializer.DeserializeAsync<List<Map>>(file);

                    if (mapsCollection?.Count != ListMaps.Count)
                    {
                        ListMaps.Clear();
                        foreach (var map in mapsCollection)
                        {
                            ListMaps.Add(map);
                            
                        }
                        mapsCollection.Clear();
                    }
                }
                catch (Exception e)
                {
                    // Console.WriteLine(e.Message + " " + e.Data);
                }
            }
            SetHeightMapPath();
            SetMapPath();
        }
        
         void SetHeightMapPath() 
         {
            foreach (var map in ListMaps)
            {
                map.HeightMapPath = GetHeightMapPath(map.Name);
            } 
         }

         void SetMapPath()
         {
             foreach (var map in ListMaps)
             {
                 map.MapPath = GetMapPath(map.Name);
             }
         }

        public string GetHeightMapPath(string name)
        {
            string path = "";

            DirectoryInfo dir = new DirectoryInfo("heightmaps");
            FileInfo[] files = dir.GetFiles();
            
            
            IEnumerable<String> n = from file in files
                where file.Name.ToLower().Remove(file.Name.LastIndexOf(@".", StringComparison.InvariantCulture)).Equals(name.ToLower())
                select file.FullName;
            

            foreach (var a in n)
            {
                path = a;
            }
            
            return path;
        }
        
        public string GetMapPath(string name)
        {
            string path = "";

            DirectoryInfo dir = new DirectoryInfo("maps");
            FileInfo[] files = dir.GetFiles();
            
            
            IEnumerable<String> n = from file in files
                where file.Name.ToLower().Remove(file.Name.LastIndexOf(@".", StringComparison.InvariantCulture)).Equals(name.ToLower())
                select file.FullName;
            
            foreach (var a in n)
            {
                path = a;
            }
            
            return path;
        }
        
    }
}