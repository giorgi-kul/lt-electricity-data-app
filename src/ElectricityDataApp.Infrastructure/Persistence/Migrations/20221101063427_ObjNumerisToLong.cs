using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectricityDataApp.Infrastructure.Migrations
{
    public partial class ObjNumerisToLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ObjNumeris",
                table: "DataItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ObjNumeris",
                table: "DataItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
