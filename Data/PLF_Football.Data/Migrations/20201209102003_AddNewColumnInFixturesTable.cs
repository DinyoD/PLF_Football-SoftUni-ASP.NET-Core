using Microsoft.EntityFrameworkCore.Migrations;

namespace PLF_Football.Data.Migrations
{
    public partial class AddNewColumnInFixturesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Fixtures",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Fixtures");
        }
    }
}
