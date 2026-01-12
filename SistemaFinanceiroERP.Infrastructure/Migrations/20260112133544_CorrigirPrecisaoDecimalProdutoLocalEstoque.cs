using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaFinanceiroERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirPrecisaoDecimalProdutoLocalEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Corrigir ProdutosLocaisEstoque.QuantidadeNoLocal
            migrationBuilder.AlterColumn<decimal>(
                name: "QuantidadeNoLocal",
                table: "ProdutosLocaisEstoque",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            // Corrigir MovimentacoesEstoque.Quantidade
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "MovimentacoesEstoque",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverter ProdutosLocaisEstoque.QuantidadeNoLocal
            migrationBuilder.AlterColumn<decimal>(
                name: "QuantidadeNoLocal",
                table: "ProdutosLocaisEstoque",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            // Reverter MovimentacoesEstoque.Quantidade
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "MovimentacoesEstoque",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}