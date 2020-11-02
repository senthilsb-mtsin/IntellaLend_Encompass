DECLARE @STATUS VARCHAR(100) = 'READY_FOR_REVIEW';
--DECLARE @STATUS VARCHAR(100) = 'READY_FOR_VALIDATION';
with eData as(
Select distinct l.LoanID, ST2.batch_instance_id, 
     substring(
        (
            Select ' | '+ST1.user_name  AS [text()]
            From dbo.hist_manual_steps_in_workflow ST1
            Where ST1.batch_instance_id = ST2.batch_instance_id AND ST1.batch_instance_status = ST2.batch_instance_status AND ST1.start_time IS NOT NULL AND ST1.end_time IS NOT NULL
            ORDER BY ST1.end_time
            For XML PATH ('')
        ), 4, 1000) AS Name, 
    substring(
        (
            Select ' | '+ CONVERT(VARCHAR(8), DATEADD (MILLISECOND, ST1.duration, '01/01/00'), 108)   AS [text()]
            From dbo.hist_manual_steps_in_workflow ST1
            Where ST1.batch_instance_id = ST2.batch_instance_id AND ST1.batch_instance_status = ST2.batch_instance_status AND ST1.start_time IS NOT NULL AND ST1.end_time IS NOT NULL
            ORDER BY ST1.end_time
            For XML PATH ('')
        ), 4, 1000) duration
From dbo.hist_manual_steps_in_workflow ST2
INNER JOIN IntellaLend_Transact.t1.Loans l with(nolock) on ST2.batch_instance_id = l.EphesoftBatchInstanceID
WHERE ST2.batch_instance_status = @STATUS 
)
update L 
set L.EphesoftReviewerName = E.Name, L.IDCReviewDuration = E.duration
FROM IntellaLend_Transact.t1.Loans L
INNER JOIN eData E ON L.LoanID = E.LoanID AND L.EphesoftBatchInstanceID = E.batch_instance_id
where E.Name is not null and E.duration is not null

--select duration, start_time, end_time, user_name from dbo.hist_manual_steps_in_workflow where batch_instance_id = 'BI31B2' AND batch_instance_status = 'READY_FOR_VALIDATION' ORDER BY end_time