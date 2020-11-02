DECLARE @parentChecklistID BIGINT = 1;
DECLARE @ChecklistDetailTable TABLE (
	RowID BIGINT
	,CheckListDetailID BIGINT
	,CheckListID BIGINT
	,Description NVARCHAR(max)
	,Active BIT
	,Name VARCHAR(max)
	,CreatedOn DATETIME
	,ModifiedOn DATETIME
	,UserID BIGINT
	,Rule_Type INT
	,SequenceID BIGINT
	,Category VARCHAR(max)
	)
DECLARE @RuleMasterTable TABLE (
	RowID BIGINT
	,RuleID BIGINT
	,CheckListDetailID BIGINT
	,RuleDescription NVARCHAR(max)
	,Active BIT
	,RuleJson NVARCHAR(max)
	,DocumentType NVARCHAR(max)
	,CreatedOn DATETIME
	,ModifiedOn DATETIME
	,ActiveDocumentType NVARCHAR(max)
	,DocVersion VARCHAR(max)
	)
DECLARE @childChecklistID BIGINT = 6

INSERT INTO @ChecklistDetailTable
SELECT ROW_NUMBER() OVER (
		ORDER BY CheckListDetailID
		)
	,cd.*
FROM [il].CheckListDetailMasters cd
WHERE CheckListID = @parentChecklistID

INSERT INTO @RuleMasterTable
SELECT ROW_NUMBER() OVER (
		ORDER BY RuleID
		)
	,r.*
FROM [il].CheckListDetailMasters cd
INNER JOIN [il].RuleMasters r ON cd.CheckListDetailID = r.CheckListDetailID
WHERE CheckListID = @parentChecklistID

DECLARE @checklistCount BIGINT;
DECLARE @RowIncrementer BIGINT = 1;
DECLARE @curChecklistDetailID BIGINT;

SELECT @checklistCount = count(1)
FROM @ChecklistDetailTable;

UPDATE [IL].CheckListDetailMasters
SET SequenceID = 0
WHERE CheckListID = @childChecklistID

WHILE @RowIncrementer <= @checklistCount
BEGIN
	SET @curChecklistDetailID = 0;

	INSERT INTO [il].CheckListDetailMasters
	SELECT @childChecklistID
		,Description
		,Active
		,Name
		,CreatedOn
		,ModifiedOn
		,UserID
		,Rule_Type
		,SequenceID
		,Category
	FROM @ChecklistDetailTable
	WHERE RowID = @RowIncrementer;

	SELECT @curChecklistDetailID = max(CheckListDetailID)
	FROM [il].CheckListDetailMasters

	INSERT INTO [il].RuleMasters
	SELECT @curChecklistDetailID
		,RuleDescription
		,Active
		,RuleJson
		,DocumentType
		,CreatedOn
		,ModifiedOn
		,ActiveDocumentType
		,DocVersion
	FROM @RuleMasterTable
	WHERE RowID = @RowIncrementer;

	SET @RowIncrementer = @RowIncrementer + 1;
END

DECLARE @maxSequenceID BIGINT = 0;

SELECT @maxSequenceID = Max(SequenceID)
FROM [IL].CheckListDetailMasters
WHERE CheckListID = @childChecklistID
	AND SequenceID <> 0

SET @maxSequenceID = @maxSequenceID + 1;

WITH UpdateData
AS (
	SELECT CheckListDetailID
		,@maxSequenceID - 1 + row_number() OVER (
			ORDER BY (
					SELECT NULL
					)
			) AS RN
	FROM [IL].CheckListDetailMasters
	WHERE CheckListID = @childChecklistID
		AND SequenceID = 0
	)
UPDATE cd
SET SequenceID = RN
FROM [IL].CheckListDetailMasters cd
INNER JOIN UpdateData ON cd.CheckListDetailID = UpdateData.CheckListDetailID
WHERE CheckListID = @childChecklistID
	AND SequenceID = 0
