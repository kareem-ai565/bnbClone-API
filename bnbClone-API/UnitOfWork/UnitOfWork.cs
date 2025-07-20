using bnbClone_API.Data;

using bnbClone_API.Models;
using Microsoft.EntityFrameworkCore;

using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;


namespace bnbClone_API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        private BookingRepo _bookingRepo;


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        AmenityRepo _Amenity;
        PropertyCategoryRepo _PropertyCategory;
        PropertyAmenityRepo _PropertyAmenity;
        HostVerificationRepo _VerificationRepo;


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
