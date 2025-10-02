using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class JwtSettingsConfiguration
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }

    }
}
