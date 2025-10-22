using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Practice_Quiz_Generator.Domain.Models;


namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class JobStatusDto
    {
        public Guid JobId { get; set; }
        public ImportJobStatus Status { get; set; }
        public int TotalRows { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public string? ErrorMessage { get; set; }
    }

}
