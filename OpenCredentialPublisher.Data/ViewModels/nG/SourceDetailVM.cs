using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class SourceDetailVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SourceUrl { get; set; }
        public string SourceType{ get; set; }
        public bool SourceIsDeletable { get; set; }
        public List<SourceClrVM> Clrs { get; set; }

    }
}
