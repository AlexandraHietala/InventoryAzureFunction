-----------------------------------------------------------

-- Notes:


-----------------------------------------------------------

USE [SEInventoryDB]
GO

-----------------------------------------------------------

CREATE TABLE [dbo].[Roles](
	[ID] [int] PRIMARY KEY NOT NULL,
	[DESCRIPTION] [varchar](250) NULL,
)

GO

-----------------------------------------------------------

CREATE TABLE [dbo].[Users](
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

ALTER TABLE [dbo].[Users] WITH CHECK ADD CONSTRAINT [FK_Users_Roles] FOREIGN KEY([ROLE_ID]) REFERENCES [dbo].[Roles] ([ID])
GO

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles] 
GO

-----------------------------------------------------------

CREATE TABLE [dbo].[Series](
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

CREATE TABLE [dbo].[Brands](
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

CREATE TABLE [dbo].[Collections](
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

CREATE TABLE [dbo].[Items](
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

ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Series] FOREIGN KEY([SERIES_ID])
REFERENCES [dbo].[Series] ([ID])
GO

ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_Series]
GO

ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Brand] FOREIGN KEY([BRAND_ID])
REFERENCES [dbo].[Brands] ([ID])
GO

ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_Brand]
GO

ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Collection] FOREIGN KEY([COLLECTION_ID])
REFERENCES [dbo].[Collections] ([ID])
GO

ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_Collection]
GO

-----------------------------------------------------------

CREATE TABLE [dbo].[ItemComments](
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

ALTER TABLE [dbo].[ItemComments]  WITH CHECK ADD  CONSTRAINT [FK_ItemComments_Items] FOREIGN KEY([ITEM_ID])
REFERENCES [dbo].[Items] ([ID])
GO

ALTER TABLE [dbo].[ItemComments] CHECK CONSTRAINT [FK_ItemComments_Items]
GO

-----------------------------------------------------------
