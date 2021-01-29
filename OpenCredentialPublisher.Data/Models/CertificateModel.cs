using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OpenCredentialPublisher.Data.Models
{
    public class CertificateModel
    {
        [Key]
        public string Host { get; set; }

        public string IssuedByName { get; set; }

        public string IssuedToName { get; set; }

        public string ToSubjectHtml()
        {
            return Format(IssuedToName);
        }

        private static string Format(string name)
        {
            var sb = new StringBuilder();

            var dictionary = new Dictionary<string, string>(
                name.Split(",").Select(n =>
                    new KeyValuePair<string, string>(n.Split("=")[0].Trim(), n.Split("=")[1].Trim())));

            if (dictionary.ContainsKey("O"))
                sb.Append($"<div>{dictionary["O"]}");

            if (dictionary.ContainsKey("L"))
            {
                sb.Append($"<div>{dictionary["L"]}");
                if (dictionary.ContainsKey("S"))
                    sb.Append($", {dictionary["S"]}");
                sb.Append("</div>");
            }

            if (dictionary.ContainsKey("C"))
                sb.Append($"<div>{dictionary["C"]}</div>");

            if (sb.Length == 0 && dictionary.ContainsKey("CN"))
                sb.Append($"<div>{dictionary["CN"]}</div>");

            return sb.ToString();
        }
    }
}
