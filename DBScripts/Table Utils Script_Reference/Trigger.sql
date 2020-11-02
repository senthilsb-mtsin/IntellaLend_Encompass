
CREATE TRIGGER TR_T1_LOANS_FORINSERT 
ON [T1].LOANS
FOR INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LOANTYPEID INT
    SELECT  @LOANTYPEID = LOANTYPEID FROM INSERTED 
	IF(@LOANTYPEID=0)
	BEGIN
	  UPDATE [T1].LOANS
      SET LOANTYPEID = 2
      FROM INSERTED
      WHERE [T1].LOANS.LOANID = INSERTED.LOANID; 
	END
END