using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBCV.Migrations
{
    /// <inheritdoc />
    public partial class Fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicantId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Resumes_ResumeId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_AspNetUsers_JobSeekerId",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "Certifications",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "CoverLetter",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "WorkExperience",
                table: "Resumes",
                newName: "Experience");

            migrationBuilder.RenameColumn(
                name: "JobSeekerId",
                table: "Resumes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Resumes_JobSeekerId",
                table: "Resumes",
                newName: "IX_Resumes_UserId");

            migrationBuilder.RenameColumn(
                name: "Deadline",
                table: "Jobs",
                newName: "DeadlineDate");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "JobApplications",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_ApplicantId",
                table: "JobApplications",
                newName: "IX_JobApplications_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Resumes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Salary",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Jobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResumeId",
                table: "JobApplications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "JobApplications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_UserId",
                table: "JobApplications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Resumes_ResumeId",
                table: "JobApplications",
                column: "ResumeId",
                principalTable: "Resumes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_AspNetUsers_UserId",
                table: "Resumes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_UserId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Resumes_ResumeId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_AspNetUsers_UserId",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Resumes",
                newName: "JobSeekerId");

            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "Resumes",
                newName: "WorkExperience");

            migrationBuilder.RenameIndex(
                name: "IX_Resumes_UserId",
                table: "Resumes",
                newName: "IX_Resumes_JobSeekerId");

            migrationBuilder.RenameColumn(
                name: "DeadlineDate",
                table: "Jobs",
                newName: "Deadline");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "JobApplications",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications",
                newName: "IX_JobApplications_ApplicantId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "Resumes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certifications",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ResumeId",
                table: "JobApplications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CoverLetter",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicantId",
                table: "JobApplications",
                column: "ApplicantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Resumes_ResumeId",
                table: "JobApplications",
                column: "ResumeId",
                principalTable: "Resumes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_AspNetUsers_JobSeekerId",
                table: "Resumes",
                column: "JobSeekerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
