Quick how-to (local)

  Spin up infra

    docker compose up -d (Elasticsearch)

    or use your local SQL/ES.

  Run API

    dotnet ef database update

    dotnet run

    Open Swagger → POST /api/products/seed, then POST /api/products/reindex.

  Run Angular

    ng serve --proxy-config proxy.conf.json

    Open http://localhost:4200

    Type a query (e.g., “keyboard”); you’ll get ES results.
