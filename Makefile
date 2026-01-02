include ./.env.dev

dev:
	docker compose --env-file .env.dev -f docker-compose.dev.yaml down
	docker compose --env-file .env.dev -f docker-compose.dev.yaml up -d

	bash -c 'set -a && source ./.env.dev && ASPNETCORE_ENVIRONMENT=$$ASPNETCORE_ENVIRONMENT && ConnectionStrings__Database="Host=localhost;Port=$$POSTGRES_PORT;Database=$$POSTGRES_DB;Username=$$POSTGRES_USER;Password=$$POSTGRES_PASSWORD" && echo $(ConnectionStrings__Database) && cd LoveShop && pwd && dotnet watch run -c Debug'
prod:
	docker compose --env-file .env.prod -f docker-compose.yaml up -d