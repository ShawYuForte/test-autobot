using System;

namespace Forte.Svc.Services.Models.Accounts
{
    /// <summary>
    /// The base public claim model.
    /// </summary>
    public class ClaimDefinitionModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
