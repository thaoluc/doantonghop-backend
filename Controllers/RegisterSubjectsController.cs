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
    public class RegisterSubjectsController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPost]
        [Route("AddRegisterSubject")]
        public IActionResult CreateSubject ([FromBody] RegisterSubject registerSubject)
        {
            try
            {
                context.RegisterSubjects.Add(registerSubject);
                context.SaveChanges();
                return Ok("True");
            }
            catch (Exception)
            {
                return Ok("False");
            }   
        }

        [HttpGet]
        [Route("SearchRegisterSubject/{keyWord}")]
        public IActionResult SearchRegisterSubject(string keyWord)
        {
           /* if (String.IsNullOrEmpty(keyWord))
            {
                return Ok("Empty");
            }
            if (context.RegisterSubjects.Find(keyWord) == null)
            {
                return Ok("Null");
            }*/

            return Ok(context.RegisterSubjects.Where(s => s.RegisterId.Contains(keyWord)
                                                        || s.StudentId.Contains(keyWord)
                                                        || s.SubjectId.Contains(keyWord)
                                                        || s.TeacherId.Contains(keyWord)
                                                        || s.DateStart.Contains(keyWord)
                                                        || s.DateEnd.Contains(keyWord)).Count());
        }

        [HttpPut]
        [Route("UpdateRegisterSubject/{id}")]
        public IActionResult Update (string id, [FromBody] RegisterSubject registerSubject)
        {
            var entity = context.RegisterSubjects.FirstOrDefault(e => e.RegisterId.Equals(id));

            try
            {
                if (entity != null)
                {
                    entity.StudentId = registerSubject.StudentId;
                    entity.SubjectId = registerSubject.SubjectId;
                    entity.TeacherId = registerSubject.TeacherId;
                    entity.DateStart = registerSubject.DateStart;
                    entity.DateEnd = registerSubject.DateEnd;

                    context.RegisterSubjects.Update(entity);
                    context.SaveChanges();

                    return Ok("True");
                }
            }
            catch (Exception)
            {
                return Ok("False");
            }

            return Ok("False");
        }

        [HttpDelete]
        [Route("DeleteRegisterSubject/{id}")]
        public IActionResult Delete (string id)
        {
            if (context.RegisterSubjects.Find(id)==null)
            {
                return Ok("Null");
            }
            else
            {
                context.Remove(context.RegisterSubjects.Find(id));
                context.SaveChanges();

                return Ok("True");
            }
        }

        [HttpGet]
        [Route("ReadListRegisterSubject/pagesize/pageNow")]
        public IActionResult Paging(int pageSize, int pageNow)
        {
            var qry = context.Set<RegisterSubject>().OrderBy(d => d.RegisterId).Skip((pageNow - 1) * pageSize).Take(pageSize).ToList();

            return Ok(qry);
        }

        [HttpGet]
        [Route("CountListRegisterSubject/pagesize/pageNow")]
        public IActionResult CountOfPaging(int pageSize, int pageNow)
        {
            int qry = context.RegisterSubjects.
                         OrderBy(d => d.RegisterId).Skip((pageNow - 1) * pageSize).Take(pageSize).Count();
            return Ok(qry);
        }

        [HttpGet]
        [Route("CountAllListRegisterSubject")]
        public IActionResult CountRegisterSubject()
        {
            return Ok(context.RegisterSubjects.Count());
        }

        [HttpGet]
        [Route("ReadAllListRegisterSubject")]
        public IActionResult ReadRegisterSubject()
        {
            return Ok(context.RegisterSubjects);
        }

        [HttpGet]
        [Route("FindRegisterSubject/{id}")]
        public IActionResult FindById (string id)
        {
            if (context.RegisterSubjects.Find(id) == null)
            {
                return Ok("Null");
            }
            return Ok(context.RegisterSubjects.Find(id));
        }

        [HttpGet]
        [Route("CountConditionRegisterSubject/{keyWord}")]
        public IActionResult CountCondition(string keyWord)
        {
            return Ok(context.RegisterSubjects.Where(s => s.RegisterId.Contains(keyWord)
                                                        || s.StudentId.Contains(keyWord)
                                                        || s.SubjectId.Contains(keyWord)
                                                        || s.TeacherId.Contains(keyWord)
                                                        || s.DateStart.Contains(keyWord)
                                                        || s.DateEnd.Contains(keyWord)).Count());
        }
    }
}
