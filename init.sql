
IF DB_ID('MyApiDb') IS NULL
BEGIN
    CREATE DATABASE MyApiDb;
END;
GO

USE MyApiDb;
GO

IF OBJECT_ID('dbo.Items', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Items
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL
    );

    INSERT INTO dbo.Items (Name)
    VALUES
        ('Keyboard'),
        ('Mouse'),
        ('Monitor');
END;
GO
