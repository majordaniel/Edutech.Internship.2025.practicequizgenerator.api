using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class ImportJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string AdminUserId { get; set; }
        public string CourseId { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ImportJobStatus Status { get; set; } = ImportJobStatus.Pending;
        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public string? ErrorMessage { get; set; }

        public ICollection<ImportJobLog> Logs { get; set; }
    }

}
