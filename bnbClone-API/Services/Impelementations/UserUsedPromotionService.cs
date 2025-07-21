using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using System.Threading.Tasks;

namespace bnbClone_API.Services.Impelementations
{
    public class UserUsedPromotionService: IUserUsedPromotionService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserUsedPromotionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserUsedPromotionDTO>> GetAllUserPromotions()
        {
            IEnumerable<UserUsedPromotion> AllUserPromotions =await unitOfWork.UserUsedPromotion.GetAllAsync();

            IEnumerable<UserUsedPromotionDTO> AllUsersPromotions =   AllUserPromotions.Select(p => new UserUsedPromotionDTO
            {
                UserId = p.UserId,
                PromotionId = p.PromotionId,
                BookingId = p.BookingId,
                DiscountedAmount= p.DiscountedAmount,
                UsedAt= p.UsedAt
            });
            return AllUsersPromotions;
        }


        public async Task<IEnumerable<UserUsedPromotionDTO>> GetAllPromotionsOfUser(int id)
        {
            IEnumerable<UserUsedPromotion> AllUserPromotions = await unitOfWork.UserUsedPromotion.FindInDataAsync(p=>p.UserId==id);
            IEnumerable<UserUsedPromotionDTO> AllUsersPromotions = AllUserPromotions.Select(p => new UserUsedPromotionDTO
            {
                UserId = p.UserId,
                PromotionId = p.PromotionId,
                BookingId = p.BookingId,
                DiscountedAmount = p.DiscountedAmount,
                UsedAt = p.UsedAt

            });
            return AllUsersPromotions;
        }


        public async Task<UserUsedPromotion> AddUserPromotion(UserUsedPromotionDTO usedPromotionDTO)
        {
            UserUsedPromotion usedPromotion= new UserUsedPromotion();
            usedPromotion.UserId= usedPromotionDTO.UserId;
            usedPromotion.PromotionId= usedPromotionDTO.PromotionId;
            usedPromotion.BookingId= usedPromotionDTO.BookingId;
            usedPromotion.DiscountedAmount= usedPromotionDTO.DiscountedAmount;
            usedPromotion.UsedAt= usedPromotionDTO.UsedAt;

            await unitOfWork.UserUsedPromotion.AddAsync(usedPromotion);
            int result= await unitOfWork.SaveAsync();


            return usedPromotion;

        }





    }
}
