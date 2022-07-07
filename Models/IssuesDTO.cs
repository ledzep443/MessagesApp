using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum IssueType
    {
        Bug,
        Feature,
        Improvement
    }
    public class IssuesDTO
    {
        public string Id { get; set; }
        public IssueType IssueType { get; set; }
        public string Description { get; set; }
        public string AssignedUserId { get; set; }
        public virtual ApplicationUser? AssignedUser { get; set; }
    }
}
