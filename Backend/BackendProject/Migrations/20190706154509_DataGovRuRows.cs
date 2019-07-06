using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TemplateApp.Migrations
{
    public partial class DataGovRuRows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataGovRuEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    title = table.Column<string>(nullable: true),
                    organization = table.Column<string>(nullable: true),
                    organization_name = table.Column<string>(nullable: true),
                    topic = table.Column<string>(nullable: true),
                    identifier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataGovRuEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataGovRuEntryRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EntryId = table.Column<int>(nullable: true),
                    Row = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataGovRuEntryRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataGovRuEntryRows_DataGovRuEntries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "DataGovRuEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataGovRuEntryRows_EntryId",
                table: "DataGovRuEntryRows",
                column: "EntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataGovRuEntryRows");

            migrationBuilder.DropTable(
                name: "DataGovRuEntries");
        }
    }
}
