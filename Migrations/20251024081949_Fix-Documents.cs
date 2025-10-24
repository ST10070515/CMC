using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG6212_CMCS.Migrations
{
    /// <inheritdoc />
    public partial class FixDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "StoragePath",
                table: "Document",
                newName: "ContentType");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "Document",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalNotes",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "Document",
                newName: "StoragePath");

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Document",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalNotes",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
