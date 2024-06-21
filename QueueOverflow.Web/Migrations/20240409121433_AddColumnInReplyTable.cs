using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueOverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnInReplyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "QuestionTags");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("54bbfa6a-c47a-4eb5-b244-dcd9ebf9062f"),
                column: "ConcurrencyStamp",
                value: "77468b52-74b7-4046-b8ac-429d412a55b9");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Questions_QuestionId",
                table: "Tags",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Replies");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "QuestionTags",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTags", x => new { x.TagId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_QuestionTags_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("54bbfa6a-c47a-4eb5-b244-dcd9ebf9062f"),
                column: "ConcurrencyStamp",
                value: "e127980a-dbfc-4f9d-90f1-da9ba29a1b3b");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTags_QuestionId",
                table: "QuestionTags",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Questions_QuestionId",
                table: "Tags",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
