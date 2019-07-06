using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RollCallApp.Models;

namespace RollCallApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AttendanceRollCallsController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPost]
        [Route("AddAttendanceRollCall")]
        public IActionResult CreateAttendanceRollCall ([FromBody] AttendanceRollCall attendanceRollCall)
        {
            try
            {
                attendanceRollCall.Id = Guid.NewGuid() + "";
                context.AttendanceRollCalls.Add(attendanceRollCall);
                context.SaveChanges();

                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }

        }

        [HttpGet]
        [Route("CheckAttendanceForStudentNoCheck")]
        public IActionResult CheckAttendanceForStudentNoCheck (string teacherID, string subjectID)
        {
            //lấy toàn bộ profile sinh viên theo 1 teacher dạy 1 môn học  
            var listAllStudent = (from registersubject in context.RegisterSubjects join student in context.Students
                                        on registersubject.StudentId equals student.StudentId

                                      join profile_student in context.ProfileStudents
                                       on student.ProfileId equals profile_student.ProfileId
                                       
                                where registersubject.TeacherId==teacherID
                                where registersubject.SubjectId==subjectID

                                select profile_student.ProfileId ).Distinct().ToList();

            //lấy danh sách sv đã điểm danh theo 1 teacher dạy 1 môn học tại 1 thời điểm
            var listStudentRollCall = (from attendance in context.AttendanceRollCalls
                                           join register_subject in context.RegisterSubjects
                                            on attendance.RegisterId equals register_subject.RegisterId

                                           join student in context.Students
                                           on register_subject.StudentId equals student.StudentId

                                           join profile_student in context.ProfileStudents
                                            on student.ProfileId equals profile_student.ProfileId

                                       where register_subject.SubjectId == subjectID
                                       where register_subject.TeacherId == teacherID
                                       where attendance.DateCheck == DateTime.Now.ToString("dd/MM/yyyy") //DateTime được set thông tin ngày tháng thời gian hiện tại theo máy tính địa phương.
                                       where attendance.CheckAttendance == "1"

                                       select profile_student.ProfileId).ToList();

            //
            ArrayList arrListAllStudent = new ArrayList();
            foreach (var item in listAllStudent)
            {
                arrListAllStudent.Add(item);
            }

            //
            ArrayList arrListStudentRollCall = new ArrayList();
            foreach (var item in listStudentRollCall)
            {
                arrListStudentRollCall.Add(item);
            }

            //danh sách sinh viên k điểm danh trong list tất cả sv (của 1 môn học, 1 gv)
            ArrayList arrListStudentIdDontCheck = new ArrayList();
            foreach(Object obj in arrListAllStudent){   //duyệt sv trong toàn bộ danh sách
                if (!arrListStudentRollCall.Contains(obj)) //sv k có trong danh sách sv
                {
                    arrListStudentIdDontCheck.Add(obj); //add vào danh sách chưa điểm danh
                }
            }

            // danh sách sv chưa điểm danh theo môn học và giáo viên
            foreach (Object item in arrListStudentIdDontCheck)
            {  //duyệt ds sv chưa điểm danh

                //obj: id của sv chưa điểm danh && subjectID truyền vào && teacherid truyền vào
                var obj = context.RegisterSubjects.Where(s => s.StudentId.Equals(item) 
                      && s.SubjectId.Equals(subjectID)
                      && s.TeacherId.Equals(teacherID)
                ).ToList();

                if (obj.Count > 0) //nếu có sv chưa điểm danh (theo môn học và gv đó)
                {
                    AttendanceRollCall attendance = new AttendanceRollCall(); //
                    attendance.Id=Guid.NewGuid()+"";
                    attendance.RegisterId = obj[0].RegisterId;
                    attendance.CheckAttendance = "0";
                    attendance.DateCheck = DateTime.Now.ToString("dd/MM/yyyy");
                    // at.DateCheck = "07/01/2019";
                    try
                    {
                        context.AttendanceRollCalls.Add(attendance); //lưu ds sv k điểm danh
                        context.SaveChanges();
                    }
                    catch (System.Exception)
                    {
                        return Ok(false);
                    }

                }

            }

            // filter Student dont check
            foreach (Object studentid in arrListStudentIdDontCheck) //duyệt sv k điểm danh
            { 
                //lấy id đk môn học theo từng sv chưa điểm danh
                //id đăng kí môn học: theo id teacher, id subject, id sv chưa điểm danh
                var registerID = //lấy id đăng kí môn học trong registersubject với teacherid, subjectid truyền vào và student trong list sv k điểm danh
                    (from reg in context.RegisterSubjects
                        
                     where reg.TeacherId == teacherID
                     where reg.SubjectId == subjectID
                     where reg.StudentId == studentid.ToString()
                     select reg.RegisterId
                    ).Distinct().ToList();

                //danh sách điểm danh theo sv (với môn học có sv chưa điểm danh đó) ?? cái này làm gì
                var listAllAttendanceByStudent =
                    (from at in context.AttendanceRollCalls
                     where at.DateCheck == DateTime.Now.ToString("dd/MM/yyyy")
                     where at.RegisterId == registerID[0]//tại sao lấy môn học đầu
                     select at
                    ).Distinct().ToList();

                string check = "0";

                foreach (var item in listAllAttendanceByStudent)//duyệt danh sách điểm danh theo sv (với môn học có sv chưa điểm danh) 
                {
                    if (!item.CheckAttendance.Equals("0"))// sv có điểm danh
                    {
                        check = "1";
                    }
                    context.AttendanceRollCalls.Remove(context.AttendanceRollCalls.Find(item.Id)); //remove item(id) trong danh sách điểm danh theo sv (chưa điểm danh)
                    context.SaveChanges();
                }

                foreach (var item in listAllAttendanceByStudent)//duyệt danh sách điểm danh theo sv (với môn học có sv chưa điểm danh)
                {
                    item.Id = Guid.NewGuid() + "";
                    item.CheckAttendance = check;
                    context.AttendanceRollCalls.Add(item);
                    context.SaveChanges();
                    break;
                }
            }

            //filter Student check
            foreach (Object p_studentid in arrListStudentRollCall) //duyệt sv điểm danh
            {
                //lấy id đk môn học theo từng sv đã điểm danh
                //id đăng kí môn học: theo id teacher, id subject, id profile sv điểm danh
                var registerID = //lấy id đăng kí môn học trong registersubject với teacherid, subjectid truyền vào và student trong list sv k điểm danh
                    (from reg in context.RegisterSubjects

                     where reg.TeacherId == teacherID
                     where reg.SubjectId == subjectID
                     where reg.StudentId == p_studentid.ToString()
                     select reg.RegisterId
                    ).Distinct().ToList();

                //danh sách điểm danh theo sv (với môn học có sv đã điểm danh) ?? cái này làm gì
                var listAllAttendanceByStudent =
                    (from at in context.AttendanceRollCalls
                     where at.DateCheck == DateTime.Now.ToString("dd/MM/yyyy")
                     where at.RegisterId == registerID[0]//tại sao lấy môn học đầu
                     select at
                    ).Distinct().ToList();

                string check = "0";

                foreach (var item in listAllAttendanceByStudent)//duyệt danh sách điểm danh theo sv (với môn học có sv chưa điểm danh) 
                {
                    if (!item.CheckAttendance.Equals("0"))// sv có điểm danh
                    {
                        check = "1";
                    }
                    context.AttendanceRollCalls.Remove(context.AttendanceRollCalls.Find(item.Id)); //remove item(id) trong danh sách điểm danh theo sv (chưa điểm danh)
                    context.SaveChanges();
                }

                foreach (var item in listAllAttendanceByStudent)//duyệt danh sách điểm danh theo sv (với môn học có sv chưa điểm danh)
                {
                    item.Id = Guid.NewGuid() + "";
                    item.CheckAttendance = check;
                    context.AttendanceRollCalls.Add(item);
                    context.SaveChanges();
                    break;
                }
            }
            return Ok(true);
        }

        [HttpGet]
        [Route("SearchAttendanceRollCall/{keyWord}")]
        public IActionResult SearchAttendanceRollCall(string keyWord)
        {
        

            return Ok(context.AttendanceRollCalls.Where(s => s.CheckAttendance.Contains(keyWord)
                                                        || s.DateCheck.Contains(keyWord)
                                                        || s.Id.Contains(keyWord)
                                                        || s.RegisterId.Contains(keyWord)));
        }

        [HttpDelete]
        [Route("DeleteAttendanceRollCall/{id}")]
        public IActionResult DeleteAttendanceRollCall(string id)
        {
            if (context.AttendanceRollCalls.Find(id) == null)
            {
                return Ok("null");
            }
            else
            {
                context.AttendanceRollCalls.Remove(context.AttendanceRollCalls.Find(id));
                context.SaveChanges();
                return Ok("true");
            }
        }

        [HttpPut]
        [Route("UpdateAttendanceRollCall/{id}")]
        public IActionResult UpdateAttendanceRollCall (string id, [FromBody] AttendanceRollCall attendanceRollCall)
        {
            var entity = context.AttendanceRollCalls.FirstOrDefault(e => e.Id.Equals(id));

            if (entity != null)
            {
                try
                {
                    entity.RegisterId = attendanceRollCall.RegisterId;
                    entity.CheckAttendance = attendanceRollCall.CheckAttendance;
                    entity.DateCheck = attendanceRollCall.DateCheck;

                    context.AttendanceRollCalls.Update(entity);
                    context.SaveChanges();
                    return Ok(true);
                }
                catch(Exception)
                {
                    return Ok(false);
                }
            }
            return Ok(false);
        }

        [HttpGet]
        [Route("ReadListAttendanceRollCall/pagesize/pageNow")]
        public IActionResult Paging(int pageSize, int pageNow)
        {
            var qry = context.Set<AttendanceRollCall>().
                       OrderBy(d => d.Id).Skip((pageNow - 1) * pageSize).Take(pageSize).ToList();


            return Ok(qry);
        }
        [HttpGet]
        [Route("CountListAttendanceRollCall/pagesize/pageNow")]
        public IActionResult CountOfPaging(int pageSize, int pageNow)
        {
            int qry = context.AttendanceRollCalls.
                         OrderBy(d => d.Id).Skip((pageNow - 1) * pageSize).Take(pageSize).Count();
            return Ok(qry);
        }

        [HttpGet]
        [Route("CountAllListAttendanceRollCall")]
        public IActionResult CountAllAttendanceRollCall()
        {
            return Ok(context.AttendanceRollCalls.Count());
        }

        [HttpPost]
        [Route("CheckAttend/studentID/subjectID/teacherID")] //check điểm danh theo môn học và gv
        public IActionResult CheckAttend (string studentID, string subjectID, string teacherID)
        {
            //lấy reg vs id student, id subject, id teacher truyền vào
            var obj = context.RegisterSubjects.Where(s => s.StudentId.Equals(studentID)
                                                      && s.SubjectId.Equals(subjectID)
                                                      && s.TeacherId.Equals(teacherID)).ToList();

            if (obj.Count > 0)
            {
                AttendanceRollCall attendance = new AttendanceRollCall();
                attendance.Id = Guid.NewGuid() + "";
                attendance.RegisterId = obj[0].RegisterId;
                attendance.CheckAttendance = "1";
                attendance.DateCheck = DateTime.Now.ToString("dd/MM/yyyy");
                Console.WriteLine(attendance.Id + "--" + attendance.RegisterId + "-----" + attendance.DateCheck);
                try
                {
                    context.AttendanceRollCalls.Add(attendance);
                    context.SaveChanges();
                    return Ok(true);
                }
                catch(System.Exception)
                {
                    return Ok(false);
                }
            }
            return Ok(false);
        }

        //truyền vào id subject, id teacher, ngày dd -> lấy danh sách dd theo condition (lấy dd, chưa dd, lấy toàn bộ)
        [HttpGet]
        [Route("LoadListAttend/subjectID/teacherID/dateCheck/condition")]
        public IActionResult LoadListAttend(string subjectID, string teacherID, string dateCheck, string condition)
        {
            switch (condition)
            {
                case "DD":
                    var listDD =
                    (from reg in context.RegisterSubjects join at in context.AttendanceRollCalls
                     on reg.RegisterId equals at.RegisterId

                     join stu in context.Students
                     on reg.StudentId equals stu.StudentId
                     join pro_stu in context.ProfileStudents
                     on stu.ProfileId equals pro_stu.ProfileId

                     where reg.SubjectId == subjectID
                     where reg.TeacherId == teacherID
                     where at.DateCheck == dateCheck
                     where at.CheckAttendance == "1"

                     select new { pro_stu, at.CheckAttendance }
                    ).Distinct().ToList();
                    return Ok(listDD);

                case "CDD":
                    var listCDD =
                    (from reg in context.RegisterSubjects join at in context.AttendanceRollCalls
                     on reg.RegisterId equals at.RegisterId

                     join stu in context.Students
                     on reg.StudentId equals stu.StudentId
                     join pro_stu in context.ProfileStudents
                     on stu.ProfileId equals pro_stu.ProfileId

                     where reg.SubjectId == subjectID
                     where reg.TeacherId == teacherID
                     where at.DateCheck == dateCheck
                     where at.CheckAttendance == "0"

                     select new { pro_stu, at.CheckAttendance }
                    ).Distinct().ToList();
                    return Ok(listCDD.ToList());

                case "ALL":
                    var listALL =
                    (from reg in context.RegisterSubjects join at in context.AttendanceRollCalls
                     on reg.RegisterId equals at.RegisterId

                     join stu in context.Students
                     on reg.StudentId equals stu.StudentId
                     join pro_stu in context.ProfileStudents
                     on stu.ProfileId equals pro_stu.ProfileId

                     where reg.SubjectId == subjectID
                     where reg.TeacherId == teacherID
                     where at.DateCheck == dateCheck
                     where at.CheckAttendance == "0" || at.CheckAttendance == "1"

                     select new { pro_stu, at.CheckAttendance }
                    ).Distinct().ToList();
                    return Ok(listALL.ToList());

                default:
                    return Ok(context.AttendanceRollCalls.ToList());
            }

        }

        [HttpGet]
        [Route("FindAttendanceRollCall/{id}")]
        public IActionResult Find (string id)
        {
            if (context.AttendanceRollCalls.Find(id) == null)
            {
                return Ok("Null");
            }
            return Ok(context.AttendanceRollCalls.Find(id));
        }

        [HttpGet]
        [Route("CountConditionAttendanceRollCall/{keyWord}")]
        public IActionResult CountConditionAttendanceRollCall(string keyWord)
        {
            return Ok(context.AttendanceRollCalls.Where(s => s.CheckAttendance.Contains(keyWord)
                                                        || s.DateCheck.Contains(keyWord)
                                                        || s.Id.Contains(keyWord)
                                                        || s.RegisterId.Contains(keyWord)).Count());
        }

        [HttpGet]
        [Route("ReadAllListAttendanceRollCall")]
        public IActionResult ReadListAttendanceRollCall()
        {
            return Ok(context.AttendanceRollCalls);
        }
    }
}
