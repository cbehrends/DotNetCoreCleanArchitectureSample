# build image
FROM node:latest as builder

# set working directory
WORKDIR /app

# install and cache app dependencies
COPY . .
RUN npm install
RUN npm run build --prod

# deployment image
FROM nginx:alpine

#Remove default Nginx page
RUN rm -rf /usr/share/nginx/html/*

# Copy the contents of the dist folder to
COPY --from=builder /app/dist/AngularUI /usr/share/nginx/html

EXPOSE 80

ENTRYPOINT ["nginx","-g","daemon off;"]

