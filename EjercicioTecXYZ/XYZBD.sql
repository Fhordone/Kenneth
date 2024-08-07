-- Crear la base de datos XYZBD
CREATE DATABASE XYZBD;

-- Usar la base de datos XYZBD
USE XYZBD;

-- Creación de la tabla Roles
CREATE TABLE Roles(
ID INT AUTO_INCREMENT PRIMARY KEY,
Nombre VARCHAR(50) NOT NULL UNIQUE
);
-- Creación de la tabla Puesto
CREATE TABLE Puesto(
ID INT AUTO_INCREMENT PRIMARY KEY,
Nombre VARCHAR(50) NOT NULL UNIQUE
);
-- Creación de la tabla Productos
CREATE TABLE Productos(
SKU INT AUTO_INCREMENT PRIMARY KEY,
Nombre VARCHAR(50) NOT NULL,
Tipo VARCHAR(50) NOT NULL,
Etiquetas VARCHAR(50) NOT NULL,
Precio DECIMAL(10, 2) NOT NULL,
Unidad_medida VARCHAR(50) NOT NULL
);
-- Crear la tabla Usuario
CREATE TABLE Usuario (
    ID_Trabajador INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Telefono VARCHAR(20),
    Puesto INT,
    Rol INT,
    FOREIGN KEY (Puesto) REFERENCES Puesto(ID),
    FOREIGN KEY (Rol) REFERENCES Roles(ID)
);
-- Creación de la tabla del estado del pedido
CREATE TABLE Estado_Pedido(
ID INT AUTO_INCREMENT PRIMARY KEY,
Nombre VARCHAR(50) NOT NULL UNIQUE
);
-- Creación de la tabla Pedido
CREATE TABLE Pedidos (
    Nro_Pedido INT AUTO_INCREMENT PRIMARY KEY,
    Productos JSON NOT NULL,
    Fecha_pedido DATE NOT NULL,
    Fecha_recepcion DATE,
    Fecha_despacho DATE,
    Fecha_entrega DATE,
    Vendedor INT,
    Repartidor INT,
    Estado INT,
    FOREIGN KEY (Vendedor) REFERENCES Usuario(ID_Trabajador),
    FOREIGN KEY (Repartidor) REFERENCES Usuario(ID_Trabajador),
    FOREIGN KEY (Estado) REFERENCES Estado_Pedido(ID)
);
-- Data de la tabla Roles
INSERT INTO Roles (Nombre) 
VALUES 
('Encargado'),
('Vendedor'),
('Delivery'),
('Repartidor');
-- Data de la tabla Puesto
INSERT INTO Puesto (Nombre) 
VALUES 
('Farmaceutico'),
('Tecnico'),
('Auxiliar'),
('Gerente'),
('Cajero');
-- Data de la tabla Estado del Pedido
INSERT INTO Estado_Pedido (Nombre) 
VALUES 
('Por atender'),
('En proceso'),
('En Delivery'),
('Recibido');
-- Insertar data en la tabla Usuario
INSERT INTO Usuario (Nombre, Email, Password, Telefono, Puesto, Rol) 
VALUES 
('Juan Pérez', 'juan.perez@botica.com','123', '123-456-7890', 5, 2), 
('María López', 'maria.lopez@botica.com','123', '987-654-3210', 2, 4); 
-- Insertar data en la tabla Productos
INSERT INTO Productos (Nombre, Tipo, Etiquetas, Precio, Unidad_medida)
VALUES
('Paracetamol 500mg', 'Medicamento', 'Analgesico, Antipiretico', 5.99, 'mg'),
('Ibuprofeno 400mg', 'Medicamento', 'Antiinflamatorio, Analgesico', 7.49, 'mg');
-- Insertar data en la tabla Pedidos
INSERT INTO Pedidos (Productos, Fecha_pedido, Fecha_recepcion, Fecha_despacho, Fecha_entrega, Vendedor, Repartidor, Estado)
VALUES (
    JSON_ARRAY(
        JSON_OBJECT('SKU', 1, 'Cantidad', 2),
        JSON_OBJECT('SKU', 2, 'Cantidad', 1)
    ),
    '2024-08-06',  -- Fecha del pedido
    NULL,          -- Fecha de recepción (puede ser NULL si aún no se ha recibido)
    NULL,          -- Fecha de despacho (puede ser NULL si aún no se ha despachado)
    NULL,          -- Fecha de entrega (puede ser NULL si aún no se ha entregado)
    3,             -- ID del vendedor (Debería ser una llave foránea, sin embargo puse este valor de ejemplo)
    4,             -- ID del repartidor (Debería ser una llave foránea, sin embargo puse este valor de ejemplo)
    1              -- Estado del pedido
);

-- Procedimiento Almacenado para autenticación del Usuario
DELIMITER //

CREATE PROCEDURE sp_validar_usuario(
    IN p_email VARCHAR(255),
    IN p_password VARCHAR(255)
)
BEGIN
    SELECT Email, Password, Rol
    FROM Usuario
    WHERE Email = p_email
    AND Password = p_password;
END //

DELIMITER ;
-- Procedimiento almacenado para actualizar estados
DELIMITER $$

CREATE PROCEDURE sp_actualizar_estado_pedido(
    IN p_Nro_Pedido INT,
    IN p_Nuevo_Estado INT
)
BEGIN
    DECLARE v_Estado_Actual INT;
    
    -- Obtener el estado actual del pedido
    SELECT Estado INTO v_Estado_Actual
    FROM Pedidos
    WHERE Nro_Pedido = p_Nro_Pedido;

    -- Verificar si el nuevo estado es mayor que el estado actual
    IF p_Nuevo_Estado > v_Estado_Actual THEN
        -- Actualizar el estado del pedido
        UPDATE Pedidos
        SET Estado = p_Nuevo_Estado
        WHERE Nro_Pedido = p_Nro_Pedido;
        
        -- Actualizar la fecha según el nuevo estado
        CASE p_Nuevo_Estado
            WHEN 2 THEN
                UPDATE Pedidos
                SET Fecha_recepcion = CURDATE()
                WHERE Nro_Pedido = p_Nro_Pedido;
            WHEN 3 THEN
                UPDATE Pedidos
                SET Fecha_despacho = CURDATE()
                WHERE Nro_Pedido = p_Nro_Pedido;
            WHEN 4 THEN
                UPDATE Pedidos
                SET Fecha_entrega = CURDATE()
                WHERE Nro_Pedido = p_Nro_Pedido;
        END CASE;
    ELSE
        -- Si el nuevo estado no es mayor, lanzar un error
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'El nuevo estado debe ser mayor que el estado actual.';
    END IF;
END$$

DELIMITER ;
-- Llamados a Procedimientos almacenados
CALL sp_actualizar_estado_pedido(1, 2);
CALL sp_validar_usuario('juan.perez@botica.com', '123');
-- Sentencias para visualizar la información
SELECT * FROM Roles;
SELECT * FROM Puesto;
SELECT * FROM Estado_Pedido;
SELECT * FROM Usuario;
SELECT * FROM Productos;
SELECT * FROM Pedidos;

