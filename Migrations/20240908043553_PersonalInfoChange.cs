using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeBuilder.Migrations
{
    /// <inheritdoc />
    public partial class PersonalInfoChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHubAccount",
                table: "PersonalInfo");

            migrationBuilder.DropColumn(
                name: "LinkedInURL",
                table: "PersonalInfo");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "PersonalInfo");

            migrationBuilder.DropColumn(
                name: "WebsiteURL",
                table: "PersonalInfo");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "PersonalInfo",
                type: "nvarchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalContactInfo",
                table: "PersonalInfo",
                type: "nvarchar(4000)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalContactInfo",
                table: "PersonalInfo");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "PersonalInfo",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubAccount",
                table: "PersonalInfo",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInURL",
                table: "PersonalInfo",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "PersonalInfo",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteURL",
                table: "PersonalInfo",
                type: "nvarchar(100)",
                nullable: true);
        }
    }
}
