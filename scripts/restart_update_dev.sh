docker compose down &&
docker image rm love_shop_api &&
docker compose --env-file ./.env.dev -f docker-compose.dev.yaml up