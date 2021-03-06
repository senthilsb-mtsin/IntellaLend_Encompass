INSERT [dbo].[MTS.IDC_CONFIGURATIONS] ([Id], [KEY], [VALUE]) VALUES (1, N'SupportingDocConfidenceThreshold', N'10')
GO
INSERT [dbo].[MTS.IDC_CONFIGURATIONS] ([Id], [KEY], [VALUE]) VALUES (2, N'DLLName', N'EphesoftCustomDocProcessing.dll')
GO
INSERT [dbo].[MTS.IDC_CONFIGURATIONS] ([Id], [KEY], [VALUE]) VALUES (3, N'CustomClassName', N'EphesoftCustomDocProcessing.CustomDocProcessing')
GO
INSERT [dbo].[MTS.IDC_CONFIGURATIONS] ([Id], [KEY], [VALUE]) VALUES (4, N'MethodName', N'processDocument')
GO
INSERT [dbo].[MTS.IDC_CONFIGURATIONS] ([Id], [KEY], [VALUE]) VALUES (5, N'Client', N'IntellaLend')
GO

SET IDENTITY_INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ON 

GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (1, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'1004D-Notice of Completion', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (2, N'Post-Closing Audit', N'lborroweraddress', N'PropertyCity', N'1004D-Notice of Completion', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (3, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'1004D-Notice of Completion', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (4, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'1004D-Notice of Completion', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (5, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'1004D-Notice of Completion', N'Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (6, N'Post-Closing Audit', N'LoanMaster', N'Appraiser', N'1004D-Notice of Completion', N'Appraiser Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (7, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Borrower Acknowledgement 92700-A', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (8, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'203K Borrower Acknowledgement 92700-A', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (9, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Borrower Identify of Interest Certification', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (10, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'203K Borrower Identify of Interest Certification', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (11, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Calculator', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (12, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Contractor Bid', N'Buyer', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (13, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'203K Contractor Bid', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (14, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Homeowner Contractor Agreement', N'Owner Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (15, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Initial Draw Request', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (16, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Rehabilitation Loan Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (17, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'203K Rehabilitation Loan Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (18, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Self Help Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (19, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'92900-LT', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (20, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'92900-LT', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (21, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'92900-LT', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (22, N'Post-Closing Audit', N'lkeyloandata', N'propertysalesprice', N'92900-LT', N'Sales Price', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (23, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'92900-LT', N'Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (24, N'Post-Closing Audit', N'lkeyloandata', N'InterestRate', N'92900-LT', N'Interest Rate', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (25, N'Post-Closing Audit', N'LoanMaster', N'loantovalue', N'92900-LT', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (26, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Adjustable-Rate Home Equity Conversion Mortgage', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (27, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Adjustable-Rate Home Equity Conversion Second Mortgage', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (28, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Adjustable-Rate Note (Home Equity Conversion)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (29, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Affiliated Business Arrangement Disclosure', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (30, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Affiliated Business Arrangement Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (31, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Amendatory Escape Clause', N'Buyer', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (32, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Amendatory Escape Clause', N'File No/Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (33, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Amortization Schedule', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (34, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Amortization Schedule', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (35, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'Appraisal', N'Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (36, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Appraisal', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (37, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'Appraisal QC Review', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (38, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Appraisal QC Review', N'Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (39, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Appraisal QC Review', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (40, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Appraisal QC Review', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (41, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'Appraisal QC Review', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (42, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'Appraisal QC Review', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (43, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Assumption-Notice to Homeowners', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (44, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Attorney Infact Affidavit', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (45, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity+PropertyState+propertypostalcode', N'Attorney Infact Affidavit', N'City State Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (46, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'AUS Desktop UW Findings Report', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (47, N'Post-Closing Audit', N'LoanMaster', N'customerloantype', N'AUS Desktop UW Findings Report', N'Loan Type', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (48, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'AUS Desktop UW Findings Report', N'Total Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (49, N'Post-Closing Audit', N'lkeyloandata', N'propertysalesprice', N'AUS Desktop UW Findings Report', N'Sales Price', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (50, N'Post-Closing Audit', N'LoanMaster', N'LoanPurpose', N'AUS Desktop UW Findings Report', N'Loan Purpose', N'REFCODE_LOOKUP > RefCodeTypeId = 25', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (51, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'AUS Desktop UW Findings Report', N'Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (52, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'AUS Desktop UW Findings Report', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (53, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'AUS Desktop UW Findings Report', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (54, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'AUS Loan Prospector', N'BORROWER NAME', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (55, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'AUS Loan Prospector', N'LOAN APPLICATION NUMBER', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (56, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'AUS Loan Prospector', N'SELECTED BORROWER', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (57, N'Post-Closing Audit', N'lkeyloandata', N'InterestRate', N'AUS Loan Prospector', N'INTEREST RATE', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (58, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'AUS Loan Prospector', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (59, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'AUS Loan Prospector', N'APPRAISED VALUE OF PROPERTY', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (60, N'Post-Closing Audit', N'LoanMaster', N'loantovalue', N'AUS Loan Prospector', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (61, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'AUS Loan Prospector', N'PROPERTY ADDRESS', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (62, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Borrower Certification and Authorization', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (63, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Buydown Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (64, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Cash Flow Analysis', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (65, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Certificate of HECM Counseling', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (66, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Change of Circumstance Form', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (67, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Change of Circumstance Form', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (68, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Change of Circumstance Form', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (69, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Closing Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (70, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Closing Disclosure', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (71, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Closing Disclosure', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (72, N'Post-Closing Audit', N'lkeyloandata', N'InterestRate', N'Closing Disclosure', N'Interest Rate', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (73, N'Post-Closing Audit', N'LoanMaster', N'loanclosingdate', N'Closing Disclosure', N'Closing Date', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (74, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Closing Instructions', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (75, N'Post-Closing Audit', N'LoanMaster', N'loanclosingdate', N'Closing Instructions', N'Closing Date', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (76, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Closing Instructions', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (77, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Closing Instructions Supplemental', N'PROPERTYADDRESS', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (78, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Closing Protection Letter', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (79, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Closing Protection Letter', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (80, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Closing Protection Letter', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (81, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Compliance Ease Report', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (82, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Compliance Ease Report', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (83, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'CONDO Project Approval', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (84, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'CONDO Project Approval', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (85, N'Post-Closing Audit', N'LoanMaster', N'underwriter', N'CONDO Project Approval', N'UnderWriter Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (86, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Consumer Credit Score Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (87, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Controlled Business Arrangement Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (88, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Credit Report Lender', N'App', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (89, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Credit Report Lender', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (90, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Credit Supplements', N'Applicant', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (91, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Credit Supplements', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (92, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'Credit Supplements', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (93, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'Credit Supplements', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (94, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'Credit Supplements', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (95, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'DAP Authorization for Counseling', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (96, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'DAP Borrower Ack Home Warranty Protection Plan', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (97, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'DAP Borrower Ack Home Warranty Protection Plan', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (98, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'DAP Borrower Affidavit for Start Up', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (99, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'DAP Borrower Seller Affidavit', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (100, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Certification of Income', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (101, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Conditional Commitment', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (102, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'DAP Conditional Commitment', N'Lender Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (103, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Deed of Trust 2nd', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (104, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Funds Verification Documentation', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (105, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Income Calculator', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (106, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Mortgage Loans or Mortgage Credit Certificate', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (107, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'DAP Mortgage Loans or Mortgage Credit Certificate', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (108, N'Post-Closing Audit', N'LoanMaster', N'primaryborrowerlastname', N'DAP Mortgage Submission Voucher', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (109, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Note 2nd', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (110, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Recapture Documents', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (111, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Reservation Form', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (112, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Underwriter Certification', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (113, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Deed of Trust Rider', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (114, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Disclosure Notices', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (115, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Disclosure Notices', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (116, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'E-Signature Certificates', N'Signer Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (117, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'E-Signature Certificates', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (118, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Earnest Money Receipt', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (119, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Earnings and Income Worksheet', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (120, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Earnings and Income Worksheet', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (121, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'ECOA Notice', N'Loan Number', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (122, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Errors and Omissions or Compliance Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (123, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Escrow Holdback Agreement', N'Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (124, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Escrow Instructions', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (125, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Escrow Waiver and Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (126, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Analysis of Appraisal Report 54114', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (127, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'FHA Analysis of Appraisal Report 54114', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (128, N'Post-Closing Audit', N'LoanMaster', N'Appraiser', N'FHA Analysis of Appraisal Report 54114', N'Appraiser Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (129, N'Post-Closing Audit', N'LoanMaster', N'underwriter', N'FHA Analysis of Appraisal Report 54114', N'DE Underwriter Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (130, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'FHA Appraisal Logging', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (131, N'Post-Closing Audit', N'LoanMaster', N'Appraiser', N'FHA Appraisal Logging', N'Appraiser Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (132, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'FHA CAIVRS', N'Borrower SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (133, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'FHA Case Number Assignment', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (134, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Case Number Assignment', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (135, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'FHA Case Number Assignment', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (136, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'FHA Case Number Assignment', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (137, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'FHA Case Number Assignment', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (138, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Case Query', N'BorrowerName', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (139, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'FHA Compliance Inspection Report 92051', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (140, N'Post-Closing Audit', N'LoanMaster', N'Appraiser', N'FHA Compliance Inspection Report 92051', N'Appraiser', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (141, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'FHA Hotel and Transient Use of Property (92561)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (142, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Indentity of Interest Certification', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (143, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'FHA Initial Addendum to Loan Application 92900A', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (144, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Initial Addendum to Loan Application 92900A', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (145, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'FHA Initial Addendum to Loan Application 92900A', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (146, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Late Submission Letter', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (147, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA MIP Netting Authorization', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (148, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'FHA MIP Netting Authorization', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (149, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Mortgage Insurance Certificate', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (150, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Nearest Living Relative Information', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (151, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'FHA Nearest Living Relative Information', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (152, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'First Payment Letter', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (153, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'First Payment Letter', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (154, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Flood Determination', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (155, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Flood Determination', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (156, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Flood Determination', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (157, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Flood Hazard Disclosure', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (158, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Flood Hazard Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (159, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Flood Insurance', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (160, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Form 1098', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (161, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Form 1099R', N'PayerFullName', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (162, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Form 4506-8821', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (163, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Form 4506-8821', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (164, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Form SSA 1099', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (165, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Form SSA 1099', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (166, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Gift Document', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (167, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Gift Document', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (168, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Good Faith Estimate', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (169, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Good Faith Estimate', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (170, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Hazard Insurance', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (171, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Hazard Insurance Binder', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (172, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Hazard Insurance Binder', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (173, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HECM Amortization Schedule', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (174, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HECM Anti-Churning Disclosure (92901)', N'BorrowerName1', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (175, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HECM Financial Analysis Worksheet', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (176, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'HECM Financial Analysis Worksheet', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (177, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HECM FNMA Submission - Input Screen', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (178, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Home Counseling Disclosure', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (179, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Home Equity Conversion Mortgage Notice of Right to Cancel', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (180, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Home Equity Conversion Mortgage Notice of Right to Cancel', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (181, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'HUD-1 Settlement Statement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (182, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HUD-1 Settlement Statement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (183, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Important Notice to Homebuyers (92900B)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (184, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Informed Consumer Choice Disclosure Notice', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (185, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Initial Escrow Account Statement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (186, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Intent to Proceed', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (187, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Lender Risk Analysis Report', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (188, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Lender Risk Analysis Report', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (189, N'Post-Closing Audit', N'LoanMaster', N'LoanPurpose', N'Lender Risk Analysis Report', N'Loan Purpose', N'REFCODE_LOOKUP > RefCodeTypeId = 25', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (190, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'Lender Risk Analysis Report', N'Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (191, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Lender Risk Analysis Report', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (192, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Letters of Explanation (LOEs)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (193, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Loan Application 1003 Continuation Sheet', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (194, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 1', N'Borrower Old Employed 3 From', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 3 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (195, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue2', N'Loan Application 1003 Format 1', N'Borrower Old Employed 3 To', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 3 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (196, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 1', N'Borrower Old Employment 3 Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 3 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (197, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 1', N'Borrower Old Employed 4 From', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 4 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (198, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue2', N'Loan Application 1003 Format 1', N'Borrower Old Employed 4 To', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 4 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (199, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 1', N'Borrower Old Employment 4 Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 4 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (200, N'Post-Closing Audit', N'lkeyloandata', N'InterestRate', N'Loan Application 1003 Format 1', N'Interest Rate', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (201, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Loan Application 1003 Format 1', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (202, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Loan Application 1003 Format 1', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (203, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Loan Application 1003 Format 1', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (204, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'Loan Application 1003 Format 1', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (205, N'Post-Closing Audit', N'lentitydata', N'entityname', N'Loan Application 1003 Format 1', N'BorrowerEmployerName', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 1', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (206, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 1', N'Borrower Employed Since', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 1', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (207, N'Post-Closing Audit', N'lentitydata', N'entitypercent', N'Loan Application 1003 Format 1', N'Borrower Profession Duration', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 1', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (208, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 1', N'Borrower Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 1', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (209, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 1', N'BorrowerOldEmployed1From', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 1 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (210, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue2', N'Loan Application 1003 Format 1', N'BorrowerOldEmployed1To', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 1 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (211, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 1', N'BorrowerOldEmployment1Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 1 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (212, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 1', N'BorrowerOldEmployed2From', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 2 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (213, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue2', N'Loan Application 1003 Format 1', N'BorrowerOldEmployed2To', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 2 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (214, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 1', N'BorrowerOldEmployment2Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 2 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (215, N'Post-Closing Audit', N'LoanMaster', N'Originator', N'Loan Application 1003 Format 1', N'Loan Originator Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (216, N'Post-Closing Audit', N'lentitydata', N'entityname', N'Loan Application 1003 Format 1', N'Loan Origination Company Name', N'sourcedocument = 81 and entitytype = ''Interviewer''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (217, N'Post-Closing Audit', N'lkeyloandata', N'InterestRate', N'Loan Application 1003 Format 2', N'Interest Rate', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (218, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Loan Application 1003 Format 2', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (219, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Loan Application 1003 Format 2', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (220, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Loan Application 1003 Format 2', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (221, N'Post-Closing Audit', N'lentitydata', N'entityname', N'Loan Application 1003 Format 2', N'BorrowerEmployerName', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 1', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (222, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 2', N'Borrower Employed Since', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 1', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (223, N'Post-Closing Audit', N'lentitydata', N'entitypercent', N'Loan Application 1003 Format 2', N'Borrower Profession Duration', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 1', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (224, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 2', N'Borrower Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 1', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (225, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 2', N'BorrowerOldEmployed1From', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 1 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (226, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue2', N'Loan Application 1003 Format 2', N'BorrowerOldEmployed1To', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 1 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (227, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 2', N'BorrowerOldEmployment1Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 1 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (228, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 2', N'BorrowerOldEmployed2From', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 2 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (229, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue2', N'Loan Application 1003 Format 2', N'BorrowerOldEmployed2To', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 2 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (230, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 2', N'BorrowerOldEmployment2Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 2 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (231, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 2', N'BorrowerOldEmployed3From', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 3 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (232, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue2', N'Loan Application 1003 Format 2', N'BorrowerOldEmployed3To', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 3 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (233, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 2', N'BorrowerOldEmployment3Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 3 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (234, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue1', N'Loan Application 1003 Format 2', N'BorrowerOldEmployed4From', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 4 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (235, N'Post-Closing Audit', N'lentitydata', N'entitydatevalue2', N'Loan Application 1003 Format 2', N'BorrowerOldEmployed4To', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 4 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (236, N'Post-Closing Audit', N'lentitydata', N'entitydescription', N'Loan Application 1003 Format 2', N'BorrowerOldEmployment4Title', N'sourcedocument = 81 and entitytype = ''Employers'' and entitybitvalue = 0 and entityname = ''<%Loan Application 1003.Borrower Old Employer 4 Name%>''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (237, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'Loan Application 1003 Format 2', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (238, N'Post-Closing Audit', N'LoanMaster', N'Originator', N'Loan Application 1003 Format 2', N'Loan Originator Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (239, N'Post-Closing Audit', N'lentitydata', N'entityname', N'Loan Application 1003 Format 2', N'Loan Origination Company Name', N'sourcedocument = 81 and entitytype = ''Interviewer''', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (240, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Loan Estimate', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (241, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Lock Confirmation', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (242, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Lock Confirmation', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (243, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Lock In Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (244, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Lock In Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (245, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Mavent Review', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (246, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'MERS', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (247, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'MERS', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (248, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'MERS', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (249, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'MERS', N'Primary Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (250, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'MERS', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (251, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Mortgage Rider 2-4 Unit', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (252, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Mortgage Rider Condo', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (253, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Mortgage Rider PUD', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (254, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Name Affidavit', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (255, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Name Affidavit', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (256, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Net Tangible Worksheet', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (257, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Net Tangible Worksheet', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (258, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Net Tangible Worksheet', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (259, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'New Construction Builders Certification of Plans 92541', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (260, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'New Construction Builders Certification of Plans 92541', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (261, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'New Construction Warranty of Completion of Construction 92544', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (262, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'NMLS License Verification', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (263, N'Post-Closing Audit', N'lkeyloandata', N'InterestRate', N'Note', N'Interest Rate', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (264, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Note', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (265, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'Note', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (266, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Note', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (267, N'Post-Closing Audit', N'LoanMaster', N'underwriter', N'Notice of Loan Approval', N'Underwriter Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (268, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Notice of Loan Approval', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (269, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'Notice of Loan Approval', N'Total Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (270, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'Notice of Loan Approval', N'Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (271, N'Post-Closing Audit', N'LoanMaster', N'loantovalue', N'Notice of Loan Approval', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (272, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Notice of Right to Cancel', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (273, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Notice of Right to Cancel', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (274, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Occupancy Certification', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (275, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Occupancy Certification', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (276, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Payment Plan - Exhibit 1', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (277, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Payment Plan - Exhibit 1', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (278, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Payoff Statement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (279, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Payoff Statement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (280, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Privacy Policy Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (281, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Private Mortgage Insurance (PMI) Certificate', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (282, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Private Mortgage Insurance (PMI) Certificate', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (283, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Property Detail Report Profile AVM', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (284, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Property Detail Report Profile AVM', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (285, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'Property Detail Report Profile AVM', N'Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (286, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Property Detail Report Profile AVM', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (287, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Property Inspection Waiver (PIW)', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (288, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Property Inspection Waiver (PIW)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (289, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Property Inspection Waiver (PIW)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (290, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Purchase Agreement Addendums', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (291, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Purchase Agreement Addendums', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (292, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Purchase Agreement Addendums', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (293, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'QM Test Results', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (294, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Retirement Assets', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (295, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Right to Receive a Copy of Appraisal Disclosure', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (296, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Right to Receive a Copy of Appraisal Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (297, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Schedule of Liens - Exhibit 2', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (298, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Security Instrument', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (299, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'Security Instrument', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (300, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Security Instrument', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (301, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Security Instrument', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (302, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Servicing Transfer Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (303, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Settlement Service Providers', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (304, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'SSA 89', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (305, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'SSA 89', N'Printed Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (306, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'SSN Verification', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (307, N'Post-Closing Audit', N'LoanMaster', N'primaryborrowerlastname', N'SSN Verification', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (308, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'State Disclosures', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (309, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Statement of Credit Denial, Termination or Change', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (310, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Statement of Credit Denial, Termination or Change', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (311, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Subordination Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (312, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Subordination Agreement', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (313, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Subordination Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (314, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (315, N'Post-Closing Audit', N'LoanMaster', N'primaryborrowerlastname', N'Tax Form 1040', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (316, N'Post-Closing Audit', N'LoanMaster', N'primaryborrowerlastname', N'Tax Form 1040ES', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (317, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040EZ', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (318, N'Post-Closing Audit', N'LoanMaster', N'primaryborrowerlastname', N'Tax Form 1040EZ', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (319, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040SCH8812', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (320, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040SCHA', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (321, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040SCHB', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (322, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040SCHC', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (323, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040SCHCEZ', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (324, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040SCHE', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (325, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040SCHEIC', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (326, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040SCHF', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (327, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040SCHSE', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (328, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 1040X', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (329, N'Post-Closing Audit', N'LoanMaster', N'primaryborrowerlastname', N'Tax Form 1040X', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (330, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1065X', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (331, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'Tax Form 1065X', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (332, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'Tax Form 1065X', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (333, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'Tax Form 1065X', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (334, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120C', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (335, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'Tax Form 1120C', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (336, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'Tax Form 1120C', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (337, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'Tax Form 1120C', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (338, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120F', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (339, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'Tax Form 1120F', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (340, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'Tax Form 1120F', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (341, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'Tax Form 1120F', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (342, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120L', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (343, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'Tax Form 1120L', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (344, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'Tax Form 1120L', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (345, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'Tax Form 1120L', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (346, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120REIT', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (347, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity+PropertyState+propertypostalcode', N'Tax Form 1120REIT', N'City State and Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (348, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120S', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (349, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'Tax Form 1120SF', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (350, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'Tax Form 1120SF', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (351, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'Tax Form 1120SF', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (352, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 2106', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (353, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Form 2106EZ', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (354, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Information Sheet', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (355, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Tax Information Sheet', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (356, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Tax Returns Transcripts', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (357, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Returns Transcripts', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (358, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Termite Inspection Report', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (359, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'Termite Inspection Report', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (360, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'Termite Inspection Report', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (361, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Cash Out T50-A6 Equity Loan', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (362, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Cash Out T50-A6 Equity Loan', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (363, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Cash Out T50-A6 Right to Cancel', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (364, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'Texas Disclosure Form T-64', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (365, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Disclosure Form T-64', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (366, N'Post-Closing Audit', N'LoanMaster', N'loanclosingdate', N'Texas Disclosure Form T-64', N'Closing Date', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (367, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Home Equity Affidavit and Agreement (First Lien)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (368, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Home Equity Affidavit and Agreement (First Lien)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (369, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Home Equity Note (Fixed Rate - First Lien)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (370, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'Texas Home Equity Note (Fixed Rate - First Lien)', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (371, N'Post-Closing Audit', N'lkeyloandata', N'InterestRate', N'Texas Home Equity Note (Fixed Rate - First Lien)', N'Interest Rate', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (372, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Home Equity Security Instrument (First Lien)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (373, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Owners Affidavit of Compliance', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (374, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Owners Affidavit of Compliance', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (375, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Title Commit Prelim', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (376, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Title Policy', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (377, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Title Policy', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (378, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'Title Policy', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (379, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Total Annual Loan Cost (TALC)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (380, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'Total Annual Loan Cost (TALC)', N'Appraised Property Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (381, N'Post-Closing Audit', N'LoanMaster', N'underwriter', N'Transmittal Summary Final', N'Underwriters Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (382, N'Post-Closing Audit', N'LoanMaster', N'loantovalue', N'Transmittal Summary Final', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (383, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Transmittal Summary Final', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (384, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'Transmittal Summary Final', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (385, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'Transmittal Summary Final', N'Appraised Property Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (386, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'Transmittal Summary Final', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (387, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'UCDP Submission Summary Report (SSR) - Fannie Mae', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (388, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'UCDP Submission Summary Report (SSR) - Fannie Mae', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (389, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'UCDP Submission Summary Report (SSR) - Fannie Mae', N'Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (390, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'UCDP Submission Summary Report (SSR) - FreddieMac', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (391, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'UCDP Submission Summary Report (SSR) - FreddieMac', N'Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (392, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'UCDP Submission Summary Report (SSR) - FreddieMac', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (393, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'US Patriot Act', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (394, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'US Patriot Act', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (395, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'US Patriot Act', N'Borrower SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (396, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'USDA Certificate of Occupancy', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (397, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'USDA Certificate of Occupancy', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (398, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA Conditional Commitment 1980-18', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (399, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'USDA GUS Underwriting Findings and Analysis Report', N'Property Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (400, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA GUS Underwriting Findings and Analysis Report', N'Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (401, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA GUS Underwriting Findings and Analysis Report', N'Primary Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (402, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'USDA GUS Underwriting Findings and Analysis Report', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (403, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'USDA GUS Underwriting Findings and Analysis Report', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (404, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA GUS Underwriting Findings and Analysis Report', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (405, N'Post-Closing Audit', N'LoanMaster', N'loantovalue', N'USDA GUS Underwriting Findings and Analysis Report', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (406, N'Post-Closing Audit', N'lkeyloandata', N'propertysalesprice', N'USDA GUS Underwriting Findings and Analysis Report', N'Sales Price', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (407, N'Post-Closing Audit', N'lreportingdata', N'appraisedvalue', N'USDA GUS Underwriting Findings and Analysis Report', N'Appraised Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (408, N'Post-Closing Audit', N'LoanMaster', N'LoanPurpose', N'USDA GUS Underwriting Findings and Analysis Report', N'Loan Purpose', N'REFCODE_LOOKUP > RefCodeTypeId = 25', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (409, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'USDA GUS Underwriting Findings and Analysis Report', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (410, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA Lender Cerfification for SFH Guaranteed Loan', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (411, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'USDA Lender Cerfification for SFH Guaranteed Loan', N'Borrower SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (412, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA Loan Note Guarantee', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (413, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'USDA Property Eligibility', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (414, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA Request for SFH Loan Guarantee 3555-21', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (415, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'USDA Request for SFH Loan Guarantee 3555-21', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (416, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'USDA Request for SFH Loan Guarantee 3555-21', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (417, N'Post-Closing Audit', N'lkeyloandata', N'PropertyCity', N'USDA Request for SFH Loan Guarantee 3555-21', N'City', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (418, N'Post-Closing Audit', N'lkeyloandata', N'PropertyState', N'USDA Request for SFH Loan Guarantee 3555-21', N'State', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (419, N'Post-Closing Audit', N'lkeyloandata', N'PropertyPostalCode', N'USDA Request for SFH Loan Guarantee 3555-21', N'Zip', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (420, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA SFH Income Eligibility', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (421, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'VA Addendum to Application 26-1802A', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (422, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'VA Addendum to Application 26-1802A', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (423, N'Post-Closing Audit', N'lkeyloandata', N'InterestRate', N'VA Addendum to Application 26-1802A', N'Interest Rate', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (424, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Addendum to Application 26-1802A', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (425, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'VA Appraisal Request Form 26-1805', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (426, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Appraisal Request Form 26-1805', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (427, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Child Care Statement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (428, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Collection Policy Notice', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (429, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Common Certifications', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (430, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Common Certifications', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (431, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Counseling Checklist 26-0592', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (432, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA DD214 and Statement of Service', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (433, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'VA DD214 and Statement of Service', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (434, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Debt Questionnaire 26-0551', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (435, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Fixed Rate Mortgage Loan Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (436, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Funding Fee Notice', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (437, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA IRRRL Worksheet 26-8923', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (438, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Loan Analysis 26-6393', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (439, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Loan Analysis 26-6393', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (440, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'VA Loan Analysis 26-6393', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (441, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Loan Comparison', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (442, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Loan Comparison', N'New Loan Number', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (443, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'VA Loan Comparison', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (444, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Loan Guaranty Certificate (26-1899)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (445, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Loan Guaranty Certificate (26-1899)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (446, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Loan Summary Sheet 26-0286', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (447, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'VA Loan Summary Sheet 26-0286', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (448, N'Post-Closing Audit', N'lkeyloandata', N'LoanAmount', N'VA Loan Summary Sheet 26-0286', N'Loan Amount', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (449, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Loan Summary Sheet 26-0286', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (450, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA National Guard Reservist Certification', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (451, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Nearest Living Relative', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (452, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Nearest Living Relative', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (453, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Notice of Value', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (454, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Notice of Value', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (455, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'VA Notice of Value', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (456, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Report and Cert of Loan Disbursement 26-1820', N'Loan Number', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (457, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Report and Cert of Loan Disbursement 26-1820', N'Lender Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (458, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Report and Cert of Loan Disbursement 26-1820', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (459, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'VA Report and Cert of Loan Disbursement 26-1820', N'Borrower SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (460, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Request for COE 26-1880', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (461, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'VA Request for COE 26-1880', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (462, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Verification Benefit Related Indebtedness 26-8937', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (463, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Verification Benefit Related Indebtedness 26-8937', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (464, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'VA Verification Benefit Related Indebtedness 26-8937', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (465, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Verification of Rent and Mortgage', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (466, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Verification of Rent and Mortgage', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (467, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VVOE', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (468, N'Post-Closing Audit', N'LoanMaster', N'CustomerLoanNumber', N'VVOE', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (469, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'VVOE', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (470, N'Post-Closing Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'W9', N'BorrowerName', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (471, N'Post-Closing Audit', N'lborrowerdata', N'socialsecuritynumber', N'W9', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (472, N'Post-Closing Audit', N'lkeyloandata', N'propertystreetaddress', N'Wire Funding Detail', N'Property Address', N'sourcedocument = 81', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (473, N'Pre-Funding Audit', N'LoanMaster', N'Appraiser', N'1004D-Notice of Completion', N'Appraiser Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (474, N'Pre-Funding Audit', N'LoanMaster', N'Appraiser', N'FHA Analysis of Appraisal Report 54114', N'Appraiser Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (475, N'Pre-Funding Audit', N'LoanMaster', N'Appraiser', N'FHA Appraisal Logging', N'Appraiser Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (476, N'Pre-Funding Audit', N'LoanMaster', N'Appraiser', N'FHA Compliance Inspection Report 92051', N'Appraiser', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (477, N'Pre-Funding Audit', N'LoanMaster', N'CLTV', N'92900-LT', N'CLTV (if 2nd permitted)', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (478, N'Pre-Funding Audit', N'LoanMaster', N'CLTV', N'Notice of Loan Approval', N'CLTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (479, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'203K Borrower Acknowledgement 92700-A', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (480, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'203K Borrower Identify of Interest Certification', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (481, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'203K Rehabilitation Loan Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (482, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Adjustable-Rate Home Equity Conversion Mortgage', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (483, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Adjustable-Rate Home Equity Conversion Second Mortgage', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (484, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Adjustable-Rate Note (Home Equity Conversion)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (485, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Affiliated Business Arrangement Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (486, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Amendatory Escape Clause', N'File No/Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (487, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Amortization Schedule', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (488, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Assumption-Notice to Homeowners', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (489, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'AUS Desktop UW Findings Report', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (490, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'AUS Loan Prospector', N'LOAN APPLICATION NUMBER', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (491, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Borrower Certification and Authorization', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (492, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Change of Circumstance Form', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (493, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Closing Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (494, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Closing Instructions', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (495, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Closing Protection Letter', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (496, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Compliance Ease Report', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (497, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'CONDO Project Approval', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (498, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Consumer Credit Score Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (499, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Controlled Business Arrangement Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (500, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'DAP Authorization for Counseling', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (501, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'DAP Borrower Seller Affidavit', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (502, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'DAP Conditional Commitment', N'Lender Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (503, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'DAP Mortgage Loans or Mortgage Credit Certificate', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (504, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Deed of Trust Rider', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (505, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'E-Signature Certificates', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (506, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Earnings and Income Worksheet', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (507, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'ECOA Notice', N'Loan Number', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (508, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Errors and Omissions or Compliance Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (509, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Escrow Waiver and Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (510, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'FHA Analysis of Appraisal Report 54114', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (511, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'FHA Hotel and Transient Use of Property (92561)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (512, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'FHA Nearest Living Relative Information', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (513, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'First Payment Letter', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (514, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Flood Determination', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (515, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Flood Hazard Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (516, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Gift Document', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (517, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Good Faith Estimate', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (518, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Hazard Insurance', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (519, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Hazard Insurance Binder', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (520, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Home Equity Conversion Mortgage Notice of Right to Cancel', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (521, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'HUD-1 Settlement Statement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (522, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Important Notice to Homebuyers (92900B)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (523, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Informed Consumer Choice Disclosure Notice', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (524, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Initial Escrow Account Statement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (525, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Intent to Proceed', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (526, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Lender Risk Analysis Report', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (527, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Loan Application 1003 Continuation Sheet', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (528, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Lock Confirmation', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (529, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Lock In Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (530, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Mavent Review', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (531, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Mortgage Rider 2-4 Unit', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (532, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Mortgage Rider Condo', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (533, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Mortgage Rider PUD', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (534, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Name Affidavit', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (535, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Net Tangible Worksheet', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (536, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Note', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (537, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Notice of Loan Approval', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (538, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Notice of Right to Cancel', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (539, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Occupancy Certification', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (540, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Payment Plan - Exhibit 1', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (541, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Payoff Statement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (542, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Privacy Policy Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (543, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Property Detail Report Profile AVM', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (544, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Property Inspection Waiver (PIW)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (545, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Purchase Agreement Addendums', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (546, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Right to Receive a Copy of Appraisal Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (547, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Schedule of Liens - Exhibit 2', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (548, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Security Instrument', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (549, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Servicing Transfer Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (550, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Settlement Service Providers', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (551, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'State Disclosures', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (552, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Statement of Credit Denial, Termination or Change', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (553, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Subordination Agreement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (554, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Tax Information Sheet', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (555, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Cash Out T50-A6 Equity Loan', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (556, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Cash Out T50-A6 Right to Cancel', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (557, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Home Equity Affidavit and Agreement (First Lien)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (558, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Home Equity Note (Fixed Rate - First Lien)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (559, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Texas Owners Affidavit of Compliance', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (560, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'Title Policy', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (561, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'UCDP Submission Summary Report (SSR) - Fannie Mae', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (562, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'UCDP Submission Summary Report (SSR) - FreddieMac', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (563, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'US Patriot Act', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (564, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'USDA Certificate of Occupancy', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (565, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'USDA GUS Underwriting Findings and Analysis Report', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (566, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Child Care Statement', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (567, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Collection Policy Notice', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (568, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Common Certifications', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (569, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Counseling Checklist 26-0592', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (570, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Debt Questionnaire 26-0551', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (571, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Fixed Rate Mortgage Loan Disclosure', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (572, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA IRRRL Worksheet 26-8923', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (573, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Loan Analysis 26-6393', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (574, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Loan Comparison', N'New Loan Number', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (575, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Loan Guaranty Certificate (26-1899)', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (576, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Loan Summary Sheet 26-0286', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (577, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA National Guard Reservist Certification', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (578, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Nearest Living Relative', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (579, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Notice of Value', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (580, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Report and Cert of Loan Disbursement 26-1820', N'Loan Number', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (581, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Report and Cert of Loan Disbursement 26-1820', N'Lender Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (582, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VA Verification Benefit Related Indebtedness 26-8937', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (583, N'Pre-Funding Audit', N'LoanMaster', N'CustomerLoanNumber', N'VVOE', N'Loan No', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (584, N'Pre-Funding Audit', N'LoanMaster', N'LoanType', N'AUS Desktop UW Findings Report', N'Loan Type', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (585, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'1004D-Notice of Completion', N'Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (586, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Borrower Acknowledgement 92700-A', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (587, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Borrower Identify of Interest Certification', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (588, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Calculator', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (589, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Contractor Bid', N'Buyer', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (590, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Homeowner Contractor Agreement', N'Owner Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (591, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Initial Draw Request', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (592, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Rehabilitation Loan Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (593, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'203K Self Help Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (594, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'92900-LT', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (595, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Affiliated Business Arrangement Disclosure', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (596, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Amendatory Escape Clause', N'Buyer', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (597, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Amortization Schedule', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (598, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Appraisal', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (599, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Appraisal QC Review', N'Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (600, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Appraisal QC Review', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (601, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Attorney Infact Affidavit', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (602, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'AUS Desktop UW Findings Report', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (603, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'AUS Loan Prospector', N'BORROWER NAME', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (604, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'AUS Loan Prospector', N'SELECTED BORROWER', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (605, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Buydown Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (606, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Cash Flow Analysis', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (607, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Change of Circumstance Form', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (608, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Closing Disclosure', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (609, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Closing Instructions', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (610, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Closing Protection Letter', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (611, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Compliance Ease Report', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (612, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'CONDO Project Approval', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (613, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Credit Report Lender', N'App', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (614, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Credit Supplements', N'Applicant', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (615, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Certification of Income', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (616, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Conditional Commitment', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (617, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Deed of Trust 2nd', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (618, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Funds Verification Documentation', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (619, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Income Calculator', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (620, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Mortgage Loans or Mortgage Credit Certificate', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (621, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Note 2nd', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (622, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Recapture Documents', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (623, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Reservation Form', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (624, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'DAP Underwriter Certification', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (625, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Disclosure Notices', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (626, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'E-Signature Certificates', N'Signer Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (627, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Earnest Money Receipt', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (628, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Earnings and Income Worksheet', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (629, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Escrow Holdback Agreement', N'Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (630, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Analysis of Appraisal Report 54114', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (631, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Case Number Assignment', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (632, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Case Query', N'BorrowerName', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (633, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Indentity of Interest Certification', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (634, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Initial Addendum to Loan Application 92900A', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (635, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Late Submission Letter', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (636, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA MIP Netting Authorization', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (637, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Mortgage Insurance Certificate', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (638, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'FHA Nearest Living Relative Information', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (639, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'First Payment Letter', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (640, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Flood Determination', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (641, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Flood Hazard Disclosure', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (642, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Form 1098', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (643, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Form 1099R', N'PayerFullName', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (644, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Form 4506-8821', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (645, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Form SSA 1099', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (646, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Gift Document', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (647, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Good Faith Estimate', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (648, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HECM Amortization Schedule', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (649, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HECM Anti-Churning Disclosure (92901)', N'BorrowerName1', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (650, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HECM Financial Analysis Worksheet', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (651, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HECM FNMA Submission - Input Screen', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (652, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Home Counseling Disclosure', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (653, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Home Equity Conversion Mortgage Notice of Right to Cancel', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (654, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'HUD-1 Settlement Statement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (655, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Lender Risk Analysis Report', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (656, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Letters of Explanation (LOEs)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (657, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Loan Application 1003 Format 1', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (658, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Loan Application 1003 Format 2', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (659, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Lock Confirmation', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (660, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Lock In Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (661, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'MERS', N'Primary Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (662, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Name Affidavit', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (663, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Net Tangible Worksheet', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (664, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'New Construction Builders Certification of Plans 92541', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (665, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'NMLS License Verification', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (666, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Notice of Right to Cancel', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (667, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Occupancy Certification', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (668, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Payment Plan - Exhibit 1', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (669, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Payoff Statement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (670, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Private Mortgage Insurance (PMI) Certificate', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (671, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Property Detail Report Profile AVM', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (672, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Property Inspection Waiver (PIW)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (673, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Purchase Agreement Addendums', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (674, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'QM Test Results', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (675, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Retirement Assets', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (676, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Right to Receive a Copy of Appraisal Disclosure', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (677, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Security Instrument', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (678, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'SSA 89', N'Printed Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (679, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Statement of Credit Denial, Termination or Change', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (680, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Subordination Agreement', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (681, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1065X', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (682, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120C', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (683, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120F', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (684, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120L', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (685, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120REIT', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (686, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Form 1120S', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (687, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Information Sheet', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (688, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Tax Returns Transcripts', N'Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (689, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Cash Out T50-A6 Equity Loan', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (690, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Disclosure Form T-64', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (691, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Home Equity Affidavit and Agreement (First Lien)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (692, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Home Equity Security Instrument (First Lien)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (693, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Texas Owners Affidavit of Compliance', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (694, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Title Policy', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (695, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Total Annual Loan Cost (TALC)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (696, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Transmittal Summary Final', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (697, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'UCDP Submission Summary Report (SSR) - Fannie Mae', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (698, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'UCDP Submission Summary Report (SSR) - FreddieMac', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (699, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'US Patriot Act', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (700, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA Conditional Commitment 1980-18', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (701, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA GUS Underwriting Findings and Analysis Report', N'Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (702, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA GUS Underwriting Findings and Analysis Report', N'Primary Borrower', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (703, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA GUS Underwriting Findings and Analysis Report', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (704, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA Lender Cerfification for SFH Guaranteed Loan', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (705, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA Loan Note Guarantee', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (706, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA Request for SFH Loan Guarantee 3555-21', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (707, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'USDA SFH Income Eligibility', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (708, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Addendum to Application 26-1802A', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (709, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Appraisal Request Form 26-1805', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (710, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Common Certifications', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (711, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA DD214 and Statement of Service', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (712, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Funding Fee Notice', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (713, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Loan Analysis 26-6393', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (714, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Loan Comparison', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (715, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Loan Guaranty Certificate (26-1899)', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (716, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Loan Summary Sheet 26-0286', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (717, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Nearest Living Relative', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (718, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Notice of Value', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (719, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Report and Cert of Loan Disbursement 26-1820', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (720, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Request for COE 26-1880', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (721, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VA Verification Benefit Related Indebtedness 26-8937', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (722, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'Verification of Rent and Mortgage', N'Applicant Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (723, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'VVOE', N'Borrower Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (724, N'Pre-Funding Audit', N'lborrowermaster', N'displayfirstname + displaylastname', N'W9', N'BorrowerName', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (725, N'Pre-Funding Audit', N'LoanMaster', N'InterestRate', N'92900-LT', N'Interest Rate', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (726, N'Pre-Funding Audit', N'LoanMaster', N'InterestRate', N'AUS Loan Prospector', N'INTEREST RATE', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (727, N'Pre-Funding Audit', N'LoanMaster', N'InterestRate', N'Closing Disclosure', N'Interest Rate', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (728, N'Pre-Funding Audit', N'LoanMaster', N'InterestRate', N'Loan Application 1003 Format 1', N'Interest Rate', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (729, N'Pre-Funding Audit', N'LoanMaster', N'InterestRate', N'Loan Application 1003 Format 2', N'Interest Rate', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (730, N'Pre-Funding Audit', N'LoanMaster', N'InterestRate', N'Note', N'Interest Rate', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (731, N'Pre-Funding Audit', N'LoanMaster', N'InterestRate', N'Texas Home Equity Note (Fixed Rate - First Lien)', N'Interest Rate', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (732, N'Pre-Funding Audit', N'LoanMaster', N'InterestRate', N'VA Addendum to Application 26-1802A', N'Interest Rate', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (733, N'Pre-Funding Audit', N'LoanMaster', N'loanclosingdate', N'Closing Disclosure', N'Closing Date', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (734, N'Pre-Funding Audit', N'LoanMaster', N'loanclosingdate', N'Closing Instructions', N'Closing Date', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (735, N'Pre-Funding Audit', N'LoanMaster', N'loanclosingdate', N'Texas Disclosure Form T-64', N'Closing Date', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (736, N'Pre-Funding Audit', N'LoanMaster', N'LoanPurpose', N'AUS Desktop UW Findings Report', N'Loan Purpose', N'REFCODE_LOOKUP > RefCodeTypeId = 25', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (737, N'Pre-Funding Audit', N'LoanMaster', N'LoanPurpose', N'Lender Risk Analysis Report', N'Loan Purpose', N'REFCODE_LOOKUP > RefCodeTypeId = 25', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (738, N'Pre-Funding Audit', N'LoanMaster', N'LoanPurpose', N'USDA GUS Underwriting Findings and Analysis Report', N'Loan Purpose', N'REFCODE_LOOKUP > RefCodeTypeId = 25', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (739, N'Pre-Funding Audit', N'LoanMaster', N'LTV', N'92900-LT', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (740, N'Pre-Funding Audit', N'LoanMaster', N'LTV', N'AUS Loan Prospector', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (741, N'Pre-Funding Audit', N'LoanMaster', N'LTV', N'Notice of Loan Approval', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (742, N'Pre-Funding Audit', N'LoanMaster', N'LTV', N'Transmittal Summary Final', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (743, N'Pre-Funding Audit', N'LoanMaster', N'LTV', N'USDA GUS Underwriting Findings and Analysis Report', N'LTV', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (744, N'Pre-Funding Audit', N'LoanMaster', N'Originator', N'Loan Application 1003 Format 1', N'Loan Originator Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (745, N'Pre-Funding Audit', N'LoanMaster', N'Originator', N'Loan Application 1003 Format 2', N'Loan Originator Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (746, N'Pre-Funding Audit', N'LoanMaster', N'primaryborrowerlastname', N'DAP Mortgage Submission Voucher', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (747, N'Pre-Funding Audit', N'LoanMaster', N'primaryborrowerlastname', N'SSN Verification', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (748, N'Pre-Funding Audit', N'LoanMaster', N'primaryborrowerlastname', N'Tax Form 1040', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (749, N'Pre-Funding Audit', N'LoanMaster', N'primaryborrowerlastname', N'Tax Form 1040ES', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (750, N'Pre-Funding Audit', N'LoanMaster', N'primaryborrowerlastname', N'Tax Form 1040EZ', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (751, N'Pre-Funding Audit', N'LoanMaster', N'primaryborrowerlastname', N'Tax Form 1040X', N'Last Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (752, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'92900-LT', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (753, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Credit Report Lender', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (754, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Credit Supplements', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (755, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'FHA CAIVRS', N'Borrower SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (756, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'FHA Case Number Assignment', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (757, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Form 4506-8821', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (758, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Form SSA 1099', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (759, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Loan Application 1003 Format 1', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (760, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Loan Application 1003 Format 2', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (761, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'SSA 89', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (762, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'SSN Verification', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (763, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (764, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040EZ', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (765, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040SCH8812', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (766, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040SCHA', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (767, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040SCHB', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (768, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040SCHC', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (769, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040SCHCEZ', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (770, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040SCHE', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (771, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040SCHEIC', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (772, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040SCHF', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (773, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040SCHSE', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (774, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 1040X', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (775, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 2106', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (776, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Form 2106EZ', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (777, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Tax Returns Transcripts', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (778, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'Transmittal Summary Final', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (779, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'US Patriot Act', N'Borrower SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (780, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'USDA Lender Cerfification for SFH Guaranteed Loan', N'Borrower SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (781, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'USDA Request for SFH Loan Guarantee 3555-21', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (782, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'VA DD214 and Statement of Service', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (783, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'VA Loan Summary Sheet 26-0286', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (784, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'VA Report and Cert of Loan Disbursement 26-1820', N'Borrower SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (785, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'VA Request for COE 26-1880', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (786, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'VA Verification Benefit Related Indebtedness 26-8937', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (787, N'Pre-Funding Audit', N'lborrowerdata', N'SSN', N'W9', N'SSN', N'DECRPT USING CRYPTION CLASS', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (788, N'Pre-Funding Audit', N'LoanMaster', N'underwriter', N'CONDO Project Approval', N'UnderWriter Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (789, N'Pre-Funding Audit', N'LoanMaster', N'underwriter', N'FHA Analysis of Appraisal Report 54114', N'DE Underwriter Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (790, N'Pre-Funding Audit', N'LoanMaster', N'underwriter', N'Notice of Loan Approval', N'Underwriter Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (791, N'Pre-Funding Audit', N'LoanMaster', N'underwriter', N'Transmittal Summary Final', N'Underwriters Name', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (792, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'1004D-Notice of Completion', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (793, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'1004D-Notice of Completion', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (794, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'1004D-Notice of Completion', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (795, N'Pre-Funding Audit', N'lborroweraddress', N'postalcode', N'1004D-Notice of Completion', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (796, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'203K Contractor Bid', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (797, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'92900-LT', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (798, N'Pre-Funding Audit', N'lborroweraddress', N'postalcode', N'Appraisal QC Review', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (799, N'Pre-Funding Audit', N'LoanMaster', N'PropertyValue', N'Appraisal QC Review', N'Property Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (800, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Appraisal QC Review', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (801, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'Appraisal QC Review', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (802, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'Appraisal QC Review', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (803, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Attorney Infact Affidavit', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (804, N'Pre-Funding Audit', N'lborroweraddress', N'city+state+postalcode', N'Attorney Infact Affidavit', N'City State Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (805, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'AUS Desktop UW Findings Report', N'Total Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (806, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'AUS Desktop UW Findings Report', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (807, N'Pre-Funding Audit', N'LoanMaster', N'PurchasePrice', N'AUS Loan Prospector', N'PURCHASE PRICE', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (808, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'AUS Loan Prospector', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (809, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'AUS Loan Prospector', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (810, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'AUS Loan Prospector', N'PROPERTY ADDRESS', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (811, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Certificate of HECM Counseling', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (812, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Change of Circumstance Form', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (813, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Closing Disclosure', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (814, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Closing Instructions Supplemental', N'PROPERTYADDRESS', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (815, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Closing Protection Letter', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (816, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Credit Report Lender', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (817, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Credit Supplements', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (818, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'Credit Supplements', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (819, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'Credit Supplements', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (820, N'Pre-Funding Audit', N'lborroweraddress', N'postalcode', N'Credit Supplements', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (821, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'DAP Borrower Ack Home Warranty Protection Plan', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (822, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'DAP Borrower Ack Home Warranty Protection Plan', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (823, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'DAP Borrower Ack Home Warranty Protection Plan', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (824, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'DAP Borrower Affidavit for Start Up', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (825, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'DAP Borrower Affidavit for Start Up', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (826, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Disclosure Notices', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (827, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Escrow Instructions', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (828, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'FHA Appraisal Logging', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (829, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'FHA Case Number Assignment', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (830, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'FHA Case Number Assignment', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (831, N'Pre-Funding Audit', N'lborroweraddress', N'postalcode', N'FHA Case Number Assignment', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (832, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'FHA Compliance Inspection Report 92051', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (833, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'FHA Initial Addendum to Loan Application 92900A', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (834, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'FHA Initial Addendum to Loan Application 92900A', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (835, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'FHA MIP Netting Authorization', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (836, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Flood Determination', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (837, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Flood Insurance', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (838, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Flood Insurance', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (839, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Gift Document', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (840, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Hazard Insurance', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (841, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Hazard Insurance Binder', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (842, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Hazard Insurance Binder', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (843, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'HECM Financial Analysis Worksheet', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (844, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'HUD-1 Settlement Statement', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (845, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Lender Risk Analysis Report', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (846, N'Pre-Funding Audit', N'LoanMaster', N'PurchasePrice', N'Lender Risk Analysis Report', N'Purchase Price', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (847, N'Pre-Funding Audit', N'LoanMaster', N'PurchasePrice', N'Loan Application 1003 Format 1', N'Purchase Price', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (848, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Loan Application 1003 Format 1', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (849, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'Loan Application 1003 Format 1', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (850, N'Pre-Funding Audit', N'lborrowermaster', N'dateofbirth', N'Loan Application 1003 Format 1', N'Borrower DOB', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (851, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Loan Application 1003 Format 1', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (852, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Loan Application 1003 Format 2', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (853, N'Pre-Funding Audit', N'lborrowermaster', N'dateofbirth', N'Loan Application 1003 Format 2', N'Borrower DOB', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (854, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Loan Application 1003 Format 2', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (855, N'Pre-Funding Audit', N'LoanMaster', N'PurchasePrice', N'Loan Application 1003 Format 2', N'Purchase Price', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (856, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'Loan Application 1003 Format 2', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (857, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Loan Estimate', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (858, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Loan Estimate', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (859, N'Pre-Funding Audit', N'LoanMaster', N'PropertyValue', N'Loan Estimate', N'Estimated Property Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (860, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'MERS', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (861, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'MERS', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (862, N'Pre-Funding Audit', N'lborroweraddress', N'postalcode', N'MERS', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (863, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'MERS', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (864, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Net Tangible Worksheet', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (865, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'New Construction Builders Certification of Plans 92541', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (866, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'New Construction Warranty of Completion of Construction 92544', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (867, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Note', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (868, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'Note', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (869, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'Notice of Loan Approval', N'Total Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (870, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Private Mortgage Insurance (PMI) Certificate', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (871, N'Pre-Funding Audit', N'LoanMaster', N'PropertyValue', N'Property Detail Report Profile AVM', N'Property Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (872, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Property Detail Report Profile AVM', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (873, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Property Inspection Waiver (PIW)', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (874, N'Pre-Funding Audit', N'LoanMaster', N'PurchasePrice', N'Purchase Agreement', N'Purchase Price', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (875, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Purchase Agreement Addendums', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (876, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'Security Instrument', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (877, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Security Instrument', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (878, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Statement of Credit Denial, Termination or Change', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (879, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Subordination Agreement', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (880, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'Tax Form 1065X', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (881, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'Tax Form 1065X', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (882, N'Pre-Funding Audit', N'lborroweraddress', N'postalcode', N'Tax Form 1065X', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (883, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Tax Form 1065X', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (884, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Tax Form 1120C', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (885, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'Tax Form 1120C', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (886, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'Tax Form 1120C', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (887, N'Pre-Funding Audit', N'lborroweraddress', N'postalcode', N'Tax Form 1120C', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (888, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'Tax Form 1120F', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (889, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'Tax Form 1120F', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (890, N'Pre-Funding Audit', N'lborroweraddress', N'postalcode', N'Tax Form 1120F', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (891, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Tax Form 1120F', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (892, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Tax Form 1120L', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (893, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'Tax Form 1120L', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (894, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'Tax Form 1120L', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (895, N'Pre-Funding Audit', N'lborroweraddress', N'postalcode', N'Tax Form 1120L', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (896, N'Pre-Funding Audit', N'lborroweraddress', N'city+state+postalcode', N'Tax Form 1120REIT', N'City State and Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (897, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Tax Form 1120REIT', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (898, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'Tax Form 1120SF', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (899, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'Tax Form 1120SF', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (900, N'Pre-Funding Audit', N'LoanMaster', N'PropertyZip', N'Tax Form 1120SF', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (901, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Tax Form 1120SF', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (902, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Termite Inspection Report', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (903, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Termite Inspection Report', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (904, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'Termite Inspection Report', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (905, N'Pre-Funding Audit', N'LoanMaster', N'PropertyZip', N'Termite Inspection Report', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (906, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'Texas Disclosure Form T-64', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (907, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'Texas Home Equity Note (Fixed Rate - First Lien)', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (908, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Title Commit Prelim', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (909, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Title Policy', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (910, N'Pre-Funding Audit', N'LoanMaster', N'PropertyValue', N'Total Annual Loan Cost (TALC)', N'Appraised Property Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (911, N'Pre-Funding Audit', N'LoanMaster', N'PropertyValue', N'Transmittal Summary Final', N'Appraised Property Value', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (912, N'Pre-Funding Audit', N'LoanMaster', N'LoanPurpose', N'Transmittal Summary Final', N'LoanPurpose', N'REFCODE_LOOKUP > RefCodeTypeId = 25', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (913, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'Transmittal Summary Final', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (914, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'USDA Certificate of Occupancy', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (915, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'USDA GUS Underwriting Findings and Analysis Report', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (916, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'USDA GUS Underwriting Findings and Analysis Report', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (917, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'USDA Property Eligibility', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (918, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'USDA Request for SFH Loan Guarantee 3555-21', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (919, N'Pre-Funding Audit', N'lborroweraddress', N'city', N'USDA Request for SFH Loan Guarantee 3555-21', N'City', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (920, N'Pre-Funding Audit', N'lborroweraddress', N'state', N'USDA Request for SFH Loan Guarantee 3555-21', N'State', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (921, N'Pre-Funding Audit', N'LoanMaster', N'PropertyZip', N'USDA Request for SFH Loan Guarantee 3555-21', N'Zip', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (922, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'VA Addendum to Application 26-1802A', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (923, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'VA Addendum to Application 26-1802A', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (924, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'VA Addendum to Application 26-1802A', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (925, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'VA Appraisal Request Form 26-1805', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (926, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'VA Appraisal Request Form 26-1805', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (927, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'VA Loan Analysis 26-6393', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (928, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'VA Loan Comparison', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (929, N'Pre-Funding Audit', N'LoanMaster', N'LoanAmount', N'VA Loan Summary Sheet 26-0286', N'Loan Amount', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (930, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'VA Notice of Value', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (931, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Verification of Rent and Mortgage', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (932, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'Verification of Rent and Mortgage', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (933, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'VVOE', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (934, N'Pre-Funding Audit', N'lborroweraddress', N'streetaddress + city + state + postalcode', N'W2 Verification', N'Borrower Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] ([MAPPING_ID], [SERVICE_NAME], [TABLE_NAME], [COLUMN_NAME], [DOCUMENT_NAME], [FIELD_NAME], [CONDITION], [CREATED_ON], [MODIFIED_ON]) VALUES (935, N'Pre-Funding Audit', N'LoanMaster', N'PropertyAddress', N'Wire Funding Detail', N'Property Address', N'', CAST(N'2018-03-06 21:48:21.000' AS DateTime), CAST(N'2018-03-06 21:48:21.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[MTS_FIELD_LOOKUP_MAPPING] OFF
GO


GO
SET IDENTITY_INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ON 

GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (1, N'92900-LT', N'ALL', CAST(N'2018-03-08T10:42:27.653' AS DateTime), CAST(N'2018-03-08T10:42:27.653' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (2, N'Appraisal', N'ALL', CAST(N'2018-03-08T10:42:27.653' AS DateTime), CAST(N'2018-03-08T10:42:27.653' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (3, N'Appraisal QC Review', N'ALL', CAST(N'2018-03-08T10:42:27.653' AS DateTime), CAST(N'2018-03-08T10:42:27.653' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (4, N'AUS Findings', N'ALL', CAST(N'2018-03-08T10:42:27.657' AS DateTime), CAST(N'2018-03-08T10:42:27.657' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (5, N'Credit Report Lender', N'ALL', CAST(N'2018-03-08T10:42:27.657' AS DateTime), CAST(N'2018-03-08T10:42:27.657' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (6, N'FHA Mortgage Insurance Certificate', N'ALL', CAST(N'2018-03-08T10:42:27.657' AS DateTime), CAST(N'2018-03-08T10:42:27.657' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (7, N'Flood Determination', N'ALL', CAST(N'2018-03-08T10:42:27.657' AS DateTime), CAST(N'2018-03-08T10:42:27.657' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (8, N'Good Faith Estimate', N'ALL', CAST(N'2018-03-08T10:42:27.657' AS DateTime), CAST(N'2018-03-08T10:42:27.657' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (9, N'Hazard Insurance', N'ALL', CAST(N'2018-03-08T10:42:27.660' AS DateTime), CAST(N'2018-03-08T10:42:27.660' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (10, N'HUD-1 Settlement Statement', N'ALL', CAST(N'2018-03-08T10:42:27.660' AS DateTime), CAST(N'2018-03-08T10:42:27.660' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (11, N'Loan Application 1003 Format 1', N'ALL', CAST(N'2018-03-08T10:42:27.660' AS DateTime), CAST(N'2018-03-08T10:42:27.660' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (12, N'Loan Application 1003 Format 2', N'ALL', CAST(N'2018-03-08T10:42:27.660' AS DateTime), CAST(N'2018-03-08T10:42:27.660' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (13, N'FHA Mortgage Insurance Certificate', N'ALL', CAST(N'2018-03-08T10:42:27.660' AS DateTime), CAST(N'2018-03-08T10:42:27.660' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (14, N'Note', N'ALL', CAST(N'2018-03-08T10:42:27.660' AS DateTime), CAST(N'2018-03-08T10:42:27.660' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (15, N'Notice of Loan Approval', N'ALL', CAST(N'2018-03-08T10:42:27.660' AS DateTime), CAST(N'2018-03-08T10:42:27.660' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (16, N'Notice of Right to Cancel', N'ALL', CAST(N'2018-03-08T10:42:27.663' AS DateTime), CAST(N'2018-03-08T10:42:27.663' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (17, N'Purchase Agreement', N'ALL', CAST(N'2018-03-08T10:42:27.663' AS DateTime), CAST(N'2018-03-08T10:42:27.663' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (18, N'Security Instrument', N'ALL', CAST(N'2018-03-08T10:42:27.663' AS DateTime), CAST(N'2018-03-08T10:42:27.663' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (19, N'Title Commit Prelim', N'ALL', CAST(N'2018-03-08T10:42:27.667' AS DateTime), CAST(N'2018-03-08T10:42:27.667' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (20, N'Transmittal Summary Final', N'ALL', CAST(N'2018-03-08T10:42:27.667' AS DateTime), CAST(N'2018-03-08T10:42:27.667' AS DateTime))
GO
INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] ([ID], [DOCUMENT_NAME], [INSTANCE], [CREATED_ON], [MODIFIED_ON]) VALUES (21, N'VA Loan Analysis 26-6393', N'ALL', CAST(N'2018-03-08T10:42:27.667' AS DateTime), CAST(N'2018-03-08T10:42:27.667' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[MTS_AUTO_VALIDATION_SKIP] OFF
GO

SET IDENTITY_INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ON 

GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (1, 2, N'Notice of Loan Approval', 1, CAST(N'2018-03-29T07:00:47.870' AS DateTime), CAST(N'2018-03-29T07:00:47.870' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (2, 2, N'AUS Desktop UW Findings Report', 2, CAST(N'2018-03-29T07:00:47.870' AS DateTime), CAST(N'2018-03-29T07:00:47.870' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (3, 2, N'AUS Loan Prospector', 3, CAST(N'2018-03-29T07:00:47.870' AS DateTime), CAST(N'2018-03-29T07:00:47.870' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (4, 2, N'USDA GUS Underwriting Findings and Analysis Report', 4, CAST(N'2018-03-29T07:00:47.870' AS DateTime), CAST(N'2018-03-29T07:00:47.870' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (5, 2, N'92900-LT', 5, CAST(N'2018-03-29T07:00:47.873' AS DateTime), CAST(N'2018-03-29T07:00:47.873' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (6, 2, N'Transmittal Summary Final', 6, CAST(N'2018-03-29T07:00:47.873' AS DateTime), CAST(N'2018-03-29T07:00:47.873' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (7, 2, N'VA Loan Analysis 26-6393', 7, CAST(N'2018-03-29T07:00:47.873' AS DateTime), CAST(N'2018-03-29T07:00:47.873' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (8, 2, N'Loan Application 1003 Format 1', 8, CAST(N'2018-03-29T07:00:47.873' AS DateTime), CAST(N'2018-03-29T07:00:47.873' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (9, 2, N'Loan Application 1003 Format 2', 9, CAST(N'2018-03-29T07:00:47.873' AS DateTime), CAST(N'2018-03-29T07:00:47.873' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (10, 2, N'Loan Application 1003 Continuation Sheet', 10, CAST(N'2018-03-29T07:00:47.877' AS DateTime), CAST(N'2018-03-29T07:00:47.877' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (11, 2, N'FHA Initial Addendum to Loan Application 92900A', 11, CAST(N'2018-03-29T07:00:47.877' AS DateTime), CAST(N'2018-03-29T07:00:47.877' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (12, 2, N'Credit Report Lender', 12, CAST(N'2018-03-29T07:00:47.880' AS DateTime), CAST(N'2018-03-29T07:00:47.880' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (13, 2, N'Credit Supplements', 13, CAST(N'2018-03-29T07:00:47.880' AS DateTime), CAST(N'2018-03-29T07:00:47.880' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (14, 2, N'Appraisal', 14, CAST(N'2018-03-29T07:00:47.880' AS DateTime), CAST(N'2018-03-29T07:00:47.880' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (15, 2, N'Appraisal QC Review', 15, CAST(N'2018-03-29T07:00:47.883' AS DateTime), CAST(N'2018-03-29T07:00:47.883' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (16, 2, N'1004D-Notice of Completion', 16, CAST(N'2018-03-29T07:00:47.883' AS DateTime), CAST(N'2018-03-29T07:00:47.883' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (17, 2, N'Flood Determination', 17, CAST(N'2018-03-29T07:00:47.883' AS DateTime), CAST(N'2018-03-29T07:00:47.883' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (18, 2, N'Hazard Insurance', 18, CAST(N'2018-03-29T07:00:47.887' AS DateTime), CAST(N'2018-03-29T07:00:47.887' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (19, 2, N'Hazard Insurance Binder', 19, CAST(N'2018-03-29T07:00:47.887' AS DateTime), CAST(N'2018-03-29T07:00:47.887' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (20, 2, N'Flood Hazard Disclosure', 20, CAST(N'2018-03-29T07:00:47.887' AS DateTime), CAST(N'2018-03-29T07:00:47.887' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (21, 2, N'Note', 21, CAST(N'2018-03-29T07:00:47.890' AS DateTime), CAST(N'2018-03-29T07:00:47.890' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (22, 2, N'Security Instrument', 22, CAST(N'2018-03-29T07:00:47.890' AS DateTime), CAST(N'2018-03-29T07:00:47.890' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (23, 2, N'Purchase Agreement', 23, CAST(N'2018-03-29T07:00:47.890' AS DateTime), CAST(N'2018-03-29T07:00:47.890' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (24, 2, N'Purchase Agreement Addendums', 24, CAST(N'2018-03-29T07:00:47.890' AS DateTime), CAST(N'2018-03-29T07:00:47.890' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (25, 2, N'Letters of Explanation (LOEs)', 25, CAST(N'2018-03-29T07:00:47.893' AS DateTime), CAST(N'2018-03-29T07:00:47.893' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (26, 2, N'Verification of Rent and Mortgage', 26, CAST(N'2018-03-29T07:00:47.893' AS DateTime), CAST(N'2018-03-29T07:00:47.893' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (27, 2, N'Written Verification of Employment', 27, CAST(N'2018-03-29T07:00:47.893' AS DateTime), CAST(N'2018-03-29T07:00:47.893' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (28, 2, N'VOE Work No', 28, CAST(N'2018-03-29T07:00:47.897' AS DateTime), CAST(N'2018-03-29T07:00:47.897' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (29, 2, N'VVOE', 29, CAST(N'2018-03-29T07:00:47.897' AS DateTime), CAST(N'2018-03-29T07:00:47.897' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (30, 2, N'Pay Stub', 30, CAST(N'2018-03-29T07:00:47.897' AS DateTime), CAST(N'2018-03-29T07:00:47.897' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (31, 2, N'W2 Verification', 31, CAST(N'2018-03-29T07:00:47.900' AS DateTime), CAST(N'2018-03-29T07:00:47.900' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (32, 2, N'Bank Statements', 32, CAST(N'2018-03-29T07:00:47.900' AS DateTime), CAST(N'2018-03-29T07:00:47.900' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (33, 2, N'Retirement Assets', 33, CAST(N'2018-03-29T07:00:47.900' AS DateTime), CAST(N'2018-03-29T07:00:47.900' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (34, 2, N'Verification of Deposits (VOD)', 34, CAST(N'2018-03-29T07:00:47.900' AS DateTime), CAST(N'2018-03-29T07:00:47.900' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (35, 2, N'Title Policy', 35, CAST(N'2018-03-29T07:00:47.903' AS DateTime), CAST(N'2018-03-29T07:00:47.903' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (36, 2, N'Title Commit Prelim', 36, CAST(N'2018-03-29T07:00:47.903' AS DateTime), CAST(N'2018-03-29T07:00:47.903' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (37, 2, N'Notice of Right to Cancel', 37, CAST(N'2018-03-29T07:00:47.903' AS DateTime), CAST(N'2018-03-29T07:00:47.903' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (38, 2, N'203K Borrower Acknowledgement 92700-A', 38, CAST(N'2018-03-29T07:00:47.907' AS DateTime), CAST(N'2018-03-29T07:00:47.907' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (39, 2, N'203K Borrower Identify of Interest Certification', 39, CAST(N'2018-03-29T07:00:47.907' AS DateTime), CAST(N'2018-03-29T07:00:47.907' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (40, 2, N'203K Calculator', 40, CAST(N'2018-03-29T07:00:47.907' AS DateTime), CAST(N'2018-03-29T07:00:47.907' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (41, 2, N'203K Change Request', 41, CAST(N'2018-03-29T07:00:47.910' AS DateTime), CAST(N'2018-03-29T07:00:47.910' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (42, 2, N'203K Contractor Bid', 42, CAST(N'2018-03-29T07:00:47.910' AS DateTime), CAST(N'2018-03-29T07:00:47.910' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (43, 2, N'203K Homeowner Contractor Agreement', 43, CAST(N'2018-03-29T07:00:47.910' AS DateTime), CAST(N'2018-03-29T07:00:47.910' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (44, 2, N'203K Initial Draw Request', 44, CAST(N'2018-03-29T07:00:47.910' AS DateTime), CAST(N'2018-03-29T07:00:47.910' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (45, 2, N'203K Rehabilitation Loan Agreement', 45, CAST(N'2018-03-29T07:00:47.913' AS DateTime), CAST(N'2018-03-29T07:00:47.913' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (46, 2, N'203K Self Help Agreement', 46, CAST(N'2018-03-29T07:00:47.913' AS DateTime), CAST(N'2018-03-29T07:00:47.913' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (47, 2, N'Adjustable-Rate Home Equity Conversion Mortgage', 47, CAST(N'2018-03-29T07:00:47.913' AS DateTime), CAST(N'2018-03-29T07:00:47.913' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (48, 2, N'Adjustable-Rate Home Equity Conversion Second Mortgage', 48, CAST(N'2018-03-29T07:00:47.917' AS DateTime), CAST(N'2018-03-29T07:00:47.917' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (49, 2, N'Adjustable-Rate Note (Home Equity Conversion)', 49, CAST(N'2018-03-29T07:00:47.917' AS DateTime), CAST(N'2018-03-29T07:00:47.917' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (50, 2, N'Affiliated Business Arrangement Disclosure', 50, CAST(N'2018-03-29T07:00:47.917' AS DateTime), CAST(N'2018-03-29T07:00:47.917' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (51, 2, N'Amendatory Escape Clause', 51, CAST(N'2018-03-29T07:00:47.920' AS DateTime), CAST(N'2018-03-29T07:00:47.920' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (52, 2, N'Amortization Schedule', 52, CAST(N'2018-03-29T07:00:47.920' AS DateTime), CAST(N'2018-03-29T07:00:47.920' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (53, 2, N'Assumption-Notice to Homeowners', 53, CAST(N'2018-03-29T07:00:47.920' AS DateTime), CAST(N'2018-03-29T07:00:47.920' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (54, 2, N'Attorney Infact Affidavit', 54, CAST(N'2018-03-29T07:00:47.923' AS DateTime), CAST(N'2018-03-29T07:00:47.923' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (55, 2, N'Borrower Certification and Authorization', 55, CAST(N'2018-03-29T07:00:47.923' AS DateTime), CAST(N'2018-03-29T07:00:47.923' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (56, 2, N'Buydown Agreement', 56, CAST(N'2018-03-29T07:00:47.923' AS DateTime), CAST(N'2018-03-29T07:00:47.923' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (57, 2, N'Cash Flow Analysis', 57, CAST(N'2018-03-29T07:00:47.927' AS DateTime), CAST(N'2018-03-29T07:00:47.927' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (58, 2, N'Certificate of HECM Counseling', 58, CAST(N'2018-03-29T07:00:47.927' AS DateTime), CAST(N'2018-03-29T07:00:47.927' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (59, 2, N'Change of Circumstance Form', 59, CAST(N'2018-03-29T07:00:47.927' AS DateTime), CAST(N'2018-03-29T07:00:47.927' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (60, 2, N'Closing Disclosure', 60, CAST(N'2018-03-29T07:00:47.930' AS DateTime), CAST(N'2018-03-29T07:00:47.930' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (61, 2, N'Closing Instructions', 61, CAST(N'2018-03-29T07:00:47.930' AS DateTime), CAST(N'2018-03-29T07:00:47.930' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (62, 2, N'Closing Instructions Supplemental', 62, CAST(N'2018-03-29T07:00:47.930' AS DateTime), CAST(N'2018-03-29T07:00:47.930' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (63, 2, N'Closing Protection Letter', 63, CAST(N'2018-03-29T07:00:47.930' AS DateTime), CAST(N'2018-03-29T07:00:47.930' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (64, 2, N'Compliance Ease Report', 64, CAST(N'2018-03-29T07:00:47.933' AS DateTime), CAST(N'2018-03-29T07:00:47.933' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (65, 2, N'CONDO Project Approval', 65, CAST(N'2018-03-29T07:00:47.933' AS DateTime), CAST(N'2018-03-29T07:00:47.933' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (66, 2, N'Consumer Credit Score Disclosure', 66, CAST(N'2018-03-29T07:00:47.937' AS DateTime), CAST(N'2018-03-29T07:00:47.937' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (67, 2, N'Controlled Business Arrangement Disclosure', 67, CAST(N'2018-03-29T07:00:47.937' AS DateTime), CAST(N'2018-03-29T07:00:47.937' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (68, 2, N'DAP Authorization for Counseling', 68, CAST(N'2018-03-29T07:00:47.937' AS DateTime), CAST(N'2018-03-29T07:00:47.937' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (69, 2, N'DAP Borrower Ack Home Warranty Protection Plan', 69, CAST(N'2018-03-29T07:00:47.940' AS DateTime), CAST(N'2018-03-29T07:00:47.940' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (70, 2, N'DAP Borrower Affidavit for Start Up', 70, CAST(N'2018-03-29T07:00:47.940' AS DateTime), CAST(N'2018-03-29T07:00:47.940' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (71, 2, N'DAP Borrower Seller Affidavit', 71, CAST(N'2018-03-29T07:00:47.940' AS DateTime), CAST(N'2018-03-29T07:00:47.940' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (72, 2, N'DAP Certification of Income', 72, CAST(N'2018-03-29T07:00:47.940' AS DateTime), CAST(N'2018-03-29T07:00:47.940' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (73, 2, N'DAP Conditional Commitment', 73, CAST(N'2018-03-29T07:00:47.943' AS DateTime), CAST(N'2018-03-29T07:00:47.943' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (74, 2, N'DAP Deed of Trust 2nd', 74, CAST(N'2018-03-29T07:00:47.943' AS DateTime), CAST(N'2018-03-29T07:00:47.943' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (75, 2, N'DAP Funds Verification Documentation', 75, CAST(N'2018-03-29T07:00:47.943' AS DateTime), CAST(N'2018-03-29T07:00:47.943' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (76, 2, N'DAP Income Calculator', 76, CAST(N'2018-03-29T07:00:47.943' AS DateTime), CAST(N'2018-03-29T07:00:47.943' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (77, 2, N'DAP Mortgage Loans or Mortgage Credit Certificate', 77, CAST(N'2018-03-29T07:00:47.943' AS DateTime), CAST(N'2018-03-29T07:00:47.943' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (78, 2, N'DAP Mortgage Submission Voucher', 78, CAST(N'2018-03-29T07:00:47.947' AS DateTime), CAST(N'2018-03-29T07:00:47.947' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (79, 2, N'DAP Note 2nd', 79, CAST(N'2018-03-29T07:00:47.947' AS DateTime), CAST(N'2018-03-29T07:00:47.947' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (80, 2, N'DAP Recapture Documents', 80, CAST(N'2018-03-29T07:00:47.947' AS DateTime), CAST(N'2018-03-29T07:00:47.947' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (81, 2, N'DAP Reservation Form', 81, CAST(N'2018-03-29T07:00:47.947' AS DateTime), CAST(N'2018-03-29T07:00:47.947' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (82, 2, N'DAP Underwriter Certification', 82, CAST(N'2018-03-29T07:00:47.947' AS DateTime), CAST(N'2018-03-29T07:00:47.947' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (83, 2, N'Deed of Trust Rider', 83, CAST(N'2018-03-29T07:00:47.950' AS DateTime), CAST(N'2018-03-29T07:00:47.950' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (84, 2, N'Disclosure Notices', 84, CAST(N'2018-03-29T07:00:47.950' AS DateTime), CAST(N'2018-03-29T07:00:47.950' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (85, 2, N'E-Signature Certificates', 85, CAST(N'2018-03-29T07:00:47.950' AS DateTime), CAST(N'2018-03-29T07:00:47.950' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (86, 2, N'Earnest Money Receipt', 86, CAST(N'2018-03-29T07:00:47.950' AS DateTime), CAST(N'2018-03-29T07:00:47.950' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (87, 2, N'Earnings and Income Worksheet', 87, CAST(N'2018-03-29T07:00:47.950' AS DateTime), CAST(N'2018-03-29T07:00:47.950' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (88, 2, N'ECOA Notice', 88, CAST(N'2018-03-29T07:00:47.950' AS DateTime), CAST(N'2018-03-29T07:00:47.950' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (89, 2, N'Errors and Omissions or Compliance Agreement', 89, CAST(N'2018-03-29T07:00:47.953' AS DateTime), CAST(N'2018-03-29T07:00:47.953' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (90, 2, N'Escrow Holdback Agreement', 90, CAST(N'2018-03-29T07:00:47.953' AS DateTime), CAST(N'2018-03-29T07:00:47.953' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (91, 2, N'Escrow Instructions', 91, CAST(N'2018-03-29T07:00:47.953' AS DateTime), CAST(N'2018-03-29T07:00:47.953' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (92, 2, N'Escrow Waiver and Agreement', 92, CAST(N'2018-03-29T07:00:47.953' AS DateTime), CAST(N'2018-03-29T07:00:47.953' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (93, 2, N'FHA Analysis of Appraisal Report 54114', 93, CAST(N'2018-03-29T07:00:47.953' AS DateTime), CAST(N'2018-03-29T07:00:47.953' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (94, 2, N'FHA Appraisal Logging', 94, CAST(N'2018-03-29T07:00:47.957' AS DateTime), CAST(N'2018-03-29T07:00:47.957' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (95, 2, N'FHA CAIVRS', 95, CAST(N'2018-03-29T07:00:47.957' AS DateTime), CAST(N'2018-03-29T07:00:47.957' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (96, 2, N'FHA Case Number Assignment', 96, CAST(N'2018-03-29T07:00:47.957' AS DateTime), CAST(N'2018-03-29T07:00:47.957' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (97, 2, N'FHA Case Query', 97, CAST(N'2018-03-29T07:00:47.957' AS DateTime), CAST(N'2018-03-29T07:00:47.957' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (98, 2, N'FHA Compliance Inspection Report 92051', 98, CAST(N'2018-03-29T07:00:47.957' AS DateTime), CAST(N'2018-03-29T07:00:47.957' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (99, 2, N'FHA Conditional Commitment 92800-5B', 99, CAST(N'2018-03-29T07:00:47.960' AS DateTime), CAST(N'2018-03-29T07:00:47.960' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (100, 2, N'FHA Hotel and Transient Use of Property (92561)', 100, CAST(N'2018-03-29T07:00:47.960' AS DateTime), CAST(N'2018-03-29T07:00:47.960' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (101, 2, N'FHA Indentity of Interest Certification', 101, CAST(N'2018-03-29T07:00:47.960' AS DateTime), CAST(N'2018-03-29T07:00:47.960' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (102, 2, N'FHA Late Submission Letter', 102, CAST(N'2018-03-29T07:00:47.960' AS DateTime), CAST(N'2018-03-29T07:00:47.960' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (103, 2, N'FHA MIP Netting Authorization', 103, CAST(N'2018-03-29T07:00:47.960' AS DateTime), CAST(N'2018-03-29T07:00:47.960' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (104, 2, N'FHA Mortgage Insurance Certificate', 104, CAST(N'2018-03-29T07:00:47.960' AS DateTime), CAST(N'2018-03-29T07:00:47.960' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (105, 2, N'FHA Nearest Living Relative Information', 105, CAST(N'2018-03-29T07:00:47.960' AS DateTime), CAST(N'2018-03-29T07:00:47.960' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (106, 2, N'First Payment Letter', 106, CAST(N'2018-03-29T07:00:47.963' AS DateTime), CAST(N'2018-03-29T07:00:47.963' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (107, 2, N'Flood Insurance', 107, CAST(N'2018-03-29T07:00:47.963' AS DateTime), CAST(N'2018-03-29T07:00:47.963' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (108, 2, N'Form 1098', 108, CAST(N'2018-03-29T07:00:47.963' AS DateTime), CAST(N'2018-03-29T07:00:47.963' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (109, 2, N'Form 1099 INT', 109, CAST(N'2018-03-29T07:00:47.963' AS DateTime), CAST(N'2018-03-29T07:00:47.963' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (110, 2, N'Form 1099 Misc', 110, CAST(N'2018-03-29T07:00:47.963' AS DateTime), CAST(N'2018-03-29T07:00:47.963' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (111, 2, N'Form 1099B', 111, CAST(N'2018-03-29T07:00:47.967' AS DateTime), CAST(N'2018-03-29T07:00:47.967' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (112, 2, N'Form 1099R', 112, CAST(N'2018-03-29T07:00:47.967' AS DateTime), CAST(N'2018-03-29T07:00:47.967' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (113, 2, N'Form 4506-8821', 113, CAST(N'2018-03-29T07:00:47.967' AS DateTime), CAST(N'2018-03-29T07:00:47.967' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (114, 2, N'Form SSA 1099', 114, CAST(N'2018-03-29T07:00:47.967' AS DateTime), CAST(N'2018-03-29T07:00:47.967' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (115, 2, N'Gift Document', 115, CAST(N'2018-03-29T07:00:47.967' AS DateTime), CAST(N'2018-03-29T07:00:47.967' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (116, 2, N'Good Faith Estimate', 116, CAST(N'2018-03-29T07:00:47.970' AS DateTime), CAST(N'2018-03-29T07:00:47.970' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (117, 2, N'GSA (SAM) List', 117, CAST(N'2018-03-29T07:00:47.970' AS DateTime), CAST(N'2018-03-29T07:00:47.970' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (118, 2, N'HECM Amortization Schedule', 118, CAST(N'2018-03-29T07:00:47.970' AS DateTime), CAST(N'2018-03-29T07:00:47.970' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (119, 2, N'HECM Anti-Churning Disclosure (92901)', 119, CAST(N'2018-03-29T07:00:47.970' AS DateTime), CAST(N'2018-03-29T07:00:47.970' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (120, 2, N'HECM Financial Analysis Worksheet', 120, CAST(N'2018-03-29T07:00:47.970' AS DateTime), CAST(N'2018-03-29T07:00:47.970' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (121, 2, N'HECM FNMA Submission - Input Screen', 121, CAST(N'2018-03-29T07:00:47.970' AS DateTime), CAST(N'2018-03-29T07:00:47.970' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (122, 2, N'Home Counseling Disclosure', 122, CAST(N'2018-03-29T07:00:47.973' AS DateTime), CAST(N'2018-03-29T07:00:47.973' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (123, 2, N'Home Equity Conversion Mortgage Notice of Right to Cancel', 123, CAST(N'2018-03-29T07:00:47.973' AS DateTime), CAST(N'2018-03-29T07:00:47.973' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (124, 2, N'HUD-1 from Sale of Borrowers Residence or Property', 124, CAST(N'2018-03-29T07:00:47.973' AS DateTime), CAST(N'2018-03-29T07:00:47.973' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (125, 2, N'HUD-1 Settlement Statement', 125, CAST(N'2018-03-29T07:00:47.973' AS DateTime), CAST(N'2018-03-29T07:00:47.973' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (126, 2, N'Important Notice to Homebuyers (92900B)', 126, CAST(N'2018-03-29T07:00:47.973' AS DateTime), CAST(N'2018-03-29T07:00:47.973' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (127, 2, N'Informed Consumer Choice Disclosure Notice', 127, CAST(N'2018-03-29T07:00:47.977' AS DateTime), CAST(N'2018-03-29T07:00:47.977' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (128, 2, N'Initial Escrow Account Statement', 128, CAST(N'2018-03-29T07:00:47.977' AS DateTime), CAST(N'2018-03-29T07:00:47.977' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (129, 2, N'Intent to Proceed', 129, CAST(N'2018-03-29T07:00:47.977' AS DateTime), CAST(N'2018-03-29T07:00:47.977' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (130, 2, N'Lender Risk Analysis Report', 130, CAST(N'2018-03-29T07:00:47.977' AS DateTime), CAST(N'2018-03-29T07:00:47.977' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (131, 2, N'Loan Estimate', 131, CAST(N'2018-03-29T07:00:47.977' AS DateTime), CAST(N'2018-03-29T07:00:47.977' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (132, 2, N'Lock Confirmation', 132, CAST(N'2018-03-29T07:00:47.980' AS DateTime), CAST(N'2018-03-29T07:00:47.980' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (133, 2, N'Lock In Agreement', 133, CAST(N'2018-03-29T07:00:47.980' AS DateTime), CAST(N'2018-03-29T07:00:47.980' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (134, 2, N'Mavent Review', 134, CAST(N'2018-03-29T07:00:47.980' AS DateTime), CAST(N'2018-03-29T07:00:47.980' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (135, 2, N'MERS', 135, CAST(N'2018-03-29T07:00:47.980' AS DateTime), CAST(N'2018-03-29T07:00:47.980' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (136, 2, N'Mortgage Rider 2-4 Unit', 136, CAST(N'2018-03-29T07:00:47.980' AS DateTime), CAST(N'2018-03-29T07:00:47.980' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (137, 2, N'Mortgage Rider Condo', 137, CAST(N'2018-03-29T07:00:47.980' AS DateTime), CAST(N'2018-03-29T07:00:47.980' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (138, 2, N'Mortgage Rider PUD', 138, CAST(N'2018-03-29T07:00:47.980' AS DateTime), CAST(N'2018-03-29T07:00:47.980' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (139, 2, N'Name Affidavit', 139, CAST(N'2018-03-29T07:00:47.983' AS DateTime), CAST(N'2018-03-29T07:00:47.983' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (140, 2, N'Net Tangible Worksheet', 140, CAST(N'2018-03-29T07:00:47.983' AS DateTime), CAST(N'2018-03-29T07:00:47.983' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (141, 2, N'New Construction Builders Certification of Plans 92541', 141, CAST(N'2018-03-29T07:00:47.983' AS DateTime), CAST(N'2018-03-29T07:00:47.983' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (142, 2, N'New Construction Warranty of Completion of Construction 92544', 142, CAST(N'2018-03-29T07:00:47.983' AS DateTime), CAST(N'2018-03-29T07:00:47.983' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (143, 2, N'NMLS License Verification', 143, CAST(N'2018-03-29T07:00:47.983' AS DateTime), CAST(N'2018-03-29T07:00:47.983' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (144, 2, N'Occupancy Certification', 144, CAST(N'2018-03-29T07:00:47.987' AS DateTime), CAST(N'2018-03-29T07:00:47.987' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (145, 2, N'Payment Plan - Exhibit 1', 145, CAST(N'2018-03-29T07:00:47.987' AS DateTime), CAST(N'2018-03-29T07:00:47.987' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (146, 2, N'Payoff Statement', 146, CAST(N'2018-03-29T07:00:47.987' AS DateTime), CAST(N'2018-03-29T07:00:47.987' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (147, 2, N'Privacy Policy Disclosure', 147, CAST(N'2018-03-29T07:00:47.987' AS DateTime), CAST(N'2018-03-29T07:00:47.987' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (148, 2, N'Private Mortgage Insurance (PMI) Certificate', 148, CAST(N'2018-03-29T07:00:47.987' AS DateTime), CAST(N'2018-03-29T07:00:47.987' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (149, 2, N'Property Detail Report Profile AVM', 149, CAST(N'2018-03-29T07:00:47.990' AS DateTime), CAST(N'2018-03-29T07:00:47.990' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (150, 2, N'Property Inspection Waiver (PIW)', 150, CAST(N'2018-03-29T07:00:47.990' AS DateTime), CAST(N'2018-03-29T07:00:47.990' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (151, 2, N'QM Test Results', 151, CAST(N'2018-03-29T07:00:47.990' AS DateTime), CAST(N'2018-03-29T07:00:47.990' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (152, 2, N'Quit Claim Deed', 152, CAST(N'2018-03-29T07:00:47.990' AS DateTime), CAST(N'2018-03-29T07:00:47.990' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (153, 2, N'Reverse Mortgage Loan Comparison', 153, CAST(N'2018-03-29T07:00:47.990' AS DateTime), CAST(N'2018-03-29T07:00:47.990' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (154, 2, N'Right to Receive a Copy of Appraisal Disclosure', 154, CAST(N'2018-03-29T07:00:47.990' AS DateTime), CAST(N'2018-03-29T07:00:47.990' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (155, 2, N'Schedule of Liens - Exhibit 2', 155, CAST(N'2018-03-29T07:00:47.990' AS DateTime), CAST(N'2018-03-29T07:00:47.990' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (156, 2, N'Servicing Transfer Disclosure', 156, CAST(N'2018-03-29T07:00:47.993' AS DateTime), CAST(N'2018-03-29T07:00:47.993' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (157, 2, N'Settlement Service Providers', 157, CAST(N'2018-03-29T07:00:47.993' AS DateTime), CAST(N'2018-03-29T07:00:47.993' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (158, 2, N'SSA 89', 158, CAST(N'2018-03-29T07:00:47.993' AS DateTime), CAST(N'2018-03-29T07:00:47.993' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (159, 2, N'SSN Verification', 159, CAST(N'2018-03-29T07:00:47.993' AS DateTime), CAST(N'2018-03-29T07:00:47.993' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (160, 2, N'State Disclosures', 160, CAST(N'2018-03-29T07:00:47.993' AS DateTime), CAST(N'2018-03-29T07:00:47.993' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (161, 2, N'Statement of Credit Denial, Termination or Change', 161, CAST(N'2018-03-29T07:00:47.997' AS DateTime), CAST(N'2018-03-29T07:00:47.997' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (162, 2, N'Subordination Agreement', 162, CAST(N'2018-03-29T07:00:47.997' AS DateTime), CAST(N'2018-03-29T07:00:47.997' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (163, 2, N'Tax Form 1040', 163, CAST(N'2018-03-29T07:00:47.997' AS DateTime), CAST(N'2018-03-29T07:00:47.997' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (164, 2, N'Tax Form 1040A', 164, CAST(N'2018-03-29T07:00:47.997' AS DateTime), CAST(N'2018-03-29T07:00:47.997' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (165, 2, N'Tax Form 1040ES', 165, CAST(N'2018-03-29T07:00:47.997' AS DateTime), CAST(N'2018-03-29T07:00:47.997' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (166, 2, N'Tax Form 1040EZ', 166, CAST(N'2018-03-29T07:00:48.000' AS DateTime), CAST(N'2018-03-29T07:00:48.000' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (167, 2, N'Tax Form 1040SCH8812', 167, CAST(N'2018-03-29T07:00:48.000' AS DateTime), CAST(N'2018-03-29T07:00:48.000' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (168, 2, N'Tax Form 1040SCHA', 168, CAST(N'2018-03-29T07:00:48.000' AS DateTime), CAST(N'2018-03-29T07:00:48.000' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (169, 2, N'Tax Form 1040SCHB', 169, CAST(N'2018-03-29T07:00:48.000' AS DateTime), CAST(N'2018-03-29T07:00:48.000' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (170, 2, N'Tax Form 1040SCHC', 170, CAST(N'2018-03-29T07:00:48.000' AS DateTime), CAST(N'2018-03-29T07:00:48.000' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (171, 2, N'Tax Form 1040SCHCEZ', 171, CAST(N'2018-03-29T07:00:48.000' AS DateTime), CAST(N'2018-03-29T07:00:48.000' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (172, 2, N'Tax Form 1040SCHD', 172, CAST(N'2018-03-29T07:00:48.003' AS DateTime), CAST(N'2018-03-29T07:00:48.003' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (173, 2, N'Tax Form 1040SCHE', 173, CAST(N'2018-03-29T07:00:48.003' AS DateTime), CAST(N'2018-03-29T07:00:48.003' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (174, 2, N'Tax Form 1040SCHEIC', 174, CAST(N'2018-03-29T07:00:48.003' AS DateTime), CAST(N'2018-03-29T07:00:48.003' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (175, 2, N'Tax Form 1040SCHF', 175, CAST(N'2018-03-29T07:00:48.003' AS DateTime), CAST(N'2018-03-29T07:00:48.003' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (176, 2, N'Tax Form 1040SCHSE', 176, CAST(N'2018-03-29T07:00:48.003' AS DateTime), CAST(N'2018-03-29T07:00:48.003' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (177, 2, N'Tax Form 1040X', 177, CAST(N'2018-03-29T07:00:48.007' AS DateTime), CAST(N'2018-03-29T07:00:48.007' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (178, 2, N'Tax Form 1065', 178, CAST(N'2018-03-29T07:00:48.007' AS DateTime), CAST(N'2018-03-29T07:00:48.007' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (179, 2, N'Tax Form 1065B', 179, CAST(N'2018-03-29T07:00:48.007' AS DateTime), CAST(N'2018-03-29T07:00:48.007' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (180, 2, N'Tax Form 1065SCHK1', 180, CAST(N'2018-03-29T07:00:48.007' AS DateTime), CAST(N'2018-03-29T07:00:48.007' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (181, 2, N'Tax Form 1065X', 181, CAST(N'2018-03-29T07:00:48.007' AS DateTime), CAST(N'2018-03-29T07:00:48.007' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (182, 2, N'Tax Form 1120', 182, CAST(N'2018-03-29T07:00:48.007' AS DateTime), CAST(N'2018-03-29T07:00:48.007' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (183, 2, N'Tax Form 1120C', 183, CAST(N'2018-03-29T07:00:48.010' AS DateTime), CAST(N'2018-03-29T07:00:48.010' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (184, 2, N'Tax Form 1120F', 184, CAST(N'2018-03-29T07:00:48.010' AS DateTime), CAST(N'2018-03-29T07:00:48.010' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (185, 2, N'Tax Form 1120L', 185, CAST(N'2018-03-29T07:00:48.010' AS DateTime), CAST(N'2018-03-29T07:00:48.010' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (186, 2, N'Tax Form 1120REIT', 186, CAST(N'2018-03-29T07:00:48.010' AS DateTime), CAST(N'2018-03-29T07:00:48.010' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (187, 2, N'Tax Form 1120S', 187, CAST(N'2018-03-29T07:00:48.010' AS DateTime), CAST(N'2018-03-29T07:00:48.010' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (188, 2, N'Tax Form 1120SF', 188, CAST(N'2018-03-29T07:00:48.010' AS DateTime), CAST(N'2018-03-29T07:00:48.010' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (189, 2, N'Tax Form 2106', 189, CAST(N'2018-03-29T07:00:48.013' AS DateTime), CAST(N'2018-03-29T07:00:48.013' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (190, 2, N'Tax Form 2106EZ', 190, CAST(N'2018-03-29T07:00:48.013' AS DateTime), CAST(N'2018-03-29T07:00:48.013' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (191, 2, N'Tax Form 4797', 191, CAST(N'2018-03-29T07:00:48.013' AS DateTime), CAST(N'2018-03-29T07:00:48.013' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (192, 2, N'Tax Form 6252', 192, CAST(N'2018-03-29T07:00:48.013' AS DateTime), CAST(N'2018-03-29T07:00:48.013' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (193, 2, N'Tax Information Sheet', 193, CAST(N'2018-03-29T07:00:48.013' AS DateTime), CAST(N'2018-03-29T07:00:48.013' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (194, 2, N'Tax Returns Transcripts', 194, CAST(N'2018-03-29T07:00:48.013' AS DateTime), CAST(N'2018-03-29T07:00:48.013' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (195, 2, N'Termite Inspection Report', 195, CAST(N'2018-03-29T07:00:48.017' AS DateTime), CAST(N'2018-03-29T07:00:48.017' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (196, 2, N'Texas Cash Out T50-A6 Equity Loan', 196, CAST(N'2018-03-29T07:00:48.017' AS DateTime), CAST(N'2018-03-29T07:00:48.017' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (197, 2, N'Texas Cash Out T50-A6 Right to Cancel', 197, CAST(N'2018-03-29T07:00:48.017' AS DateTime), CAST(N'2018-03-29T07:00:48.017' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (198, 2, N'Texas Disclosure Form T-64', 198, CAST(N'2018-03-29T07:00:48.017' AS DateTime), CAST(N'2018-03-29T07:00:48.017' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (199, 2, N'Texas Home Equity Affidavit and Agreement (First Lien)', 199, CAST(N'2018-03-29T07:00:48.017' AS DateTime), CAST(N'2018-03-29T07:00:48.017' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (200, 2, N'Texas Home Equity Note (Fixed Rate - First Lien)', 200, CAST(N'2018-03-29T07:00:48.020' AS DateTime), CAST(N'2018-03-29T07:00:48.020' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (201, 2, N'Texas Home Equity Security Instrument (First Lien)', 201, CAST(N'2018-03-29T07:00:48.020' AS DateTime), CAST(N'2018-03-29T07:00:48.020' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (202, 2, N'Texas Owners Affidavit of Compliance', 202, CAST(N'2018-03-29T07:00:48.020' AS DateTime), CAST(N'2018-03-29T07:00:48.020' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (203, 2, N'Total Annual Loan Cost (TALC)', 203, CAST(N'2018-03-29T07:00:48.020' AS DateTime), CAST(N'2018-03-29T07:00:48.020' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (204, 2, N'UCDP Submission Summary Report (SSR) - Fannie Mae', 204, CAST(N'2018-03-29T07:00:48.020' AS DateTime), CAST(N'2018-03-29T07:00:48.020' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (205, 2, N'UCDP Submission Summary Report (SSR) - FreddieMac', 205, CAST(N'2018-03-29T07:00:48.020' AS DateTime), CAST(N'2018-03-29T07:00:48.020' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (206, 2, N'US Patriot Act', 206, CAST(N'2018-03-29T07:00:48.023' AS DateTime), CAST(N'2018-03-29T07:00:48.023' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (207, 2, N'USDA Certificate of Occupancy', 207, CAST(N'2018-03-29T07:00:48.023' AS DateTime), CAST(N'2018-03-29T07:00:48.023' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (208, 2, N'USDA Conditional Commitment 1980-18', 208, CAST(N'2018-03-29T07:00:48.023' AS DateTime), CAST(N'2018-03-29T07:00:48.023' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (209, 2, N'USDA Lender Cerfification for SFH Guaranteed Loan', 209, CAST(N'2018-03-29T07:00:48.023' AS DateTime), CAST(N'2018-03-29T07:00:48.023' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (210, 2, N'USDA Loan Note Guarantee', 210, CAST(N'2018-03-29T07:00:48.023' AS DateTime), CAST(N'2018-03-29T07:00:48.023' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (211, 2, N'USDA Property Eligibility', 211, CAST(N'2018-03-29T07:00:48.027' AS DateTime), CAST(N'2018-03-29T07:00:48.027' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (212, 2, N'USDA Request for SFH Loan Guarantee 3555-21', 212, CAST(N'2018-03-29T07:00:48.027' AS DateTime), CAST(N'2018-03-29T07:00:48.027' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (213, 2, N'USDA SFH Income Eligibility', 213, CAST(N'2018-03-29T07:00:48.027' AS DateTime), CAST(N'2018-03-29T07:00:48.027' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (214, 2, N'VA Addendum to Application 26-1802A', 214, CAST(N'2018-03-29T07:00:48.027' AS DateTime), CAST(N'2018-03-29T07:00:48.027' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (215, 2, N'VA Appraisal Request Form 26-1805', 215, CAST(N'2018-03-29T07:00:48.027' AS DateTime), CAST(N'2018-03-29T07:00:48.027' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (216, 2, N'VA Certificate of Eligibility', 216, CAST(N'2018-03-29T07:00:48.030' AS DateTime), CAST(N'2018-03-29T07:00:48.030' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (217, 2, N'VA Child Care Statement', 217, CAST(N'2018-03-29T07:00:48.030' AS DateTime), CAST(N'2018-03-29T07:00:48.030' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (218, 2, N'VA Collection Policy Notice', 218, CAST(N'2018-03-29T07:00:48.030' AS DateTime), CAST(N'2018-03-29T07:00:48.030' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (219, 2, N'VA Common Certifications', 219, CAST(N'2018-03-29T07:00:48.030' AS DateTime), CAST(N'2018-03-29T07:00:48.030' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (220, 2, N'VA Compliance Inspection Report 26-1839', 220, CAST(N'2018-03-29T07:00:48.030' AS DateTime), CAST(N'2018-03-29T07:00:48.030' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (221, 2, N'VA Counseling Checklist 26-0592', 221, CAST(N'2018-03-29T07:00:48.030' AS DateTime), CAST(N'2018-03-29T07:00:48.030' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (222, 2, N'VA DD214 and Statement of Service', 222, CAST(N'2018-03-29T07:00:48.030' AS DateTime), CAST(N'2018-03-29T07:00:48.030' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (223, 2, N'VA Debt Questionnaire 26-0551', 223, CAST(N'2018-03-29T07:00:48.033' AS DateTime), CAST(N'2018-03-29T07:00:48.033' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (224, 2, N'VA Fixed Rate Mortgage Loan Disclosure', 224, CAST(N'2018-03-29T07:00:48.033' AS DateTime), CAST(N'2018-03-29T07:00:48.033' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (225, 2, N'VA Funding Fee Notice', 225, CAST(N'2018-03-29T07:00:48.033' AS DateTime), CAST(N'2018-03-29T07:00:48.033' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (226, 2, N'VA IRRRL Worksheet 26-8923', 226, CAST(N'2018-03-29T07:00:48.033' AS DateTime), CAST(N'2018-03-29T07:00:48.033' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (227, 2, N'VA Loan Comparison', 227, CAST(N'2018-03-29T07:00:48.033' AS DateTime), CAST(N'2018-03-29T07:00:48.033' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (228, 2, N'VA Loan Guaranty Certificate (26-1899)', 228, CAST(N'2018-03-29T07:00:48.037' AS DateTime), CAST(N'2018-03-29T07:00:48.037' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (229, 2, N'VA Loan Summary Sheet 26-0286', 229, CAST(N'2018-03-29T07:00:48.037' AS DateTime), CAST(N'2018-03-29T07:00:48.037' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (230, 2, N'VA National Guard Reservist Certification', 230, CAST(N'2018-03-29T07:00:48.037' AS DateTime), CAST(N'2018-03-29T07:00:48.037' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (231, 2, N'VA Nearest Living Relative', 231, CAST(N'2018-03-29T07:00:48.037' AS DateTime), CAST(N'2018-03-29T07:00:48.037' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (232, 2, N'VA Notice of Value', 232, CAST(N'2018-03-29T07:00:48.037' AS DateTime), CAST(N'2018-03-29T07:00:48.037' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (233, 2, N'VA Report and Cert of Loan Disbursement 26-1820', 233, CAST(N'2018-03-29T07:00:48.040' AS DateTime), CAST(N'2018-03-29T07:00:48.040' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (234, 2, N'VA Request for COE 26-1880', 234, CAST(N'2018-03-29T07:00:48.040' AS DateTime), CAST(N'2018-03-29T07:00:48.040' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (235, 2, N'VA Verification Benefit Related Indebtedness 26-8937', 235, CAST(N'2018-03-29T07:00:48.040' AS DateTime), CAST(N'2018-03-29T07:00:48.040' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (236, 2, N'W9', 236, CAST(N'2018-03-29T07:00:48.040' AS DateTime), CAST(N'2018-03-29T07:00:48.040' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (237, 2, N'Wire Funding Detail', 237, CAST(N'2018-03-29T07:00:48.040' AS DateTime), CAST(N'2018-03-29T07:00:48.040' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (238, 2, N'3rd Party Employer Lookup', 238, CAST(N'2018-03-29T07:00:48.040' AS DateTime), CAST(N'2018-03-29T07:00:48.040' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (239, 2, N'Appraisal Related Docs', 239, CAST(N'2018-03-29T07:00:48.040' AS DateTime), CAST(N'2018-03-29T07:00:48.040' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (240, 2, N'Assignment of Mortgage', 240, CAST(N'2018-03-29T07:00:48.043' AS DateTime), CAST(N'2018-03-29T07:00:48.043' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (241, 2, N'Builder Warranty', 241, CAST(N'2018-03-29T07:00:48.043' AS DateTime), CAST(N'2018-03-29T07:00:48.043' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (242, 2, N'CONDO-PUD HOA Master Insurance', 242, CAST(N'2018-03-29T07:00:48.043' AS DateTime), CAST(N'2018-03-29T07:00:48.043' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (243, 2, N'DAP Eligibility Certificate', 243, CAST(N'2018-03-29T07:00:48.043' AS DateTime), CAST(N'2018-03-29T07:00:48.043' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (244, 2, N'DAP Lender Certification', 244, CAST(N'2018-03-29T07:00:48.047' AS DateTime), CAST(N'2018-03-29T07:00:48.047' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (245, 2, N'EarlyChecks', 245, CAST(N'2018-03-29T07:00:48.047' AS DateTime), CAST(N'2018-03-29T07:00:48.047' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (246, 2, N'FACT Act Disclosure', 246, CAST(N'2018-03-29T07:00:48.047' AS DateTime), CAST(N'2018-03-29T07:00:48.047' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (247, 2, N'FHA Real Estate Certification', 247, CAST(N'2018-03-29T07:00:48.047' AS DateTime), CAST(N'2018-03-29T07:00:48.047' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (248, 2, N'FHA Request Form', 248, CAST(N'2018-03-29T07:00:48.047' AS DateTime), CAST(N'2018-03-29T07:00:48.047' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (249, 2, N'For Your Protection Home Inspection (92564CN)', 249, CAST(N'2018-03-29T07:00:48.050' AS DateTime), CAST(N'2018-03-29T07:00:48.050' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (250, 2, N'Form 1099 G', 250, CAST(N'2018-03-29T07:00:48.050' AS DateTime), CAST(N'2018-03-29T07:00:48.050' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (251, 2, N'Grant Deed', 251, CAST(N'2018-03-29T07:00:48.050' AS DateTime), CAST(N'2018-03-29T07:00:48.050' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (252, 2, N'HECM Federal Truth-In-Lending Loan Closing Disclosure Statement', 252, CAST(N'2018-03-29T07:00:48.050' AS DateTime), CAST(N'2018-03-29T07:00:48.050' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (253, 2, N'HECM Residential Loan Application for Reverse Mortgages 1009', 253, CAST(N'2018-03-29T07:00:48.050' AS DateTime), CAST(N'2018-03-29T07:00:48.050' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (254, 2, N'Immigration Documents', 254, CAST(N'2018-03-29T07:00:48.050' AS DateTime), CAST(N'2018-03-29T07:00:48.050' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (255, 2, N'Initial Closing Disclosures', 255, CAST(N'2018-03-29T07:00:48.050' AS DateTime), CAST(N'2018-03-29T07:00:48.050' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (256, 2, N'Interspousal Deed', 256, CAST(N'2018-03-29T07:00:48.053' AS DateTime), CAST(N'2018-03-29T07:00:48.053' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (257, 2, N'Investment Statements', 257, CAST(N'2018-03-29T07:00:48.053' AS DateTime), CAST(N'2018-03-29T07:00:48.053' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (258, 2, N'Invoice', 258, CAST(N'2018-03-29T07:00:48.053' AS DateTime), CAST(N'2018-03-29T07:00:48.053' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (259, 2, N'LDP List', 259, CAST(N'2018-03-29T07:00:48.053' AS DateTime), CAST(N'2018-03-29T07:00:48.053' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (260, 2, N'Legal Description', 260, CAST(N'2018-03-29T07:00:48.053' AS DateTime), CAST(N'2018-03-29T07:00:48.053' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (261, 2, N'Misc 203K Docs', 261, CAST(N'2018-03-29T07:00:48.057' AS DateTime), CAST(N'2018-03-29T07:00:48.057' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (262, 2, N'Misc Application Documents', 262, CAST(N'2018-03-29T07:00:48.057' AS DateTime), CAST(N'2018-03-29T07:00:48.057' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (263, 2, N'Misc Assets', 263, CAST(N'2018-03-29T07:00:48.057' AS DateTime), CAST(N'2018-03-29T07:00:48.057' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (264, 2, N'Misc Checklists', 264, CAST(N'2018-03-29T07:00:48.057' AS DateTime), CAST(N'2018-03-29T07:00:48.057' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (265, 2, N'Misc Closing Documents', 265, CAST(N'2018-03-29T07:00:48.057' AS DateTime), CAST(N'2018-03-29T07:00:48.057' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (266, 2, N'Misc Compliance', 266, CAST(N'2018-03-29T07:00:48.060' AS DateTime), CAST(N'2018-03-29T07:00:48.060' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (267, 2, N'Misc Credit', 267, CAST(N'2018-03-29T07:00:48.060' AS DateTime), CAST(N'2018-03-29T07:00:48.060' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (268, 2, N'Misc DAP Bond Docs', 268, CAST(N'2018-03-29T07:00:48.060' AS DateTime), CAST(N'2018-03-29T07:00:48.060' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (269, 2, N'Misc Disclosures', 269, CAST(N'2018-03-29T07:00:48.060' AS DateTime), CAST(N'2018-03-29T07:00:48.060' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (270, 2, N'Misc FHA Government Docs', 270, CAST(N'2018-03-29T07:00:48.060' AS DateTime), CAST(N'2018-03-29T07:00:48.060' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (271, 2, N'Misc HECM Docs', 271, CAST(N'2018-03-29T07:00:48.060' AS DateTime), CAST(N'2018-03-29T07:00:48.060' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (272, 2, N'Misc Income', 272, CAST(N'2018-03-29T07:00:48.060' AS DateTime), CAST(N'2018-03-29T07:00:48.060' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (273, 2, N'Misc Property', 273, CAST(N'2018-03-29T07:00:48.063' AS DateTime), CAST(N'2018-03-29T07:00:48.063' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (274, 2, N'Misc Title', 274, CAST(N'2018-03-29T07:00:48.063' AS DateTime), CAST(N'2018-03-29T07:00:48.063' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (275, 2, N'Misc VA Docs', 275, CAST(N'2018-03-29T07:00:48.063' AS DateTime), CAST(N'2018-03-29T07:00:48.063' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (276, 2, N'Mortgage Rider 2nd Home', 276, CAST(N'2018-03-29T07:00:48.063' AS DateTime), CAST(N'2018-03-29T07:00:48.063' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (277, 2, N'Mortgage Rider Arm Rate', 277, CAST(N'2018-03-29T07:00:48.063' AS DateTime), CAST(N'2018-03-29T07:00:48.063' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (278, 2, N'Mortgage Statement', 278, CAST(N'2018-03-29T07:00:48.063' AS DateTime), CAST(N'2018-03-29T07:00:48.063' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (279, 2, N'Note 2nd', 279, CAST(N'2018-03-29T07:00:48.067' AS DateTime), CAST(N'2018-03-29T07:00:48.067' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (280, 2, N'Pension Income', 280, CAST(N'2018-03-29T07:00:48.067' AS DateTime), CAST(N'2018-03-29T07:00:48.067' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (281, 2, N'Plat or Survey', 281, CAST(N'2018-03-29T07:00:48.067' AS DateTime), CAST(N'2018-03-29T07:00:48.067' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (282, 2, N'Power of Attorney', 282, CAST(N'2018-03-29T07:00:48.067' AS DateTime), CAST(N'2018-03-29T07:00:48.067' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (283, 2, N'Receipt for Deposit', 283, CAST(N'2018-03-29T07:00:48.067' AS DateTime), CAST(N'2018-03-29T07:00:48.067' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (284, 2, N'Revised Loan Estimates', 284, CAST(N'2018-03-29T07:00:48.070' AS DateTime), CAST(N'2018-03-29T07:00:48.070' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (285, 2, N'Road Maintenance Agreement', 285, CAST(N'2018-03-29T07:00:48.070' AS DateTime), CAST(N'2018-03-29T07:00:48.070' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (286, 2, N'Settlement Statement', 286, CAST(N'2018-03-29T07:00:48.070' AS DateTime), CAST(N'2018-03-29T07:00:48.070' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (287, 2, N'Signature', 287, CAST(N'2018-03-29T07:00:48.070' AS DateTime), CAST(N'2018-03-29T07:00:48.070' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (288, 2, N'Surplus Documents', 288, CAST(N'2018-03-29T07:00:48.070' AS DateTime), CAST(N'2018-03-29T07:00:48.070' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (289, 2, N'Texas Cash Out Misc T50-A6', 289, CAST(N'2018-03-29T07:00:48.070' AS DateTime), CAST(N'2018-03-29T07:00:48.070' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (290, 2, N'Title Doc - Cover Page', 290, CAST(N'2018-03-29T07:00:48.073' AS DateTime), CAST(N'2018-03-29T07:00:48.073' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (291, 2, N'Title Doc - Schedule A', 291, CAST(N'2018-03-29T07:00:48.073' AS DateTime), CAST(N'2018-03-29T07:00:48.073' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (292, 2, N'Title Doc - Schedule B', 292, CAST(N'2018-03-29T07:00:48.073' AS DateTime), CAST(N'2018-03-29T07:00:48.073' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (293, 2, N'Undisclosed Debt Monitoring', 293, CAST(N'2018-03-29T07:00:48.073' AS DateTime), CAST(N'2018-03-29T07:00:48.073' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (294, 2, N'Unknown', 294, CAST(N'2018-03-29T07:00:48.073' AS DateTime), CAST(N'2018-03-29T07:00:48.073' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (295, 2, N'Unsigned Documents', 295, CAST(N'2018-03-29T07:00:48.073' AS DateTime), CAST(N'2018-03-29T07:00:48.073' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (296, 2, N'USDA New Construction Soil Treatment Record NPCA99B', 296, CAST(N'2018-03-29T07:00:48.077' AS DateTime), CAST(N'2018-03-29T07:00:48.077' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (297, 2, N'USDA Notice of Applicants', 297, CAST(N'2018-03-29T07:00:48.077' AS DateTime), CAST(N'2018-03-29T07:00:48.077' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (298, 2, N'USPS Zip Code Lookup', 298, CAST(N'2018-03-29T07:00:48.077' AS DateTime), CAST(N'2018-03-29T07:00:48.077' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (299, 2, N'Warranty Deed', 299, CAST(N'2018-03-29T07:00:48.077' AS DateTime), CAST(N'2018-03-29T07:00:48.077' AS DateTime))
GO
INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] ([ID], [ConfigID], [DocumentName], [SequenceNumber], [CreatedON], [ModifiedON]) VALUES (300, 2, N'Wiring Instructions', 300, CAST(N'2018-03-29T07:00:48.077' AS DateTime), CAST(N'2018-03-29T07:00:48.077' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[MTS.DOCUMENT_STACKING_ORDER] OFF
GO

