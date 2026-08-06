# medor-practical — Warehouse Management

Simple web app for managing per-client product warehouse stock: a single page with a
filterable, paginated grid where quantities are editable inline.

## Prerequisites

- .NET 8 SDK
- Node.js 22+ / npm 11+

## Running the backend

```bash
dotnet run --project backend/Warehouse.Api
```

- API: http://localhost:5080/api
- Swagger UI: http://localhost:5080/swagger

The SQLite database (`backend/Warehouse.Api/warehouse.db`) is dropped and reseeded with test
data every time the backend starts.

## Running the frontend

In a separate terminal (backend must already be running):

```bash
cd frontend
npm install
npm run dev
```

Open http://localhost:5173. The dev server proxies `/api` requests to the backend.

## Regenerating the API client

Whenever the backend API contract changes, regenerate the TypeScript client (backend must be
running):

```bash
cd frontend
npm run generate:api
```

This overwrites `frontend/src/api/generated/api-client.ts`, which is checked into source
control so a fresh clone doesn't need to run this before `npm run dev`.
