
# API endpoints for IT008 project

---

## Table of Content
- [How to get Json template](#how-to-get-json-template)
- [Authentication](#authentication)
- [Category](#category)
- [Movement](#movement)
- [Permission](#movement)
- [Product](#movement)
- [Supplier](#movement)
- [Users](#users)
- [Warehouse](#warehouse)
---

## How to get json template
> Json templates can be get by send a `GET` request to `api/[endpoint]/json`

---

### Authentication
```
POST   api/Auth/register
POST   api/Auth/login
POST   api/Auth/refresh
POST   api/Auth/logout
```

### Category
```
GET    api/Category
GET    api/Category/json
GET    api/Category/[CategoryId]
POST   api/Category
PUT    api/Category/[CategoryId]
DELETE api/Category/[CategoryId]
```
### Movement
```
GET    api/Movement/json
GET    api/Movement/[MovementId]
POST   api/Movement
PUT    api/Movement/[MovementId]
DELETE api/Movement/[MovementId]
```
### Permission
```
GET    api/Permission/json
GET    api/Permission/[userId]-[WarehouseId]
GET    api/Permission/user/[UserId]
GET    api/Permission/warehouse/[WarehouseId]
POST   api/Permission
PUT    api/Permission/[userId]-[WarehouseId]
DELETE api/Permission/[userId]-[WarehouseId]
```
### Product
```
GET    api/Product/json
GET    api/Product/[ProductId]
GET    api/Product/warehouse/[WarehouseId]
POST   api/Product
PUT    api/Product/[ProductId]
DELETE api/Product/[ProductId]

```
### Supplier
```
GET    POST api/Supplier
GET    api/Supplier/json
GET    api/Supplier/[SupplierId]
PUT    api/Supplier/[SupplierId]
DELETE api/Supplier/[SupplierId]
```
### User
```
GET    api/User/json
GET    api/User/[UserId]
GET    api/User/[UserId]/warehouses
POST   api/User
PUT    api/User/[UserId]
DELETE api/User/[UserId]
```
### Warehouse
```
GET    api/Warehouse/json
GET    api/Warehouse/[WarehouseId]
GET    api/Warehouse/[WarehouseId]/users
POST   api/Warehouse
PUT    api/Warehouse/[WarehouseId]
DELETE api/Warehouse/[WarehouseId]
```