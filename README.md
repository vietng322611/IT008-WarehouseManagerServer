# API endpoints for IT008 project

## Table of Content

- [How to get Json template](#how-to-get-json-template)
- [Authentication](#authentication)
- [Category](#category)
- [Movement](#movement)
- [Permission](#movement)
- [Product](#movement)
- [Supplier](#movement)
- [User](#user)
- [Warehouse](#warehouse)

---

## How to get json template

> Json templates are available at `api/[endpoint]/json`

---

## How to get JSON template
JSON templates can be accessed by sending a `GET` request to `api/[endpoint]/json`  
For Authentication endpoints: `api/Auth/[endpoint]/json`

---

### Authentication

```
POST   api/auth/register
POST   api/auth/login
POST   api/auth/refresh
POST   api/auth/logout
```

### Category

```
GET    api/warehouses/[WarehouseId]/categories
GET    api/warehouses/[WarehouseId]/categories/[CategoryId]
POST   api/warehouses/[WarehouseId]/categories
PUT    api/warehouses/[WarehouseId]/categories/[CategoryId]
DELETE api/warehouses/[WarehouseId]/categories/[CategoryId]
```

### Movement

```
GET    api/warehouses/[WarehouseId]/movements
GET    api/warehouses/[WarehouseId]/movements/[MovementId]
POST   api/warehouses/[WarehouseId]/movements
PUT    api/warehouses/[WarehouseId]/movements/[MovementId]
DELETE api/warehouses/[WarehouseId]/movements/[MovementId]
```

### Permission

```
GET    api/Permission/[userId]-[WarehouseId]
GET    api/Permission/user/[UserId]
GET    api/Permission/warehouse/[WarehouseId]
POST   api/Permission
PUT    api/Permission/[userId]-[WarehouseId]
DELETE api/Permission/[userId]-[WarehouseId]
```

### Product

```
GET    api/warehouses/[WarehouseId]products/
GET    api/warehouses/[WarehouseId]products/[ProductId]
POST   api/warehouses/[WarehouseId]products
PUT    api/warehouses/[WarehouseId]products/[ProductId]
DELETE api/warehouses/[WarehouseId]products/[ProductId]

```

### Supplier

```
GET    api/warehouses/[WarehouseId]/suppliers
GET    api/warehouses/[WarehouseId]/suppliers/[SupplierId]
POST   api/warehouses/[WarehouseId]/suppliers
PUT    api/warehouses/[WarehouseId]/suppliers/[SupplierId]
DELETE api/warehouses/[WarehouseId]/suppliers/[SupplierId]
```

### User

```
GET    api/user/[UserId]
GET    api/user/[UserId]/warehouses
POST   api/user
PUT    api/user/[UserId]
DELETE api/user/[UserId]
```

### Warehouse

```
GET    api/warehouse/[WarehouseId]
GET    api/warehouse/[WarehouseId]/users
POST   api/warehouse
PUT    api/warehouse/[WarehouseId]
DELETE api/warehouse/[WarehouseId]
```
