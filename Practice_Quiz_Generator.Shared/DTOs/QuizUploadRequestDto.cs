using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Shared.DTOs
{
    public class QuizUploadRequestDto
    {
        public string CourseId { get; set; }
        public string? QuestionType { get; set; }
        public int NumberOfQuestions { get; set; } //Reminder -->> Add fluent validation
        public string QuestionSource { get; set; }
        public int Timer { get; set; }
        public IFormFile File { get; set; }
        public string UserId { get; set; }

    }
}
