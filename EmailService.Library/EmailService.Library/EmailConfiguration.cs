using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Library
{
    public class EmailConfiguration
    {
        public Boolean ByPass { get; set; } = true;
        public string? SmtpServerAddress { get; set; } = "";
        public int SmtpServerPort { get; set; } = 587;
        public string? SmtpServerUserId { get; set; } = "";
        public string? SmtpServerPassword { get; set; } = "";
        public string? EmailFromAddress { get; set; } = "";
        public string? DisplayName { get; set; } = "";
    }
}
