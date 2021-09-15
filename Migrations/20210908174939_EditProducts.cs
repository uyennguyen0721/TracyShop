using Microsoft.EntityFrameworkCore.Migrations;

namespace TracyShop.Migrations
{
    public partial class EditProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Trandemark_TrandemarkId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Trandemark");

            migrationBuilder.DropIndex(
                name: "IX_Product_TrandemarkId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TrandemarkId",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "Product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Trandemark",
                table: "Product",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ShoppingFee",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Trandemark",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ShoppingFee",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "TrandemarkId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Trandemark",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trandemark", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_TrandemarkId",
                table: "Product",
                column: "TrandemarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Trandemark_TrandemarkId",
                table: "Product",
                column: "TrandemarkId",
                principalTable: "Trandemark",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
