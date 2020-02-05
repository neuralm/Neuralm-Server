# base image
FROM node:10.13-alpine as build

ARG VUE_APP_MESSAGEQUEUE_HOST
ARG VUE_APP_MESSAGEQUEUE_PORT

ENV VUE_APP_MESSAGEQUEUE_HOST=$VUE_APP_MESSAGEQUEUE_HOST
ENV VUE_APP_MESSAGEQUEUE_PORT=$VUE_APP_MESSAGEQUEUE_PORT

# set working directory
WORKDIR /usr/src/app

# copy files
COPY ./src/Neuralm.Presentation.Web/package*.json ./
COPY ./src/Neuralm.Presentation.Web/tsconfig.json ./
COPY ./src/Neuralm.Presentation.Web/ ./

# install and cache app dependencies
RUN npm install --silent
RUN npm run build

# production environment
FROM nginx:1.16.0-alpine
COPY ./src/Neuralm.Presentation.Web/nginx/nginx.conf /temp/prod.conf
RUN envsubst /app < /temp/prod.conf > /etc/nginx/conf.d/default.conf
COPY --from=build /usr/src/app/dist /usr/share/nginx/html

EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]