using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialStudenData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "DOB", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "SKLM", new DateTime(1997, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "apk@gmail.com", "APK" },
                    { 2, "VIZAG", new DateTime(2000, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "poojita@gmail.com", "Poojita" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
