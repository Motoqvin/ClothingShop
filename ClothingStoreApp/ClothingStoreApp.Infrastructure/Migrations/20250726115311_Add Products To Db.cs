using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClothingStoreApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Soft and breathable cotton t-shirt perfect for everyday wear.", "https://images.unsplash.com/photo-1618354691214-31c9b27638f3?auto=format&fit=crop&w=600&q=80", "Classic White T-Shirt", 19.99m },
                    { 2, "Stylish blue denim jacket suitable for all seasons.", "https://images.unsplash.com/photo-1602810316636-d0f9e16e9b80?auto=format&fit=crop&w=600&q=80", "Denim Jacket", 59.99m },
                    { 3, "Comfy fleece hoodie for cool evenings and casual outings.", "https://images.unsplash.com/photo-1585386959984-a4155227c290?auto=format&fit=crop&w=600&q=80", "Black Hoodie", 39.50m },
                    { 4, "Modern slim fit jeans with stretchable comfort.", "https://images.unsplash.com/photo-1603394445910-5aa4f2030c7b?auto=format&fit=crop&w=600&q=80", "Slim Fit Jeans", 44.95m },
                    { 5, "Lightweight running sneakers ideal for daily wear.", "https://images.unsplash.com/photo-1589987601542-dcdbf7b0a1cf?auto=format&fit=crop&w=600&q=80", "Sneakers", 69.99m },
                    { 6, "Adjustable cotton cap for sun protection and style.", "https://images.unsplash.com/photo-1588776814546-b4d91d0bde5d?auto=format&fit=crop&w=600&q=80", "Baseball Cap", 14.99m },
                    { 7, "Light and breezy floral summer dress.", "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=600&q=80", "Summer Dress", 34.99m },
                    { 8, "Button-down formal shirt for work or events.", "https://images.unsplash.com/photo-1520962910013-1eaa9e2c92e3?auto=format&fit=crop&w=600&q=80", "Formal Shirt", 29.99m },
                    { 9, "Durable leather belt with polished metal buckle.", "https://images.unsplash.com/photo-1614270532526-2c74ac92cb2b?auto=format&fit=crop&w=600&q=80", "Leather Belt", 24.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
