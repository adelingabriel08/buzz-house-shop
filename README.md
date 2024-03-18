# Buzz House Shop

An advanced distributed application of an online clothing store which offers custom made clothing. The customization is given by the customer who uploads the design or our predefined designs. The design will be placed as a logo on the desired clothing item. 

<h4>Some technical details:</h4>
Server: .NET Web API <br />
Client: React.Js mobile responsive web interface <br />
Database: Cosmos NoSql database <br />
Deployment infrastructure: Dockerized app deployed in Azure Container Apps <br />
Scaling: Using the autoscaler from the Azure Container Apps <br />
Multi-region: Deploying the app within two regions and routing based on user location (end goal) <br />
Background processing: Application will use a queue and a couple of workers to do tasks that can be processed in background (e.g. time consuming tasks) <br />
CI/CD: Github actions<br /><br />

<h4>Team:</h4>
Adelin Chis - backend <br /> 
Andreea Miculescu - backend <br />
Andrei Balgradean - frontend <br />
Denis Ardelean - frontend <br />
