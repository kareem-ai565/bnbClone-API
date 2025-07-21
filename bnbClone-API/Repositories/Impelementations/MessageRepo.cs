using bnbClone_API.Data;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Impelementations
{
    public class MessageRepo:GenericRepo<Message>,IMessageRepo
    {
        private readonly ApplicationDbContext dbContext;
        public MessageRepo(ApplicationDbContext dbContext) : base(dbContext) 
        {
            this.dbContext=dbContext;
        }

        public async Task<Message?> GetLatestMessageAsync(int ConversationId)
        {
            return await dbContext.Messages.
                 Where(m => m.ConversationId == ConversationId).
                 OrderByDescending(m => m.SentAt).
                 FirstOrDefaultAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(int messageId)
        {
            return await dbContext.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<List<Message>> GetMessagesByConversationIdAsync (int ConversationId)
        {
            return await dbContext.Set<Message>().
                Where(m => m.ConversationId == ConversationId).
                OrderBy(m => m.SentAt).
                ToListAsync();
        }

     
    }
}
