DECLARE @T1_CustomerID BIGINT = 41,
	@IL_CustomerID BIGINT = 1,
	@LoanTypeID BIGINT = 1,
	@T1_StackingOrderID = 203,
	@IL_StackingOrderID = 25;

WITH T1_STACK
AS (
	SELECT s.StackingOrderDetailID AS T1_StackID
		,d.Name
		,s.SequenceID AS T1_SequenceID
	FROM [t1].DocumentTypeMasters d
	INNER JOIN [t1].CustLoanDocMapping c ON d.DocumentTypeID = c.DocumentTypeID
		AND CustomerID = @T1_CustomerID
		AND LoanTypeID = @LoanTypeID
	INNER JOIN [t1].StackingOrderDetailMasters s ON c.DocumentTypeID = s.DocumentTypeID
		AND StackingOrderID = @T1_StackingOrderID
		--order by s.SequenceID
	)
	,IL_STACK
AS (
	SELECT s.StackingOrderDetailID AS IL_StackID
		,d.Name
		,s.SequenceID AS IL_SequenceID
	FROM [il].DocumentTypeMasters d
	INNER JOIN [il].CustLoanDocMapping c ON d.DocumentTypeID = c.DocumentTypeID
		AND CustomerID = @IL_CustomerID
		AND LoanTypeID = @LoanTypeID
	INNER JOIN [il].StackingOrderDetailMasters s ON c.DocumentTypeID = s.DocumentTypeID
		AND StackingOrderID = @IL_StackingOrderID
		--order by s.SequenceID
	)
	,ARRSTACK
AS (
	SELECT T1_StackID
		,I.Name AS IL_NAME
		,I.Name AS T_NAME
		,T1_SequenceID
		,IL_SequenceID
	FROM T1_STACK T
	INNER JOIN IL_STACK I ON T.Name = I.Name
	)
UPDATE S
SET S.SequenceID = A.IL_SequenceID
FROM [T1].StackingOrderDetailMasters S
INNER JOIN ARRSTACK A ON A.T1_StackID = S.StackingOrderDetailID
