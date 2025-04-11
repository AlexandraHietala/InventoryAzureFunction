-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [SEInventory]
GO

-----------------------------------------------------------

CREATE TABLE [app].[Roles](
	[ID] [int] PRIMARY KEY NOT NULL,
	[DESCRIPTION] [varchar](250) NULL,
)

GO

-----------------------------------------------------------

CREATE TABLE [app].[Users](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[NAME] [varchar](250) NOT NULL,
	[PASS_SALT] [varchar](250) NOT NULL,
	[PASS_HASH] [varchar](250) NOT NULL,
	[ROLE_ID] [int] NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
)

GO

ALTER TABLE [app].[Users] WITH CHECK ADD CONSTRAINT [FK_Users_Roles] FOREIGN KEY([ROLE_ID]) REFERENCES [app].[Roles] ([ID])
GO

ALTER TABLE [app].[Users] CHECK CONSTRAINT [FK_Users_Roles] 
GO

-----------------------------------------------------------

CREATE TABLE [app].[Series](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[SERIES_NAME] [varchar](250) NOT NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
)

GO

-----------------------------------------------------------

CREATE TABLE [app].[Brands](
	[ID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[BRAND_NAME] [varchar](50) NOT NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
)

GO

-----------------------------------------------------------

CREATE TABLE [app].[Collections](
	[ID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[COLLECTION_NAME] [varchar](50) NOT NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
)

GO

-----------------------------------------------------------

CREATE TABLE [app].[Items](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[COLLECTION_ID] [int] NOT NULL,
	[STATUS] [varchar](10) NOT NULL CHECK ([STATUS] IN ('OWNED','NOT OWNED','WISHLIST','PENDING')) DEFAULT 'OWNED',
	[TYPE] [varchar](15) NOT NULL CHECK ([TYPE] IN ('BLIND','SOLD SEPARATELY','SET')) DEFAULT 'BLIND',
	[BRAND_ID] [int] NULL,
	[SERIES_ID] [int] NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[FORMAT] [varchar](15) NOT NULL CHECK ([FORMAT] IN ('KEYCHAIN','FIGURE','PLUSH','OTHER')) DEFAULT 'FIGURE',
	[SIZE] [varchar](10) NOT NULL CHECK ([SIZE] IN ('MINI','REGULAR','LARGE','GIANT','IRREGULAR')) DEFAULT 'REGULAR',
	[YEAR] [int] NULL,
	[PHOTO] [varchar](max) NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
)

GO

ALTER TABLE [app].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Series] FOREIGN KEY([SERIES_ID])
REFERENCES [app].[Series] ([ID])
GO

ALTER TABLE [app].[Items] CHECK CONSTRAINT [FK_Items_Series]
GO

ALTER TABLE [app].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Brand] FOREIGN KEY([BRAND_ID])
REFERENCES [app].[Brands] ([ID])
GO

ALTER TABLE [app].[Items] CHECK CONSTRAINT [FK_Items_Brand]
GO

ALTER TABLE [app].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Collection] FOREIGN KEY([COLLECTION_ID])
REFERENCES [app].[Collections] ([ID])
GO

ALTER TABLE [app].[Items] CHECK CONSTRAINT [FK_Items_Collection]
GO

-----------------------------------------------------------

CREATE TABLE [app].[ItemComments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ITEM_ID] [int] NOT NULL,
	[COMMENT] [varchar](max) NOT NULL,
	[CREATED_BY] [varchar](100) NOT NULL DEFAULT 'SYSTEM',
	[CREATED_DATE] [datetime] NOT NULL DEFAULT GetDate(),
	[LAST_MODIFIED_BY] [varchar](100) NULL DEFAULT 'SYSTEM',
	[LAST_MODIFIED_DATE] [datetime] NULL DEFAULT GetDate(),
 CONSTRAINT [PK_ItemComments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,		
	[ITEM_ID] ASC
	
))

GO

ALTER TABLE [app].[ItemComments]  WITH CHECK ADD  CONSTRAINT [FK_ItemComments_Items] FOREIGN KEY([ITEM_ID])
REFERENCES [app].[Items] ([ID])
GO

ALTER TABLE [app].[ItemComments] CHECK CONSTRAINT [FK_ItemComments_Items]
GO

-----------------------------------------------------------
