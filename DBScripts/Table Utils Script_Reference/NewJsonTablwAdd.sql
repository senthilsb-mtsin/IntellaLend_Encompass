DECLARE @DOCTABLE TABLE (
	DocumentTypeName NVARCHAR(max)
	,TableName NVARCHAR(max)
	,TableJson NVARCHAR(max)
	);
DECLARE @DISTDOCS TABLE (
	DocumentTypeName NVARCHAR(max)
	,ID BIGINT
	);
DECLARE @DISTDOCCOUNT BIGINT = 0;
DECLARE @DISTDOCINDEX BIGINT = 1;
DECLARE @DOCID BIGINT = 0;
DECLARE @DOCNAME NVARCHAR(MAX) = '';
DECLARE @DOCIDS TABLE (
	DocumentTypeID BIGINT
	,ID BIGINT
	);
DECLARE @DOCIDSCOUNT BIGINT = 0;
DECLARE @DOCIDSINDEX BIGINT = 1;

INSERT INTO @DOCTABLE
SELECT D.Name
	,DT.TableName
	,DT.TABLEJSON
FROM [IL].DocumetTypeTables DT
INNER JOIN [IL].DocumentTypeMasters D ON D.DocumentTypeID = DT.DocumentTypeID
ORDER BY 1;

WITH DIST_DOC
AS (
	SELECT DISTINCT DocumentTypeName
	FROM @DOCTABLE
	)
INSERT INTO @DISTDOCS
SELECT DocumentTypeName
	,ROW_NUMBER() OVER (
		ORDER BY DocumentTypeName
		)
FROM DIST_DOC

SELECT @DISTDOCCOUNT = COUNT(1)
FROM @DISTDOCS

WHILE @DISTDOCINDEX <= @DISTDOCCOUNT
BEGIN
	SELECT @DOCNAME = DocumentTypeName
	FROM @DISTDOCS
	WHERE ID = @DISTDOCINDEX;
	
	DELETE FROM @DOCIDS

	INSERT INTO @DOCIDS
	SELECT DocumentTypeID
		,ROW_NUMBER() OVER (
			ORDER BY DocumentTypeID
			)
	FROM [T1].DocumentTypeMasters
	WHERE Name = @DOCNAME

	SELECT @DOCIDSCOUNT = COUNT(1)
	FROM @DOCIDS;

	SET @DOCIDSINDEX = 1;

	WHILE @DOCIDSINDEX <= @DOCIDSCOUNT
	BEGIN
		SELECT @DOCID = DocumentTypeID
		FROM @DOCIDS
		WHERE ID = @DOCIDSINDEX

		IF @DOCID > 0
		BEGIN
			INSERT INTO [T1].DocumetTypeTables
			SELECT @DOCID
				,TableName
				,TableJson
				,GETDATE()
				,GETDATE()
			FROM @DOCTABLE
			WHERE DocumentTypeName = @DOCNAME
		END
		
		SET @DOCIDSINDEX  = @DOCIDSINDEX + 1;

		SET @DOCID = 0;
	END

	SET @DISTDOCINDEX = @DISTDOCINDEX + 1;
END
