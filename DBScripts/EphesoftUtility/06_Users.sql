/****** Object:  User [mts]    Script Date: 2/28/2020 9:24:03 AM ******/
CREATE USER [mts] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[mts]
GO
/****** Object:  User [mtsadmin]    Script Date: 2/28/2020 9:24:03 AM ******/
CREATE USER [mtsadmin] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [mts]
GO
ALTER ROLE [db_owner] ADD MEMBER [mtsadmin]
GO
