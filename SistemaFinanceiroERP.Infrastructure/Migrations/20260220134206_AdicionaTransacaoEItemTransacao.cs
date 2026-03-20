using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaFinanceiroERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTransacaoEItemTransacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalEstoqueId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Diferenca",
                table: "AjusteEstoque");

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroTransacao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmpresaVendedoraId = table.Column<int>(type: "int", nullable: false),
                    EmpresaCompradoraId = table.Column<int>(type: "int", nullable: false),
                    DataTransacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StatusTransacao = table.Column<int>(type: "int", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ObservacoesTransacao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacoes_Empresas_EmpresaCompradoraId",
                        column: x => x.EmpresaCompradoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transacoes_Empresas_EmpresaVendedoraId",
                        column: x => x.EmpresaVendedoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItensTransacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TransacaoId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensTransacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensTransacao_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensTransacao_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensTransacao_Transacoes_TransacaoId",
                        column: x => x.TransacaoId,
                        principalTable: "Transacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ItensTransacao_EmpresaId",
                table: "ItensTransacao",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensTransacao_ProdutoId",
                table: "ItensTransacao",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensTransacao_TransacaoId",
                table: "ItensTransacao",
                column: "TransacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_EmpresaCompradoraId",
                table: "Transacoes",
                column: "EmpresaCompradoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_EmpresaVendedoraId",
                table: "Transacoes",
                column: "EmpresaVendedoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_UsuarioId",
                table: "Transacoes",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensTransacao");

            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.AddColumn<int>(
                name: "LocalEstoqueId",
                table: "Produtos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Diferenca",
                table: "AjusteEstoque",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
