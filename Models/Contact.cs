using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollCallApp.Models
{
    public class Contact
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string StudentId { get; set; }
        public string SubjectId { get; set; }
    }
}
