# LoginSystemTask API
 
## Overview
LoginSystemTask consists of several components, including the API, Application,and Infrastructure Layers. This README provides instructions on how to set up and run the project.

## Prerequisites
Before you begin, ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download) (version compatible with the project)
- [Visual Studio](https://visualstudio.microsoft.com/) or any preferred IDE
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (for database access)

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/AbdallahElshenawy/LoginSystemTask-API.git
   
   cd LoginSystemTask-API

2. Restore dependencies for each project:
    ```bash
    LoginSystemTask.API    
    dotnet restore
    
    cd ../LoginSystemTask.Infrastructure
    
    dotnet restore
    
    cd ../LoginSystemTask.Application
    
    dotnet restore
    

3. Update the database: After restoring dependencies, update the database to match the application's requirements:
   ```bash
    cd ../LoginSystemTask.API    

    dotnet ef database update

4. Run the project:
   ```bash
    To run the application:
    
    cd ../LoginSystemTask.API    
    
    dotnet run

## API Endpoints

### AuthController

post /api/auth/login
# Logs in an existing user and returns a token.
# Example Credentials To Test The Endpoints:
# Admin: "username": "admin", "password": "admin123"

post /api/auth/users/{userId}/assign-role
# Assigns a role to a specified user.

put /api/auth/users/{userId}/role
# Updates the role of a specified user.

### EmployeesController
post /api/employees
# Creates a new employee.

put /api/employees/{id}
# Updates an existing employee.

get /api/employees/{employeeCode}
# Retrieves an employee by its employeeCode.

get /api/employees
# Retrieves a filtered list of employees.

delete /api/employees/{id}
# Deletes an employee by ID.

### UsersController

post /api/users
# Creates a new user.

put /api/users/{id}
# Updates an existing user.

delete /api/users/{id}
# Deletes a user by ID.

get /api/users/{id}
# Retrieves a user by ID.

get /api/users
# Retrieves all users.

get /api/users/username/{username}
# Retrieves a user by username.

### RolesController

post /api/roles
# Creates a new role.

post /api/roles/{roleId}/permissions/{permissionId}
# Assigns a permission to a role.

get /api/roles
# Retrieves all roles with their permissions.

delete /api/roles/{roleId}/permissions/{permissionId}
# Removes a permission from a role.

get /api/roles/{roleId}/permissions
# Retrieves permissions for a specific role.

### PermissionsController

post /api/permissions
# Creates a new permission.

get /api/permissions/{id}
# Retrieves a permission by its ID.

get /api/permissions
# Retrieves all permissions.

put /api/permissions/{id}
# Updates an existing permission.

delete /api/permissions/{id}
# Deletes a permission by ID.


### AuditLogsController

get /api/auditlogs
# Retrieves a filtered list of audit logs.

