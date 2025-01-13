@echo off
setlocal

REM Step 1: Start only the database in detached mode
echo Starting the database...
docker compose up -d database

REM Step 2: Wait for the database to be ready
echo Waiting for the database to be ready...
:wait_for_db
FOR /F "tokens=*" %%i IN ('docker ps -q -f "name=database"') DO SET CONTAINER_ID=%%i
docker exec %CONTAINER_ID% pg_isready -U postgres >nul 2>&1
IF ERRORLEVEL 1 (
    echo Waiting for database connection...
    timeout /t 2 >nul
    GOTO wait_for_db
)

REM Step 3: Dropping the database
echo Dropping the database...
dotnet ef database drop -f --project ./CubeTimer.WebApi/CubeTimer.WebApi --startup-project ./CubeTimer.WebApi/CubeTimer.WebApi --context CubeTimer.WebApi.Infrastructure.Database.ApplicationDbContext

REM Step 4: Removing existing migrations
echo Removing existing migrations...
dotnet ef migrations remove --project ./CubeTimer.WebApi/CubeTimer.WebApi --startup-project ./CubeTimer.WebApi/CubeTimer.WebApi --context CubeTimer.WebApi.Infrastructure.Database.ApplicationDbContext

REM Step 5: Apply migrations
echo Applying migrations...
dotnet ef migrations add Initial --project ./CubeTimer.WebApi/CubeTimer.WebApi --startup-project ./CubeTimer.WebApi/CubeTimer.WebApi --context CubeTimer.WebApi.Infrastructure.Database.ApplicationDbContext

REM Step 6: Update the database
echo Updating the database...
dotnet ef database update --project ./CubeTimer.WebApi/CubeTimer.WebApi --startup-project ./CubeTimer.WebApi/CubeTimer.WebApi --context CubeTimer.WebApi.Infrastructure.Database.ApplicationDbContext

REM Step 7: Shut down the database
echo Stopping the database...
docker compose down

endlocal