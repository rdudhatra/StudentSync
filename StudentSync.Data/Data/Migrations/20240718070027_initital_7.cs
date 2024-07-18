using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentSync.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class initital_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "CourseExams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "CourseExams",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
