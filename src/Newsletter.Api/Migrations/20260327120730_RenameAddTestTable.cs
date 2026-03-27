using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newsletter.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameAddTestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiveOnUtl",
                table: "TestSendingMessages",
                newName: "ReceiveOnUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiveOnUtc",
                table: "TestSendingMessages",
                newName: "ReceiveOnUtl");
        }
    }
}
