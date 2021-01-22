using System;
using System.Collections.Generic;
using System.Linq;
using forte.models.classes;
using Forte.Svc.Services.Models.Accounts;

namespace forte.models.accounts
{
    /// <summary>
    ///     The extended editable user model.
    /// </summary>
    public class UserExModel : UserModel
    {
        public UserExModel()
        {
            Attributes = new List<AttributeModel>();
            Groups = new List<GroupModel>();
            ClassTypes = new List<ClassTypeModel>();
        }

        public string AgeGroup { get; set; }

        [Obsolete("Users are no longer restricted, they can invite as many people as they want")]
        public int AllowedInvitations { get; set; }

        public List<AttributeModel> Attributes { get; set; }

        public string City { get; set; }

        public string FitnessLevel { get; set; }

        public string Gender { get; set; }

        public List<GroupModel> Groups { get; set; }

        public AccountLockoutReasons LockoutReason { get; set; }

        public double ProfileCompleteness
        {
            get
            {
                // Calculate profile completeness as vale between 0..1

                var staticFieldsValues = new List<string> { FirstName, LastName };
                if (ClassTypes.Any())
                {
                    staticFieldsValues.Add(nameof(ClassTypes));
                }
                var dynamicFieldsValues = Attributes.Select(x => x.Value).ToArray();
                var allFieldsCount = dynamicFieldsValues.Length + staticFieldsValues.Count;
                var setFieldsCount = staticFieldsValues.Count(x => !string.IsNullOrEmpty(x)) +
                                     dynamicFieldsValues.Count(x => !string.IsNullOrEmpty(x));

                // Return value between 0..1 with 2 decimal places after the dot.
                var percent = allFieldsCount > 0
                    ? (double)setFieldsCount / allFieldsCount
                    : 0;

                var completeness = System.Math.Round(percent, 2);

                return completeness;
            }
        }

        public string RegistrationCode { get; set; }

        public string State { get; set; }

        public byte[] Version { get; set; }

        public string ZipCode { get; set; }

        public List<ClassTypeModel> ClassTypes { get; set; }

        public bool HasStravaAccount { get; set; }

        public bool? StravaAutoPosting { get; set; }

        public string StravaFullName { get; set; }

        public string StravaAvatarUrl { get; set; }

        public string Nickname { get; set; }
    }
}
