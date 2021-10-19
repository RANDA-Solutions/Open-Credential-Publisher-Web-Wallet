using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Attributes
{
    public class SchemaAttribute: Attribute
    {
        public string Name { get; set; }
        public string Version { get; set; }

        public SchemaAttribute(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}
