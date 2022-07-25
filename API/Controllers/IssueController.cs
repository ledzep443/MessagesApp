
using Business.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IssueController : ControllerBase
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IUserRepository _userRepository;
        public IssueController(IIssueRepository issueRepository, IUserRepository userRepository)
        {
            _issueRepository = issueRepository;
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllIssues()
        {
            var allIssues = await _issueRepository.GetAllIssues();
            return Ok(allIssues);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAssignedIssues(string userId)
        {
            var assignedIssues = await _issueRepository.GetAssignedIssues(userId);
            return Ok(assignedIssues);
        }
        [HttpPost]
        public async Task<IActionResult> SaveIssue(IssuesDTO issuesDTO)
        {
            var issue = await _issueRepository.SaveIssue(issuesDTO);
            return Ok(issue);
        }
        [HttpDelete("{issueId}")]
        public async Task<IActionResult> DeleteIssue(string issueId)
        {
            var issue = await _issueRepository.DeleteIssue(issueId);
            return Ok();
        }
    }
    
    
}
