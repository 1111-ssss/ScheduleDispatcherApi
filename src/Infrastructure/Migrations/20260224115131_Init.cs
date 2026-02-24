using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "classrooms",
                columns: table => new
                {
                    id_classroom = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    classroom_number = table.Column<int>(type: "integer", nullable: true),
                    is_pc_classroom = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("classrooms_pkey", x => x.id_classroom);
                });

            migrationBuilder.CreateTable(
                name: "employer",
                columns: table => new
                {
                    cn_e = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    surname = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    name = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    father_name = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("employer_pkey", x => x.cn_e);
                });

            migrationBuilder.CreateTable(
                name: "schedule",
                columns: table => new
                {
                    id_schedule = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    lessonnumber = table.Column<int>(type: "integer", nullable: true),
                    date_1 = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    date_2 = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_practical = table.Column<bool>(type: "boolean", nullable: true),
                    is_over = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("schedule_pkey", x => x.id_schedule);
                });

            migrationBuilder.CreateTable(
                name: "teacher",
                columns: table => new
                {
                    cn_t = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    idcategory = table.Column<int>(type: "integer", nullable: true),
                    idck = table.Column<int>(type: "integer", nullable: true),
                    cn_e = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("teacher_pkey", x => x.cn_t);
                    table.ForeignKey(
                        name: "fk_teacher_employer",
                        column: x => x.cn_e,
                        principalTable: "employer",
                        principalColumn: "cn_e");
                });

            migrationBuilder.CreateTable(
                name: "exam",
                columns: table => new
                {
                    id_schedule = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("exam_pkey", x => x.id_schedule);
                    table.ForeignKey(
                        name: "fk_exam_schedule",
                        column: x => x.id_schedule,
                        principalTable: "schedule",
                        principalColumn: "id_schedule");
                });

            migrationBuilder.CreateTable(
                name: "okr",
                columns: table => new
                {
                    id_schedule = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("okr_pkey", x => x.id_schedule);
                    table.ForeignKey(
                        name: "fk_okr_schedule",
                        column: x => x.id_schedule,
                        principalTable: "schedule",
                        principalColumn: "id_schedule");
                });

            migrationBuilder.CreateTable(
                name: "removal",
                columns: table => new
                {
                    id_removal = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_schedule1 = table.Column<int>(type: "integer", nullable: false),
                    id_schedule2 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("removal_pkey", x => x.id_removal);
                    table.ForeignKey(
                        name: "fk_removal_sched1",
                        column: x => x.id_schedule1,
                        principalTable: "schedule",
                        principalColumn: "id_schedule");
                    table.ForeignKey(
                        name: "fk_removal_sched2",
                        column: x => x.id_schedule2,
                        principalTable: "schedule",
                        principalColumn: "id_schedule");
                });

            migrationBuilder.CreateTable(
                name: "specialty",
                columns: table => new
                {
                    cn_spec = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    fullname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    educationperiod = table.Column<int>(type: "integer", nullable: true),
                    department = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    cn_t = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("specialty_pkey", x => x.cn_spec);
                    table.ForeignKey(
                        name: "fk_specialty_teacher",
                        column: x => x.cn_t,
                        principalTable: "teacher",
                        principalColumn: "cn_t");
                });

            migrationBuilder.CreateTable(
                name: "group",
                columns: table => new
                {
                    cn_g = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Name = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    cours = table.Column<int>(type: "integer", nullable: false),
                    cn_spec = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("group_pkey", x => x.cn_g);
                    table.ForeignKey(
                        name: "fk_group_specialty",
                        column: x => x.cn_spec,
                        principalTable: "specialty",
                        principalColumn: "cn_spec");
                });

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    id_subject = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fullname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    optional = table.Column<bool>(type: "boolean", nullable: true),
                    totalhourcount = table.Column<int>(type: "integer", nullable: false),
                    practichourcount = table.Column<int>(type: "integer", nullable: false),
                    pc_class_need = table.Column<bool>(type: "boolean", nullable: false),
                    cn_spec = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("subject_pkey", x => x.id_subject);
                    table.ForeignKey(
                        name: "fk_subject_specialty",
                        column: x => x.cn_spec,
                        principalTable: "specialty",
                        principalColumn: "cn_spec");
                });

            migrationBuilder.CreateTable(
                name: "curator",
                columns: table => new
                {
                    id_curator = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Primary = table.Column<bool>(type: "boolean", nullable: false),
                    cn_t = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    cn_g = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("curator_pkey", x => x.id_curator);
                    table.ForeignKey(
                        name: "fk_curator_group",
                        column: x => x.cn_g,
                        principalTable: "group",
                        principalColumn: "cn_g");
                    table.ForeignKey(
                        name: "fk_curator_teacher",
                        column: x => x.cn_t,
                        principalTable: "teacher",
                        principalColumn: "cn_t");
                });

            migrationBuilder.CreateTable(
                name: "limitation_on_choice_of_audience_subject",
                columns: table => new
                {
                    id_subject = table.Column<int>(type: "integer", nullable: false),
                    id_classroom = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("limitation_on_choice_of_audience_subject_pkey", x => new { x.id_subject, x.id_classroom });
                    table.ForeignKey(
                        name: "fk_lim_classroom",
                        column: x => x.id_classroom,
                        principalTable: "classrooms",
                        principalColumn: "id_classroom");
                    table.ForeignKey(
                        name: "fk_lim_subject",
                        column: x => x.id_subject,
                        principalTable: "subject",
                        principalColumn: "id_subject");
                });

            migrationBuilder.CreateTable(
                name: "subject_teacher",
                columns: table => new
                {
                    idsubject_teacher = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    id_subject = table.Column<int>(type: "integer", nullable: false),
                    cn_g = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    cn_t = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("subject_teacher_pkey", x => x.idsubject_teacher);
                    table.ForeignKey(
                        name: "fk_subj_teacher_group",
                        column: x => x.cn_g,
                        principalTable: "group",
                        principalColumn: "cn_g");
                    table.ForeignKey(
                        name: "fk_subj_teacher_subject",
                        column: x => x.id_subject,
                        principalTable: "subject",
                        principalColumn: "id_subject");
                    table.ForeignKey(
                        name: "fk_subj_teacher_teacher",
                        column: x => x.cn_t,
                        principalTable: "teacher",
                        principalColumn: "cn_t");
                });

            migrationBuilder.CreateTable(
                name: "subject_teacher_schedule",
                columns: table => new
                {
                    idsubject_teacher = table.Column<int>(type: "integer", nullable: false),
                    id_schedule = table.Column<int>(type: "integer", nullable: false),
                    practical_hours_2term = table.Column<int>(type: "integer", nullable: true),
                    group_split = table.Column<bool>(type: "boolean", nullable: true),
                    lecture_hours_1term = table.Column<int>(type: "integer", nullable: true),
                    lecture_hours_2term = table.Column<int>(type: "integer", nullable: true),
                    practical_hours_1term = table.Column<int>(type: "integer", nullable: true),
                    examination_hours = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("subject_teacher_schedule_pkey", x => new { x.idsubject_teacher, x.id_schedule });
                    table.ForeignKey(
                        name: "fk_sts_schedule",
                        column: x => x.id_schedule,
                        principalTable: "schedule",
                        principalColumn: "id_schedule");
                    table.ForeignKey(
                        name: "fk_sts_subj_teacher",
                        column: x => x.idsubject_teacher,
                        principalTable: "subject_teacher",
                        principalColumn: "idsubject_teacher");
                });

            migrationBuilder.CreateTable(
                name: "subject_teacher_semester",
                columns: table => new
                {
                    idsubject_teacher_semester = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    semester1 = table.Column<bool>(type: "boolean", nullable: true),
                    semester2 = table.Column<bool>(type: "boolean", nullable: true),
                    idsubject_teacher = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("subject_teacher_semester_pkey", x => x.idsubject_teacher_semester);
                    table.ForeignKey(
                        name: "fk_stsem_subj_teacher",
                        column: x => x.idsubject_teacher,
                        principalTable: "subject_teacher",
                        principalColumn: "idsubject_teacher");
                });

            migrationBuilder.CreateIndex(
                name: "IX_curator_cn_g",
                table: "curator",
                column: "cn_g");

            migrationBuilder.CreateIndex(
                name: "IX_curator_cn_t",
                table: "curator",
                column: "cn_t");

            migrationBuilder.CreateIndex(
                name: "IX_group_cn_spec",
                table: "group",
                column: "cn_spec");

            migrationBuilder.CreateIndex(
                name: "IX_limitation_on_choice_of_audience_subject_id_classroom",
                table: "limitation_on_choice_of_audience_subject",
                column: "id_classroom");

            migrationBuilder.CreateIndex(
                name: "IX_removal_id_schedule1",
                table: "removal",
                column: "id_schedule1");

            migrationBuilder.CreateIndex(
                name: "IX_removal_id_schedule2",
                table: "removal",
                column: "id_schedule2");

            migrationBuilder.CreateIndex(
                name: "IX_specialty_cn_t",
                table: "specialty",
                column: "cn_t");

            migrationBuilder.CreateIndex(
                name: "IX_subject_cn_spec",
                table: "subject",
                column: "cn_spec");

            migrationBuilder.CreateIndex(
                name: "IX_subject_teacher_cn_g",
                table: "subject_teacher",
                column: "cn_g");

            migrationBuilder.CreateIndex(
                name: "IX_subject_teacher_cn_t",
                table: "subject_teacher",
                column: "cn_t");

            migrationBuilder.CreateIndex(
                name: "IX_subject_teacher_id_subject",
                table: "subject_teacher",
                column: "id_subject");

            migrationBuilder.CreateIndex(
                name: "IX_subject_teacher_schedule_id_schedule",
                table: "subject_teacher_schedule",
                column: "id_schedule");

            migrationBuilder.CreateIndex(
                name: "IX_subject_teacher_semester_idsubject_teacher",
                table: "subject_teacher_semester",
                column: "idsubject_teacher");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_cn_e",
                table: "teacher",
                column: "cn_e");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "curator");

            migrationBuilder.DropTable(
                name: "exam");

            migrationBuilder.DropTable(
                name: "limitation_on_choice_of_audience_subject");

            migrationBuilder.DropTable(
                name: "okr");

            migrationBuilder.DropTable(
                name: "removal");

            migrationBuilder.DropTable(
                name: "subject_teacher_schedule");

            migrationBuilder.DropTable(
                name: "subject_teacher_semester");

            migrationBuilder.DropTable(
                name: "classrooms");

            migrationBuilder.DropTable(
                name: "schedule");

            migrationBuilder.DropTable(
                name: "subject_teacher");

            migrationBuilder.DropTable(
                name: "group");

            migrationBuilder.DropTable(
                name: "subject");

            migrationBuilder.DropTable(
                name: "specialty");

            migrationBuilder.DropTable(
                name: "teacher");

            migrationBuilder.DropTable(
                name: "employer");
        }
    }
}
