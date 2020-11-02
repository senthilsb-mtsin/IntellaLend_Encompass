BEGIN
DECLARE @DOCID BIGINT;
SELECT @DOCID = DocumentTypeID FROM [IL].DocumentTypeMasters where Name = 'Transmittal Summary Final'
 

INSERT INTO [IL].DocumentFieldMasters
SELECT @DOCID, 'Lease/Ground Rent', 'Lease/Ground Rent', GETDATE(), GETDATE(), 1, NULL, 1
UNION ALL
SELECT @DOCID, 'Other Housing Payments', 'Other Housing Payments', GETDATE(), GETDATE(), 1, NULL, 1

DECLARE @TEMPTABLE TABLE (ID BIGINT, DocumentTypeID BIGINT);
INSERT INTO @TEMPTABLE
SELECT ROW_NUMBER() OVER(ORDER BY DocumentTypeID ) ID , DocumentTypeID FROM [T1].DocumentTypeMasters where Name = 'Transmittal Summary Final'

DECLARE @ROWCOUNT BIGINT
DECLARE @COUNT BIGINT = 1;

SELECT @ROWCOUNT = COUNT(1) FROM @TEMPTABLE;
DECLARE @ROWID BIGINT;

WHILE @COUNT <= @ROWCOUNT
BEGIN

SELECT @ROWID = DOCUMENTTYPEID FROM @TEMPTABLE WHERE ID = @COUNT

INSERT INTO [T1].DocumentFieldMasters
SELECT @ROWID, 'Lease/Ground Rent', 'Lease/Ground Rent', 1, GETDATE(), GETDATE(), NULL,1
UNION ALL
SELECT @ROWID, 'Other Housing Payments', 'Other Housing Payments', 1, GETDATE(), GETDATE(), NULL,1


SET @COUNT = @COUNT + 1;
END

END
