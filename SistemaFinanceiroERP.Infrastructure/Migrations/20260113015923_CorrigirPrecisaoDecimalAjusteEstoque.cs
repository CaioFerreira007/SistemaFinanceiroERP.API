using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaFinanceiroERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirPrecisaoDecimalAjusteEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Corrigir QuantidadeAnterior
            migrationBuilder.AlterColumn<decimal>(
                name: "QuantidadeAnterior",
                table: "ajusteestoque",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            // Corrigir QuantidadeNova
            migrationBuilder.AlterColumn<decimal>(
                name: "QuantidadeNova",
                table: "ajusteestoque",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            // Corrigir Diferenca
            migrationBuilder.AlterColumn<decimal>(
                name: "Diferenca",
                table: "ajusteestoque",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverter QuantidadeAnterior
            migrationBuilder.AlterColumn<decimal>(
                name: "QuantidadeAnterior",
                table: "ajusteestoque",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            // Reverter QuantidadeNova
            migrationBuilder.AlterColumn<decimal>(
                name: "QuantidadeNova",
                table: "ajusteestoque",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            // Reverter Diferenca
            migrationBuilder.AlterColumn<decimal>(
                name: "Diferenca",
                table: "ajusteestoque",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}