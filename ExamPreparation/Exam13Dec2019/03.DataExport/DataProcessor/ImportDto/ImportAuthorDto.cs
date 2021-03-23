using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.DataProcessor.ImportDto
{
    public class ImportAuthorDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [RegularExpression("^(\\d{3})\\-(\\d{3})\\-(\\d{4})$")]
        [Required]
        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [EmailAddress]
        [Required]
        [JsonProperty("Email")]
        public string Email { get; set; }

        public ImportAuthorBookDto[] Books { get; set; }

    }
}

//"FirstName": "K",
//    "LastName": "Tribbeck",
//    "Phone": "808-944-5051",
//    "Email": "btribbeck0@last.fm",
//    "Books": [
