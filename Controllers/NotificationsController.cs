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
    public class NotificationsController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPost]
        [Route("AddNotification")]

        public IActionResult AddNotification ([FromBody] Notification notification)
        {
            try
            {
                context.Notifications.Add(notification);
                context.SaveChanges();

                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }
  
        }

        [HttpGet]
        [Route("SearchNotification/{keyWord}")]
        public IActionResult SearchNotificationByKeyword (string keyWord)
        {
            /* if (String.IsNullOrEmpty(keyWord))
             {
                 return Ok("Empty");
             }
             if (context.Notifications.Find(keyWord) == null) { return Ok("Null"); }*/

            return Ok(context.Notifications.Where(s => s.Title.Contains(keyWord)
                                                     || s.Content.Contains(keyWord)
                                                     || s.SubjectId.Contains(keyWord)
                                                     || s.TeacherId.Contains(keyWord)
                                                     || s.Id.Contains(keyWord)
                                                     
                                                     ));
        }

        [HttpPut]
        [Route("UpdateNotification/{id}")]
        public IActionResult UpdateNotification ([FromBody] Notification notification,string idNotification)
        {
            var entity = context.Notifications.FirstOrDefault(e => e.Id.Equals(idNotification));

            if(entity != null)
            {
                try
                {
                    entity.Content = notification.Content;
                    entity.Title = notification.Title;
                    entity.DateCreate = notification.DateCreate;
                    entity.SubjectId = notification.SubjectId;
                    entity.TeacherId = notification.TeacherId;

                    context.Notifications.Update(entity);
                    context.SaveChanges();

                    return Ok(true);
                }
                catch
                {
                    return Ok(false);
                }
                
            }

            return Ok(false);
        }

        [HttpDelete]
        [Route("DeleteNotification/{id}")]
        public IActionResult DeleteNotification (string id)
        {
            if (context.Notifications.Find(id) == null) { return Ok("null"); }
            else
            {
                context.Remove(context.Notifications.Find(id));
                context.SaveChanges();

                return Ok("true");
            }
        }

        [HttpGet]
        [Route("ReadListNotification/pagesize/pageNow")]
        public IActionResult Paging(int pageSize, int pageNow)
        {
            var qry = context.Set<Notification>().OrderBy(d => d.Id).Skip((pageNow - 1) * pageSize).Take(pageSize).ToList();

            return Ok(qry);
        }

        [HttpGet]
        [Route("CountListNotification/pagesize/pageNow")]
        public IActionResult CountOfPaging(int pageSize, int pageNow)
        {
            int qry = context.Notifications.
                         OrderBy(d => d.Id).Skip((pageNow - 1) * pageSize).Take(pageSize).Count();
            return Ok(qry);
        }

        [HttpGet]
        [Route("CountAllListNotification")]
        public IActionResult CountListNotification()
        {
            return Ok(context.Notifications.Count());
        }

        [HttpGet]
        [Route("ReadAllListNotification")]
        public IActionResult ReadListNotification()
        {
            return Ok(context.Notifications);
        }

        [HttpGet]
        [Route("FindNotification/{id}")]
        public IActionResult FindNotificationById(string id)
        {
            if (context.Notifications.Find(id) == null) { return Ok("Null"); }
            return Ok(context.Notifications.Find(id));
        }

        [HttpGet]
        [Route("CountConditionNotification/{keyWord}")]
        public IActionResult CountConditionNotification(string keyWord)
        {
            return Ok(context.Notifications.Where(s => s.Title.Contains(keyWord)
                                                     || s.Content.Contains(keyWord)
                                                     || s.SubjectId.Contains(keyWord)
                                                     || s.TeacherId.Contains(keyWord)
                                                     || s.Id.Contains(keyWord)
                                                     
                                                     ).Count());
        }

        [HttpGet]
        [Route("ViewNotificationOfTeacherBySubject")]
        public IActionResult ViewNotificationOfTeacherBySubject (string teacherID, string subjectID)
        {
           /* if(context.Notifications.Where(s=>s.TeacherId.Contains(teacherID) || 
                                              s.SubjectId.Contains(subjectID))==null)
            {
                return Ok("Null");
            }*/
            //có 3 đối tượng: notification, teacher, subject
            //join noti vs subject (subjectid)
            //join noti vs teacher (teacherid equals teacherid)
            //lấy tất cả notification dựa trên subjectID và teacherID truyền vào có trong noti
            var listNoti = (from noti in context.Notifications join subject in context.Subjects
                            on noti.SubjectId equals subject.SubjectId

                            join teacher in context.Teachers
                            on noti.TeacherId equals teacher.TeacherId

                            where noti.TeacherId == teacherID
                            where noti.SubjectId == subjectID

                            select noti).Distinct().ToList();

            return Ok(listNoti);
        }
    }
}
