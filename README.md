# hermes
Sistema de integração de dispositivos para broker MQTT.

# Docker build 
docker build -f Api-Dockerfile -t deviot.hermes.api .

docker build -f App-Dockerfile -t deviot.hermes.app .

docker build -f Server-Dockerfile -t deviot.hermes.server .

# Docker run 
docker run -d -p 5000:80 -p 5001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=5001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="Paula@123" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v %USERPROFILE%\.aspnet\https:/https/ --name deviot-hermes-api deviot.hermes.api:latest

docker run -d -p 8000:8000 --name hermes-app deviot.hermes.app:latest

docker run -d -p 80:80 --name hermes-server deviot.hermes.server:latest

# Docker-compose
docker-compose -f docker-compose.yml up --build

# Others
--restart unless-stopped -it -d

# Gerar certificado autoassinado (executar pelo GIT BASH)
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout localhost.key -out localhost.crt -config localhost.conf -passin pass:Paula@123

# Gerar certificado local
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p Paula@123

dotnet dev-certs https --trust
