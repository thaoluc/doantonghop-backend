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
    public class AuthenticationController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPost]
        [Route("LoginStudent")]
        public IActionResult LoginStudent ([FromBody] string password, string studentID)
        {
            try
            {
                //muốn login thì trùng password --> muốn so sánh password thì phải tìm id của student truyền vào
                var findStudent = context.Students.Find(studentID);

                if (findStudent.Password == password)
                {
                    return Ok(true);
                }
                else { return Ok(false); }
            }
            catch
            {
                return Ok(false);
            }
        }

        [HttpPost]
        [Route("LoginTeacher")]
        public IActionResult LoginTeacher ([FromBody] string password, string teacherID)
        {
            try
            {
                var findTeacher = context.Teachers.Find(teacherID);
                if (findTeacher.Password == password)
                {
                    return Ok(true);
                }
                else { Ok(false); }
            }
            catch
            {
                return Ok(false);
            }
            return Ok(false);
        }

        [HttpPost]
        [Route("ChangePasswordStudent")]
        public IActionResult ChangePasswordStudent ([FromBody] string password, string studentID)
        {
            var entity = context.Students.FirstOrDefault(e => e.StudentId.Equals(studentID));

            if(entity != null)
            {
                try
                {
                    entity.Password = password;
                    context.Students.Update(entity);
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

        [HttpPost]
        [Route("ChangePasswordTeacher")]
        public IActionResult ChangePasswordTeacher ([FromBody] string password, string teacherID)
        {
            var entity = context.Teachers.FirstOrDefault(e => e.TeacherId.Equals(teacherID));

            if(entity != null)
            {
                try
                {
                    entity.Password = password;
                    context.Teachers.Update(entity);
                    context.SaveChanges();

                    return Ok(true);
                }
                catch (Exception){ return Ok(false); }

            }

            return Ok(false);
        }
    }
}