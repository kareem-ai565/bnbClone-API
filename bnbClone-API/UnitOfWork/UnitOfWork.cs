using bnbClone_API.Data;
using bnbClone_API.Models;
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

        //============For Undrestand ==============

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
        public IBookingRepo BookingRepo => _bookingRepo??=  new BookingRepo(dbContext);

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
