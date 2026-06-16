using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechMoveGLMS.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "ContactDetails", "Name", "Region" },
                values: new object[,]
                {
                    { 1, "info@techmove.com", "TechMove Ltd", "Durban" },
                    { 2, "support@globallogistics.com", "Global Logistics", "Cape Town" }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ClientId", "EndDate", "ServiceLevel", "SignedAgreementPath", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2027, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium", "contracts/techmove.pdf", new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { 2, 2, new DateTime(2028, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Standard", "contracts/global.pdf", new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" }
                });
        }
    }
}
