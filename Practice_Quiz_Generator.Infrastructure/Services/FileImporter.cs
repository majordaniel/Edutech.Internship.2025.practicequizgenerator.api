using Practice_Quiz_Generator.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;





namespace Practice_Quiz_Generator.Infrastructure.Services
{
    public class FileImporter
    {
        private readonly AppDbContext _db;
        public FileImporter(AppDbContext db) => _db = db;

        public async Task<(int total, int success, int failed, string error)> ProcessFileAsync(Stream fileStream, string fileName, string courseId, Guid jobId)
        {
            int total = 0, success = 0, failed = 0;
            string errorMessage = "";

            try
            {
                var rows = fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)
                    ? ParseCsv(fileStream)
                    : ParseExcel(fileStream);

                foreach (var row in rows)
                {
                    total++;
                    var (ok, msg) = await ValidateAndPersistRowAsync(row, courseId);
                    _db.ImportJobLogs.Add(new ImportJobLog
                    {
                        ImportJobId = jobId,
                        RowNumber = total,
                        IsSuccess = ok,
                        Message = msg
                    });

                    if (ok) success++;
                    else failed++;
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return (total, success, failed, errorMessage);
        }

        private List<Dictionary<string, string>> ParseCsv(Stream stream)
        {
            var rows = new List<Dictionary<string, string>>();
            using var reader = new StreamReader(stream);
            var header = reader.ReadLine()?.Split(',') ?? Array.Empty<string>();

            while (!reader.EndOfStream)
            {
                var values = reader.ReadLine()?.Split(',');
                if (values == null) continue;

                var row = new Dictionary<string, string>();
                for (int i = 0; i < header.Length && i < values.Length; i++)
                    row[header[i].Trim()] = values[i].Trim();

                rows.Add(row);
            }
            return rows;
        }

        private List<Dictionary<string, string>> ParseExcel(Stream stream)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            var rows = new List<Dictionary<string, string>>();
            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets[0];

            var header = new List<string>();
            for (int col = 1; col <= ws.Dimension.End.Column; col++)
                header.Add(ws.Cells[1, col].Text.Trim());

            for (int row = 2; row <= ws.Dimension.End.Row; row++)
            {
                var dict = new Dictionary<string, string>();
                for (int col = 1; col <= ws.Dimension.End.Column; col++)
                    dict[header[col - 1]] = ws.Cells[row, col].Text.Trim();
                rows.Add(dict);
            }
            return rows;
        }

        // ✅ UPDATED TO MATCH YOUR Question.cs
        private async Task<(bool ok, string message)> ValidateAndPersistRowAsync(IDictionary<string, string> row, string courseId)
        {
            string Get(IDictionary<string, string> d, params string[] keys)
            {
                foreach (var k in keys)
                    if (d.TryGetValue(k, out var v) && !string.IsNullOrWhiteSpace(v))
                        return v.Trim();
                return string.Empty;
            }

            var questionText = Get(row, "QuestionText", "Question", "Text");
            if (string.IsNullOrWhiteSpace(questionText))
                return (false, "QuestionText is required");

            var questionType = Get(row, "QuestionType", "Type");
            if (string.IsNullOrWhiteSpace(questionType))
                questionType = "MCQ";

            var correctAnswer = Get(row, "CorrectAnswer", "Answer", "Correct");
            var source = "BulkUpload";

            var options = new List<string>();
            foreach (var key in new[] { "OptionA", "OptionB", "OptionC", "OptionD", "A", "B", "C", "D", "Option1", "Option2", "Option3", "Option4" })
            {
                var val = Get(row, key);
                if (!string.IsNullOrWhiteSpace(val))
                    options.Add(val.Trim());
            }

            if (questionType.Equals("MCQ", StringComparison.OrdinalIgnoreCase))
            {
                if (options.Count < 2)
                    return (false, "MCQ must have at least 2 options");

                if (string.IsNullOrWhiteSpace(correctAnswer))
                    return (false, "CorrectAnswer required for MCQ");

                if (correctAnswer.Length == 1 && "ABCD".Contains(correctAnswer.ToUpper()))
                {
                    int index = correctAnswer.ToUpper()[0] - 'A';
                    if (index >= 0 && index < options.Count)
                        correctAnswer = options[index];
                }

                if (!options.Any(o => o.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase)))
                    return (false, "CorrectAnswer does not match any options");
            }

            var question = new Question
            {
                Text = questionText,
                QuestionType = questionType,
                Source = source,
                CourseId = courseId,
                Option = new List<Option>()
            };

            foreach (var opt in options)
            {
                question.Option.Add(new Option
                {
                    Text = opt,
                    IsCorrect = opt.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase)
                });
            }

            _db.Set<Question>().Add(question);
            await _db.SaveChangesAsync();

            return (true, "Imported");
        }
    }

}
