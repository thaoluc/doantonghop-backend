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
    public class RecogintionController : ControllerBase
    {
        RollCallContext context = new RollCallContext();

        [HttpPut]
        [Route("UpdatePersonIDAndFaceID")]

        public IActionResult UpdatePersonIDAndFaceID (string personID, string studentID)
        {
            var entity = context.Students.FirstOrDefault(e => e.StudentId.Equals(studentID));   //tìm sinh viên (theo id sv)

            if (entity != null) //nếu có
            {
                try
                {
                    entity.PersonId = personID; 
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
    }
}