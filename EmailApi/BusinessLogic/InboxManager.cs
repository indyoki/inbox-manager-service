using EmailApi.BusinessLogic.Interface;
using EmailApi.Models;
using EmailApi.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailApi.BusinessLogic
{
    public class InboxManager : IInboxManager
    {
        private readonly SqlRepository _repository;
        public InboxManager(SqlRepository repository)
        {
            _repository = repository;
        }

        public async Task SendEmail(EmailRequest emailRequest)
        {
            try
            {
                var email = new Email()
                {
                    Sender = emailRequest.Sender,
                    Subject = emailRequest.Subject,
                    Content = emailRequest.Content
                };
                await _repository.SendEmail(email, emailRequest.Recipients);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteEmail(string username, int emailId)
        {
            return await _repository.UpdateInbox(username, emailId, Status.Deleted);
        }

        public async Task<bool> RecoverEmail(string username, int emailId)
        {
            return await _repository.UpdateInbox(username, emailId, Status.Active);
        }

        public async Task<List<EmailResponse>> GetAllDeleted(string username)
        {
            var emails = await _repository.GetEmailsByStatus(username, Status.Deleted);
            return mapEmailResponse(emails);
        }
        public async Task<List<EmailResponse>> GetAllActive(string username)
        {
            var emails = await _repository.GetEmailsByStatus(username, Status.Active);
            return mapEmailResponse(emails);
        }

        public Task<int> CreateLabel(string description)
        {
            return _repository.CreateLabel(new Label { Description = description });
        }

        public Task<bool> DeleteLabel(int labelId)
        {
            return _repository.DeleteLabel(labelId);
        }

        public Task<bool> UpdateEmailLabel(int emailId, int labelId)
        {
            return _repository.UpdateEmailLabel(emailId, labelId);
        }

        public async Task<List<EmailResponse>> FilterByLabel(string username, int labelId)
        {
            var emails = await _repository.FilterByLabel(username, labelId);
            return mapEmailResponse(emails);
        }

        public async Task<List<EmailResponse>> FilterBySender(string username, string fromUser)
        {
            var emails = await _repository.FilterBySender(username, fromUser);
            return mapEmailResponse(emails);
        }

        public async Task<bool> RemoveEmailLabel(int emailId)
        {
            return await _repository.RemoveEmailLabel(emailId);
        }
        private List<EmailResponse> mapEmailResponse(List<Email> emails)
        {
            var emailResponses = new List<EmailResponse>();

            foreach (var item in emails)
            {
                emailResponses.Add(new EmailResponse
                {
                    Id = item.EmailId,
                    Sender = item.Sender,
                    Subject = item.Subject,
                    Content = item.Content,
                    LabelDescription = item.Label?.Description
                });
            };
            return emailResponses;
        }
    }
}
