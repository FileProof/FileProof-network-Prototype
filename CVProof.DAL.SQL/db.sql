CREATE TABLE [dbo].[Header](
	[Id] [nvarchar](100) NULL,
	[Category] [nvarchar](100) NULL,
	[IssuerName] [nvarchar](100) NULL,
	[IssuerUUID] [nvarchar](100) NULL,
	[ValidatorName] [nvarchar](100) NULL,
	[ValidatorUUID] [nvarchar](100) NULL,
	[ValidatorLegitimationHeaderID] [nvarchar](100) NULL,
	[RecipientName] [nvarchar](100) NULL,
	[RecipientUUID] [nvarchar](100) NULL,
	[PreviousHeaderID] [nvarchar](100) NULL,
	[ValidationCounter] [nvarchar](100) NULL,
	[NextHeaderID] [nvarchar](100) NULL,
	[Timestamp] [nvarchar](100) NULL,
	[BlockNumber] [nvarchar](100) NULL,
	[DataAddress] [nvarchar](100) NULL,
	[ValidationExpiry] [nvarchar](100) NULL,
	[DataHash] [nvarchar](256) NULL,
	[Nonce] [nvarchar](100) NULL,
	[Stored] [bit] NOT NULL,
	[Attachment] [bit] NOT NULL,
	[GlobalHash] [nvarchar](256) NULL
)

GO



CREATE TABLE [dbo].[Profile](
	[Id] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[Roles] [nvarchar](100) NULL,
	[Image] [varbinary](max) NULL
	
)

GO