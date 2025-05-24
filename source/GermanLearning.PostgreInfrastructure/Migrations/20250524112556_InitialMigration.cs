using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GermanLearning.PostgreInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "integer", nullable: false),
                    TotalQuestions = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseResults_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GermanText = table.Column<string>(type: "text", nullable: false),
                    EnglishTranslation = table.Column<List<string>>(type: "jsonb", nullable: false),
                    SpanishTranslation = table.Column<List<string>>(type: "jsonb", nullable: false),
                    WordTypeId = table.Column<int>(type: "integer", nullable: false),
                    GenderId = table.Column<int>(type: "integer", nullable: true),
                    ExampleSentences = table.Column<List<string>>(type: "jsonb", nullable: false),
                    Synonyms = table.Column<List<string>>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GenderId1 = table.Column<int>(type: "integer", nullable: true),
                    WordTypeId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Words_Genders_GenderId1",
                        column: x => x.GenderId1,
                        principalTable: "Genders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Words_WordTypes_WordTypeId1",
                        column: x => x.WordTypeId1,
                        principalTable: "WordTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExerciseWord",
                columns: table => new
                {
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    WordsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseWord", x => new { x.ExerciseId, x.WordsId });
                    table.ForeignKey(
                        name: "FK_ExerciseWord_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseWord_Words_WordsId",
                        column: x => x.WordsId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordTopic",
                columns: table => new
                {
                    TopicsId = table.Column<Guid>(type: "uuid", nullable: false),
                    WordsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTopic", x => new { x.TopicsId, x.WordsId });
                    table.ForeignKey(
                        name: "FK_WordTopic_Topics_TopicsId",
                        column: x => x.TopicsId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordTopic_Words_WordsId",
                        column: x => x.WordsId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
                    { 1, "Masculine" },
                    { 2, "Feminine" },
                    { 3, "Neutral" }
                });

            migrationBuilder.InsertData(
                table: "WordTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
                    { 1, "Verb" },
                    { 2, "Noun" },
                    { 3, "Adjective" },
                    { 4, "Adverb" },
                    { 5, "Expression" },
                    { 6, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseResults_ExerciseId",
                table: "ExerciseResults",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseWord_WordsId",
                table: "ExerciseWord",
                column: "WordsId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_Name",
                table: "Topics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Words_GenderId1",
                table: "Words",
                column: "GenderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Words_WordTypeId1",
                table: "Words",
                column: "WordTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_WordTopic_WordsId",
                table: "WordTopic",
                column: "WordsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseResults");

            migrationBuilder.DropTable(
                name: "ExerciseWord");

            migrationBuilder.DropTable(
                name: "WordTopic");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "WordTypes");
        }
    }
}
