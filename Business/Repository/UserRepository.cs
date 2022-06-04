using Business.Repository.IRepository;
using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Business.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _context = context;
            _httpContext = httpContext;
        }
        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            var userId = _httpContext.HttpContext.User.Claims.Where(a => a.Type == ClaimTypes.Name).Select(a => a.Value).FirstOrDefault();
            var allUsers = await _context.Users.Where(user => user.Name != userId).ToListAsync();
            return allUsers;

        }

        public async Task<ApplicationUser> GetUserDetailsAsync(string userName)
        {
            var user = await _context.Users.Where(user => user.Name == userName).FirstOrDefaultAsync();
            return user;
        }
    }
}
