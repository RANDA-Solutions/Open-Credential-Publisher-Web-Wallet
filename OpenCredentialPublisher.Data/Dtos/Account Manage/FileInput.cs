using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos.Account_Manage
{
    public class FileInput
    {
        public Guid? Guid { get; set; }
        public IFormFile File { get; set; }
        public Int32? AttachmentTypeId { get; set; }
        public String Description { get; set; }
        public String Url { get; set; }
        public String Name { get; set; }
    }
}
