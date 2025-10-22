using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class ImportJobLog
    {
        public int Id { get; set; }
        public Guid ImportJobId { get; set; }
        public ImportJob ImportJob { get; set; }
        public int RowNumber { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

}
