using Microsoft.EntityFrameworkCore.Migrations;

namespace ACTO.Data.Migrations
{
    public partial class ForgottenFKTicketToRepresentative : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Representatives_RepresentativeId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "RepresentativeId",
                table: "Tickets",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
                name: "FK_Tickets_Representatives_RepresentativeId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "RepresentativeId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Representatives_RepresentativeId",
                table: "Tickets",
                column: "RepresentativeId",
                principalTable: "Representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
