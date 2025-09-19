using AutoMapper;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
// using Practice_Quiz_Generator.Shared.DTOs;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class QuizSubmissionService : IQuizSubmissionService
    {
        private readonly IQuizAttemptRepository _attemptRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuizSubmissionService(
            IQuizAttemptRepository attemptRepository, 
            IQuestionRepository questionRepository, 
            IMapper mapper)
        {
            _attemptRepository = attemptRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<QuizResultResponseDto> SubmitAndGradeAsync(QuizSubmissionDto submissionDto)
        {
            var questions = await _questionRepository.GetQuestionsWithAnswersByQuizIdAsync(submissionDto.QuizId);

            var attempt = new QuizAttempt
            {
                QuizId = submissionDto.QuizId.ToString(),
                StudentId = submissionDto.StudentId,
                TotalQuestions = questions.Count,
                Answers = new List<QuizAttemptAnswer>()
            };

            int score = 0;
            var resultResponse = new QuizResultResponseDto
            {
                QuizId = submissionDto.QuizId,
                StudentId = submissionDto.StudentId,
                TotalQuestions = questions.Count,
            };

            // Grading Logic
            foreach (var submittedAnswer in submissionDto.Answers)
            {
                var question = questions.FirstOrDefault(q => q.Id == submittedAnswer.QuestionId);
                if (question == null) continue;

                var correctAnswer = question.Options.FirstOrDefault(o => o.IsCorrect);

                bool isCorrect = submittedAnswer.SelectedOptionId.HasValue && submittedAnswer.SelectedOptionId.Value == correctAnswer?.Id;

                if (isCorrect) score++;

                resultResponse.QuestionResults.Add(new QuestionResultDto
                {
                    QuestionId = question.Id,
                    QuestionText = question.QuestionText,
                    IsCorrect = isCorrect,
                    StudentAnswerText = question.Options.FirstOrDefault(o => o.Id == submittedAnswer.SelectedOptionId)?.Text,
                    CorrectAnswerText = correctAnswer?.Text
                });

                attempt.Answers.Add(new QuizAttemptAnswer
                {
                    QuestionId = submittedAnswer.QuestionId,
                    SelectedOptionId = submittedAnswer.SelectedOptionId,
                    // SelectedAnswerText = question.Options.FirstOrDefault(o => o.Id == submittedAnswer.SelectedOptionId)?.Text,
                    IsCorrect = isCorrect
                });
            }

            attempt.Score = score;
            resultResponse.Score = score;
            resultResponse.Percentage = (decimal)score / questions.Count * 100;

            await _attemptRepository.SaveAttemptAsync(attempt);

            return resultResponse;
        }

        private int GradeQuiz(QuizSubmissionDto submissionDto)
        {
            // Implement your grading logic here
            // For demonstration, returning a fixed score
            return 100;
        }
    }
}
