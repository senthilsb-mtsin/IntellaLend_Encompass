/*
Missing Index Details from DocumentClassificationSummary_Report_Prasad.sql - DESKTOP-7CT072B\SQL2017.IntellaLend_Reporting (AzureAD\PrasadSubramaniam (56))
The Query Processor estimates that implementing the following index could improve the query cost by 49.6168%.
*/


USE [IntellaLend_Reporting]
GO
CREATE NONCLUSTERED INDEX [PAGES_BI_DDP]
ON [dbo].[PAGES] ([BATCH_INSTANCEID])
INCLUDE ([PAGEID],[DOCID],[DOCTYPE],[PATTERN])
GO

