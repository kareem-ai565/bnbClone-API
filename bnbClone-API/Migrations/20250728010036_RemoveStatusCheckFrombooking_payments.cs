using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bnbClone_API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusCheckFrombooking_payments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
         name: "CK_BookingPayments_Status",
         table: "booking_payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

          

            migrationBuilder.AddCheckConstraint(
                name: "CK_BookingPayments_Status",
                table: "booking_payments",
                sql: "[status] IN ('pending', 'completed', 'failed', 'refunded', 'partially_refunded')");
        }
    }
}
