using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace VaporStore.DataProcessor.Dto
{
  public  class ImportGameDto
    {
       
       [JsonProperty("Name")]
       [Required]
        public string Name { get; set; }

        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [JsonProperty("ReleaseDate")]
        [Required]
        public string ReleaseDate { get; set; }

        [JsonProperty("Developer")]
        [Required]
        public string Developer { get; set; }

      
        [Required]
        [JsonProperty("Genre")]
        public  string Genre { get; set; }

     
        [Required]
        [JsonProperty("Tags")]
        public List<string> Tags { get; set; }

    }
}

//"Name": "Invalid",
//		"Price": -5,
//		"ReleaseDate": "2013-07-09",
//		"Developer": "Valid Dev",
//		"Genre": "Valid Genre",
//		"Tags": [
//			"Valid Tag"
//		] 