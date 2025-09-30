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
        public string? QuestionType { get; set; }
        public int NumberOfQuestions { get; set; }
        public IFormFile File { get; set; }
    }
}
