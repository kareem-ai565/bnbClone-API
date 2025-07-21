using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class ConversationRepo : GenericRepo<Conversation>,IConversationRepo
    {
        private readonly ApplicationDbContext dbContext;
        public ConversationRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Conversation?> GetByIdWithUsersAndPropertyAsync(int id)
        {
            return await dbContext.Conversations
                .Include(c=>c.User1)
                .Include(c=>c.User2)
                .Include(c=>c.Property)
                .Include(c=>c.Messages)
                .FirstOrDefaultAsync(c=>c.Id==id);
        }

        public async Task<Conversation?> GetExistingBetweenUsersAsync(int user1Id, int user2Id, int? propertyId)
        {
            return await dbContext.Conversations
                .FirstOrDefaultAsync(c =>
                c.user1Id == user1Id &&
                c.user2Id == user2Id &&
                c.PropertyId == propertyId
                );
        }

        public async Task<List<Conversation>> GetUserConversationsAsync(int userId)
        {
            return await dbContext.Conversations
                 .Include(c => c.User1)
                 .Include(c => c.User2)
                 .Include(c => c.Property)
                 .Include(c => c.Messages)
                 .Where(c => c.user1Id == userId || c.user2Id == userId)        
                 .ToListAsync();
        }
    }
}
