using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuestionOption
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public string QuestionId { get; set; }
        public Question Question { get; set; }
    }

}
