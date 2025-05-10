using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class UptoOrderEntiy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Books_BookEntityBookId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Books_BookEntityBookId",
                table: "Wishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Users_UserId",
                table: "Wishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_BookEntityBookId",
                table: "Wishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BookEntityBookId",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Wishlists",
                newName: "WishList");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameColumn(
                name: "WishlistId",
                table: "WishList",
                newName: "WishListId");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlists_UserId",
                table: "WishList",
                newName: "IX_WishList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserId",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_BookEntityBookId",
                table: "Orders",
                newName: "IX_Orders_BookEntityBookId");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "WishList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishList",
                table: "WishList",
                column: "WishListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WishList_BookId",
                table: "WishList",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BookId",
                table: "Orders",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Books_BookEntityBookId",
                table: "Orders",
                column: "BookEntityBookId",
                principalTable: "Books",
                principalColumn: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Books_BookId",
                table: "Orders",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishList_Books_BookId",
                table: "WishList",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WishList_Users_UserId",
                table: "WishList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Books_BookEntityBookId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Books_BookId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_WishList_Books_BookId",
                table: "WishList");

            migrationBuilder.DropForeignKey(
                name: "FK_WishList_Users_UserId",
                table: "WishList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WishList",
                table: "WishList");

            migrationBuilder.DropIndex(
                name: "IX_WishList_BookId",
                table: "WishList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AddressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BookId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "WishList");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "WishList",
                newName: "Wishlists");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "WishListId",
                table: "Wishlists",
                newName: "WishlistId");

            migrationBuilder.RenameIndex(
                name: "IX_WishList_UserId",
                table: "Wishlists",
                newName: "IX_Wishlists_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Order",
                newName: "IX_Order_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BookEntityBookId",
                table: "Order",
                newName: "IX_Order_BookEntityBookId");

            migrationBuilder.AddColumn<int>(
                name: "BookEntityBookId",
                table: "Wishlists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Order",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists",
                column: "WishlistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_BookEntityBookId",
                table: "Wishlists",
                column: "BookEntityBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Books_BookEntityBookId",
                table: "Order",
                column: "BookEntityBookId",
                principalTable: "Books",
                principalColumn: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Books_BookEntityBookId",
                table: "Wishlists",
                column: "BookEntityBookId",
                principalTable: "Books",
                principalColumn: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Users_UserId",
                table: "Wishlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
