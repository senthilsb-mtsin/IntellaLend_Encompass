WITH PDFNotGenerated
AS (
	SELECT DISTINCT Loanid
	FROM [t1].LoanDetails
	WHERE LoanID NOT IN (
			SELECT Loanid
			FROM [t1].loanpdf
			)
	)
SELECT b.LoanID
	,b.BoxEntityID
	,b.UserID
FROM [t1].BoxDownloadQueue b
INNER JOIN PDFNotGenerated p ON b.LoanID = p.LoanID
ORDER BY b.LoanID
