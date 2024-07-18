using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentSync.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_82 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "Enrollments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
