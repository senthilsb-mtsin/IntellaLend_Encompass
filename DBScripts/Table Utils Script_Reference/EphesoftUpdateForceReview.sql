update field_type set operator_review_reqd =0 where id in (
select --d.document_type_name, f.*
f.id
from batch_class_document_type bd
inner join document_type d on bd.document_type_id = d.id
inner join field_type f on bd.document_type_id = f.document_type_id
where bd.batch_class_id = 11 and f.operator_review_reqd = 1 
--order by 1, 2
)




--order by 1, f.field_type_name

--select * from field_training