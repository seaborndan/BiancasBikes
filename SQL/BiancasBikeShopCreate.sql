USE [master]

IF db_id('BiancasBikeShop') IS NULL
  CREATE DATABASE [BiancasBikeShop]
GO

USE [BiancasBikeShop]
GO


DROP TABLE IF EXISTS [WorkOrder];
DROP TABLE IF EXISTS [Bike];
DROP TABLE IF EXISTS [BikeType];
DROP TABLE IF EXISTS [Owner];
GO


CREATE TABLE [Bike] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Brand] nvarchar(255) NOT NULL,
  [Color] nvarchar(255) NOT NULL,
  [OwnerId] int NOT NULL,
  [BikeTypeId] int NOT NULL
)
GO

CREATE TABLE [WorkOrder] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [DateInitiated] datetime NOT NULL,
  [Description] nvarchar(1000) NOT NULL,
  [DateCompleted] datetime,
  [BikeId] int NOT NULL
)
GO

CREATE TABLE [BikeType] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(255) NOT NULL
)
GO

CREATE TABLE [Owner] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(255) NOT NULL,
  [Address] nvarchar(255) NOT NULL,
  [Email] nvarchar(255) NOT NULL,
  [Telephone] nvarchar(255) NOT NULL
)
GO

ALTER TABLE [WorkOrder] ADD FOREIGN KEY ([BikeId]) REFERENCES [Bike] ([Id])
GO

ALTER TABLE [Bike] ADD FOREIGN KEY ([OwnerId]) REFERENCES [Owner] ([Id])
GO

ALTER TABLE [Bike] ADD FOREIGN KEY ([BikeTypeId]) REFERENCES [BikeType] ([Id])
GO
