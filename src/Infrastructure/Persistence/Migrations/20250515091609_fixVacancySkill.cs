using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fixVacancySkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_vacancy_skills_vacancies_vacancy_id1",
                table: "vacancy_skills");

            migrationBuilder.DropIndex(
                name: "ix_vacancy_skills_vacancy_id1",
                table: "vacancy_skills");

            migrationBuilder.DropColumn(
                name: "vacancy_id1",
                table: "vacancy_skills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "vacancy_id1",
                table: "vacancy_skills",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_vacancy_skills_vacancy_id1",
                table: "vacancy_skills",
                column: "vacancy_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_vacancy_skills_vacancies_vacancy_id1",
                table: "vacancy_skills",
                column: "vacancy_id1",
                principalTable: "vacancies",
                principalColumn: "id");
        }
    }
}
