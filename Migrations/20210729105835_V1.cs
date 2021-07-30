using Microsoft.EntityFrameworkCore.Migrations;

namespace TracyShop.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRoleId",
                table: "AppUser",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_UserRoleId",
                table: "AppUser",
                column: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_UserRoles_UserRoleId",
                table: "AppUser",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_UserRoles_UserRoleId",
                table: "AppUser");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_UserRoleId",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "UserRoleId",
                table: "AppUser");
        }
    }
}
