CREATE TABLE IF NOT EXISTS public.categories
(
    category_id
    SERIAL
    PRIMARY
    KEY,
    name
    TEXT
    NOT
    NULL
);

CREATE TABLE IF NOT EXISTS products
(
    product_id
    SERIAL
    PRIMARY
    KEY,
    name
    TEXT
    NOT
    NULL,
    sku
    VARCHAR
(
    50
) UNIQUE,
    category_id INT,
    unit_price NUMERIC
(
    12,
    2
) NOT NULL DEFAULT 1,
    quantity INT NOT NULL DEFAULT 0,
    FOREIGN KEY
(
    category_id
) REFERENCES categories
(
    category_id
),
    CHECK
(
    unit_price >
    0
),
    CHECK
(
    quantity
    >=
    0
)
    );

CREATE TABLE IF NOT EXISTS suppliers
(
    supplier_id
    SERIAL
    PRIMARY
    KEY,
    name
    TEXT
    NOT
    NULL,
    contact_info
    TEXT
);

CREATE TYPE movement_type_enum as ENUM ('in', 'out', 'adjustment', 'transfer', 'remove');

CREATE TABLE IF NOT EXISTS movements
(
    movement_id
    SERIAL
    PRIMARY
    KEY,
    product_id
    INT
    NOT
    NULL,
    warehouse_id
    INT
    NOT
    NULL
    quantity
    INT
    NOT
    NULL,
    movement_type
    movement_type_enum
    NOT
    NULL,
    date
    TIMESTAMP
    WITH
    TIME
    ZONE
    DEFAULT
    CURRENT_TIMESTAMP,
    FOREIGN
    KEY
(
    product_id
) REFERENCES products
(
    product_id
),
    CHECK
(
    quantity >
    0
)
    );

CREATE TABLE IF NOT EXISTS warehouses
(
    warehouse_id
    SERIAL
    PRIMARY
    KEY,
    name
    varchar
(
    40
) NOT NULL
    );

CREATE TABLE IF NOT EXISTS users
(
    user_id
    SERIAL
    PRIMARY
    KEY,
    username
    varchar
(
    40
) NOT NULL,
    email varchar
(
    40
) NOT NULL,
    password_hash TEXT NOT NULL,
    salt TEXT NOT NULL,
    join_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
                            );

CREATE TYPE permission_enum as ENUM ('in', 'out', 'adjustment', 'transfer', 'remove');

CREATE TABLE IF NOT EXISTS user_permissions
(
    user_id
    INT
    NOT
    NULL,
    warehouse_id
    INT
    NOT
    NULL,
    permissions
    permission_enum[],
    PRIMARY
    KEY
(
    user_id,
    warehouse_id
)
    );

CREATE INDEX idx_products_category ON products (category_id);
CREATE INDEX idx_movements_product ON movements (product_id);