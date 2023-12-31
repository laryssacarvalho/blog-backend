﻿using BlogApi.Application.Interfaces;
using BlogApi.Domain.Entities;
using BlogApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BlogDbContext dbContext) : base(dbContext)
        {
        }
        public async Task AddUserAsync(User user) => await AddAsync(user);

        public async Task<User> GetUserByEmail(string email)
        {
            return await FirstOrDefaultAsync(u => u.Email == email, i => i.Include(u => u.Roles));
        }
    }
}
