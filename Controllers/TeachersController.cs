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
    public class TeachersController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPost]
        [Route("AddTeacher")]
        public IActionResult Create([FromBody] Teacher _Teacher)
        {
            try
            {
              //  _Teacher.TeacherId = Guid.NewGuid().ToString(); //Guid: tạo id là duy nhất
                context.Teachers.Add(_Teacher);
                context.SaveChanges();
                return Ok(true);
            }
            catch (Exception e)
            {

                return Ok(e.Message);
            };


        }


        [HttpGet]
        [Route("SearchTeacher/{keyWord}")]

        public IActionResult SearchTeacher(string keyWord)
        {
           /* if (String.IsNullOrEmpty(keyWord)) //nội dung truyền vào nếu null or empty
                return Ok("Empty");
            if (context.Teachers.Find(keyWord) == null)
            {
                return Ok("Null");
            }*/
            //tìm gv theo nội dung truyền vào của gv
         
            return Ok(context.Teachers.Where(s => s.ProfileId.Contains(keyWord)
            || s.TeacherId.Contains(keyWord)));
        }

        [HttpDelete]
        [Route("DeleteTeacher/{id}")]
        public IActionResult Delete(string id)
        {
            if (context.Teachers.Find(id) == null) //tìm gv theo id nếu k thấy
            {
                return Ok("null");
            }
            else
            {
                context.Teachers.Remove(context.Teachers.Find(id)); //nếu tìm thấy remove gv vs id đó
                context.SaveChanges(); //lưu thay đổi
                return Ok("true");
            }

        }

        //
        [HttpPut]
        [Route("UpdateTeacher/{id}")]
        public IActionResult Update(string id, [FromBody] Teacher teacher)
        {
            var tEntity = context.Teachers.FirstOrDefault(e => e.TeacherId.Equals(id)); //Trả về đối tượng (gv) đầu tiên trong danh sách tìm thấy
            //so sánh id của gv vs id truyền vào

            if (tEntity != null) //nếu tìm thấy gv
            {
                try
                {

                    tEntity.ProfileId = teacher.ProfileId; //gán profileid của gv cho đối tượng vừa tìm thấy 

                    context.Teachers.Update(tEntity); // update info cho gv
                    context.SaveChanges(); //lưu thay đổi
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
        [Route("ReadListTeacher/pagesize/pageNow")]
        //danh sách giảng viên mỗi page
        public IActionResult Paging(int pageSize, int pageNow)
        {
            var qry = context.Set<Teacher>().OrderBy(d => d.TeacherId).Skip((pageNow - 1) * pageSize).Take(pageSize).ToList();

            return Ok(qry);
        }

        //đếm số gv của mỗi page
        [HttpGet]
        [Route("CountListTeacher/pagesize/pageNow")]
        public IActionResult CountOfPaging(int pageSize, int pageNow)
        {
            int qry = context.Teachers.
                         OrderBy(d => d.TeacherId).Skip((pageNow - 1) * pageSize).Take(pageSize).Count();
            return Ok(qry);
        }

        //đếm tất cả gv
        [HttpGet]
        [Route("CountAllListTeacher")]
        public IActionResult CountAll()
        {
            return Ok(context.Teachers.Count());
        }

        //Tìm gv theo id
        [HttpGet]
        [Route("FindTeacher/{id}")]
        public IActionResult FindById(string id)
        {

            if (context.Teachers.Find(id) == null)
            {
                return Ok("Null");
            }
            return Ok(context.Teachers.Find(id));
        }

        //đếm số gv theo nội dung bất kì trong id profile hoặc id teacher
        [HttpGet]
        [Route("CountConditionTeacher/{keyWord}")]
        public IActionResult CountCondition(string keyWord)
        {
            return Ok(context.Teachers.Where(s => s.ProfileId.Contains(keyWord)
            || s.TeacherId.Contains(keyWord)).Count());
        }

        //lấy danh sách tất cả gv
        [HttpGet]
        [Route("ReadAllListTeacher")]
        public IActionResult ReadAllList()
        {
            return Ok(context.Teachers);
        }
    }
}
