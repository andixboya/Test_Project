using Microsoft.EntityFrameworkCore.Migrations;

namespace ACTO.Data.Migrations
{
    public partial class MergingRepresentativeWithUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Liquidations_Representatives_RepresentativeId",
                table: "Liquidations");

            migrationBuilder.DropTable(
                name: "Representatives");

            migrationBuilder.AlterColumn<string>(
                name: "RepresentativeId",
                table: "Liquidations",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Liquidations_AspNetUsers_RepresentativeId",
                table: "Liquidations",
                column: "RepresentativeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Liquidations_AspNetUsers_RepresentativeId",
                table: "Liquidations");

            migrationBuilder.AlterColumn<int>(
                name: "RepresentativeId",
                table: "Liquidations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Representatives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Representatives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Representatives_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Representatives_UserId",
                table: "Representatives",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Liquidations_Representatives_RepresentativeId",
                table: "Liquidations",
                column: "RepresentativeId",
                principalTable: "Representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
