using Microsoft.EntityFrameworkCore.Migrations;

namespace EVotingApplication.Data.Migrations
{
    public partial class updateCadnidate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "votes",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "votes",
                table: "Candidates");
        }
    }
}
