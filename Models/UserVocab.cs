using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TgBotApi.Models
{
    public class UserVocab
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int ChatId { get; set; }
        public virtual ICollection<VocabItem> VocabItems { get; set; } 
    }
}
