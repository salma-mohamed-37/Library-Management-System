using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Addduedate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Borrowed",
                table: "Borrowed");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Borrowed",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Borrowed",
                table: "Borrowed",
                columns: new[] { "UserId", "BookId", "BorrowDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Borrowed",
                table: "Borrowed");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Borrowed");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Borrowed",
                table: "Borrowed",
                columns: new[] { "UserId", "BookId", "BorrowDate", "ReturnDate" });
        }
    }
}
