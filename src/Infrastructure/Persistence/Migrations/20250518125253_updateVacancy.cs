using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateVacancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_vacancy_skills_vacancy_id",
                table: "vacancy_skills");

            migrationBuilder.AddForeignKey(
                name: "fk_vacancy_skills_vacancy_id",
                table: "vacancy_skills",
                column: "vacancy_id",
                principalTable: "vacancies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_vacancy_skills_vacancy_id",
                table: "vacancy_skills");

            migrationBuilder.AddForeignKey(
                name: "fk_vacancy_skills_vacancy_id",
                table: "vacancy_skills",
                column: "vacancy_id",
                principalTable: "vacancies",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
