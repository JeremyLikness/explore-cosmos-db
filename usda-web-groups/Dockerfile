FROM busybox:latest

# This will create a tiny app to run the Cosmos DB explore 
# Warning: you must update the API link in cosmos.js for it to work! 

RUN mkdir /www 
COPY index.html /www 
COPY serverless.js /www 
EXPOSE 80
CMD ["httpd", "-f", "-p", "80", "-h", "/www"]