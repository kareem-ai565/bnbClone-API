using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bnbClone_API.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_reviewer_id",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_bookings_booking_id",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_reviewer_id",
                table: "Reviews",
                column: "reviewer_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_bookings_booking_id",
                table: "Reviews",
                column: "booking_id",
                principalTable: "bookings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_reviewer_id",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_bookings_booking_id",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_reviewer_id",
                table: "Reviews",
                column: "reviewer_id",
                principalTable: "AspNetUsers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_bookings_booking_id",
                table: "Reviews",
                column: "booking_id",
                principalTable: "bookings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
