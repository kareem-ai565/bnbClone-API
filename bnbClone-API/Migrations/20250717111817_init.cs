using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bnbClone_API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "amenities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    icon_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_amenities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    profile_picture_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    account_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "active"),
                    email_verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    phone_verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_login = table.Column<DateTime>(type: "datetime", nullable: true),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "guest"),
                    refresh_token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    refresh_token_expiry_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.id);
                    table.CheckConstraint("CK_Users_AccountStatus", "[account_status] IN ('active', 'inactive', 'suspended', 'deleted')");
                    table.CheckConstraint("CK_Users_Gender", "[gender] IN ('male', 'female', 'other', 'prefer_not_to_say')");
                    table.CheckConstraint("CK_Users_Role", "[role] IN ('guest', 'host', 'admin')");
                });

            migrationBuilder.CreateTable(
                name: "cancellation_policies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    refund_percentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cancellation_policies", x => x.id);
                    table.CheckConstraint("CK_CancellationPolicies_Name", "[name] IN ('flexible', 'moderate', 'strict', 'non_refundable')");
                    table.CheckConstraint("CK_CancellationPolicies_RefundPercentage", "[refund_percentage] >= 0 AND [refund_percentage] <= 100");
                });

            migrationBuilder.CreateTable(
                name: "promotions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    discount_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    max_uses = table.Column<int>(type: "int", nullable: false),
                    used_count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotions", x => x.id);
                    table.CheckConstraint("CK_Promotions_Amount", "[amount] > 0");
                    table.CheckConstraint("CK_Promotions_Dates", "[start_date] <= [end_date]");
                    table.CheckConstraint("CK_Promotions_DiscountType", "[discount_type] IN ('Percentage', 'FixedAmount')");
                    table.CheckConstraint("CK_Promotions_MaxUses", "[max_uses] > 0");
                    table.CheckConstraint("CK_Promotions_UsedCount", "[used_count] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "PropertyCategories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    icon_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyCategories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hosts",
                columns: table => new
                {
                    host_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    about_me = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    work = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false, defaultValue: 0m),
                    total_reviews = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    education = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    languages = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    total_earnings = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    available_balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    stripe_account_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    default_payout_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    payout_account_details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    lives_in = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dream_destination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fun_fact = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    pets = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    obsessed_with = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    special_about = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hosts", x => x.host_id);
                    table.CheckConstraint("CK_Hosts_AvailableBalance", "[available_balance] >= 0");
                    table.CheckConstraint("CK_Hosts_Rating", "[rating] >= 0 AND [rating] <= 5");
                    table.CheckConstraint("CK_Hosts_TotalEarnings", "[total_earnings] >= 0");
                    table.CheckConstraint("CK_Hosts_TotalReviews", "[total_reviews] >= 0");
                    table.ForeignKey(
                        name: "FK_hosts_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    is_read = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.CheckConstraint("CK_Notifications_Users", "[sender_id] IS NULL OR [sender_id] != [user_id]");
                    table.ForeignKey(
                        name: "FK_notifications_AspNetUsers_sender_id",
                        column: x => x.sender_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_notifications_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "host_payouts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    host_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    payout_method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    payout_account_details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    transaction_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    processed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_host_payouts", x => x.id);
                    table.CheckConstraint("CK_HostPayouts_Amount", "[amount] > 0");
                    table.CheckConstraint("CK_HostPayouts_Status", "[status] IN ('pending', 'processing', 'completed', 'failed')");
                    table.ForeignKey(
                        name: "FK_host_payouts_hosts_host_id",
                        column: x => x.host_id,
                        principalTable: "hosts",
                        principalColumn: "host_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "host_verifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    host_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    document_url1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    document_url2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    submitted_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    verified_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_host_verifications", x => x.id);
                    table.CheckConstraint("CK_HostVerifications_Status", "[status] IN ('pending', 'approved', 'rejected')");
                    table.CheckConstraint("CK_HostVerifications_Type", "[type] IN ('identity', 'address', 'phone', 'email', 'government_id')");
                    table.ForeignKey(
                        name: "FK_host_verifications_hosts_host_id",
                        column: x => x.host_id,
                        principalTable: "hosts",
                        principalColumn: "host_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    host_id = table.Column<int>(type: "int", nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    property_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    postal_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    price_per_night = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    cleaning_fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    service_fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    min_nights = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    max_nights = table.Column<int>(type: "int", nullable: false),
                    bedrooms = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    bathrooms = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    max_guests = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    check_in_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    check_out_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    instant_book = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    cancellation_policy_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.id);
                    table.CheckConstraint("CK_Properties_Status", "[status] IN ('active', 'pending', 'suspended')");
                    table.ForeignKey(
                        name: "FK_Properties_PropertyCategories_category_id",
                        column: x => x.category_id,
                        principalTable: "PropertyCategories",
                        principalColumn: "category_id");
                    table.ForeignKey(
                        name: "FK_Properties_cancellation_policies_cancellation_policy_id",
                        column: x => x.cancellation_policy_id,
                        principalTable: "cancellation_policies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Properties_hosts_host_id",
                        column: x => x.host_id,
                        principalTable: "hosts",
                        principalColumn: "host_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    property_id = table.Column<int>(type: "int", nullable: false),
                    guest_id = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    check_in_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    check_out_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    promotion_id = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.CheckConstraint("CK_Bookings_CheckInStatus", "[check_in_status] IN ('pending', 'completed')");
                    table.CheckConstraint("CK_Bookings_CheckOutStatus", "[check_out_status] IN ('pending', 'completed')");
                    table.CheckConstraint("CK_Bookings_Dates", "[start_date] < [end_date]");
                    table.CheckConstraint("CK_Bookings_Status", "[status] IN ('confirmed', 'denied', 'pending', 'cancelled', 'completed')");
                    table.CheckConstraint("CK_Bookings_TotalAmount", "[total_amount] > 0");
                    table.ForeignKey(
                        name: "FK_bookings_AspNetUsers_guest_id",
                        column: x => x.guest_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    property_id = table.Column<int>(type: "int", nullable: true),
                    subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    user1_id = table.Column<int>(type: "int", nullable: false),
                    user2_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversations", x => x.id);
                    table.CheckConstraint("CK_Conversations_Users", "[user1_id] != [user2_id]");
                    table.ForeignKey(
                        name: "FK_conversations_AspNetUsers_user1_id",
                        column: x => x.user1_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_conversations_AspNetUsers_user2_id",
                        column: x => x.user2_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_conversations_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "favourites",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    property_id = table.Column<int>(type: "int", nullable: false),
                    favourited_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    PropertyId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favourites", x => x.id);
                    table.ForeignKey(
                        name: "FK_favourites_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_favourites_Properties_PropertyId1",
                        column: x => x.PropertyId1,
                        principalTable: "Properties",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_favourites_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "property_amenities",
                columns: table => new
                {
                    property_id = table.Column<int>(type: "int", nullable: false),
                    amenity_id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_property_amenities", x => new { x.property_id, x.amenity_id });
                    table.ForeignKey(
                        name: "FK_property_amenities_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_property_amenities_amenities_amenity_id",
                        column: x => x.amenity_id,
                        principalTable: "amenities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyAvailabilities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    property_id = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    is_available = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    blocked_reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    min_nights = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyAvailabilities", x => x.id);
                    table.ForeignKey(
                        name: "FK_PropertyAvailabilities_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    property_id = table.Column<int>(type: "int", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_primary = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImages", x => x.id);
                    table.CheckConstraint("CK_PropertyImages_Category", "[category] IN ('Bedroom', 'Bathroom', 'Living Area', 'Kitchen', 'Exterior', 'Additional')");
                    table.ForeignKey(
                        name: "FK_PropertyImages_Properties_property_id",
                        column: x => x.property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "violations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reported_by_id = table.Column<int>(type: "int", nullable: false),
                    reported_property_id = table.Column<int>(type: "int", nullable: true),
                    reported_host_id = table.Column<int>(type: "int", nullable: true),
                    violation_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    admin_notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    resolved_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_violations", x => x.id);
                    table.CheckConstraint("CK_Violations_Status", "[status] IN ('Pending', 'UnderReview', 'Resolved', 'Dismissed')");
                    table.CheckConstraint("CK_Violations_Subject", "[reported_property_id] IS NOT NULL OR [reported_host_id] IS NOT NULL");
                    table.CheckConstraint("CK_Violations_ViolationType", "[violation_type] IN ('PropertyMisrepresentation', 'HostMisconduct', 'SafetyIssue', 'PolicyViolation', 'FraudulentActivity', 'Other')");
                    table.ForeignKey(
                        name: "FK_violations_AspNetUsers_reported_by_id",
                        column: x => x.reported_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_violations_Properties_reported_property_id",
                        column: x => x.reported_property_id,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_violations_hosts_reported_host_id",
                        column: x => x.reported_host_id,
                        principalTable: "hosts",
                        principalColumn: "host_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "booking_payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    payment_method_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    transaction_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    refunded_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    payment_gateway_response = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_payments", x => x.id);
                    table.CheckConstraint("CK_BookingPayments_Amount", "[amount] > 0");
                    table.CheckConstraint("CK_BookingPayments_RefundedAmount", "[refunded_amount] >= 0");
                    table.CheckConstraint("CK_BookingPayments_RefundedAmount_Amount", "[refunded_amount] <= [amount]");
                    table.CheckConstraint("CK_BookingPayments_Status", "[status] IN ('pending', 'completed', 'failed', 'refunded', 'partially_refunded')");
                    table.ForeignKey(
                        name: "FK_booking_payments_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_payouts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_payouts", x => x.id);
                    table.CheckConstraint("CK_BookingPayouts_Amount", "[amount] > 0");
                    table.CheckConstraint("CK_BookingPayouts_Status", "[status] IN ('pending', 'processing', 'completed', 'failed')");
                    table.ForeignKey(
                        name: "FK_booking_payouts_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    reviewer_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_reviewer_id",
                        column: x => x.reviewer_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Reviews_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_used_promotions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    promotion_id = table.Column<int>(type: "int", nullable: false),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    discounted_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    used_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    BookingId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_used_promotions", x => x.id);
                    table.CheckConstraint("CK_UserUsedPromotions_DiscountedAmount", "[discounted_amount] > 0");
                    table.ForeignKey(
                        name: "FK_user_used_promotions_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_used_promotions_bookings_BookingId1",
                        column: x => x.BookingId1,
                        principalTable: "bookings",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_used_promotions_bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_used_promotions_promotions_promotion_id",
                        column: x => x.promotion_id,
                        principalTable: "promotions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    conversation_id = table.Column<int>(type: "int", nullable: false),
                    sender_id = table.Column<int>(type: "int", nullable: false),
                    receiver_id = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    sent_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    read_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                    table.CheckConstraint("CK_Messages_Users", "[sender_id] != [receiver_id]");
                    table.ForeignKey(
                        name: "FK_messages_AspNetUsers_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_messages_AspNetUsers_sender_id",
                        column: x => x.sender_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_messages_conversations_conversation_id",
                        column: x => x.conversation_id,
                        principalTable: "conversations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_Category",
                table: "amenities",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_Name",
                table: "amenities",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "AspNetUsers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "AspNetUsers",
                column: "phone_number");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_BookingId",
                table: "booking_payments",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_Status",
                table: "booking_payments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_TransactionId",
                table: "booking_payments",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayouts_BookingId",
                table: "booking_payouts",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayouts_Status",
                table: "booking_payouts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Dates",
                table: "bookings",
                columns: new[] { "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_GuestId",
                table: "bookings",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PropertyId",
                table: "bookings",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_Status",
                table: "bookings",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationPolicies_Name",
                table: "cancellation_policies",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_PropertyId",
                table: "conversations",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_conversations_user2_id",
                table: "conversations",
                column: "user2_id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_Users",
                table: "conversations",
                columns: new[] { "user1_id", "user2_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_PropertyId",
                table: "favourites",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_favourites_PropertyId1",
                table: "favourites",
                column: "PropertyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_UserProperty",
                table: "favourites",
                columns: new[] { "user_id", "property_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HostPayouts_HostId",
                table: "host_payouts",
                column: "host_id");

            migrationBuilder.CreateIndex(
                name: "IX_HostPayouts_Status",
                table: "host_payouts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_HostPayouts_TransactionId",
                table: "host_payouts",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_HostVerifications_HostId",
                table: "host_verifications",
                column: "host_id");

            migrationBuilder.CreateIndex(
                name: "IX_HostVerifications_HostType",
                table: "host_verifications",
                columns: new[] { "host_id", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HostVerifications_Status",
                table: "host_verifications",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Hosts_UserId",
                table: "hosts",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "messages",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "messages",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SentAt",
                table: "messages",
                column: "sent_at");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                table: "notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsRead",
                table: "notifications",
                column: "is_read");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_sender_id",
                table: "notifications",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserIsRead",
                table: "notifications",
                columns: new[] { "user_id", "is_read" });

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Code",
                table: "promotions",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_Dates",
                table: "promotions",
                columns: new[] { "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_IsActive",
                table: "promotions",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_cancellation_policy_id",
                table: "Properties",
                column: "cancellation_policy_id");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_category_id",
                table: "Properties",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_host_id",
                table: "Properties",
                column: "host_id");

            migrationBuilder.CreateIndex(
                name: "IX_property_amenities_amenity_id",
                table: "property_amenities",
                column: "amenity_id");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAvailabilities_property_id_date",
                table: "PropertyAvailabilities",
                columns: new[] { "property_id", "date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImages_property_id",
                table: "PropertyImages",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_booking_id",
                table: "Reviews",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_reviewer_id",
                table: "Reviews",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_used_promotions_BookingId1",
                table: "user_used_promotions",
                column: "BookingId1",
                unique: true,
                filter: "[BookingId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserUsedPromotions_BookingId",
                table: "user_used_promotions",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserUsedPromotions_PromotionId",
                table: "user_used_promotions",
                column: "promotion_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserUsedPromotions_UserId",
                table: "user_used_promotions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserUsedPromotions_UserPromotion",
                table: "user_used_promotions",
                columns: new[] { "user_id", "promotion_id" });

            migrationBuilder.CreateIndex(
                name: "IX_Violations_CreatedAt",
                table: "violations",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_ReportedById",
                table: "violations",
                column: "reported_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_ReportedHostId",
                table: "violations",
                column: "reported_host_id");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_ReportedPropertyId",
                table: "violations",
                column: "reported_property_id");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_Status",
                table: "violations",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_ViolationType",
                table: "violations",
                column: "violation_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "booking_payments");

            migrationBuilder.DropTable(
                name: "booking_payouts");

            migrationBuilder.DropTable(
                name: "favourites");

            migrationBuilder.DropTable(
                name: "host_payouts");

            migrationBuilder.DropTable(
                name: "host_verifications");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "property_amenities");

            migrationBuilder.DropTable(
                name: "PropertyAvailabilities");

            migrationBuilder.DropTable(
                name: "PropertyImages");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "user_used_promotions");

            migrationBuilder.DropTable(
                name: "violations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "conversations");

            migrationBuilder.DropTable(
                name: "amenities");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "promotions");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "PropertyCategories");

            migrationBuilder.DropTable(
                name: "cancellation_policies");

            migrationBuilder.DropTable(
                name: "hosts");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
