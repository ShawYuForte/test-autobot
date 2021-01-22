using System;
using forte.models;

namespace Forte.Svc.Services.Models.Accounts
{
    /// <summary>
    /// The base public group of claims model.
    /// </summary>
    public class GroupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int UsersCount { get; set; }

        public GroupType Type { get; set; }
    }
}
