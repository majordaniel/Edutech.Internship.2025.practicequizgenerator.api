using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class TheoryGradingRequestDto
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public List<string> Keywords { get; set; }
    }
}
