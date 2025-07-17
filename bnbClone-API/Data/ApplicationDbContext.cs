//#region MyRegion
//using bnbClone_API.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Reflection.Emit;
//using System.Text.Json.Serialization;


//namespace bnbClone_API.Data
//{
//    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//            : base(options) { }

//        // Tables
//        public DbSet<ApplicationUser> Users { get; set; }
//        public DbSet<Models.Host> Hosts { get; set; }
//        public DbSet<Property> Properties { get; set; }
//        public DbSet<PropertyImage> PropertyImages { get; set; }
//        public DbSet<Amenity> Amenities { get; set; }
//        public DbSet<PropertyAvailability> PropertyAvailabilities { get; set; }
//        public DbSet<Booking> Bookings { get; set; }
//        public DbSet<BookingPayment> BookingPayments { get; set; }
//        public DbSet<BookingPayout> BookingPayouts { get; set; }
//        public DbSet<Review> Reviews { get; set; }
//        public DbSet<HostVerification> HostVerifications { get; set; }
//        public DbSet<HostPayout> HostPayouts { get; set; }
//        public DbSet<Promotion> Promotions { get; set; }
//        public DbSet<UserUsedPromotion> UserUsedPromotions { get; set; }
//        public DbSet<Favourite> Favourites { get; set; }
//        public DbSet<Message> Messages { get; set; }
//        public DbSet<Conversation> Conversations { get; set; }
//        public DbSet<CancellationPolicy> CancellationPolicies { get; set; }
//        public DbSet<PropertyCategory> PropertyCategories { get; set; }

//        // Views
//        public DbSet<PropertyAvailabilityView> PropertyAvailabilityViews { get; set; }
//        public DbSet<PropertyDetailsView> PropertyDetailsViews { get; set; }
//        public DbSet<UserBookingHistoryView> UserBookingHistoryViews { get; set; }
//        public DbSet<HostPerformanceView> HostPerformanceViews { get; set; }
//        public DbSet<ActivePromotionView> ActivePromotionViews { get; set; }

//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);

//            // Views (Read-only)
//            builder.Entity<PropertyAvailabilityView>().HasNoKey().ToView("vw_property_availability");
//            builder.Entity<PropertyDetailsView>().HasNoKey().ToView("vw_property_details");
//            builder.Entity<UserBookingHistoryView>().HasNoKey().ToView("vw_user_booking_history");
//            builder.Entity<HostPerformanceView>().HasNoKey().ToView("vw_host_performance");
//            builder.Entity<ActivePromotionView>().HasNoKey().ToView("vw_active_promotions");


//            // Additional fluent configurations can go here if needed


//            //// Configure Identity tables
//            //builder.Entity<ApplicationUser>().ToTable("Users");
//            //builder.Entity<IdentityRole<int>>().ToTable("Roles");
//            //builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
//            //builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
//            //builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
//            //builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
//            //builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");

//            // Configure PropertyAmenity composite key
//            builder.Entity<PropertyAmenity>()
//                .HasKey(pa => new { pa.PropertyId, pa.AmenityId });

//            // Configure many-to-many relationship between Property and Amenity
//            builder.Entity<PropertyAmenity>()
//                .HasKey(pa => new { pa.PropertyId, pa.AmenityId }); // Composite key

//            builder.Entity<PropertyAmenity>()
//                .HasOne(pa => pa.Property)
//                .WithMany() // No navigation property on Property side for PropertyAmenity
//                .HasForeignKey(pa => pa.PropertyId);

//            builder.Entity<PropertyAmenity>()
//                .HasOne(pa => pa.Amenity)
//                .WithMany(a => a.PropertyAmenities)
//                .HasForeignKey(pa => pa.AmenityId);

//            // Configure the many-to-many relationship through the join table
//            builder.Entity<Property>()
//                .HasMany(p => p.Amenities)
//                .WithMany()
//                .UsingEntity<PropertyAmenity>(
//                    j => j.HasOne(pa => pa.Amenity)
//                          .WithMany(a => a.PropertyAmenities)
//                          .HasForeignKey(pa => pa.AmenityId),
//                    j => j.HasOne(pa => pa.Property)
//                          .WithMany()
//                          .HasForeignKey(pa => pa.PropertyId),
//                    j => j.HasKey(pa => new { pa.PropertyId, pa.AmenityId })
//                );

//            builder.Entity<PropertyAmenity>()
//                .HasOne(pa => pa.Amenity)
//                .WithMany(a => a.PropertyAmenities)
//                .HasForeignKey(pa => pa.AmenityId);

//            // Configure Host-User relationship
//            builder.Entity<Models.Host>()
//                .HasOne(h => h.User)
//                .WithOne()
//                .HasForeignKey<Models.Host>(h => h.UserId)
//                .OnDelete(DeleteBehavior.Cascade);

//            // Configure Property relationships
//            builder.Entity<Property>()
//                .HasOne(p => p.Host)
//                .WithMany(h => h.Properties)
//                .HasForeignKey(p => p.HostId)
//                .OnDelete(DeleteBehavior.Cascade);

//            builder.Entity<Property>()
//                .HasOne(p => p.Category)
//                .WithMany(c => c.Properties)
//                .HasForeignKey(p => p.CategoryId)
//                .OnDelete(DeleteBehavior.SetNull);

//            builder.Entity<Property>()
//                .HasOne(p => p.CancellationPolicy)
//                .WithMany(cp => cp.Properties)
//                .HasForeignKey(p => p.CancellationPolicyId)
//                .OnDelete(DeleteBehavior.SetNull);

//            // Configure Booking relationships
//            builder.Entity<Booking>()
//                .HasOne(b => b.Property)
//                .WithMany(p => p.Bookings)
//                .HasForeignKey(b => b.PropertyId)
//                .OnDelete(DeleteBehavior.Cascade);

//            builder.Entity<Booking>()
//                .HasOne(b => b.Guest)
//                .WithMany()
//                .HasForeignKey(b => b.GuestId)
//                .OnDelete(DeleteBehavior.Restrict);

//            // Configure Review relationship (one-to-one with Booking)
//            builder.Entity<Review>()
//                .HasOne(r => r.Booking)
//                .WithOne(b => b.Review)
//                .HasForeignKey<Review>(r => r.BookingId)
//                .OnDelete(DeleteBehavior.Cascade);

//            builder.Entity<Review>()
//                .HasOne(r => r.Reviewer)
//                .WithMany()
//                .HasForeignKey(r => r.ReviewerId)
//                .OnDelete(DeleteBehavior.Restrict);

//            // Configure UserUsedPromotion relationship (one-to-one with Booking)
//            builder.Entity<UserUsedPromotion>()
//                .HasOne(uup => uup.Booking)
//                .WithOne(b => b.UsedPromotion)
//                .HasForeignKey<UserUsedPromotion>(uup => uup.BookingId)
//                .OnDelete(DeleteBehavior.Cascade);

//            builder.Entity<UserUsedPromotion>()
//                .HasOne(uup => uup.Promotion)
//                .WithMany(p => p.UserUsedPromotions)
//                .HasForeignKey(uup => uup.PromotionId)
//                .OnDelete(DeleteBehavior.Cascade);

//            builder.Entity<UserUsedPromotion>()
//                .HasOne(uup => uup.User)
//                .WithMany()
//                .HasForeignKey(uup => uup.UserId)
//                .OnDelete(DeleteBehavior.Restrict);

//            // Configure Payment relationships
//            builder.Entity<BookingPayment>()
//                .HasOne(bp => bp.Booking)
//                .WithMany(b => b.Payments)
//                .HasForeignKey(bp => bp.BookingId)
//                .OnDelete(DeleteBehavior.Cascade);

//            // Configure Conversation relationships
//            builder.Entity<Conversation>()
//                .HasOne(c => c.User1)
//                .WithMany()
//                .HasForeignKey(c => c.user1Id)
//                .OnDelete(DeleteBehavior.Restrict);

//            builder.Entity<Conversation>()
//                .HasOne(c => c.User2)
//                .WithMany()
//                .HasForeignKey(c => c.user2Id)
//                .OnDelete(DeleteBehavior.Restrict);

//            builder.Entity<Conversation>()
//                .HasOne(c => c.Property)
//                .WithMany(p => p.Conversations)
//                .HasForeignKey(c => c.PropertyId)
//                .OnDelete(DeleteBehavior.SetNull);

//            // Configure Message relationships
//            builder.Entity<Message>()
//                .HasOne(m => m.Conversation)
//                .WithMany(c => c.Messages)
//                .HasForeignKey(m => m.ConversationId)
//                .OnDelete(DeleteBehavior.Cascade);

//            builder.Entity<Message>()
//                .HasOne(m => m.Sender)
//                .WithMany()
//                .HasForeignKey(m => m.SenderId)
//                .OnDelete(DeleteBehavior.Restrict);

//            builder.Entity<Message>()
//                .HasOne(m => m.Receiver)
//                .WithMany()
//                .HasForeignKey(m => m.ReceiverId)
//                .OnDelete(DeleteBehavior.Restrict);

//            // Configure Favourite relationship
//            builder.Entity<Favourite>()
//                .HasOne(f => f.User)
//                .WithMany()
//                .HasForeignKey(f => f.UserId)
//                .OnDelete(DeleteBehavior.NoAction); // or NoAction

//            builder.Entity<Favourite>()
//                .HasOne(f => f.Property)
//                .WithMany(p => p.Favourites)
//                .HasForeignKey(f => f.PropertyId)
//                .OnDelete(DeleteBehavior.NoAction); // or NoAction


//            // Configure indexes for better performance
//            builder.Entity<Property>()
//                .HasIndex(p => p.HostId);

//            builder.Entity<Property>()
//                .HasIndex(p => new { p.City, p.Country });

//            builder.Entity<Property>()
//                .HasIndex(p => p.Status);

//            builder.Entity<Booking>()
//                .HasIndex(b => b.PropertyId);

//            builder.Entity<Booking>()
//                .HasIndex(b => b.GuestId);

//            builder.Entity<Booking>()
//                .HasIndex(b => new { b.StartDate, b.EndDate });

//            builder.Entity<Favourite>()
//                .HasIndex(f => new { f.UserId, f.PropertyId })
//                .IsUnique();

//            builder.Entity<Promotion>()
//                .HasIndex(p => p.Code)
//                .IsUnique();

//            // Configure decimal precision
//            builder.Entity<Property>()
//                .Property(p => p.PricePerNight)
//                .HasPrecision(10, 2);

//            builder.Entity<Property>()
//                .Property(p => p.CleaningFee)
//                .HasPrecision(10, 2);

//            builder.Entity<Property>()
//                .Property(p => p.ServiceFee)
//                .HasPrecision(10, 2);

//            builder.Entity<Booking>()
//                .Property(b => b.TotalAmount)
//                .HasPrecision(10, 2);

//            builder.Entity<BookingPayment>()
//                .Property(bp => bp.Amount)
//                .HasPrecision(10, 2);

//            builder.Entity<BookingPayment>()
//                .Property(bp => bp.RefundedAmount)
//                .HasPrecision(10, 2);

//            builder.Entity<Models.Host>()
//                .Property(h => h.Rating)
//                .HasPrecision(3, 2);

//            builder.Entity<Models.Host>()
//                .Property(h => h.TotalEarnings)
//                .HasPrecision(12, 2);

//            builder.Entity<Models.Host>()
//                .Property(h => h.AvailableBalance)
//                .HasPrecision(12, 2);





//        }

//    }


//}

//#endregion

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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Violation> Violations { get; set; }

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

            // ===== APPLICATION USER CONFIGURATION =====
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255).HasColumnName("email");
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255).HasColumnName("password_hash");
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50).HasColumnName("first_name");
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50).HasColumnName("last_name");
                entity.Property(u => u.DateOfBirth).HasColumnType("date").HasColumnName("date_of_birth");
                entity.Property(u => u.Gender).HasMaxLength(10).HasColumnName("gender");
                entity.Property(u => u.ProfilePictureUrl).HasMaxLength(255).HasColumnName("profile_picture_url");
                entity.Property(u => u.PhoneNumber).HasMaxLength(20).HasColumnName("phone_number");
                entity.Property(u => u.AccountStatus).HasDefaultValue("active").HasMaxLength(20).HasColumnName("account_status");
                entity.Property(u => u.EmailVerified).HasDefaultValue(false).HasColumnName("email_verified");
                entity.Property(u => u.PhoneVerified).HasDefaultValue(false).HasColumnName("phone_verified");
                entity.Property(u => u.LastLogin).HasColumnType("datetime").HasColumnName("last_login");
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20).HasDefaultValue("guest").HasColumnName("role");
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnName("created_at").HasColumnType("datetime");
                entity.Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime");
                entity.Property(u => u.RefreshToken).HasMaxLength(500).HasColumnName("refresh_token");
                entity.Property(u => u.RefreshTokenExpiryTime).HasColumnType("datetime").HasColumnName("refresh_token_expiry_time");

                // Check constraints
                entity.HasCheckConstraint("CK_Users_AccountStatus", "[account_status] IN ('active', 'inactive', 'suspended', 'deleted')");
                entity.HasCheckConstraint("CK_Users_Role", "[role] IN ('guest', 'host', 'admin')");
                entity.HasCheckConstraint("CK_Users_Gender", "[gender] IN ('male', 'female', 'other', 'prefer_not_to_say')");

                // Indexes
                entity.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IX_Users_Email");
                entity.HasIndex(u => u.PhoneNumber).HasDatabaseName("IX_Users_PhoneNumber");
            });
            // ===== HOST CONFIGURATION =====
            builder.Entity<Models.Host>(entity =>
            {
                entity.ToTable("hosts");
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Id).ValueGeneratedOnAdd().HasColumnName("host_id");
                entity.Property(h => h.UserId).IsRequired().HasColumnName("user_id");
                entity.Property(h => h.StartDate).HasDefaultValueSql("GETDATE()").HasColumnName("start_date").HasColumnType("datetime");
                entity.Property(h => h.AboutMe).HasMaxLength(500).HasColumnName("about_me");
                entity.Property(h => h.Work).HasMaxLength(100).HasColumnName("work");
                entity.Property(h => h.Rating).HasDefaultValue(0).HasColumnName("rating").HasColumnType("decimal(3,2)");
                entity.Property(h => h.TotalReviews).HasDefaultValue(0).HasColumnName("total_reviews");
                entity.Property(h => h.Education).HasMaxLength(100).HasColumnName("education");
                entity.Property(h => h.Languages).HasMaxLength(100).HasColumnName("languages");
                entity.Property(h => h.IsVerified).HasDefaultValue(false).HasColumnName("is_verified");
                entity.Property(h => h.TotalEarnings).HasDefaultValue(0).HasColumnType("decimal(18,2)").HasColumnName("total_earnings");
                entity.Property(h => h.AvailableBalance).HasDefaultValue(0).HasColumnType("decimal(18,2)").HasColumnName("available_balance");
                entity.Property(h => h.StripeAccountId).HasMaxLength(255).HasColumnName("stripe_account_id");
                entity.Property(h => h.DefaultPayoutMethod).HasMaxLength(50).HasColumnName("default_payout_method");
                entity.Property(h => h.PayoutAccountDetails).HasMaxLength(500).HasColumnName("payout_account_details");
                entity.Property(h => h.LivesIn).HasMaxLength(100).HasColumnName("lives_in");
                entity.Property(h => h.DreamDestination).HasMaxLength(100).HasColumnName("dream_destination");
                entity.Property(h => h.FunFact).HasMaxLength(200).HasColumnName("fun_fact");
                entity.Property(h => h.Pets).HasMaxLength(100).HasColumnName("pets");
                entity.Property(h => h.ObsessedWith).HasMaxLength(100).HasColumnName("obsessed_with");
                entity.Property(h => h.SpecialAbout).HasMaxLength(200).HasColumnName("special_about");

                // Relationships
                entity.HasOne(h => h.User)
                    .WithOne()
                    .HasForeignKey<Models.Host>(h => h.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Check constraints
                entity.HasCheckConstraint("CK_Hosts_Rating", "[rating] >= 0 AND [rating] <= 5");
                entity.HasCheckConstraint("CK_Hosts_TotalReviews", "[total_reviews] >= 0");
                entity.HasCheckConstraint("CK_Hosts_TotalEarnings", "[total_earnings] >= 0");
                entity.HasCheckConstraint("CK_Hosts_AvailableBalance", "[available_balance] >= 0");

                // Indexes
                entity.HasIndex(h => h.UserId).IsUnique().HasDatabaseName("IX_Hosts_UserId");
            });

            // ===== AMENITY CONFIGURATION =====
            builder.Entity<Amenity>(entity =>
            {
                entity.ToTable("amenities");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(a => a.Name).IsRequired().HasMaxLength(100).HasColumnName("name");
                entity.Property(a => a.Category).IsRequired().HasMaxLength(50).HasColumnName("category");
                entity.Property(a => a.IconUrl).IsRequired().HasMaxLength(255).HasColumnName("icon_url");

                // Indexes
                entity.HasIndex(a => a.Name).IsUnique().HasDatabaseName("IX_Amenities_Name");
                entity.HasIndex(a => a.Category).HasDatabaseName("IX_Amenities_Category");
            });

            // ===== PROPERTY AMENITY CONFIGURATION =====
            builder.Entity<PropertyAmenity>(entity =>
            {
                entity.ToTable("property_amenities");
                entity.HasKey(pa => new { pa.PropertyId, pa.AmenityId });
                entity.Property(pa => pa.PropertyId).HasColumnName("property_id");
                entity.Property(pa => pa.AmenityId).HasColumnName("amenity_id");

                // Relationships
                entity.HasOne(pa => pa.Property)
                    .WithMany(p => p.PropertyAmenities)
                    .HasForeignKey(pa => pa.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pa => pa.Amenity)
                    .WithMany(a => a.PropertyAmenities)
                    .HasForeignKey(pa => pa.AmenityId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== BOOKING CONFIGURATION =====
            builder.Entity<Booking>(entity =>
            {
                entity.ToTable("bookings");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(b => b.PropertyId).IsRequired().HasColumnName("property_id");
                entity.Property(b => b.GuestId).IsRequired().HasColumnName("guest_id");
                entity.Property(b => b.StartDate).IsRequired().HasColumnType("date").HasColumnName("start_date");
                entity.Property(b => b.EndDate).IsRequired().HasColumnType("date").HasColumnName("end_date");
                entity.Property(b => b.CheckInStatus).HasMaxLength(20).HasDefaultValue("pending").HasColumnName("check_in_status");
                entity.Property(b => b.CheckOutStatus).HasMaxLength(20).HasDefaultValue("pending").HasColumnName("check_out_status");
                entity.Property(b => b.Status).IsRequired().HasMaxLength(20).HasColumnName("status");
                entity.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)").HasColumnName("total_amount");
                entity.Property(b => b.PromotionId).HasDefaultValue(0).HasColumnName("promotion_id");
                entity.Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");
                entity.Property(b => b.UpdatedAt).HasColumnType("datetime").HasColumnName("updated_at");

                // Relationships
                entity.HasOne(b => b.Property)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(b => b.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Guest)
                    .WithMany()
                    .HasForeignKey(b => b.GuestId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Check constraints
                entity.HasCheckConstraint("CK_Bookings_Status", "[status] IN ('confirmed', 'denied', 'pending', 'cancelled', 'completed')");
                entity.HasCheckConstraint("CK_Bookings_CheckInStatus", "[check_in_status] IN ('pending', 'completed')");
                entity.HasCheckConstraint("CK_Bookings_CheckOutStatus", "[check_out_status] IN ('pending', 'completed')");
                entity.HasCheckConstraint("CK_Bookings_Dates", "[start_date] < [end_date]");
                entity.HasCheckConstraint("CK_Bookings_TotalAmount", "[total_amount] > 0");

                // Indexes
                entity.HasIndex(b => b.PropertyId).HasDatabaseName("IX_Bookings_PropertyId");
                entity.HasIndex(b => b.GuestId).HasDatabaseName("IX_Bookings_GuestId");
                entity.HasIndex(b => new { b.StartDate, b.EndDate }).HasDatabaseName("IX_Bookings_Dates");
                entity.HasIndex(b => b.Status).HasDatabaseName("IX_Bookings_Status");
            });

            // ===== BOOKING PAYMENT CONFIGURATION =====
            builder.Entity<BookingPayment>(entity =>
            {
                entity.ToTable("booking_payments");
                entity.HasKey(bp => bp.Id);
                entity.Property(bp => bp.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(bp => bp.BookingId).IsRequired().HasColumnName("booking_id");
                entity.Property(bp => bp.Amount).IsRequired().HasColumnType("decimal(18,2)").HasColumnName("amount");
                entity.Property(bp => bp.PaymentMethodType).IsRequired().HasMaxLength(50).HasColumnName("payment_method_type");
                entity.Property(bp => bp.Status).IsRequired().HasMaxLength(50).HasColumnName("status");
                entity.Property(bp => bp.TransactionId).HasMaxLength(255).HasColumnName("transaction_id");
                entity.Property(bp => bp.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");
                entity.Property(bp => bp.UpdatedAt).HasColumnType("datetime").HasColumnName("updated_at");
                entity.Property(bp => bp.RefundedAmount).HasDefaultValue(0).HasColumnType("decimal(18,2)").HasColumnName("refunded_amount");
                entity.Property(bp => bp.PayementGateWayResponse).HasColumnName("payment_gateway_response").HasColumnType("nvarchar(max)");

                // Relationships
                entity.HasOne(bp => bp.Booking)
                    .WithMany(b => b.Payments)
                    .HasForeignKey(bp => bp.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Check constraints
                entity.HasCheckConstraint("CK_BookingPayments_Amount", "[amount] > 0");
                entity.HasCheckConstraint("CK_BookingPayments_RefundedAmount", "[refunded_amount] >= 0");
                entity.HasCheckConstraint("CK_BookingPayments_RefundedAmount_Amount", "[refunded_amount] <= [amount]");
                entity.HasCheckConstraint("CK_BookingPayments_Status", "[status] IN ('pending', 'completed', 'failed', 'refunded', 'partially_refunded')");

                // Indexes
                entity.HasIndex(bp => bp.BookingId).HasDatabaseName("IX_BookingPayments_BookingId");
                entity.HasIndex(bp => bp.TransactionId).HasDatabaseName("IX_BookingPayments_TransactionId");
                entity.HasIndex(bp => bp.Status).HasDatabaseName("IX_BookingPayments_Status");
            });

            // ===== BOOKING PAYOUT CONFIGURATION =====
            builder.Entity<BookingPayout>(entity =>
            {
                entity.ToTable("booking_payouts");
                entity.HasKey(bp => bp.Id);
                entity.Property(bp => bp.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(bp => bp.BookingId).IsRequired().HasColumnName("booking_id");
                entity.Property(bp => bp.Amount).IsRequired().HasColumnType("decimal(18,2)").HasColumnName("amount");
                entity.Property(bp => bp.Status).IsRequired().HasMaxLength(50).HasColumnName("status");
                entity.Property(bp => bp.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");

                // Relationships
                entity.HasOne(bp => bp.Booking)
                    .WithMany()
                    .HasForeignKey(bp => bp.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Check constraints
                entity.HasCheckConstraint("CK_BookingPayouts_Amount", "[amount] > 0");
                entity.HasCheckConstraint("CK_BookingPayouts_Status", "[status] IN ('pending', 'processing', 'completed', 'failed')");

                // Indexes
                entity.HasIndex(bp => bp.BookingId).HasDatabaseName("IX_BookingPayouts_BookingId");
                entity.HasIndex(bp => bp.Status).HasDatabaseName("IX_BookingPayouts_Status");
            });





            // ===== PROPERTY CONFIGURATION =====
            builder.Entity<Property>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(p => p.HostId).IsRequired().HasColumnName("host_id");
                entity.Property(p => p.CategoryId).HasColumnName("category_id");
                entity.Property(p => p.Title).IsRequired().HasMaxLength(255).HasColumnName("title");
                entity.Property(p => p.Description).HasMaxLength(1000).HasColumnName("description");
                entity.Property(p => p.PropertyType).IsRequired().HasMaxLength(50).HasColumnName("property_type");
                entity.Property(p => p.Country).IsRequired().HasMaxLength(50).HasColumnName("country");
                entity.Property(p => p.Address).IsRequired().HasMaxLength(100).HasColumnName("address");
                entity.Property(p => p.City).IsRequired().HasMaxLength(50).HasColumnName("city");
                entity.Property(p => p.PostalCode).HasMaxLength(20).HasColumnName("postal_code");
                entity.Property(p => p.Latitude).HasColumnType("decimal(9,6)").HasColumnName("latitude");
                entity.Property(p => p.Longitude).HasColumnType("decimal(9,6)").HasColumnName("longitude");
                entity.Property(p => p.Currency).IsRequired().HasMaxLength(10).HasColumnName("currency");
                entity.Property(p => p.PricePerNight).HasColumnType("decimal(18,2)").HasColumnName("price_per_night");
                entity.Property(p => p.CleaningFee).HasDefaultValue(0).HasColumnType("decimal(18,2)").HasColumnName("cleaning_fee");
                entity.Property(p => p.ServiceFee).HasDefaultValue(0).HasColumnType("decimal(18,2)").HasColumnName("service_fee");
                entity.Property(p => p.MinNights).HasDefaultValue(1).HasColumnName("min_nights");
                entity.Property(p => p.MaxNights).HasColumnName("max_nights");
                entity.Property(p => p.Bedrooms).HasDefaultValue(1).HasColumnName("bedrooms");
                entity.Property(p => p.Bathrooms).HasDefaultValue(1).HasColumnName("bathrooms");
                entity.Property(p => p.MaxGuests).HasDefaultValue(1).HasColumnName("max_guests");
                entity.Property(p => p.CheckInTime).HasColumnType("time").HasColumnName("check_in_time");
                entity.Property(p => p.CheckOutTime).HasColumnType("time").HasColumnName("check_out_time");
                entity.Property(p => p.InstantBook).HasDefaultValue(false).HasColumnName("instant_book");
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");
                entity.Property(p => p.UpdatedAt).HasColumnType("datetime").HasColumnName("updated_at");
                entity.Property(p => p.CancellationPolicyId).HasColumnName("cancellation_policy_id");
                entity.Property(p => p.Status).HasMaxLength(20).HasColumnName("status").HasConversion<string>().HasDefaultValue(PropertyStatus.Pending.ToString());

                entity.HasCheckConstraint("CK_Properties_Status", "[status] IN ('active', 'pending', 'suspended')");

                entity.HasOne(p => p.Host)
                    .WithMany(h => h.Properties)
                    .HasForeignKey(p => p.HostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Properties)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(p => p.CancellationPolicy)
                    .WithMany(c => c.Properties)
                    .HasForeignKey(p => p.CancellationPolicyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== PROPERTY CATEGORY CONFIGURATION =====
            builder.Entity<PropertyCategory>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.Property(c => c.CategoryId).ValueGeneratedOnAdd().HasColumnName("category_id");
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50).HasColumnName("name");
                entity.Property(c => c.Description).HasMaxLength(255).HasColumnName("description");
                entity.Property(c => c.IconUrl).HasMaxLength(255).HasColumnName("icon_url");
            });

            // ===== PROPERTY IMAGE CONFIGURATION =====
            builder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(pi => pi.Id);
                entity.Property(pi => pi.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(pi => pi.PropertyId).IsRequired().HasColumnName("property_id");
                entity.Property(pi => pi.ImageUrl).IsRequired().HasMaxLength(255).HasColumnName("image_url");
                entity.Property(pi => pi.Description).HasMaxLength(255).HasColumnName("description");
                entity.Property(pi => pi.IsPrimary).HasDefaultValue(false).HasColumnName("is_primary");
                entity.Property(pi => pi.Category).HasMaxLength(50).HasColumnName("category");
                entity.Property(pi => pi.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");

                entity.HasCheckConstraint("CK_PropertyImages_Category", "[category] IN ('Bedroom', 'Bathroom', 'Living Area', 'Kitchen', 'Exterior', 'Additional')");

                entity.HasOne(pi => pi.Property)
                    .WithMany(p => p.PropertyImages)
                    .HasForeignKey(pi => pi.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(pi => pi.PropertyId);
            });

            // ===== CANCELLATION POLICY CONFIGURATION =====
            builder.Entity<CancellationPolicy>(entity =>
            {
                entity.ToTable("cancellation_policies");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50).HasColumnName("name");
                entity.Property(c => c.Description).HasMaxLength(255).HasColumnName("description");
                entity.Property(c => c.RefundPercentage).HasColumnType("decimal(5,2)").HasColumnName("refund_percentage");

                // Check constraints
                entity.HasCheckConstraint("CK_CancellationPolicies_RefundPercentage", "[refund_percentage] >= 0 AND [refund_percentage] <= 100");
                entity.HasCheckConstraint("CK_CancellationPolicies_Name", "[name] IN ('flexible', 'moderate', 'strict', 'non_refundable')");

                // Indexes
                entity.HasIndex(c => c.Name).IsUnique().HasDatabaseName("IX_CancellationPolicies_Name");
            });

            // ===== CONVERSATION CONFIGURATION =====
            builder.Entity<Conversation>(entity =>
            {
                entity.ToTable("conversations");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(c => c.PropertyId).HasColumnName("property_id");
                entity.Property(c => c.Subject).HasMaxLength(255).HasColumnName("subject");
                entity.Property(c => c.user1Id).IsRequired().HasColumnName("user1_id");
                entity.Property(c => c.user2Id).IsRequired().HasColumnName("user2_id");
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");

                // Relationships
                entity.HasOne(c => c.Property)
                    .WithMany(p => p.Conversations)
                    .HasForeignKey(c => c.PropertyId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(c => c.User1)
                    .WithMany()
                    .HasForeignKey(c => c.user1Id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.User2)
                    .WithMany()
                    .HasForeignKey(c => c.user2Id)
                    .OnDelete(DeleteBehavior.Restrict);

                // Check constraints
                entity.HasCheckConstraint("CK_Conversations_Users", "[user1_id] != [user2_id]");

                // Indexes
                entity.HasIndex(c => c.PropertyId).HasDatabaseName("IX_Conversations_PropertyId");
                entity.HasIndex(c => new { c.user1Id, c.user2Id }).HasDatabaseName("IX_Conversations_Users");
            });

            // ===== FAVOURITE CONFIGURATION =====
            builder.Entity<Favourite>(entity =>
            {
                entity.ToTable("favourites");
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(f => f.UserId).IsRequired().HasColumnName("user_id");
                entity.Property(f => f.PropertyId).IsRequired().HasColumnName("property_id");
                entity.Property(f => f.FavouritedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("favourited_at");

                // Relationships
                entity.HasOne(f => f.User)
                    .WithMany()
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(f => f.Property)
                    .WithMany()
                    .HasForeignKey(f => f.PropertyId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Indexes
                entity.HasIndex(f => new { f.UserId, f.PropertyId }).IsUnique().HasDatabaseName("IX_Favourites_UserProperty");
                entity.HasIndex(f => f.PropertyId).HasDatabaseName("IX_Favourites_PropertyId");
            });


            // ===== PROPERTY AVAILABILITY CONFIGURATION =====
            builder.Entity<PropertyAvailability>(entity =>
            {
                entity.HasKey(pa => pa.Id);
                entity.Property(pa => pa.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(pa => pa.PropertyId).IsRequired().HasColumnName("property_id");
                entity.Property(pa => pa.Date).IsRequired().HasColumnType("date").HasColumnName("date");
                entity.Property(pa => pa.IsAvailable).HasDefaultValue(true).HasColumnName("is_available");
                entity.Property(pa => pa.BlockedReason).HasMaxLength(255).HasColumnName("blocked_reason");
                entity.Property(pa => pa.Price).HasColumnType("decimal(18,2)").HasColumnName("price");
                entity.Property(pa => pa.MinNights).IsRequired().HasDefaultValue(1).HasColumnName("min_nights");

                entity.HasOne(pa => pa.Property)
                    .WithMany(p => p.Availabilities)
                    .HasForeignKey(pa => pa.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

           
            // ===== REVIEW CONFIGURATION =====
            builder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(r => r.ReviewerId).IsRequired().HasColumnName("reviewer_id");
                entity.Property(r => r.BookingId).IsRequired().HasColumnName("booking_id");
                entity.Property(r => r.Rating).IsRequired().HasColumnName("rating");
                entity.Property(r => r.Comment).HasMaxLength(1000).HasColumnName("comment");
                entity.Property(r => r.CreatedAt).HasDefaultValueSql("SYSDATETIME()").HasColumnType("datetime").HasColumnName("created_at");
                entity.Property(r => r.UpdatedAt).HasColumnType("datetime").HasColumnName("updated_at");

                entity.HasOne(r => r.Booking)
                    .WithOne(b => b.Review)
                    .HasForeignKey<Review>(r => r.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Reviewer)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(r => r.ReviewerId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // ===== HOST PAYOUT CONFIGURATION =====
            builder.Entity<HostPayout>(entity =>
            {
                entity.ToTable("host_payouts");
                entity.HasKey(hp => hp.Id);
                entity.Property(hp => hp.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(hp => hp.HostId).IsRequired().HasColumnName("host_id");
                entity.Property(hp => hp.Amount).IsRequired().HasColumnType("decimal(18,2)").HasColumnName("amount");
                entity.Property(hp => hp.Status).IsRequired().HasMaxLength(50).HasColumnName("status");
                entity.Property(hp => hp.PayoutMethod).IsRequired().HasMaxLength(50).HasColumnName("payout_method");
                entity.Property(hp => hp.PayoutAccountDetails).IsRequired().HasMaxLength(500).HasColumnName("payout_account_details");
                entity.Property(hp => hp.TransactionId).IsRequired().HasMaxLength(255).HasColumnName("transaction_id");
                entity.Property(hp => hp.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");
                entity.Property(hp => hp.ProcessedAt).HasColumnType("datetime").HasColumnName("processed_at");
                entity.Property(hp => hp.Notes).IsRequired().HasMaxLength(1000).HasColumnName("notes");

                // Relationships
                entity.HasOne(hp => hp.Host)
                    .WithMany(h => h.Payouts)
                    .HasForeignKey(hp => hp.HostId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Check constraints
                entity.HasCheckConstraint("CK_HostPayouts_Amount", "[amount] > 0");
                entity.HasCheckConstraint("CK_HostPayouts_Status", "[status] IN ('pending', 'processing', 'completed', 'failed')");

                // Indexes
                entity.HasIndex(hp => hp.HostId).HasDatabaseName("IX_HostPayouts_HostId");
                entity.HasIndex(hp => hp.Status).HasDatabaseName("IX_HostPayouts_Status");
                entity.HasIndex(hp => hp.TransactionId).HasDatabaseName("IX_HostPayouts_TransactionId");
            });

            // ===== HOST VERIFICATION CONFIGURATION =====
            builder.Entity<HostVerification>(entity =>
            {
                entity.ToTable("host_verifications");
                entity.HasKey(hv => hv.Id);
                entity.Property(hv => hv.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(hv => hv.HostId).IsRequired().HasColumnName("host_id");
                entity.Property(hv => hv.Type).IsRequired().HasMaxLength(50).HasColumnName("type");
                entity.Property(hv => hv.Status).IsRequired().HasMaxLength(20).HasDefaultValue("pending").HasColumnName("status");
                entity.Property(hv => hv.DocumentUrl1).IsRequired().HasMaxLength(255).HasColumnName("document_url1");
                entity.Property(hv => hv.DocumentUrl2).IsRequired().HasMaxLength(255).HasColumnName("document_url2");
                entity.Property(hv => hv.SubmittedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("submitted_at");
                entity.Property(hv => hv.VerifiedAt).HasColumnType("datetime").HasColumnName("verified_at");

                // Relationships
                entity.HasOne(hv => hv.Host)
                    .WithMany(h => h.Verifications)
                    .HasForeignKey(hv => hv.HostId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Check constraints
                entity.HasCheckConstraint("CK_HostVerifications_Status", "[status] IN ('pending', 'approved', 'rejected')");
                entity.HasCheckConstraint("CK_HostVerifications_Type", "[type] IN ('identity', 'address', 'phone', 'email', 'government_id')");

                // Indexes
                entity.HasIndex(hv => hv.HostId).HasDatabaseName("IX_HostVerifications_HostId");
                entity.HasIndex(hv => hv.Status).HasDatabaseName("IX_HostVerifications_Status");
                entity.HasIndex(hv => new { hv.HostId, hv.Type }).IsUnique().HasDatabaseName("IX_HostVerifications_HostType");
            });

            // ===== MESSAGE CONFIGURATION =====
            builder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(m => m.ConversationId).IsRequired().HasColumnName("conversation_id");
                entity.Property(m => m.SenderId).IsRequired().HasColumnName("sender_id");
                entity.Property(m => m.ReceiverId).IsRequired().HasColumnName("receiver_id");
                entity.Property(m => m.Content).IsRequired().HasMaxLength(1000).HasColumnName("content");
                entity.Property(m => m.SentAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("sent_at");
                entity.Property(m => m.ReadAt).HasColumnType("datetime").HasColumnName("read_at");

                // Relationships
                entity.HasOne(m => m.Conversation)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(m => m.ConversationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Sender)
                    .WithMany()
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Receiver)
                    .WithMany()
                    .HasForeignKey(m => m.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Check constraints
                entity.HasCheckConstraint("CK_Messages_Users", "[sender_id] != [receiver_id]");

                // Indexes
                entity.HasIndex(m => m.ConversationId).HasDatabaseName("IX_Messages_ConversationId");
                entity.HasIndex(m => m.SenderId).HasDatabaseName("IX_Messages_SenderId");
                entity.HasIndex(m => m.ReceiverId).HasDatabaseName("IX_Messages_ReceiverId");
                entity.HasIndex(m => m.SentAt).HasDatabaseName("IX_Messages_SentAt");
            });

            // ===== PROMOTION CONFIGURATION =====
            builder.Entity<Promotion>(entity =>
            {
                entity.ToTable("promotions");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(p => p.Code).IsRequired().HasMaxLength(50).HasColumnName("code");
                entity.Property(p => p.DiscountType).IsRequired().HasMaxLength(20).HasColumnName("discount_type");
                entity.Property(p => p.Amount).HasColumnType("decimal(18,2)").HasColumnName("amount");
                entity.Property(p => p.StartDate).HasColumnType("datetime").HasColumnName("start_date");
                entity.Property(p => p.EndDate).HasColumnType("datetime").HasColumnName("end_date");
                entity.Property(p => p.MaxUses).HasColumnName("max_uses");
                entity.Property(p => p.UsedCount).HasDefaultValue(0).HasColumnName("used_count");
                entity.Property(p => p.IsActive).HasDefaultValue(true).HasColumnName("is_active");
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");

                // Check constraints
                entity.HasCheckConstraint("CK_Promotions_Amount", "[amount] > 0");
                entity.HasCheckConstraint("CK_Promotions_UsedCount", "[used_count] >= 0");
                entity.HasCheckConstraint("CK_Promotions_MaxUses", "[max_uses] > 0");
                entity.HasCheckConstraint("CK_Promotions_Dates", "[start_date] <= [end_date]");
                entity.HasCheckConstraint("CK_Promotions_DiscountType", "[discount_type] IN ('Percentage', 'FixedAmount')");

                // Indexes
                entity.HasIndex(p => p.Code).IsUnique().HasDatabaseName("IX_Promotions_Code");
                entity.HasIndex(p => p.IsActive).HasDatabaseName("IX_Promotions_IsActive");
                entity.HasIndex(p => new { p.StartDate, p.EndDate }).HasDatabaseName("IX_Promotions_Dates");
            });

            // ===== USER USED PROMOTION CONFIGURATION =====
            builder.Entity<UserUsedPromotion>(entity =>
            {
                entity.ToTable("user_used_promotions");
                entity.HasKey(uup => uup.Id);
                entity.Property(uup => uup.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(uup => uup.PromotionId).IsRequired().HasColumnName("promotion_id");
                entity.Property(uup => uup.BookingId).IsRequired().HasColumnName("booking_id");
                entity.Property(uup => uup.UserId).IsRequired().HasColumnName("user_id");
                entity.Property(uup => uup.DiscountedAmount).HasColumnType("decimal(18,2)").HasColumnName("discounted_amount");
                entity.Property(uup => uup.UsedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("used_at");

                // Relationships
                entity.HasOne(uup => uup.Promotion)
                    .WithMany(p => p.UserUsedPromotions)
                    .HasForeignKey(uup => uup.PromotionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(uup => uup.Booking)
                    .WithMany()
                    .HasForeignKey(uup => uup.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(uup => uup.User)
                    .WithMany()
                    .HasForeignKey(uup => uup.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Check constraints
                entity.HasCheckConstraint("CK_UserUsedPromotions_DiscountedAmount", "[discounted_amount] > 0");

                // Indexes
                entity.HasIndex(uup => uup.PromotionId).HasDatabaseName("IX_UserUsedPromotions_PromotionId");
                entity.HasIndex(uup => uup.BookingId).HasDatabaseName("IX_UserUsedPromotions_BookingId");
                entity.HasIndex(uup => uup.UserId).HasDatabaseName("IX_UserUsedPromotions_UserId");
                entity.HasIndex(uup => new { uup.UserId, uup.PromotionId }).HasDatabaseName("IX_UserUsedPromotions_UserPromotion");
            });

            // ===== NOTIFICATION CONFIGURATION =====
            builder.Entity<Notification>(entity =>
            {
                entity.ToTable("notifications");
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(n => n.UserId).IsRequired().HasColumnName("user_id");
                entity.Property(n => n.SenderId).HasColumnName("sender_id");
                entity.Property(n => n.Message).IsRequired().HasMaxLength(500).HasColumnName("message");
                entity.Property(n => n.IsRead).HasDefaultValue(false).HasColumnName("is_read");
                entity.Property(n => n.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");

                // Relationships
                entity.HasOne(n => n.User)
                    .WithMany()
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(n => n.Sender)
                    .WithMany()
                    .HasForeignKey(n => n.SenderId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Check constraints
                entity.HasCheckConstraint("CK_Notifications_Users", "[sender_id] IS NULL OR [sender_id] != [user_id]");

                // Indexes
                entity.HasIndex(n => n.UserId).HasDatabaseName("IX_Notifications_UserId");
                entity.HasIndex(n => n.IsRead).HasDatabaseName("IX_Notifications_IsRead");
                entity.HasIndex(n => new { n.UserId, n.IsRead }).HasDatabaseName("IX_Notifications_UserIsRead");
                entity.HasIndex(n => n.CreatedAt).HasDatabaseName("IX_Notifications_CreatedAt");
            });

            // ===== VIOLATION CONFIGURATION =====
            builder.Entity<Violation>(entity =>
            {
                entity.ToTable("violations");
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Id).ValueGeneratedOnAdd().HasColumnName("id");
                entity.Property(v => v.ReportedById).IsRequired().HasColumnName("reported_by_id");
                entity.Property(v => v.ReportedPropertyId).HasColumnName("reported_property_id");
                entity.Property(v => v.ReportedHostId).HasColumnName("reported_host_id");
                entity.Property(v => v.ViolationType).IsRequired().HasMaxLength(50).HasColumnName("violation_type");
                entity.Property(v => v.Description).IsRequired().HasMaxLength(1000).HasColumnName("description");
                entity.Property(v => v.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Pending").HasColumnName("status");
                entity.Property(v => v.CreatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("created_at");
                entity.Property(v => v.UpdatedAt).HasDefaultValueSql("GETDATE()").HasColumnType("datetime").HasColumnName("updated_at");
                entity.Property(v => v.AdminNotes).HasMaxLength(1000).HasColumnName("admin_notes");
                entity.Property(v => v.ResolvedAt).HasColumnType("datetime").HasColumnName("resolved_at");

                // Relationships
                entity.HasOne(v => v.ReportedBy)
                    .WithMany()
                    .HasForeignKey(v => v.ReportedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.ReportedProperty)
                    .WithMany()
                    .HasForeignKey(v => v.ReportedPropertyId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(v => v.ReportedHost)
                    .WithMany()
                    .HasForeignKey(v => v.ReportedHostId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Check constraints
                entity.HasCheckConstraint("CK_Violations_ViolationType", "[violation_type] IN ('PropertyMisrepresentation', 'HostMisconduct', 'SafetyIssue', 'PolicyViolation', 'FraudulentActivity', 'Other')");
                entity.HasCheckConstraint("CK_Violations_Status", "[status] IN ('Pending', 'UnderReview', 'Resolved', 'Dismissed')");
                entity.HasCheckConstraint("CK_Violations_Subject", "[reported_property_id] IS NOT NULL OR [reported_host_id] IS NOT NULL");

                // Indexes
                entity.HasIndex(v => v.ReportedById).HasDatabaseName("IX_Violations_ReportedById");
                entity.HasIndex(v => v.ReportedPropertyId).HasDatabaseName("IX_Violations_ReportedPropertyId");
                entity.HasIndex(v => v.ReportedHostId).HasDatabaseName("IX_Violations_ReportedHostId");
                entity.HasIndex(v => v.Status).HasDatabaseName("IX_Violations_Status");
                entity.HasIndex(v => v.ViolationType).HasDatabaseName("IX_Violations_ViolationType");
                entity.HasIndex(v => v.CreatedAt).HasDatabaseName("IX_Violations_CreatedAt");
            });


        }
    }
}
