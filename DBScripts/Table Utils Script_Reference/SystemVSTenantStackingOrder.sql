WITH SystemStackingOrder
AS (
	SELECT c.ReviewTypeID
		,R.ReviewTypeName
		,C.LoanTypeID
		,l.LoanTypeName
		,COUNT(S.StackingOrderID) TotalStackCount
	FROM [IL].CustReviewLoanStackMapping C
	INNER JOIN [IL].StackingOrderDetailMasters S ON C.StackingOrderID = S.StackingOrderID
	INNER JOIN [IL].ReviewTypeMasters R ON C.ReviewTypeID = R.ReviewTypeID
	INNER JOIN [IL].LoanTypeMasters L ON C.LoanTypeID = L.LoanTypeID
	GROUP BY c.ReviewTypeID
		,r.ReviewTypeName
		,C.LoanTypeID
		,l.LoanTypeName
	)
	,TenantStackingOder
AS (
	SELECT C.CustomerID
		,Cus.CustomerName
		,c.ReviewTypeID
		,R.ReviewTypeName
		,C.LoanTypeID
		,l.LoanTypeName
		,COUNT(S.StackingOrderID) TotalStackCount
	FROM [T1].CustReviewLoanStackMapping C
	INNER JOIN [T1].StackingOrderDetailMasters S ON C.StackingOrderID = S.StackingOrderID
	INNER JOIN [T1].ReviewTypeMasters R ON C.ReviewTypeID = R.ReviewTypeID
	INNER JOIN [T1].LoanTypeMasters L ON C.LoanTypeID = L.LoanTypeID
	INNER JOIN [T1].CustomerMasters Cus ON C.CustomerID = Cus.CustomerID
	GROUP BY C.CustomerID
		,Cus.CustomerName
		,c.ReviewTypeID
		,r.ReviewTypeName
		,C.LoanTypeID
		,l.LoanTypeName
	)
SELECT SysS.ReviewTypeID SystemReviewTypeID
	,SysS.LoanTypeID SystemLoanTypeID
	,TentS.CustomerName TenantCustomerName
	,TentS.ReviewTypeName TenantReviewTypeName
	,TentS.LoanTypeName TenantLoanTypeName
	,SysS.TotalStackCount SystemTotalStackCount
	,TentS.TotalStackCount TenantTotalStackCount
FROM SystemStackingOrder SysS
INNER JOIN TenantStackingOder TentS ON SysS.ReviewTypeID = TentS.ReviewTypeID
	AND SysS.LoanTypeID = TentS.LoanTypeID
ORDER BY 1
	,2
