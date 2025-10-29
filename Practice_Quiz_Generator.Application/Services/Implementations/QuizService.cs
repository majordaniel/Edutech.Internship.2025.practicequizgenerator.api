using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.Constants;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Text.Json;
using System.Linq;
using System.Net;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly IGeminiService _geminiService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizAttemptRepository _quizAttemptRepository;
        private readonly ILogger<QuizService> _logger;

        public QuizService(IGeminiService geminiService, IUnitOfWork unitOfWork, IQuizRepository quizRepository, IQuizAttemptRepository quizAttemptRepository, ILogger<QuizService> logger)
        {
            _geminiService = geminiService;
            _unitOfWork = unitOfWork;
            _quizRepository = quizRepository;
            _quizAttemptRepository = quizAttemptRepository;
            _logger = logger;
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

        public async Task<StandardResponse<QuizResultsResponseDto>> QuizSubmitAsync(QuizSubmissionRequestDto request)
        {
            try
            {
                var quiz = await _unitOfWork.QuizRepository.GetQuizWithQuestions(request.QuizId);
                if (quiz == null)
                    return StandardResponse<QuizResultsResponseDto>.Failed("Quiz not found");

                var user = await _unitOfWork.UserRepository.FindUserById(request.UserId);
                if (user == null)
                    return StandardResponse<QuizResultsResponseDto>.Failed("User not found");

                _unitOfWork.UserRepository.AttachAsUnchanged(user);

                // Check if already attempted (optional)
                var existingAttempt = await _unitOfWork.QuizAttemptRepository.GetAttemptByQuizAndUserAsync(request.QuizId, request.UserId);
                if (existingAttempt != null)
                    return StandardResponse<QuizResultsResponseDto>.Failed("Quiz already completed");

                var totalQuestions = quiz.QuizQuestion.Count;
                var questionResults = new List<QuestionResultsDto>();
                var userResponses = new List<UserResponse>();
                int score = 0;

                foreach (var qq in quiz.QuizQuestion)
                {
                    var userAnswer = request.Answers.FirstOrDefault(a => a.QuizQuestionId == qq.Id);
                    if (userAnswer == null)
                        return StandardResponse<QuizResultsResponseDto>.Failed($"Missing answer for question {qq.Id}");

                    var options = qq.QuizOption.ToList();
                    var selectedIndex = userAnswer.SelectedOptionIndex;
                    if (selectedIndex < 0 || selectedIndex >= options.Count)
                        return StandardResponse<QuizResultsResponseDto>.Failed($"Invalid option index for question {qq.Id}");

                    var selectedOption = options[selectedIndex];
                    bool isCorrect = selectedOption.IsCorrect;

                    if (isCorrect) score++;

                    var correctOption = options.FirstOrDefault(o => o.IsCorrect);
                    string userText = selectedOption.QuizOptionText;
                    string correctText = correctOption?.QuizOptionText ?? "";

                    questionResults.Add(new QuestionResultsDto
                    {
                        QuizQuestionId = qq.Id,
                        QuestionText = qq.QuestionText,
                        IsCorrect = isCorrect,
                        UserAnswerText = userText,  // Always show what user selected
                        CorrectAnswerText = !isCorrect ? correctText : null  // Show correct answer only if wrong
                    });

                    userResponses.Add(new UserResponse
                    {
                        IsCorrect = isCorrect,
                        QuizId = quiz.Id,
                        QuizQuestionId = qq.Id,
                        SelectedOptionId = selectedOption.Id,
                        UserId = request.UserId,
                        Quiz = quiz,
                        QuizQuestion = qq,
                        SelectedOption = selectedOption,
                        User = user
                    });
                }

                double percentage = totalQuestions > 0 ? (double)score / totalQuestions * 100 : 0;

                var attempt = new QuizAttempt
                {
                    QuizId = quiz.Id,
                    UserId = request.UserId,
                    Score = score,
                    TimeSpent = request.TimeSpent,
                    Answer = JsonSerializer.Serialize(request.Answers),  // Serialize for storage
                    Quiz = quiz,
                    User = user
                };

                await _unitOfWork.QuizAttemptRepository.CreateAsync(attempt);
                await _unitOfWork.UserResponseRepository.CreateBulkAsync(userResponses);
                await _unitOfWork.SaveChangesAsync();

                // Update quiz as completed (optional)
                quiz.IsCompleted = true;
                quiz.TimeSpent = request.TimeSpent;
                await _unitOfWork.SaveChangesAsync();

                var results = new QuizResultsResponseDto
                {
                    Score = score,
                    Percentage = percentage,
                    TotalQuestions = totalQuestions,
                    TimeSpent = request.TimeSpent,
                    QuestionResults = questionResults
                };

                return StandardResponse<QuizResultsResponseDto>.Success("Quiz submitted successfully", results);
            }
            catch (Exception ex)
            {
                return StandardResponse<QuizResultsResponseDto>.Failed(ex.Message);
            }
        }

        public async Task<StandardResponse<QuizResultsResponseDto>> GetQuizResultsAsync(string quizId, string userId)
        {
            try
            {
                var attempt = await _unitOfWork.QuizAttemptRepository.GetAttemptByQuizAndUserAsync(quizId, userId);
                if (attempt == null)
                    return StandardResponse<QuizResultsResponseDto>.Failed("No results found for this quiz and user");

                var responses = await _unitOfWork.UserResponseRepository.GetResponsesByQuizAndUserAsync(quizId, userId);
                if (responses == null || !responses.Any())
                    return StandardResponse<QuizResultsResponseDto>.Failed("No detailed responses found");

                var quiz = await _unitOfWork.QuizRepository.GetQuizWithQuestions(quizId);
                if (quiz == null)
                    return StandardResponse<QuizResultsResponseDto>.Failed("Quiz not found");

                var totalQuestions = quiz.QuizQuestion.Count;
                var questionResults = new List<QuestionResultsDto>();

                foreach (var ur in responses)
                {
                    var qq = ur.QuizQuestion;
                    var correctOption = qq.QuizOption.FirstOrDefault(o => o.IsCorrect);
                    string userText = ur.SelectedOption?.QuizOptionText ?? "";
                    string correctText = correctOption?.QuizOptionText ?? "";

                    questionResults.Add(new QuestionResultsDto
                    {
                        QuizQuestionId = qq.Id,
                        QuestionText = qq.QuestionText,
                        IsCorrect = ur.IsCorrect,
                        UserAnswerText = userText,  // Always show what user selected
                        CorrectAnswerText = !ur.IsCorrect ? correctText : null  // Show correct answer only if wrong
                    });
                }

                double percentage = totalQuestions > 0 ? (double)attempt.Score / totalQuestions * 100 : 0;

                var results = new QuizResultsResponseDto
                {
                    Score = attempt.Score,
                    Percentage = percentage,
                    TotalQuestions = totalQuestions,
                    TimeSpent = attempt.TimeSpent,
                    QuestionResults = questionResults
                };

                return StandardResponse<QuizResultsResponseDto>.Success("Results retrieved successfully", results);
            }
            catch (Exception ex)
            {
                return StandardResponse<QuizResultsResponseDto>.Failed(ex.Message);
            }
        }
        public async Task<StandardResponse<QuizDetailsResponseDto>> GetQuizDetailsAsync(string quizId, string userId)
        {
            try
            {
                var quiz = await _quizRepository.GetQuizWithQuestions(quizId);

                if (quiz == null)
                {
                    return StandardResponse<QuizDetailsResponseDto>.Failed(
                        "Quiz not found",
                        (int)HttpStatusCode.NotFound);
                }

                var response = new QuizDetailsResponseDto
                {
                    QuizId = quiz.Id,
                    Title = $"Quiz {quiz.Id}",
                    TotalQuestions = quiz.QuizQuestion?.Count ?? 0,
                    TimeLimitInMinutes = 30, // Default time limit
                    Questions = quiz.QuizQuestion?.Select(q => new QuestionResponseDto
                    {
                        QuestionId = q.Id,
                        QuestionText = q.QuestionText,
                        Options = q.QuizOption != null
                            ? q.QuizOption.Select(o => o.QuizOptionText).ToList()
                            : new List<string>()
                    }).ToList() ?? new List<QuestionResponseDto>()
                };

                return StandardResponse<QuizDetailsResponseDto>.Success(
                    
                    "Quiz details retrieved successfully", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving quiz details for quiz {QuizId}", quizId);
                return StandardResponse<QuizDetailsResponseDto>.Failed(
                    "An error occurred while retrieving quiz details",
                    (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<StandardResponse<QuizResultResponseDto>> SubmitQuizAsync(SubmitQuizRequestDto request, string userId)
        {
            try
            {
                // Get quiz with questions
                var quiz = await _quizRepository.GetQuizWithQuestions(request.QuizId);

                if (quiz == null)
                {
                    return StandardResponse<QuizResultResponseDto>.Failed(
                        "Quiz not found",
                        (int)HttpStatusCode.NotFound);
                }

                // Calculate score
                int correctAnswers = 0;
                var questionResults = new List<QuestionResultDto>();

                foreach (var answer in request.Answers)
                {
                    var question = quiz.QuizQuestion?.FirstOrDefault(q => q.Id == answer.QuestionId);
                    if (question != null)
                    {

                        var options = question.QuizOption?.Select(o => o.QuizOptionText).ToList() ?? new List<string>();

                        string correctAnswer = (question.CorrectOptionIndex >= 0 && question.CorrectOptionIndex < options.Count)
                            ? options[question.CorrectOptionIndex]
                            : string.Empty;

                        bool isCorrect = correctAnswer.Equals(answer.SelectedAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);

                        if (isCorrect) correctAnswers++;

                        questionResults.Add(new QuestionResultDto
                        {
                            QuestionId = question.Id,
                            QuestionText = question.QuestionText,
                            SelectedAnswer = answer.SelectedAnswer,
                            CorrectOptionIndex = question.CorrectOptionIndex,
                            IsCorrect = isCorrect
                        });
                    }
                }

                // Create quiz attempt
                var quizAttempt = new QuizAttempt
                {
                    QuizId = request.QuizId,
                    UserId = userId, 
                    Score = correctAnswers,
                    Quiz = quiz,  
                    AttemptDate = DateTime.UtcNow,
                    TimeSpent = request.TimeSpentInSeconds,
                    Answer = JsonSerializer.Serialize(request.Answers)
                };

                
                //_unitOfWork.Context.Entry(quizAttempt).Property("UserId1").CurrentValue = userId;

                await _quizAttemptRepository.CreateAsync(quizAttempt);
                await _unitOfWork.SaveChangesAsync();

                var totalQuestions = quiz.QuizQuestion?.Count ?? 0;
                var percentageScore = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0;

                var response = new QuizResultResponseDto
                {
                    AttemptId = quizAttempt.Id,
                    QuizId = quiz.Id,
                    Score = correctAnswers,
                    TotalQuestions = totalQuestions,
                    PercentageScore = Math.Round(percentageScore, 2),
                    TimeSpentInSeconds = request.TimeSpentInSeconds,
                    AttemptDate = quizAttempt.AttemptDate,
                    QuestionResults = questionResults
                };

                return StandardResponse<QuizResultResponseDto>.Success(
                    
                    "Quiz submitted successfully", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting quiz for user {UserId}", userId);
                return StandardResponse<QuizResultResponseDto>.Failed(
                    "An error occurred while submitting the quiz",
                    (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<StandardResponse<List<QuizResultResponseDto>>> GetQuizHistoryAsync(string userId)
        {
            try
            {
                var attempts = await _quizAttemptRepository.GetAttemptsByUserIdAsync(userId);

                var response = new List<QuizResultResponseDto>();

                foreach (var attempt in attempts)
                {
                    var quiz = await _quizRepository.GetQuizWithQuestions(attempt.QuizId);
                    var totalQuestions = quiz?.QuizQuestion?.Count ?? 0;
                    var percentageScore = totalQuestions > 0 ? (double)attempt.Score / totalQuestions * 100 : 0;

                    response.Add(new QuizResultResponseDto
                    {
                        AttemptId = attempt.Id,
                        QuizId = attempt.QuizId,
                        Score = attempt.Score,
                        TotalQuestions = totalQuestions,
                        PercentageScore = Math.Round(percentageScore, 2),
                        TimeSpentInSeconds = attempt.TimeSpent,
                        AttemptDate = attempt.AttemptDate,
                        QuestionResults = new List<QuestionResultDto>() // Empty for history list
                    });
                }

                return StandardResponse<List<QuizResultResponseDto>>.Success(
                    
                    "Quiz history retrieved successfully", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving quiz history for user {UserId}", userId);
                return StandardResponse<List<QuizResultResponseDto>>.Failed(
                    "An error occurred while retrieving quiz history",
                    (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<StandardResponse<QuizUserStatsDto>> GetUserQuizStatsAsync(string userId)
        {
            try
            {
                // 1. Get all attempts for the user (including the quiz total-questions)
                var attemptsQuery = await _unitOfWork.QuizAttemptRepository
                    .FindByCondition(a => a.UserId == userId);

                var attempts = await attemptsQuery
                    .Include(a => a.Quiz)
                    .OrderByDescending(a => a.DateCreated)   // newest first
                    .ToListAsync();

                if (!attempts.Any())
                    return StandardResponse<QuizUserStatsDto>.Success(
                        "No quiz attempts found", new QuizUserStatsDto());

                // 2. Compute stats
                int totalQuizzes = attempts.Count;

                // Average score = Σ(score / totalQuestions) / count
                double avgScore = attempts.Average(a =>
                {
                    int totalQ = a.Quiz?.QuizQuestion?.Count ?? 1; // safety
                    return (double)a.Score / totalQ * 100;
                });

                // Last quiz score (most recent attempt)
                var lastAttempt = attempts.First();
                int lastTotalQ = lastAttempt.Quiz?.QuizQuestion?.Count ?? 1;
                double lastScore = (double)lastAttempt.Score / lastTotalQ * 100;

                // Average time spent
                double avgTime = attempts.Average(a => a.TimeSpent);

                var stats = new QuizUserStatsDto
                {
                    TotalQuizzes      = totalQuizzes,
                    AverageScore      = Math.Round(avgScore, 2),
                    LastQuizScore     = Math.Round(lastScore, 2),
                    AverageTimeSpent  = Math.Round(avgTime, 2)
                };

                return StandardResponse<QuizUserStatsDto>.Success("Stats retrieved", stats);
            }
            catch (Exception ex)
            {
                return StandardResponse<QuizUserStatsDto>.Failed(ex.Message);
            }
        }

    }
}


