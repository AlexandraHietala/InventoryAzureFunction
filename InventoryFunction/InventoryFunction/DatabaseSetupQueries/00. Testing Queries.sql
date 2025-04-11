USE [SEInventory]
GO

EXEC [app].[spGetUsers]
EXEC [app].[spGetUser] 1
EXEC [app].[spGetRoles]
EXEC [app].[spGetRole] 1000
EXEC [app].[spAddUser] 'Alexandra', 'salt', 'hash', 9999, 'AHIETALA'
EXEC [app].[spGetAuth] 2
EXEC [app].[spUpdateUser] 2, 'Alexandra', 'salt', 'hash', 1000, 'AHIETALA' 
EXEC [app].[spRemoveUser] 2, 'AHIETALA'
EXEC [app].[spGetUserHistory] 2

EXEC [app].[spGetASeries] 1
EXEC [app].[spGetSeries] NULL
EXEC [app].[spGetSeries] 'uni'
EXEC [app].[spAddSeries] 'un-cornos', 1, 'not unicornos', 'AHIETALA'
EXEC [app].[spUpdateSeries] 3, 'un-cornos', 1, 'def not unicornos', 'AHIETALA'
EXEC [app].[spRemoveSeries] 3, 'AHIETALA'
EXEC [app].[spGetSeriesHistory] 3

EXEC [app].[spGetBrand] 1
EXEC [app].[spGetBrands] NULL
EXEC [app].[spGetBrands] 'cocoa'
EXEC [app].[spAddBrand] 'MyTest Brand', 'This is just for testing', 'AHIETALA'
EXEC [app].[spAddBrand] 'St. James Infirmary', 'Oh Cocoa', 'AHIETALA'
EXEC [app].[spUpdateBrand] 3, 'St. James Infirmary', NULL, 'AHIETALA'
EXEC [app].[spRemoveBrand] 3, 'AHIETALA'
EXEC [app].[spGetBrandHistory] 3

EXEC [app].[spGetCollection] 1
EXEC [app].[spGetCollections] NULL
EXEC [app].[spGetCollections] 'cocoa'
EXEC [app].[spAddCollection] 'MyTest Brand', 'This is just for testing', 'AHIETALA'
EXEC [app].[spAddCollection] 'St. James Infirmary', 'Oh Cocoa', 'AHIETALA'
EXEC [app].[spUpdateCollection] 2, 'St. James Infirmary', NULL, 'AHIETALA'
EXEC [app].[spRemoveCollection] 2, 'AHIETALA'
EXEC [app].[spGetHistory] 1, 'Collection'

EXEC [app].[spGetItems] NULL
EXEC [app].[spGetItems] 'min'
EXEC [app].[spGetItemsPerCollection] 1, 'pri'
EXEC [app].[spGetItemsPerCollection] 1, NULL
EXEC [app].[spSearchItems] 0, 5, 'STATUS', 'ASC', NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
EXEC [app].[spGetItem] 2
EXEC [app].[spAddItem] 'OWNED', 'BLIND', 1, 'My Item', NULL, 'KEYCHAIN', 'LARGE', 2023, NULL, 'AHIETALA'
EXEC [app].[spUpdateItem] 2, 'OWNED', 'BLIND', 1, 'My Item Record', NULL, 'FIGURE', 'REGULAR', 2016, NULL, 'AHIETALA'
EXEC [app].[spRemoveItem] 2, 'AHIETALA'
EXEC [app].[spGetHistory] 2, 'Item'
EXEC [app].[spGetGeneralHistory] 2

EXEC [app].[spGetItemComment] 1 
EXEC [app].[spGetItemComments] 1 
EXEC [app].[spAddItemComment] 1, 'Test Comment :)', 'AHIETALA'
EXEC [app].[spRemoveItemComment] 2, 'AHIETALA'
EXEC [app].[spGetHistory] 1, 'ItemComment' -- Input is ITEM ID not Comment ID
