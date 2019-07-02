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
    public class ContactsController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

       //truyển vào môn học, lấy tất cả contact, tất cả sinh viên của môn học đó
       [HttpGet]
       [Route("ViewAllContactsBySubject")]
       public IActionResult ViewAllContactsBySubject(string subjectID)
        {
            
            var listContacts = (from contact in context.Contacts join subject in context.Subjects
                               on contact.SubjectId equals subject.SubjectId
                               join student in context.Students
                               on contact.StudentId equals student.StudentId
                               join profile_student in context.ProfileStudents on student.ProfileId equals profile_student.ProfileId

                               where contact.SubjectId == subjectID
                              

                               select new { contact, profile_student }
                               ).Distinct().ToList();

            return Ok(listContacts);
        }

        //truyền vào môn học, sinh viên --> lấy tất cả contact of 1 sinh viên trong 1 môn học
        [HttpGet]
        [Route("ViewAllContactsOfStudentBySubject")]
        public IActionResult ViewAllContactsOfStudentBySubject(string subjectID, string studentID)
        {
            var listContact = (from contact in context.Contacts join subject in context.Subjects
                               on contact.SubjectId equals subject.SubjectId
                               join student in context.Students
                               on contact.StudentId equals student.StudentId
                               join profile_student in context.ProfileStudents on student.ProfileId equals profile_student.ProfileId

                               where contact.StudentId == studentID
                               where contact.SubjectId == subjectID

                               select new { contact, profile_student }).Distinct().ToList();

            return Ok(listContact);
        }

        [HttpPost]
        [Route("AddContact")]
        public IActionResult CreateContact ([FromBody] Contact contact)
        {
            try
            {
                contact.Id = Guid.NewGuid() + "";
                context.Contacts.Add(contact);
                context.SaveChanges();
                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }
            
        }

        [HttpGet]
        [Route("SearchContact/{keyWord}")]
        public IActionResult SearchContact (string keyWord)
        {
           /* if (String.IsNullOrEmpty(keyWord))
            {
                return Ok("Empty");
            }
            if (context.Contacts.Find(keyWord) == null)
            {
                return Ok("Null");
            }*/

            return Ok(context.Contacts.Where(s => s.Content.Contains(keyWord)
                                                || s.Title.Contains(keyWord)
                                                || s.Id.Contains(keyWord)
                                                || s.StudentId.Contains(keyWord)
                                                || s.SubjectId.Contains(keyWord)));
        }

        [HttpPut]
        [Route("UpdateContact/{id}")]
        public IActionResult UpdateContact (string id, [FromBody] Contact contact)
        {
            var entity = context.Contacts.FirstOrDefault(e => e.Id.Equals(id));
            try
            {
                if(entity != null)
                {
                    entity.Title = contact.Title;
                    entity.Content = contact.Content;
                    entity.StudentId = contact.StudentId;
                    entity.SubjectId = contact.SubjectId;

                    context.Contacts.Update(entity);
                    context.SaveChanges();

                    return Ok(true);
                }
            }
            catch
            {
                return Ok(false);
            }

            return Ok(false);
        }

        [HttpDelete]
        [Route("DeleteContact/{id}")]
        public IActionResult DeleteContact (string id)
        {
            if (context.Contacts.Find(id) == null)
            {
                return Ok("Null");
            }
            else
            {
                context.Remove(context.Contacts.Find(id));
                context.SaveChanges();

                return Ok("True");
            }
        }

        [HttpGet]
        [Route("ReadListContact/pagesize/pageNow")]
        public IActionResult Paging(int pageSize, int pageNow)
        {
            var qry = context.Set<Contact>().OrderBy(d => d.Id).Skip((pageNow - 1) * pageSize).Take(pageSize).ToList();

            return Ok(qry);
        }

        [HttpGet]
        [Route("CountListContact/pagesize/pageNow")]
        public IActionResult CountOfPaging(int pageSize, int pageNow)
        {
            int qry = context.Contacts.
                         OrderBy(d => d.Id).Skip((pageNow - 1) * pageSize).Take(pageSize).Count();
            return Ok(qry);
        }

        [HttpGet]
        [Route("ReadAllListContact")]
        public IActionResult ReadAllListContact()
        {
            return Ok(context.Contacts);
        }

        [HttpGet]
        [Route("CountAllListContact")]
        public IActionResult CountAllListContact()
        {
            return Ok(context.Contacts.Count());
        }

        [HttpGet]
        [Route("FindContact/{id}")]
        public IActionResult FindContactById (string id)
        {
            if (context.Contacts.Find(id) == null)
            {
                return Ok("Null");
            }
            return Ok(context.Contacts.Find(id));
        }

        [HttpGet]
        [Route("CountConditionContact/{keyWord}")]
        public IActionResult CountConditionContact (string keyWord)
        {
            return Ok(context.Contacts.Where(s => s.Content.Contains(keyWord)
                                                || s.Title.Contains(keyWord)
                                                || s.Id.Contains(keyWord)
                                                || s.StudentId.Contains(keyWord)
                                                || s.SubjectId.Contains(keyWord)).Count());
        }
    }
}
