/****** Object:  StoredProcedure [dbo].[GET_AUTO_VALIDATION_SKIP_DOCS]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GET_AUTO_VALIDATION_SKIP_DOCS]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT CASE 
			WHEN DOCUMENT_NAME = 'Loan Application 1003 Format 1'
				OR DOCUMENT_NAME = 'Loan Application 1003 Format 2'
				THEN 'Loan Application 1003'
			ELSE DOCUMENT_NAME
			END DOCUMENT_NAME
	FROM MTS_AUTO_VALIDATION_SKIP WITH (NOLOCK)

END


GO
/****** Object:  StoredProcedure [dbo].[GetAdvancedRules]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		MTS
-- Create date: 5/20/2015
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAdvancedRules] @inputConfigId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT [ConfigId]
      ,[AdvancedOptions]
      ,[Flag]
  FROM [dbo].[MTS.IDC_DOC_CONFIG]
  WHERE ConfigId = @inputConfigId
END





GO
/****** Object:  StoredProcedure [dbo].[GetAppendRules]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		MTS
-- Create date: 5/20/2015
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAppendRules] @inputConfigId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT [ConfigId]
      ,[Sequence]
      ,[DocType1]
      ,[DocLocation]
      ,[DocType2]
  FROM [dbo].[MTS.IDC_DOC_APPEND]
  WHERE ConfigId = @inputConfigId
  ORDER BY Sequence ASC
END




GO
/****** Object:  StoredProcedure [dbo].[GetConcatenateRules]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Author:  MTS  
-- Create date: 05/19/2015  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[GetConcatenateRules] @inputConfigId int 
	,@ephesoftModule int
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    /****** Script for SelectTopNRows command from SSMS  ******/  
SELECT [ConfigId]  
      ,[DocType]  
      ,[ConsecutiveFlag]  
  FROM [MTS.IDC_DOC_CONCATENATE]  
  WHERE ConfigId = @inputConfigId  AND EphesoftModuleID = @ephesoftModule
END  
  
  

GO
/****** Object:  StoredProcedure [dbo].[GetConfigId]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetConfigId] @EphesoftBatchClass nvarchar(30)
AS
BEGIN
SET NOCOUNT ON
SELECT [ConfigId]
      ,[EphesoftBatchClass]
  FROM [MTS.IDC_RULE_CONFIG]
  WHERE EphesoftBatchClass = @EphesoftBatchClass
END




GO
/****** Object:  StoredProcedure [dbo].[GetConfigurations]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		MTS
-- Create date: 5/20/2015
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetConfigurations] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT [Id],
	   [KEY],
       [VALUE]
  FROM [dbo].[MTS.IDC_CONFIGURATIONS]
END






GO
/****** Object:  StoredProcedure [dbo].[GetConversionRules]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConversionRules] 
(@inputConfigId int,@ephesoftModuleId bigint)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT [ConfigId]
      ,[DocType]
      ,[ToDocType] 
	  ,CONVERSION_RULE
  FROM [MTS.IDC_DOC_CONVERSION]
  WHERE ConfigId = @inputConfigId and EphesoftModuleId=@ephesoftModuleId
END


GO
/****** Object:  StoredProcedure [dbo].[GetDocStackingOrder]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetDocStackingOrder] @configID bigint
AS
BEGIN
SET NOCOUNT ON
SELECT ConfigID,SequenceNumber,DocumentName
  FROM [dbo].[MTS.DOCUMENT_STACKING_ORDER]
  WHERE ConfigID =@configID order by SequenceNumber

  End

GO
/****** Object:  StoredProcedure [dbo].[GetDocumentAutoValidationSkip]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetDocumentAutoValidationSkip]
AS
BEGIN
	SELECT ID
		,[DOCUMENT_NAME]
		,[INSTANCE]
	FROM MTS_AUTO_VALIDATION_SKIP WITH (NOLOCK)
END



GO
/****** Object:  StoredProcedure [dbo].[GetDocumentFieldMapping]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetDocumentFieldMapping] (@DOCUMENTLIST VARCHAR(MAX),@SERVICE_NAME VARCHAR(255))
AS
BEGIN
	SELECT [MAPPING_ID]
		,[TABLE_NAME]
		,[DOCUMENT_NAME]
		,[FIELD_NAME]
		,[COLUMN_NAME]
		,[CONDITION]
		,[CREATED_ON]
		,[MODIFIED_ON]
	FROM [dbo].[MTS_FIELD_LOOKUP_MAPPING]
	WHERE DOCUMENT_NAME IN (
			SELECT DATA
			FROM [dbo].[function_string_to_table](@DOCUMENTLIST, '|')
			)
			AND SERVICE_NAME=@SERVICE_NAME
	ORDER BY DOCUMENT_NAME,TABLE_NAME
END





GO
/****** Object:  StoredProcedure [dbo].[GetParentChildMergeRules]    Script Date: 2/28/2020 9:21:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetParentChildMergeRules] @INPUTCONFIGID BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [ConfigId]
		,[ParentFirstPageDocType]
		,[ChildFirstPageDocType]
		,[Active]
	FROM [dbo].[MTS.IDC_PARENT_CHILD_MERGE_CONFIG]
	WHERE ConfigId = @INPUTCONFIGID

END





GO
