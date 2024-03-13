using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreIdentityApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class update_app_user_gender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Gender",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
