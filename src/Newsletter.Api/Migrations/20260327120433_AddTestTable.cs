using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newsletter.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestSendingMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SendingEmail = table.Column<string>(type: "text", nullable: false),
                    ReceivedEmail = table.Column<string>(type: "text", nullable: false),
                    SentOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceiveOnUtl = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSendingMessages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestSendingMessages");
        }
    }
}
