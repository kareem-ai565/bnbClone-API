using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bnbClone_API.Migrations
{
    /// <inheritdoc />
    public partial class FinalFix_SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUsers",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AspNetUsers",
                newName: "id");
        }
    }
}
