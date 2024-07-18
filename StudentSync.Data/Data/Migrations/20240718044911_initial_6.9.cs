using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentSync.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial_69 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "Batches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
