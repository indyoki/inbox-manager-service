using EmailApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailApi.Repository
{
    public class SqlRepository
    {
        private InboxDBContext _dBContext;
        public SqlRepository(InboxDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task SendEmail(Email email, List<string> recipients)
        {
            await _dBContext.Emails.AddAsync(email);
            _dBContext.SaveChanges();

            foreach(var user in recipients)
            {
                var inboxItem = new Inbox { EmailId = email.EmailId, Username = user, Status = Status.Active.ToString() };
                await _dBContext.Inboxes.AddAsync(inboxItem);
            }
            _dBContext.SaveChanges();
        }
        public async Task<bool> UpdateInbox(string username, int emailId, Status emailStatus)
        {
            var inboxItem = _dBContext.Inboxes.Where(x => (x.Username == username && x.EmailId == emailId)).FirstOrDefault();
            if(inboxItem == null)
                return false;
            
            inboxItem.Status = emailStatus.ToString();

            await _dBContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<Email>> GetEmailsByStatus(string username, Status status)
        {
            var inbox = _dBContext.Inboxes.Where(x => (x.Username == username && x.Status == status.ToString())).Select(p => p.EmailId).ToList();
            return await _dBContext.Emails.Include(x => x.Label).Where(x => inbox.Contains(x.EmailId)).ToListAsync();
        }
        public async Task<int> CreateLabel(Label label)
        {
            await _dBContext.Labels.AddAsync(label);
            _dBContext.SaveChanges();

            return label.LabelId;
        }
        public async Task<bool> DeleteLabel(int labelId)
        {
            var label = await _dBContext.Labels.Where(x => x.LabelId == labelId).FirstOrDefaultAsync();
            if (label == null)
                return false;

            _dBContext.Labels.Remove(label);
            _dBContext.SaveChanges();
            return true;
        }
        public async Task<bool> UpdateEmailLabel(int emailId, int labelId)
        {
            var email = _dBContext.Emails.Where(x => x.EmailId == emailId).FirstOrDefault();
            if (email == null)
                return false;
            
            email.LabelId = labelId;
            await _dBContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveEmailLabel(int emailId)
        {
            var email = _dBContext.Emails.Where(x => x.EmailId == emailId && x.LabelId != null).FirstOrDefault();
            if (email == null)
                return false;

            email.LabelId = null;
            await _dBContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<Email>> FilterByLabel(string username, int labelId)
        {

            var activeEmails = await GetEmailsByStatus(username, Status.Active);
            return activeEmails.Where(x => x.LabelId == labelId).ToList();
        }
        public async Task<List<Email>> FilterBySender(string username, string fromUser)
        {
            var activeEmails = await GetEmailsByStatus(username, Status.Active);
            return activeEmails.Where(x => x.Sender == fromUser).ToList();
        }
    }
}
