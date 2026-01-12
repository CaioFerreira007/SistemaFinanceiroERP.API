using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaFinanceiroERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaAjusteEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AjusteEstoque",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LocalEstoqueId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    QuantidadeAnterior = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Diferenca = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    QuantidadeNova = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Observacao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataDoAjuste = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AjusteEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AjusteEstoque_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AjusteEstoque_LocaisEstoque_LocalEstoqueId",
                        column: x => x.LocalEstoqueId,
                        principalTable: "LocaisEstoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AjusteEstoque_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AjusteEstoque_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AjusteEstoque_EmpresaId",
                table: "AjusteEstoque",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_AjusteEstoque_LocalEstoqueId",
                table: "AjusteEstoque",
                column: "LocalEstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_AjusteEstoque_ProdutoId",
                table: "AjusteEstoque",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_AjusteEstoque_UsuarioId",
                table: "AjusteEstoque",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AjusteEstoque");
        }
    }
}
