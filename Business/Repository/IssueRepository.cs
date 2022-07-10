using Business.Repository.IRepository;
using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class IssueRepository : IIssueRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        
        public IssueRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _context = context;
            _httpContext = httpContext;
        }
        
        public async Task<IEnumerable<Issue>> GetAllIssues()
        {
            var allIssues = await _context.Issues.ToListAsync();
            return allIssues;
        }
        
        public async Task<IEnumerable<Issue>> GetAssignedIssues(string userId)
        {
            var assignedIssues = await _context.Issues.Where(issue => issue.AssignedUserId == userId).ToListAsync();
            return assignedIssues;
        }
        
        public async Task<IssuesDTO> SaveIssue(IssuesDTO issuesDTO)
        {
            var issue = new Issue
            {
                IssueType = issuesDTO.IssueType,
                AssignedUserId = issuesDTO.AssignedUserId,
                IssueStatus = issuesDTO.IssueStatus,
                Description = issuesDTO.Description
                
            };
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
            return issuesDTO;
        }
        
        public async Task<bool> DeleteIssue(string issueId) {
            var issue = await _context.Issues.FindAsync(issueId);
            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();
            return true;
        }
        
    }
}
