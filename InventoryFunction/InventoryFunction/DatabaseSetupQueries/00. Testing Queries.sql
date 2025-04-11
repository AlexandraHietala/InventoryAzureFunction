USE [SEInventoryDB]
GO

EXEC [dbo].[spGetUsers]
EXEC [dbo].[spGetUser] 1
EXEC [dbo].[spGetRoles]
EXEC [dbo].[spGetRole] 1000
EXEC [dbo].[spAddUser] 'Alexandra', 'salt', 'hash', 9999, 'AHIETALA'
EXEC [dbo].[spGetAuth] 2
EXEC [dbo].[spUpdateUser] 2, 'Alexandra', 'salt', 'hash', 1000, 'AHIETALA' 
EXEC [dbo].[spRemoveUser] 2, 'AHIETALA'
EXEC [dbo].[spGetUserHistory] 2

EXEC [dbo].[spGetASeries] 1
EXEC [dbo].[spGetSeries] NULL
EXEC [dbo].[spGetSeries] 'uni'
EXEC [dbo].[spAddSeries] 'un-cornos', 1, 'not unicornos', 'AHIETALA'
EXEC [dbo].[spUpdateSeries] 3, 'un-cornos', 1, 'def not unicornos', 'AHIETALA'
EXEC [dbo].[spRemoveSeries] 3, 'AHIETALA'
EXEC [dbo].[spGetSeriesHistory] 3

EXEC [dbo].[spGetBrand] 1
EXEC [dbo].[spGetBrands] NULL
EXEC [dbo].[spGetBrands] 'cocoa'
EXEC [dbo].[spAddBrand] 'MyTest Brand', 'This is just for testing', 'AHIETALA'
EXEC [dbo].[spAddBrand] 'St. James Infirmary', 'Oh Cocoa', 'AHIETALA'
EXEC [dbo].[spUpdateBrand] 3, 'St. James Infirmary', NULL, 'AHIETALA'
EXEC [dbo].[spRemoveBrand] 3, 'AHIETALA'
EXEC [dbo].[spGetBrandHistory] 3

EXEC [dbo].[spGetCollection] 1
EXEC [dbo].[spGetCollections] NULL
EXEC [dbo].[spGetCollections] 'cocoa'
EXEC [dbo].[spAddCollection] 'MyTest Brand', 'This is just for testing', 'AHIETALA'
EXEC [dbo].[spAddCollection] 'St. James Infirmary', 'Oh Cocoa', 'AHIETALA'
EXEC [dbo].[spUpdateCollection] 2, 'St. James Infirmary', NULL, 'AHIETALA'
EXEC [dbo].[spRemoveCollection] 2, 'AHIETALA'
EXEC [dbo].[spGetHistory] 1, 'Collection'

EXEC [dbo].[spGetItems] NULL
EXEC [dbo].[spGetItems] 'min'
EXEC [dbo].[spGetItemsPerCollection] 1, 'pri'
EXEC [dbo].[spGetItemsPerCollection] 1, NULL
EXEC [dbo].[spSearchItems] 0, 5, 'STATUS', 'ASC', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
EXEC [dbo].[spGetItem] 2
EXEC [dbo].[spAddItem] 'OWNED', 'BLIND', 1, 'My Item', NULL, 'KEYCHAIN', 'LARGE', 2023, NULL, 'AHIETALA'
EXEC [dbo].[spUpdateItem] 2, 'OWNED', 'BLIND', 1, 'My Item Record', NULL, 'FIGURE', 'REGULAR', 2016, NULL, 'AHIETALA'
EXEC [dbo].[spRemoveItem] 2, 'AHIETALA'
EXEC [dbo].[spGetHistory] 2, 'Item'
EXEC [dbo].[spGetGeneralHistory] 2

EXEC [dbo].[spGetItemComment] 1 
EXEC [dbo].[spGetItemComments] 1 
EXEC [dbo].[spAddItemComment] 1, 'Test Comment :)', 'AHIETALA'
EXEC [dbo].[spRemoveItemComment] 2, 'AHIETALA'
EXEC [dbo].[spGetHistory] 1, 'ItemComment' -- Input is ITEM ID not Comment ID
