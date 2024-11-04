using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMgmt.Migrations
{
    /// <inheritdoc />
    public partial class secondcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "guid",
                table: "Users",
                newName: "Id");
        }
    }
}
