using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IUserRepository
    {
        public Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        public Task<ApplicationUser> GetUserDetailsAsync(string userId);
    }
}
