/****** Object:  Table [dbo].[auditdescriptions]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[auditdescriptions](
	[auditid] [int] NOT NULL,
	[auditname] [varchar](max) NOT NULL,
	[auditdatecreated] [smalldatetime] NOT NULL,
	[auditcreatedby] [varchar](max) NOT NULL,
	[audittype] [varchar](max) NOT NULL,
	[auditstatus] [bit] NOT NULL,
	[auditbegindate] [smalldatetime] NOT NULL,
	[auditenddate] [smalldatetime] NOT NULL,
	[auditcustomtext1] [varchar](max) NOT NULL,
	[auditprefundyn] [bit] NOT NULL,
	[auditMgmtStatus] [char](1) NOT NULL,
	[audit2webfolderYN] [bit] NOT NULL,
	[defaultquestionresponse] [varchar](max) NOT NULL,
	[pipelinepriority] [tinyint] NOT NULL,
	[pipelineduedate] [smalldatetime] NOT NULL,
	[pipelinecompletedyn] [bit] NOT NULL,
	[pipelinestatus] [tinyint] NOT NULL,
	[customerosclientid] [int] NOT NULL,
	[pipelinecriticalyn] [bit] NOT NULL,
	[exceptionfunctionalityyn] [bit] NOT NULL,
	[excludetrendingreports] [bit] NOT NULL,
	[Comments] [varchar](max) NULL,
	[AuditTrendingType] [int] NULL,
	[fundingtapereceiveddate] [smalldatetime] NOT NULL,
	[randomselectionsentdate] [smalldatetime] NOT NULL,
	[filesuploadeddate] [smalldatetime] NOT NULL,
	[duedate] [smalldatetime] NOT NULL,
	[managerapproveddate] [smalldatetime] NOT NULL,
	[auditcompletedate] [smalldatetime] NOT NULL,
	[lqrtdepartment] [varchar](max) NULL,
	[dateloansinitialized] [smalldatetime] NOT NULL,
	[auditbilleddate] [smalldatetime] NOT NULL,
	[auditselectedmonth] [varchar](max) NULL,
 CONSTRAINT [PK_auditid] PRIMARY KEY CLUSTERED 
(
	[auditid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[auditjunction]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[auditjunction](
	[auditjunctionid] [int] NOT NULL,
	[auditid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
 CONSTRAINT [PK_auditjunctionid] PRIMARY KEY CLUSTERED 
(
	[auditjunctionid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[auditpipelineStatus]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[auditpipelineStatus](
	[id] [int] NOT NULL,
	[auditid] [int] NULL,
	[isBilled] [bit] NULL,
	[pipelinestatus] [varchar](max) NULL,
	[pipelinestatusid] [int] NULL,
 CONSTRAINT [PK_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[customer]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[customer](
	[customerid] [int] NOT NULL,
	[processcustomerid] [int] NOT NULL,
	[customername] [varchar](max) NOT NULL,
	[samplesize] [decimal](18, 0) NOT NULL,
	[allowmanualloanentryyn] [bit] NOT NULL,
	[servicecreditreportyn] [bit] NOT NULL,
	[servicessnverificationyn] [bit] NOT NULL,
	[servicefraudcheckyn] [bit] NOT NULL,
	[servicevaulationmodelyn] [bit] NOT NULL,
	[servicetaxverificationyn] [bit] NOT NULL,
	[primarycreditreportagency] [varchar](max) NULL,
	[autolettersvoe] [bit] NULL,
	[autolettersvo2] [bit] NULL,
	[autolettersvo3] [bit] NULL,
	[contractenddate] [smalldatetime] NULL,
	[ccreatedby] [varchar](max) NULL,
	[cdatecreated] [smalldatetime] NULL,
	[clasteditedby] [varchar](max) NULL,
	[clastdateedited] [smalldatetime] NULL,
	[defaultauditid] [int] NOT NULL,
	[preclosecustomeryn] [bit] NOT NULL,
	[servicecomplianceyn] [bit] NOT NULL,
	[verificationaddress] [varchar](max) NOT NULL,
	[verificationcity] [varchar](max) NOT NULL,
	[verificationstate] [varchar](max) NOT NULL,
	[verificationpostalcode] [varchar](max) NOT NULL,
	[billunlimitedyn] [bit] NOT NULL,
	[billloansinpacket] [smallint] NOT NULL,
	[ipfilteringyn] [bit] NOT NULL,
	[allowdisplaybyclientyn] [bit] NOT NULL,
	[daysuntilverifalert] [tinyint] NOT NULL,
	[reportlogofilepath] [varchar](max) NOT NULL,
	[onlinereportaccessYN] [bit] NOT NULL,
	[allowdropdownYN] [bit] NOT NULL,
	[purchasedpremiumreportingYN] [bit] NOT NULL,
	[billdefaultdiscountpercent] [decimal](18, 0) NOT NULL,
	[allowloandeletionyn] [bit] NOT NULL,
	[ocrimageavailableyn] [bit] NOT NULL,
 CONSTRAINT [PK_customerid] PRIMARY KEY CLUSTERED 
(
	[customerid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[customerosclients]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[customerosclients](
	[customerosclientid] [int] NOT NULL,
	[customerid] [int] NOT NULL,
	[osclientname] [varchar](max) NOT NULL,
	[activeyn] [bit] NOT NULL,
	[LQRTImport] [bit] NOT NULL,
	[MaterialTargetRate] [int] NULL,
	[SLAtext] [varchar](max) NOT NULL,
	[alertRed] [tinyint] NOT NULL,
 CONSTRAINT [PK_customerosclientid] PRIMARY KEY CLUSTERED 
(
	[customerosclientid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lborrowerdata]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lborrowerdata](
	[borrowersystemid] [int] NOT NULL,
	[borrowerid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[sourcedocument] [int] NULL,
	[subdocumentid] [int] NULL,
	[firstname] [varchar](max) NULL,
	[middlename] [varchar](max) NULL,
	[lastname] [varchar](max) NULL,
	[namesuffix] [varchar](max) NULL,
	[socialsecuritynumber] [varbinary](1) NULL,
	[borrowerage] [tinyint] NULL,
	[borrowergendercode] [varchar](max) NULL,
	[borrowerracecode] [smallint] NULL,
	[curenthousingexpensemonthly] [money] NULL,
	[debtchildsupportmonthly] [money] NULL,
	[debtchildsupportpaidto] [varchar](max) NULL,
	[debtchildsupportunpaid] [money] NULL,
	[debtinstallmentmonthly] [money] NULL,
	[debtinstallmentunpaid] [money] NULL,
	[debtjobrelatedmonthly] [money] NULL,
	[debtothermonthly] [money] NULL,
	[debtotherunpaid] [money] NULL,
	[incomebaseemployment] [money] NULL,
	[incomebonuses] [money] NULL,
	[incomecommissions] [money] NULL,
	[incomedividendsinterest] [money] NULL,
	[incomenetrealestate] [money] NULL,
	[incomeotheramount] [money] NULL,
	[incomeotherdesc] [varchar](max) NULL,
	[incomeovertime] [money] NULL,
	[incomepositivecashflow] [money] NULL,
	[incometotal] [money] NULL,
	[vadeductionsfederaltax] [money] NULL,
	[vadeductionsotheramount] [money] NULL,
	[vadeductionsotherdesc] [varchar](max) NULL,
	[vadeductionsretiresecurity] [money] NULL,
	[vadeductionsstatetax] [money] NULL,
	[vadeductionstotal] [money] NULL,
	[vanettakehomepay] [money] NULL,
	[yearsatemployment] [tinyint] NULL,
	[yearsofeducation] [tinyint] NULL,
	[borrowercaivrnumber] [varchar](max) NULL,
	[documentdate] [smalldatetime] NULL,
	[boramountvalue3] [money] NULL,
	[boramountvalue2] [money] NULL,
	[boramountvalue4] [money] NULL,
	[ficotransunion] [smallint] NULL,
	[ficoequifax] [smallint] NULL,
	[ficoexperian] [smallint] NULL,
	[creditreportagency] [varchar](max) NULL,
	[creditreportdateissued] [smalldatetime] NULL,
	[creditreportalertstatus] [varchar](max) NULL,
	[currentemployer] [varchar](max) NULL,
	[bordatevalue1] [smalldatetime] NULL,
	[bordatevalue2] [smalldatetime] NULL,
	[boramountvalue1] [money] NULL,
	[bortextvalue1] [varchar](max) NULL,
	[incomepayfrequency] [varchar](max) NULL,
 CONSTRAINT [PK_borrowersystemid] PRIMARY KEY CLUSTERED 
(
	[borrowersystemid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lborrowerdataaddress]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lborrowerdataaddress](
	[borroweraddressid] [int] NOT NULL,
	[borrowerid] [int] NOT NULL,
	[sourcedocument] [int] NOT NULL,
	[presentaddressyn] [bit] NULL,
	[streetaddress] [varchar](max) NULL,
	[city] [varchar](max) NULL,
	[state] [varchar](max) NULL,
	[postalcode] [varchar](max) NULL,
	[propertyownedyn] [bit] NULL,
	[numberyearsresided] [decimal](18, 0) NULL,
 CONSTRAINT [PK_borroweraddressid] PRIMARY KEY CLUSTERED 
(
	[borroweraddressid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lborrowermaster]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lborrowermaster](
	[borrowerid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[borrowerordinate] [tinyint] NOT NULL,
	[displayfirstname] [varchar](max) NOT NULL,
	[displaylastname] [varchar](max) NOT NULL,
	[displaydocumentid] [int] NOT NULL,
	[borrowerincomeaudited] [bit] NOT NULL,
	[Borrweractiveyn] [bit] NOT NULL,
 CONSTRAINT [PK_borrowerid] PRIMARY KEY CLUSTERED 
(
	[borrowerid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lentitydata]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lentitydata](
	[entitydataid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[borrowerid] [int] NULL,
	[sourcedocument] [int] NOT NULL,
	[subdocumentid] [int] NULL,
	[entitytype] [varchar](max) NULL,
	[entitysubtype] [varchar](max) NULL,
	[entitycity] [varchar](max) NULL,
	[entitydescription] [varchar](max) NULL,
	[entitymonthlypayment] [money] NULL,
	[entitymonthslefttopay] [smallint] NULL,
	[entityname] [varchar](max) NULL,
	[entityphonenumber] [char](1) NULL,
	[entitypostalcode] [varchar](max) NULL,
	[entitystate] [varchar](max) NULL,
	[entitystreetaddress] [varchar](max) NULL,
	[entityvalueorbalance] [money] NULL,
	[identificationcode] [varbinary](1) NULL,
	[positiontitle] [varchar](max) NULL,
	[entitypercent] [decimal](18, 0) NULL,
	[secondvalueorbalance] [money] NULL,
	[amountpaidborrower] [money] NULL,
	[amountpaidseller] [money] NULL,
	[entityterms] [varchar](max) NULL,
	[entityvaluedatereported] [smalldatetime] NULL,
	[numberyearsatentity] [decimal](18, 0) NULL,
	[entitybitvalue] [bit] NULL,
	[entityamountvalue1] [money] NULL,
	[entityamountvalue2] [money] NULL,
	[entityamountvalue3] [money] NULL,
	[entitydatevalue1] [smalldatetime] NULL,
	[entitydatevalue2] [smalldatetime] NULL,
 CONSTRAINT [PK_entitydataid] PRIMARY KEY CLUSTERED 
(
	[entitydataid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lkeyloandata]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lkeyloandata](
	[lkeyloandataid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[sourcedocument] [int] NOT NULL,
	[appraisedpropertyvalue] [money] NULL,
	[loanamoritizationtype] [varchar](max) NULL,
	[loanamoritizationtypedesc] [varchar](max) NULL,
	[Loanprogram] [varchar](max) NOT NULL,
	[loanamount] [money] NULL,
	[loaninterestrate] [decimal](18, 0) NULL,
	[loanlienposition] [varchar](max) NULL,
	[loanpurpose] [int] NULL,
	[loanpurposeotherdesc] [varchar](max) NULL,
	[loanterminmonths] [smallint] NULL,
	[loantype] [varchar](max) NULL,
	[loancashdown] [money] NULL,
	[loanccbuyer] [money] NULL,
	[loanccseller] [money] NULL,
	[loancctotal] [money] NULL,
	[mifinancedamount] [money] NULL,
	[mortgageinsurancecoverage] [decimal](18, 0) NULL,
	[micertificatenumber] [varchar](max) NULL,
	[propertycounty] [varchar](max) NULL,
	[propertycity] [varchar](max) NULL,
	[propertyoccupancystatus] [varchar](max) NULL,
	[propertypostalcode] [varchar](max) NULL,
	[propertysalesprice] [money] NULL,
	[propertystate] [char](1) NULL,
	[propertystreetaddress] [varchar](max) NULL,
	[propertytype] [varchar](max) NULL,
	[ratiodebt] [decimal](18, 0) NULL,
	[ratiohousing] [decimal](18, 0) NULL,
	[ratioloantovalue] [decimal](18, 0) NULL,
	[ratiootherLTV] [decimal](18, 0) NULL,
	[propertyparcelno] [varchar](max) NULL,
	[propertyblockno] [varchar](max) NULL,
	[propertylotno] [varchar](max) NULL,
	[propertypinnumber] [varchar](max) NULL,
	[propertylegaldescother] [varchar](max) NULL,
	[loandatesettlement] [smalldatetime] NULL,
	[loandepositamount] [money] NULL,
	[loannotedate] [smalldatetime] NULL,
	[propertyprojclasstype] [varchar](max) NULL,
	[economiclifeyears] [decimal](18, 0) NULL,
	[combinedtotalincome] [money] NULL,
	[combinedtotalassets] [money] NULL,
	[loandatevalue1] [smalldatetime] NULL,
	[loanintegervalue1] [int] NULL,
	[loansellersname] [varchar](max) NULL,
	[docdatesigned] [smalldatetime] NULL,
	[amountvalue1] [money] NULL,
	[amountvalue2] [money] NULL,
	[loantextvalue1] [varchar](max) NULL,
	[loanbitvalue1] [bit] NULL,
	[docformnumber] [varchar](max) NULL,
	[docformversion] [varchar](max) NULL,
	[loandatevalue2] [smalldatetime] NULL,
	[loanmaturitydate] [smalldatetime] NULL,
	[loanfeesimpleyn] [bit] NOT NULL,
	[loancashreserves] [money] NULL,
	[loandiscountpoints] [decimal](18, 0) NULL,
	[PITI] [money] NULL,
	[Documentationtype] [varchar](max) NULL,
	[austype] [varchar](max) NULL,
	[fieldReviewChk] [bit] NULL,
 CONSTRAINT [PK_lkeyloandataid] PRIMARY KEY CLUSTERED 
(
	[lkeyloandataid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[loanmaster]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[loanmaster](
	[loanmasterid] [int] NOT NULL,
	[customerid] [int] NOT NULL,
	[customerloannumber] [varchar](max) NOT NULL,
	[primaryborrowerlastname] [varchar](max) NULL,
	[qproprocessdate] [smalldatetime] NULL,
	[qloantypeid] [int] NOT NULL,
	[customerprepost] [varchar](max) NOT NULL,
	[customerloanstatus] [varchar](max) NULL,
	[customerloanstatusdate] [smalldatetime] NULL,
	[customerloantype] [varchar](max) NULL,
	[regiondescription] [varchar](max) NULL,
	[branchofficestate] [char](1) NOT NULL,
	[branchoffice] [varchar](max) NULL,
	[originator] [varchar](max) NULL,
	[customerproducttype] [varchar](max) NULL,
	[broker] [varchar](max) NULL,
	[closingagent] [varchar](max) NULL,
	[funder] [varchar](max) NULL,
	[investor] [varchar](max) NULL,
	[loanofficer] [varchar](max) NULL,
	[processor] [varchar](max) NULL,
	[titlecompany] [varchar](max) NULL,
	[underwriter] [varchar](max) NULL,
	[lmcreatedby] [char](1) NULL,
	[lmdatecreated] [smalldatetime] NULL,
	[finalqprorating] [varchar](max) NULL,
	[qprostatus] [varchar](max) NULL,
	[generalcomment] [varchar](max) NULL,
	[businesssource] [varchar](max) NULL,
	[loanpurpose] [int] NOT NULL,
	[appraiser] [varchar](max) NOT NULL,
	[loantovalue] [decimal](18, 0) NOT NULL,
	[loanclosingdate] [smalldatetime] NOT NULL,
	[qprodatauploadid] [int] NOT NULL,
	[completeloanimportedyn] [bit] NOT NULL,
	[auditassetsincomeyn] [bit] NOT NULL,
	[underwriterapprovaldate] [smalldatetime] NOT NULL,
	[billinvoicerecordsid] [int] NOT NULL,
	[qprodateforbilling] [smalldatetime] NOT NULL,
	[qprocomplianceyn] [bit] NOT NULL,
	[billfilepackid] [int] NOT NULL,
	[loanislocked] [bit] NOT NULL,
	[subclientname] [varchar](max) NOT NULL,
	[settlementagent] [varchar](max) NOT NULL,
	[criticaloverrideyn] [bit] NOT NULL,
	[ausdecision] [varchar](max) NOT NULL,
	[pipelinestatus] [smallint] NOT NULL,
	[lientype] [varchar](max) NOT NULL,
	[brokerid] [varchar](max) NOT NULL,
	[lqrtstatus] [varchar](max) NOT NULL,
	[AssignedTo] [varchar](max) NULL,
	[austype] [varchar](max) NULL,
 CONSTRAINT [PK_loanmasterid] PRIMARY KEY CLUSTERED 
(
	[loanmasterid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[loverflowloandata]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[loverflowloandata](
	[loverflowloandataid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[sourcedocument] [int] NOT NULL,
	[alterationsamount] [money] NOT NULL,
	[improvementamount] [money] NOT NULL,
	[repairsamount] [money] NOT NULL,
	[landamount] [money] NOT NULL,
	[refinanceamount] [money] NOT NULL,
	[prepaiditemsamount] [money] NOT NULL,
	[closingcostamount] [money] NOT NULL,
	[pmifinancedamount] [money] NOT NULL,
	[discountamount] [money] NOT NULL,
	[subordinatefinancing] [money] NOT NULL,
	[bclosingcostspaidbyseller] [money] NOT NULL,
	[lendercreditamount] [money] NOT NULL,
	[fundstocloseamount] [money] NOT NULL,
	[perdiem] [money] NOT NULL,
	[prepaidinterest] [money] NOT NULL,
	[hazardescrow] [money] NOT NULL,
	[taxescrow] [money] NOT NULL,
	[totalcosts] [money] NOT NULL,
	[pmimipfffamount] [money] NOT NULL,
	[overflowbitvalue] [bit] NOT NULL,
	[otherescrow] [money] NOT NULL,
	[Loanbaseamount] [money] NOT NULL,
	[OtherCredits] [money] NULL,
 CONSTRAINT [PK_loverflowloandataid] PRIMARY KEY CLUSTERED 
(
	[loverflowloandataid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lpipelinedata]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lpipelinedata](
	[lpipelinedataid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[pipelinestatus] [smallint] NOT NULL,
	[statusdate] [smalldatetime] NOT NULL,
	[statusqprouserid] [int] NOT NULL,
	[statuscompletedyn] [bit] NOT NULL,
 CONSTRAINT [PK_lpipelinedataid] PRIMARY KEY CLUSTERED 
(
	[lpipelinedataid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lpropertyloandata]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lpropertyloandata](
	[propertyloandataid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[sourcedocument] [int] NOT NULL,
	[documentbarcode] [int] NULL,
	[estexpenseallotherpayments] [money] NULL,
	[estexpensefirstmortgage] [money] NULL,
	[estexpensehazardinsurance] [money] NULL,
	[estexpensehomeownerdues] [money] NULL,
	[estexpenseleasegroundrent] [money] NULL,
	[estexpensemaintenance] [money] NULL,
	[estexpensemortgageinsurance] [money] NULL,
	[estexpensenegcashflow] [money] NULL,
	[estexpenseotherfinancing] [money] NULL,
	[estexpenseotherhousing] [money] NULL,
	[estexpenseotherhousingdesc] [varchar](max) NULL,
	[estexpenserealeastatetaxes] [money] NULL,
	[estexpensespecialassessments] [money] NULL,
	[estexpensetotal] [money] NULL,
	[estexpensetotalhousing] [money] NULL,
	[estexpenseutilities] [money] NULL,
	[sectionofhousingact] [varchar](max) NULL,
	[loanitem1desc] [varchar](max) NULL,
	[loanitem1amount] [money] NULL,
	[loanitem2desc] [varchar](max) NULL,
	[loanitem2amount] [money] NULL,
	[loanitem3desc] [varchar](max) NULL,
	[loanitem3amount] [money] NULL,
	[loanitem4desc] [varchar](max) NULL,
	[loanitem4amount] [money] NULL,
	[loanitem5desc] [varchar](max) NULL,
	[loanitem5amount] [money] NULL,
	[loanitem6desc] [varchar](max) NULL,
	[loanitem6amount] [money] NULL,
	[loantotalsetborrower] [money] NULL,
	[loantotalsetseller] [money] NULL,
	[mortgageinsurercode] [varchar](max) NULL,
	[loanunderwitername] [varchar](max) NULL,
	[loanappraisername] [varchar](max) NULL,
	[loanappraisalcompany] [varchar](max) NULL,
	[combinedtotalrealestate] [money] NULL,
	[debttotalbalancechildsup] [money] NULL,
	[debttotalbalanceother] [money] NULL,
	[debttotalbalancetotal] [money] NULL,
	[debttotalbalanceinstall] [money] NULL,
	[debttotalmopaychildsup] [money] NULL,
	[debttotalmopayother] [money] NULL,
	[debttotalmopaytotal] [money] NULL,
	[debttotalmopayinstall] [money] NULL,
	[cashinprepaidexpenses] [money] NULL,
	[cashinrepairs] [money] NULL,
	[cashinnonrealityother] [money] NULL,
	[cashinamtgift] [money] NULL,
	[cashin2mortgage] [money] NULL,
	[cashinufmipcash] [money] NULL,
	[interviewcommunicationmode] [varchar](max) NULL,
	[interviewname] [varchar](max) NULL,
	[loanintegervalue1] [int] NULL,
	[combincomepositivecashflow] [money] NULL,
	[propertyloanbitvalue1] [bit] NOT NULL,
	[loanitem7amount] [money] NULL,
	[loanitem8amount] [money] NULL,
	[loanitem9amount] [money] NULL,
	[propertyloandecimalvalue1] [decimal](18, 0) NULL,
	[assetlifeinsurancefaceamount] [money] NULL,
	[assetlifeinsurancevalue] [money] NULL,
	[assetstotalliquid] [money] NULL,
	[cashdepositamount] [money] NULL,
 CONSTRAINT [PK_propertyloandataid] PRIMARY KEY CLUSTERED 
(
	[propertyloandataid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lreportingdata]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lreportingdata](
	[lreportingdataid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[amortizationtype] [varchar](max) NOT NULL,
	[appraiser] [varchar](max) NOT NULL,
	[appraisedvalue] [money] NOT NULL,
	[branchoffice] [varchar](max) NOT NULL,
	[branchofficestate] [char](1) NOT NULL,
	[broker] [varchar](max) NOT NULL,
	[businesssource] [varchar](max) NOT NULL,
	[settlementdate] [smalldatetime] NOT NULL,
	[disbursementdate] [smalldatetime] NOT NULL,
	[closingagent] [varchar](max) NOT NULL,
	[ltv] [decimal](18, 0) NOT NULL,
	[cltv] [decimal](18, 0) NOT NULL,
	[ratiodebt] [decimal](18, 0) NOT NULL,
	[ratiohousing] [decimal](18, 0) NOT NULL,
	[funder] [varchar](max) NOT NULL,
	[fundstoclose] [money] NOT NULL,
	[investor] [varchar](max) NOT NULL,
	[loanpurpose] [int] NOT NULL,
	[loantype] [varchar](max) NOT NULL,
	[loanamount] [money] NOT NULL,
	[loaninterestrate] [decimal](18, 0) NOT NULL,
	[loanofficer] [varchar](max) NOT NULL,
	[loanprogram] [varchar](max) NOT NULL,
	[monthlypiamount] [money] NOT NULL,
	[monthlymipayment] [money] NOT NULL,
	[originator] [varchar](max) NOT NULL,
	[processor] [varchar](max) NOT NULL,
	[salesprice] [money] NOT NULL,
	[settlementagent] [varchar](max) NOT NULL,
	[subclientname] [varchar](max) NOT NULL,
	[titlecompany] [varchar](max) NOT NULL,
	[underwriter] [varchar](max) NOT NULL,
	[documentationtype] [varchar](max) NOT NULL,
	[datecreated] [smalldatetime] NOT NULL,
	[propertytype] [varchar](max) NOT NULL,
	[ausdecision] [varchar](max) NOT NULL,
	[nounits] [tinyint] NOT NULL,
 CONSTRAINT [PK_lreportingdataid] PRIMARY KEY CLUSTERED 
(
	[lreportingdataid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lservicestoragecompliance]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lservicestoragecompliance](
	[lservicestoragecomplianceid] [int] NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[datecreated] [smalldatetime] NOT NULL,
	[requestXML] [text] NOT NULL,
	[reportXML] [text] NOT NULL,
	[ceID] [varchar](max) NOT NULL,
	[calculatedApr] [decimal](18, 0) NOT NULL,
	[riskIndicator] [varchar](max) NOT NULL,
	[reporttimeStamp] [varchar](max) NOT NULL,
 CONSTRAINT [PK_lservicestoragecomplianceid] PRIMARY KEY CLUSTERED 
(
	[lservicestoragecomplianceid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[luploadbatchloans]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[luploadbatchloans](
	[luploadbatchloanid] [int] NOT NULL,
	[customerid] [int] NOT NULL,
	[batchfileinfoid] [int] NOT NULL,
	[loanclosingdate] [smalldatetime] NOT NULL,
	[customerloannumber] [varchar](max) NOT NULL,
	[primaryborlastname] [varchar](max) NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[branchoffice] [varchar](max) NOT NULL,
	[branchofficestate] [varchar](max) NOT NULL,
	[appraisername] [varchar](max) NOT NULL,
	[loanofficer] [varchar](max) NOT NULL,
	[broker] [varchar](max) NOT NULL,
	[loantovalue] [decimal](18, 0) NOT NULL,
	[loaninterestrate] [decimal](18, 0) NOT NULL,
	[underwriter] [varchar](max) NOT NULL,
	[processor] [varchar](max) NOT NULL,
	[subclientname] [varchar](max) NOT NULL,
	[loantype] [varchar](max) NOT NULL,
	[loanpurpose] [int] NOT NULL,
	[loanpurposedesc] [varchar](max) NOT NULL,
	[occupancytype] [varchar](max) NOT NULL,
	[amortizationtype] [varchar](max) NOT NULL,
	[propertytype] [varchar](max) NOT NULL,
	[originator] [varchar](max) NOT NULL,
	[region] [varchar](max) NOT NULL,
	[closingagent] [varchar](max) NOT NULL,
	[funder] [varchar](max) NOT NULL,
	[investor] [varchar](max) NOT NULL,
	[titlecompany] [varchar](max) NOT NULL,
	[businesschannel] [varchar](max) NOT NULL,
	[settlementagent] [varchar](max) NOT NULL,
	[housingratio] [decimal](18, 0) NOT NULL,
	[debtratio] [decimal](18, 0) NOT NULL,
	[loanamount] [money] NOT NULL,
	[documentationtype] [varchar](max) NOT NULL,
	[loanprogram] [varchar](max) NOT NULL,
	[clientloanstatus] [varchar](max) NOT NULL,
	[ausdecision] [varchar](max) NOT NULL,
	[borrower1age] [tinyint] NOT NULL,
	[borrower1racecode] [smallint] NOT NULL,
	[borrower1ficoscore] [smallint] NOT NULL,
	[borrower1gender] [varchar](max) NOT NULL,
	[borrower1ethnicity] [smallint] NOT NULL,
	[borrower2age] [tinyint] NOT NULL,
	[borrower2racecode] [smallint] NOT NULL,
	[borrower2ficoscore] [smallint] NOT NULL,
	[borrower2gender] [varchar](max) NOT NULL,
	[borrower2ethnicity] [smallint] NOT NULL,
	[brokerid] [varchar](max) NOT NULL,
 CONSTRAINT [PK_luploadbatchloanid] PRIMARY KEY CLUSTERED 
(
	[luploadbatchloanid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[refcodes]    Script Date: 10/26/2017 1:47:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[refcodes](
	[refcodeid] [int] NOT NULL,
	[refcodetypeid] [int] NOT NULL,
	[refcodetext] [varchar](max) NOT NULL,
	[refcodeorder] [smallint] NULL,
	[refcodesubtext] [nvarchar](max) NULL,
	[refcodesubtype] [int] NULL,
	[refcodeactiveyn] [bit] NOT NULL,
	[refcodecategoryid] [int] NOT NULL,
	[refcodecompliance] [varchar](max) NOT NULL,
	[preclosereplacementdocid] [int] NOT NULL,
	[verificationdocyn] [bit] NOT NULL,
	[excludefromdocdisplayyn] [bit] NOT NULL,
 CONSTRAINT [PK_refcodeid] PRIMARY KEY CLUSTERED 
(
	[refcodeid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
