using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Models
{
    public class Option
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Option(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
