using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RollCallApp.Migrations
{
    public partial class create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ATTENDANCE_ROLL_CALL",
                columns: table => new
                {
                    ID = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    registerID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    dateCheck = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    checkAttendance = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_ROLL_CALL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CONTACT",
                columns: table => new
                {
                    ID = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    title = table.Column<string>(maxLength: 255, nullable: true),
                    content = table.Column<string>(maxLength: 255, nullable: true),
                    StudentID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    SubjectID = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTACT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NOTIFICATION",
                columns: table => new
                {
                    ID = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    title = table.Column<string>(maxLength: 255, nullable: true),
                    content = table.Column<string>(maxLength: 255, nullable: true),
                    subjectID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    teacherID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    dateCreate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOTIFICATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PROFILE_STUDENT",
                columns: table => new
                {
                    profileID = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    fullName = table.Column<string>(maxLength: 255, nullable: true),
                    phoneNumber = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    email = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    address = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    classID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    departmentID = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Avatar = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROFILE_STUDENT", x => x.profileID);
                });

            migrationBuilder.CreateTable(
                name: "PROFILE_TEACHER",
                columns: table => new
                {
                    profileID = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    fullName = table.Column<string>(maxLength: 255, nullable: true),
                    phoneNumber = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    email = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    address = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    departmentID = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    specialize = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Avatar = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROFILE_TEACHER", x => x.profileID);
                });

            migrationBuilder.CreateTable(
                name: "REGISTER_SUBJECT",
                columns: table => new
                {
                    registerID = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    studentID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    teacherID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    subjectID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    dateStart = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    dateEnd = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REGISTER_SUBJECT", x => x.registerID);
                });

            migrationBuilder.CreateTable(
                name: "STUDENT",
                columns: table => new
                {
                    studentID = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    profileID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    personID = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    faceID = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENT", x => x.studentID);
                });

            migrationBuilder.CreateTable(
                name: "SUBJECT",
                columns: table => new
                {
                    subjectID = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    subjectName = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUBJECT", x => x.subjectID);
                });

            migrationBuilder.CreateTable(
                name: "TEACHER",
                columns: table => new
                {
                    teacherID = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    profileID = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEACHER", x => x.teacherID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ATTENDANCE_ROLL_CALL");

            migrationBuilder.DropTable(
                name: "CONTACT");

            migrationBuilder.DropTable(
                name: "NOTIFICATION");

            migrationBuilder.DropTable(
                name: "PROFILE_STUDENT");

            migrationBuilder.DropTable(
                name: "PROFILE_TEACHER");

            migrationBuilder.DropTable(
                name: "REGISTER_SUBJECT");

            migrationBuilder.DropTable(
                name: "STUDENT");

            migrationBuilder.DropTable(
                name: "SUBJECT");

            migrationBuilder.DropTable(
                name: "TEACHER");
        }
    }
}
