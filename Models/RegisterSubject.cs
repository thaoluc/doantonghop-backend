using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollCallApp.Models
{
    public class RegisterSubject
    {
        public string RegisterId { get; set; }
        public string StudentId { get; set; }
        public string TeacherId { get; set; }
        public string SubjectId { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
    }
}
