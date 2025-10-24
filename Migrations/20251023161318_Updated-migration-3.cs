using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG6212_CMCS.Migrations
{
    /// <inheritdoc />
    public partial class Updatedmigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "role",
                table: "User",
                newName: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "User",
                newName: "role");
        }
    }
}
