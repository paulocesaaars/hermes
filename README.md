# hermes
Sistema de integração de dispositivos para broker MQTT.

# Docker build 
docker build -f Api-Dockerfile -t deviot.hermes.modbustcp.api .

docker build -f WebApp-Dockerfile -t deviot.hermes.modbustcp.app .

# Docker-compose
docker-compose -f docker-compose.yml up --build

# Others
--restart unless-stopped -it -d

# Gerar certificado local
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p Paula@123

dotnet dev-certs https --trust
