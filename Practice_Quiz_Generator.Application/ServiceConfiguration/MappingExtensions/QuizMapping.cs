using Practice_Quiz_Generator.Shared.DTOs;
using Practice_Quiz_Generator.Shared.DTOs.Request;

namespace Practice_Quiz_Generator.Application.ServiceConfiguration.MappingExtensions
{
    public static class QuizMapping
    {
        public static QuizAndPersistRequestDto ToQuizPersistRequestDto(this QuizPersistUploadRequestDto quizUploadRequest)
        {
            return new QuizAndPersistRequestDto
            {
                NumberOfQuestions = quizUploadRequest.NumberOfQuestions,
                QuestionType = "MCQ",
                QuestionSource = quizUploadRequest.QuestionSource,
                Timer = quizUploadRequest.Timer,
                UserId = quizUploadRequest.UserId,
                CourseId = quizUploadRequest.CourseId,
            };
        }

        public static QuizRequestDto ToQuizRequestDto(this QuizUploadRequestDto quizUploadRequest)
        {
            return new QuizRequestDto
            {
                NumberOfQuestions = quizUploadRequest.NumberOfQuestions,
                QuestionType = "MCQ"
               
                //QuestionSource = "UCM",
                //Timer = quizUploadRequest.Timer,
                //UserId = quizUploadRequest.UserId,
                //CourseId = quizUploadRequest.CourseId,
            };
        }
    }
}
