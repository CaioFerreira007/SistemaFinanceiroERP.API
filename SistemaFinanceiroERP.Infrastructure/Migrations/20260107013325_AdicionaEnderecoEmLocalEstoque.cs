using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaFinanceiroERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaEnderecoEmLocalEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "LocaisEstoque",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "LocaisEstoque",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "LocaisEstoque",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Rua",
                table: "LocaisEstoque",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "LocaisEstoque");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "LocaisEstoque");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "LocaisEstoque");

            migrationBuilder.DropColumn(
                name: "Rua",
                table: "LocaisEstoque");
        }
    }
}
