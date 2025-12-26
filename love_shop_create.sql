CREATE TABLE products (
  id uuid PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
  name varchar(200) NOT NULL,
  description text NULL,
  price decimal(12, 2) NOT NULL CHECK (price >= 0),
  is_deleted boolean NOT NULL DEFAULT false
);

CREATE TABLE carts (
  id uuid PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
  customer_id uuid NOT NULL,
  is_deleted boolean NOT NULL DEFAULT false
);

CREATE TABLE products_in_carts (
  id uuid PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
  cart_id uuid NOT NULL,
  product_id uuid NOT NULL,
  quantity integer NOT NULL DEFAULT 1 CHECK (quantity > 0),
  is_deleted boolean NOT NULL DEFAULT false,
  UNIQUE(cart_id, product_id)
);

CREATE TABLE customers (
  id uuid PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
  name varchar(200) NOT NULL,
  email varchar(255) NOT NULL UNIQUE,
  phone_number varchar(11) NOT NULL UNIQUE CHECK (phone_number ~ '^\d{11}$'),
  is_deleted boolean NOT NULL DEFAULT false
);

CREATE TABLE categories (
  id uuid PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
  name varchar(200) NOT NULL,
  parent_category_id uuid NULL,
  is_deleted boolean NOT NULL DEFAULT false,
  CHECK (id != parent_category_id)
);

CREATE TABLE product_categories (
	id uuid PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
	product_id uuid NOT NULL,
	category_id uuid NOT NULL,
	UNIQUE(product_id, category_id)
);

CREATE TABLE orders (
  id uuid PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
  created_at timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
  completed_at timestamptz NULL,
  customer_id uuid NOT NULL,
  supplier_id uuid NOT NULL,
  is_active boolean NOT NULL DEFAULT true,
  is_deleted boolean NOT NULL DEFAULT false,
  CONSTRAINT valid_dates CHECK (completed_at IS NULL OR created_at <= completed_at)
);

CREATE TABLE products_in_orders (
  id uuid PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
  order_id uuid NOT NULL,
  product_id uuid NOT NULL,
  quantity integer NOT NULL DEFAULT 1 CHECK (quantity > 0),
  unit_price decimal(12, 2) NOT NULL CHECK (unit_price >= 0),
  is_deleted boolean NOT NULL DEFAULT false,
  UNIQUE(order_id, product_id)
);

CREATE TABLE suppliers (
  id uuid PRIMARY KEY NOT NULL DEFAULT gen_random_uuid(),
  name varchar(200) NOT NULL,
  email varchar(255) NOT NULL UNIQUE,
  phone_number varchar(11) NOT NULL UNIQUE CHECK (phone_number ~ '^\d{11}$'),
  is_deleted boolean NOT NULL DEFAULT false
);

ALTER TABLE products ADD FOREIGN KEY (category_id) REFERENCES categories (id);

ALTER TABLE categories ADD FOREIGN KEY (parent_category_id) REFERENCES categories (id);

ALTER TABLE carts ADD FOREIGN KEY (customer_id) REFERENCES customers (id);

ALTER TABLE products_in_carts ADD FOREIGN KEY (cart_id) REFERENCES carts (id);

ALTER TABLE products_in_carts ADD FOREIGN KEY (product_id) REFERENCES products (id);

ALTER TABLE orders ADD FOREIGN KEY (customer_id) REFERENCES customers (id);

ALTER TABLE products_in_orders ADD FOREIGN KEY (order_id) REFERENCES orders (id);

ALTER TABLE products_in_orders ADD FOREIGN KEY (product_id) REFERENCES products (id);

ALTER TABLE orders ADD FOREIGN KEY (supplier_id) REFERENCES suppliers (id);

---- 1. Для продуктов (поиск по имени, категории, фильтрация по удаленным)
--CREATE INDEX idx_products_name ON products(name varchar_pattern_ops);
--CREATE INDEX idx_products_category ON products(category_id);
--CREATE INDEX idx_products_active ON products(is_deleted) WHERE is_deleted = false;
--CREATE INDEX idx_products_price ON products(price) WHERE is_deleted = false;

---- 2. Для корзин (поиск корзин пользователя)
--CREATE INDEX idx_carts_customer ON carts(customer_id);
--CREATE INDEX idx_carts_active ON carts(is_deleted) WHERE is_deleted = false;

---- 3. Для продуктов в корзине (самые частые операции!)
--CREATE INDEX idx_pic_cart ON products_in_carts(cart_id);
--CREATE INDEX idx_pic_product ON products_in_carts(product_id);
---- Составной индекс для быстрого поиска товара в конкретной корзине
--CREATE INDEX idx_pic_cart_product ON products_in_carts(cart_id, product_id);

---- 4. Для клиентов (поиск по имени)
--CREATE INDEX idx_customers_name ON customers(name varchar_pattern_ops);
--CREATE INDEX idx_customers_active ON customers(is_deleted) WHERE is_deleted = false;
--CREATE INDEX idx_customers_email ON customers(email) WHERE is_deleted = false;
--CREATE INDEX idx_customers_phone ON customers(phone_number) WHERE is_deleted = false;

---- 5. Для категорий (иерархические запросы + поиск по имени)
--CREATE INDEX idx_categories_name ON categories(name varchar_pattern_ops);
--CREATE INDEX idx_categories_parent ON categories(parent_category_id) WHERE parent_category_id IS NOT NULL;
--CREATE INDEX idx_categories_active ON categories(is_deleted) WHERE is_deleted = false;

---- 6. Для заказов (много критериев поиска)
--CREATE INDEX idx_orders_customer ON orders(customer_id);
--CREATE INDEX idx_orders_supplier ON orders(supplier_id);
--CREATE INDEX idx_orders_created ON orders(created_at DESC);
--CREATE INDEX idx_orders_completed ON orders(completed_at) WHERE completed_at IS NOT NULL;
--CREATE INDEX idx_orders_active ON orders(is_active) WHERE is_active = true;

---- 7. Для продуктов в заказах
--CREATE INDEX idx_pio_order ON products_in_orders(order_id);
--CREATE INDEX idx_pio_product ON products_in_orders(product_id);
---- Составной индекс для анализа: какие товары в каком заказе
--CREATE INDEX idx_pio_order_product ON products_in_orders(order_id, product_id);

---- 8. Для поставщиков
--CREATE INDEX idx_suppliers_name ON suppliers(name varchar_pattern_ops);
--CREATE INDEX idx_suppliers_active ON suppliers(is_deleted) WHERE is_deleted = false;
--CREATE INDEX idx_suppliers_email ON suppliers(email) WHERE is_deleted = false;
--CREATE INDEX idx_suppliers_phone ON suppliers(phone_number) WHERE is_deleted = false;