🏢 Sistema ERP/Marketplace B2B Multi-Tenant Um sistema de gerenciamento empresarial (ERP) completo desenvolvido com ASP.NET Core 8, C# 12 e MySQL, seguindo Clean Architecture e padrões profissionais de desenvolvimento. 📋 Visão Geral Este projeto implementa uma plataforma B2B onde empresas cadastradas podem realizar transações comerciais entre si. Diferente de um ERP tradicional com cadastros separados de "clientes" e "fornecedores", aqui toda entidade é uma empresa que pode atuar como compradora, vendedora ou ambas. Stack Tecnológico:

Backend: ASP.NET Core 8.0 Linguagem: C# 12 Banco de Dados: MySQL ORM: Entity Framework Core Arquitetura: Clean Architecture (4 camadas) Autenticação: JWT Bearer Documentação: Swagger/OpenAPI

🎯 Funcionalidades Implementadas ✅ Fase 1: Fundação

Autenticação e autorização com JWT Sistema multi-tenant com isolamento de dados Gestão de empresas e usuários Controle de acesso baseado em papéis

✅ Fase 2: Estoque

Cadastro de produtos Múltiplos locais de armazenamento Movimentações de estoque (entrada/saída) Ajustes de inventário Consulta de estoque baixo Histórico completo de movimentações

✅ Fase 3.1: Transações B2B

Entidades de Transação e ItemTransacao Transações entre duas empresas diferentes Query Filters especializados para B2B Endpoints para criar e consultar transações Endpoints de leitura para itens de transação Validações de negócio (empresa não transaciona consigo mesma) Geração automática de número único (TRN-YYYY-000001)

📊 Arquitetura O projeto segue Clean Architecture em 4 camadas: SistemaFinanceiroERP/ ├── Domain/ # Entidades, Interfaces, Enums │ ├── Entities/ # TransacaoEntity, ItemTransacao, Empresa, Produto, etc. │ ├── Enums/ # StatusTransacao, TipoEmpresa │ └── Interfaces/ # ITransacaoRepository, IItemTransacaoRepository, etc. │ ├── Application/ # DTOs, Validators, AutoMapper Profiles │ ├── DTOs/ # TransacaoCreateDto, TransacaoResponseDto │ ├── Validators/ # TransacaoCreateValidator, ItemTransacaoValidator │ └── Profiles/ # TransacaoProfile, ItemTransacaoProfile │ ├── Infrastructure/ # Implementações, Banco de Dados │ ├── Data/ # AppDbContext, AppDbContextFactory │ ├── Repositories/ # TransacaoRepository, ItemTransacaoRepository │ └── Security/ # PasswordHasher, TokenService, TenantProvider │ └── API/ # Controllers, Endpoints └── Controllers/ # TransacaoController, ItemTransacaoController 🔌 Endpoints Principais Autenticação POST /api/Auth/login # Login e obter token JWT POST /api/Auth/register # Registrar novo usuário Empresas GET /api/Empresa # Listar minhas empresas POST /api/Empresa # Criar empresa GET /api/Empresa/{id} # Buscar empresa por ID Produtos GET /api/Produto # Listar produtos POST /api/Produto # Criar produto GET /api/Produto/{id} # Buscar produto PUT /api/Produto/{id} # Atualizar produto Estoque GET /api/LocalEstoque # Listar locais POST /api/MovimentacaoEstoque # Registrar movimentação GET /api/AjusteEstoque # Consultar ajustes Transações B2B POST /api/Transacao # Criar transação GET /api/Transacao # Listar todas GET /api/Transacao/{id} # Buscar por ID GET /api/Transacao/{id}/itens # Buscar com itens GET /api/Transacao/como-vendedor # Minhas vendas GET /api/Transacao/como-comprador # Minhas compras PUT /api/Transacao/{id}/status # Atualizar status Itens de Transação GET /api/ItemTransacao/{id} # Buscar item GET /api/ItemTransacao/transacao/{transacaoId} # Itens da transação GET /api/ItemTransacao/produto/{produtoId} # Itens com produto 🚀 Como Rodar Localmente Pré-requisitos

.NET 8.0 SDK MySQL Server (versão 8.0+) Visual Studio ou VS Code

Passos de Instalação

Clone o repositório

bashgit clone https://github.com/CaioFerreira007/SistemaFinanceiroERP.API.git cd SistemaFinanceiroERP.API

Configure a connection string Edite appsettings.json:

json{ "ConnectionStrings": { "DefaultConnection": "Server=localhost;Port=3306;Database=SistemaFinanceiroERP;User=root;Password=sua_senha;" } }

Instale as dependências

bashdotnet restore

Execute as migrations

bashdotnet ef database update

Execute a aplicação

bashdotnet run

Acesse a documentação Abra no navegador: https://localhost:7206/swagger

🧪 Testando os Endpoints

Fazer Login bashcurl -X POST "https://localhost:7206/api/Auth/login"
-H "Content-Type: application/json"
-d '{ "email": "seu_email@example.com", "senha": "sua_senha" }'
Copiar o Token JWT O response retornará um token. Copie-o.
Usar o Token Clique no botão "Authorize" no Swagger e cole: Bearer seu_token_aqui
Testar Endpoints Use o Swagger UI para testar todos os endpoints com documentação interativa. 🏗️ Padrões Implementados Repository Pattern Abstração de acesso a dados com interfaces genéricas e específicas. Dependency Injection Todas as dependências injetadas via construtor para fácil teste e manutenção. AutoMapper Mapeamento automático entre Entidades e DTOs. FluentValidation Validações fluentes e reutilizáveis. Multi-Tenancy Isolamento de dados por empresa usando Query Filters do EF Core. Clean Architecture Separação clara de responsabilidades em 4 camadas. 🔐 Segurança
Autenticação JWT: Tokens seguros com expiração configurável Hash de Senhas: Usando PBKDF2 Multi-Tenancy: Cada empresa vê apenas seus próprios dados Query Filters: Aplicados automaticamente em todas as consultas [Authorize]: Proteção de endpoints que requerem autenticação

📈 Progresso do Projeto FaseDescriçãoStatus1Fundação (Auth, Multi-tenant)✅ Completo2Estoque (Movimentações, Ajustes)✅ Completo3.1Transações B2B✅ Completo3.2Validações Avançadas⏳ Futuro4Módulo Financeiro⏳ Futuro5Relatórios e Analytics⏳ Futuro 💡 Decisões Arquiteturais Por que Clean Architecture? Facilita testes, manutenção e escalabilidade. Cada camada tem responsabilidade única. Por que Multi-Tenancy? Simula um ambiente real de SaaS onde múltiplas empresas usam o mesmo sistema com dados isolados. Por que ItemTransacao é agregado? Seguindo DDD (Domain-Driven Design), itens só existem no contexto de uma transação. Por que NumeroTransacao é gerado no banco? Garante unicidade global e impossibilita duplicação mesmo com requisições simultâneas. 📚 Tecnologias Aprendidas

ASP.NET Core com padrão MVC Entity Framework Core com lazy loading e includes JWT Authentication e Authorization Query Filters para multi-tenancy AutoMapper para DTOs FluentValidation para validações complexas Padrões SOLID e Clean Architecture Best practices de API REST

🎓 Objetivo Educacional Este projeto foi desenvolvido como exercício de aprendizado de:

Arquitetura de software em camadas Padrões de design profissionais Desenvolvimento de APIs em produção Segurança em aplicações web Conceitos avançados de C# e .NET

📞 Contato Desenvolvedor: Caio Gustavo Bernardo Ferreira Email: caiogggustavo49@gmail.com LinkedIn: linkedin.com/in/caioferreira007 GitHub: github.com/CaioFerreira007 📝 Licença Este projeto está sob licença MIT. Veja o arquivo LICENSE para mais detalhes.

Desenvolvido com ❤️ durante jornada de aprendizado em desenvolvimento backend profissional.