using System;
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
    public class StudentsController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPost]
        [Route("AddStudent")]
        public IActionResult Create ([FromBody] Student student)
        {
            try
            {
              //  student.StudentId = Guid.NewGuid() + ""; //Guid: tạo id là duy nhất
                context.Students.Add(student);
                context.SaveChanges();
                return Ok(true);
            }
            catch
            {
                return Ok(false);
            };
        }

        [HttpGet]
        [Route("SearchStudent/{keyWord}")]

        public IActionResult SearchStudent(string keyWord)
        {
          /*  if (String.IsNullOrEmpty(keyWord))
                return Ok("Empty");
            if (context.Students.Find(keyWord) == null)
            {
                return Ok("Null");
            }*/

            return Ok(context.Students.Where(s => s.StudentId.Contains(keyWord)
                || s.FaceId.Contains(keyWord)
                || s.ProfileId.Contains(keyWord)
                || s.PersonId.Contains(keyWord)
                ));
        }

        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public IActionResult Delete(string id)
        {
            if (context.Students.Find(id) == null)
            {
                return Ok("null");
            }
            else
            {
                context.Students.Remove(context.Students.Find(id));
                context.SaveChanges();
                return Ok("true");
            }

        }

        [HttpPut]
        [Route("UpdateStudent/{id}")]
        public IActionResult Update(string id, [FromBody] Student student)
        {
            var tEntity = context.Students.FirstOrDefault(e => e.StudentId.Equals(id));


            if (tEntity != null)
            {
                try
                {

                    tEntity.FaceId = student.FaceId;
                    tEntity.PersonId = student.PersonId;
                   // tEntity.StudentId = student.StudentId;

                    context.Students.Update(tEntity);
                    context.SaveChanges();
                    return Ok(true);
                }
                catch (Exception)
                {
                    return Ok(false);
                }

            }
            return Ok(false);
        }

        [HttpGet]
        [Route("CountAllListStudent")]
        public IActionResult CountAll()
        {
            return Ok(context.Students.Count());
        }

        [HttpGet]
        [Route("FindStudent/{id}")]
        public IActionResult Find(string id)
        {
            if (context.Students.Find(id) == null)
            {
                return Ok("Null");
            }
            return Ok(context.Students.Find(id));
        }

        [HttpGet]
        [Route("CountConditionStudent/{keyWord}")]
        public IActionResult CountCondition(string keyWord)
        {
            return Ok(context.Students.Where(s => s.StudentId.Contains(keyWord)
                || s.FaceId.Contains(keyWord)
                || s.ProfileId.Contains(keyWord)
                || s.PersonId.Contains(keyWord)
                ).Count());
        }

        [HttpGet]
        [Route("ReadAllListStudent")]
        public IActionResult ReadAllList()
        {
            return Ok(context.Students);
        }

        [HttpGet]
        [Route("CheckAtten/studentID")]
        public IActionResult LoadListAttend(string studentID)   //lấy danh sách điểm danh của 1 sv
        {
            var listAttend = (from reg in context.RegisterSubjects
                              join at in context.AttendanceRollCalls
                              on reg.RegisterId equals at.RegisterId
                              join stu in context.Students
                              on reg.StudentId equals stu.StudentId

                              where stu.StudentId == studentID
                              select new
                              {
                                  face = stu.FaceId,
                                  id = stu.PersonId,
                                  sid = stu.StudentId,
                                  sub = reg.SubjectId,
                                  tea = reg.TeacherId,
                                  date = at.DateCheck
                              }).ToList();
            return Ok(listAttend);
        }

        [HttpGet]
        [Route("ReadListStudent/pagesize/pageNow")]
        public IActionResult Paging(int pageSize, int pageNow)
        {
            var qry = context.Set<Student>().
                       OrderBy(d => d.StudentId).Skip((pageNow - 1) * pageSize).Take(pageSize).ToList();


            return Ok(qry);
        }
        [HttpGet]
        [Route("CountListStudent/pagesize/pageNow")]
        public IActionResult CountOfPaging(int pageSize, int pageNow)
        {
            int qry = context.Students.
                         OrderBy(d => d.StudentId).Skip((pageNow - 1) * pageSize).Take(pageSize).Count();
            return Ok(qry);
        }
    }
}
