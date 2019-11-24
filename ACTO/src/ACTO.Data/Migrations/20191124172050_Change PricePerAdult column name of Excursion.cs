using Microsoft.EntityFrameworkCore.Migrations;

namespace ACTO.Data.Migrations
{
    public partial class ChangePricePerAdultcolumnnameofExcursion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildPrice",
                table: "Excursions");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Excursions");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerAdult",
                table: "Excursions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerChild",
                table: "Excursions",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerAdult",
                table: "Excursions");

            migrationBuilder.DropColumn(
                name: "PricePerChild",
                table: "Excursions");

            migrationBuilder.AddColumn<decimal>(
                name: "ChildPrice",
                table: "Excursions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Excursions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
