# Game Store

## Starting SQL Server docker container
```zsh
sa_password="SA PASSWORD HERE"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -e "MSSQL_PID=Evaluation" -p 1433:1433  --name sqlpreview --hostname sqlpreview -d -v sqlvolume:/var/opt/mssql --name mssql mcr.microsoft.com/mssql/server:2022-latest
```