markdown# Sistema ERP Multi-Tenant

Sistema ERP completo desenvolvido com ASP.NET Core 8 e Clean Architecture, focado em gestão empresarial com isolamento total entre empresas (multi-tenancy).

## Sobre o Projeto

ERP (Enterprise Resource Planning) profissional com arquitetura limpa, autenticação JWT, multi-tenancy robusto e padrões de desenvolvimento modernos. O sistema permite gestão completa de vendas, compras, estoque e finanças com total isolamento de dados entre empresas.

## Status do Projeto

**Em Desenvolvimento** - Fase 1 concluída (96%)

**Tempo investido:** ~36 horas  
**Progresso:** 14.6% do projeto total (247h estimadas)

SEÇÃO 2: Tecnologias
markdown## Stack Tecnológica

**Backend:**
- ASP.NET Core 8.0
- C# 12
- Entity Framework Core 8.0
- MySQL

**Arquitetura e Padrões:**
- Clean Architecture (4 camadas)
- Repository Pattern
- Dependency Injection
- CQRS (planejado)

**Segurança:**
- JWT (JSON Web Tokens)
- BCrypt (hash de senhas)
- Multi-tenancy com Query Filters

**Validação e Mapeamento:**
- FluentValidation
- AutoMapper

**Documentação:**
- Swagger / OpenAPI

SEÇÃO 3: Arquitetura
markdown## Arquitetura do Projeto

O projeto segue Clean Architecture com 4 camadas bem definidas:
```
SistemaFinanceiroERP/
├── Domain/              # Entidades e regras de negócio
│   └── Entities/        # Classes de domínio (Produto, Usuario, Empresa)
│
├── Application/         # Lógica de aplicação e interfaces
│   └── Interfaces/      # Contratos (IRepository, ITenantProvider)
│
├── Infrastructure/      # Implementações de infraestrutura
│   ├── Data/           # DbContext, Migrations
│   ├── Repositories/   # Implementação dos repositories
│   └── Security/       # JWT, Hashing, TenantProvider
│
└── API/                # Camada de apresentação
    ├── Controllers/    # Endpoints REST
    ├── DTOs/          # Data Transfer Objects
    └── Validators/    # Validadores FluentValidation
```

**Princípios aplicados:**
- Separação de responsabilidades
- Inversão de dependências
- Código limpo e testável

SEÇÃO 4: Funcionalidades
markdown## Funcionalidades Implementadas

### Autenticação e Autorização
- [x] Registro de empresas com primeiro usuário admin
- [x] Login com JWT
- [x] Hash de senhas com BCrypt
- [x] Proteção de rotas com [Authorize]

### Multi-Tenancy
- [x] Isolamento automático de dados por empresa
- [x] Query Filters do Entity Framework
- [x] TenantProvider para extrair empresa do token
- [x] Impossível acessar dados de outras empresas

### Gestão de Produtos
- [x] CRUD completo de produtos
- [x] Validação com FluentValidation
- [x] Soft Delete (campo Ativo)
- [x] Auditoria (DataCriacao, DataAtualizacao)

### Gestão de Usuários
- [x] CRUD completo de usuários
- [x] Troca de senha com validação
- [x] Vínculo automático com empresa

### Gestão de Empresas
- [x] Visualização de dados da empresa
- [x] Atualização de dados cadastrais
- [x] Proteção contra acesso a outras empresas

### Repository Pattern
- [x] Repository genérico (IRepository<T>)
- [x] Repositories específicos por entidade
- [x] Centralização de queries
- [x] Código testável

SEÇÃO 5: Como Rodar
markdown## Como Rodar o Projeto

### Pré-requisitos

- .NET SDK 8.0 ou superior
- MySQL 8.0 ou superior
- Visual Studio 2022 ou VS Code

### Configuração

1. Clone o repositório
```bash
git clone [seu-repositorio]
cd SistemaFinanceiroERP
```

2. Configure a connection string no `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=erp_db;User=root;Password=sua_senha;"
  }
}
```

3. Execute as migrations
```bash
dotnet ef database update
```

4. Rode o projeto
```bash
dotnet run
```

5. Acesse o Swagger
```
https://localhost:7123/swagger
```

### Primeiro Uso

1. Registre uma empresa em `POST /api/Auth/register`
2. Faça login em `POST /api/Auth/login`
3. Copie o token retornado
4. Clique em "Authorize" no Swagger
5. Cole o token e explore os endpoints

SEÇÃO 6: Estrutura de Dados
markdown## Entidades Principais

### Empresa
- Dados cadastrais (CNPJ, Razão Social, Nome Fantasia)
- Contato (Email, Telefone)
- Tipo (MEI, Simples Nacional, Lucro Presumido, Lucro Real)

### Usuario
- Dados pessoais (Nome, Email, Telefone)
- Autenticação (Senha hasheada)
- Vínculo com Empresa

### Produto
- Informações básicas (Nome, Categoria, Descrição)
- Controle de estoque (Quantidade, Unidade de Medida)
- Precificação (Preço Unitário)
- Rastreabilidade (Código de Barras)

SEÇÃO 7: Roadmap
markdown## Roadmap

### Fase 1: Base Profissional (concluída)
- [x] FluentValidation
- [x] Hash de Senhas
- [x] JWT + Autenticação
- [x] Multi-tenancy
- [x] Repository Pattern
- [x] Documentação Básica

### Fase 2: Entidades Core (0%)
- [ ] Clientes
- [ ] Fornecedores
- [ ] Relacionamentos

### Fase 3: Gestão de Estoque (0%)
- [ ] Movimentações
- [ ] Inventário
- [ ] Alertas

### Fases 4-10: Vendas, Compras, Financeiro, Relatórios, Testes, Performance, Deploy
