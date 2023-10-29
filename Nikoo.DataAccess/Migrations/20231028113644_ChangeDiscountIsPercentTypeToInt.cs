using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nikoo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDiscountIsPercentTypeToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsPercent",
                table: "Discount",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsPercent",
                table: "Discount",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
