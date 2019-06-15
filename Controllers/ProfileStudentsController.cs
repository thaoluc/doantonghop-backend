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
    public class ProfileStudentsController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPost]
        [Route("AddProfileStudent")]

        public IActionResult Create([FromBody] ProfileStudent profileStudent)
        {
            try
            {
              //  profileStudent.ProfileId = Guid.NewGuid() + "";
                context.ProfileStudents.Add(profileStudent);
                context.SaveChanges();
                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }

        }

        [HttpGet]
        [Route("SearchProfileStudent/{keyWord}")]
        public IActionResult SearchProfileStudent(string keyWord)
        {
         /*   if (String.IsNullOrEmpty(keyWord))
            {
                return Ok("Empty");
            }
            if (context.ProfileStudents.Find(keyWord) == null)
            {
                return Ok("False");
            }*/

            return Ok(context.ProfileStudents.Where(s => s.FullName.Contains(keyWord)
                                                       || s.ClassId.Contains(keyWord)
                                                       || s.Address.Contains(keyWord)
                                                       || s.DepartmentId.Contains(keyWord)
                                                       || s.Email.Contains(keyWord)
                                                       || s.PhoneNumber.Contains(keyWord)
                                                       || s.ProfileId.Contains(keyWord)));
        }

        [HttpPut]
        [Route("UpdateProfileStudent/{id}")]
        public IActionResult Update(string id, [FromBody] ProfileStudent profileStudent)
        {
            var entity = context.ProfileStudents.FirstOrDefault(e => e.ProfileId.Equals(id));

            if (entity != null)
            {
                try
                {
                    entity.FullName = profileStudent.FullName;
                    entity.DepartmentId = profileStudent.DepartmentId;
                    entity.Email = profileStudent.Email;
                    entity.Address = profileStudent.Address;
                    entity.ClassId = profileStudent.ClassId;
                    entity.PhoneNumber = profileStudent.PhoneNumber;

                    context.ProfileStudents.Update(entity);
                    context.SaveChanges();

                    return Ok("true");
                }
                catch (Exception)
                {
                    return Ok("false");
                }

            }

            return Ok("false");

        }

        [HttpDelete]
        [Route("DeleteProfileStudent/{id}")]
        public IActionResult Delete(string id)
        {
            if (context.ProfileStudents.Find(id) == null)
            {
                return Ok("null");
            }
            else
            {
                context.ProfileStudents.Remove(context.ProfileStudents.Find(id));
                context.SaveChanges();
                return Ok("true");
            }
        }

        [HttpGet]
        [Route("ReadListProfileStudent/pagesize/pageNow")]
        public IActionResult Paging(int pageSize, int pageNow)
        {
            var qry = context.Set<ProfileStudent>().OrderBy(d => d.ProfileId).Skip((pageNow - 1) * pageSize).Take(pageSize).ToList();

            return Ok(qry);
        }

        [HttpGet]
        [Route("CountListProfileStudent/pagesize/pageNow")]
        public IActionResult CountOfPaging(int pageSize, int pageNow)
        {
            int qry = context.ProfileStudents.
                         OrderBy(d => d.ProfileId).Skip((pageNow - 1) * pageSize).Take(pageSize).Count();
            return Ok(qry);
        }

        [HttpGet]
        [Route("CountAllListProfileStudent")]
        public IActionResult CountListProfileStudent()
        {
            return Ok(context.ProfileStudents.Count());
        }

        [HttpGet]
        [Route("FindProfileStudent/{id}")]
        public IActionResult FindById(string id)
        {
            if (context.ProfileStudents.Find(id) == null)
            {
                return Ok("Null");
            }
            return Ok(context.ProfileStudents.Find(id));
        }

        [HttpGet]
        [Route("CountConditionProfileStudent/{keyWord}")]
        public IActionResult CountConditionProfileStudent (string keyWord)
        {
            return Ok(context.ProfileStudents.Where(s => s.FullName.Contains(keyWord)
                                                       || s.ClassId.Contains(keyWord)
                                                       || s.Address.Contains(keyWord)
                                                       || s.DepartmentId.Contains(keyWord)
                                                       || s.Email.Contains(keyWord)
                                                       || s.PhoneNumber.Contains(keyWord)
                                                       || s.ProfileId.Contains(keyWord)).Count());
        }

        [HttpGet]
        [Route("ReadAllListProfileStudent")]
        public IActionResult ReadListStudent()
        {
            return Ok(context.ProfileStudents);
        }

    }
}
