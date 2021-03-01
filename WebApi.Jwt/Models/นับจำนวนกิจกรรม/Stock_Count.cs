using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.นับจำนวนกิจกรรม
{

        public class Stock_info
        {
            public Stock_info()
            {
                Data = new List<SeedAnimal_info>();
            }
        //[JsonProperty ("DLDzone")]
        //public Int64 DLDZone { get; set; }
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Total")]
        public double Total { get; set; }

        [JsonProperty("Color")]
        public string Color { get; set; }

        public List<SeedAnimal_info> Data { get; set; }
        }
        public class SeedAnimal_info
        {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Weight")]
        public double Weight { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }

    }
    public class StockAnimals

    {
        public StockAnimals()
        {
            Data = new List<SeedAnimalStock_info>();
        }

        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Total")]
        public double Total { get; set; }

        [JsonProperty("Color")]
        public string Color { get; set; }

        public List<SeedAnimalStock_info> Data { get; set; }
    }
    public class SeedAnimalStock_info
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Weight")]
        public double Weight { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }

    }


}

