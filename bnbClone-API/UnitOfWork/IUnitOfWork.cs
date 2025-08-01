﻿using bnbClone_API.Data;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Repositories.Interfaces.admin;

namespace bnbClone_API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

        ApplicationDbContext Context { get; }

        IBookingRepo BookingRepo { get; }
        IFavouriteRepo FavouriteRepo { get; }
        IAvailabilityRepo AvailabilityRepo { get; }
        IViolationRepo ViolationRepo { get; }

        IAmenityRepo _Amenities { get; }
        IPropertyAmenityRepo PropAmenities { get; }
        IPropertyCategoryRepo PropCategory { get; }
        IBookingPaymentRepo BookingPaymentRepo { get; }
        IBookingPayoutRepo BookingPayoutRepo { get; }
        IHostPayoutRepo HostPayoutRepo { get; }
        IHostVerificationRepo hostVerification { get; }



        IUserRepository Users { get; }
        IHostRepository Hosts { get; } // Add this

        IPropertyRepository Properties { get; }
        IViolationRepository Violations { get; }
        IHostVerificationRepository HostVerifications { get; }
        INotificationRepository Notifications { get; }



        IUserUsedPromotionRepo UserUsedPromotion { get; }
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        IPropertyRepo PropertyRepo { get; }
        IPropertyImageRepo PropertyImageRepo { get; }
        ICancellationPolicyRepo CancellationPolicies { get; }
        IReviewRepo Reviews { get; }
        IPromotionRepository Promotions { get; }
        IMessageRepo MessageRepo { get; }
        IConversationRepo ConversationRepo { get; }
        INotificationRepo NotificationRepo { get; }

        Task <int>SaveAsync();
        Task<int> CompleteAsync();



    }
}
