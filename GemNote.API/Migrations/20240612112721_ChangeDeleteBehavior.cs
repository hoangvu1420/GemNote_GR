using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GemNote.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardReviewSessions_AspNetUsers_AppUserId",
                table: "CardReviewSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_CardReviewSessions_Flashcards_FlashcardId",
                table: "CardReviewSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_CardReviewSessions_AspNetUsers_AppUserId",
                table: "CardReviewSessions",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardReviewSessions_Flashcards_FlashcardId",
                table: "CardReviewSessions",
                column: "FlashcardId",
                principalTable: "Flashcards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardReviewSessions_AspNetUsers_AppUserId",
                table: "CardReviewSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_CardReviewSessions_Flashcards_FlashcardId",
                table: "CardReviewSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_CardReviewSessions_AspNetUsers_AppUserId",
                table: "CardReviewSessions",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardReviewSessions_Flashcards_FlashcardId",
                table: "CardReviewSessions",
                column: "FlashcardId",
                principalTable: "Flashcards",
                principalColumn: "Id");
        }
    }
}
