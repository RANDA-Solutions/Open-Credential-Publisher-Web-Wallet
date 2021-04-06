using System;

namespace Randa.Portal.Shared.Models
{
    public class AccessionTest
    {
        public String AccessionId { get; set; }
        public String ClientCode { get; set; }

        public String TestOrdered { get; set; }
        public String TestPerformed { get; set; }

        public String TestName { get; set; }
        public String TestResult { get; set; }
        public DateTime TestCreated { get; set; }
    }
}
