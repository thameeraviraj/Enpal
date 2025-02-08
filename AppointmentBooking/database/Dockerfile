# Use the official PostgreSQL image from the Docker Hub
FROM postgres:16

# Add the init.sql script to the Docker image
COPY init.sql /docker-entrypoint-initdb.d/

# Set environment variables for PostgreSQL
ENV POSTGRES_DB=coding-challenge
ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=mypassword123!

