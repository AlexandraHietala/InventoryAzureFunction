-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [SEInventoryDB]
GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [dbo].[vwRoles] AS
SELECT *
FROM [dbo].[Roles]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [dbo].[vwUsers] AS
	
SELECT *
FROM [dbo].[Users]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [dbo].[vwSeries] AS
	
SELECT *
FROM [dbo].[Series]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [dbo].[vwBrands] AS
	
SELECT *
FROM [dbo].[Brands]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [dbo].[vwCollections] AS
	
SELECT *
FROM [dbo].[Collections]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [dbo].[vwItems] AS
	
SELECT *
FROM [dbo].[Items]

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [dbo].[vwItems_Expanded]
AS

	SELECT
		item.*, 
		
		-- Series in format of   1 SeriesA 2 SeriesB 3 SeriesC
		SUBSTRING (
		(
			SELECT ' ' + CAST(ser.ID as varchar) + ' ' + ser.SERIES_NAME AS [text()]
			FROM [dbo].[vwSeries] ser
			WHERE item.SERIES_ID = ser.ID
			ORDER BY ser.ID
			FOR XML PATH ('')
		), 1, 10000) [SERIES],

		-- Brand in format of   1 BrandA 2 BrandB 3 BrandC
		SUBSTRING (
		(
			SELECT ' ' + CAST(bra.ID as varchar) + ' ' + bra.BRAND_NAME AS [text()]
			FROM [dbo].[vwBrands] bra
			WHERE item.BRAND_ID = bra.ID
			ORDER BY bra.ID
			FOR XML PATH ('')
		), 1, 10000) [BRAND],

		-- Collection in format of   1 CollectionA 2 CollectionB 3 CollectionC
		SUBSTRING (
		(
			SELECT ' ' + CAST(col.ID as varchar) + ' ' + col.COLLECTION_NAME AS [text()]
			FROM [dbo].[vwCollections] col
			WHERE item.COLLECTION_ID = col.ID
			ORDER BY col.ID
			FOR XML PATH ('')
		), 1, 10000) [COLLECTION]

	FROM [dbo].[vwItems] item;

GO

-----------------------------------------------------------

CREATE OR ALTER VIEW [dbo].[vwItems_Search]
AS

SELECT
	    item.*,
		(CONCAT([ID],' ',[STATUS],' ',[TYPE],' ',[SERIES],' ',[BRAND],' ',[NAME], ' ', [DESCRIPTION], ' ',[FORMAT],' ',[SIZE],' ',[YEAR])) as [FULLDATA]
FROM [dbo].[vwItems_Expanded] item

GO

-----------------------------------------------------------
