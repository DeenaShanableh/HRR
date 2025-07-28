using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRR.Migrations
{
    /// <inheritdoc />
    public partial class foreign_loopkups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Postion",
                table: "Employees");

            migrationBuilder.AddColumn<long>(
                name: "PostionId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PostionId",
                table: "Employees",
                column: "PostionId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TypeId",
                table: "Departments",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Lookups_TypeId",
                table: "Departments",
                column: "TypeId",
                principalTable: "Lookups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Lookups_PostionId",
                table: "Employees",
                column: "PostionId",
                principalTable: "Lookups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Lookups_TypeId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Lookups_PostionId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PostionId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Departments_TypeId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "PostionId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Departments");

            migrationBuilder.AddColumn<string>(
                name: "Postion",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
