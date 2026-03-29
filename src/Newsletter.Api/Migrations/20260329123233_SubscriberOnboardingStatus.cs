using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newsletter.Api.Migrations
{
    /// <inheritdoc />
    public partial class SubscriberOnboardingStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OnboardingCompletedAtUtc",
                table: "Subscribers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OnboardingFaultReason",
                table: "Subscribers",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OnboardingStatus",
                table: "Subscribers",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "Pending");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnboardingCompletedAtUtc",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "OnboardingFaultReason",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "OnboardingStatus",
                table: "Subscribers");
        }
    }
}
