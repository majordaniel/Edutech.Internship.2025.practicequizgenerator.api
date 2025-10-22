using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class BulkUploadRequestDto
    {
        public string CourseId { get; set; }
        public IFormFile File { get; set; }
    }

}
