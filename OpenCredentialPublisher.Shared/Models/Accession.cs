using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Randa.Portal.Shared.Models
{
    public class Accession
    {
        [Key]
        public String AccessionId { get; set; }
        public DateTime AccessionCollected { get; set; }
        public DateTime? AccessionReceived { get; set; }
        public DateTime? AccessionReleased { get; set; }
        public String AccessionSpecimen { get; set; }
        public DateTime AccessionCreated { get; set; }

        public String PhysicianCode { get; set; }
        public String PhysicianName { get; set; }

        public String PatientId { get; set; }
        public String PatientFirst { get; set; }
        public String PatientLast { get; set; }
        public DateTime? PatientDob { get; set; }
        public String PatientSex { get; set; }
        public String PatientEthnicity { get; set; }
        public String PatientAddress1 { get; set; }
        public String PatientAddress2 { get; set; }
        public String PatientCity { get; set; }
        public String PatientState { get; set; }
        public String PatientZip { get; set; }
        public String PatientPhone { get; set; }

        public String ClientCode { get; set; }
        public String ClientName { get; set; }
        public String ClientAddress1 { get; set; }
        public String ClientAddress2 { get; set; }
        public String ClientCity { get; set; }
        public String ClientState { get; set; }
        public String ClientZip { get; set; }
        public String ClientPhone { get; set; }

        public List<AccessionTest> Tests { get; set; }
        public List<AccessionText> Texts { get; set; }

        public String PatientName => $"{PatientLast}, {PatientFirst}";

        public String PatientAddress
        {
            get
            {
                var sb = new StringBuilder();

                if (!String.IsNullOrWhiteSpace(PatientAddress1))
                {
                    sb.AppendLine(PatientAddress1);
                }
                if (!String.IsNullOrWhiteSpace(PatientAddress2))
                {
                    sb.AppendLine(PatientAddress2);
                }
                if (!String.IsNullOrWhiteSpace(PatientCity))
                {
                    sb.AppendLine(PatientCity);
                }
                if (!String.IsNullOrWhiteSpace(PatientState))
                {
                    sb.Append($", {PatientState}");

                }
                if (!String.IsNullOrWhiteSpace(PatientZip))
                {
                    sb.Append($" {PatientZip}");
                }

                return sb.ToString();
            }
        }
    }
}
