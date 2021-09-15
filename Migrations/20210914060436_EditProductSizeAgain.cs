using Microsoft.EntityFrameworkCore.Migrations;

namespace TracyShop.Migrations
{
    public partial class EditProductSizeAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSize_Product_ProductsId",
                table: "ProductSize");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSize_Sizes_SizesId",
                table: "ProductSize");

            migrationBuilder.RenameColumn(
                name: "SizesId",
                table: "ProductSize",
                newName: "SizeId");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "ProductSize",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSize_SizesId",
                table: "ProductSize",
                newName: "IX_ProductSize_SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSize_Product_ProductId",
                table: "ProductSize",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSize_Sizes_SizeId",
                table: "ProductSize",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSize_Product_ProductId",
                table: "ProductSize");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSize_Sizes_SizeId",
                table: "ProductSize");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "ProductSize",
                newName: "SizesId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductSize",
                newName: "ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSize_SizeId",
                table: "ProductSize",
                newName: "IX_ProductSize_SizesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSize_Product_ProductsId",
                table: "ProductSize",
                column: "ProductsId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSize_Sizes_SizesId",
                table: "ProductSize",
                column: "SizesId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
