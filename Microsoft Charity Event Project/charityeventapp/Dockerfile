# FROM node:14 as build-deps
# WORKDIR /usr/src/app
# RUN npm install react-scripts@3.4.1 -g --silent
# COPY . ./
# RUN yarn install
# RUN yarn build


# FROM nginx
# COPY --from=build-deps /usr/src/app/build /usr/share/nginx/html
# EXPOSE 80
# CMD ["nginx", "-g", "daemon off;"]

FROM node:14
WORKDIR /app
ENV PATH /app/node_modules/.bin:$PATH
COPY package.json ./
COPY package-lock.json ./
RUN npm install --silent
RUN npm install react-scripts@3.4.1 -g --silent
COPY . ./
CMD ["npm", "start"]
