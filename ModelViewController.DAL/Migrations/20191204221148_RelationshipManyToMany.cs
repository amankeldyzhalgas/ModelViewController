using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelViewController.DAL.Migrations
{
    public partial class RelationshipManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAward",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    AwardId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAward", x => new { x.UserId, x.AwardId });
                    table.ForeignKey(
                        name: "FK_UserAward_Awards_AwardId",
                        column: x => x.AwardId,
                        principalTable: "Awards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAward_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAward_AwardId",
                table: "UserAward",
                column: "AwardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAward");
        }
    }
}
