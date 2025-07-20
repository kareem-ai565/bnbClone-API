using bnbClone_API.Data;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Models;
using Microsoft.EntityFrameworkCore;
using bnbClone_API.Repositories.Impelementations;


namespace bnbClone_API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext dbContext;

        public IFavouriteRepo FavouriteRepo { get; }
        public IAvailabilityRepo AvailabilityRepo { get; }
        public IViolationRepo ViolationRepo { get; }
        
        AmenityRepo _Amenity;
        PropertyCategoryRepo _PropertyCategory;
        PropertyAmenityRepo _PropertyAmenity;
        HostVerificationRepo _VerificationRepo;

        public UnitOfWork(
            ApplicationDbContext dbContext,
            IBookingRepo bookingRepo,
            IFavouriteRepo favouriteRepo,
            IAvailabilityRepo availabilityRepo,
            IViolationRepo violationRepo)
        {
            this.dbContext = dbContext;
            FavouriteRepo = favouriteRepo;
            AvailabilityRepo = availabilityRepo;
            ViolationRepo = violationRepo;
        }
    

        private BookingRepo _bookingRepo;

      

        //public IBookingRepo BookingRepo
        //{
        //    get
        //    {
        //        if (_bookingRepo == null)
        //        {
        //            _bookingRepo = new BookingRepo(dbContext);
        //        }
        //        return _bookingRepo;
        //    }
        //}
        public IBookingRepo BookingRepo => _bookingRepo ??= new BookingRepo(dbContext);


        public void Dispose()
        {
            dbContext.Dispose();
        }

        public async Task SaveAsync()
        {
          await  dbContext.SaveChangesAsync();
        }

        public IAmenityRepo _Amenities
        {
            get
            {
                if (_Amenity == null)
                {
                    _Amenity = new AmenityRepo(dbContext);
                }
                return _Amenity;

            }

        }
        public IPropertyAmenityRepo PropAmenities
        {
            get{
                if (_PropertyAmenity == null)
                _PropertyAmenity=new PropertyAmenityRepo(dbContext);
                return _PropertyAmenity;

            }
        }
        public IPropertyCategoryRepo PropCategory { 
            get{

                if (_PropertyCategory == null)
                _PropertyCategory=new PropertyCategoryRepo(dbContext);
                return _PropertyCategory;

            }
        }
        public IHostVerificationRepo hostVerification { 
            get{
                if(_VerificationRepo == null)
            
                 _VerificationRepo = new HostVerificationRepo(dbContext);
                return _VerificationRepo;
            
            } 
        }

    }

}
