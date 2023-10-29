using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nikoo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountUseStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "Discount",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "Discount");
        }
    }
}
