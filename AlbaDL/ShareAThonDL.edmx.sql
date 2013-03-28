
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/28/2013 16:34:12
-- Generated from EDMX file: C:\Users\Santhosh.TILLID\Desktop\My Works\BibleThonLatest\BibleThonWebPortal\AlbaDL\ShareAThonDL.edmx
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

IF OBJECT_ID(N'[dbo].[FK_ShareAThonDonationShareAThonDonationFrequency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ShareAThonDonationFrequencies] DROP CONSTRAINT [FK_ShareAThonDonationShareAThonDonationFrequency];
GO
IF OBJECT_ID(N'[dbo].[FK_ShareAThonDonationShareAThonOfferLine]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ShareAThonOfferLines] DROP CONSTRAINT [FK_ShareAThonDonationShareAThonOfferLine];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ShareAThonDonations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShareAThonDonations];
GO
IF OBJECT_ID(N'[dbo].[ShareAThonDonationFrequencies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShareAThonDonationFrequencies];
GO
IF OBJECT_ID(N'[dbo].[ShareAThonOfferLines]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShareAThonOfferLines];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ShareAThonDonations'
CREATE TABLE [dbo].[ShareAThonDonations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] varchar(50)  NOT NULL,
    [CurrentlyDonorOf] decimal(19,4)  NULL,
    [InitialChargeOn] datetime  NULL,
    [DonationAmount] decimal(19,4)  NULL,
    [IncreasingTo] decimal(19,4)  NULL,
    [DayToChargeMonthly] int  NULL,
    [OrderId] varchar(50)  NOT NULL,
    [AuthorizeNetSubscriptionId] bigint  NOT NULL
);
GO

-- Creating table 'ShareAThonDonationFrequencies'
CREATE TABLE [dbo].[ShareAThonDonationFrequencies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] varchar(50)  NOT NULL,
    [DueDate] datetime  NOT NULL,
    [Amount] decimal(19,4)  NOT NULL,
    [Status] varchar(50)  NOT NULL,
    [ShareAThonDonationId] int  NOT NULL,
    [ShareAThonDonationId1] int  NOT NULL,
    [ModeOfDonation] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ShareAThonOfferLines'
CREATE TABLE [dbo].[ShareAThonOfferLines] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderId] varchar(50)  NOT NULL,
    [OfferNo] varchar(50)  NOT NULL,
    [Description] varchar(50)  NULL,
    [Qty] int  NOT NULL,
    [ShareAThonDonationId] int  NOT NULL,
    [ShareAThonDonationId1] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ShareAThonDonations'
ALTER TABLE [dbo].[ShareAThonDonations]
ADD CONSTRAINT [PK_ShareAThonDonations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ShareAThonDonationFrequencies'
ALTER TABLE [dbo].[ShareAThonDonationFrequencies]
ADD CONSTRAINT [PK_ShareAThonDonationFrequencies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [OrderId], [OfferNo], [Qty], [ShareAThonDonationId] in table 'ShareAThonOfferLines'
ALTER TABLE [dbo].[ShareAThonOfferLines]
ADD CONSTRAINT [PK_ShareAThonOfferLines]
    PRIMARY KEY CLUSTERED ([Id], [OrderId], [OfferNo], [Qty], [ShareAThonDonationId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ShareAThonDonationId1] in table 'ShareAThonDonationFrequencies'
ALTER TABLE [dbo].[ShareAThonDonationFrequencies]
ADD CONSTRAINT [FK_ShareAThonDonationShareAThonDonationFrequency]
    FOREIGN KEY ([ShareAThonDonationId1])
    REFERENCES [dbo].[ShareAThonDonations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShareAThonDonationShareAThonDonationFrequency'
CREATE INDEX [IX_FK_ShareAThonDonationShareAThonDonationFrequency]
ON [dbo].[ShareAThonDonationFrequencies]
    ([ShareAThonDonationId1]);
GO

-- Creating foreign key on [ShareAThonDonationId1] in table 'ShareAThonOfferLines'
ALTER TABLE [dbo].[ShareAThonOfferLines]
ADD CONSTRAINT [FK_ShareAThonDonationShareAThonOfferLine]
    FOREIGN KEY ([ShareAThonDonationId1])
    REFERENCES [dbo].[ShareAThonDonations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ShareAThonDonationShareAThonOfferLine'
CREATE INDEX [IX_FK_ShareAThonDonationShareAThonOfferLine]
ON [dbo].[ShareAThonOfferLines]
    ([ShareAThonDonationId1]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------