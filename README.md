# PortfoliosController

Este documento descreve o funcionamento do `PortfoliosController`, responsável por expor a API REST de gerenciamento de portfólios dentro da aplicação **PortifolioEngine**.

## Visão geral

A classe está anotada com `[ApiController]` e `[Route("api/[controller]")]`, portanto todas as rotas começam com `/api/portfolios`. Ela depende de uma implementação de `IPortfolioService`, que encapsula as regras de negócio e a persistência.

Cada ação retorna objetos `PortfolioResponseDto` ou respostas HTTP apropriadas (201, 200, 204, 400 ou 404). As exceções de negócio (`InvalidOperationException`) são convertidas em respostas `400 Bad Request` com um corpo JSON informando o erro.

## Endpoints

### POST `/api/portfolios`
Cria um novo portfólio. Recebe um corpo JSON com os dados de criação (`PortfolioCreateDto`) e retorna `201 Created`, incluindo o objeto criado e o cabeçalho `Location` apontando para `GET /api/portfolios/user/{userId}`.

**JSON de requisição (exemplo):**
```json
{
  "userId": "c8d6dc6f-0a71-4c64-91ea-d0f778436bc1",
  "name": "Portfólio Conservador",
  "description": "Carteira focada em renda fixa"
}
```

**JSON de resposta (201 Created):**
```json
{
  "id": "1c37f9b1-1f7f-45f6-8d18-dc199991f1a4",
  "userId": "c8d6dc6f-0a71-4c64-91ea-d0f778436bc1",
  "name": "Portfólio Conservador",
  "description": "Carteira focada em renda fixa",
  "createdAt": "2024-03-01T12:34:56Z",
  "updatedAt": "2024-03-01T12:34:56Z"
}
```

Em caso de violação das regras de negócio, o serviço lança `InvalidOperationException` e a API devolve:
```json
{
  "error": "Mensagem explicando a inconsistência nos dados"
}
```

### GET `/api/portfolios/user/{userId}`
Obtém todos os portfólios de um usuário específico. O `userId` é um GUID informado na URL.

- Retorna `200 OK` com uma lista JSON de `PortfolioResponseDto` quando existem portfólios.
- Retorna `404 Not Found` quando o usuário não possui nenhum portfólio cadastrado.

**JSON de resposta (200 OK):**
```json
[
  {
    "id": "1c37f9b1-1f7f-45f6-8d18-dc199991f1a4",
    "userId": "c8d6dc6f-0a71-4c64-91ea-d0f778436bc1",
    "name": "Portfólio Conservador",
    "description": "Carteira focada em renda fixa",
    "createdAt": "2024-03-01T12:34:56Z",
    "updatedAt": "2024-03-01T12:34:56Z"
  }
]
```

### PUT `/api/portfolios/{id}`
Atualiza um portfólio existente identificado por `id` (GUID). Recebe um `PortfolioUpdateDto` no corpo e retorna `200 OK` com os dados atualizados. Se o portfólio não for encontrado, retorna `404 Not Found`. Regras de negócio inválidas geram `400 Bad Request` com o mesmo formato de erro exibido acima.

### DELETE `/api/portfolios/{id}`
Remove um portfólio identificado por `id`. Retorna `204 No Content` quando a exclusão é bem-sucedida ou `404 Not Found` quando o recurso não existe.

## Tratamento de erros

- **400 Bad Request**: disparado quando o serviço lança `InvalidOperationException`. O corpo da resposta segue o padrão `{ "error": "mensagem" }`.
- **404 Not Found**: retornado quando o recurso não existe ou, no caso do `GET user/{userId}`, quando o usuário não possui portfólios.
- **204 No Content**: indica que a exclusão foi realizada com sucesso e não há corpo na resposta.

## Objetos de transferência (DTOs)

Embora os DTOs sejam definidos em outro projeto (`InvestorTrust.Contracts.Portfolios`), o controlador depende deles para serializar/deserializar JSON:

- `PortfolioCreateDto`: usado no corpo do `POST`. Contém as informações necessárias para criar um portfólio.
- `PortfolioResponseDto`: representa o portfólio retornado em respostas. Inclui identificadores, metadados e timestamps.
- `PortfolioUpdateDto`: usado para atualizar dados via `PUT`.

A serialização e desserialização JSON dos DTOs é feita automaticamente pelo ASP.NET Core utilizando o `System.Text.Json`, garantindo que os nomes das propriedades em C# sejam convertidos para camelCase no JSON.
