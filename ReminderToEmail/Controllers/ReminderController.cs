using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Primitives;
using ReminderToEmail.Core.Interfaces;
using ReminderToEmail.Dtos.Request;
using ReminderToEmail.Helper;
using ReminderToEmail.Models;
using System.ComponentModel.DataAnnotations;

namespace ReminderToEmail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(MyActionFilter))]
    public class ReminderController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ReminderController> logger;
        private readonly IEmailService emailService;

        public ReminderController(IUnitOfWork unitOfWork , ILogger<ReminderController> logger, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.emailService = emailService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ReminderDto model)
        {

            var user_id=Request.Headers["Id"];

            if(StringValues.IsNullOrEmpty(user_id))
            {
                return BadRequest("Invalid user attepd");

            }
            var new_reminder = new Reminder()
            {
                to = model.to,
                content = model.content,
                sendAt = model.sendAt,
                method = model.method,
                createBy=Guid.Parse(user_id)
            };

            await unitOfWork.reminderRepository.Add(new_reminder);

            await unitOfWork.saveAsync();

            return Ok("Success"); 
        }
        [EnableRateLimiting("Sliding")]
        [HttpGet("Read")]
        public async Task<IActionResult> reead()
        {

            var user_id = Request.Headers["Id"];

            if (StringValues.IsNullOrEmpty(user_id))
            {
                return BadRequest("Invalid user attepd");

            }
            var reminders = await unitOfWork.reminderRepository.GetAll();

            if (reminders == null)
            {
                return NotFound("There is no reminder");
            }

            return Ok(reminders.Select(x=>x.AsDto()));
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([Required]Guid id, [FromBody] ReminderDto model)
        {

            var user_id = Request.Headers["Id"];

            if (StringValues.IsNullOrEmpty(user_id))
            {
                return BadRequest("Invalid user attepd");

            }
            if(id == null)
            {
                return BadRequest("Id cannot be null");
            }
            var reminder = await unitOfWork.reminderRepository.GetById(id);

            if (reminder == null)
            {
                return NotFound("Not found reminder with this id");
            }

            reminder.to = model.to;
            reminder.content = model.content;
            reminder.sendAt = model.sendAt;
            reminder.method = model.method;
           

            await unitOfWork.reminderRepository.Update(reminder);
            await unitOfWork.saveAsync();

            return Ok("Success");
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete([Required]Guid id)
        {
            var user_id = Request.Headers["Id"];

            if (StringValues.IsNullOrEmpty(user_id))
            {
                return BadRequest("Invalid user attepd");

            }
            if (id == null)
            {
                return BadRequest("Id cannot be null");
            }
            var reminder = await unitOfWork.reminderRepository.GetById(id);

            if (reminder == null)
            {
                return NotFound("Not found reminder with this id");
            }

            await unitOfWork.reminderRepository.Delete(reminder);
            await unitOfWork.saveAsync();

            return Ok($"{id.ToString()} id is deleted by successfully");
        }
      
    }
}
