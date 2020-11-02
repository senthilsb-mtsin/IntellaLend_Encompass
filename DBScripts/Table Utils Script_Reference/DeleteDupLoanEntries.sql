WITH DupLoans
AS (
	SELECT loanid
		,count(loanid) AS DupCount
	FROM [t1].LoanDetails
	GROUP BY loanid
	HAVING count(loanid) > 1
	)
--	select ld.* from [t1].LoanDetails ld
--	inner join DupLoans d on ld.LoanID = d.LoanID
--	where ld.TotalDocCount = 0


--	,correctLoans
--AS (
--	SELECT ls.*
--	FROM [t1].loans ld
--	INNER JOIN [t1].LoanSearch ls ON ls.LoanID = ld.LoanID
--		AND ls.STATUS = ld.STATUS
--	INNER JOIN DupLoans d ON ld.LoanID = d.LoanID
--	WHERE ls.ModifiedOn IS NOT NULL
--	)
--delete ld from [t1].LoanSearch ld
--INNER JOIN DupLoans d ON ld.LoanID = d.LoanID
--WHERE ld.ID NOT IN (
--		SELECT id
--		FROM correctLoans
--		)
		
		
	,	LoanImages as (
select i.LoanID,DocumentTypeID, PageNo, Version, row_number() over(partition by i.LoanID,DocumentTypeID, PageNo, Version order by i.LoanID,DocumentTypeID, PageNo, Version) ID from [t1].LoanImages i 
inner join DupLoans d on i.LoanID = d.LoanID)
select * from LoanImages where id > 2 order by 1,2,3,4
 
	--select ld.* from [t1].LoanDetails ld
	--inner join DupLoans d on ld.LoanID = d.LoanID
	--where ld.TotalDocCount = 0
 --order by 2,3,4,5