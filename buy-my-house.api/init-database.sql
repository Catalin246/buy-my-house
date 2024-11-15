IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'buymyhousedb')
BEGIN
    CREATE DATABASE buymyhousedb;
    -- Additional initialization logic for the database can go here
END
GO

