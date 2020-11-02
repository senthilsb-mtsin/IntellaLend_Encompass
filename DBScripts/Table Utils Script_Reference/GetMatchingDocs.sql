declare @custreviewloantable table (
ID BIGINT,
CUSTOMERID BIGINT,
REVIEWTYPEID BIGINT,
LOANTYPEID BIGINT
)

INSERT INTO @custreviewloantable
SELECT ROW_NUMBER() OVER(ORDER BY CustomerID), CustomerID, ReviewTypeID, LoanTypeID from [t1].CustReviewLoanMapping where LoanTypeID = 1

declare @incr bigint = 1;
declare @tablecount bigint;

select @tablecount = count(1) from @custreviewloantable

declare @TEMPCUSTID bigint;
declare @TEMPREVIEWID bigint;
declare @TEMPLOANTYPEID bigint;

DECLARE @TEMPSTACKTABLE TABLE(DOCID BIGINT);
DECLARE @TEMPLOANDOCTABLE TABLE(DOCID BIGINT);


DECLARE @TEMPMAPPINGCOUNT TABLE(
CUSTOMERID NVARCHAR(MAX),
REVIEWTYPEID NVARCHAR(MAX),
LOANTYPEID NVARCHAR(MAX),
CUSTLOANCOUNT BIGINT,
CUSTSTACKCOUNT BIGINT,
MATCHING BIGINT);

WHILE @incr < @tablecount
BEGIN

DELETE FROM @TEMPSTACKTABLE;
DELETE FROM @TEMPLOANDOCTABLE;

DECLARE @TEMPSTACKCOUNT BIGINT;
DECLARE @TEMPLOANDOCCOUNT BIGINT;

DECLARE @TEMPCUSTNAME NVARCHAR(MAX);
DECLARE @TEMPREVIEWNAME NVARCHAR(MAX);
DECLARE @TEMPLOANNAME NVARCHAR(MAX);

SELECT @TEMPCUSTID = CUSTOMERID, @TEMPREVIEWID = REVIEWTYPEID, @TEMPLOANTYPEID = LOANTYPEID FROM @custreviewloantable WHERE ID = @incr

SELECT @TEMPCUSTNAME = CustomerName FROM [T1].CustomerMasters WHERE CustomerID = @TEMPCUSTID
SELECT @TEMPREVIEWNAME = ReviewTypeName FROM [T1].ReviewTypeMasters WHERE ReviewTypeID = @TEMPREVIEWID
SELECT @TEMPLOANNAME = LoanTypeName FROM [T1].LoanTypeMasters WHERE LoanTypeID = @TEMPLOANTYPEID

INSERT INTO @TEMPSTACKTABLE
SELECT S.DocumentTypeID FROM [T1].CustReviewLoanStackMapping C
INNER JOIN [T1].StackingOrderDetailMasters S ON C.StackingOrderID = S.StackingOrderID AND C.CustomerID = @TEMPCUSTID AND C.ReviewTypeID = @TEMPREVIEWID AND C.LoanTypeID = @TEMPLOANTYPEID

INSERT INTO @TEMPLOANDOCTABLE
SELECT C.DocumentTypeID FROM [T1].CustLoanDocMapping C WHERE C.CustomerID = @TEMPCUSTID AND C.LoanTypeID = @TEMPLOANTYPEID

SELECT @TEMPSTACKCOUNT = COUNT(DOCID) FROM @TEMPSTACKTABLE
SELECT @TEMPLOANDOCCOUNT = COUNT(DOCID) FROM @TEMPLOANDOCTABLE

INSERT INTO @TEMPMAPPINGCOUNT
SELECT @TEMPCUSTNAME AS CustomerName, @TEMPREVIEWNAME as ServiceTypeName, @TEMPLOANNAME as LoanTypeName, @TEMPLOANDOCCOUNT as CustLoanDocCount, @TEMPSTACKCOUNT as CustServiceLoanDocCount, COUNT(TS.DOCID) as MatchingCount
FROM @TEMPSTACKTABLE TS 
INNER JOIN @TEMPLOANDOCTABLE TL ON TS.DOCID = TL.DOCID


SET @incr = @incr + 1;

END

SELECT * FROM @TEMPMAPPINGCOUNT