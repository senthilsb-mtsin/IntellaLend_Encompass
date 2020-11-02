USE [EProductDev]
GO

/****** Object:  Table [IL].[ELoanAttachmentUpload]    Script Date: 4/22/2020 12:55:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [T1].[ELoanAttachmentDownload](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LoanID] [bigint] NOT NULL,
	[ELoanGUID] [uniqueidentifier] NOT NULL,
	[TypeOfDownload] [bigint] NOT NULL,
	[Status] [bigint] NOT NULL,
	[Error] [nvarchar](max) NULL,
	[Createdon] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


-------------------------------

USE [EProductDev]
GO

/****** Object:  Table [T1].[ELoanAttachmentUpload]    Script Date: 4/22/2020 12:55:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [T1].[ELoanAttachmentDownload](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LoanID] [bigint] NOT NULL,
	[ELoanGUID] [uniqueidentifier] NOT NULL,
	[TypeOfDownload] [bigint] NOT NULL,
	[Status] [bigint] NOT NULL,
	[Error] [nvarchar](max) NULL,
	[Createdon] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

------------------------
------------Modifying CustomFieldValue----------------------
update [EProductDev].[IL].[IntellaAndEncompassFetchFields] set EncompassFieldValue='N' where ID=2

--------------------------------------------
USE [EProductDev]
GO

INSERT INTO [IL].[IntellaAndEncompassFetchFields]
           ([FieldType]
           ,[EncompassFieldID]
           ,[EncompassFieldDescription]
           ,[EncompassFieldValue]
           ,[IntellaMappingValue]
           ,[IntellaMappingColumn]
           ,[Notes]
           ,[Active]
           ,[IsSingleValue])
     VALUES
           ('Import'
           ,'Fields.CX.DOWNLOADED'
           ,'File Downloaded by Intella'
           ,''
           ,''
		   ,NULL
           ,'Empty File Downloaded by Intella field should be fetched from Encompass'
           ,1
           ,1)
GO
---------------------------