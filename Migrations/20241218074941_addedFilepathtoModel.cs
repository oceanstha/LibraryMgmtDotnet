using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMgmt.Migrations
{
    /// <inheritdoc />
    public partial class addedFilepathtoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Books");
        }
    }
}
