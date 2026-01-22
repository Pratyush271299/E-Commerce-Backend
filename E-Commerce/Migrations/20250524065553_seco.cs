using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class seco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_All-Products",
                table: "All-Products");

            migrationBuilder.RenameTable(
                name: "All-Products",
                newName: "AllProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllProducts",
                table: "AllProducts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AllProducts",
                table: "AllProducts");

            migrationBuilder.RenameTable(
                name: "AllProducts",
                newName: "All-Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_All-Products",
                table: "All-Products",
                column: "Id");
        }
    }
}
