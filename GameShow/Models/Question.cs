using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameShow.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Ques { get; set; } = string.Empty;
        [Required]
        public string AnswerA { get; set; } = string.Empty;
        [Required]
        public string AnswerB { get; set; } = string.Empty;
        [Required]
        public string AnswerC { get; set; } = string.Empty;
        [Required]
        public string AnswerD { get; set; } = string.Empty;
        [Required]
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}
