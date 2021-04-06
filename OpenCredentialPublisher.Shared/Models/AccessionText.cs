using System;
using System.ComponentModel.DataAnnotations;

namespace Randa.Portal.Shared.Models
{
    public class AccessionText
    {
        public String AccessionId { get; set; }

        [Key]
        public Int32 TextId { get; set; }

        public String TextGuid { get; set; }
        public DateTime TextDate { get; set; }
        public String TextPhone { get; set; }
        public Boolean TextSuccess { get; set; }
        public String TextSid { get; set; }
        public String TextData { get; set; }
    }
}
