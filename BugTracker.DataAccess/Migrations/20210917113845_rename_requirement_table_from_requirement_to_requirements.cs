using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.DataAccess.Migrations
{
    public partial class rename_requirement_table_from_requirement_to_requirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requirement_Projects_ProjectId",
                table: "Requirement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requirement",
                table: "Requirement");

            migrationBuilder.RenameTable(
                name: "Requirement",
                newName: "Requirements");

            migrationBuilder.RenameIndex(
                name: "IX_Requirement_ProjectId",
                table: "Requirements",
                newName: "IX_Requirements_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requirements",
                table: "Requirements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requirements_Projects_ProjectId",
                table: "Requirements",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requirements_Projects_ProjectId",
                table: "Requirements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requirements",
                table: "Requirements");

            migrationBuilder.RenameTable(
                name: "Requirements",
                newName: "Requirement");

            migrationBuilder.RenameIndex(
                name: "IX_Requirements_ProjectId",
                table: "Requirement",
                newName: "IX_Requirement_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requirement",
                table: "Requirement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requirement_Projects_ProjectId",
                table: "Requirement",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
