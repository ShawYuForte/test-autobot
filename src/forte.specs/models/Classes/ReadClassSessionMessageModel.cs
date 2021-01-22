using System;
using System.ComponentModel.DataAnnotations;

namespace forte.models.classes
{
    public class ReadClassSessionMessageModel
    {
        [Required]
        public Guid ClassSessionMessageId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
