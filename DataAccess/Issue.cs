using Utility;

namespace DataAccess
{
    
    public class Issue
    {
        public string Id { get; set; }
        public IssueType IssueType { get; set; }
        public IssueStatus IssueStatus { get; set; }
        public string Description { get; set; }
        public string AssignedUserId { get; set; }
        public virtual ApplicationUser? AssignedUser { get; set; }
    }
}
