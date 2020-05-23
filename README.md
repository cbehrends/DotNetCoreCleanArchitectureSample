# Clean Architecture Style



# Docker Support

To build the container

`docker build -t <my_container_name> .`

To run the container in your local Docker instance

`docker run -p 8080:80 <my_container_name>`

This will setup the container to listen on port 8080, you may adjust as needed.

Once the container is running, navigate to `http://localhost:8080`
