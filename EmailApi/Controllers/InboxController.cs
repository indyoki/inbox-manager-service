using EmailApi.BusinessLogic.Interface;
using EmailApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InboxController : ControllerBase
    {
        private readonly ILogger<InboxController> _logger;
        private readonly IInboxManager _inboxManager;
        public InboxController(ILogger<InboxController> logger, IInboxManager inboxManager)
        {
            _inboxManager = inboxManager;
            _logger = logger;
        }

        [HttpPost("SendEmail")]
        public async Task<ActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            try
            {
                await _inboxManager.SendEmail(emailRequest);
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occured while attempting to save email. Exception : @ex", ex);
                throw ex;
            }
        }
        [HttpDelete("DeleteEmail/username/{username}/emailId/{emailId}")]
        public async Task<ActionResult> DeleteEmail(string username, int emailId)
        {
            try
            {
                var result = await _inboxManager.DeleteEmail(username, emailId);

                if (result)
                    return Ok(); 
                
                return NotFound($"Email with emailId: {emailId} does exist.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while attempting to delete email with id: @emailId. Exception : @ex", emailId, ex);
                throw ex;
            }
        }

        [HttpPut("RecoverEmail/username/{username}/emailId/{emailId}")]
        public async Task<ActionResult> RecoverEmail(string username, int emailId)
        {
            try
            {
                var result = await _inboxManager.RecoverEmail(username, emailId);

                if (result)
                    return Ok();

                return NotFound($"Email with emailId: {emailId} does not exist.");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while attempting to recover email with id: @emailId. Exception : @ex", emailId, ex);
                throw ex;
            }
        }
        [HttpGet("GetAllDeleted/Username/{username}")]
        public async Task<ActionResult<List<EmailResponse>>> GetAllDeleted(string username)
        {
            try
            {
                return await _inboxManager.GetAllDeleted(username);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while retrieving deleted emails. Exception : @ex", ex);
                throw ex;
            }
        }
        [HttpGet("GetAllActive/Username/{username}")]
        public async Task<ActionResult<List<EmailResponse>>> GetAllActive(string username)
        {
            try
            {
                return await _inboxManager.GetAllActive(username);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while retrieving deleted emails. Exception : @ex", ex);
                throw ex;
            }
        }
        [HttpGet("CreateLabel/description/{description}")]
        public async Task<ActionResult<int>> CreateLabel(string description)
        {
            try
            {
                return await _inboxManager.CreateLabel(description);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating a label. Exception : @ex", ex);
                throw ex;
            }
        }
        [HttpDelete("DeleteLabel/labelId/{labelId}")]
        public async Task<ActionResult> DeleteLabel(int labelId)
        {
            try
            {
                var results = await _inboxManager.DeleteLabel(labelId);
                if (results)
                    return Ok();

                return NotFound($"Label not found {labelId}");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while removing a label. Exception : @ex", ex);
                throw ex;
            }
        }
        [HttpPost("AddLabelToEmail/emailId/{emailId}/labelId/{labelId}")]
        public async Task<ActionResult> AddLabelToEmail(int emailId, int labelId)
        {
            try
            {
                var result = await _inboxManager.UpdateEmailLabel(emailId, labelId);
                if (result)
                    return Ok();

                return NotFound($"Email not found emailId {emailId}");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while creating a label. Exception : @ex", ex);
                throw ex;
            }
        }
        [HttpPost("RemoveEmailLabel/emailId/{emailId}")]
        public async Task<ActionResult> RemoveEmailLabel(int emailId)
        {
            try
            {
                var result = await _inboxManager.RemoveEmailLabel(emailId);
                if (result)
                    return Ok();

                return NotFound($"Email not labeled {emailId}");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while removing a label. Exception : @ex", ex);
                throw ex;
            }
        }
        [HttpGet("FilterByLabel/username/{username}/labelId/{labelId}")]
        public async Task<ActionResult<List<EmailResponse>>> FilterByLabel(string username, int labelId)
        {
            try
            {
                return await _inboxManager.FilterByLabel(username, labelId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to filter emails. Exception : @ex", ex);
                throw ex;
            }
        }
        [HttpGet("FilterBySender/username/{username}/fromUser/{fromUser}")]
        public async Task<ActionResult<List<EmailResponse>>> FilterBySender(string username, string fromUser)
        {
            try
            {
                return await _inboxManager.FilterBySender(username, fromUser);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to filter emails. Exception: @ex", ex);
                throw ex;
            }
        }
    }
}
