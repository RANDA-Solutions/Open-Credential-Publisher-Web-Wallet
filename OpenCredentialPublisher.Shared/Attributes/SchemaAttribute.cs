using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Shared.Attributes
{
    public class SchemaAttribute: Attribute
    {
        public string Name { get; set; }
        public SchemaAttribute(string name)
        {
            Name = name;
        }
    }
}
