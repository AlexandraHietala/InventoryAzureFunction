-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [SEInventory]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spAddUser]
	@name varchar(250),
	@salt varchar(250),
	@hash varchar(250),
	@role int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	INSERT INTO [app].[Users] ([NAME], [PASS_SALT], [PASS_HASH], [ROLE_ID], [LAST_MODIFIED_BY], [CREATED_BY])
	VALUES (@name, @salt, @hash, @role, @lastmodifiedby, @lastmodifiedby);

    SELECT SCOPE_IDENTITY();

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetAuth]
	@id varchar(100)
AS

BEGIN TRY

	SELECT [PASS_SALT], [PASS_HASH], [ROLE_ID]
	FROM [app].[vwUsers] 
	WHERE [ID] = @id 

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetUsers]
AS

BEGIN TRY

	SELECT u.[ID], u.[NAME], u.[PASS_SALT], u.[PASS_HASH], u.[ROLE_ID], u.[CREATED_BY], u.[CREATED_DATE], u.[LAST_MODIFIED_BY], u.[LAST_MODIFIED_DATE]
	FROM [app].[vwUsers] u

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetUser]
	@id int
AS

BEGIN TRY

	SELECT TOP 1 u.[ID], u.[NAME], u.[PASS_SALT], u.[PASS_HASH], u.[ROLE_ID], u.[CREATED_BY], u.[CREATED_DATE], u.[LAST_MODIFIED_BY], u.[LAST_MODIFIED_DATE]
	FROM [app].[vwUsers] u
	WHERE u.ID = @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spUpdateUser]
	@id int,
	@name varchar(250),
	@salt varchar(250),
	@hash varchar(250),
	@role int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Users] 
	SET [NAME] = @name, [PASS_SALT] = @salt, [PASS_HASH] = @hash, [ROLE_ID] = @role, [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE() 
	WHERE [ID] = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH



GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spRemoveUser]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Users]
	SET [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE()
	WHERE [ID] = @id

	DELETE FROM [app].[Users]
	WHERE [ID] = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetRole]
	@id int
AS

BEGIN TRY

	SELECT TOP 1 [ID] as [ROLE_ID], [DESCRIPTION] as [ROLE_DESCRIPTION]
	FROM [app].[vwRoles]
	WHERE ID = @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetRoles]
AS

BEGIN TRY

	SELECT [ID] as [ROLE_ID], [DESCRIPTION] as [ROLE_DESCRIPTION]
	FROM [app].[vwRoles]

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetSeries]
	@search varchar(250)
AS

BEGIN TRY
	
	IF (@search IS NOT NULL AND LEN(LTRIM(RTRIM(@search))) > 0) SELECT [ID] as [SERIES_ID], [SERIES_NAME], [DESCRIPTION] as [SERIES_DESCRIPTION], [CREATED_BY] as [SERIES_CREATED_BY], [CREATED_DATE] as [SERIES_CREATED_DATE], [LAST_MODIFIED_BY] as [SERIES_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [SERIES_LAST_MODIFIED_DATE] FROM [app].[vwSeries] WHERE ID LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR SERIES_NAME LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR DESCRIPTION LIKE '%' + LTRIM(RTRIM(@search)) + '%'
	ELSE SELECT [ID] as [SERIES_ID], [SERIES_NAME], [DESCRIPTION] as [SERIES_DESCRIPTION], [CREATED_BY] as [SERIES_CREATED_BY], [CREATED_DATE] as [SERIES_CREATED_DATE], [LAST_MODIFIED_BY] as [SERIES_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [SERIES_LAST_MODIFIED_DATE] FROM [app].[vwSeries]

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetASeries]
	@id int
AS

BEGIN TRY
	
	SELECT [ID] as [SERIES_ID], [SERIES_NAME], [DESCRIPTION] as [SERIES_DESCRIPTION], [CREATED_BY] as [SERIES_CREATED_BY], [CREATED_DATE] as [SERIES_CREATED_DATE], [LAST_MODIFIED_BY] as [SERIES_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [SERIES_LAST_MODIFIED_DATE] 
	FROM [app].[vwSeries] WHERE ID = @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spAddSeries]
	@series_name varchar(250),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	INSERT INTO [app].[Series] ([SERIES_NAME],[DESCRIPTION],[LAST_MODIFIED_BY],[CREATED_BY]) 
	VALUES (@series_name,@description,@lastmodifiedby,@lastmodifiedby);

	SELECT SCOPE_IDENTITY();

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spUpdateSeries]
	@id int,
	@series_name varchar(250),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	UPDATE [app].[Series] 
	SET [SERIES_NAME] = @series_name, [DESCRIPTION] = @description, [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE()
	WHERE ID = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spRemoveSeries]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Series] 
	SET LAST_MODIFIED_BY = @lastmodifiedby, LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id

	DELETE FROM [app].[Series] 
	WHERE ID = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetBrands]
	@search varchar(250)
AS

BEGIN TRY
	
	IF (@search IS NOT NULL AND LEN(LTRIM(RTRIM(@search))) > 0) SELECT [ID] as [BRAND_ID], [BRAND_NAME], [DESCRIPTION] as [BRAND_DESCRIPTION], [CREATED_BY] as [BRAND_CREATED_BY], [CREATED_DATE] as [BRAND_CREATED_DATE], [LAST_MODIFIED_BY] as [BRAND_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [BRAND_LAST_MODIFIED_DATE] FROM [app].[vwBrands] WHERE ID LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR BRAND_NAME LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR DESCRIPTION LIKE '%' + LTRIM(RTRIM(@search)) + '%'
	ELSE SELECT [ID] as [BRAND_ID], [BRAND_NAME], [DESCRIPTION] as [BRAND_DESCRIPTION], [CREATED_BY] as [BRAND_CREATED_BY], [CREATED_DATE] as [BRAND_CREATED_DATE], [LAST_MODIFIED_BY] as [BRAND_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [BRAND_LAST_MODIFIED_DATE] FROM [app].[vwBrands]

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetBrand]
	@id int
AS

BEGIN TRY
	
	SELECT [ID] as [BRAND_ID], [BRAND_NAME], [DESCRIPTION] as [BRAND_DESCRIPTION], [CREATED_BY] as [BRAND_CREATED_BY], [CREATED_DATE] as [BRAND_CREATED_DATE], [LAST_MODIFIED_BY] as [BRAND_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [BRAND_LAST_MODIFIED_DATE] 
	FROM [app].[vwBrands]
	WHERE ID = @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spAddBrand]
	@brand_name varchar(50),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	INSERT INTO [app].[Brands] ([BRAND_NAME],[DESCRIPTION],[LAST_MODIFIED_BY],[CREATED_BY]) 
	VALUES (@brand_name,@description,@lastmodifiedby,@lastmodifiedby);

	SELECT SCOPE_IDENTITY();

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spUpdateBrand]
	@id int,
	@brand_name varchar(50),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	UPDATE [app].[Brands] 
	SET [BRAND_NAME] = @brand_name, [DESCRIPTION] = @description, [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE()
	WHERE ID = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spRemoveBrand]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Brands] 
	SET LAST_MODIFIED_BY = @lastmodifiedby, LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id

	DELETE FROM [app].[Brands] 
	WHERE ID = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetCollections]
	@search varchar(250)
AS

BEGIN TRY
	
	IF (@search IS NOT NULL AND LEN(LTRIM(RTRIM(@search))) > 0) SELECT [ID] as [COLLECTION_ID], [COLLECTION_NAME], [DESCRIPTION] as [COLLECTION_DESCRIPTION], [CREATED_BY] as [COLLECTION_CREATED_BY], [CREATED_DATE] as [COLLECTION_CREATED_DATE], [LAST_MODIFIED_BY] as [COLLECTION_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [COLLECTION_LAST_MODIFIED_DATE] FROM [app].[vwCollections] WHERE ID LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR COLLECTION_NAME LIKE '%' + LTRIM(RTRIM(@search)) + '%' OR DESCRIPTION LIKE '%' + LTRIM(RTRIM(@search)) + '%'
	ELSE SELECT [ID] as [COLLECTION_ID], [COLLECTION_NAME], [DESCRIPTION] as [COLLECTION_DESCRIPTION], [CREATED_BY] as [COLLECTION_CREATED_BY], [CREATED_DATE] as [COLLECTION_CREATED_DATE], [LAST_MODIFIED_BY] as [COLLECTION_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [COLLECTION_LAST_MODIFIED_DATE] FROM [app].[vwCollections]

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetCollection]
	@id int
AS

BEGIN TRY
	
	SELECT [ID] as [COLLECTION_ID], [COLLECTION_NAME], [DESCRIPTION] as [COLLECTION_DESCRIPTION], [CREATED_BY] as [COLLECTION_CREATED_BY], [CREATED_DATE] as [COLLECTION_CREATED_DATE], [LAST_MODIFIED_BY] as [COLLECTION_LAST_MODIFIED_BY], [LAST_MODIFIED_DATE] as [COLLECTION_LAST_MODIFIED_DATE] 
	FROM [app].[vwCollections]
	WHERE ID = @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spAddCollection]
	@collection_name varchar(50),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	INSERT INTO [app].[Collections] ([COLLECTION_NAME],[DESCRIPTION],[LAST_MODIFIED_BY],[CREATED_BY]) 
	VALUES (@collection_name,@description,@lastmodifiedby,@lastmodifiedby);

	SELECT SCOPE_IDENTITY();

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spUpdateCollection]
	@id int,
	@collection_name varchar(50),
	@description varchar(250),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	
	UPDATE [app].[Collections] 
	SET [COLLECTION_NAME] = @collection_name, [DESCRIPTION] = @description, [LAST_MODIFIED_BY] = @lastmodifiedby, [LAST_MODIFIED_DATE] = GETDATE()
	WHERE ID = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spRemoveCollection]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Collections] 
	SET LAST_MODIFIED_BY = @lastmodifiedby, LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id

	DELETE FROM [app].[Collections] 
	WHERE ID = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spAddItemComment]
	@item_id int,
	@comment varchar(max),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	INSERT INTO [app].[ItemComments] (ITEM_ID, COMMENT, CREATED_BY, LAST_MODIFIED_BY)
	VALUES (@item_id, @comment, @lastmodifiedby, @lastmodifiedby);

	SELECT SCOPE_IDENTITY();

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spRemoveItemComment]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[ItemComments]
	SET LAST_MODIFIED_BY = @lastmodifiedby, LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id

	DELETE FROM [app].[ItemComments]
	WHERE ID = @id

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetItemComments]
	@item_id int
AS

BEGIN TRY

	SELECT ID as [COMMENT_ID], ITEM_ID, COMMENT, CREATED_BY as [COMMENT_CREATED_BY], CREATED_DATE as [COMMENT_CREATED_DATE]
	FROM [app].[vwItemComments]
	WHERE ITEM_ID = @item_id
	ORDER BY ID DESC

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spUpdateItemComment]
	@id int,
	@item_id int,
	@comment varchar(max),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[ItemComments]
	SET	
		COMMENT = @comment,
		LAST_MODIFIED_BY = @lastmodifiedby,
		LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id AND ITEM_ID = @item_id;

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetItemComment]
	@id int
AS

BEGIN TRY

	SELECT ID as [COMMENT_ID], ITEM_ID, COMMENT, CREATED_BY as [COMMENT_CREATED_BY], CREATED_DATE as [COMMENT_CREATED_DATE]
	FROM [app].[vwItemComments]
	WHERE ID = @id
	ORDER BY ID DESC

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spAddItem]
	@collection_id int,
	@status varchar(10),
	@type varchar(15),
	@brand_id int,
	@series_id int,
	@name varchar(100),
	@description varchar(250),
	@format varchar(15), 
	@size varchar(10),
	@year int,
	@photo varchar(max),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	INSERT INTO [app].[Items] ([COLLECTION_ID],[STATUS],[TYPE],[BRAND_ID],[SERIES_ID],[NAME],[DESCRIPTION],[FORMAT],[SIZE],[YEAR],[PHOTO],[CREATED_BY],[LAST_MODIFIED_BY])
	VALUES (@collection_id,@status,@type,@brand_id,@series_id,@name,@description,@format,@size,@year,@photo,@lastmodifiedby,@lastmodifiedby)

	SELECT SCOPE_IDENTITY();

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spRemoveItem]
	@id int,
	@lastmodifiedby varchar(100)
AS

BEGIN TRY
	DECLARE @countComments int;

	SET @countComments = (SELECT COUNT(*) FROM [app].[ItemComments] WHERE ITEM_ID = @id);

	IF (@countComments > 0) UPDATE [app].[ItemComments] SET LAST_MODIFIED_BY = @lastmodifiedby, LAST_MODIFIED_DATE = GETDATE() WHERE ITEM_ID = @id;
	IF (@countComments > 0) DELETE FROM [app].[ItemComments] WHERE ITEM_ID = @id;

	UPDATE [app].[Items]
	SET LAST_MODIFIED_BY = @lastmodifiedby, LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id;	

	DELETE FROM [app].[Items]
	WHERE ID = @id;	

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetItem]
	@id int
AS

BEGIN TRY
	
	SELECT 
		item.*
	FROM [app].vwItems item	
	WHERE ID = @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spUpdateItem]
	@id int,
	@collection_id int,
	@status varchar(10),
	@type varchar(15),
	@brand_id int,
	@series_id int,
	@name varchar(100),
	@description varchar(250),
	@format varchar(15),
	@size varchar(10),
	@year int,
	@photo varchar(max),
	@lastmodifiedby varchar(100)
AS

BEGIN TRY

	UPDATE [app].[Items]
	SET	
		COLLECTION_ID = @collection_id,
		STATUS = @status,
		TYPE = @type,
		BRAND_ID = @brand_id,
		SERIES_ID = @series_id,
		NAME = @name,
		DESCRIPTION = @description,
		FORMAT = @format,
		SIZE = @size,
		YEAR = @year,
		PHOTO = @photo,
		LAST_MODIFIED_BY = @lastmodifiedby,
		LAST_MODIFIED_DATE = GETDATE()
	WHERE ID = @id;

	SELECT @id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH


GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spSearchItems]
	@startingindex int,
	@endingindex int,
	@orderby varchar(50),
	@order varchar(4),
	@id int,
	@collection_id int,
	@status varchar(10),
	@type varchar(15),
	@brand_id int,
	@series_id int,
	@name varchar(100),
	@description varchar(250),
	@format varchar(15),
	@size varchar(10),
	@year int,
	@freeform varchar(250),
    @modified_within varchar(10)	
AS

BEGIN TRY
	DECLARE @sqlfinal nvarchar(1000);
	DECLARE @sqlfinal2 nvarchar(1000);
	DECLARE @sqlbasecommand1 nvarchar(1000);
	DECLARE @sqlbasecommand2 nvarchar(1000);
	DECLARE @countstatement nvarchar(1000);
	DECLARE @sqlwhereclause nvarchar(1000);
	DECLARE @rowclause nvarchar(1000);
	DECLARE @stringstart varchar(5);

	SET @sqlbasecommand1 = ';WITH CTE AS (SELECT RowNumber = ROW_NUMBER() OVER (ORDER by ' + @orderby + ' ' + @order + '), * FROM [app].[vwItems_Search]';
	SET @sqlbasecommand2 = 'SELECT COUNT(*) as Results FROM [app].[vwItems_Search]';
	SET @sqlwhereclause = 
	(CASE WHEN @id IS NOT NULL THEN ' AND [ID] = ' + CAST(@id as varchar(50)) ELSE '' END)
	+ (CASE WHEN @collection_id IS NOT NULL THEN ' AND [COLLECTION_ID] = ' + CAST(@collection_id as varchar(50)) ELSE '' END)
	+ (CASE WHEN @status IS NOT NULL THEN ' AND [STATUS] = ''' + LTRIM(RTRIM(@status)) + '''' ELSE '' END)
	+ (CASE WHEN @type IS NOT NULL THEN ' AND [TYPE] = ''' + LTRIM(RTRIM(@type)) + '''' ELSE '' END)
	+ (CASE WHEN @brand_id IS NOT NULL THEN ' AND [BRAND_ID] = ' + CAST(@brand_id as varchar(50)) ELSE '' END)
	+ (CASE WHEN @series_id IS NOT NULL THEN ' AND [SERIES_ID] = ' + CAST(@series_id as varchar(50)) ELSE '' END)
	+ (CASE WHEN @name IS NOT NULL THEN ' AND [NAME] LIKE ''% ' + LTRIM(RTRIM(@name)) + ' %''' ELSE '' END)
	+ (CASE WHEN @description IS NOT NULL THEN ' AND [DESCRIPTION] LIKE ''% ' + LTRIM(RTRIM(@description)) + ' %''' ELSE '' END)
	+ (CASE WHEN @format IS NOT NULL THEN ' AND [FORMAT] = ''' + LTRIM(RTRIM(@format)) + '''' ELSE '' END)
	+ (CASE WHEN @size IS NOT NULL THEN ' AND [SIZE] = ''' + LTRIM(RTRIM(@size)) + '''' ELSE '' END)
	+ (CASE WHEN @year IS NOT NULL THEN ' AND [YEAR] = ''' + CAST(@year as varchar(50)) + '''' ELSE '' END)
	+ (CASE WHEN @freeform IS NOT NULL THEN ' AND [FULLDATA] LIKE ''%' + LTRIM(RTRIM(@freeform)) + '%''' ELSE '' END)
	+ (CASE WHEN @modified_within IS NOT NULL THEN ' AND DATEDIFF(day,LAST_MODIFIED_DATE,GETDATE()) BETWEEN 0 AND ' + @modified_within ELSE '' END);

	SET @rowclause = ') SELECT [ID],[COLLECTION_ID],[STATUS],[TYPE],[BRAND_ID],[SERIES_ID],[NAME],[FORMAT],[SIZE],[YEAR],[CREATED_BY],[CREATED_DATE],[LAST_MODIFIED_BY],[LAST_MODIFIED_DATE] FROM CTE WHERE RowNumber >= ' + CAST(@startingindex as varchar(50)) + ' AND RowNumber <= ' + CAST(@endingindex as varchar(50));

	SET @stringstart = LEFT(@sqlwhereclause, 5);
	IF (@stringstart = ' AND ') SET @sqlwhereclause = RIGHT(@sqlwhereclause, LEN(@sqlwhereclause)-5);
	
	IF (@sqlwhereclause = '') SET @sqlfinal = @sqlbasecommand1 + @rowclause;
	ELSE SET @sqlfinal = @sqlbasecommand1 + ' WHERE ' + @sqlwhereclause + @rowclause;

	IF (@sqlwhereclause = '') SET @countstatement = @sqlbasecommand2;
	ELSE SET @countstatement = @sqlbasecommand2 + ' WHERE ' + @sqlwhereclause;

	EXECUTE sp_executesql @countstatement;
	EXECUTE sp_executesql @sqlfinal;

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetItems]
	@search varchar(250)
AS

BEGIN TRY

	IF (@search IS NOT NULL AND LEN(LTRIM(RTRIM(@search))) > 0) SELECT * FROM [app].[vwItems_Search] WHERE FULLDATA LIKE '%' + LTRIM(RTRIM(@search)) + '%'
	ELSE SELECT * FROM [app].[vwItems]

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------

CREATE OR ALTER PROCEDURE [app].[spGetItemsPerCollection]
	@collection_id int,
	@search varchar(250)
	
AS

BEGIN TRY

	IF (@search IS NOT NULL AND LEN(LTRIM(RTRIM(@search))) > 0) SELECT * FROM [app].[vwItems_Search] WHERE COLLECTION_ID = @collection_id AND FULLDATA LIKE '%' + LTRIM(RTRIM(@search)) + '%'
	ELSE SELECT * FROM [app].[vwItems] WHERE COLLECTION_ID = @collection_id

END TRY

BEGIN CATCH

	DECLARE @ErrorMessage nvarchar(4000);
	DECLARE @ErrorSeverity int;
	DECLARE @ErrorState int;

	SELECT @ErrorMessage = ERROR_MESSAGE(),
		   @ErrorSeverity = ERROR_SEVERITY(),
		   @ErrorState = ERROR_STATE();

	SELECT @ErrorMessage, @ErrorSeverity, @ErrorState;

	IF (@@TRANCOUNT > 0)
		ROLLBACK TRANSACTION;

END CATCH

GO

-----------------------------------------------------------
