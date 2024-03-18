# buzz-house-shop

An advanced distributed application of an online clothing store which offers custom made clothing. The customization is given by the customer who uploads the design or our predefined designs. The design will be placed as a logo on the desired clothing item. 

Some technical details:
Server: .NET Web API
Client: React.Js mobile responsive web interface
Database: Cosmos NoSql database
Deployment infrastructure: Dockerized app deployed in Azure Container Apps
Scaling: Using the autoscaler from the Azure Container Apps
Multi-region: Deploying the app within two regions and routing based on user location (end goal)
Background processing: Application will use a queue and a couple of workers to do tasks that can be processed in background (e.g. time consuming tasks)
CI/CD: Github actions

Team:
Adelin Chis - backend
Andreea Miculescu - backend 
Andrei Balgradean - frontend
Denis Ardelean - frontend
