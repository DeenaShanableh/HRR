using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRR.Migrations
{
    /// <inheritdoc />
    public partial class new_column_userId_employees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "userId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_userId",
                table: "Employees",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_userId",
                table: "Employees",
                column: "userId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_userId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_userId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Employees");
        }
    }
}
