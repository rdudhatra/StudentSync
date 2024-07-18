using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentSync.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_83 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "Inquiries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "Inquiries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
