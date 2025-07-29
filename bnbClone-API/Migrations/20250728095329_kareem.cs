using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bnbClone_API.Migrations
{
    /// <inheritdoc />
    public partial class kareem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_HostVerifications_Type",
                table: "host_verifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_HostVerifications_Type",
                table: "host_verifications",
                sql: "[type] IN ('identity', 'address', 'phone', 'email', 'government_id')");
        }
    }
}
