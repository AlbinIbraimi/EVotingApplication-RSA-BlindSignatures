using Microsoft.EntityFrameworkCore.Migrations;

namespace EVotingApplication.Data.Migrations
{
    public partial class uppdatedCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "voted",
                table: "VotersCredentials",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "voted",
                table: "VotersCredentials");
        }
    }
}
