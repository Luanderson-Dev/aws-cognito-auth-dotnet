# **AWS Cognito Auth .NET API**

A robust RESTful API developed in .NET 10 for user identity and authentication management, using AWS Cognito as the identity provider (IdP). The project follows Clean Architecture principles and implements the CQRS pattern using MediatR.

## **🚀 Features**

- **User Registration (Sign Up):** Creation of new accounts with a confirmation code sent via email.
- **Account Confirmation (Confirm Sign Up):** Validation of new registrations using the received verification code.
- **Login (Sign In):** Secure authentication returning JWT tokens (Access Token, Id Token, and Refresh Token).
- **Session Renewal (Refresh Token):** Generation of a new Access Token without the need for manual re-authentication.
- **Password Reset (Forgot Password):** Request for password recovery with a temporary code sent to the email.
- **Reset Confirmation (Confirm Forgot Password):** Creation of a new password using the security code.
- **Logout (Sign Out):** Session termination and invalidation of active tokens.
- **Exception Handling:** Standardized error responses mapping native AWS Cognito exceptions to appropriate HTTP Status Codes.

## **🛠 Technologies and Architecture**

- **Framework:** .NET 10.0
- **Identity Provider:** AWS Cognito (AWSSDK.CognitoIdentityProvider)
- **Design Patterns:** Clean Architecture, CQRS (with MediatR)
- **Documentation:** Swagger / OpenAPI
- **Containerization:** Docker and Docker Compose

### **Project Structure**

The project is divided into 4 main layers:

1. AuthService.Api: HTTP Controllers, Dependency Injection, Middlewares, and startup configurations.
2. AuthService.Application: Use cases (Commands/Handlers), containing the orchestration logic.
3. AuthService.Domain: Entities, Value Objects, and repository interfaces.
4. AuthService.Infrastructure: Data access implementations and external integrations (AWS Cognito).

## **⚙️ Prerequisites**

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/) and Docker Compose
- An AWS account with a configured **Cognito User Pool** and an **App Client**.

## **☁️ AWS Cognito Setup Guide**

To run this API, you must configure a User Pool in AWS Cognito. Follow these steps in the AWS Management Console:

## 1. Access the AWS Console

1. Go to: https://console.aws.amazon.com/
2. In the search bar, type **Cognito**
3. Click on **Amazon Cognito**

## 2. Create a User Pool

1. In the left menu, click on **User pools**
2. Click **Create user pool**

## 3. Configure the User Pool

### 🔐 Application type

Select:

- ✅ **Traditional web application**

### 🏷️ Application name

Enter a name, for example: My web app

### Sign-in identifiers

Select only:

- ✅ Email
- ⛔ Phone number
- ⛔ Username

Users will log in using their email only.

### Self-registration

- ✅ Enable self-registration

This allows users to sign up by themselves.

### Required attributes

Select:

- ✅ email

### Finish setup

Click: `Create user pool`

## 4. Configure the App Client

After creating the User Pool:

1. Open your newly created User Pool
2. In the left menu, go to:

`Applications → App clients`

3. Click on the created App Client

## 5. Edit App Client settings

1. Click **Edit** (top right corner)

### 6. Enable password authentication

In the **Authentication flows** section, enable:

- ✅ `ALLOW_USER_PASSWORD_AUTH`

## 7. Save changes

1. Scroll to the bottom of the page
2. Click: `Save changes`

## 8. Retrieving Required Credentials

To integrate the application with AWS Cognito, you will need the following values:

- User Pool ID
- Client ID
- Client Secret
- AWS Region

### 🧩 8.1 Get the User Pool ID

1. Go to **Amazon Cognito**
2. Click on **User pools**
3. Select your User Pool
4. On the **Overview** page, locate: `User pool ID`

### 🧩 8.2 Get the Client ID and Client Secret

1. Inside your User Pool, go to: `Applications → App clients`

2. Click on your App Client (e.g., `My web app - XXXXX`)

3. On the **Overview** page, locate: `Client ID` and `Client secret`

## **🚀 Getting Started**

### **1\. Clone the repository**

```
git clone \<YOUR\_REPOSITORY\_URL\>
cd aws-cognito-auth-dotnet
```

### **2\. Configure Environment Variables**

Create a .env file in the root of the project, using .env.example as a template, and insert the data obtained from the Cognito setup step:

```
\# Cognito
Cognito\_\_UserPoolId=us-east-1\_XXXXXXXXX
Cognito\_\_ClientId=your-client-id
Cognito\_\_ClientSecret=your-client-secret
Cognito\_\_Region=us-east-1
```

### **3\. Running the Application**

#### **Option A: Using Docker Compose (Recommended)**

The project includes a Dockerfile and a compose.yaml. To run everything in isolated containers:

```
docker-compose up --build
```

The API will be available at: http://localhost:8080

#### **Option B: Running Locally (.NET CLI)**

If you prefer to run it directly on your host machine:

```
cd src/AuthService.Api
dotnet run
```

The API will start at the addresses configured in launchSettings.json (usually http://localhost:5077 and https://localhost:7021).

## **📖 API Documentation (Swagger)**

With the application running, access the interactive Swagger UI to test the endpoints:

- **URL:** http://localhost:8080/swagger (if running via Docker) or http://localhost:5077/swagger (if running locally).

### **Available Endpoints**

All endpoints are available under the /api/auth base route:

| Method | Route                    | Description                                                                    |
| :----- | :----------------------- | :----------------------------------------------------------------------------- |
| POST   | /signup                  | Registers a new user and sends an email with the confirmation code.            |
| POST   | /confirm-signup          | Validates the code received via email to activate the account.                 |
| POST   | /signin                  | Authenticates the user and returns the AccessToken, IdToken, and RefreshToken. |
| POST   | /refresh-token           | Generates a new valid AccessToken from a previous RefreshToken.                |
| POST   | /forgot-password         | Initiates the password recovery flow.                                          |
| POST   | /confirm-forgot-password | Completes the recovery by setting a new password with the received code.       |
| POST   | /signout                 | Globally invalidates the session and tokens associated with the user.          |
