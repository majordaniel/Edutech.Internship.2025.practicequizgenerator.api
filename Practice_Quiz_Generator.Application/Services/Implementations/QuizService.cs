using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.Constants;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Text.Json;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly IGeminiService _geminiService;
        private readonly IUnitOfWork _unitOfWork;

        public QuizService(IGeminiService geminiService, IUnitOfWork unitOfWork)
        {
            _geminiService = geminiService;
            _unitOfWork = unitOfWork;
        }

        public async Task<StandardResponse<QuizResponseDto>> GenerateQuizAsync(QuizAndPersistRequestDto quizRequest)
        {//Reminder -->> Separate concerns base on question type and source
            try
            {
                if (quizRequest.NumberOfQuestions <= 5)
                {
                    return StandardResponse<QuizResponseDto>.Failed("Number of questions cannot be less than 5");
                }

                var course = await _unitOfWork.CourseRepository.FindCourseById(quizRequest.CourseId);
                var user = await _unitOfWork.UserRepository.FindUserById(quizRequest.UserId);

                if (course == null)
                {
                    return StandardResponse<QuizResponseDto>.Failed("Course not found");
                }

                if (user == null)
                {
                    return StandardResponse<QuizResponseDto>.Failed("User not found");
                }

                CreateQuizResponseDto quizResponse;

                var questionSource = quizRequest.QuestionSource.Replace(" ", string.Empty);

                if (questionSource.Equals("FileUpload", StringComparison.OrdinalIgnoreCase))
                {
                    if (quizRequest == null || string.IsNullOrWhiteSpace(quizRequest.UploadedText))
                    {
                        return StandardResponse<QuizResponseDto>.Failed("Uploaded text cannot be empty");
                    }

                    var prompt = PromptTemplates.GenerateFromUploadPrompt(
                        quizRequest.UploadedText,
                        quizRequest.NumberOfQuestions
                    );

                    var rawResponse = await _geminiService.GetLLMResponseAsync(prompt);
                    quizResponse = Parse(rawResponse);
                }
                else if (questionSource.Equals("QuestionBank", StringComparison.OrdinalIgnoreCase))
                {
                    var questions = await _unitOfWork.QuestionBankRepository
                        .FindRandomQuestionsByCourseId(quizRequest.CourseId, quizRequest.NumberOfQuestions);

                    if (questions == null || !questions.Any())
                        return StandardResponse<QuizResponseDto>.Failed("No questions found in question bank for this course.");

                    var questionBankText = string.Join("\n\n", questions.Select(q =>
                        $"Question: {q.Text}\nOptions: {string.Join(", ", q.Option.Select(o => o.OptionText))}\nCorrect Answer: {questions.FirstOrDefault()?.Option.FirstOrDefault(o => o.IsCorrect)?.OptionText}"
                    ));

                    var prompt = PromptTemplates.GenerateFromQuestionBankPrompt(
                        questionBankText,
                        quizRequest.NumberOfQuestions
                    );


                    var rawResponse = await _geminiService.GetLLMResponseAsync(prompt);
                    quizResponse = Parse(rawResponse);
                }
                else
                {
                    return StandardResponse<QuizResponseDto>.Failed("Unsupported question source");
                }

                if (quizResponse?.Questions == null || !quizResponse.Questions.Any())
                    return StandardResponse<QuizResponseDto>.Failed("Failed to generate questions from AI");

                var quiz = new Quiz
                {
                    Title = course.Title,
                    QuestionType = quizRequest.QuestionType,
                    QuestionSource = quizRequest.QuestionSource,
                    Timer = quizRequest.Timer, // Reminder -->> Set to minutes
                    CourseId = quizRequest.CourseId,
                    UserId = quizRequest.UserId,
                    QuizQuestion = quizResponse.Questions.Select(q => new QuizQuestion
                    {
                        QuestionText = q.Question,
                        QuizOption = q.Options.Select((opt, index) => new QuizOption
                        {
                            QuizOptionText = opt,
                            IsCorrect = index == q.CorrectOptionIndex
                        }).ToList()
                    }).ToList()
                };

                await _unitOfWork.QuizRepository.CreateAsync(quiz);
                await _unitOfWork.SaveChangesAsync();

                var quizDto = new QuizResponseDto
                {
                    QuizId = quiz.Id,
                    Title = quiz.Title,
                    QuestionType = quiz.QuestionType,
                    QuestionSource = quiz.QuestionSource,
                    Timer = quiz.Timer,
                    CourseId = quiz.CourseId,
                    UserId = quiz.UserId,
                    CreatedAt = quiz.DateCreated,
                    IsCompleted = quiz.IsCompleted,
                    TimeSpent = quiz.TimeSpent,
                    NumberOfQuestions = quiz.QuizQuestion.Count,
                    Questions = quiz.QuizQuestion.Select(q => new QuizQuestionResponseDto
                    {
                        QuestionText = q.QuestionText,
                        CorrectOptionIndex = q.QuizOption.ToList().FindIndex(opt => opt.IsCorrect),
                        Options = q.QuizOption.Select(o => new QuizOptionResponseDto
                        {
                            Text = o.QuizOptionText,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                    }).ToList()
                };

                return StandardResponse<QuizResponseDto>.Success("Quiz generated successfully", quizDto
                );
            }
            catch (Exception ex)
            {
                return StandardResponse<QuizResponseDto>.Failed($"{ex.Message}");
            }
        }
       

     /*   public async Task<StandardResponse<CreateQuizResponseDto>> GenerateQuizAsync(QuizRequestDto quizRequest)
        {//Reminder -->> Separate concerns base on question type and source
            try
            {
                if (quizRequest == null || string.IsNullOrWhiteSpace(quizRequest.UploadedText))
                {
                    return StandardResponse<CreateQuizResponseDto>.Failed("Uploaded text cannot be empty");
                }

                if (quizRequest.NumberOfQuestions <= 5)
                {
                    return StandardResponse<CreateQuizResponseDto>.Failed("Number of questions must be greater than 5");
                }
                var prompt = PromptTemplates.GenerateFromUploadPrompt(
                quizRequest.UploadedText,
                quizRequest.NumberOfQuestions
            );

                var rawResponse = await _geminiService.GetLLMResponseAsync(
                    prompt
                );

                var quizResponse = Parse(rawResponse);

                if (quizResponse.Questions == null || !quizResponse.Questions.Any())
                {
                    return StandardResponse<CreateQuizResponseDto>.Failed("Failed to generate questions from AI");
                }

                //var course = await _unitOfWork.CourseRepository.FindCourseById(quizRequest.CourseId);
                //var user = await _unitOfWork.UserRepository.FindUserById(quizRequest.UserId);

                //if (course == null)
                //{
                //    return StandardResponse<CreateQuizResponseDto>.Failed("Course not found");
                //}

                //if (user == null)
                //{
                //    return StandardResponse<CreateQuizResponseDto>.Failed("User not found");
                //}           //var quiz = new Quiz
                //{
                //    Title = course.Title,
                //    QuestionType = quizRequest.QuestionType,
                //    QuestionSource = quizRequest.QuestionSource,
                //    Timer = quizRequest.Timer, // Reminder -->> Set to minutes
                //    CourseId = quizRequest.CourseId,
                //    UserId = quizRequest.UserId,
                //    QuizQuestion = quizResponse.Questions.Select(q => new QuizQuestion
                //    {
                //        QuestionText = q.QuestionBank,
                //        QuizOption = q.Options.Select((opt, index) => new QuizOption
                //        {
                //            QuizOptionText = opt,
                //            IsCorrect = index == q.CorrectOptionIndex
                //        }).ToList()
                //    }).ToList()
                //};

                //await _unitOfWork.QuizRepository.CreateAsync(quiz);
                //await _unitOfWork.SaveChangesAsync();



                return StandardResponse<CreateQuizResponseDto>.Success("Quiz generated successfully",
                    quizResponse

                );
            }
            catch (Exception ex)
            {
                return StandardResponse<CreateQuizResponseDto>.Failed($"{ex.Message}");
            }
        } */



        //public async Task<StandardResponse<QuizResponseDto>> GetQuizByIdAsync(string id)
        //{
        //    var quiz = await _unitOfWork.QuizRepository.GetQuizWithQuestions(id);
        //    if (quiz == null)
        //        return StandardResponse<QuizResponseDto>.Failed("Quiz not found");
        //    int correctOptionIndex = 0;
        //    var quizToReturn = new QuizResponseDto
        //    {
        //        Id = quiz.Id,
        //        Title = quiz.Title,
        //        Questions = (quiz.QuizQuestion ?? new List<QuizQuestion>())
        //        .Select(q =>
        //        {
        //            int correctOptionIndex = -1;
        //            new QuizQuestionResponseDto
        //            {

        //                QuestionText = q.QuestionText,
        //                Options = (q.QuizOption ?? new List<QuizOption>())
        //                    .Select(o => new QuizOptionResponseDto
        //                    {
        //                        Text = o.QuizOptionText,
        //                        IsCorrect = o.IsCorrect
        //                    }).ToList(),
        //                CorrectOptionIndex = q.CorrectOptionIndex

        //            };
        //        }).ToList()

        //    };

        //    return StandardResponse<QuizResponseDto>.Success("", quizToReturn);
        //}

        public async Task<StandardResponse<QuizResponseDto>> GetQuizByIdAsync(string id)
        {
            var quiz = await _unitOfWork.QuizRepository.GetQuizWithQuestions(id);
            if (quiz == null)
                return StandardResponse<QuizResponseDto>.Failed("Quiz not found");

            var quizToReturn = new QuizResponseDto
            {
                QuizId = quiz.Id,
                Title = quiz.Title,
                QuestionType = quiz.QuestionType,
                QuestionSource = quiz.QuestionSource,
                Timer = quiz.Timer,
                CourseId = quiz.CourseId,
                UserId = quiz.UserId,
                CreatedAt = quiz.DateCreated,
                IsCompleted = quiz.IsCompleted,
                TimeSpent = quiz.TimeSpent,
                NumberOfQuestions = quiz.QuizQuestion.Count,
                TotalNumberOfQuiz = 1,
                Questions = (quiz.QuizQuestion ?? new List<QuizQuestion>())
                    .Select(q =>
                    {
                        int correctOptionIndex = -1;

                        var options = (q.QuizOption ?? new List<QuizOption>())
                            .Select((o, i) =>
                            {
                                if (o.IsCorrect)
                                    correctOptionIndex = i;

                                return new QuizOptionResponseDto
                                {
                                    Text = o.QuizOptionText,
                                    IsCorrect = o.IsCorrect
                                };
                            }).ToList();

                        return new QuizQuestionResponseDto
                        {
                            QuestionText = q.QuestionText,
                            Options = options,
                            CorrectOptionIndex = correctOptionIndex
                        };
                    }).ToList()
            };

            return StandardResponse<QuizResponseDto>.Success("", quizToReturn);
        }

        public async Task<StandardResponse<IEnumerable<QuizResponseDto>>> GetAllUserQuizzesAsync(string userId)
        {
            //Reminder-->> user exit edge case
            Console.WriteLine($"Incoming userId: '{userId}'");


            var quizzes = await _unitOfWork.QuizRepository.GetAllUserQuizzesWithQuestions(userId);

            var total = quizzes.Count();

            if (quizzes == null || !quizzes.Any())
                return StandardResponse<IEnumerable<QuizResponseDto>>.Failed("No quizzes found for this user");
            int count = 0;
            var result = quizzes.Select(q =>
                new QuizResponseDto
                {
                    QuizId = q.Id,
                    Title = q.Title,
                    QuestionType = q.QuestionType,
                    QuestionSource = q.QuestionSource,
                    Timer = q.Timer,
                    CourseId = q.CourseId,
                    UserId = q.UserId,
                    CreatedAt = q.DateCreated,
                    IsCompleted = q.IsCompleted,
                    TimeSpent = q.TimeSpent,
                    NumberOfQuestions = q.QuizQuestion.Count,
                    TotalNumberOfQuiz = total,
                    Questions = q.QuizQuestion.Select(question =>
                    {
                        int correctOptionIndex = -1;

                        var options = question.QuizOption.Select((o, i) =>
                        {
                            if (o.IsCorrect) correctOptionIndex = i;

                            return new QuizOptionResponseDto
                            {
                                Text = o.QuizOptionText,
                                IsCorrect = o.IsCorrect
                            };
                        }).ToList();

                        return new QuizQuestionResponseDto
                        {
                            QuestionText = question.QuestionText,
                            Options = options,
                            CorrectOptionIndex = correctOptionIndex
                        };
                    }).ToList()
                }).ToList();

            result.Select(q => q.TotalNumberOfQuiz = count);

            return StandardResponse<IEnumerable<QuizResponseDto>>.Success("Quizzes retrieved successfully", result);
        }


        public CreateQuizResponseDto Parse(string rawResponse)
        {
            try
            {
                rawResponse = rawResponse.Trim();
                if (rawResponse.StartsWith("```"))
                {
                    var firstLineEnd = rawResponse.IndexOf('\n');
                    rawResponse = rawResponse.Substring(firstLineEnd + 1);
                    rawResponse = rawResponse.Trim('`', '\n', '\r');
                }

                var quizResponse = JsonSerializer.Deserialize<CreateQuizResponseDto>(rawResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return quizResponse ?? new CreateQuizResponseDto { Questions = new List<QuizQuestionDto>() };
            }
            catch (JsonException)
            {
                return new CreateQuizResponseDto
                {
                    Questions = new List<QuizQuestionDto>
            {
                new QuizQuestionDto
                {
                    Question = "Failed to parse AI response",
                    Options = new List<string> { "QuizOption A", "QuizOption B", "QuizOption C", "QuizOption D" },
                    CorrectOptionIndex = 0
                }
            }
                };
            }
        }
    }
}


