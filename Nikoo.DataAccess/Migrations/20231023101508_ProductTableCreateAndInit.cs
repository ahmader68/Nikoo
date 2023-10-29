using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nikoo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProductTableCreateAndInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsImportant = table.Column<bool>(type: "bit", nullable: false),
                    IsSuggested = table.Column<bool>(type: "bit", nullable: false),
                    SellCount = table.Column<int>(type: "int", nullable: false),
                    StoreCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "IsActive", "IsImportant", "IsSuggested", "Price", "SellCount", "StoreCapacity", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Product1 Description", true, false, false, 1000, 0, 100, "Product1" },
                    { 2, 1, "Product2 Description", true, false, false, 2000, 0, 200, "Product2" },
                    { 3, 2, "Product3 Description", true, false, false, 3000, 0, 300, "Product3" },
                    { 4, 2, "Product4 Description", true, false, false, 4000, 0, 400, "Product4" },
                    { 5, 3, "Product3 Description", true, false, false, 3000, 0, 300, "Product3" },
                    { 6, 3, "Product6 Description", true, false, false, 6000, 0, 600, "Product6" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
