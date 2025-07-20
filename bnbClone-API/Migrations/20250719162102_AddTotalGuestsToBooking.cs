using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bnbClone_API.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalGuestsToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalGuests",
                table: "bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalGuests",
                table: "bookings");
        }
    }
}
