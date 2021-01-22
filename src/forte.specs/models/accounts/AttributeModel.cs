using System;
using System.Linq;

namespace Forte.Svc.Services.Models.Accounts
{
    public class AttributeModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public bool IsSet { get; set; }

        public bool IsList => (Options != null && Options.Any());

        public IdValueModel[] Options { get; set; }
    }
}
