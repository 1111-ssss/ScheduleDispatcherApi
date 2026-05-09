using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class draftSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_removal_sched1",
                table: "removal");

            migrationBuilder.DropForeignKey(
                name: "fk_removal_sched2",
                table: "removal");

            migrationBuilder.DropIndex(
                name: "IX_removal_id_schedule1",
                table: "removal");

            migrationBuilder.DropIndex(
                name: "IX_removal_id_schedule2",
                table: "removal");

            migrationBuilder.RenameColumn(
                name: "id_schedule2",
                table: "removal",
                newName: "IdSchedule2");

            migrationBuilder.RenameColumn(
                name: "id_schedule1",
                table: "removal",
                newName: "IdSchedule1");

            migrationBuilder.CreateTable(
                name: "day_schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    group_name = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_day_schedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lessons",
                columns: table => new
                {
                    groupid = table.Column<int>(type: "integer", nullable: false),
                    LessonNumber = table.Column<int>(type: "integer", nullable: false),
                    subject_1 = table.Column<string>(type: "text", nullable: false),
                    subject_2 = table.Column<string>(type: "text", nullable: false),
                    teacher_1 = table.Column<string>(type: "text", nullable: false),
                    teacher_2 = table.Column<string>(type: "text", nullable: false),
                    classroom_1 = table.Column<string>(type: "text", nullable: false),
                    classroom_2 = table.Column<string>(type: "text", nullable: false),
                    starttime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    endtime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    split = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lessons", x => new { x.groupid, x.LessonNumber });
                    table.ForeignKey(
                        name: "FK_lessons_day_schedule_groupid",
                        column: x => x.groupid,
                        principalTable: "day_schedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_removal_IdSchedule1",
                table: "removal",
                column: "IdSchedule1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_removal_IdSchedule2",
                table: "removal",
                column: "IdSchedule2",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_removal_schedule_IdSchedule1",
                table: "removal",
                column: "IdSchedule1",
                principalTable: "schedule",
                principalColumn: "id_schedule");

            migrationBuilder.AddForeignKey(
                name: "FK_removal_schedule_IdSchedule2",
                table: "removal",
                column: "IdSchedule2",
                principalTable: "schedule",
                principalColumn: "id_schedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_removal_schedule_IdSchedule1",
                table: "removal");

            migrationBuilder.DropForeignKey(
                name: "FK_removal_schedule_IdSchedule2",
                table: "removal");

            migrationBuilder.DropTable(
                name: "lessons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "day_schedule");

            migrationBuilder.DropIndex(
                name: "IX_removal_IdSchedule1",
                table: "removal");

            migrationBuilder.DropIndex(
                name: "IX_removal_IdSchedule2",
                table: "removal");

            migrationBuilder.RenameColumn(
                name: "IdSchedule2",
                table: "removal",
                newName: "id_schedule2");

            migrationBuilder.RenameColumn(
                name: "IdSchedule1",
                table: "removal",
                newName: "id_schedule1");

            migrationBuilder.CreateIndex(
                name: "IX_removal_id_schedule1",
                table: "removal",
                column: "id_schedule1");

            migrationBuilder.CreateIndex(
                name: "IX_removal_id_schedule2",
                table: "removal",
                column: "id_schedule2");

            migrationBuilder.AddForeignKey(
                name: "fk_removal_sched1",
                table: "removal",
                column: "id_schedule1",
                principalTable: "schedule",
                principalColumn: "id_schedule");

            migrationBuilder.AddForeignKey(
                name: "fk_removal_sched2",
                table: "removal",
                column: "id_schedule2",
                principalTable: "schedule",
                principalColumn: "id_schedule");
        }
    }
}
