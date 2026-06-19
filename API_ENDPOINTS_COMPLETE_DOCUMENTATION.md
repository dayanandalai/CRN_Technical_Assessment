# API Endpoint Structure Documentation

## Overview
This document provides a complete reference of all API endpoints following RESTful conventions.

---

## Endpoint Structure Pattern

### Standard Resource Operations
```
GET    /api/resource              → Get all resources (paginated where applicable)
GET    /api/resource/{id}         → Get specific resource by ID
POST   /api/resource              → Create new resource
PUT    /api/resource/{id}         → Update specific resource
DELETE /api/resource/{id}         → Delete specific resource
```

### Related Resources
```
GET    /api/resource/{id}/related → Get related sub-resources
```

---

## 1. AUTHENTICATION ENDPOINTS
**Base URL:** `/api/auth`

### 1.1 Register User
```
POST /api/auth/register
```
**Description:** Register a new user account
**Authorization:** None (Anonymous)
**Request Body:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "password": "string"
}
```
**Response (201 Created):**
```json
{
  "success": true,
  "message": "User registered successfully",
  "data": {
    "accessToken": "string",
    "refreshToken": "string",
    "expiresAt": "2024-01-01T00:00:00Z"
  },
  "errors": null,
  "statusCode": 201
}
```

### 1.2 Login User
```
POST /api/auth/login
```
**Description:** Authenticate user and retrieve JWT tokens
**Authorization:** None (Anonymous)
**Request Body:**
```json
{
  "email": "string",
  "password": "string"
}
```
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "accessToken": "string",
    "refreshToken": "string",
    "expiresAt": "2024-01-01T00:00:00Z"
  },
  "errors": null,
  "statusCode": 200
}
```

### 1.3 Refresh Token
```
POST /api/auth/refresh-token
```
**Description:** Refresh expired JWT token
**Authorization:** Bearer Token
**Request Body:**
```json
{
  "refreshToken": "string"
}
```
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Token refreshed successfully",
  "data": {
    "accessToken": "string",
    "refreshToken": "string",
    "expiresAt": "2024-01-01T00:00:00Z"
  },
  "errors": null,
  "statusCode": 200
}
```

---

## 2. USERS ENDPOINTS
**Base URL:** `/api/users`
**Authorization:** Bearer Token (except POST for creation)

### 2.1 Get All Users
```
GET /api/users
```
**Description:** Retrieve all users
**Authorization:** Required
**Query Parameters:** None
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Users retrieved successfully",
  "data": [
    {
      "id": 1,
      "firstName": "string",
      "lastName": "string",
      "email": "string",
      "status": "Active"
    }
  ],
  "errors": null,
  "statusCode": 200
}
```

### 2.2 Get User by ID
```
GET /api/users/{id}
```
**Description:** Retrieve specific user details
**Authorization:** Required
**Path Parameters:**
- `id` (integer, required): User ID
**Response (200 OK):**
```json
{
  "success": true,
  "message": "User retrieved successfully",
  "data": {
    "id": 1,
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "status": "Active"
  },
  "errors": null,
  "statusCode": 200
}
```

### 2.3 Create User
```
POST /api/users
```
**Description:** Create new user
**Authorization:** Not required
**Request Body:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "password": "string"
}
```
**Response (201 Created):**
```json
{
  "success": true,
  "message": "User created successfully",
  "data": 1,
  "errors": null,
  "statusCode": 201
}
```

### 2.4 Update User
```
PUT /api/users/{id}
```
**Description:** Update user profile information
**Authorization:** Required
**Path Parameters:**
- `id` (integer, required): User ID
**Request Body:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "password": "string"
}
```
**Response (200 OK):**
```json
{
  "success": true,
  "message": "User updated successfully",
  "data": null,
  "errors": null,
  "statusCode": 200
}
```

### 2.5 Delete User
```
DELETE /api/users/{id}
```
**Description:** Delete user account
**Authorization:** Required
**Path Parameters:**
- `id` (integer, required): User ID
**Response (200 OK):**
```json
{
  "success": true,
  "message": "User deleted successfully",
  "data": null,
  "errors": null,
  "statusCode": 200
}
```

---

## 3. PRODUCTS ENDPOINTS
**Base URL:** `/api/products`
**Authorization:** Bearer Token Required

### 3.1 Get All Products
```
GET /api/products
```
**Description:** Retrieve all products with pagination
**Authorization:** Required
**Query Parameters:**
- `pageNumber` (integer, optional, default: 1): Page number
- `pageSize` (integer, optional, default: 10): Items per page
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Products retrieved successfully",
  "data": {
    "data": [
      {
        "id": 1,
        "name": "string",
        "description": "string",
        "price": 0.00,
        "categoryId": 1,
        "category": "string"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalRecords": 100,
    "totalPages": 10
  },
  "errors": null,
  "statusCode": 200
}
```

### 3.2 Get Product by ID
```
GET /api/products/{id}
```
**Description:** Retrieve specific product details
**Authorization:** Required
**Path Parameters:**
- `id` (integer, required): Product ID
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Product retrieved successfully",
  "data": {
    "id": 1,
    "name": "string",
    "description": "string",
    "price": 0.00,
    "categoryId": 1,
    "category": "string"
  },
  "errors": null,
  "statusCode": 200
}
```

### 3.3 Create Product
```
POST /api/products
```
**Description:** Create new product
**Authorization:** Required
**Request Body:**
```json
{
  "name": "string",
  "description": "string",
  "price": 0.00,
  "categoryId": 1
}
```
**Response (201 Created):**
```json
{
  "success": true,
  "message": "Product created successfully",
  "data": 1,
  "errors": null,
  "statusCode": 201
}
```

### 3.4 Update Product
```
PUT /api/products/{id}
```
**Description:** Update product information
**Authorization:** Required
**Path Parameters:**
- `id` (integer, required): Product ID
**Request Body:**
```json
{
  "name": "string",
  "description": "string",
  "price": 0.00,
  "categoryId": 1
}
```
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Product updated successfully",
  "data": null,
  "errors": null,
  "statusCode": 200
}
```

### 3.5 Delete Product
```
DELETE /api/products/{id}
```
**Description:** Delete product
**Authorization:** Required
**Path Parameters:**
- `id` (integer, required): Product ID
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Product deleted successfully",
  "data": null,
  "errors": null,
  "statusCode": 200
}
```

---

## 4. CATEGORIES ENDPOINTS
**Base URL:** `/api/categories`
**Authorization:** Bearer Token Required

### 4.1 Get All Categories
```
GET /api/categories
```
**Description:** Retrieve all categories
**Authorization:** Required
**Query Parameters:** None
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Categories retrieved successfully",
  "data": [
    {
      "id": 1,
      "name": "string"
    }
  ],
  "errors": null,
  "statusCode": 200
}
```

### 4.2 Get Category by ID
```
GET /api/categories/{id}
```
**Description:** Retrieve specific category details
**Authorization:** Required
**Path Parameters:**
- `id` (integer, required): Category ID
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Category retrieved successfully",
  "data": {
    "id": 1,
    "name": "string"
  },
  "errors": null,
  "statusCode": 200
}
```

### 4.3 Create Category
```
POST /api/categories
```
**Description:** Create new category
**Authorization:** Required
**Request Body:**
```json
{
  "name": "string"
}
```
**Response (201 Created):**
```json
{
  "success": true,
  "message": "Category created successfully",
  "data": 1,
  "errors": null,
  "statusCode": 201
}
```

### 4.4 Update Category
```
PUT /api/categories/{id}
```
**Description:** Update category information
**Authorization:** Required
**Path Parameters:**
- `id` (integer, required): Category ID
**Request Body:**
```json
{
  "name": "string"
}
```
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Category updated successfully",
  "data": null,
  "errors": null,
  "statusCode": 200
}
```

### 4.5 Delete Category
```
DELETE /api/categories/{id}
```
**Description:** Delete category
**Authorization:** Required
**Path Parameters:**
- `id` (integer, required): Category ID
**Response (200 OK):**
```json
{
  "success": true,
  "message": "Category deleted successfully",
  "data": null,
  "errors": null,
  "statusCode": 200
}
```

---

## Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "message": "Invalid request data",
  "data": null,
  "errors": [
    "Field validation error"
  ],
  "statusCode": 400
}
```

### 401 Unauthorized
```json
{
  "success": false,
  "message": "Authentication failed or token expired",
  "data": null,
  "errors": null,
  "statusCode": 401
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "Resource not found",
  "data": null,
  "errors": null,
  "statusCode": 404
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "An error occurred while processing your request",
  "data": null,
  "errors": [
    "Exception message"
  ],
  "statusCode": 500
}
```

---

## HTTP Status Codes Summary

| Code | Meaning | Use Case |
|------|---------|----------|
| 200 | OK | Successful GET, PUT, DELETE |
| 201 | Created | Successful POST creating resource |
| 400 | Bad Request | Invalid input/validation failure |
| 401 | Unauthorized | Missing/invalid authentication token |
| 404 | Not Found | Resource doesn't exist |
| 500 | Server Error | Unhandled exception |

---

## Authentication
All endpoints (except `/auth/register` and `/auth/login`) require Bearer token authentication:

```
Authorization: Bearer {accessToken}
```

---

## Response Structure

All API responses follow this consistent structure:

```json
{
  "success": boolean,
  "message": "string",
  "data": object|array|null,
  "errors": ["string"]|null,
  "statusCode": integer
}
```

### Fields:
- **success** (boolean): Indicates if the request was successful
- **message** (string): Human-readable message about the result
- **data** (object|array|null): Response payload (resource data)
- **errors** (array|null): List of error messages if applicable
- **statusCode** (integer): HTTP status code

