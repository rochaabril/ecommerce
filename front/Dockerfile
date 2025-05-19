# Build Angular
FROM node:18 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

# Servir con Nginx
FROM nginx:alpine
COPY --from=build /app/dist/front /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
