DECLARE @DOCIDCOUNT BIGINT;
DECLARE @DOCIDINDEX BIGINT = 1;
DECLARE @DOCFILEDS VARCHAR(MAX) = 'Interest Rate,Borrower Address,HUDFHAMortgage (CheckBox),Loan Type,Co-Borrower Name';
DECLARE @DOCIDTABLE TABLE(ID BIGINT,DOCUMENTTYPEID BIGINT);
DECLARE @CURRTDOCID BIGINT;
DECLARE @DOCNAME = 'FHA Initial Addendum to Loan Application 92900A';

INSERT INTO @DOCIDTABLE
SELECT ROW_NUMBER() OVER (
		ORDER BY DocumentTypeID
		)
	,DocumentTypeID FROM [T1].DocumentTypeMasters WHERE Name = @DOCNAME

SELECT @DOCIDCOUNT = COUNT(1) FROM @DOCIDTABLE;

SET @DOCIDINDEX = 1;

--SELECT * FROM @DOCIDTABLE
DECLARE @DOCTABLE TABLE (		
Name VARCHAR(MAX),
DisplayName VARCHAR(MAX),
Active BIT,
CreatedOn DATETIME,
ModifiedOn DATETIME,
DocOrderByField VARCHAR(MAX),
AllowAccuracyCalc BIT
	);

	
INSERT INTO @DOCTABLE
SELECT data,data,1,GETDATE(),GETDATE(),NULL,1
FROM dbo.function_string_to_table(@DOCFILEDS, ',');

WHILE @DOCIDINDEX <= @DOCIDCOUNT
BEGIN

SELECT @CURRTDOCID = DocumentTypeID FROM @DOCIDTABLE WHERE ID =@DOCIDINDEX;

UPDATE [T1].DocumentFieldMasters SET DisplayName = 'FirstTiimeBuyer', Name = 'FirstTiimeBuyer' where Name = 'FirstTimeHomeBuyer' AND DocumentTypeID = @CURRTDOCID
UPDATE [T1].DocumentFieldMasters SET DisplayName = 'OccupancyHUD', Name = 'OccupancyHUD' where Name = 'Line252a' AND DocumentTypeID = @CURRTDOCID
UPDATE [T1].DocumentFieldMasters SET DisplayName = 'LeadPaintPoisoning', Name = 'LeadPaintPoisoning' where Name = 'Line256' AND DocumentTypeID = @CURRTDOCID
UPDATE [T1].DocumentFieldMasters SET DisplayName = 'ReasonableValueHUD', Name = 'ReasonableValueHUD' where Name = 'Line2531' AND DocumentTypeID = @CURRTDOCID
UPDATE [T1].DocumentFieldMasters SET DisplayName = 'ReasonableValueVA', Name = 'ReasonableValueVA' where Name = 'Line2532' AND DocumentTypeID = @CURRTDOCID
UPDATE [T1].DocumentFieldMasters SET DisplayName = 'HUDFHAMortgage', Name = 'HUDFHAMortgage' where Name = 'Line22a' AND DocumentTypeID = @CURRTDOCID

SET @DOCIDINDEX = @DOCIDINDEX + 1;

END


