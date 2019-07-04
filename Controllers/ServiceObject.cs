using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RollCallApp.Models;

namespace RollCallApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceObjectController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpGet]
        [Route("getListTeacherOfStudent/studentID")]
        //truyền vào 1 sinh viên --> lấy ra danh sách GV dạy SV đó
        public IActionResult getListTeacherOfStudent(string studentID)
        {
            //join Register Subject vs Profile Teacher
            var listDD =
            (from reg_subject in context.RegisterSubjects join teacher in context.Teachers
                 on reg_subject.TeacherId equals teacher.TeacherId

                 join profile_teacher in context.ProfileTeachers
                 on teacher.ProfileId equals profile_teacher.ProfileId

             where reg_subject.StudentId == studentID

             select profile_teacher
            ).Distinct().ToList();
            return Ok(listDD);
        }


        [HttpGet]
        [Route("getListSubjectOfTeacher/teacherID")]
        //truyền vào id GV --> lấy ra danh sách môn học mà GV đó dạy
        public IActionResult LoadListAttend(string teacherID)
        {
            var listDD =
            (from reg_subject in context.RegisterSubjects join subject in context.Subjects
                on reg_subject.SubjectId equals subject.SubjectId
                join teacher in context.Teachers on reg_subject.TeacherId equals teacher.TeacherId
                join profile_teacher in context.ProfileTeachers on teacher.ProfileId equals profile_teacher.ProfileId

             where profile_teacher.ProfileId == teacherID

             select subject
            ).Distinct().ToList();
            return Ok(listDD);
        }

        [HttpGet]
        [Route("getListSubjectOfTeacherID/teacherID")]
        //truyền vào id GV --> lấy ra danh sách môn học mà GV đó dạy
        public IActionResult getListSubjectOfTeacherID(string teacherID)
        {
            var listDD =
            (from reg_subject in context.RegisterSubjects join subject in context.Subjects
                     on reg_subject.SubjectId equals subject.SubjectId

             where reg_subject.TeacherId == teacherID

             select subject
            ).Distinct().ToList();
            return Ok(listDD);
        }

        [HttpGet]
        [Route("loadListStudentBySubjectAndTeacher/teacherID/subjectID")]
        //truyền vào gv, môn học --> lấy ra danh sách sv của gv dạy môn học đó
        public IActionResult LoadListAttend(string teacherID, string subjectID)
        {
            var listDD =
            (from reg_subject in context.RegisterSubjects join student in context.Students
             on reg_subject.StudentId equals student.StudentId

             join profile_student in context.ProfileStudents
             on student.ProfileId equals profile_student.ProfileId

             where reg_subject.TeacherId == teacherID
             where reg_subject.SubjectId == subjectID

             select profile_student
            ).Distinct().ToList();
            return Ok(listDD);
        }

        [HttpGet]
        [Route("viewAttendanceOfStudent/studentID/teacherID/subjectID")]
        //truyền vào student, teacher, subject --> lấy danh sách điểm danh
        public IActionResult viewAttendanceOfStudent(string studentID, string teacherID, string subjectID)
        {
            var listDD =
            (from reg_subject in context.RegisterSubjects join student in context.Students
                  on reg_subject.StudentId equals student.StudentId

                  join attendance in context.AttendanceRollCalls 
                  on reg_subject.RegisterId equals attendance.RegisterId

                  join profile_student in context.ProfileStudents
                  on student.PersonId equals profile_student.ProfileId

             where reg_subject.TeacherId == teacherID
             where reg_subject.SubjectId == subjectID
             where profile_student.ProfileId == studentID

             select new { profile_student, attendance.CheckAttendance, attendance.DateCheck }
            ).Distinct().ToList();
            return Ok(listDD);
        }
    }
}
