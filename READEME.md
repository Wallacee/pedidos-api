# 📦 Pedidos API — Enterprise Case (.NET 8)

API RESTful para gerenciamento de pedidos, desenvolvida como **case técnico**, com foco em **arquitetura limpa, regras de negócio explícitas, testabilidade e boas práticas** adotadas em ambientes corporativos.

---

## 🚀 Como Executar e Avaliar o Projeto (Quick Start)


📂 Onde executar os comandos

Todos os comandos devem ser executados na raiz da solution (onde está o arquivo .sln).

### ▶️ Executar a API
```bash
dotnet restore
dotnet ef database update
dotnet run --project Pedidos.Api


Acesse o Swagger:
https://localhost:{porta}/swagger


🧪 Executar Testes
dotnet test

📊 Gerar Relatório de Cobertura
dotnet test Pedidos.Tests --collect:"XPlat Code Coverage"
reportgenerator -reports:"Pedidos.Tests/TestResults/**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

Abrir relatório:
explorer coverage-report

Abrir -> coverage-report/index.html (Relatório de cobertura)

🧭 Arquitetura e Organização

A solução segue princípios inspirados em Clean Architecture e DDD tático, com separação clara entre responsabilidades:

Domain → regras de negócio e invariantes

Application → casos de uso, orquestração e contratos

Infrastructure → persistência e detalhes técnicos

API → exposição HTTP e configuração

Princípios aplicados

SRP / SOLID

Inversão de Dependência

Entidades ricas (regras no domínio)

Fail fast para regras de negócio

Código orientado a casos de uso

⚙️ Stack Tecnológica

.NET 8 / ASP.NET Core Web API

Entity Framework Core 8

SQLite (banco relacional para desenvolvimento)

AutoMapper

FluentValidation

xUnit + Moq + FluentAssertions

Swagger / OpenAPI

ILogger (logs estruturados)

🧩 Modelo de Domínio
Pedido

Id (Guid)

ClienteNome

DataCriacao

Status (Novo, Pago, Cancelado)

ValorTotal

Itens

ItemPedido

ProdutoNome

Quantidade

PrecoUnitario

Regras de Negócio

Pedido deve conter ao menos um item

Quantidade e preço devem ser maiores que zero

Pedido pago não pode ser cancelado

Pedido cancelado não pode ser pago

Valor total calculado automaticamente

As regras estão concentradas no Domínio, evitando lógica espalhada em controllers ou services.

🔌 Endpoints
Método	Endpoint	            Descrição
POST	/pedidos	            Criar pedido
GET	    /pedidos	            Listar pedidos (paginação + filtro)
GET	    /pedidos/{id}	        Buscar pedido por ID
PUT	    /pedidos/{id}/pagar	    Marcar pedido como pago
PUT	    /pedidos/{id}/cancelar	Cancelar pedido


🛡️ Validações

FluentValidation aplicado nos DTOs de entrada

Validações de contrato retornam HTTP 400

Regras de negócio lançam exceções de domínio específicas, com mensagens claras

Exemplo:

{
  "errors": {
    "Itens[0].Quantidade": [
      "'Quantidade' deve ser superior a '0'."
    ]
  }
}

❗ Tratamento de Erros

Exceções de domínio possuem ErrorCode e mensagem explícita

Middleware global converte exceções em respostas HTTP adequadas

Stack trace preservado apenas para logs, não exposto ao consumidor

🧪 Testes Automatizados

Estratégia

Domain Tests: valida regras críticas de negócio

Application Tests: valida fluxos e serviços

Cobertura focada em comportamento, não apenas linhas

🌿 Sobre a Branch main

Este repositório utiliza apenas a branch main por se tratar de um case técnico individual, entregue como artefato final de avaliação.

Em cenários de time, a estratégia usual incluiria:

dev

feature branches

pull requests

pipelines de CI/CD

Para este contexto, a escolha da main prioriza:

Clareza para avaliação

Código final e estável

Facilidade de leitura pelo avaliador

👤 Autor

Wallace Veridiano de Jesus
Backend Developer — .NET / Arquitetura / APIs

Projeto desenvolvido como case técnico, simulando padrões, decisões e práticas adotadas em ambientes corporativos reais.

