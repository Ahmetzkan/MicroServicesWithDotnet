docker run -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=Password12* -p 8080:8080 quay.io/keycloak/keycloak:24.0.1 start-dev


//member account UI
http://localhost:8080/realms/MyCompany/account



http://localhost:8080/realms/MyCompany/.well-known/openid-configuration