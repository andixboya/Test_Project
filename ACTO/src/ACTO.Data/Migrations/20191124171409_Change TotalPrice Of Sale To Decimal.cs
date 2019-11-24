using Microsoft.EntityFrameworkCore.Migrations;

namespace ACTO.Data.Migrations
{
    public partial class ChangeTotalPriceOfSaleToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Sales",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalPrice",
                table: "Sales",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
