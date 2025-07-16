using bnbClone_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;
using System.Text.Json.Serialization;


namespace bnbClone_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Tables
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Models.Host> Hosts { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<PropertyAvailability> PropertyAvailabilities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingPayment> BookingPayments { get; set; }
        public DbSet<BookingPayout> BookingPayouts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<HostVerification> HostVerifications { get; set; }
        public DbSet<HostPayout> HostPayouts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<UserUsedPromotion> UserUsedPromotions { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<CancellationPolicy> CancellationPolicies { get; set; }
        public DbSet<PropertyCategory> PropertyCategories { get; set; }

        // Views
        public DbSet<PropertyAvailabilityView> PropertyAvailabilityViews { get; set; }
        public DbSet<PropertyDetailsView> PropertyDetailsViews { get; set; }
        public DbSet<UserBookingHistoryView> UserBookingHistoryViews { get; set; }
        public DbSet<HostPerformanceView> HostPerformanceViews { get; set; }
        public DbSet<ActivePromotionView> ActivePromotionViews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Views (Read-only)
            builder.Entity<PropertyAvailabilityView>().HasNoKey().ToView("vw_property_availability");
            builder.Entity<PropertyDetailsView>().HasNoKey().ToView("vw_property_details");
            builder.Entity<UserBookingHistoryView>().HasNoKey().ToView("vw_user_booking_history");
            builder.Entity<HostPerformanceView>().HasNoKey().ToView("vw_host_performance");
            builder.Entity<ActivePromotionView>().HasNoKey().ToView("vw_active_promotions");


            // Additional fluent configurations can go here if needed


            //// Configure Identity tables
            //builder.Entity<ApplicationUser>().ToTable("Users");
            //builder.Entity<IdentityRole<int>>().ToTable("Roles");
            //builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            //builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            //builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            //builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
            //builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");

            // Configure PropertyAmenity composite key
            builder.Entity<PropertyAmenity>()
                .HasKey(pa => new { pa.PropertyId, pa.AmenityId });

            // Configure many-to-many relationship between Property and Amenity
            builder.Entity<PropertyAmenity>()
                .HasKey(pa => new { pa.PropertyId, pa.AmenityId }); // Composite key

            builder.Entity<PropertyAmenity>()
                .HasOne(pa => pa.Property)
                .WithMany() // No navigation property on Property side for PropertyAmenity
                .HasForeignKey(pa => pa.PropertyId);

            builder.Entity<PropertyAmenity>()
                .HasOne(pa => pa.Amenity)
                .WithMany(a => a.PropertyAmenities)
                .HasForeignKey(pa => pa.AmenityId);

            // Configure the many-to-many relationship through the join table
            builder.Entity<Property>()
                .HasMany(p => p.Amenities)
                .WithMany()
                .UsingEntity<PropertyAmenity>(
                    j => j.HasOne(pa => pa.Amenity)
                          .WithMany(a => a.PropertyAmenities)
                          .HasForeignKey(pa => pa.AmenityId),
                    j => j.HasOne(pa => pa.Property)
                          .WithMany()
                          .HasForeignKey(pa => pa.PropertyId),
                    j => j.HasKey(pa => new { pa.PropertyId, pa.AmenityId })
                );

            builder.Entity<PropertyAmenity>()
                .HasOne(pa => pa.Amenity)
                .WithMany(a => a.PropertyAmenities)
                .HasForeignKey(pa => pa.AmenityId);

            // Configure Host-User relationship
            builder.Entity<Models.Host>()
                .HasOne(h => h.User)
                .WithOne()
                .HasForeignKey<Models.Host>(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Property relationships
            builder.Entity<Property>()
                .HasOne(p => p.Host)
                .WithMany(h => h.Properties)
                .HasForeignKey(p => p.HostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Property>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Properties)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Property>()
                .HasOne(p => p.CancellationPolicy)
                .WithMany(cp => cp.Properties)
                .HasForeignKey(p => p.CancellationPolicyId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Booking relationships
            builder.Entity<Booking>()
                .HasOne(b => b.Property)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Booking>()
                .HasOne(b => b.Guest)
                .WithMany()
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Review relationship (one-to-one with Booking)
            builder.Entity<Review>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.Review)
                .HasForeignKey<Review>(r => r.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany()
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure UserUsedPromotion relationship (one-to-one with Booking)
            builder.Entity<UserUsedPromotion>()
                .HasOne(uup => uup.Booking)
                .WithOne(b => b.UsedPromotion)
                .HasForeignKey<UserUsedPromotion>(uup => uup.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserUsedPromotion>()
                .HasOne(uup => uup.Promotion)
                .WithMany(p => p.UserUsedPromotions)
                .HasForeignKey(uup => uup.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserUsedPromotion>()
                .HasOne(uup => uup.User)
                .WithMany()
                .HasForeignKey(uup => uup.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Payment relationships
            builder.Entity<BookingPayment>()
                .HasOne(bp => bp.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(bp => bp.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Conversation relationships
            builder.Entity<Conversation>()
                .HasOne(c => c.User1)
                .WithMany()
                .HasForeignKey(c => c.user1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Conversation>()
                .HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.user2Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Conversation>()
                .HasOne(c => c.Property)
                .WithMany(p => p.Conversations)
                .HasForeignKey(c => c.PropertyId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Message relationships
            builder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Favourite relationship
            builder.Entity<Favourite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction); // or NoAction

            builder.Entity<Favourite>()
                .HasOne(f => f.Property)
                .WithMany(p => p.Favourites)
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.NoAction); // or NoAction


            // Configure indexes for better performance
            builder.Entity<Property>()
                .HasIndex(p => p.HostId);

            builder.Entity<Property>()
                .HasIndex(p => new { p.City, p.Country });

            builder.Entity<Property>()
                .HasIndex(p => p.Status);

            builder.Entity<Booking>()
                .HasIndex(b => b.PropertyId);

            builder.Entity<Booking>()
                .HasIndex(b => b.GuestId);

            builder.Entity<Booking>()
                .HasIndex(b => new { b.StartDate, b.EndDate });

            builder.Entity<Favourite>()
                .HasIndex(f => new { f.UserId, f.PropertyId })
                .IsUnique();

            builder.Entity<Promotion>()
                .HasIndex(p => p.Code)
                .IsUnique();

            // Configure decimal precision
            builder.Entity<Property>()
                .Property(p => p.PricePerNight)
                .HasPrecision(10, 2);

            builder.Entity<Property>()
                .Property(p => p.CleaningFee)
                .HasPrecision(10, 2);

            builder.Entity<Property>()
                .Property(p => p.ServiceFee)
                .HasPrecision(10, 2);

            builder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(10, 2);

            builder.Entity<BookingPayment>()
                .Property(bp => bp.Amount)
                .HasPrecision(10, 2);

            builder.Entity<BookingPayment>()
                .Property(bp => bp.RefundedAmount)
                .HasPrecision(10, 2);

            builder.Entity<Models.Host>()
                .Property(h => h.Rating)
                .HasPrecision(3, 2);

            builder.Entity<Models.Host>()
                .Property(h => h.TotalEarnings)
                .HasPrecision(12, 2);

            builder.Entity<Models.Host>()
                .Property(h => h.AvailableBalance)
                .HasPrecision(12, 2);





        }

    }


}
