with batchesCount as (
select distinct b.BATCH_INSTANCEID, p.PATTERN From BATCHES b with(nolock) 
inner join pages p with(nolock) on b.BATCH_INSTANCEID = p.BATCH_INSTANCEID
where b.STATUS = 'FINISHED' and ( p.PATTERN like '%Export%' or p.PATTERN like '%Document_Assembly%' or p.PATTERN like '%Automated_Validation%')
)
select BATCH_INSTANCEID, count(PATTERN) from batchesCount
group by BATCH_INSTANCEID order by 2