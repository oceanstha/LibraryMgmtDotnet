using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMgmt.Migrations
{
    /// <inheritdoc />
    public partial class thirdCreateBookIssue2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BookIssues",
                newName: "guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "guid",
                table: "BookIssues",
                newName: "Id");
        }
    }
}
