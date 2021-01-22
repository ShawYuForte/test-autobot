using System.Collections.Generic;
using forte.models.accounts;

namespace forte.services
{
    public interface IBadgeDefinition
    {
        string Name { get; }

        string ImageName { get; }

        bool IsApplicableToActions(List<BadgeActionModel> actions);
    }
}
