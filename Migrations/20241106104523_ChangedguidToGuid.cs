using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMgmt.Migrations
{
    /// <inheritdoc />
    public partial class ChangedguidToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "guid",
                table: "BookIssues",
                newName: "Guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "BookIssues",
                newName: "guid");
        }
    }
}
