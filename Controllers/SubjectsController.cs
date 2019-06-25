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
    public class SubjectsController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPost]
        [Route("AddSubject")]
        public IActionResult CreateSubject([FromBody] Subject subject)
        {
            try
            {
                context.Subjects.Add(subject);
                context.SaveChanges();

                return Ok("true");
            }
            catch
            {
                return Ok("false");
            }
        }

        [HttpGet]
        [Route("SearchSubject/{keyWord}")]
        public IActionResult SearchByKeyword (string keyWord)
        {
           

            return Ok(context.Subjects.Where(s => s.SubjectId.Contains(keyWord) || s.SubjectName.Contains(keyWord)));
        }

        [HttpDelete]
        [Route("DeleteSubject/{id}")]
        public IActionResult Delete (string id)
        {
            if (context.Subjects.Find(id) == null)
            {
                return Ok("Null");
            }
            else
            {
                context.Subjects.Remove(context.Subjects.Find(id));
                context.SaveChanges();

                return Ok("true");
            }
        }

        [HttpPut]
        [Route("UpdateSubject/{id}")]
        public IActionResult Update(string id, [FromBody] Subject subject)
        {
            var entity = context.Subjects.FirstOrDefault(e => e.SubjectId.Equals(id));

            try
            {
                if(entity != null)
                {
                    entity.SubjectName = subject.SubjectName;

                    context.Subjects.Update(entity);
                    context.SaveChanges();

                    return Ok("True");
                }
            }
            catch(Exception)
            {
                return Ok("false");
            }

            return Ok("False");
        }

        [HttpGet]
        [Route("ReadListSubject/pagesize/pageNow")]
        public IActionResult Paging(int pageSize, int pageNow)
        {
            var qry = context.Set<Subject>().OrderBy(d => d.SubjectId).Skip((pageNow - 1) * pageSize).Take(pageSize).ToList();

            return Ok(qry);
        }

        [HttpGet]
        [Route("CountListSubject/pagesize/pageNow")]
        public IActionResult CountOfPaging(int pageSize, int pageNow)
        {
            int qry = context.Subjects.
                         OrderBy(d => d.SubjectId).Skip((pageNow - 1) * pageSize).Take(pageSize).Count();
            return Ok(qry);
        }

        [HttpGet]
        [Route("CountAllListSubject")]
        public IActionResult CountListSubject()
        {
            return Ok(context.Subjects.Count());
        }

        [HttpGet]
        [Route("ReadAllListSubject")]
        public IActionResult ReadListSubject()
        {
            return Ok(context.Subjects);
        }

        [HttpGet]
        [Route("FindSubject/{id}")]
        public IActionResult FindSubjectbyId(string id)
        {
            if (context.Subjects.Find(id) == null)
            {
                return Ok("Null");
            }
            return Ok(context.Subjects.Find(id));
        }

        [HttpGet]
        [Route("CountConditionSubject/{keyWord}")]
        public IActionResult CountConditionSubject(string keyWord)
        {
            return Ok(context.Subjects.Where(s => s.SubjectId.Contains(keyWord) || s.SubjectName.Contains(keyWord)).Count());
        }
    }
}
