using Microsoft.EntityFrameworkCore.Migrations;

namespace t3winc.version.data.Migrations
{
    public partial class SufixColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Suffix",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Version",
                columns: new[] { "Id", "Key", "Organization" },
                values: new object[] { 1, "6b42bb5b-45ae-44ba-b3ef-53b9ef342cfa", "The Web We Weave, Inc." });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Major", "Master", "Minor", "Name", "Patch", "Revision", "VersionId" },
                values: new object[] { 1, 1, "", 1, "AffirmStore", 0, 214, 1 });

            migrationBuilder.InsertData(
                table: "Branch",
                columns: new[] { "Id", "Major", "Minor", "Name", "Patch", "ProductId", "Revision", "Status", "Suffix", "Version" },
                values: new object[] { 1, 1, 2, "feature/convertChargeToSubscription", 0, 1, 237, "Active", null, "1.2.0-alpha.237" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Version",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Suffix",
                table: "Branch");
        }
    }
}
