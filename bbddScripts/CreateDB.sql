CREATE DATABASE IF NOT EXISTS BiblioDB;
USE BiblioDB;

CREATE TABLE Autor (
    Id INT PRIMARY KEY NOT NULL,
    Nombre VARCHAR(100),
    Apellido VARCHAR(100),
    Nacionalidad VARCHAR(50),
    FechaNacimiento DATE,
    EstaVivo TINYINT(1),
    Biografia TEXT
);

CREATE TABLE Editorial (
    Id INT PRIMARY KEY NOT NULL,
    Nombre VARCHAR(100),
    Direccion VARCHAR(200),
    Telefono VARCHAR(20),
    Email VARCHAR(100),
    FechaFundacion DATE,
    SitioWeb VARCHAR(100)
);

CREATE TABLE Libro (
    ISBN VARCHAR(20) PRIMARY KEY NOT NULL,
    Titulo VARCHAR(200),
    Genero VARCHAR(50),
    NumeroPaginas INT,
    Precio DECIMAL(10,2),
    Disponible TINYINT(1),
    FechaPublicacion DATE
);

CREATE TABLE Usuario (
    Id INT PRIMARY KEY NOT NULL,
    Nombre VARCHAR(100),
    Apellido VARCHAR(100),
    Email VARCHAR(100),
    FechaRegistro DATE,
    EstaActivo TINYINT(1)
);

CREATE TABLE Prestamo (
    Id INT PRIMARY KEY NOT NULL,
    LibroISBN VARCHAR(20) NOT NULL,
    UsuarioId INT NOT NULL,
    FechaPrestamo DATE,
    FechaDevolucionPrevista DATE,
    FechaDevolucionReal DATE,
    EstadoPrestamo VARCHAR(50),
    Multa DECIMAL(10,2),
    FOREIGN KEY (LibroISBN) REFERENCES Libro(ISBN),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

-- Insertar Editoriales
INSERT INTO Editorial (Id, Nombre, Direccion, Telefono, Email, FechaFundacion, SitioWeb) VALUES
(1, 'Editorial Alfaguara', 'Calle Falsa 123, Madrid, España', '+34 91 123 45 67', 'contacto@alfaguara.com', '1964-05-15', 'www.alfaguara.com'),
(2, 'Penguin Random House', 'Avenida Real 456, Barcelona, España', '+34 93 987 65 43', 'info@penguinrandomhouse.com', '2013-07-01', 'www.penguinrandomhouse.com');

-- Insertar Autores
INSERT INTO Autor (Id, Nombre, Apellido, Nacionalidad, FechaNacimiento, EstaVivo, Biografia) VALUES
(1, 'Gabriel', 'García Márquez', 'Colombiana', '1927-03-06', 0, 'Escritor y periodista colombiano, premio Nobel de Literatura en 1982.'),
(2, 'Isabel', 'Allende', 'Chilena', '1942-08-02', 1, 'Escritora chilena, conocida por novelas como "La casa de los espíritus".');

-- Insertar Libros (sin relación con editorial ni autor, por lo que no tenemos esos campos)
INSERT INTO Libro (ISBN, Titulo, Genero, NumeroPaginas, Precio, Disponible, FechaPublicacion) VALUES
('978-84-204-8312-5', 'Cien años de soledad', 'Realismo mágico', 471, 19.95, 1, '1967-05-30'),
('978-84-01-45001-2', 'La casa de los espíritus', 'Novela', 499, 18.50, 1, '1982-01-01');

-- Insertar Usuarios
INSERT INTO Usuario (Id, Nombre, Apellido, Email, FechaRegistro, EstaActivo) VALUES
(1, 'Juan', 'Pérez', 'juan.perez@email.com', '2023-01-15', 1),
(2, 'María', 'Gómez', 'maria.gomez@email.com', '2023-02-20', 1);

-- Insertar Préstamos
INSERT INTO Prestamo (Id, LibroISBN, UsuarioId, FechaPrestamo, FechaDevolucionPrevista, FechaDevolucionReal, EstadoPrestamo, Multa) VALUES
(1, '978-84-204-8312-5', 1, '2023-03-01', '2023-03-15', NULL, 'Activo', NULL),
(2, '978-84-01-45001-2', 2, '2023-03-02', '2023-03-16', '2023-03-14', 'Devuelto', 0.00);