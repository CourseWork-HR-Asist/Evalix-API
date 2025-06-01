using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "vacancies",
                type: "varchar(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)");

            migrationBuilder.AddColumn<string>(
                name: "original_file_name",
                table: "resumes",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "original_file_name",
                table: "resumes");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "vacancies",
                type: "varchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000)");
        }
    }
}
