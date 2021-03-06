/****** Object:  StoredProcedure [IL].[Cus_Notification_ServiceController]    Script Date: 3/9/2020 5:13:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [IL].[Cus_Notification_ServiceController]
	(@EMAILSP varchar(400))
AS
BEGIN
	INSERT INTO [IL].[EmailMaster] (
		[TEMPLATEID]
		,[EMAILSP]
		,[REQUESTTIME]
		,[STATUS]
		)
		values(8,@EMAILSP,GETDATE(),0)
	
END
GO
/****** Object:  StoredProcedure [IL].[UPDATETIMERCOUNT]    Script Date: 3/9/2020 5:13:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [IL].[UPDATETIMERCOUNT] (@ConfigValue Datetime )
AS
BEGIN
	
	if exists (select 1 from il.appconfig where ConfigKey='SERVICE_CONTROLLER_LAST_ACCESS_TIME')
		begin
			UPDATE [IL].AppConfig WITH (ROWLOCK)
			SET ConfigValue =CONVERT(VARCHAR(20), @ConfigValue,120) WHERE ConfigKey='SERVICE_CONTROLLER_LAST_ACCESS_TIME';
		end
	else
		begin
			insert into il.AppConfig (ConfigKey,ConfigValue) values ('SERVICE_CONTROLLER_LAST_ACCESS_TIME',CONVERT(VARCHAR(20), GETDATE(),120))
		end
END;
GO

--------------------------------

CREATE PROCEDURE [IL].[GETSERVICECONTROLLERACCESSTIME]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from IL.Appconfig where configKey='SERVICE_CONTROLLER_LAST_ACCESS_TIME'
END
GO

