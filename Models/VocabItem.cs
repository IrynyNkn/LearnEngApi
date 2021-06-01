using System.ComponentModel.DataAnnotations;

namespace TgBotApi.Models
{
    public class VocabItem
    {
        [Key]
        public int VocabItemId { get; set; } 

        [Required]
        public string EnglishWord { get; set; }
        public string Translation { get; set; }
        public string Explanation { get; set; }

        public int UserVocabId { get; set; }
        public virtual UserVocab UserVocab { get; set; }
    }
}