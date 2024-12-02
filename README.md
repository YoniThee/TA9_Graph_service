## Overview
This service provides a backend API for managing graph data. It leverages Entity Framework Core to interact with a SQL Server database.

### Prerequisites
* .NET SDK: Ensure you have the .NET SDK installed.
* SQL Server: A SQL Server instance with the specified database and credentials.
* Docker (Optional): For containerization and deployment.

## Getting Started
#### 1. Clone the Repository:
```
git clone https://github.com/YoniThee/TA9_Graph_service.git
```

#### 2. Configure the Database:
This app is configured to use microsoft sqlServer so its will work the best for you if you will create the same environmant.
  1. Open microsoftsqlServer and create new DB
  2. Connect your DB to your project
  3. Update the appsettings.json file with your SQL Server connection string.  
  4. run migration commans to create the correct tables
    ```
      Install-Package Microsoft.EntityFrameworkCore
      Add-Migration InitialCreate
      Update-Database
    ```  
#### 3. Build and Run the Application:

**Locally:**

Open the solution in Visual Studio.
Build and run the project.

**Using Docker:**
Build the Docker image:
```
docker build -t your-image-name .
docker run -it --rm -p 44339:8080 your-image-name
```
you can replace the ports or the name of the image

### API Endpoints
the app is expose CRUD endpoint to Edges and to Nodes
![image](https://github.com/user-attachments/assets/0d2a7af7-3ca8-4214-83db-8403aded7e1b)
The logic of the creating and removing new instances(nodes/edges) is by graph(undirected graph) theory. each Node can be connected or not connected to other node/nodes(nighbors) but 
each edge must to be connectd to 2 nodes.
The Update function reffer to node is uodate the data that the node contain and refer to edge is uodating the nodes that this edge is connect between

Each request (Post/Delete/Update) is effect the relevant nodes and edges

### Use this App
Postman collection is attached to this repo for better understanding of the logic and the functionality of the service
