using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Benday.Presidents.Api.Models
{
    public class Term
    {
        public Term()
        {
            
        }

        public int Id { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }

        [Required]
        public string Role { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        public DateTime Start { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        public DateTime End { get; set; }
        public int Number { get; set; }
    }
}
