GO
/****** Object:  Table [dbo].[lappraisaldata]    Script Date: 11/16/2017 3:02:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lappraisaldata](
	[appraisaldataid] [int] IDENTITY(1,1) NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[sourcedocument] [int] NOT NULL,
	[comp1adjsalesprice] [money] NULL,
	[comp1dateofsales] [smalldatetime] NULL,
	[comp1grosspercent] [decimal](6, 4) NULL,
	[comp1livingareasqft] [int] NULL,
	[comp1netadjamount] [money] NULL,
	[comp1netpercent] [decimal](6, 4) NULL,
	[comp1proximity] [varchar](50) NULL,
	[comp1salesprice] [int] NULL,
	[comp2adjsalesprice] [money] NULL,
	[comp2dateofsales] [smalldatetime] NULL,
	[comp2grosspercent] [decimal](6, 4) NULL,
	[comp2livingareasqft] [int] NULL,
	[comp2netadjamount] [money] NULL,
	[comp2netpercent] [decimal](6, 4) NULL,
	[comp2proximity] [varchar](50) NULL,
	[comp2salesprice] [int] NULL,
	[comp3adjsalesprice] [money] NULL,
	[comp3dateofsales] [smalldatetime] NULL,
	[comp3grosspercent] [decimal](6, 4) NULL,
	[comp3livingareasqft] [int] NULL,
	[comp3netadjamount] [money] NULL,
	[comp3netpercent] [decimal](6, 4) NULL,
	[comp3proximity] [varchar](50) NULL,
	[comp3salesprice] [int] NULL,
	[datereconcilation] [smalldatetime] NULL,
	[locationtype] [varchar](15) NULL,
	[neighborhooddemand] [varchar](15) NULL,
	[neighborhoodgrowthrate] [varchar](15) NULL,
	[percentlanduseother] [decimal](6, 3) NULL,
	[percentlanduse1family] [decimal](6, 3) NULL,
	[percentlanduse24family] [decimal](6, 3) NULL,
	[percentlandusecommercial] [decimal](6, 3) NULL,
	[percentlandusemultifamily] [decimal](6, 3) NULL,
	[percentpropertybuiltup] [varchar](15) NULL,
	[propertyrightsappraised] [varchar](32) NULL,
	[propertyvaluetrend] [varchar](15) NULL,
	[subjectdesignstyles] [varchar](24) NULL,
	[subjectdetached] [bit] NULL,
	[subjecteffectiveageyears] [varchar](12) NULL,
	[subjectexisting] [bit] NULL,
	[subjectindicatedvalue] [money] NULL,
	[subjectnoofstories] [tinyint] NULL,
	[subjectnoofunits] [tinyint] NULL,
	[subjectroomcountbaths] [decimal](4, 2) NULL,
	[subjectroomcountbedrooms] [decimal](4, 2) NULL,
	[subjectroomcounttotal] [decimal](4, 2) NULL,
	[subjectlivingareasqft] [int] NULL,
	[subjectyearbuilt] [int] NULL,
	[timetosellproperty] [varchar](32) NULL,
	[FEMAfloodareayn] [bit] NULL,
	[FEMAzone] [varchar](5) NULL,
	[FEMAmapdate] [smalldatetime] NULL,
	[FEMAmapnumber] [varchar](50) NULL,
	[appraisercertificationlicense] [varchar](50) NULL,
	[appraiserstateissued] [varchar](50) NULL,
	[appraiserlicensedate] [smalldatetime] NULL,
	[comp1roomcountbaths] [decimal](4, 2) NULL,
	[comp1roomcountbedrooms] [decimal](4, 2) NULL,
	[comp1roomcounttotal] [decimal](4, 2) NULL,
	[comp2roomcountbaths] [decimal](4, 2) NULL,
	[comp2roomcountbedrooms] [decimal](4, 2) NULL,
	[comp2roomcounttotal] [decimal](4, 2) NULL,
	[comp3roomcountbaths] [decimal](4, 2) NULL,
	[comp3roomcountbedrooms] [decimal](4, 2) NULL,
	[comp3roomcounttotal] [decimal](4, 2) NULL,
	[subjectvaluationconfidence] [int] NULL,
	[subjectvaluationhighestimate] [money] NULL,
	[subjectvaluationlowestimate] [money] NULL,
	[subjectlotsize] [int] NULL,
	[subjectac] [bit] NULL,
	[subjectpool] [bit] NULL,
	[subjectgarage] [varchar](50) NULL,
	[comp1streetaddress] [varchar](150) NULL,
	[comp1usecode] [varchar](50) NULL,
	[comp2streetaddress] [varchar](120) NULL,
	[comp2usecode] [varchar](50) NULL,
	[comp3streetaddress] [varchar](150) NULL,
	[comp3usecode] [varchar](50) NULL,
	[subjectusecode] [varchar](50) NULL,
	[comp1lotsize] [int] NULL,
	[comp1ac] [bit] NULL,
	[comp1pool] [bit] NULL,
	[comp1garage] [varchar](50) NULL,
	[comp2lotsize] [int] NULL,
	[comp2ac] [bit] NULL,
	[comp2pool] [bit] NULL,
	[comp2garage] [varchar](50) NULL,
	[comp3lotsize] [int] NULL,
	[comp3ac] [bit] NULL,
	[comp3pool] [bit] NULL,
	[comp3garage] [varchar](50) NULL,
	[comp1proximityradius] [decimal](18, 0) NULL,
	[comp2proximityradius] [decimal](18, 0) NULL,
	[comp3proximityradius] [decimal](18, 0) NULL,
	[subjectcensustractno] [varchar](20) NULL,
	[subjectcensustractmap] [varchar](20) NULL,
 CONSTRAINT [PK_lappraisaldata] PRIMARY KEY NONCLUSTERED 
(
	[appraisaldataid] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lborrowerdataaddress]    Script Date: 11/16/2017 3:02:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lborrowerdataaddress](
	[borroweraddressid] [int] IDENTITY(1,1) NOT NULL,
	[borrowerid] [int] NOT NULL,
	[sourcedocument] [int] NOT NULL,
	[presentaddressyn] [bit] NULL,
	[streetaddress] [varchar](200) NULL,
	[city] [varchar](100) NULL,
	[state] [varchar](50) NULL,
	[postalcode] [varchar](50) NULL,
	[propertyownedyn] [bit] NULL,
	[numberyearsresided] [decimal](5, 3) NULL,
 CONSTRAINT [PK_lborrowerdataaddress] PRIMARY KEY NONCLUSTERED 
(
	[borroweraddressid] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lborrowermaster]    Script Date: 11/16/2017 3:02:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lborrowermaster](
	[borrowerid] [int] IDENTITY(100,1) NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[borrowerordinate] [tinyint] NOT NULL,
	[displayfirstname] [varchar](50) NOT NULL,
	[displaylastname] [varchar](50) NOT NULL,
	[displaydocumentid] [int] NOT NULL,
	[borrowerincomeaudited] [bit] NOT NULL,
	[Borrweractiveyn] [bit] NOT NULL,
 CONSTRAINT [PK_lborrowermaster] PRIMARY KEY NONCLUSTERED 
(
	[borrowerid] DESC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lentitydata]    Script Date: 11/16/2017 3:02:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lentitydata](
	[entitydataid] [int] IDENTITY(1,1) NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[borrowerid] [int] NULL,
	[sourcedocument] [int] NOT NULL,
	[subdocumentid] [int] NULL,
	[entitytype] [varchar](50) NULL,
	[entitysubtype] [varchar](50) NULL,
	[entitycity] [varchar](50) NULL,
	[entitydescription] [varchar](150) NULL,
	[entitymonthlypayment] [money] NULL,
	[entitymonthslefttopay] [smallint] NULL,
	[entityname] [varchar](75) NULL,
	[entityphonenumber] [char](10) NULL,
	[entitypostalcode] [varchar](50) NULL,
	[entitystate] [varchar](2) NULL,
	[entitystreetaddress] [varchar](100) NULL,
	[entityvalueorbalance] [money] NULL,
	[identificationcode] [varbinary](100) NULL,
	[positiontitle] [varchar](100) NULL,
	[entitypercent] [decimal](6, 4) NULL,
	[secondvalueorbalance] [money] NULL,
	[amountpaidborrower] [money] NULL,
	[amountpaidseller] [money] NULL,
	[entityterms] [varchar](50) NULL,
	[entityvaluedatereported] [smalldatetime] NULL,
	[numberyearsatentity] [decimal](10, 2) NULL,
	[entitybitvalue] [bit] NULL,
	[entityamountvalue1] [money] NULL,
	[entityamountvalue2] [money] NULL,
	[entityamountvalue3] [money] NULL,
	[entitydatevalue1] [smalldatetime] NULL,
	[entitydatevalue2] [smalldatetime] NULL,
 CONSTRAINT [lentitydata_PK] PRIMARY KEY NONCLUSTERED 
(
	[entitydataid] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lkeyloandata]    Script Date: 11/16/2017 3:02:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lkeyloandata](
	[lkeyloandataid] [int] IDENTITY(1,1) NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[sourcedocument] [int] NOT NULL,
	[appraisedpropertyvalue] [money] NULL,
	[loanamoritizationtype] [varchar](50) NULL,
	[loanamoritizationtypedesc] [varchar](100) NULL,
	[Loanprogram] [varchar](100) NOT NULL,
	[loanamount] [money] NULL,
	[loaninterestrate] [decimal](6, 4) NULL,
	[loanlienposition] [varchar](24) NULL,
	[loanpurpose] [int] NULL,
	[loanpurposeotherdesc] [varchar](250) NULL,
	[loanterminmonths] [smallint] NULL,
	[loantype] [varchar](50) NULL,
	[loancashdown] [money] NULL,
	[loanccbuyer] [money] NULL,
	[loanccseller] [money] NULL,
	[loancctotal] [money] NULL,
	[mifinancedamount] [money] NULL,
	[mortgageinsurancecoverage] [decimal](7, 4) NULL,
	[micertificatenumber] [varchar](32) NULL,
	[propertycounty] [varchar](50) NULL,
	[propertycity] [varchar](100) NULL,
	[propertyoccupancystatus] [varchar](24) NULL,
	[propertypostalcode] [varchar](50) NULL,
	[propertysalesprice] [money] NULL,
	[propertystate] [char](2) NULL,
	[propertystreetaddress] [varchar](200) NULL,
	[propertytype] [varchar](24) NULL,
	[ratiodebt] [decimal](7, 4) NULL,
	[ratiohousing] [decimal](7, 4) NULL,
	[ratioloantovalue] [decimal](7, 4) NULL,
	[ratiootherLTV] [decimal](7, 4) NULL,
	[propertyparcelno] [varchar](50) NULL,
	[propertyblockno] [varchar](50) NULL,
	[propertylotno] [varchar](50) NULL,
	[propertypinnumber] [varchar](50) NULL,
	[propertylegaldescother] [varchar](750) NULL,
	[loandatesettlement] [smalldatetime] NULL,
	[loandepositamount] [money] NULL,
	[loannotedate] [smalldatetime] NULL,
	[propertyprojclasstype] [varchar](15) NULL,
	[economiclifeyears] [decimal](7, 4) NULL,
	[combinedtotalincome] [money] NULL,
	[combinedtotalassets] [money] NULL,
	[loandatevalue1] [smalldatetime] NULL,
	[loanintegervalue1] [int] NULL,
	[loansellersname] [varchar](100) NULL,
	[docdatesigned] [smalldatetime] NULL,
	[amountvalue1] [money] NULL,
	[amountvalue2] [money] NULL,
	[loantextvalue1] [varchar](50) NULL,
	[loanbitvalue1] [bit] NULL,
	[docformnumber] [varchar](50) NULL,
	[docformversion] [varchar](50) NULL,
	[loandatevalue2] [smalldatetime] NULL,
	[loanmaturitydate] [smalldatetime] NULL,
	[loanfeesimpleyn] [bit] NOT NULL,
	[loancashreserves] [money] NULL,
	[loandiscountpoints] [decimal](6, 4) NULL,
	[PITI] [money] NULL,
	[Documentationtype] [varchar](50) NULL,
	[austype] [varchar](50) NULL,
	[fieldReviewChk] [bit] NULL,
 CONSTRAINT [PK_lkeyloandata] PRIMARY KEY NONCLUSTERED 
(
	[lkeyloandataid] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[loanmaster]    Script Date: 11/16/2017 3:02:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[loanmaster](
	[loanmasterid] [int] IDENTITY(1,1) NOT NULL,
	[customerid] [int] NOT NULL,
	[customerloannumber] [varchar](50) NOT NULL,
	[primaryborrowerlastname] [varchar](50) NULL,
	[qproprocessdate] [smalldatetime] NULL,
	[qloantypeid] [int] NOT NULL,
	[customerprepost] [varchar](12) NOT NULL,
	[customerloanstatus] [varchar](24) NULL,
	[customerloanstatusdate] [smalldatetime] NULL,
	[customerloantype] [varchar](50) NULL,
	[regiondescription] [varchar](24) NULL,
	[branchofficestate] [char](2) NOT NULL,
	[branchoffice] [varchar](50) NULL,
	[originator] [varchar](50) NULL,
	[customerproducttype] [varchar](50) NULL,
	[broker] [varchar](50) NULL,
	[closingagent] [varchar](50) NULL,
	[funder] [varchar](50) NULL,
	[investor] [varchar](50) NULL,
	[loanofficer] [varchar](50) NULL,
	[processor] [varchar](50) NULL,
	[titlecompany] [varchar](50) NULL,
	[underwriter] [varchar](50) NULL,
	[lmcreatedby] [char](12) NULL,
	[lmdatecreated] [smalldatetime] NULL,
	[finalqprorating] [varchar](36) NULL,
	[qprostatus] [varchar](24) NULL,
	[generalcomment] [varchar](5000) NULL,
	[businesssource] [varchar](50) NULL,
	[loanpurpose] [int] NOT NULL,
	[appraiser] [varchar](50) NOT NULL,
	[loantovalue] [decimal](7, 4) NOT NULL,
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
	[subclientname] [varchar](250) NOT NULL,
	[settlementagent] [varchar](50) NOT NULL,
	[criticaloverrideyn] [bit] NOT NULL,
	[ausdecision] [varchar](40) NOT NULL,
	[pipelinestatus] [smallint] NOT NULL,
	[lientype] [varchar](36) NOT NULL,
	[brokerid] [varchar](18) NOT NULL,
	[lqrtstatus] [varchar](20) NOT NULL,
	[AssignedTo] [varchar](30) NULL,
	[austype] [varchar](50) NULL,
 CONSTRAINT [PK_loanmaster] PRIMARY KEY CLUSTERED 
(
	[loanmasterid] DESC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[loanXMLstore]    Script Date: 11/16/2017 3:02:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[loanXMLstore](
	[loanXMLstoreid] [int] IDENTITY(1,1) NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[loanXML] [varchar](4000) NULL,
	[borrowerXML] [varchar](6000) NULL,
	[bssn1] [varbinary](100) NULL,
	[bssn2] [varbinary](100) NULL,
	[bssn3] [varbinary](100) NULL,
 CONSTRAINT [PK_loanXMLstore] PRIMARY KEY NONCLUSTERED 
(
	[loanXMLstoreid] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[loverflowloandata]    Script Date: 11/16/2017 3:02:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[loverflowloandata](
	[loverflowloandataid] [int] IDENTITY(1,1) NOT NULL,
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
 CONSTRAINT [PK_loverflowloandata] PRIMARY KEY NONCLUSTERED 
(
	[loverflowloandataid] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lpropertyloandata]    Script Date: 11/16/2017 3:02:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lpropertyloandata](
	[propertyloandataid] [int] IDENTITY(1,1) NOT NULL,
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
	[estexpenseotherhousingdesc] [varchar](150) NULL,
	[estexpenserealeastatetaxes] [money] NULL,
	[estexpensespecialassessments] [money] NULL,
	[estexpensetotal] [money] NULL,
	[estexpensetotalhousing] [money] NULL,
	[estexpenseutilities] [money] NULL,
	[sectionofhousingact] [varchar](12) NULL,
	[loanitem1desc] [varchar](50) NULL,
	[loanitem1amount] [money] NULL,
	[loanitem2desc] [varchar](50) NULL,
	[loanitem2amount] [money] NULL,
	[loanitem3desc] [varchar](50) NULL,
	[loanitem3amount] [money] NULL,
	[loanitem4desc] [varchar](50) NULL,
	[loanitem4amount] [money] NULL,
	[loanitem5desc] [varchar](50) NULL,
	[loanitem5amount] [money] NULL,
	[loanitem6desc] [varchar](50) NULL,
	[loanitem6amount] [money] NULL,
	[loantotalsetborrower] [money] NULL,
	[loantotalsetseller] [money] NULL,
	[mortgageinsurercode] [varchar](15) NULL,
	[loanunderwitername] [varchar](100) NULL,
	[loanappraisername] [varchar](100) NULL,
	[loanappraisalcompany] [varchar](50) NULL,
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
	[interviewcommunicationmode] [varchar](32) NULL,
	[interviewname] [varchar](50) NULL,
	[loanintegervalue1] [int] NULL,
	[combincomepositivecashflow] [money] NULL,
	[propertyloanbitvalue1] [bit] NOT NULL,
	[loanitem7amount] [money] NULL,
	[loanitem8amount] [money] NULL,
	[loanitem9amount] [money] NULL,
	[propertyloandecimalvalue1] [decimal](9, 4) NULL,
	[assetlifeinsurancefaceamount] [money] NULL,
	[assetlifeinsurancevalue] [money] NULL,
	[assetstotalliquid] [money] NULL,
	[cashdepositamount] [money] NULL,
	[cashdepositholder] [varchar](100) NULL,
	[networth] [money] NULL,
 CONSTRAINT [lpropertyloandata_PK] PRIMARY KEY CLUSTERED 
(
	[propertyloandataid] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[lreportingdata]    Script Date: 11/16/2017 3:02:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lreportingdata](
	[lreportingdataid] [int] IDENTITY(1,1) NOT NULL,
	[loanmasterid] [int] NOT NULL,
	[amortizationtype] [varchar](50) NOT NULL,
	[appraiser] [varchar](50) NOT NULL,
	[appraisedvalue] [money] NOT NULL,
	[branchoffice] [varchar](50) NOT NULL,
	[branchofficestate] [char](2) NOT NULL,
	[broker] [varchar](100) NOT NULL,
	[businesssource] [varchar](50) NOT NULL,
	[settlementdate] [smalldatetime] NOT NULL,
	[disbursementdate] [smalldatetime] NOT NULL,
	[closingagent] [varchar](50) NOT NULL,
	[ltv] [decimal](7, 4) NOT NULL,
	[cltv] [decimal](7, 4) NOT NULL,
	[ratiodebt] [decimal](7, 4) NOT NULL,
	[ratiohousing] [decimal](7, 4) NOT NULL,
	[funder] [varchar](50) NOT NULL,
	[fundstoclose] [money] NOT NULL,
	[investor] [varchar](50) NOT NULL,
	[loanpurpose] [int] NOT NULL,
	[loantype] [varchar](50) NOT NULL,
	[loanamount] [money] NOT NULL,
	[loaninterestrate] [decimal](6, 4) NOT NULL,
	[loanofficer] [varchar](50) NOT NULL,
	[loanprogram] [varchar](100) NOT NULL,
	[monthlypiamount] [money] NOT NULL,
	[monthlymipayment] [money] NOT NULL,
	[originator] [varchar](50) NOT NULL,
	[processor] [varchar](50) NOT NULL,
	[salesprice] [money] NOT NULL,
	[settlementagent] [varchar](50) NOT NULL,
	[subclientname] [varchar](250) NOT NULL,
	[titlecompany] [varchar](50) NOT NULL,
	[underwriter] [varchar](50) NOT NULL,
	[documentationtype] [varchar](50) NOT NULL,
	[datecreated] [smalldatetime] NOT NULL,
	[propertytype] [varchar](24) NOT NULL,
	[ausdecision] [varchar](50) NOT NULL,
	[nounits] [tinyint] NOT NULL,
	[qcanalyst] [varchar](50) NOT NULL,
	[austype] [varchar](50) NULL,
 CONSTRAINT [PK_lreportingdata] PRIMARY KEY NONCLUSTERED 
(
	[lreportingdataid] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_sourcedocument]  DEFAULT ((0)) FOR [sourcedocument]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1adjsalesprice]  DEFAULT ((0)) FOR [comp1adjsalesprice]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1dateofsales]  DEFAULT ((0)) FOR [comp1dateofsales]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1grosspercent]  DEFAULT ((0)) FOR [comp1grosspercent]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1livingareasqft]  DEFAULT ((0)) FOR [comp1livingareasqft]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1netadjamount]  DEFAULT ((0)) FOR [comp1netadjamount]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1netpercent]  DEFAULT ((0)) FOR [comp1netpercent]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1proximity]  DEFAULT ((0)) FOR [comp1proximity]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1salesprice]  DEFAULT ((0)) FOR [comp1salesprice]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2adjsalesprice]  DEFAULT ((0)) FOR [comp2adjsalesprice]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2dateofsales]  DEFAULT ((0)) FOR [comp2dateofsales]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2grosspercent]  DEFAULT ((0)) FOR [comp2grosspercent]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2livingareasqft]  DEFAULT ((0)) FOR [comp2livingareasqft]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2netadjamount]  DEFAULT ((0)) FOR [comp2netadjamount]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2netpercent]  DEFAULT ((0)) FOR [comp2netpercent]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2proximity]  DEFAULT ((0)) FOR [comp2proximity]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2salesprice]  DEFAULT ((0)) FOR [comp2salesprice]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3adjsalesprice]  DEFAULT ((0)) FOR [comp3adjsalesprice]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3dateofsales]  DEFAULT ((0)) FOR [comp3dateofsales]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3grosspercent]  DEFAULT ((0)) FOR [comp3grosspercent]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3livingareasqft]  DEFAULT ((0)) FOR [comp3livingareasqft]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3netadjamount]  DEFAULT ((0)) FOR [comp3netadjamount]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3netpercent]  DEFAULT ((0)) FOR [comp3netpercent]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3proximity]  DEFAULT ((0)) FOR [comp3proximity]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3salesprice]  DEFAULT ((0)) FOR [comp3salesprice]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_datereconcilation]  DEFAULT ((0)) FOR [datereconcilation]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_locationtype]  DEFAULT ((0)) FOR [locationtype]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_neighborhooddemand]  DEFAULT ((0)) FOR [neighborhooddemand]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_neighborhoodgrowthrate]  DEFAULT ((0)) FOR [neighborhoodgrowthrate]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_percentlanduseother]  DEFAULT ((0)) FOR [percentlanduseother]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_percentlanduse1family]  DEFAULT ((0)) FOR [percentlanduse1family]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_percentlanduse24family]  DEFAULT ((0)) FOR [percentlanduse24family]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_percentlandusecommercial]  DEFAULT ((0)) FOR [percentlandusecommercial]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_percentlandusemultifamily]  DEFAULT ((0)) FOR [percentlandusemultifamily]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_percentpropertybuiltup]  DEFAULT ((0)) FOR [percentpropertybuiltup]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_propertyrightsappraised]  DEFAULT ((0)) FOR [propertyrightsappraised]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_propertyvaluetrend]  DEFAULT ((0)) FOR [propertyvaluetrend]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectdesignstyles]  DEFAULT ((0)) FOR [subjectdesignstyles]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectdetached]  DEFAULT ((0)) FOR [subjectdetached]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjecteffectiveageyears]  DEFAULT ((0)) FOR [subjecteffectiveageyears]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectexisting]  DEFAULT ((0)) FOR [subjectexisting]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectindicatedvalue]  DEFAULT ((0)) FOR [subjectindicatedvalue]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectnoofstories]  DEFAULT ((0)) FOR [subjectnoofstories]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectnoofunits]  DEFAULT ((0)) FOR [subjectnoofunits]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectroomcountbaths]  DEFAULT ((0)) FOR [subjectroomcountbaths]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectroomcountbedrooms]  DEFAULT ((0)) FOR [subjectroomcountbedrooms]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectroomcounttotal]  DEFAULT ((0)) FOR [subjectroomcounttotal]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectlivingareasqft]  DEFAULT ((0)) FOR [subjectlivingareasqft]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectyearbuilt]  DEFAULT ((0)) FOR [subjectyearbuilt]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_timetosellproperty]  DEFAULT ((0)) FOR [timetosellproperty]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_FEMAfloodareayn]  DEFAULT ((0)) FOR [FEMAfloodareayn]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_FEMAzone]  DEFAULT ((0)) FOR [FEMAzone]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_FEMAmapdate]  DEFAULT ((0)) FOR [FEMAmapdate]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_FEMAmapnumber]  DEFAULT ((0)) FOR [FEMAmapnumber]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_appraisercertificationlicense]  DEFAULT ((0)) FOR [appraisercertificationlicense]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_appraiserstateissued]  DEFAULT ((0)) FOR [appraiserstateissued]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_appraiserlicensedate]  DEFAULT ((0)) FOR [appraiserlicensedate]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1roomcountbaths]  DEFAULT ((0)) FOR [comp1roomcountbaths]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1roomcountbedrooms]  DEFAULT ((0)) FOR [comp1roomcountbedrooms]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1roomcounttotal]  DEFAULT ((0)) FOR [comp1roomcounttotal]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2roomcountbaths]  DEFAULT ((0)) FOR [comp2roomcountbaths]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2roomcountbedrooms]  DEFAULT ((0)) FOR [comp2roomcountbedrooms]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2roomcounttotal]  DEFAULT ((0)) FOR [comp2roomcounttotal]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3roomcountbaths]  DEFAULT ((0)) FOR [comp3roomcountbaths]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3roomcountbedrooms]  DEFAULT ((0)) FOR [comp3roomcountbedrooms]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3roomcounttotal]  DEFAULT ((0)) FOR [comp3roomcounttotal]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectvaluationconfidence]  DEFAULT ((0)) FOR [subjectvaluationconfidence]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectvaluationhighestimate]  DEFAULT ((0)) FOR [subjectvaluationhighestimate]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectvaluationlowestimate]  DEFAULT ((0)) FOR [subjectvaluationlowestimate]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectlotsize]  DEFAULT ((0)) FOR [subjectlotsize]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectac]  DEFAULT ((0)) FOR [subjectac]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectpool]  DEFAULT ((0)) FOR [subjectpool]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectgarage]  DEFAULT ((0)) FOR [subjectgarage]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1streetaddress]  DEFAULT ((0)) FOR [comp1streetaddress]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1usecode]  DEFAULT ((0)) FOR [comp1usecode]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2streetaddress]  DEFAULT ((0)) FOR [comp2streetaddress]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2usecode]  DEFAULT ((0)) FOR [comp2usecode]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3streetaddress]  DEFAULT ((0)) FOR [comp3streetaddress]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3usecode]  DEFAULT ((0)) FOR [comp3usecode]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectusecode]  DEFAULT ((0)) FOR [subjectusecode]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1lotsize]  DEFAULT ((0)) FOR [comp1lotsize]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1ac]  DEFAULT ((0)) FOR [comp1ac]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1pool]  DEFAULT ((0)) FOR [comp1pool]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1garage]  DEFAULT ((0)) FOR [comp1garage]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2lotsize]  DEFAULT ((0)) FOR [comp2lotsize]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2ac]  DEFAULT ((0)) FOR [comp2ac]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2pool]  DEFAULT ((0)) FOR [comp2pool]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2garage]  DEFAULT ((0)) FOR [comp2garage]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3lotsize]  DEFAULT ((0)) FOR [comp3lotsize]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3ac]  DEFAULT ((0)) FOR [comp3ac]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3pool]  DEFAULT ((0)) FOR [comp3pool]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3garage]  DEFAULT ((0)) FOR [comp3garage]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp1proximityradius]  DEFAULT ((0)) FOR [comp1proximityradius]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp2proximityradius]  DEFAULT ((0)) FOR [comp2proximityradius]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_comp3proximityradius]  DEFAULT ((0)) FOR [comp3proximityradius]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectcensustractno]  DEFAULT ((0)) FOR [subjectcensustractno]
GO
ALTER TABLE [dbo].[lappraisaldata] ADD  CONSTRAINT [DF_lappraisaldata_subjectcensustractmap]  DEFAULT ((0)) FOR [subjectcensustractmap]
GO
ALTER TABLE [dbo].[lborrowerdataaddress] ADD  CONSTRAINT [DF_lborrowerdataaddress_borrowerid]  DEFAULT ((0)) FOR [borrowerid]
GO
ALTER TABLE [dbo].[lborrowerdataaddress] ADD  CONSTRAINT [DF_lborrowerdataaddress_sourcedocument]  DEFAULT ((0)) FOR [sourcedocument]
GO
ALTER TABLE [dbo].[lborrowerdataaddress] ADD  CONSTRAINT [DF_lborrowerdataaddress_presentaddressyn]  DEFAULT ((1)) FOR [presentaddressyn]
GO
ALTER TABLE [dbo].[lborrowerdataaddress] ADD  CONSTRAINT [DF_lborrowerdataaddress_streetaddress]  DEFAULT ((0)) FOR [streetaddress]
GO
ALTER TABLE [dbo].[lborrowerdataaddress] ADD  CONSTRAINT [DF_lborrowerdataaddress_city]  DEFAULT ((0)) FOR [city]
GO
ALTER TABLE [dbo].[lborrowerdataaddress] ADD  CONSTRAINT [DF_lborrowerdataaddress_state]  DEFAULT ((0)) FOR [state]
GO
ALTER TABLE [dbo].[lborrowerdataaddress] ADD  CONSTRAINT [DF_lborrowerdataaddress_postalcode]  DEFAULT ((0)) FOR [postalcode]
GO
ALTER TABLE [dbo].[lborrowerdataaddress] ADD  CONSTRAINT [DF_lborrowerdataaddress_propertyownedyn]  DEFAULT ((1)) FOR [propertyownedyn]
GO
ALTER TABLE [dbo].[lborrowerdataaddress] ADD  CONSTRAINT [DF_lborrowerdataaddress_numberyearsresided]  DEFAULT ((0)) FOR [numberyearsresided]
GO
ALTER TABLE [dbo].[lborrowermaster] ADD  CONSTRAINT [DF_lborrowermaster_borrowerordinate]  DEFAULT ((1)) FOR [borrowerordinate]
GO
ALTER TABLE [dbo].[lborrowermaster] ADD  CONSTRAINT [DF_lborrowermaster_displayfirstname]  DEFAULT ((0)) FOR [displayfirstname]
GO
ALTER TABLE [dbo].[lborrowermaster] ADD  CONSTRAINT [DF_lborrowermaster_displaylastname]  DEFAULT ((0)) FOR [displaylastname]
GO
ALTER TABLE [dbo].[lborrowermaster] ADD  CONSTRAINT [DF_lborrowermaster_displaydocumentid]  DEFAULT ((0)) FOR [displaydocumentid]
GO
ALTER TABLE [dbo].[lborrowermaster] ADD  CONSTRAINT [DF_lborrowermaster_borrrowerincomeaudited]  DEFAULT ((1)) FOR [borrowerincomeaudited]
GO
ALTER TABLE [dbo].[lborrowermaster] ADD  CONSTRAINT [DF_lborrowermaster_Borrweractiveyn]  DEFAULT ((1)) FOR [Borrweractiveyn]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_loanmasterid]  DEFAULT ((0)) FOR [loanmasterid]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_borrowerid]  DEFAULT ((0)) FOR [borrowerid]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_sourcedocument]  DEFAULT ((0)) FOR [sourcedocument]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_subdocumentid]  DEFAULT ((0)) FOR [subdocumentid]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitytype]  DEFAULT ((0)) FOR [entitytype]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitysubtype]  DEFAULT ((0)) FOR [entitysubtype]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitycity]  DEFAULT ((0)) FOR [entitycity]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitydescription]  DEFAULT ((0)) FOR [entitydescription]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitymonthlypayment]  DEFAULT ((0)) FOR [entitymonthlypayment]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitymonthslefttopay]  DEFAULT ((0)) FOR [entitymonthslefttopay]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entityname]  DEFAULT ((0)) FOR [entityname]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entityphonenumber]  DEFAULT ((0)) FOR [entityphonenumber]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitypostalcode]  DEFAULT ((0)) FOR [entitypostalcode]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitystate]  DEFAULT ((0)) FOR [entitystate]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitystreetaddress]  DEFAULT ((0)) FOR [entitystreetaddress]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entityvalueorbalance]  DEFAULT ((0)) FOR [entityvalueorbalance]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_positiontitle]  DEFAULT ((0)) FOR [positiontitle]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitypercent]  DEFAULT ((0)) FOR [entitypercent]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_secondvalueorbalance]  DEFAULT ((0)) FOR [secondvalueorbalance]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_amountpaidborrower]  DEFAULT ((0)) FOR [amountpaidborrower]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_amountpaidseller]  DEFAULT ((0)) FOR [amountpaidseller]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entityterms]  DEFAULT ((0)) FOR [entityterms]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entityvaluedatereported]  DEFAULT ((0)) FOR [entityvaluedatereported]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_numberyearsatentity]  DEFAULT ((0)) FOR [numberyearsatentity]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitybitvalue]  DEFAULT ((1)) FOR [entitybitvalue]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entityamountvalue1]  DEFAULT ((0)) FOR [entityamountvalue1]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entityamountvalue2]  DEFAULT ((0)) FOR [entityamountvalue2]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entityamountvalue3]  DEFAULT ((0)) FOR [entityamountvalue3]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitydatevalue1]  DEFAULT ((0)) FOR [entitydatevalue1]
GO
ALTER TABLE [dbo].[lentitydata] ADD  CONSTRAINT [DF_lentitydata_entitydatevalue2]  DEFAULT ((0)) FOR [entitydatevalue2]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanmasterid]  DEFAULT ((0)) FOR [loanmasterid]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_sourcedocument]  DEFAULT ((0)) FOR [sourcedocument]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_appraisedpropertyvalue]  DEFAULT ((0)) FOR [appraisedpropertyvalue]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanamoritizationtype]  DEFAULT ((0)) FOR [loanamoritizationtype]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanamoritizationtypedesc]  DEFAULT ((0)) FOR [loanamoritizationtypedesc]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_Loanprogram]  DEFAULT ((0)) FOR [Loanprogram]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanamount]  DEFAULT ((0)) FOR [loanamount]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loaninterestrate]  DEFAULT ((0)) FOR [loaninterestrate]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanlienposition]  DEFAULT ((0)) FOR [loanlienposition]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanpurpose]  DEFAULT ((0)) FOR [loanpurpose]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanpurposeotherdesc]  DEFAULT ((0)) FOR [loanpurposeotherdesc]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanterminmonths]  DEFAULT ((0)) FOR [loanterminmonths]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loantype]  DEFAULT ((0)) FOR [loantype]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loancashdown]  DEFAULT ((0)) FOR [loancashdown]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanccbuyer]  DEFAULT ((0)) FOR [loanccbuyer]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanccseller]  DEFAULT ((0)) FOR [loanccseller]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loancctotal]  DEFAULT ((0)) FOR [loancctotal]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_mifinancedamount]  DEFAULT ((0)) FOR [mifinancedamount]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_mortgageinsurancecoverage]  DEFAULT ((0)) FOR [mortgageinsurancecoverage]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_micertificatenumber]  DEFAULT ((0)) FOR [micertificatenumber]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertycounty]  DEFAULT ((0)) FOR [propertycounty]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertycity]  DEFAULT ((0)) FOR [propertycity]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertyoccupancystatus]  DEFAULT ((0)) FOR [propertyoccupancystatus]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertypostalcode]  DEFAULT ((0)) FOR [propertypostalcode]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertysalesprice]  DEFAULT ((0)) FOR [propertysalesprice]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertystate]  DEFAULT ((0)) FOR [propertystate]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertystreetaddress]  DEFAULT ((0)) FOR [propertystreetaddress]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertytype]  DEFAULT ((0)) FOR [propertytype]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_ratiodebt]  DEFAULT ((0)) FOR [ratiodebt]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_ratiohousing]  DEFAULT ((0)) FOR [ratiohousing]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_ratioloantovalue]  DEFAULT ((0)) FOR [ratioloantovalue]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_ratiootherLTV]  DEFAULT ((0)) FOR [ratiootherLTV]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertyparcelno]  DEFAULT ((0)) FOR [propertyparcelno]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertyblockno]  DEFAULT ((0)) FOR [propertyblockno]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertylotno]  DEFAULT ((0)) FOR [propertylotno]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertypinnumber]  DEFAULT ((0)) FOR [propertypinnumber]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertylegaldescother]  DEFAULT ((0)) FOR [propertylegaldescother]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loandatesettlement]  DEFAULT ((0)) FOR [loandatesettlement]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loandepositamount]  DEFAULT ((0)) FOR [loandepositamount]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loannotedate]  DEFAULT ((0)) FOR [loannotedate]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_propertyprojclasstype]  DEFAULT ((0)) FOR [propertyprojclasstype]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_economiclifeyears]  DEFAULT ((0)) FOR [economiclifeyears]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_combinedtotalincome]  DEFAULT ((0)) FOR [combinedtotalincome]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_combinedtotalassets]  DEFAULT ((0)) FOR [combinedtotalassets]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loandatevalue1]  DEFAULT ((0)) FOR [loandatevalue1]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanintegervalue1]  DEFAULT ((0)) FOR [loanintegervalue1]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loansellersname]  DEFAULT ((0)) FOR [loansellersname]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_docdatesigned]  DEFAULT ((0)) FOR [docdatesigned]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_amountvalue1]  DEFAULT ((0)) FOR [amountvalue1]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_amountvalue2]  DEFAULT ((0)) FOR [amountvalue2]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loantextvalue1]  DEFAULT ((0)) FOR [loantextvalue1]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanbitvalue1]  DEFAULT ((1)) FOR [loanbitvalue1]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_docformnumber]  DEFAULT ((0)) FOR [docformnumber]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_docformversion]  DEFAULT ((0)) FOR [docformversion]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loandatevalue2]  DEFAULT ((0)) FOR [loandatevalue2]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanmaturitydate]  DEFAULT ((0)) FOR [loanmaturitydate]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loanfeesimpleyn]  DEFAULT ((1)) FOR [loanfeesimpleyn]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loancashreserves]  DEFAULT ((0)) FOR [loancashreserves]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_loandiscountpoints]  DEFAULT ((0)) FOR [loandiscountpoints]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_PITI]  DEFAULT ((0)) FOR [PITI]
GO
ALTER TABLE [dbo].[lkeyloandata] ADD  CONSTRAINT [DF_lkeyloandata_Documentationtype]  DEFAULT ((0)) FOR [Documentationtype]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_customerid]  DEFAULT ((0)) FOR [customerid]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_customerloannumber]  DEFAULT ((0)) FOR [customerloannumber]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_primaryborrowerlastname]  DEFAULT ((0)) FOR [primaryborrowerlastname]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_qproprocessdate]  DEFAULT ((0)) FOR [qproprocessdate]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_qloantypeid]  DEFAULT ((0)) FOR [qloantypeid]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_customerprepost]  DEFAULT ('Post Close') FOR [customerprepost]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_customerloanstatus]  DEFAULT ((0)) FOR [customerloanstatus]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_customerloanstatusdate]  DEFAULT ((0)) FOR [customerloanstatusdate]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_customerloantype]  DEFAULT ((0)) FOR [customerloantype]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_regiondescription]  DEFAULT ((0)) FOR [regiondescription]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_branchofficestate]  DEFAULT ((0)) FOR [branchofficestate]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_branchoffice]  DEFAULT ((0)) FOR [branchoffice]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_originator]  DEFAULT ((0)) FOR [originator]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_customerproducttype]  DEFAULT ((0)) FOR [customerproducttype]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_broker]  DEFAULT ((0)) FOR [broker]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_closingagent]  DEFAULT ((0)) FOR [closingagent]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_funder]  DEFAULT ((0)) FOR [funder]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_investor]  DEFAULT ((0)) FOR [investor]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_loanofficer]  DEFAULT ((0)) FOR [loanofficer]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_processor]  DEFAULT ((0)) FOR [processor]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_titlecompany]  DEFAULT ((0)) FOR [titlecompany]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_underwriter]  DEFAULT ((0)) FOR [underwriter]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_lmcreatedby]  DEFAULT ((0)) FOR [lmcreatedby]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_lmdatecreated]  DEFAULT ((0)) FOR [lmdatecreated]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_finalqprorating]  DEFAULT ((0)) FOR [finalqprorating]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_qprostatus]  DEFAULT ('Not Begun') FOR [qprostatus]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_generalcomment]  DEFAULT ((0)) FOR [generalcomment]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_businesssource]  DEFAULT ((0)) FOR [businesssource]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_loanpurpose]  DEFAULT ((0)) FOR [loanpurpose]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_appraiser]  DEFAULT ((0)) FOR [appraiser]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_loantovalue]  DEFAULT ((0)) FOR [loantovalue]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_loanclosingdate]  DEFAULT ((0)) FOR [loanclosingdate]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_qprodatauploadid]  DEFAULT ((0)) FOR [qprodatauploadid]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_completeloanimportedyn]  DEFAULT ((0)) FOR [completeloanimportedyn]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_auditassetsincomeyn]  DEFAULT ((0)) FOR [auditassetsincomeyn]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_underwriterapprovaldate]  DEFAULT ((0)) FOR [underwriterapprovaldate]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_loaninvoicedyn]  DEFAULT ((0)) FOR [billinvoicerecordsid]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_qprodateforbilling]  DEFAULT ((0)) FOR [qprodateforbilling]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_qprocomplianceyn]  DEFAULT ((0)) FOR [qprocomplianceyn]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_billfilepackid]  DEFAULT ((0)) FOR [billfilepackid]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_loanislocked]  DEFAULT ((0)) FOR [loanislocked]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_subclientname]  DEFAULT ((0)) FOR [subclientname]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_settlementagent]  DEFAULT ((0)) FOR [settlementagent]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_criticaloverrideyn]  DEFAULT ((0)) FOR [criticaloverrideyn]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_ausdecision]  DEFAULT ((0)) FOR [ausdecision]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_pipelinestatus]  DEFAULT ((0)) FOR [pipelinestatus]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_lientype]  DEFAULT ((0)) FOR [lientype]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_brokerid]  DEFAULT ((0)) FOR [brokerid]
GO
ALTER TABLE [dbo].[loanmaster] ADD  CONSTRAINT [DF_loanmaster_lqrtstatus]  DEFAULT ('Ready') FOR [lqrtstatus]
GO
ALTER TABLE [dbo].[loanXMLstore] ADD  CONSTRAINT [DF_loanXMLstore_loanmasterid]  DEFAULT ((0)) FOR [loanmasterid]
GO
ALTER TABLE [dbo].[loanXMLstore] ADD  CONSTRAINT [DF_loanXMLstore_loanXML]  DEFAULT ((0)) FOR [loanXML]
GO
ALTER TABLE [dbo].[loanXMLstore] ADD  CONSTRAINT [DF_loanXMLstore_borrowerXML]  DEFAULT ((0)) FOR [borrowerXML]
GO
ALTER TABLE [dbo].[loanXMLstore] ADD  CONSTRAINT [DF_loanXMLstore_bssn1]  DEFAULT ((0)) FOR [bssn1]
GO
ALTER TABLE [dbo].[loanXMLstore] ADD  CONSTRAINT [DF_loanXMLstore_bssn11]  DEFAULT ((0)) FOR [bssn2]
GO
ALTER TABLE [dbo].[loanXMLstore] ADD  CONSTRAINT [DF_loanXMLstore_bssn12]  DEFAULT ((0)) FOR [bssn3]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_alterationsamount]  DEFAULT ((0)) FOR [alterationsamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_improvementamount]  DEFAULT ((0)) FOR [improvementamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_repairsamount]  DEFAULT ((0)) FOR [repairsamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_landamount]  DEFAULT ((0)) FOR [landamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_refinanceamount]  DEFAULT ((0)) FOR [refinanceamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_prepaiditemsamount]  DEFAULT ((0)) FOR [prepaiditemsamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_closingcostamount]  DEFAULT ((0)) FOR [closingcostamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_pmifinancedamount]  DEFAULT ((0)) FOR [pmifinancedamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_discountamount]  DEFAULT ((0)) FOR [discountamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_subordinatefinancing]  DEFAULT ((0)) FOR [subordinatefinancing]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_bclosingcostspaidbyseller]  DEFAULT ((0)) FOR [bclosingcostspaidbyseller]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_lendercreditamount]  DEFAULT ((0)) FOR [lendercreditamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_fundstocloseamount]  DEFAULT ((0)) FOR [fundstocloseamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_perdiem]  DEFAULT ((0)) FOR [perdiem]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_prepaidinterest]  DEFAULT ((0)) FOR [prepaidinterest]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_hazardescrow]  DEFAULT ((0)) FOR [hazardescrow]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_taxescrow]  DEFAULT ((0)) FOR [taxescrow]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_totalcosts]  DEFAULT ((0)) FOR [totalcosts]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_pmimipfffamount]  DEFAULT ((0)) FOR [pmimipfffamount]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_overflowbitvalue_1]  DEFAULT ((1)) FOR [overflowbitvalue]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_otherescrow]  DEFAULT ((0)) FOR [otherescrow]
GO
ALTER TABLE [dbo].[loverflowloandata] ADD  CONSTRAINT [DF_loverflowloandata_Loanbaseamount]  DEFAULT ((0)) FOR [Loanbaseamount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanmasterid]  DEFAULT ((0)) FOR [loanmasterid]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_sourcedocument]  DEFAULT ((0)) FOR [sourcedocument]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_documentbarcode]  DEFAULT ((0)) FOR [documentbarcode]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpenseallotherpayments]  DEFAULT ((0)) FOR [estexpenseallotherpayments]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpensefirstmortgage]  DEFAULT ((0)) FOR [estexpensefirstmortgage]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpensehazardinsurance]  DEFAULT ((0)) FOR [estexpensehazardinsurance]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpensehomeownerdues]  DEFAULT ((0)) FOR [estexpensehomeownerdues]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpenseleasegroundrent]  DEFAULT ((0)) FOR [estexpenseleasegroundrent]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpensemaintenance]  DEFAULT ((0)) FOR [estexpensemaintenance]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpensemortgageinsurance]  DEFAULT ((0)) FOR [estexpensemortgageinsurance]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpensenegcashflow]  DEFAULT ((0)) FOR [estexpensenegcashflow]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpenseotherfinancing]  DEFAULT ((0)) FOR [estexpenseotherfinancing]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpenseotherhousing]  DEFAULT ((0)) FOR [estexpenseotherhousing]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpenseotherhousingdesc]  DEFAULT ((0)) FOR [estexpenseotherhousingdesc]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpenserealeastatetaxes]  DEFAULT ((0)) FOR [estexpenserealeastatetaxes]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpensespecialassessments]  DEFAULT ((0)) FOR [estexpensespecialassessments]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpensetotal]  DEFAULT ((0)) FOR [estexpensetotal]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpensetotalhousing]  DEFAULT ((0)) FOR [estexpensetotalhousing]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_estexpenseutilities]  DEFAULT ((0)) FOR [estexpenseutilities]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_sectionofhousingact]  DEFAULT ((0)) FOR [sectionofhousingact]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem1desc]  DEFAULT ((0)) FOR [loanitem1desc]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem1amount]  DEFAULT ((0)) FOR [loanitem1amount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem2desc]  DEFAULT ((0)) FOR [loanitem2desc]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem2amount]  DEFAULT ((0)) FOR [loanitem2amount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem3desc]  DEFAULT ((0)) FOR [loanitem3desc]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem3amount]  DEFAULT ((0)) FOR [loanitem3amount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem4desc]  DEFAULT ((0)) FOR [loanitem4desc]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem4amount]  DEFAULT ((0)) FOR [loanitem4amount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem5desc]  DEFAULT ((0)) FOR [loanitem5desc]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem5amount]  DEFAULT ((0)) FOR [loanitem5amount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem6desc]  DEFAULT ((0)) FOR [loanitem6desc]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem6amount]  DEFAULT ((0)) FOR [loanitem6amount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loantotalsetborrower]  DEFAULT ((0)) FOR [loantotalsetborrower]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loantotalsetseller]  DEFAULT ((0)) FOR [loantotalsetseller]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_mortgageinsurercode]  DEFAULT ((0)) FOR [mortgageinsurercode]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanunderwitername]  DEFAULT ((0)) FOR [loanunderwitername]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanappraisername]  DEFAULT ((0)) FOR [loanappraisername]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanappraisalcompany]  DEFAULT ((0)) FOR [loanappraisalcompany]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_combinedtotalrealestate]  DEFAULT ((0)) FOR [combinedtotalrealestate]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_debttotalbalancechildsup]  DEFAULT ((0)) FOR [debttotalbalancechildsup]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_debttotalbalanceother]  DEFAULT ((0)) FOR [debttotalbalanceother]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_debttotalbalancetotal]  DEFAULT ((0)) FOR [debttotalbalancetotal]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_debttotalbalanceinstall]  DEFAULT ((0)) FOR [debttotalbalanceinstall]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_debttotalmopaychildsup]  DEFAULT ((0)) FOR [debttotalmopaychildsup]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_debttotalmopayother]  DEFAULT ((0)) FOR [debttotalmopayother]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_debttotalmopaytotal]  DEFAULT ((0)) FOR [debttotalmopaytotal]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_debttotalmopayinstall]  DEFAULT ((0)) FOR [debttotalmopayinstall]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_cashinprepaidexpenses]  DEFAULT ((0)) FOR [cashinprepaidexpenses]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_cashinrepairs]  DEFAULT ((0)) FOR [cashinrepairs]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_cashinnonrealityother]  DEFAULT ((0)) FOR [cashinnonrealityother]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_cashinamtgift]  DEFAULT ((0)) FOR [cashinamtgift]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_cashin2mortgage]  DEFAULT ((0)) FOR [cashin2mortgage]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_cashinufmipcash]  DEFAULT ((0)) FOR [cashinufmipcash]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_interviewcommunicationmode]  DEFAULT ((0)) FOR [interviewcommunicationmode]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_interviewname]  DEFAULT ((0)) FOR [interviewname]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanintegervalue1]  DEFAULT ((0)) FOR [loanintegervalue1]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_combincomepositivecashflow]  DEFAULT ((0)) FOR [combincomepositivecashflow]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_propertyloanbitvalue1]  DEFAULT ((1)) FOR [propertyloanbitvalue1]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem7amount]  DEFAULT ((0)) FOR [loanitem7amount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem8amount]  DEFAULT ((0)) FOR [loanitem8amount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_loanitem9amount]  DEFAULT ((0)) FOR [loanitem9amount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_propertyloandecimalvalue1]  DEFAULT ((0)) FOR [propertyloandecimalvalue1]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_assetlifeinsurancefaceamount]  DEFAULT ((0)) FOR [assetlifeinsurancefaceamount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_assetlifeinsurancevalue]  DEFAULT ((0)) FOR [assetlifeinsurancevalue]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_assetstotalliquid]  DEFAULT ((0)) FOR [assetstotalliquid]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_cashdepositamount]  DEFAULT ((0)) FOR [cashdepositamount]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_cashdepositholder]  DEFAULT ((0)) FOR [cashdepositholder]
GO
ALTER TABLE [dbo].[lpropertyloandata] ADD  CONSTRAINT [DF_lpropertyloandata_networth]  DEFAULT ((0)) FOR [networth]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_amortizationtype]  DEFAULT ((0)) FOR [amortizationtype]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_appraiser]  DEFAULT ((0)) FOR [appraiser]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_appraisedvalue]  DEFAULT ((0)) FOR [appraisedvalue]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_branchoffice]  DEFAULT ((0)) FOR [branchoffice]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_branchofficestate]  DEFAULT ((0)) FOR [branchofficestate]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_broker]  DEFAULT ((0)) FOR [broker]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_businesssource]  DEFAULT ((0)) FOR [businesssource]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_settlementdate]  DEFAULT (((1)/(1))/(1900)) FOR [settlementdate]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_disbursementdate]  DEFAULT (((1)/(1))/(1900)) FOR [disbursementdate]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_closingagent]  DEFAULT ((0)) FOR [closingagent]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_ltv]  DEFAULT ((0)) FOR [ltv]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_cltv]  DEFAULT ((0)) FOR [cltv]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_ratiodebt]  DEFAULT ((0)) FOR [ratiodebt]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_ratiohousing]  DEFAULT ((0)) FOR [ratiohousing]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_funder]  DEFAULT ((0)) FOR [funder]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_fundstoclose]  DEFAULT ((0)) FOR [fundstoclose]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_investor]  DEFAULT ((0)) FOR [investor]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_loanamount]  DEFAULT ((0)) FOR [loanamount]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_loaninterestrate]  DEFAULT ((0)) FOR [loaninterestrate]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_loanofficer]  DEFAULT ((0)) FOR [loanofficer]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_loanprogram]  DEFAULT ((0)) FOR [loanprogram]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_monthlypiamount]  DEFAULT ((0)) FOR [monthlypiamount]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_monthlymipayment]  DEFAULT ((0)) FOR [monthlymipayment]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_originator]  DEFAULT ((0)) FOR [originator]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_processor]  DEFAULT ((0)) FOR [processor]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_salesprice]  DEFAULT ((0)) FOR [salesprice]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_settlementagent]  DEFAULT ((0)) FOR [settlementagent]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_subclientname]  DEFAULT ((0)) FOR [subclientname]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_titlecompany]  DEFAULT ((0)) FOR [titlecompany]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_underwriter]  DEFAULT ((0)) FOR [underwriter]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_documentationtype]  DEFAULT ((0)) FOR [documentationtype]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_datecreated]  DEFAULT (getdate()) FOR [datecreated]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_propertytype_1]  DEFAULT ((0)) FOR [propertytype]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_ausdecision_1]  DEFAULT ((0)) FOR [ausdecision]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_nounits_1]  DEFAULT ((0)) FOR [nounits]
GO
ALTER TABLE [dbo].[lreportingdata] ADD  CONSTRAINT [DF_lreportingdata_qcanalyst]  DEFAULT ((0)) FOR [qcanalyst]
GO
