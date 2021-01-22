using System;
using forte.models.commands;
using forte.models.events;

namespace Forte.Domains.Commands.Models
{
    public class CommandEvenelope
    {
        public Command Command { get; set; }

        public DateTime? ScheduledTime { get; set; }

        public EventModel TriggerOnEvent { get; set; }
    }
}
