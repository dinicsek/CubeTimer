echo "Starting the database"
docker compose up -d database

echo "Waiting for the database to be ready..."
until docker exec -it $(docker ps -q -f "name=database") pg_isready -U postgres > /dev/null 2>&1; do
    echo "Waiting for database connection..."
    sleep 2
done

echo "Database is ready!"

echo "Starting the migration"
dotnet ef migrations add Initial --project ./CubeTimer.WebApi --startup-project ./CubeTimer.WebApi --context CubeTimer.WebApi.Infrastructure.Database.ApplicationDbContext

echo "Droping the database"
dotnet ef database drop -f --project ./CubeTimer.WebApi --startup-project ./CubeTimer.WebApi --context CubeTimer.WebApi.Infrastructure.Database.ApplicationDbContext

echo "Creating the database"
dotnet ef database update --project ./CubeTimer.WebApi --startup-project ./CubeTimer.WebApi --context CubeTimer.WebApi.Infrastructure.Database.ApplicationDbContext

echo "Migration completed!"

echo "Stopping the database"
docker compose down

echo "Starting the application"
docker compose up --build