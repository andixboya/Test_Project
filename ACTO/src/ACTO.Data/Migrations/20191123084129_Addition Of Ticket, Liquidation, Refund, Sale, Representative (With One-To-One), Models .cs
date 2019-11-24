using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ACTO.Data.Migrations
{
    public partial class AdditionOfTicketLiquidationRefundSaleRepresentativeWithOneToOneModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ChildPrice",
                table: "Excursions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Representative",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Representative", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Representative_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Liquidations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cash = table.Column<decimal>(nullable: false),
                    CreditCard = table.Column<decimal>(nullable: false),
                    RepresentativeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Liquidations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Liquidations_Representative_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "Representative",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cash = table.Column<decimal>(nullable: false),
                    CreditCard = table.Column<decimal>(nullable: false),
                    Discount = table.Column<int>(nullable: false),
                    LiquidationId = table.Column<int>(nullable: false),
                    RepresentativeId = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Liquidations_LiquidationId",
                        column: x => x.LiquidationId,
                        principalTable: "Liquidations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sales_Representative_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "Representative",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cash = table.Column<decimal>(nullable: false),
                    CreditCard = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ChildCount = table.Column<int>(nullable: false),
                    AdultCount = table.Column<int>(nullable: false),
                    SaleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refunds_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdultCount = table.Column<int>(nullable: false),
                    ChildrenCount = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    ExcursionId = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    TourLanguageId = table.Column<int>(nullable: false),
                    SaleId = table.Column<int>(nullable: false),
                    RepresentativeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Excursions_ExcursionId",
                        column: x => x.ExcursionId,
                        principalTable: "Excursions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Representative_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "Representative",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_LanguageTypes_TourLanguageId",
                        column: x => x.TourLanguageId,
                        principalTable: "LanguageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Liquidations_RepresentativeId",
                table: "Liquidations",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_SaleId",
                table: "Refunds",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Representative_UserId",
                table: "Representative",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_LiquidationId",
                table: "Sales",
                column: "LiquidationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_RepresentativeId",
                table: "Sales",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CustomerId",
                table: "Tickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ExcursionId",
                table: "Tickets",
                column: "ExcursionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RepresentativeId",
                table: "Tickets",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SaleId",
                table: "Tickets",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TourLanguageId",
                table: "Tickets",
                column: "TourLanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Liquidations");

            migrationBuilder.DropTable(
                name: "Representative");

            migrationBuilder.DropColumn(
                name: "ChildPrice",
                table: "Excursions");
        }
    }
}
