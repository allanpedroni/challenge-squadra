# Stage 1
FROM node:alpine as build-step

WORKDIR /app
EXPOSE 4200

COPY ./src/Weather.Web /app/.

RUN npm install -g @angular/cli
RUN npm install

COPY . /app

RUN npm run build --prod

# Stage 2
FROM nginx:1.25.5-alpine
COPY --from=build-step /app/dist/angular-project-weather-board/browser /usr/share/nginx/html

