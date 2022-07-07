using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IIssueRepository
    {
        public Task<IEnumerable<Issue>> GetAllIssues();
        public Task<IEnumerable<Issue>> GetAssignedIssues(string userId);
        public Task<IssuesDTO> SaveIssue(IssuesDTO issue);
        public Task<bool> DeleteIssue(string issueId);
    }
}
