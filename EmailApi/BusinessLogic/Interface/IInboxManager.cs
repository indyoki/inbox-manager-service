using EmailApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailApi.BusinessLogic.Interface
{
    public interface IInboxManager
    {
        public Task SendEmail(EmailRequest emailRequest);
        public Task<bool> DeleteEmail(string username, int emailId);
        public Task<bool> RecoverEmail(string username, int emailId);
        public Task<List<EmailResponse>> GetAllDeleted(string username);
        public Task<List<EmailResponse>> GetAllActive(string username);
        public Task<int> CreateLabel(string description);
        public Task<bool> DeleteLabel(int labelId);
        public Task<bool> UpdateEmailLabel(int emailId, int labelId);
        public Task<bool> RemoveEmailLabel(int emailId);
        public Task<List<EmailResponse>> FilterByLabel(string username, int labelId);
        public Task<List<EmailResponse>> FilterBySender(string username, string fromUser);
    }
}
