using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReminderToEmail.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreat225e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isSent",
                table: "Reminders",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSent",
                table: "Reminders");
        }
    }
}
