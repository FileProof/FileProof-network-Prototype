USE [master]
GO
/****** Object:  Database [cvproof-stage]    Script Date: 10/12/2018 4:54:34 PM ******/
CREATE DATABASE [cvproof-stage]
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [cvproof-stage].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [cvproof-stage] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [cvproof-stage] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [cvproof-stage] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [cvproof-stage] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [cvproof-stage] SET ARITHABORT OFF 
GO
ALTER DATABASE [cvproof-stage] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [cvproof-stage] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [cvproof-stage] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [cvproof-stage] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [cvproof-stage] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [cvproof-stage] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [cvproof-stage] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [cvproof-stage] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [cvproof-stage] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [cvproof-stage] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [cvproof-stage] SET  ENABLE_BROKER 
GO
ALTER DATABASE [cvproof-stage] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [cvproof-stage] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [cvproof-stage] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [cvproof-stage] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [cvproof-stage] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [cvproof-stage] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [cvproof-stage] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [cvproof-stage] SET RECOVERY FULL 
GO
ALTER DATABASE [cvproof-stage] SET  MULTI_USER 
GO
ALTER DATABASE [cvproof-stage] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [cvproof-stage] SET DB_CHAINING OFF 
GO
USE [cvproof-stage]
GO
/****** Object:  Table [dbo].[Header]    Script Date: 10/12/2018 4:54:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	[Stored] [bit] NOT NULL
)

GO
INSERT [dbo].[Header] ([Id], [Category], [IssuerName], [IssuerUUID], [ValidatorName], [ValidatorUUID], [ValidatorLegitimationHeaderID], [RecipientName], [RecipientUUID], [PreviousHeaderID], [ValidationCounter], [NextHeaderID], [Timestamp], [BlockNumber], [DataAddress], [ValidationExpiry], [DataHash], [Nonce], [Stored]) VALUES (N'0x42ca9db895526ac9b82d230445f3f6400723efd510ab93d026a87ac0dd508db7', N'File', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'0x34952b6e856df5542e5973bb02aaa88030f2e770d5f92e9a7892bccaf66e2d8a', N'e?sNz?[8I66??o?;G;???(', 0)
INSERT [dbo].[Header] ([Id], [Category], [IssuerName], [IssuerUUID], [ValidatorName], [ValidatorUUID], [ValidatorLegitimationHeaderID], [RecipientName], [RecipientUUID], [PreviousHeaderID], [ValidationCounter], [NextHeaderID], [Timestamp], [BlockNumber], [DataAddress], [ValidationExpiry], [DataHash], [Nonce], [Stored]) VALUES (N'0x1115658ed8e51bd1eb6b9bff1fc3e981aa9a47ba473bcf7058f7139aec742bcd', N'File', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'0x7a3babb0baa478298788546fc318d5a5fb4034ee5bd136dfdce9a0cb3fd0341f', N'?????l???????lUH???8f]5????', 1)
INSERT [dbo].[Header] ([Id], [Category], [IssuerName], [IssuerUUID], [ValidatorName], [ValidatorUUID], [ValidatorLegitimationHeaderID], [RecipientName], [RecipientUUID], [PreviousHeaderID], [ValidationCounter], [NextHeaderID], [Timestamp], [BlockNumber], [DataAddress], [ValidationExpiry], [DataHash], [Nonce], [Stored]) VALUES (N'0x4df393d64dcda363ea10b089a79f1c6bd96312a22037533948a0bc7ec914e329', N'TempId', N'MASTER-MASTER', N'', N'0x0100000000000000000000000000000000000000000000000000000000000000', N'0x0100000000000000000000000000000000000000000000000000000000000000', N'', N'CVPROOF', N'', N'', N'1', N'', N'1539161523', N'4204752', N'0xace2fbd526914cce865dbc74a5c39cf517deb3eec4e5dccf1c74306af5cdb321', N'', N'0x89195828673a49ccd44d6edb6cc66e383cf78a08288c0e6b8df82c90ee7b5891', N'?F?????B?8??Z?c[XI????~?jq?', 0)
INSERT [dbo].[Header] ([Id], [Category], [IssuerName], [IssuerUUID], [ValidatorName], [ValidatorUUID], [ValidatorLegitimationHeaderID], [RecipientName], [RecipientUUID], [PreviousHeaderID], [ValidationCounter], [NextHeaderID], [Timestamp], [BlockNumber], [DataAddress], [ValidationExpiry], [DataHash], [Nonce], [Stored]) VALUES (N'0x0100000000000000000000000000000000000000000000000000000000000000', N'Root', N'', N'', N'0x0100000000000000000000000000000000000000000000000000000000000000', N'0x0100000000000000000000000000000000000000000000000000000000000000', N'', N'', N'', N'', N'1', N'', N'1536625353', N'4014541', N'0xb5d5052f9417fc4b52fe06dc2ad82d849b03661ab452a2a81256d49818dcb4ed', N'', N'', N'', 0)
INSERT [dbo].[Header] ([Id], [Category], [IssuerName], [IssuerUUID], [ValidatorName], [ValidatorUUID], [ValidatorLegitimationHeaderID], [RecipientName], [RecipientUUID], [PreviousHeaderID], [ValidationCounter], [NextHeaderID], [Timestamp], [BlockNumber], [DataAddress], [ValidationExpiry], [DataHash], [Nonce], [Stored]) VALUES (N'0xa9f40ad2c2d9e00d6a4315261c6278428ecd8692bc4958f558f36bb4139fec82', N'TempId', N'CVPROOF', N'', N'0x4df393d64dcda363ea10b089a79f1c6bd96312a22037533948a0bc7ec914e329', N'0x4df393d64dcda363ea10b089a79f1c6bd96312a22037533948a0bc7ec914e329', N'', N'Ray CHOW-TOUN', N'', N'', N'1', N'', N'1539161825', N'4204767', N'0xc5ac590dcac2f73e221ad5172664d8b5e363c4296043fd88bad5c42b9e04ae8c', N'', N'0x91a5dab5b4b9ac40e03658b6c818fdacf63c9fdde5c5b2a5a3ff97bd1afa9a86', N'h??Q	??u:h???2l???^=?\?C?', 1)
INSERT [dbo].[Header] ([Id], [Category], [IssuerName], [IssuerUUID], [ValidatorName], [ValidatorUUID], [ValidatorLegitimationHeaderID], [RecipientName], [RecipientUUID], [PreviousHeaderID], [ValidationCounter], [NextHeaderID], [Timestamp], [BlockNumber], [DataAddress], [ValidationExpiry], [DataHash], [Nonce], [Stored]) VALUES (N'0x47d9cc083b625cb9eef58b02f6c00b88eed07e3113fb29ac595437498ebd46bc', N'ValidatorLegitimation', N'CVPROOF', N'', N'0x4df393d64dcda363ea10b089a79f1c6bd96312a22037533948a0bc7ec914e329', N'0x4df393d64dcda363ea10b089a79f1c6bd96312a22037533948a0bc7ec914e329', N'', N'Ray CHOW-TOUN', N'0xa9f40ad2c2d9e00d6a4315261c6278428ecd8692bc4958f558f36bb4139fec82', N'', N'1', N'', N'1539162644', N'4204819', N'0x4adf3bca2e0de641d2c2e7d819ad54a24fe323090c65c9fdd523c71172252dbc', N'', N'0x7638d3a228efb1abf2a7c47a6cf648a08ed0b7a4ea919a17c525099ae9b2a974', N'&??|#
??3?
??;?l{{T>??m???K?', 1)
INSERT [dbo].[Header] ([Id], [Category], [IssuerName], [IssuerUUID], [ValidatorName], [ValidatorUUID], [ValidatorLegitimationHeaderID], [RecipientName], [RecipientUUID], [PreviousHeaderID], [ValidationCounter], [NextHeaderID], [Timestamp], [BlockNumber], [DataAddress], [ValidationExpiry], [DataHash], [Nonce], [Stored]) VALUES (N'0x41de17c0e618d52c21220ed4ebb8b070cde699adbb3935ab56462869213d62b6', N'TempId', N'CVPROOF', N'', N'0x4df393d64dcda363ea10b089a79f1c6bd96312a22037533948a0bc7ec914e329', N'0x4df393d64dcda363ea10b089a79f1c6bd96312a22037533948a0bc7ec914e329', N'', N'Professionals.aero', N'', N'', N'1', N'', N'1539175976', N'4206667', N'0xeb7040768a8f0d54fa342736ebad5dc5870037032c1364b9d8a2982700c01fee', N'', N'0x7f7daa522201dc625fe7f3cd8ed86ae2e5ee2943d994c55fe43307797f634677', N'B????6d?J?,6???IqD?0?9Z???`', 1)
USE [master]
GO
ALTER DATABASE [cvproof-stage] SET  READ_WRITE 
GO
