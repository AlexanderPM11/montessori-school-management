# Etapa 1: Construcción
FROM node:20-alpine AS build

# Definimos el argumento de construcción para la URL de la API
# Esto permite pasar la variable así: docker build --build-arg VITE_API_URL=http://tu-api.com .
ARG VITE_API_URL
ENV VITE_API_URL=${VITE_API_URL}

WORKDIR /app

# Copiamos archivos de dependencias primero para optimizar el caché de Docker
COPY package*.json ./
RUN npm install

# Copiamos el resto del código y generamos el build de producción
COPY . .
RUN npm run build

# Etapa 2: Servidor de Producción (Nginx)
FROM nginx:stable-alpine

# Copiamos los archivos estáticos generados por Vite (carpeta dist)
COPY --from=build /app/dist /usr/share/nginx/html

# Copiamos la configuración personalizada de Nginx (SPA + Proxy)
COPY nginx.conf /etc/nginx/conf.d/default.conf


EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
