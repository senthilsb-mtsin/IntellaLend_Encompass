/****** Object:  UserDefinedFunction [dbo].[function_string_to_table]    Script Date: 11/13/2017 03:55:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[function_string_to_table] (
	@string VARCHAR(MAX)
	,@delimiter CHAR(1)
	)
RETURNS @output TABLE (data NVARCHAR(500))

BEGIN
	DECLARE @start INT
		,@end INT

	SELECT @start = 1
		,@end = CHARINDEX(@delimiter, @string)

	WHILE @start < LEN(@string) + 1
	BEGIN
		IF @end = 0
			SET @end = LEN(@string) + 1

		INSERT INTO @output (data)
		VALUES (SUBSTRING(@string, @start, @end - @start))

		SET @start = @end + 1
		SET @end = CHARINDEX(@delimiter, @string, @start)
	END

	RETURN
END



GO
