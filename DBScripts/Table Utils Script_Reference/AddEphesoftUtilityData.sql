DECLARE @CONFIGID BIGINT = 18
DECLARE @PARENTCONFIGID BIGINT = 2
DECLARE @BATCHCLASSID VARCHAR(20) = 'BC65'


--INSERT INTO [MTS.IDC_RULE_CONFIG]
--SELECT @CONFIGID, @BATCHCLASSID

--INSERT INTO [MTS.DOCUMENT_STACKING_ORDER]
--SELECT @CONFIGID,
--DocumentName,
--SequenceNumber,
--GETDATE(),
--GETDATE() FROM [MTS.DOCUMENT_STACKING_ORDER] WHERE CONFIGID = @PARENTCONFIGID

--INSERT INTO [MTS.IDC_DOC_APPEND_BACKUP]
--SELECT @CONFIGID,
--Sequence,
--DocType1,
--DocLocation,
--DocType2 FROM [MTS.IDC_DOC_APPEND_BACKUP] WHERE CONFIGID = @PARENTCONFIGID

--INSERT INTO [MTS.IDC_DOC_APPEND]
--SELECT @CONFIGID,
--Sequence,
--DocType1,
--DocLocation,
--DocType2 FROM [MTS.IDC_DOC_APPEND] WHERE CONFIGID = @PARENTCONFIGID

--INSERT INTO [MTS.IDC_DOC_CONCATENATE]
--SELECT @CONFIGID,
--DocType,
--ConsecutiveFlag,
--EphesoftModuleID FROM [MTS.IDC_DOC_CONCATENATE] WHERE CONFIGID = @PARENTCONFIGID

--INSERT INTO [MTS.IDC_DOC_CONFIG]
--SELECT @CONFIGID,
--AdvancedOptions,
--Flag FROM [MTS.IDC_DOC_CONFIG] WHERE CONFIGID = @PARENTCONFIGID

--INSERT INTO [MTS.IDC_DOC_CONVERSION]
--SELECT @CONFIGID,
--DocType,
--ToDocType,
--CONVERSION_RULE,
--EphesoftModuleID FROM [MTS.IDC_DOC_CONVERSION] WHERE CONFIGID = @PARENTCONFIGID

--INSERT INTO [MTS.IDC_PARENT_CHILD_MERGE_CONFIG]
--SELECT @CONFIGID,
--ParentFirstPageDocType,
--ChildFirstPageDocType,
--Active FROM [MTS.IDC_PARENT_CHILD_MERGE_CONFIG] WHERE CONFIGID = @PARENTCONFIGID