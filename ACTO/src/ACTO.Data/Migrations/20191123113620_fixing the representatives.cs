using Microsoft.EntityFrameworkCore.Migrations;

namespace ACTO.Data.Migrations
{
    public partial class fixingtherepresentatives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Liquidations_Representative_RepresentativeId",
                table: "Liquidations");

            migrationBuilder.DropForeignKey(
                name: "FK_Representative_AspNetUsers_UserId",
                table: "Representative");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Representative_RepresentativeId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Representative_RepresentativeId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Representative",
                table: "Representative");

            migrationBuilder.RenameTable(
                name: "Representative",
                newName: "Representatives");

            migrationBuilder.RenameIndex(
                name: "IX_Representative_UserId",
                table: "Representatives",
                newName: "IX_Representatives_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Liquidations_Representatives_RepresentativeId",
                table: "Liquidations",
                column: "RepresentativeId",
                principalTable: "Representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Representatives_AspNetUsers_UserId",
                table: "Representatives",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Representatives_RepresentativeId",
                table: "Sales",
                column: "RepresentativeId",
                principalTable: "Representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Representatives_RepresentativeId",
                table: "Tickets",
                column: "RepresentativeId",
                principalTable: "Representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Liquidations_Representatives_RepresentativeId",
                table: "Liquidations");

            migrationBuilder.DropForeignKey(
                name: "FK_Representatives_AspNetUsers_UserId",
                table: "Representatives");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Representatives_RepresentativeId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Representatives_RepresentativeId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Representatives",
                table: "Representatives");

            migrationBuilder.RenameTable(
                name: "Representatives",
                newName: "Representative");

            migrationBuilder.RenameIndex(
                name: "IX_Representatives_UserId",
                table: "Representative",
                newName: "IX_Representative_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Representative",
                table: "Representative",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Liquidations_Representative_RepresentativeId",
                table: "Liquidations",
                column: "RepresentativeId",
                principalTable: "Representative",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Representative_AspNetUsers_UserId",
                table: "Representative",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Representative_RepresentativeId",
                table: "Sales",
                column: "RepresentativeId",
                principalTable: "Representative",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Representative_RepresentativeId",
                table: "Tickets",
                column: "RepresentativeId",
                principalTable: "Representative",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
