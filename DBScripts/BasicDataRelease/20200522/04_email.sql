Declare @id int
INSERT INTO [IL].[EmailTemplate]
           ([TEMPLATENAME]
           ,[XMLTEMPLATE]
           ,[SMTPID]
           ,[ACTIVE]
           ,[HTMLPAGE])
     VALUES
           ('SERVICECONTROLLER_NOTIFICATION'
           ,'<email />'
           ,1
           ,1
           ,'ServiceControllerEmail')

SET @id=SCOPE_IDENTITY()
---------------

INSERT INTO [IL].[EmailSchedule]
           ([TEMPLATEID]
           ,[SCHEDULEDESCRIPTION]
           ,[SENDBY]
           ,[DAY]
           ,[TIME])
     VALUES
           (@id
           ,'ServiceController Notification'
           ,1
           ,''
           ,NULL)
GO

-----------------------