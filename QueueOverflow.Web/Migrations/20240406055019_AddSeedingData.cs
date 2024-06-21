using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueOverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("3a6cd56b-bb0d-4ed5-aff6-183e54efbab8"), null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("54bbfa6a-c47a-4eb5-b244-dcd9ebf9062f"), 0, "e127980a-dbfc-4f9d-90f1-da9ba29a1b3b", "admin@gmail.com", true, null, null, false, null, null, null, "AQAAAAIAAYagAAAAEMZuowgbN90FAp0dQ/Ntby55gPvWCaeKlqfuMgQhyut2hxWl+kbNX3G/Gwa6ayumvA==", null, false, "URNPMVYKJBV6NYGP7M7LOWANMHZCMUUK", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("3a6cd56b-bb0d-4ed5-aff6-183e54efbab8"), new Guid("54bbfa6a-c47a-4eb5-b244-dcd9ebf9062f") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("3a6cd56b-bb0d-4ed5-aff6-183e54efbab8"), new Guid("54bbfa6a-c47a-4eb5-b244-dcd9ebf9062f") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3a6cd56b-bb0d-4ed5-aff6-183e54efbab8"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("54bbfa6a-c47a-4eb5-b244-dcd9ebf9062f"));
        }
    }
}
