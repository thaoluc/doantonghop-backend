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
    public class ProfileTeachersController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPost]
        [Route("AddProfileTeacher")]
        public IActionResult Create([FromBody] ProfileTeacher profileTeacher)
        {
            try
            {
              //  profileTeacher.ProfileId = Guid.NewGuid() + "";
                context.ProfileTeachers.Add(profileTeacher);
                context.SaveChanges();
                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }
        }

        [HttpGet]
        [Route("SearchProfileTeacher/{keyWord}")]
        public IActionResult SearchByKeyword(string keyWord)
        {
           /* if (String.IsNullOrEmpty(keyWord))
            {
                return Ok("Empty");
            }
            if (context.ProfileTeachers.Find(keyWord) == null)
            {
                return Ok("Null");
            }
            */
            return Ok(context.ProfileTeachers.Where(s => s.ProfileId.Contains(keyWord)
                                                    || s.DepartmentId.Contains(keyWord)
                                                    || s.Email.Contains(keyWord)
                                                    || s.FullName.Contains(keyWord)
                                                    || s.PhoneNumber.Contains(keyWord)
                                                    || s.Address.Contains(keyWord)
                                                    || s.Specialize.Contains(keyWord)));
        }

        [HttpDelete]
        [Route("DeleteProfileTeacher/{id}")]
        public IActionResult Delete (string id)
        {
            if (context.ProfileTeachers.Find(id) == null)
            {
                return Ok("null");
            }
            else
            {
                context.ProfileTeachers.Remove(context.ProfileTeachers.Find(id));
                context.SaveChanges();
                return Ok("true");
            }
        }

        [HttpPut]
        [Route("UpdateProfileTeacher/{id}")]
        public IActionResult Update(string id, [FromBody] ProfileTeacher profileTeacher)
        {
            var entity = context.ProfileTeachers.FirstOrDefault(e => e.ProfileId.Equals(id)); //Trả về đối tượng (gv) đầu tiên trong danh sách tìm thấy
            //so sánh id của gv vs id truyền vào

            if (entity != null)
            {
                try
                {
                    entity.FullName = profileTeacher.FullName;
                    entity.Email = profileTeacher.Email;
                    entity.Address = profileTeacher.Address;
                    entity.PhoneNumber = profileTeacher.PhoneNumber;
                    entity.DepartmentId = profileTeacher.DepartmentId;
                    entity.Specialize = profileTeacher.Specialize;


                    context.ProfileTeachers.Update(entity);
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
        [Route("ReadListProfileTeacher/pagesize/pageNow")]
        public IActionResult Paging (int pageSize, int pageNow)
        {
            var qry = context.Set<ProfileTeacher>().OrderBy(d => d.ProfileId).Skip((pageNow - 1) * pageSize).Take(pageSize).ToList();

            return Ok(qry);
        }

        //đếm số gv của mỗi page
        [HttpGet]
        [Route("CountListProfileTeacher/pagesize/pageNow")]
        public IActionResult CountOfPaging(int pageSize, int pageNow)
        {
            int qry = context.ProfileTeachers.
                         OrderBy(d => d.ProfileId).Skip((pageNow - 1) * pageSize).Take(pageSize).Count();
            return Ok(qry);
        }

        [HttpGet]
        [Route("CountAllListProfileTeacher")]
        public IActionResult CountAll()
        {
            return Ok(context.ProfileTeachers.Count());
        }

        [HttpGet]
        [Route("FindProfileTeacher/{id}")]
        public IActionResult Find(string id)
        {
            if (context.ProfileTeachers.Find(id) == null)
            {
                return Ok("Null");
            }
            return Ok(context.ProfileTeachers.Find(id));
        }

        [HttpGet]
        [Route("CountConditionProfileTeacher/{keyWord}")]
        public IActionResult CounCondition(string keyWord)
        {
            return Ok(context.ProfileTeachers.Where(s => s.Address.Contains(keyWord)
                                                    || s.DepartmentId.Contains(keyWord)
                                                    || s.Email.Contains(keyWord)
                                                    || s.FullName.Contains(keyWord)
                                                    || s.PhoneNumber.Contains(keyWord)
                                                    || s.ProfileId.Contains(keyWord)
                                                    || s.Specialize.Contains(keyWord)).Count());
        }

        [HttpGet]
        [Route("ReadAllListProfileTeacher")]
        public IActionResult ReadAllList()
        {
            return Ok(context.ProfileTeachers);
        }
    }

}
