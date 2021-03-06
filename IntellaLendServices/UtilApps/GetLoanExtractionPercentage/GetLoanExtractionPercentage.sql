DECLARE @BATCHCLASS VARCHAR(5) = '{BATCHCLASSID}'
	,@BATCHINSTANCEID VARCHAR(100) = '{BATCHINSTANCEID}';

BEGIN
	DECLARE @TEMP TABLE (
		BATCH_INSTANCEID VARCHAR(50)
		,DOCID VARCHAR(50)
		,DOCTYPE VARCHAR(50)
		,FIELDNAME VARCHAR(50)
		,FIELDVALUE_P1 VARCHAR(255)
		,FIELDVALUE_P2 VARCHAR(255)
		);

	WITH MAINSELECT
	AS (
		SELECT E.BATCH_INSTANCEID
			,E.DOCID
			,E.DOCTYPE AS 'EDOCTYPE'
			,M.DOCTYPE AS 'MDOCTYPE'
			,E.FIELDNAME
			,E.FIELDVALUE AS 'EFIELDVALUE'
			,M.FIELDVALUE AS 'MFIELDVALUE'
			,M.FIELDNAME AS 'MFIELDNAME'
			,E.FIELDNAME AS 'EFIELDNAME'
		FROM FIELDS E WITH (NOLOCK)
		INNER JOIN FIELDS M ON E.BATCH_INSTANCEID = M.BATCH_INSTANCEID
			AND E.DOCID = M.DOCID
		--AND E.DOCTYPE = M.DOCTYPE
		--AND E.FIELDNAME = M.FIELDNAME
		INNER JOIN BATCHES B ON E.BATCH_INSTANCEID = @BATCHINSTANCEID
			AND BATCHCLASS_ID = @BATCHCLASS
			AND STATUS = 'FINISHED'
		INNER JOIN IntellaLend_IDC_Service.dbo.MTS_AUTO_VALIDATION_SKIP IDC ON IDC.DOCUMENT_NAME = E.DOCTYPE
		--WHERE B.LASTMODIFIEDDATE BETWEEN @STARTDATE
		--		AND @ENDDATE
		--	AND E.PATTERN LIKE '%' + @STARTPATTERN + '%'
		--	AND M.PATTERN LIKE '%' + @ENDPATTERN + '%'
		)
		,ALL_DOCS
	AS (
		SELECT BATCH_INSTANCEID
			,DOCID
			,EDOCTYPE AS 'DOCTYPE'
			,FIELDNAME
			,EFIELDVALUE AS 'VALUE_1'
			,MFIELDVALUE AS 'VALUE_2'
		FROM MAINSELECT
		WHERE EDOCTYPE = MDOCTYPE
			AND EFIELDNAME = MFIELDNAME
		)
		,LOAN_APP
	AS (
		SELECT BATCH_INSTANCEID
			,DOCID
			,EDOCTYPE AS 'DOCTYPE'
			,FIELDNAME
			,EFIELDVALUE AS 'VALUE_1'
			,MFIELDVALUE AS 'VALUE_2'
		FROM MAINSELECT
		WHERE EFIELDNAME = MFIELDNAME
			AND MDOCTYPE = 'Loan Application 1003'
			AND (
				EDOCTYPE = 'Loan Application 1003 Format 1'
				OR EDOCTYPE = 'Loan Application 1003 Format 2'
				)
		)
	INSERT INTO @TEMP
	SELECT BATCH_INSTANCEID
		,DOCID
		,DOCTYPE
		,FIELDNAME
		,VALUE_1
		,VALUE_2
	FROM ALL_DOCS
	
	UNION
	
	SELECT BATCH_INSTANCEID
		,DOCID
		,DOCTYPE
		,FIELDNAME
		,VALUE_1
		,VALUE_2
	FROM LOAN_APP;

	DECLARE @DIST_TEMP TABLE (
		BATCH_INSTANCEID VARCHAR(100)
		,DOCTYPE VARCHAR(MAX)
		--,FIELDNAME VARCHAR(MAX)
		,TOTAL BIGINT
		);
	DECLARE @CORRECT_TEMP TABLE (
		BATCH_INSTANCEID VARCHAR(100)
		,DOCTYPE VARCHAR(MAX)
		--,FIELDNAME VARCHAR(MAX)
		,CORRECT BIGINT
		);

	INSERT INTO @DIST_TEMP
	SELECT BATCH_INSTANCEID
		,DOCTYPE
		--,FIELDNAME
		,COUNT(1) TOTAL
	FROM @TEMP T
	GROUP BY T.BATCH_INSTANCEID
		,T.DOCTYPE
		--,T.FIELDNAME

	INSERT INTO @CORRECT_TEMP
	SELECT T.BATCH_INSTANCEID
		,T.DOCTYPE
		--,T.FIELDNAME
		,COUNT(1) CORRECT
	FROM @TEMP T
	WHERE T.FIELDVALUE_P1 = T.FIELDVALUE_P2
	GROUP BY T.BATCH_INSTANCEID
		,T.DOCTYPE
		--,T.FIELDNAME

	SELECT T1.BATCH_INSTANCEID
		--,T1.DOCTYPE
		--,T1.FIELDNAME
		,SUM(T1.TOTAL) AS TOTAL
		,SUM(T3.CORRECT) AS CORRECT
		--,ISNULL(STR((T3.CORRECT * 100.0 / T1.TOTAL), 5, 2), 0.00) + '%' AS 'CORRECT%'
		,ISNULL(STR((SUM(T3.CORRECT) * 100.0 / SUM(T1.TOTAL)), 5, 2), 0.00) AS DOCUMENT_PERCENTAGE
	FROM @DIST_TEMP T1
	LEFT JOIN @CORRECT_TEMP T3 ON  T1.BATCH_INSTANCEID = T3.BATCH_INSTANCEID 
		AND T1.DOCTYPE = T3.DOCTYPE
		--AND T1.FIELDNAME = T3.FIELDNAME
	GROUP BY T1.BATCH_INSTANCEID
	ORDER BY 1

	SELECT T1.BATCH_INSTANCEID
		,T1.DOCTYPE
		--,T1.FIELDNAME
		,T1.TOTAL
		,T3.CORRECT
		--,ISNULL(STR((T3.CORRECT * 100.0 / T1.TOTAL), 5, 2), 0.00) + '%' AS 'CORRECT%'
		,ISNULL(STR((T3.CORRECT * 100.0 / T1.TOTAL), 5, 2), 0.00) AS DOCUMENT_PERCENTAGE
	FROM @DIST_TEMP T1
	LEFT JOIN @CORRECT_TEMP T3 ON  T1.BATCH_INSTANCEID = T3.BATCH_INSTANCEID 
		AND T1.DOCTYPE = T3.DOCTYPE
		--AND T1.FIELDNAME = T3.FIELDNAME
	ORDER BY 1
		,2
END
