make run:
	@echo "Running CPM"
	@echo "-----------------"
	@echo "run docker-compose -f docker-compose.production.yml up --build"
	docker compose -f docker-compose.production.yml up --build
