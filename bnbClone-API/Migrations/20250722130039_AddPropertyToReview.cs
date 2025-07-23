using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bnbClone_API.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add the column as nullable first
            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Reviews",
                type: "int",
                nullable: true);

            // Step 2: If you have existing reviews, delete them or update manually
            // For simplicity, let's delete existing reviews (if any)
            migrationBuilder.Sql("DELETE FROM Reviews");

            // Step 3: Make the column NOT NULL with default value
            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 1, // Assuming you have at least one property with id = 1
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Step 4: Create the index
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PropertyId",
                table: "Reviews",
                column: "PropertyId");

            // Step 5: Add the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Properties_PropertyId",
                table: "Reviews",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Properties_PropertyId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_PropertyId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Reviews");
        }
    }
}