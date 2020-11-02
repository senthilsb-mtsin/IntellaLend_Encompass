
with epBatchs as (
SELECT batch_instance_id, end_time, start_time, batch_instance_status, user_name + ' | ' as username , 
convert(char(8),dateadd(s,datediff(s,start_time,end_time),'1900-1-1'),8) + ' | ' as duractionCAl
FROM Report.dbo.hist_manual_steps_in_workflow where
(batch_instance_status = 'READY_FOR_REVIEW'  or batch_instance_status = 'READY_FOR_VALIDATION') and
batch_instance_id  in
(SELECT BATCH_INSTANCEID FROM IntellaLend_Reporting.dbo.BATCHES where BATCHCLASS_ID = 'BC5')
)
--select * from epBatchs where batch_instance_status = 'READY_FOR_VALIDATION' and end_time is not null
--group by batch_instance_id order by 2 desc
update i set i.IDCLevelTwoCompletionDate = e.end_time , i.IDCLevelTwoDuration = e.duractionCAl,  i.IDCValidatorName = username
from  T1.IDCFields i inner join epBatchs e on  i.IDCBatchInstanceID = e.batch_instance_id 
where batch_instance_status = 'READY_FOR_VALIDATION' and end_time is not null 