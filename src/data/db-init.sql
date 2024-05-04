Print 'Criando banco WeatherDb...'

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'WeatherDb')
    BEGIN
        CREATE DATABASE WeatherDb
    END
GO

USE [WeatherDb]
GO 

Print 'Fim criação banco WeatherDb...'