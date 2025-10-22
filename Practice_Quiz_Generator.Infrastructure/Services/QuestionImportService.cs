using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;


namespace Practice_Quiz_Generator.Infrastructure.Services
{   
        public class QuestionImportService : IQuestionImportService
        {
            private readonly AppDbContext _db;
            private readonly FileImporter _importer;

            public QuestionImportService(AppDbContext db)
            {
                _db = db;
                _importer = new FileImporter(db);
            }

            public async Task<Guid> StartImportJobAsync(IFormFile file, string courseId, string adminUserId)
            {
                var job = new ImportJob
                {
                    AdminUserId = adminUserId,
                    CourseId = courseId,
                    FileName = file.FileName,
                    Status = ImportJobStatus.Pending
                };

                _db.ImportJobs.Add(job);
                await _db.SaveChangesAsync();

                // process asynchronously
                _ = Task.Run(async () =>
                {
                    job.Status = ImportJobStatus.Processing;
                    await _db.SaveChangesAsync();

                    try
                    {
                        using var stream = new MemoryStream();
                        await file.CopyToAsync(stream);
                        stream.Position = 0;

                        var (total, success, failed, errorMsg) = await _importer.ProcessFileAsync(stream, file.FileName, courseId, job.Id);

                        job.TotalRows = total;
                        job.SuccessCount = success;
                        job.FailedCount = failed;
                        job.Status = ImportJobStatus.Completed;
                        job.ErrorMessage = errorMsg;
                    }
                    catch (Exception ex)
                    {
                        job.Status = ImportJobStatus.Failed;
                        job.ErrorMessage = ex.Message;
                    }

                    await _db.SaveChangesAsync();
                });

                return job.Id;
            }

            public async Task<object> GetJobStatusAsync(Guid jobId)
            {
                var job = await _db.ImportJobs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == jobId);
                if (job == null) return null;

                return new
                {
                    job.Id,
                    job.Status,
                    job.TotalRows,
                    job.SuccessCount,
                    job.FailedCount,
                    job.ErrorMessage
                };
            }
        }   
}
