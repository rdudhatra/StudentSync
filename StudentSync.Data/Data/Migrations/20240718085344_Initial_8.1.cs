using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentSync.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_81 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "CourseSyllabi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "CourseSyllabi",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
