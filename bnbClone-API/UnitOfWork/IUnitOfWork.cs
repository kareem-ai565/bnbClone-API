using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;

namespace bnbClone_API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

       
        IBookingRepo BookingRepo { get; }


        IAmenityRepo _Amenities { get; }
        IPropertyAmenityRepo PropAmenities { get; }
        IPropertyCategoryRepo PropCategory { get; }
        IHostVerificationRepo hostVerification { get; }

        Task SaveAsync();

    }
}
