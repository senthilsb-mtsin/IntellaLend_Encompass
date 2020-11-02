DECLARE @bcID BIGINT = 9;

--Ephesoft to IL Document Fields
WITH ilDocFields
AS (
	SELECT ROW_NUMBER() OVER (
			ORDER BY (
					SELECT 0
					)
			) AS Row_Num
		,D.DocumentTypeID
		,D.Name AS DocName
		,df.Name AS FieldName
	FROM il.DocumentTypeMasters d
	INNER JOIN il.DocumentFieldMasters df ON d.DocumentTypeID = df.DocumentTypeID
	)
	,epDocFields
AS (
	SELECT ROW_NUMBER() OVER (
			ORDER BY (
					SELECT 0
					)
			) AS Row_Num
		,D.document_type_name
		,F.field_type_name
	FROM Ephesoft.DBO.batch_class_document_type BD
	INNER JOIN Ephesoft.DBO.document_type D ON BD.document_type_id = D.id
	INNER JOIN Ephesoft.DBO.field_type F ON BD.document_type_id = F.document_type_id
	WHERE BD.batch_class_id = @bcID
	)
	,commonFields
AS (
	SELECT ed.*
		,il.DocumentTypeID
	FROM epDocFields ed
	INNER JOIN ilDocFields il ON ed.document_type_name = il.DocName
		AND ed.field_type_name = il.FieldName
	)
	,missFields
AS (
	SELECT e.*
	FROM epDocFields e
	WHERE Row_Num NOT IN (
			SELECT ROW_NUM
			FROM commonFields c
			)
	)
INSERT INTO il.DocumentFieldMasters (
	DocumentTypeID
	,Name
	,DisplayName
	,CreatedOn
	,ModifiedOn
	,Active
	,DocOrderByField
	,AllowAccuracyCalc
	,IsDocName
	)
SELECT df.DocumentTypeID
	,m.field_type_name
	,m.field_type_name
	,getdate()
	,getdate()
	,1
	,NULL
	,0
	,0
FROM missFields m
INNER JOIN il.DocumentTypeMasters df ON m.document_type_name = df.Name;

--IL Document Fields to T1 Document Fields
WITH ilDocFields
AS (
	SELECT ROW_NUMBER() OVER (
			ORDER BY (
					SELECT 0
					)
			) AS Row_Num
		,D.DocumentTypeID
		,D.Name AS DocName
		,df.Name AS FieldName
	FROM T1.DocumentTypeMasters d
	INNER JOIN T1.DocumentFieldMasters df ON d.DocumentTypeID = df.DocumentTypeID
	)
	,epDocFields
AS (
	SELECT ROW_NUMBER() OVER (
			ORDER BY (
					SELECT 0
					)
			) AS Row_Num
		,D.Name AS document_type_name
		,df.Name AS field_type_name
	FROM IL.DocumentTypeMasters d
	INNER JOIN IL.DocumentFieldMasters df ON d.DocumentTypeID = df.DocumentTypeID
	)
	,commonFields
AS (
	SELECT ed.*
		,t1.DocumentTypeID
	FROM epDocFields ed
	INNER JOIN ilDocFields t1 ON ed.document_type_name = t1.DocName
		AND ed.field_type_name = t1.FieldName
	)
	,missFields
AS (
	SELECT e.*
	FROM epDocFields e
	WHERE Row_Num NOT IN (
			SELECT ROW_NUM
			FROM commonFields c
			)
	)
INSERT INTO t1.DocumentFieldMasters (
	DocumentTypeID
	,Name
	,DisplayName
	,Active
	,CreatedOn
	,ModifiedOn
	,DocOrderByField
	,AllowAccuracyCalc
	,IsDocName
	)
SELECT d.DocumentTypeID
	,m.field_type_name
	,m.field_type_name
	,1
	,getdate()
	,getdate()
	,NULL
	,0
	,0
FROM missFields m
INNER JOIN t1.DocumentTypeMasters d ON m.document_type_name = d.Name
ORDER BY 1
