
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/28/2013 13:16:57
-- Generated from EDMX file: C:\Users\Santhosh.TILLID\Desktop\My Works\BibleThonLatest\BibleThonWebPortal\AlbaDL\BibleThonDL.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TWO];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[OrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderDetails];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'OrderDetails'
CREATE TABLE [dbo].[OrderDetails] (
    [Id] int  NOT NULL,
    [OrdDate] datetime  NULL,
    [Status] nchar(12)  NULL,
    [FormData] nvarchar(max)  NULL,
    [OrdNo] nchar(25)  NULL,
    [Operator] nchar(255)  NULL,
    [CustomerName] nchar(255)  NULL,
    [OrdTotal] decimal(18,2)  NULL
);
GO

-- Creating table 'AuthorizeNetTransactions'
CREATE TABLE [dbo].[AuthorizeNetTransactions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CardNumber] nvarchar(30)  NOT NULL,
    [AuthorizationCode] nvarchar(255)  NOT NULL,
    [InvoiceNumber] nvarchar(255)  NOT NULL,
    [TransactionID] nvarchar(255)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [Approved] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [PK_OrderDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AuthorizeNetTransactions'
ALTER TABLE [dbo].[AuthorizeNetTransactions]
ADD CONSTRAINT [PK_AuthorizeNetTransactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------