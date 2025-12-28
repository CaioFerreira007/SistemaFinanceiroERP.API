Sistema Financeiro ERP
Sistema ERP desenvolvido em ASP.NET Core 8.0 com Clean Architecture para gestão empresarial multi-tenant.

Stack Tecnológico

.NET 8.0 / ASP.NET Core Web API
Entity Framework Core 8.0
MySQL 8.0
AutoMapper 12.0.1
Swagger/OpenAPI


Arquitetura
Projeto estruturado em 4 camadas seguindo Clean Architecture:

API: Controllers, DTOs, AutoMapper Profiles
Application: Casos de uso e serviços
Domain: Entidades, enums, regras de negócio
Infrastructure: EF Core, repositórios, migrations


Funcionalidades
Gestão de Empresas
CRUD completo com soft delete, tipos de empresa (Fornecedora, Compradora, Ambos), dados cadastrais e CNPJ.
Gestão de Usuários
CRUD de usuários vinculados a empresas, controle de acesso e perfis.
Gestão de Produtos
CRUD de produtos com controle de estoque, categorização, precificação e código de barras.

Padrões Implementados

DTOs: Separação entre domínio e transferência de dados (Create, Update, Response)
AutoMapper: Mapeamento automático entre DTOs e entidades (88% redução de código)
Soft Delete: Desativação lógica com campo Ativo
Auditoria: Timestamps de criação e atualização em todas as entidades


Configuração
Pré-requisitos

.NET SDK 8.0+
MySQL 8.0+

String de Conexão
Configurar em appsettings.json:
json{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sistema_financeiro_erp;Uid=root;Pwd=senha;"
  }
}
Executar
bashdotnet ef database update --project SistemaFinanceiroERP.Infrastructure
dotnet run --project SistemaFinanceiroERP.API
Acesse: https://localhost:7206/swagger

Endpoints
Empresas: POST, GET, PUT, DELETE /api/empresa
Usuários: POST, GET, PUT, DELETE /api/usuario
Produtos: POST, GET, PUT, DELETE /api/produto

Roadmap

Fase 4: FluentValidation (CNPJ, email, senha forte)
Fase 5: JWT, autenticação, autorização, onboarding
Fase 6: Multi-tenancy com isolamento por empresa
Fase 7: Repository Pattern e Unit of Work
Fase 8: Paginação, filtros, logs estruturados
Fase 9: Testes unitários e de integração


Pendências Técnicas

Remover EmpresaId dos DTOs após implementar JWT
Implementar hash de senhas (bcrypt)
Centralizar validações em repositórios
Middleware global de exceções


Autor
Caio Ferreira
GitHub: github.com/CaioFerreira007

Status
Em desenvolvimento ativo - (Fase 3/9)
Última atualização: Dezembro 2025
