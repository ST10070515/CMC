using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG6212_CMCS.Migrations
{
    /// <inheritdoc />
    public partial class Updatedmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ClaimStatus_ClaimStatusStatusID",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Course_CourseID",
                table: "Claims");

            migrationBuilder.DropTable(
                name: "ClaimStatus");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ClaimStatusStatusID",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_CourseID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ClaimStatusStatusID",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "CourseID",
                table: "Claims");

            migrationBuilder.AddColumn<int>(
                name: "ClaimStatus",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimStatus",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "Claims");

            migrationBuilder.AddColumn<int>(
                name: "ClaimStatusStatusID",
                table: "Claims",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseID",
                table: "Claims",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClaimStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoordinatorID = table.Column<int>(type: "int", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_Course_User_CoordinatorID",
                        column: x => x.CoordinatorID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ClaimStatusStatusID",
                table: "Claims",
                column: "ClaimStatusStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_CourseID",
                table: "Claims",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Course_CoordinatorID",
                table: "Course",
                column: "CoordinatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ClaimStatus_ClaimStatusStatusID",
                table: "Claims",
                column: "ClaimStatusStatusID",
                principalTable: "ClaimStatus",
                principalColumn: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Course_CourseID",
                table: "Claims",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "CourseID");
        }
    }
}
