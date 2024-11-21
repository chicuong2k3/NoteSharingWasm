using Microsoft.EntityFrameworkCore;
using SharingNote.Api.GraphQL.Types;

namespace SharingNote.Api.GraphQL
{
    [QueryType]
    public static class StatisticsQueries
    {
        public static async Task<IEnumerable<UserActivity>> GetUserActivity(AppDbContext dbContext)
        {
            return await dbContext.Users
            .Select(x => new UserActivity
            {
                UserId = x.Id,
                Email = x.Email!,
                TotalPosts = dbContext.Posts.Count(p => p.UserId == x.Id),
                TotalComments = dbContext.Comments.Count(c => c.UserId == x.Id)
            }).ToListAsync();
        }


    }
}
