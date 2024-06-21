using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueueOverflow.Web.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserInfoTableAndFKActionInQuestionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureLink",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "YearsOfJoining",
                table: "UserInfo");
			migrationBuilder.DropForeignKey(name: "FK_UsersReply_UserId",
				table: "Replies");
            migrationBuilder.DropForeignKey(name: "FK_UsersComment_UserId",
				table: "Comments");
			migrationBuilder.DropForeignKey(name: "FK_ReplyQuestion_QuestionId",
			   table: "Replies");
			migrationBuilder.DropForeignKey(name: "FK_CommentsQuestion_QuestionId",
			   table: "Comments");

			migrationBuilder.AddForeignKey(name: "FK_ReplyQuestion_QuestionId",
				table: "Replies", column: "QuestionId", principalTable: "Questions", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
			
            migrationBuilder.AddForeignKey(name: "FK_CommentsQuestion_QuestionId",
				table: "Comments", column: "QuestionId", principalTable: "Questions", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureLink",
                table: "UserInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "YearsOfJoining",
                table: "UserInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
			migrationBuilder.AddForeignKey(name: "FK_UsersComment_UserId",
				table: "Comments", column: "UserId", principalTable: "UserInfo", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
			
            migrationBuilder.AddForeignKey(name: "FK_UsersReply_UserId",
				table: "Replies", column: "UserId", principalTable: "UserInfo", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
			
            migrationBuilder.DropForeignKey(name: "FK_ReplyQuestion_QuestionId",
			   table: "Replies");
			migrationBuilder.DropForeignKey(name: "FK_CommentsQuestion_QuestionId",
			   table: "Comments");
		}
    }
}
