using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RollCallApp.Models
{
    public class AttendanceRollCall
    {
        public string Id { get; set; }
        public string RegisterId { get; set; }
        public string DateCheck { get; set; }
        public string CheckAttendance { get; set; }
    }
}
