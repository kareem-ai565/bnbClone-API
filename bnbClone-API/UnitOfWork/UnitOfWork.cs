using bnbClone_API.Data;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Models;
using Microsoft.EntityFrameworkCore;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.ML;
using Microsoft.EntityFrameworkCore.Storage;
using bnbClone_API.Repositories.Interfaces.admin;
using bnbClone_API.Repositories.Impelementations.admin;
using bnbClone_API.Repositories.Implementations.admin;



namespace bnbClone_API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext dbContext;
        private BookingRepo _BookingRepo;
        AmenityRepo _Amenity;
        PropertyCategoryRepo _PropertyCategory;
        PropertyAmenityRepo _PropertyAmenity;
        private BookingPaymentRepo _BookingPaymentRepo;
        private BookingPayoutRepo _BookingPayoutRepo;
        private HostVerificationRepo _VerificationRepo;
        private HostRepository _HostRepository;
        private UserRepository _UserRepository;
        private HostPayoutRepo _HostPayoutRepo;
        private HostVerificationRepository _HostVerificationRepo;
        private NotificationRepository _NotificationRepository;
        private PropertyRepository _PropertyRepository;
        private ViolationRepository _ViolationRepository;


        private IDbContextTransaction _transaction;


        public IFavouriteRepo FavouriteRepo { get; }
        public IAvailabilityRepo AvailabilityRepo { get; }
        public IViolationRepo ViolationRepo { get; }


        UserUsedPromotionRepo _UserUsedPromotion;
        
       

        public UnitOfWork(
            ApplicationDbContext dbContext,
            IFavouriteRepo favouriteRepo,
            IAvailabilityRepo availabilityRepo,
            IViolationRepo violationRepo)
        {
            this.dbContext = dbContext;
            FavouriteRepo = favouriteRepo;
            AvailabilityRepo = availabilityRepo;
            ViolationRepo = violationRepo;
        }
    



      

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
        public IBookingRepo BookingRepo => _BookingRepo ??= new BookingRepo(dbContext);


        public void Dispose()
        {
            dbContext.Dispose();
        }


        public async Task<int> SaveAsync()

        {
          return await  dbContext.SaveChangesAsync();
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


        public IBookingPaymentRepo BookingPaymentRepo => _BookingPaymentRepo ??= new BookingPaymentRepo(dbContext);
        public IBookingPayoutRepo BookingPayoutRepo =>_BookingPayoutRepo??= new BookingPayoutRepo(dbContext);
        public IHostPayoutRepo HostPayoutRepo => _HostPayoutRepo??= new HostPayoutRepo(dbContext);




        public IHostVerificationRepo hostVerification { 
            get{
                if(_VerificationRepo == null)
            
                 _VerificationRepo = new HostVerificationRepo(dbContext);
                return _VerificationRepo;
            
            } 
        }
        public IUserRepository Users => _UserRepository??= new UserRepository(dbContext);
        public IHostRepository Hosts => _HostRepository??= new HostRepository(dbContext); // Add this
        public IPropertyRepository Properties => _PropertyRepository??= new PropertyRepository(dbContext);
        public IViolationRepository Violations => _ViolationRepository??= new ViolationRepository(dbContext);
        public IHostVerificationRepository HostVerifications => _HostVerificationRepo??= new HostVerificationRepository(dbContext);
        public INotificationRepository Notifications => _NotificationRepository??= new NotificationRepository(dbContext);


        public IUserUsedPromotionRepo UserUsedPromotion => _UserUsedPromotion ??= new UserUsedPromotionRepo(dbContext);


                    
           

        public async Task BeginTransactionAsync()
        {
            _transaction = await dbContext.Database.BeginTransactionAsync();
        }


        public async Task CommitTransactionAsync()
        {
            try
            {
                await dbContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }


    }

}
