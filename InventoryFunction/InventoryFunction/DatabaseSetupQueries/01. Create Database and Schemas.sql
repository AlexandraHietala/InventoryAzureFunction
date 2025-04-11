-----------------------------------------------------------

-- Notes:
-- When the server is setup be sure to enable SQL Authentication, which can be found under
--			(Right-Click Server) > Properties > Security > Server Authentication > SQL Server and Windows Authentication Mode
-- Run this setup under local db admin account

-----------------------------------------------------------

-- Drop Existing, If Needed

/*
USE [Master]
GO

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'app')
DROP SCHEMA [app]
GO

IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'hist')
DROP SCHEMA [hist]
GO

ALTER DATABASE [SEInventory] SET single_user WITH rollback immediate
GO

DROP DATABASE IF EXISTS [SEInventory]
GO
*/

-----------------------------------------------------------

CREATE DATABASE [SEInventory]
GO

ALTER DATABASE [SEInventory] SET MULTI_USER
GO

-----------------------------------------------------------

USE [SEInventory]
GO

IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE name = N'StarryEdenAppAdmin')
	CREATE LOGIN [StarryEdenAppAdmin] WITH PASSWORD=N'Tomorrow@9', DEFAULT_DATABASE=[SEInventory], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [StarryEdenAppAdmin] ENABLE
GO 

IF NOT EXISTS (SELECT * FROM sys.sysusers WHERE name = N'StarryEdenAppAdmin')
	CREATE USER [StarryEdenAppAdmin] FOR LOGIN [StarryEdenAppAdmin];
GO

-----------------------------------------------------------

USE [SEInventory]
GO

CREATE SCHEMA hist AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [SEInventory]
GO

CREATE SCHEMA app AUTHORIZATION [StarryEdenAppAdmin]
GO

-----------------------------------------------------------

USE [SEInventory]
GO

ALTER USER [StarryEdenAppAdmin]
WITH DEFAULT_SCHEMA = app;

GO

-----------------------------------------------------------

USE [SEInventory]

ALTER ROLE [db_owner] ADD MEMBER [StarryEdenAppAdmin];
GO

-----------------------------------------------------------