using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueOverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnInQuestionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Questions");
        }
    }
}
