import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.OutputStream;
import java.util.Arrays;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;
import java.util.Map;
import java.util.regex.Pattern;
import java.text.SimpleDateFormat;
import java.util.Vector;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import org.apache.http.client.ClientProtocolException;
import org.dom4j.Node;
import org.jdom.Document;
import org.jdom.Element;
import org.jdom.JDOMException;
import org.jdom.input.SAXBuilder;
import org.jdom.output.XMLOutputter;
import org.jdom.xpath.XPath;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.ephesoft.dcma.script.IJDomScript;
import com.ephesoft.dcma.util.logger.EphesoftLogger;
import com.ephesoft.dcma.util.logger.ScriptLoggerFactory;
import com.ephesoft.dcma.core.component.ICommonConstants;
import com.ephesoft.dcma.util.ApplicationConfigProperties;
import com.mts.idc.ephesoft.BaseScript;
import com.mts.idc.ephesoft.LogLevel;
import com.mts.idc.ephesoft.LogUtil;
import com.mts.idc.ephesoft.service.QCIQLookupRequest;
import com.mts.idc.ephesoft.service.QCIQLookupWSClient;


import com.mts.idc.ephesoft.service.EphesoftUtilityRequest;
import com.mts.idc.ephesoft.service.EphesoftModule;
import com.mts.idc.ephesoft.service.EphesoftUtilityWSClient;

/**
 * The <code>ScriptAutomaticValidation</code> class represents the script execute structure. Writer of scripts plug-in should implement
 * this IScript interface to execute it from the scripting plug-in. Via implementing this interface writer can change its java file at
 * run time. Before the actual call of the java Scripting plug-in will compile the java and run the new class file.
 * 
 * @author Ephesoft
 * @version 1.0
 */
public class ScriptAutomaticValidation extends BaseScript {

	private static EphesoftLogger LOGGER = ScriptLoggerFactory.getLogger(ScriptAutomaticValidation.class);
	private static String DOCUMENT = "Document";
	private static String DOCUMENTS = "Documents";
	private static String DOCUMENT_LEVEL_FIELDS = "DocumentLevelFields";
	private static String TRUE = "true";
	private static String FALSE = "false";
	private static String TYPE = "Type";
	private static String VALUE = "Value";
	private static String BATCH_LOCAL_PATH = "BatchLocalPath";
	private static String BATCH_INSTANCE_ID = "BatchInstanceIdentifier";
	private static String EXT_BATCH_XML_FILE = "_batch.xml";
	private static String VALID = "Valid";
	private static String PATTERN = "dd/MM/yyyy";
	private static String DATE = "DATE";
	private static String LONG = "LONG";
	private static String DOUBLE = "DOUBLE";
	private static String STRING = "STRING";
	private static String ZIP_FILE_EXT = ".zip";
	
	private static final String REST_SERVICE_URL = "http://10.0.2.229/RevampEphesoftUtilityAPI/FieldMapping/Execute";
	private static final String AUTO_VALIDATION_URL = "http://10.0.2.229/RevampEphesoftUtilityAPI/AutoValidation/Execute";
	private static final String MAS_DOCUMENT_INGESTION_URL = "http://10.0.2.229/RevampEphesoftUtilityAPI/IntellaLendWrapper/UpdateLOSExportFileStaging";
	
	private static List<String> DT_92900LT_DDLB_KEYS = Arrays.asList("LDPGSA");
	private static List<String> DT_92900LT_DDLB_VALUES = Arrays.asList("No");
	
	private static List<String> DT_92900A_DDLB_KEYS = Arrays.asList("Allconditionssatisfied","FirstTimeBuyer","HUDFHAMortgage","LeadPaintPoisoning","MortgageeFinancialInterest","OccupancyHUD","ReasonableValueHUD");
	private static List<String> DT_92900A_DDLB_VALUES = Arrays.asList("Satisfied","Yes","Yes","Not Applicable","Do Not have interest","Occupy within 60 days","Yes");
	
	private static List<String> DT_1802_DDLB_KEYS = Arrays.asList("FirstTimeBuyer","HUDFHAMortgage","LeadPaintPoisoning","OccupancyHUD","ReasonableValueHUD");
	private static List<String> DT_1802_DDLB_VALUES = Arrays.asList("Yes","No","Not Applicable","Occupy within 60 days","Yes");
	
	private static List<String> APPRAISAL_DDLB_KEYS = Arrays.asList("ATT","DET","Existing","Proposed","SDetEndUnit","UnderConst","Units");
	private static List<String> APPRAISAL_DDLB_VALUES = Arrays.asList("No","Yes","Yes","No","No","No","One");
	
	private static List<String> FLD_DET_DDLB_KEYS = Arrays.asList("BuildinginFloodZone");
	private static List<String> FLD_DET_DDLB_VALUES = Arrays.asList("No");
	
	private static List<String> SEC_INST_DDLB_KEYS = Arrays.asList("AdjustableRateRider","BalloonRider","BiweeklyPaymentRider","CondominiumRider","FamilyRider14","Other","PlannedUnitDevelopmentRider","SecondHomeRider");
	private static List<String> SEC_INST_DDLB_VALUES = Arrays.asList("No","No","No","No","No","No","No","No");
	
	private static List<String> DT_1008_DDLB_KEYS = Arrays.asList("AUS","DU","LPA","ManualUnderwriting","OccupancyStatus","Other","PropertyType");
	private static List<String> DT_1008_DDLB_VALUES = Arrays.asList("Yes","Yes","No","No","Primary Residence","No","1 Unit");
	
	private static List<String> DT_1003_DDLB_FIELDS = Arrays.asList("AmortizationType","BorrowerAddressType","BorrowerEthnicity","BorrowerFormerAddressType","BorrowerFormerlySelfEmployed1","BorrowerFormerlySelfEmployed2","BorrowerMaritalStatus","BorrowerRefusal","BorrowerSelfEmployed","CoBorrowerAddressType","CoBorrowerEthnicity","CoBorrowerFormerAddressType","CoBorrowerFormerlySelfEmployed1","CoBorrowerFormerlySelfEmployed2","CoBorrowerMaritalStatus","CoBorrowerRefusal","CoBorrowerSelfEmployed","Completed","EstatewillbeheldinFeeSimple","MortgageType","PropertyType","PurposeOfLoan");
	private static List<String> DT_1003_DDLB_VALUES = Arrays.asList("FixedRate","Rent","NotHispanicOrLatino","Rent","No","No","Married","No","No","Rent","NotHispanicOrLatino","Rent","No","No","Married","No","No","Jointly","FeeSimple","Conventional","PrimaryResidence","Purchase");
	
	private static List<String> APPRAISAL_QC_DDLB_KEYS = Arrays.asList("IsMarketValueAccurate");
	private static List<String> APPRAISAL_QC_DDLB_VALUES = Arrays.asList("Yes");
	
	private LogUtil logUtil;
	private LogLevel logLevel = LogLevel.INFO;
	private Logger logger = LoggerFactory.getLogger(ScriptAutomaticValidation.class);

	/**
	 * The <code>execute</code> method will execute the script written by the writer at run time with new compilation of java file. It
	 * will execute the java file dynamically after new compilation.
	 * 
	 * @param document {@link Document}
	 */
	public Object execute(Document document, String methodName, String documentIdentifier) {
		Exception exception = null;
		try {
			LOGGER.info("*************  Inside ScriptAutomaticValidation scripts.");

			LOGGER.info("*************  Start execution of ScriptAutomaticValidation scripts.");

			if (null == document) {
				LOGGER.error("Input document is null.");
			}
			
			boolean isWrite = true;
			Element validNode = null;
			String valueText = null;
			String typeText = null;
			Element documents = document.getRootElement().getChild(DOCUMENTS);
			List<?> documentList = documents.getChildren(DOCUMENT);
			if (null != documentList) {
				for (int index = 0; index < documentList.size(); index++) {
					Element documentNode = (Element) documentList.get(index);
					if (null == documentNode) {
						continue;
					}
					List<?> childNodeList = documentNode.getChildren();
					if (null == childNodeList) {
						continue;
					}
					validNode = null;
					outerloop: for (int y = 0; y < childNodeList.size(); y++) {
						Element childDoc = (Element) childNodeList.get(y);
						if (null == childDoc) {
							continue;
						}
						String nodeName = childDoc.getName();
						if (null == nodeName) {
							continue;
						}
						if (nodeName.equals(VALID)) {
							validNode = childDoc;
						} else {
							if (nodeName.equals(DOCUMENT_LEVEL_FIELDS)) {
								List<?> dlfNodeList = childDoc.getChildren();
								if (null == dlfNodeList) {
									continue;
								}
								for (int dlf = 0; dlf < dlfNodeList.size(); dlf++) {
									Element dlfDoc = (Element) dlfNodeList.get(dlf);
									if (null == dlfDoc) {
										continue;
									}
									List<?> dlfValueNodeList = dlfDoc.getChildren();
									if (null == dlfValueNodeList) {
										continue;
									}
									valueText = null;
									typeText = null;
									for (int x = 0; x < dlfValueNodeList.size(); x++) {
										Element dlfValueDoc = (Element) dlfValueNodeList.get(x);
										if (null == dlfValueDoc) {
											continue;
										}
										String nName = dlfValueDoc.getName();
										if (nName.equals(VALUE)) {
											valueText = dlfValueDoc.getText();
										} else {
											if (nName.equals(TYPE)) {
												typeText = dlfValueDoc.getText();
											}
										}
										if (null != typeText) {
											boolean isValid = checkValueText(valueText, typeText);
											if (isValid) {
												// validNode.setText(TRUE);
												break;
											} else {
												validNode.setText(FALSE);
												break outerloop;
											}
										}
									}
								}
							}
						}

					}
				}

				// Write the document object to the xml file. Currently following IF block is commented for performance improvement.
				/*if (isWrite) {					
					writeToXML(document);
					LOGGER.info("*************  Successfully write the xml file for the ScriptAutomaticValidation scripts.");
				}*/
			}
			
			//Initialize 
			this.initialize(document);
			
			//QCIQ Lookup Call goes here
			this.QCIQLookupWSCall();
			//Write changes back to batch xml
						
			this.OrderBYStacking();
			
			ManageDropDownFields(document); // assign default values to drop down fields
			
			manageTables(document); // Manage table data
			iterateDel(document); // remove marked rows
			//mark all fields as valid Call goes here
			this.AutoValidationWSCall();
			
			this.MASDocumentIngestion(document);
			
			LOGGER.info("*************  End execution of the ScriptAutomaticValidation scripts.");
		} catch (Exception e) {
			LOGGER.error("*************  Error occurred in scripts." + e.getMessage());
			e.printStackTrace();
			exception = e;
		}
		return exception;
	}
	
	private void MASDocumentIngestion(Document document){
		try {
		EphesoftUtilityRequest requestContent = new EphesoftUtilityRequest();
		requestContent.ephesoftModule = EphesoftModule.AUTOMATEDVALIDATION.ordinal();
		XMLOutputter xmlOut = new XMLOutputter();
		requestContent.inputXML = xmlOut.outputString(document);
		//log(requestContent.inputXML);
		//Request and response block starts
			Document responseDoc = null;
				EphesoftUtilityWSClient client = new EphesoftUtilityWSClient();
				responseDoc = client.invokeEphesoftUtilityWS(requestContent, MAS_DOCUMENT_INGESTION_URL);
				log("Ephesoft Utility web service call for MAS Document Ingestion is successful.");
			 //} catch (Exception e) {

			if (responseDoc != null) {
				log("MAS Document Ingestion call got Successfull Response...");
			} else {
				// TODO 
				log("Response for MAS Document Ingestion call is Null.");
			}
			 }catch (Exception e) {
				 log("Error occurred while MAS Document Ingestion  - " + e.getMessage());
			e.printStackTrace();
		}
	}

	private void OrderBYStacking(){
		
		String REST_SERVICE_URL = "http://10.0.2.229/RevampEphesoftUtilityAPI/Document/OrderByStacking";		
		try {
		EphesoftUtilityRequest requestContent = new EphesoftUtilityRequest();
		requestContent.appendFlag = false; 
		requestContent.concatenateFlag = false; 
		requestContent.convertFlag = false;
		requestContent.pageSequenceFlag = false;
		requestContent.ephesoftModule = EphesoftModule.DOCUMENTASSEMBLY.ordinal();
		XMLOutputter xmlOut = new XMLOutputter();
		requestContent.inputXML = xmlOut.outputString(document);
		//log(requestContent.inputXML);
		
		//Request and response block starts
			Document responseDoc = null;
				EphesoftUtilityWSClient client = new EphesoftUtilityWSClient();
				responseDoc = client.invokeEphesoftUtilityWS(requestContent, REST_SERVICE_URL);
				log("Ephesoft Utility web service call successful.");
			 //} catch (Exception e) {

			if (responseDoc != null) {
				document.detachRootElement();
				if(!document.hasRootElement())
				{
					document.setRootElement(responseDoc.detachRootElement());
					this.documentUtil.writeToXML(document);
				}
			} else {
				// TODO 
				log("ResponseDoc is Null.");
			}
			 }catch (Exception e) {
			log("Error occurred - " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	/**
	 * @param message
	 */
	private void log(String message) {
		this.logUtil.log(message);
	}
	
	/**
	 * This method invokes the WS to make all the fields valid 
	 * 
	 */
	private void AutoValidationWSCall()
	{
		LOGGER.info("Automatic validation Inside the Ephesoft mark all field as valid web service call .");
		
		QCIQLookupRequest requestContent=new QCIQLookupRequest();
		XMLOutputter xmlOut = new XMLOutputter();
		requestContent.inputXML=xmlOut.outputString(document);
		requestContent.isManual = false;
		//LOGGER.info(requestContent.inputXML);
		Document responseDoc = null;
		QCIQLookupWSClient client=new QCIQLookupWSClient();
		try {
			responseDoc=client.invokeQCIQLookupWS(requestContent, AUTO_VALIDATION_URL);
		} catch (ClientProtocolException e) {
			// TODO Auto-generated catch block
			LOGGER.info("Error occured - " + e.getMessage());
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			LOGGER.info("Error occured - " + e.getMessage());
			e.printStackTrace();
		} catch (JDOMException e) {
			// TODO Auto-generated catch block
			LOGGER.info("Error occured - " + e.getMessage());
			e.printStackTrace();
		}
		
		if (responseDoc != null) {
			document.detachRootElement();
			if (!document.hasRootElement()) {
				document.setRootElement(responseDoc.detachRootElement());
				this.documentUtil.writeToXML(document);
			}
		} else {
			LOGGER.info("Response is Null. Ephesoft Utility Web service call failed.");
			//throw new Exception();
		}
		LOGGER.info("Ephesoft mark all field as valid web service call successful.");
		
	}
	/**
	 * This method invokes the WS to Validate the extracted document based on the QCIQ DB Lookup. 
	 * 
	 * @throws Exception
	 */
	
	private void QCIQLookupWSCall() {
		LOGGER.info("Automatic validation Inside the Ephesoft QCIQ Lookup web service call .");
		
		QCIQLookupRequest requestContent=new QCIQLookupRequest();
		XMLOutputter xmlOut = new XMLOutputter();
		requestContent.inputXML=xmlOut.outputString(document);
		requestContent.isManual = false;
		//LOGGER.info(requestContent.inputXML);
		Document responseDoc = null;
		QCIQLookupWSClient client=new QCIQLookupWSClient();
		try {
			responseDoc=client.invokeQCIQLookupWS(requestContent, REST_SERVICE_URL);
		} catch (ClientProtocolException e) {
			// TODO Auto-generated catch block
			LOGGER.info("Error occured - " + e.getMessage());
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			LOGGER.info("Error occured - " + e.getMessage());
			e.printStackTrace();
		} catch (JDOMException e) {
			// TODO Auto-generated catch block
			LOGGER.info("Error occured - " + e.getMessage());
			e.printStackTrace();
		}
		
		if (responseDoc != null) {
			document.detachRootElement();
			if (!document.hasRootElement()) {
				document.setRootElement(responseDoc.detachRootElement());
				this.documentUtil.writeToXML(document);
			}
		} else {
			LOGGER.info("Response is Null. Ephesoft Utility Web service call failed.");
			//throw new Exception();
		}
		LOGGER.info("Ephesoft QCIQ Lookup web service call successful.");
	}
	/**
	 * The <code>writeToXML</code> method will write the state document to the XML file.
	 * 
	 * @param document {@link Document}.
	 */
	private void writeToXML(Document document) {
		String batchLocalPath = null;
		List<?> batchLocalPathList = document.getRootElement().getChildren(BATCH_LOCAL_PATH);
		if (null != batchLocalPathList) {
			batchLocalPath = ((Element) batchLocalPathList.get(0)).getText();
		}

		if (null == batchLocalPath) {
			LOGGER.error("Unable to find the local folder path in batch xml file.");
			return;
		}

		String batchInstanceID = null;
		List<?> batchInstanceIDList = document.getRootElement().getChildren(BATCH_INSTANCE_ID);
		if (null != batchInstanceIDList) {
			batchInstanceID = ((Element) batchInstanceIDList.get(0)).getText();

		}

		if (null == batchInstanceID) {
			LOGGER.error("Unable to find the batch instance ID in batch xml file.");
			return;
		}

		String batchXMLPath = batchLocalPath.trim() + File.separator + batchInstanceID + File.separator + batchInstanceID
				+ EXT_BATCH_XML_FILE;

		boolean isZipSwitchOn = true;
		try {
			ApplicationConfigProperties prop = ApplicationConfigProperties.getApplicationConfigProperties();
			isZipSwitchOn = Boolean.parseBoolean(prop.getProperty(ICommonConstants.ZIP_SWITCH));
		} catch (IOException ioe) {
			LOGGER.error("Unable to read the zip switch value. Taking default value as true. Exception thrown is:" + ioe.getMessage(),
					ioe);
		}

		LOGGER.info("isZipSwitchOn************" + isZipSwitchOn);
		OutputStream outputStream = null;
		FileWriter writer = null;
		XMLOutputter out = new com.ephesoft.dcma.batch.encryption.util.BatchInstanceXmlOutputter(batchInstanceID);

		try {
			if (isZipSwitchOn) {
				LOGGER.info("Found the batch xml zip file.");
				outputStream = getOutputStreamFromZip(batchXMLPath, batchInstanceID + EXT_BATCH_XML_FILE);
				out.output(document, outputStream);
			} else {
				writer = new java.io.FileWriter(batchXMLPath);
				out.output(document, writer);
				writer.flush();
				writer.close();
			}
		} catch (Exception e) {
			LOGGER.error(e.getMessage());
		} finally {
			if (outputStream != null) {
				try {
					outputStream.close();
				} catch (IOException e) {
				}
			}
		}
	}

	/**
	 * The <code>checkValueText</code> method will check the valueText with typeText compatibility.
	 * 
	 * @param valueText {@link String}
	 * @param typeText {@link String}
	 * @return boolean true if pass the test otherwise false.
	 */
	private boolean checkValueText(String valueText, String typeText) {

		boolean isValid = false;
		if (null == valueText || "".equals(valueText)) {
			isValid = false;
		} else {
			if (typeText.equals(DATE)) {
				SimpleDateFormat format = new SimpleDateFormat(PATTERN);
				try {
					format.parse(valueText);
					isValid = true;
				} catch (Exception e) {
					// the value couldn't be parsed by the pattern, return
					// false.
					isValid = false;
				}
			} else {
				if (typeText.equals(LONG)) {
					try {
						Long.parseLong(valueText);
						isValid = true;
					} catch (Exception e) {
						// the value couldn't be parsed by the pattern, return
						// false
						isValid = false;
					}
				} else {
					if (typeText.equals(DOUBLE)) {
						try {
							Float.parseFloat(valueText);
							isValid = true;
						} catch (Exception e) {
							// the value couldn't be parsed by the pattern,
							// return false
							isValid = false;
						}
					} else {
						if (typeText.equals(STRING)) {
							isValid = true;
						} else {
							isValid = false;
						}
					}
				}
			}
		}

		return isValid;
	}

	public static OutputStream getOutputStreamFromZip(final String zipName, final String fileName) throws FileNotFoundException,
			IOException {
		ZipOutputStream stream = null;
		stream = new ZipOutputStream(new FileOutputStream(new File(zipName + ZIP_FILE_EXT)));
		ZipEntry zipEntry = new ZipEntry(fileName);
		stream.putNextEntry(zipEntry);
		return stream;
	}
	
	/**
	 *
	**/
	public void ManageDropDownFields(Document document)
	{
		try 
		{	
			Element root = document.getRootElement();
			List<Element> docList = root.getChild("Documents").getChildren("Document");
			for(Element doc : docList)	
			{
				String docTypeName = "";
				Element docTypeElement = doc.getChild("Type");
				if (docTypeElement != null)
				{
					docTypeName = docTypeElement.getText();
				}
				
				if (docTypeName.equals("92900-LT"))
					AssignDefaultDropdownValues(doc,docTypeName,DT_92900LT_DDLB_KEYS,DT_92900LT_DDLB_VALUES);
				else if (docTypeName.equals("FHA Initial Addendum to Loan Application 92900A"))
					AssignDefaultDropdownValues(doc,docTypeName,DT_92900A_DDLB_KEYS,DT_92900A_DDLB_VALUES);
				else if (docTypeName.equals("VA Addendum to Application 26-1802A"))
					AssignDefaultDropdownValues(doc,docTypeName,DT_1802_DDLB_KEYS,DT_1802_DDLB_VALUES);
				else if (docTypeName.equals("Appraisal"))
					AssignDefaultDropdownValues(doc,docTypeName,APPRAISAL_DDLB_KEYS,APPRAISAL_DDLB_VALUES);
				else if (docTypeName.equals("Flood Determination"))
					AssignDefaultDropdownValues(doc,docTypeName,FLD_DET_DDLB_KEYS,FLD_DET_DDLB_VALUES);
				else if (docTypeName.equals("Security Instrument"))
					AssignDefaultDropdownValues(doc,docTypeName,SEC_INST_DDLB_KEYS,SEC_INST_DDLB_VALUES);
				else if (docTypeName.equals("Transmittal Summary Final"))
					AssignDefaultDropdownValues(doc,docTypeName,DT_1008_DDLB_KEYS,DT_1008_DDLB_VALUES);
				else if ((docTypeName.equals("Loan Application 1003 Format 1")) || (docTypeName.equals("Loan Application 1003 Format 2")))
					AssignDefaultDropdownValues(doc,docTypeName,DT_1003_DDLB_FIELDS,DT_1003_DDLB_VALUES);
				else if (docTypeName.equals("Appraisal QC Review"))
					AssignDefaultDropdownValues(doc,docTypeName,APPRAISAL_QC_DDLB_KEYS,APPRAISAL_QC_DDLB_VALUES);
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in ManageDropDownFields: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void AssignDefaultDropdownValues(Element doc,String docTypeName,List<String> Keys, List<String> Values) {
		try
		{	
			int ind = 0;
			String fldValue = "";
			//String CoBorrowerName = "";
			
			Element dlFields = doc.getChild(DOCUMENT_LEVEL_FIELDS);
			//Element	validElement = doc.getChild("Valid");
			if (dlFields != null) {
				List<Element> allFields = dlFields.getChildren("DocumentLevelField");
				if (allFields != null) {
					for (Element fld : allFields) {					
						String name = fld.getChildText("Name");
						ind = 0;
						log("DocumentLevelField Name: " + name);
						for (String docfld : Keys) {
							if (docfld.equals(name)) {
								log("docfld matches name: " + docfld);
								log("number of items : " + Values.size());
								
								Element	valueElement = fld.getChild("Value");
								if (ind < Values.size()) {
									fldValue = Values.get(ind);
									if (valueElement != null) {
										valueElement.setText(fldValue);
										log("found and replaced : " + fldValue);
										break;
									}
								}
							}
							ind++;
						}
					}
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in AssignDefaultDropdownValues: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	/**
	 * methods to manage table extraction
	*/
	
	public void manageTables(Document document) {
		try 
		{	
			Element root = document.getRootElement();
			List<Element> docList = root.getChild("Documents").getChildren("Document");
			for(Element doc : docList)	
			{
				String docTypeName = "";
				Element docTypeElement = doc.getChild("Type");
				if (docTypeElement != null)
				{
					docTypeName = docTypeElement.getText();
				}
				
				if ((docTypeName.equals("Credit Report Lender")) || (docTypeName.equals("Credit Supplements")))
				{
					manageCreditReport(doc);
				}
					
				if ((docTypeName.equals("Loan Application 1003 Format 1")) || (docTypeName.equals("Loan Application 1003 Format 2")) ||
						(docTypeName.equals("Loan Application 1003 Continuation Sheet")))
				{
					manageLoanApp(doc);
				}
				
				if (docTypeName.equals("AUS Desktop UW Findings Report"))
				{
					manageAUS(doc);
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in Manage Tables: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void manageAUS(Element doc) {
		try
		{	
			String BorrowerName = "";
			String CoBorrowerName = "";
			
			Element dlFields = doc.getChild(DOCUMENT_LEVEL_FIELDS);
			Element	validElement = doc.getChild("Valid");
			if (dlFields != null) {
				List<Element> allFields = dlFields.getChildren("DocumentLevelField");
				if (allFields != null) {
					for (Element fld : allFields) {					
						String name = fld.getChildText("Name");
						if (name.equals("Borrower Name"))
							BorrowerName = fld.getChildText("Value");
						else if (name.equals("Co-Borrower"))
							CoBorrowerName = fld.getChildText("Value");					
					}
				}
			}
			
			//System.out.println("Manage AUS : BorrowerName : " + BorrowerName);
			//System.out.println("Manage AUS : CoBorrowerName : " + CoBorrowerName);
			
			List<Element> tables = null;			
			Element allTable = doc.getChild("DataTables");
			
			if (allTable != null)
				tables = allTable.getChildren("DataTable");			
			
			if(tables != null)
			{				
				for(Element table : tables) 
				{
					Element nameElement = table.getChild("Name");
					String tableName = nameElement.getText();
					
					if (tableName.equals("Sources of Income"))  // Sources of Income Table
					{						
						int nameMatch = manageAUSSourcesofIncome(table, BorrowerName, CoBorrowerName);
						if (nameMatch == 1) {
							if (validElement != null)
								validElement.setText("false");
						}
					}
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in Manage AUS: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public int manageAUSSourcesofIncome(Element table, String BorrowerName, String CoBorrowerName)
	{
		String ruleName = "";
		Element ruleNameElement = table.getChild("RuleName");
		if (ruleNameElement != null)
			ruleName = ruleNameElement.getText();		
		else
			ruleName = "Sources of Income";		
		
		int hasReview = 0;
		List<Element> rowList = null;
		Element allRows = table.getChild("Rows");
		
		if (allRows != null)
			rowList = allRows.getChildren("Row");
		
		if (rowList == null)
			return 0;
		
		if (ruleName.equals("Income")) {
			for(int i = 0; i < rowList.size(); i++)
			{
				Element nameColumn = getDesiredColumn(rowList.get(i),"Borrower");
				Element nameValue = nameColumn.getChild("Value");
				//System.out.println("Manage AUS : nameValue : " + nameValue.getText());
				
				if (!(nameValue.getText().equals(BorrowerName)) && (!(nameValue.getText().equals(CoBorrowerName)))) {
					Element validElement = nameColumn.getChild("Valid");
					Element forceReviewElement = nameColumn.getChild("ForceReview");
					
					if (validElement != null)
						validElement.setText("false");
					if (forceReviewElement != null)
						forceReviewElement.setText("true");
					hasReview = 1;
				}
			}
		}
		return hasReview;
	}
	
	public void manageLoanApp(Element doc) {
		try
		{	
			List<Element> tables = null;
			Element allTable = doc.getChild("DataTables");
			
			if (allTable != null)
				tables = allTable.getChildren("DataTable");			
			
			if(tables != null)
			{				
				for(Element table : tables) 
				{
					Element nameElement = table.getChild("Name");
					String tableName = nameElement.getText();
					
					if (tableName.equals("Liability Table"))  // Liability Table
					{						
						manageLoanAppLiabilityTable(table);
					}
					else if (tableName.equals("Assets Table"))  // Assets Table
					{
						manageLoanAppAssetTable(table);
					}
					else if (tableName.equals("Monthly Income"))  // Monthly Income
					{
						manageLoanAppIncomeTable(table);
					}
					else if (tableName.equals("Housing Expense Information"))  // Housing Expense Information
					{
						manageLoanAppExpenseTable(table);
					}
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageLoanApp: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void manageLoanAppExpenseTable(Element table)
	{
		String ruleName = "";
		Element ruleNameElement = table.getChild("RuleName");
		if (ruleNameElement != null) {
			ruleName = ruleNameElement.getText();			
		}
		else {
			ruleName = "Housing Expense Information";			
		}
		
		List<Element> rowList = null;
		Element allRows = table.getChild("Rows");
		
		if (allRows != null)
			rowList = allRows.getChildren("Row");
		
		if (rowList == null)
			return;
		
		if (ruleName.equals("Housing Expense Information")) {
			for(int i = 0; i < rowList.size(); i++)
			{
				if (i > 8) {
					rowList.get(i).addContent(new Element("ToDelete").setText("DEL"));
					continue;
				}
				Element expenseColumn = getDesiredColumn(rowList.get(i),"Housing Expense");
				Element expenseName = expenseColumn.getChild("Value");
				Element presentColumn = getDesiredColumn(rowList.get(i),"Present");
				Element presentAmount = presentColumn.getChild("Value");
				Element proposedColumn = getDesiredColumn(rowList.get(i),"Proposed");
				Element proposedAmount = proposedColumn.getChild("Value");				
				
				if (i == 0) {
					if (expenseName != null)
						expenseName.setText("Rent");
					else
						expenseColumn.addContent(new Element("Value").setText("Rent"));
				}
				
				if (i == 1) {
					if (expenseName != null)
						expenseName.setText("First Mortgage (P&I)");
					else
						expenseColumn.addContent(new Element("Value").setText("First Mortgage (P&I)"));
				}
				
				if (i == 2) {
					if (expenseName != null)
						expenseName.setText("Other Financing (P&I)");
					else
						expenseColumn.addContent(new Element("Value").setText("Other Financing (P&I)"));
				}
				
				if (i == 3) {
					if (expenseName != null)
						expenseName.setText("Hazard Insurance");
					else
						expenseColumn.addContent(new Element("Value").setText("Hazard Insurance"));
				}
				
				if (i == 4) {
					if (expenseName != null)
						expenseName.setText("Real Estate Taxes");
					else
						expenseColumn.addContent(new Element("Value").setText("Real Estate Taxes"));
				}
				
				if (i == 5) {
					if (expenseName != null)
						expenseName.setText("Mortgage Insurance");
					else
						expenseColumn.addContent(new Element("Value").setText("Mortgage Insurance"));
				}
					
				if (i == 6) {
					if (expenseName != null)
						expenseName.setText("Homeowner Assn. Dues");
					else
						expenseColumn.addContent(new Element("Value").setText("Homeowner Assn. Dues"));
				}
				
				if (i == 7) {
					if (expenseName != null)
						expenseName.setText("Other");
					else
						expenseColumn.addContent(new Element("Value").setText("Other"));
				}
				
				if (i == 8) {
					if (expenseName != null)
						expenseName.setText("Total");
					else
						expenseColumn.addContent(new Element("Value").setText("Total"));
				}
			}
		}
	}
	
	public void manageLoanAppIncomeTable(Element table)
	{
		String ruleName = "";
		Element ruleNameElement = table.getChild("RuleName");
		if (ruleNameElement != null) {
			ruleName = ruleNameElement.getText();			
		}
		else {
			ruleName = "Monthly Income";			
		}
		
		List<Element> rowList = null;
		Element allRows = table.getChild("Rows");
		
		if (allRows != null)
			rowList = allRows.getChildren("Row");
		
		if (rowList == null)
			return;
		
		if (ruleName.equals("Monthly Income")) {			
			for(int i = 0; i < rowList.size(); i++)
			{
				if (i > 7) {
					rowList.get(i).addContent(new Element("ToDelete").setText("DEL"));
					continue;
				}
				Element incomeColumn = getDesiredColumn(rowList.get(i),"Gross Monthly Income");
				Element incomeName = incomeColumn.getChild("Value");
				Element borrowerColumn = getDesiredColumn(rowList.get(i),"Borrower");
				Element borrowerAmount = borrowerColumn.getChild("Value");
				Element coborrowerColumn = getDesiredColumn(rowList.get(i),"Co-Borrower");
				Element coborrowerAmount = coborrowerColumn.getChild("Value");
				Element totalColumn = getDesiredColumn(rowList.get(i),"Total");
				Element totalAmount = totalColumn.getChild("Value");
				
				if (i == 0) {
					if (incomeName != null)
						incomeName.setText("Base Empl. Income");
					else
						incomeColumn.addContent(new Element("Value").setText("Base Empl. Income"));
					
					if (borrowerAmount != null) {
						if ((borrowerAmount.getTextTrim().equals("$")) && (totalAmount != null)) {							
							borrowerAmount.setText(totalAmount.getText());							
						}
					}
				}
				
				if (i == 1) {
					if (incomeName != null)
						incomeName.setText("Overtime");
					else
						incomeColumn.addContent(new Element("Value").setText("Overtime"));
				}
				
				if (i == 2) {
					if (incomeName != null)
						incomeName.setText("Bonuses");
					else
						incomeColumn.addContent(new Element("Value").setText("Bonuses"));
				}
				
				if (i == 3) {
					if (incomeName != null)
						incomeName.setText("Commissions");
					else
						incomeColumn.addContent(new Element("Value").setText("Commissions"));
				}
				
				if (i == 4) {
					if (incomeName != null)
						incomeName.setText("Dividends/Interest");
					else
						incomeColumn.addContent(new Element("Value").setText("Dividends/Interest"));
				}
				
				if (i == 5) {
					if (incomeName != null)
						incomeName.setText("Net Rental Income");
					else
						incomeColumn.addContent(new Element("Value").setText("Net Rental Income"));
				}
					
				if (i == 6) {
					if (incomeName != null)
						incomeName.setText("Other");
					else
						incomeColumn.addContent(new Element("Value").setText("Other"));
				}
				
				if (i == 7) {
					if (incomeName != null)
						incomeName.setText("Total");
					else
						incomeColumn.addContent(new Element("Value").setText("Total"));
					
					if (borrowerAmount != null)
						if ((borrowerAmount.getTextTrim().equals("$")) && (totalAmount != null))
							borrowerAmount.setText(totalAmount.getText());
				}
			}
		}
	}

	public void manageLoanAppAssetTable(Element table)
	{
		String ruleName = "";
		Element ruleNameElement = table.getChild("RuleName");
		if (ruleNameElement != null) {
			ruleName = ruleNameElement.getText();			
		}
		else {
			ruleName = "Assets";			
		}
		
		List<Element> rowList = null;
		Element allRows = table.getChild("Rows");
		
		if (allRows != null)
			rowList = allRows.getChildren("Row");
		
		if (rowList == null)
			return;
		
		String bankName = "";
		String cashAmount = "";
		String accountNumber = "";
		String accountNumber1 = "";
		String accountNumber2 = "";
		
		String bankHeaderRow = "N";
		String tmpValue = "";
		String lastRow = "N";
		for(Element row : rowList)
		{							
			if (ruleName.equals("Assets"))
			{
				Element bankNameColumn = getDesiredColumn(row,"Bank Name");
				Element bankNameValue = bankNameColumn.getChild("Value");								
				Element amountColumn = getDesiredColumn(row,"Cash Value");
				Element amountValue = amountColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				
				if (bankNameValue != null)
				{
					tmpValue = bankNameValue.getTextTrim();
					//log("tmpValue: " + tmpValue);
					
					if (tmpValue.length() > 0) 
					{
						if ((bankHeaderRow.equals("Y"))) { //&& (bankName.equals(""))) {
							bankName = tmpValue;
							bankHeaderRow = "N";
							
							//log("bankName: " + bankName);
							//log("Setting bankHeaderRow to N");
						}
						else
						{
							for (int i = 0; i < tmpValue.length(); i++) {
								if ((tmpValue.charAt(i) == ' ') && (accountNumber1.length() > 10)) {
									break;
								}
								if ((Character.isDigit(tmpValue.charAt(i))) || (tmpValue.charAt(i) == '*')) {
									accountNumber1 = accountNumber1 + Character.toString(tmpValue.charAt(i));
								}
							}
						}					
						
						if ((tmpValue.contains("Name")) || (tmpValue.toLowerCase().contains("address")) || (tmpValue.contains("ASSETS")) ||
								(tmpValue.contains("Credit Union")) || (tmpValue.contains("Acct"))) {
							bankHeaderRow = "Y";
							//log("Setting bankHeaderRow to Y");
						}
					}
						
					if ((tmpValue.contains("Acct")) || (tmpValue.contains("Aoot")) || (tmpValue.contains("Aid.")) || (tmpValue.contains("no.")) ||
							(tmpValue.contains("no "))) {
						if (bankName.trim().length() > 0) {
							lastRow = "Y";
						}
						//log("Setting lastRow to Y");
						for (int i = 0; i < tmpValue.length(); i++) {
							if ((tmpValue.charAt(i) == ' ') && (accountNumber2.length() > 10)) {
								break;
							}
							if ((Character.isDigit(tmpValue.charAt(i))) || (tmpValue.charAt(i) == '*')) {
								accountNumber2 = accountNumber2 + Character.toString(tmpValue.charAt(i));
							}
						}			
					}
				}
				
				if (amountValue != null) {
					tmpValue = amountValue.getText();
					cashAmount = "";
					for (int i = 0; i < tmpValue.length(); i++) {
						if ((Character.isDigit(tmpValue.charAt(i))) || (tmpValue.charAt(i) == '$') || (tmpValue.charAt(i) == ',')
								|| (tmpValue.charAt(i) == '.') || (tmpValue.charAt(i) == ' ')) {
							cashAmount = cashAmount + Character.toString(tmpValue.charAt(i));
						}
					}
					
					if ((cashAmount.trim().length() > 0) && (bankName.trim().length() > 0)) {
						lastRow = "Y";
					}
					
					//if ((tmpValue.trim().length() > 0) && (lastRow.equals("Y"))) {
					//	cashAmount = tmpValue;
					//}					
				}
				
				//log("bankName: " + bankName);
				//log("accountNumber: " + accountNumber);
				//log("cashAmount: " + cashAmount);
				
				if ((lastRow.equals("N")) || (bankName.toLowerCase().contains("name")) || (bankName.toLowerCase().contains("address")) || 
						(bankName.toLowerCase().contains("acct.")) || (bankName.toLowerCase().contains("account")))
				{
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
				else {
					if (accountNumber1.length() > 0)
						accountNumber = accountNumber1;
					if (accountNumber2.length() > 0)
						accountNumber = accountNumber2;
					
					if (bankNameValue != null)
						bankNameValue.setText(bankName);
					else
						bankNameColumn.addContent(new Element("Value").setText(bankName));
					
					if (amountValue != null)
						amountValue.setText(cashAmount);
					else
						amountColumn.addContent(new Element("Value").setText(cashAmount));
					
					if (acctNoValue != null)
						acctNoValue.setText(accountNumber);
					else
						acctNoColumn.addContent(new Element("Value").setText(accountNumber));
					
					// remove row if it is only the last row
					
					if ((bankName.contains("Acct")) || (bankName.contains("Aoot")) || (bankName.contains("Aid.")) || (bankName.contains("no.")) ||
							(bankName.contains("no "))) {
						if (cashAmount.equals("") && accountNumber.equals("")) {
							row.addContent(new Element("ToDelete").setText("DEL"));
						}
					}

					bankName = "";
					cashAmount = "";
					accountNumber = "";
					accountNumber1 = "";
					accountNumber2 = "";
					lastRow = "N";
				}
			}
		}
	}
	
	public void manageNonTabularFormat(Element table)
	{
		List<Element> rowList = null;
		Element allRows = table.getChild("Rows");
		
		if (allRows != null)
			rowList = allRows.getChildren("Row");
		
		if (rowList == null)
			return;		
		
		String tmpAmount = "";		
		String tmpAcctNo = "";
		String RowDel = "N";
		
		for(Element row : rowList)
		{
			Element acctNameColumn = getDesiredColumn(row,"Company Name");
			Element acctNameValue = acctNameColumn.getChild("Value");								
			Element amountColumn = getDesiredColumn(row,"Monthly Payment");
			Element amountValue = amountColumn.getChild("Value");
			Element acctNoColumn = getDesiredColumn(row,"Account Number");
			Element acctNoValue = acctNoColumn.getChild("Value");
			
			if (RowDel.equals("Y"))
			{
				RowDel = "N";
				if (amountValue != null)
					amountValue.setText(tmpAmount);
				else
					amountColumn.addContent(new Element("Value").setText(tmpAmount));
				
				if (acctNoValue != null)
					acctNoValue.setText(tmpAcctNo);
				else
					acctNoColumn.addContent(new Element("Value").setText(tmpAcctNo));
				//log("manageNonTabularFormat: Assigning data");
				continue;
			}
			
			if ((amountValue != null) && (acctNoValue != null))
			{				
				tmpAmount = amountValue.getTextTrim();
				tmpAcctNo = acctNoValue.getTextTrim();
				
				if ((RowDel.equals("N")) && (tmpAmount.length() > 0) && (tmpAcctNo.length() > 0))  
				{ //first row
					//log("manageNonTabularFormat: Remove First Row"); 
					RowDel = "Y";
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
			}
		}
	}
	
	public void manageLoanAppLiabilityTable(Element table)
	{
		String ruleName = "";
		Element ruleNameElement = table.getChild("RuleName");
		if (ruleNameElement != null) {
			ruleName = ruleNameElement.getText();			
		}
		else {
			ruleName = "Liabilities Short Format";			
		}
		
		//log("ruleName: " + ruleName);
		
		if (ruleName.equals("Liability Table")) {
			manageNonTabularFormat(table);
			return;
		}
		
		List<Element> rowList = null;
		Element allRows = table.getChild("Rows");
		
		if (allRows != null)
			rowList = allRows.getChildren("Row");
		
		if (rowList == null)
			return;
		
		String companyValue = "";
		String paymentValue = "";
		String monthValue = "";
		String acctValue1 = "";
		String acctValue2 = "";
		String acctValue = "";
		String RowAdded = "N";
		String companyHeaderRow = "Y";
		String paymentFound = "N";
		String tmpValue = "";
		String lastRow = "N";
		for(Element row : rowList)
		{							
			if ((ruleName.equals("Liabilities Short Format")) || (ruleName.equals("Liabilities Long Format")))
			{
				Element acctNameColumn = getDesiredColumn(row,"Company Name");
				Element acctNameValue = acctNameColumn.getChild("Value");								
				Element amountColumn = getDesiredColumn(row,"Monthly Payment");
				Element amountValue = amountColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				
				if (acctNameValue != null)
				{
					tmpValue = acctNameValue.getTextTrim();
					if (tmpValue.length() > 0) 
					{
						//log("acctNameValue: " + tmpValue);
						if (((companyHeaderRow.equals("Y")) || (RowAdded.equals("Y"))) && (companyValue.equals(""))) {
							// remove amount if present in the company name
							
							for (int i = 0; i < tmpValue.length(); i++) {
								if ((Character.isLetter(tmpValue.charAt(i)))) {
									companyValue = tmpValue.substring(i);
									break;
								}
							}							
							companyHeaderRow = "N";
							RowAdded = "N";
							//log("companyValue: " + companyValue);
						}
						else
						{
							for (int i = 0; i < tmpValue.length(); i++) {
								if ((Character.isDigit(tmpValue.charAt(i))) || (tmpValue.charAt(i) == '*')) {
									acctValue1 = acctValue1 + Character.toString(tmpValue.charAt(i));
								}
							}
							if (acctValue1.length() > 5) {
								//log("acctValue 1: " + acctValue1);
								lastRow = "Y";
							}
						}
						
						if ((tmpValue.contains("Name")) || (tmpValue.toLowerCase().contains("address")) || (tmpValue.contains("Company")) ||
								(tmpValue.contains("Compan")) || (tmpValue.contains("addiess"))) {
							companyHeaderRow = "Y";
							companyValue = ""; // 03-21, clear the value if any so it will be assigned from next row
						}
					}
						
					if ((tmpValue.toLowerCase().contains("acct")) || (tmpValue.toLowerCase().contains("aoot")) || (tmpValue.toLowerCase().contains("aid.")) || 
							(tmpValue.toLowerCase().contains("no.")) || (tmpValue.toLowerCase().contains("ac<,t"))) {
						lastRow = "Y";
						//log("Setting lastRow to True 2");
						int st = tmpValue.indexOf("Acct");
						if (st == -1)
							st = tmpValue.indexOf("Aoot");
						if (st == -1)
							st = tmpValue.indexOf("Aid.");
						if (st == -1)
							st = tmpValue.indexOf("no.");
						if (st == -1)
							st = tmpValue.indexOf("No.");
						if (st == -1)
							st = 0;						
						for (int i = st; i < tmpValue.length(); i++) {
							if ((Character.isDigit(tmpValue.charAt(i))) || (tmpValue.charAt(i) == '*')) {
								acctValue2 = acctValue2 + Character.toString(tmpValue.charAt(i));
							}
							//log("acctValue 2: " + acctValue2);
						}
					}
				}
				
				if (amountValue != null) {
					tmpValue = amountValue.getText();
					//log("amountValue: " + tmpValue);
					if ((tmpValue.trim().length() > 0) && (!tmpValue.contains("Pay")) && (!tmpValue.contains("Month")) && 
								(!tmpValue.contains("Form")) && (!tmpValue.contains("Fannie")) && (!tmpValue.contains("Pmt")) &&
								(!tmpValue.contains("Mos")) && (!tmpValue.contains("Poit"))) {
						if (paymentFound.equals("N")) {							
							//paymentValue = tmpValue;							
							String sValues[] = tmpValue.split("/");
							if (sValues.length > 0) {								
								paymentValue = tmpValue.split("/")[0];
								paymentValue = paymentValue.trim();
								if (paymentValue.length() > 0)
									paymentFound = "Y";
							}
						}
						else {
							monthValue = tmpValue;
							//if (paymentValue.length() > 0)
							//	paymentValue = paymentValue + "/" + monthValue;
						}
					}
				}
				
				if ((lastRow.equals("N")) || (paymentFound.equals("N")) || (paymentValue.equals("0.00")) || (paymentValue.equals("0")) || (paymentValue.equals("0.0")) 
							|| (paymentValue.equals("$0.00")) || (paymentValue.equals("$0.0")) || (paymentValue.equals("$0"))
							|| (paymentValue.equals("(0.00)")) || (paymentValue.equals("($0.00)")))
				{
					row.addContent(new Element("ToDelete").setText("DEL"));
					if (lastRow.equals("Y")) {
						companyValue = "";
						paymentValue = "";
						monthValue = "";
						acctValue1 = "";
						acctValue2 = "";
						acctValue = "";
						lastRow = "N";
						paymentFound = "N";
						RowAdded = "Y";
					}
				}
				else {
					if (acctValue1.length() > 0)
						acctValue = acctValue1;
					if (acctValue2.length() > 0)
						acctValue = acctValue2;
					
					if (acctNameValue != null)
						acctNameValue.setText(companyValue);
					else
						acctNameColumn.addContent(new Element("Value").setText(companyValue));
					
					if (amountValue != null)
						amountValue.setText(paymentValue);
					else
						amountColumn.addContent(new Element("Value").setText(paymentValue));
					
					if (acctNoValue != null)
						acctNoValue.setText(acctValue);
					else
						acctNoColumn.addContent(new Element("Value").setText(acctValue));
					
					// remove row if it is only the last row
					
					if ((companyValue.contains("Acct")) || (companyValue.contains("Aoot")) || (companyValue.contains("Aid.")) || 
								(companyValue.contains("no.")) || (companyValue.contains("No.")) || (companyValue.contains("Ac<,t"))) {
						if (paymentValue.equals("") && acctValue.equals("")) {
							row.addContent(new Element("ToDelete").setText("DEL"));
						}
					}

					companyValue = "";
					paymentValue = "";
					monthValue = "";
					acctValue1 = "";
					acctValue2 = "";
					acctValue = "";
					lastRow = "N";
					paymentFound = "N";
					RowAdded = "Y";
				}
			}
		}		
	}
	
	public void manageCRAcroNetFormat(Element table)
	{
		try
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null)
				return;
						
			String rowDEL = "N";
				
			String acctName = "";
			String acctNumber = "";
			String rptdDate = "";
			String balAmount = "";
			String paymentAmount = "";
			String termsName = "";
			String zeroBalance = "";
			int RPTDCount = 0;
			
			for(Element row : rowList)
			{	
				Element acctNameColumn = getDesiredColumn(row,"Account Name");
				Element acctNameValue = acctNameColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				Element rptdColumn = getDesiredColumn(row,"Reported Date");
				Element rptdValue = rptdColumn.getChild("Value");
				Element balColumn = getDesiredColumn(row,"Balance");
				Element balValue = balColumn.getChild("Value");
				Element termsColumn = getDesiredColumn(row,"Payment Amount");
				Element termsValue = termsColumn.getChild("Value");
				Element typeColumn = getDesiredColumn(row,"Terms");
				Element typeValue = typeColumn.getChild("Value");
				rowDEL = "Y";
				
				//if ((rptdValue != null) && (((balValue != null) || (termsValue != null)) || (RPTDCount > 0))) {
				if ((rptdValue != null) && ((balValue != null) || (RPTDCount > 0))) {
					RPTDCount++;
					
					if (RPTDCount == 3) {
						//RPTDCount = 0;
						rowDEL = "N";
					}
				}
				
				if (RPTDCount == 1)
				{
					if (acctNameValue != null)
						acctName = acctNameValue.getTextTrim();
					
					if (rptdValue != null)
						rptdDate = rptdValue.getTextTrim();
					
					if (typeValue != null)
						termsName = typeValue.getTextTrim();
				}
				
				if (RPTDCount == 2)
				{
					if (balValue != null) {
						balAmount = balValue.getTextTrim();
						zeroBalance = balAmount; 
					}
					
					if (typeValue != null)
						paymentAmount = typeValue.getTextTrim();
					
					if (acctNoValue != null)
						acctNumber = acctNoValue.getTextTrim();
				}	
								
				/* System.out.println("CR AcroNet Data: Log all fields: Start");
				if (acctNameValue != null)
					System.out.println("acctNameValue: " + acctNameValue.getText());
				if (acctNoValue != null)
					System.out.println("acctNoValue: " + acctNoValue.getText());
				if (rptdValue != null)
					System.out.println("rptdValue: " + rptdValue.getText());
				if (balValue != null)
					System.out.println("balValue: " + balValue.getText());
				if (termsValue != null)
					System.out.println("termsValue: " + termsValue.getText());
				if (typeValue != null)
					System.out.println("typeValue: " + typeValue.getText());

				System.out.println("CR AcroNet Data: Log all fields: End"); */
				
				if ((rowDEL.equals("Y")) || ((RPTDCount == 3) && ((zeroBalance.equals("0")) || (zeroBalance.equals("$0")))))
				{					
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
				else //if ((rowDEL.equals("N")) && (acctNoCount > 0) && (acctNameCount > 0)) 
				{					
					if (acctNameValue == null)
						acctNameColumn.addContent(new Element("Value").setText(acctName));
					else
						acctNameValue.setText(acctName);
					
					if (rptdDate.length() > 6) // mm/dd mm/dd
						rptdDate = rptdDate.substring(6);
					if (rptdValue == null)
						rptdColumn.addContent(new Element("Value").setText(rptdDate));
					else
						rptdValue.setText(rptdDate);
					
					if (balValue == null)
						balColumn.addContent(new Element("Value").setText(balAmount));
					else
						balValue.setText(balAmount);
					
					if (termsValue == null)
						termsColumn.addContent(new Element("Value").setText(paymentAmount));
					else
						termsValue.setText(paymentAmount);
					
					if (typeValue == null)
						typeColumn.addContent(new Element("Value").setText(termsName));
					else
						typeValue.setText(termsName);
					
					if (acctNoValue == null)
						acctNoColumn.addContent(new Element("Value").setText(acctNumber));
					else
						acctNoValue.setText(acctNumber);
				}
				
				if (RPTDCount == 3) {
					acctName = "";
					acctNumber = "";
					rptdDate = "";
					balAmount = "";
					paymentAmount = "";
					termsName = "";
					RPTDCount = 0;
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageCRAcroNetFormat Credit Report: " + e.getMessage());
			e.printStackTrace();
		}		
	}
	
	public void manageCRAcroNetNewFormat(Element table)
	{
		try
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null)
				return;
						
			String rowDEL = "N";
				
			String acctName = "";
			String acctNumber = "";
			String rptdDate = "";
			String balAmount = "";
			String paymentAmount = "";
			String termsName = "";
			String zeroBalance = "";
			int RPTDCount = 0;
			
			for(Element row : rowList)
			{	
				Element acctNameColumn = getDesiredColumn(row,"Account Name");
				Element acctNameValue = acctNameColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				Element rptdColumn = getDesiredColumn(row,"Reported Date");
				Element rptdValue = rptdColumn.getChild("Value");
				Element balColumn = getDesiredColumn(row,"Balance");
				Element balValue = balColumn.getChild("Value");
				Element paymentColumn = getDesiredColumn(row,"Payment Amount");
				Element paymentValue = paymentColumn.getChild("Value");
				Element termsColumn = getDesiredColumn(row,"Terms");
				Element termsValue = termsColumn.getChild("Value");
				rowDEL = "Y";
				
				if ((rptdValue != null) || (RPTDCount > 0)) {
					RPTDCount++;
					
					if (RPTDCount == 2) {						
						rowDEL = "N";
					}
				}
				
				if (RPTDCount == 1)
				{
					if (acctNameValue != null)
						acctName = acctNameValue.getTextTrim();
					
					if (rptdValue != null)
						rptdDate = rptdValue.getTextTrim();
					
					if (balValue != null) {
						balAmount = balValue.getTextTrim();
						zeroBalance = balAmount; 
					}					
				}
				
				if (RPTDCount == 2)
				{
					if (termsValue != null)
						termsName = termsValue.getTextTrim();
					
					if (paymentValue != null) {
						paymentAmount = GetTypeAndTerms(paymentValue.getTextTrim());
						String sValues[] = paymentAmount.split(",");
						if (sValues.length > 0)								
							termsName = paymentAmount.split(",")[0];
						if (sValues.length > 1)
							paymentAmount = paymentAmount.split(",")[1];						
					}
					
					if (acctNameValue != null)
						acctNumber = acctNameValue.getTextTrim();
				}	
								
				/*System.out.println("Log all fields: Start");
				if (acctNameValue != null)
					System.out.println("acctNameValue: " + acctNameValue.getText());
				if (acctNoValue != null)
					System.out.println("acctNoValue: " + acctNoValue.getText());
				if (rptdValue != null)
					System.out.println("rptdValue: " + rptdValue.getText());
				if (balValue != null)
					System.out.println("balValue: " + balValue.getText());
				if (termsValue != null)
					System.out.println("termsValue: " + termsValue.getText());
				if (paymentValue != null)
					System.out.println("paymentValue: " + paymentValue.getText());

				System.out.println("Log all fields: End");*/
				
				if ((rowDEL.equals("Y")) || (zeroBalance.equals("0")) || (zeroBalance.equals("$0")))
				{					
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
				else 
				{					
					if (acctNameValue == null)
						acctNameColumn.addContent(new Element("Value").setText(acctName));
					else
						acctNameValue.setText(acctName);
					
					if (rptdDate.length() > 7) // mm/yyyy
						rptdDate = rptdDate.substring(7);
					if (rptdValue == null)
						rptdColumn.addContent(new Element("Value").setText(rptdDate));
					else
						rptdValue.setText(rptdDate);
					
					if (balValue == null)
						balColumn.addContent(new Element("Value").setText(balAmount));
					else
						balValue.setText(balAmount);
					
					if (paymentValue == null)
						paymentColumn.addContent(new Element("Value").setText(paymentAmount));
					else
						paymentValue.setText(paymentAmount);
					
					if (termsValue == null)
						termsColumn.addContent(new Element("Value").setText(termsName));
					else
						termsValue.setText(termsName);
					
					if (acctNoValue == null)
						acctNoColumn.addContent(new Element("Value").setText(acctNumber));
					else
						acctNoValue.setText(acctNumber);
				}
				
				if (RPTDCount == 2) {
					acctName = "";
					acctNumber = "";
					rptdDate = "";
					balAmount = "";
					zeroBalance = "";
					paymentAmount = "";
					termsName = "";
					RPTDCount = 0;
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageCRAcroNetNewFormat Credit Report: " + e.getMessage());
			e.printStackTrace();
		}		
	}
	
	public void manageCRTextTrendCP(Element table)
	{
		try
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null)
				return;
						
			String rowDEL = "N";
				
			String acctName = "";
			String acctNumber = "";
			String rptdDate = "";
			String balAmount = "";
			String paymentAmount = "";
			String termsName = "";
			String zeroBalance = "";
			int RPTDCount = 0;
			
			for(Element row : rowList)
			{	
				Element acctNameColumn = getDesiredColumn(row,"Account Name");
				Element acctNameValue = acctNameColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				Element rptdColumn = getDesiredColumn(row,"Reported Date");
				Element rptdValue = rptdColumn.getChild("Value");
				Element balColumn = getDesiredColumn(row,"Balance");
				Element balValue = balColumn.getChild("Value");
				Element termsColumn = getDesiredColumn(row,"Payment Amount");
				Element termsValue = termsColumn.getChild("Value");
				Element typeColumn = getDesiredColumn(row,"Terms");
				Element typeValue = typeColumn.getChild("Value");
				rowDEL = "Y";
				
				if ((rptdValue != null)) {
					RPTDCount++;
					
					if (RPTDCount == 2) {
						//RPTDCount = 0;
						rowDEL = "N";
					}
				}
				else
				{
					if (RPTDCount == 1) {
						rowDEL = "N";
					}
					//RPTDCount = 0;
				}
				
				if (RPTDCount == 1)
				{
					if (balValue != null) { // first row
						if (acctNameValue != null)
							acctName = acctNameValue.getTextTrim();
						
						if (rptdValue != null)
							rptdDate = rptdValue.getTextTrim();
						
						if (balValue != null) {
							balAmount = balValue.getTextTrim();
							zeroBalance = balAmount; 
						}
					}
					else { // second row
						if (rptdValue != null)
							rptdDate = rptdValue.getTextTrim();
						
						if (typeValue != null)
							termsName = typeValue.getTextTrim();
					
						if (termsValue != null)
							paymentAmount = termsValue.getTextTrim();
						
						if (acctNameValue != null)
							acctNumber = acctNameValue.getTextTrim();
					}
				}
				
				if (RPTDCount == 2)
				{
					if (typeValue != null)
						termsName = typeValue.getTextTrim();
					
					if (termsValue != null)
						paymentAmount = termsValue.getTextTrim();
					
					if (acctNameValue != null)
						acctNumber = acctNameValue.getTextTrim();
				}	
								
				/* System.out.println("CR manageCRTextTrended Data: Log all fields: Start");
				if (acctNameValue != null)
					System.out.println("acctNameValue: " + acctNameValue.getText());
				if (acctNoValue != null)
					System.out.println("acctNoValue: " + acctNoValue.getText());
				if (rptdValue != null)
					System.out.println("rptdValue: " + rptdValue.getText());
				if (balValue != null)
					System.out.println("balValue: " + balValue.getText());
				if (termsValue != null)
					System.out.println("termsValue: " + termsValue.getText());
				if (typeValue != null)
					System.out.println("typeValue: " + typeValue.getText());

				System.out.println("CR manageCRTextTrended Data: Log all fields: End"); */
				
				if ((rowDEL.equals("Y")) || ((RPTDCount > 0) && ((zeroBalance.equals("0")) || (zeroBalance.equals("$0")))))
				{					
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
				else //if ((rowDEL.equals("N")) && (acctNoCount > 0) && (acctNameCount > 0)) 
				{					
					if (acctNameValue == null)
						acctNameColumn.addContent(new Element("Value").setText(acctName));
					else
						acctNameValue.setText(acctName);
					
					if (rptdDate.length() > 6) // mm/dd mm/dd
						rptdDate = rptdDate.substring(6);
					if (rptdValue == null)
						rptdColumn.addContent(new Element("Value").setText(rptdDate));
					else
						rptdValue.setText(rptdDate);
					
					if (balValue == null)
						balColumn.addContent(new Element("Value").setText(balAmount));
					else
						balValue.setText(balAmount);
					
					if (termsValue == null)
						termsColumn.addContent(new Element("Value").setText(paymentAmount));
					else
						termsValue.setText(paymentAmount);
					
					if (typeValue == null)
						typeColumn.addContent(new Element("Value").setText(termsName));
					else
						typeValue.setText(termsName);
					
					if (acctNoValue == null)
						acctNoColumn.addContent(new Element("Value").setText(acctNumber));
					else
						acctNoValue.setText(acctNumber);
				}
				
				if (rowDEL.equals("N")) {
					acctName = "";
					acctNumber = "";
					rptdDate = "";
					balAmount = "";
					paymentAmount = "";
					termsName = "";
					RPTDCount = 0;
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageCRTextTrended Credit Report: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void manageFraudAlert(Element table) {
		try 
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null)
				return;
			
			int rowCount = 0;
			for(Element row : rowList)
			{	
				rowCount++;
				
				if (rowCount > 10) {
					row.addContent(new Element("ToDelete").setText("DEL"));		
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageFraudAlert Credit Report: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void manageCreditReport(Element doc) {
		try
		{
			String ruleName = "";			
			List<Element> tables = null;
			
			Element allTables = doc.getChild("DataTables");
			
			if (allTables != null)
				tables = allTables.getChildren("DataTable");
			
			if(tables != null)
			{				
				//System.out.println(tables.size());
				for(Element table : tables) 
				{
					String tableName = "";
					Element nameElement = table.getChild("Name");
					if (nameElement != null)
						tableName = nameElement.getText();
					
					if (tableName.equals("Fraud Alert"))  // Fraud Alert
					{
						manageFraudAlert(table);
					}
					
					if (tableName.equals("Trade Info"))  // Trade Info
					{	
						// check for CRCommonFormat rule
						Element prevTypeColumn = null;
						Element prevAcctColumn = null;
						Element prevTermsColumn = null;
						Element ruleNameElement = table.getChild("RuleName");
						if (ruleNameElement != null) {
							ruleName = ruleNameElement.getText();
						}
						else {
							ruleName = "CRCommonFormat";
						}
						
						if (ruleName.equals("CRTextFormat")) {
							manageCRTextFormat(table);
							continue;
						}
						
						if (ruleName.equals("CRFactualData")) {
							manageCRFactualDataFormat(table);
							continue;
						}
						
						if (ruleName.equals("CRTextTrend")) {
							manageCRTextTrendFormat(table);
							continue;
						}
						
						if (ruleName.equals("CRCredco")) {
							manageCRCredcoFormat(table);
							continue;
						}
						
						if (ruleName.equals("CRAcroNet")) {
							manageCRAcroNetFormat(table);
							continue;
						}
						
						if (ruleName.equals("CRAcroNetNew")) {
							manageCRAcroNetNewFormat(table);
							continue;
						}
						
						if (ruleName.equals("CRCabrillo")) {
							manageCRCabrillo(table);
							continue;
						}
						
						if (ruleName.equals("CRTextTrendCP")) {
							manageCRTextTrendCP(table);
							continue;
						}
						
						if (ruleName.equals("CRChronos")) {
							manageCRChronosFormat(table);
							continue;
						}
						
						List<Element> rowList = null;
						Element allRows = table.getChild("Rows");
						
						if (allRows != null)
							rowList = allRows.getChildren("Row");
						
						if (rowList == null)
							continue;
						
						String rowFlag = "Y";
						String rowDEL = "N";
						String strRPTD = "";
						String strFinalRPTD = "";
						String zeroBalance = "";
						for(Element row : rowList)
						{
							if ((ruleName.equals("CRCommonFormat")) || (ruleName.equals("CRNewFormat")))
							{
								Element acctNameColumn = getDesiredColumn(row,"Account Name");
								Element acctNameValue = acctNameColumn.getChild("Value");
								Element acctNoColumn = getDesiredColumn(row,"Account Number");
								Element acctNoValue = acctNoColumn.getChild("Value");
								Element rptdColumn = getDesiredColumn(row,"Reported Date");
								Element rptdValue = rptdColumn.getChild("Value");
								Element balColumn = getDesiredColumn(row,"Balance");
								Element balValue = balColumn.getChild("Value");
								Element termsColumn = getDesiredColumn(row,"Payment Amount");
								Element termsValue = termsColumn.getChild("Value");
								Element typeColumn = getDesiredColumn(row,"Terms");
								Element typeValue = typeColumn.getChild("Value");
																
								if (balValue == null)
									zeroBalance = "";
								else
									zeroBalance = balValue.getTextTrim();
								
								// if 0 reads as () convert to 0
								if (rptdValue != null) {
									strRPTD = rptdValue.getText();
									strFinalRPTD = ReplaceParenthesisWithZero(strRPTD);
									rptdValue.setText(strFinalRPTD);
								}
								
								//String typeValueString = typeValue.getText();
								
								/*System.out.println("CR NEW FORMAT: Log all fields: Start");
								if (acctNameValue != null)
									System.out.println("acctNameValue: " + acctNameValue.getText());
								if (acctNoValue != null)
									System.out.println("acctNoValue: " + acctNoValue.getText());
								if (rptdValue != null)
									System.out.println("rptdValue: " + rptdValue.getText());
								if (balValue != null)
									System.out.println("balValue: " + balValue.getText());
								if (termsValue != null)
									System.out.println("termsValue: " + termsValue.getText());
								if (typeValue != null)
									System.out.println("typeValue: " + typeValue.getText());
				
								System.out.println("CR NEW FORMAT: Log all fields: End");*/
								
								if (rowFlag.equals("N")) {
									if (typeValue != null) {
										if (prevTypeColumn != null) {											
											prevTypeColumn.addContent(new Element("Value").setText(typeValue.getText()));
											rowDEL = "Y";
											//System.out.println("CR NEW Format: typeValue: " + typeValue.getText());
										}
									}
									if (acctNameValue != null) {
										if (prevAcctColumn != null) {								
											prevAcctColumn.addContent(new Element("Value").setText(ReplaceParenthesisWithZero(acctNameValue.getText())));
											rowDEL = "Y";
											//System.out.println("CR NEW Format: acctNameValue: " + acctNameValue.getText());
										}
									}
									if ((ruleName.equals("CRNewFormat")) && (termsValue != null)) {
										if (prevTermsColumn != null) {											
											prevTermsColumn.addContent(new Element("Value").setText(MatchandGetOuterString(termsValue.getText(), "X", "$")));
											rowDEL = "Y";
											//System.out.println("CR NEW Format: termsValue: " + termsValue.getText());
										}
									}
									rowFlag = "Y";
								}
								
								if ((rptdValue != null) && (typeValue == null) && (rowDEL.equals("N"))) { 
									// good row without type value
									prevTypeColumn = typeColumn;
									rowFlag = "N";
									//System.out.println("CR NEW Format: SET termsValue");
								}
								
								if ((rptdValue != null) && (acctNoValue == null) && (rowDEL.equals("N"))) { 
									// good row without type value
									prevAcctColumn = acctNoColumn;
									rowFlag = "N";
									//System.out.println("CR NEW Format: SET acctNoValue");
								}
								
								if ((rptdValue != null) && (ruleName.equals("CRNewFormat")) && (rowDEL.equals("N"))) { 
									// good row without type value
									prevTermsColumn = termsColumn;
									rowFlag = "N";
									//System.out.println("CR NEW Format: SET termsColumn");
									
									// copy terms to balance if balance is empty
									
									if ((balValue == null) && (termsValue != null)) {
										//System.out.println("CR NEW Format: TermsValue is not null");
										if (balColumn != null) {
											//System.out.println("CR NEW Format: BalColumn is not null");
											balColumn.addContent(new Element("Value").setText(termsValue.getText()));
											zeroBalance = termsValue.getTextTrim();
										}
									}
								}
								
								//zeroBalance = zeroBalance.trim();
								
								if ((rptdValue == null) || (rowDEL.equals("Y")) || (zeroBalance.equals("0")) || 
										(zeroBalance.equals("")) || (zeroBalance.equals("$0"))) 
								{
									rowDEL = "N";
									row.addContent(new Element("ToDelete").setText("DEL"));
									//System.out.println("CR NEW Format: Mark Row as DEL");									
								}
							}
						}
					}
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in Manage Credit Report: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void manageCRFactualDataFormat(Element table) {
		try
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null)
				return;
						
			String rowDEL = "N";
			String tmpValue = "";			
			String acctName = "";
			String acctNumber = "";
			String rptdDate = "";
			String balAmount = "";
			String paymentAmount = "";
			String termsName = "";
			String zeroBalance = "";
			int skipRPTD = 0;
			int acctNoCount = 0;
			int acctNameCount = 0;
			
			for(Element row : rowList)
			{	
				Element acctNameColumn = getDesiredColumn(row,"Account Name");
				Element acctNameValue = acctNameColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				Element rptdColumn = getDesiredColumn(row,"Reported Date");
				Element rptdValue = rptdColumn.getChild("Value");
				Element balColumn = getDesiredColumn(row,"Balance");
				Element balValue = balColumn.getChild("Value");
				Element termsColumn = getDesiredColumn(row,"Payment Amount");
				Element termsValue = termsColumn.getChild("Value");
				Element typeColumn = getDesiredColumn(row,"Terms");
				Element typeValue = typeColumn.getChild("Value");
				rowDEL = "Y";
								
				/*if (balValue == null)
					zeroBalance = "";
				else
					zeroBalance = balValue.getTextTrim();
				*/
				
				/*System.out.println("CR Factual Data: Log all fields: Start");
				if (acctNameValue != null)
					System.out.println("acctNameValue: " + acctNameValue.getText());
				if (acctNoValue != null)
					System.out.println("acctNoValue: " + acctNoValue.getText());
				if (rptdValue != null)
					System.out.println("rptdValue: " + rptdValue.getText());
				if (balValue != null)
					System.out.println("balValue: " + balValue.getText());
				if (termsValue != null)
					System.out.println("termsValue: " + termsValue.getText());
				if (typeValue != null)
					System.out.println("typeValue: " + typeValue.getText());

				System.out.println("CR Factual Data: Log all fields: End");*/
				
				if (rptdValue != null) {
					skipRPTD = 0;
					if (acctNoValue != null) {
						tmpValue = acctNoValue.getTextTrim();
						//System.out.println("CRFactualData tmpValue: " + tmpValue);
						//System.out.println("CRFactualData Test Skipping Row: " + tmpValue.substring(0,7));
						if (tmpValue.length() >= 6) {
							/*if ((tmpValue.substring(0,7).equals("History")) || (tmpValue.substring(0,7).equals("Trended")) ||
									(tmpValue.substring(0,7).equals("Schedul")) || (tmpValue.substring(0,7).equals("Actual ")) ||
									(tmpValue.substring(0,7).equals("Balance"))) { */
									
							if ((tmpValue.contains("History")) || (tmpValue.contains("Trended")) ||
									(tmpValue.contains("Schedul")) || (tmpValue.contains("Actual")) ||
									(tmpValue.contains("Balance"))) {
										skipRPTD = 1;
									}
						}
					}
					if (skipRPTD == 0) {
						rptdDate = rptdValue.getTextTrim();
						
						if (balValue != null) {
							if (!balValue.getTextTrim().equals(""))
								balAmount = ""; // remove old value of balance
						}
						
						if (termsValue != null) {
							if (!termsValue.getTextTrim().equals(""))
								paymentAmount = termsValue.getTextTrim(); // remove old value of payment amount
						}
					}
					//System.out.println("CRFactualData acctName: " + acctName);
					//System.out.println("CRFactualData rptdDate: " + rptdValue.getTextTrim());
				}
				
				if (!rptdDate.equals("")) {
					// look for account name
					//System.out.println("CRFactualData Found rptdDate: " + rptdDate);
					if (acctNameCount == 0) {
						if (acctNameValue != null)
							tmpValue = acctNameValue.getTextTrim();
						
						for (int i = 0; i < tmpValue.length(); i++) {
							if ((Character.isAlphabetic(tmpValue.charAt(i))) || (tmpValue.charAt(i) == ' ')) {
								acctName = acctName + tmpValue.charAt(i);
								acctNameCount++;
							}
						}						
						acctName = acctName.trim();
						if (acctName.length() > 7) {
							if ((acctName.substring(0,8).equals("DLA ECOA")) || (acctName.substring(0,8).equals("DIA ECOA")) ||
									(acctName.substring(0,8).equals("BX CX BU"))) {
								acctName = "";
							}
						}
						acctNameCount = acctName.length();
					}
					else {
						acctNumber = "";
						//System.out.println("CRFactualData Found acctName: " + acctName);
						if (acctNameValue != null)
							tmpValue = acctNameValue.getTextTrim();
						for (int i = 0; i < tmpValue.length(); i++) {
							if (Character.isDigit(tmpValue.charAt(i))) {								 
								acctNumber = acctNumber + tmpValue.charAt(i);
								acctNoCount++;
							}
							else if (acctNoCount > 0) {
								if (tmpValue.charAt(i) == ' ') {
									if (acctNoCount < 6) {
										acctNumber = "";
										acctNoCount = 0;
									}
									else
										break;
								}
								acctNumber = acctNumber + tmpValue.charAt(i);
								acctNoCount++;
							}
						}
					}
					if (acctNameCount < 4)
						acctNameCount = 0;
					if (acctNoCount < 6)
						acctNoCount = 0;
				}
				
				if (termsValue != null) {
					if ((termsValue.getText().length() > 0) && (!rptdDate.equals("")) && (paymentAmount.trim().equals(""))) {
						paymentAmount = MatchandGetOuterString(termsValue.getText(), "X", "$");
						//System.out.println("CR Factual Data: Got Payment 2: " + paymentAmount);
					}
				}
				
				if (balValue != null) {
					if (balAmount.equals("")) {
						if ((!balValue.getTextTrim().equals("")) && (!rptdDate.equals("")) && (skipRPTD == 0)) {
							balAmount = balValue.getTextTrim(); //trim balance
							zeroBalance = balValue.getTextTrim();
							//System.out.println("CR Factual Data: Got Balance: " + balAmount);
						}
					}
					else { // balance is already available
						if ((paymentAmount.trim().equals("")) && (!rptdDate.equals(""))) {
							paymentAmount = balValue.getTextTrim();
							//System.out.println("CR Factual Data: Got Payment: " + paymentAmount);
						}
					}
				}
				
				if (typeValue != null) 
					termsName = typeValue.getText();			
				
				if ((!rptdDate.equals("")) && (acctNoCount > 0) && (acctNameCount > 0)) {
					rowDEL = "N";
					//System.out.println("CR Factual Data: Write Rec: " + acctNumber);
					
					acctName = GetOuterString(acctName, "DLA ECOA", true);
					
					if (acctNameValue == null)
						acctNameColumn.addContent(new Element("Value").setText(acctName));
					else
						acctNameValue.setText(acctName);
					
					if (rptdDate.length() > 6) // mm/dd mm/dd
						rptdDate = rptdDate.substring(6);
					if (rptdValue == null)
						rptdColumn.addContent(new Element("Value").setText(rptdDate));
					else
						rptdValue.setText(rptdDate);
					
					if (balValue == null)
						balColumn.addContent(new Element("Value").setText(balAmount));
					else
						balValue.setText(balAmount);
					
					if (termsValue == null)
						termsColumn.addContent(new Element("Value").setText(paymentAmount));
					else
						termsValue.setText(paymentAmount);
					
					if (typeValue == null)
						typeColumn.addContent(new Element("Value").setText(termsName));
					else
						typeValue.setText(termsName);
					
					if (acctNoValue == null)
						acctNoColumn.addContent(new Element("Value").setText(acctNumber));
					else
						acctNoValue.setText(acctNumber);
					
					acctName = "";
					acctNumber = "";
					acctNameCount = 0;
					acctNoCount = 0;
					rptdDate = "";
					balAmount = "";
					paymentAmount = "";
					termsName = "";
				}
				
				if ((rowDEL.equals("Y")) || (zeroBalance.equals("0")) || (zeroBalance.equals("$0")))
				{
					//rowDEL = "N";
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageCRFactualDataFormat Credit Report: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void manageCRTextFormat(Element table) {
		try 
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null)
				return;
			
			String tmpValue = "";
			String acctName = "";
			String acctNumber = "";
			String prevRptdDate = "";
			String prevPaymentAmount = "";
			String prevAccountName = "";
			String rowDEL = "N";
			String zeroBalance = "";
			int RPTDCount = 0;
			int acctNoFound = 0;
			for(Element row : rowList)
			{	
				Element acctNameColumn = getDesiredColumn(row,"Account Name");
				Element acctNameValue = acctNameColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				Element rptdColumn = getDesiredColumn(row,"Reported Date");
				Element rptdValue = rptdColumn.getChild("Value");
				Element balColumn = getDesiredColumn(row,"Balance");
				Element balValue = balColumn.getChild("Value");
				Element termsColumn = getDesiredColumn(row,"Payment Amount");
				Element termsValue = termsColumn.getChild("Value");
				Element typeColumn = getDesiredColumn(row,"Terms");
				Element typeValue = typeColumn.getChild("Value");
				
				//if (RPTDCount == 0)
				//{
				/*System.out.println("Log all fields: Start");
					if (acctNameValue != null)
						System.out.println("acctNameValue: " + acctNameValue.getText());
					if (acctNoValue != null)
						System.out.println("acctNoValue: " + acctNoValue.getText());
					if (rptdValue != null)
						System.out.println("rptdValue: " + rptdValue.getText());
					if (balValue != null)
						System.out.println("balValue: " + balValue.getText());
					if (termsValue != null)
						System.out.println("termsValue: " + termsValue.getText());
					if (typeValue != null)
						System.out.println("typeValue: " + typeValue.getText());
				//}
				System.out.println("Log all fields: End");*/
				
				if (balValue == null)
					zeroBalance = "";
				else
					zeroBalance = balValue.getTextTrim();
				
				// find out if accountNumber is found	
				if (acctNameValue != null) {
					tmpValue = acctNameValue.getTextTrim();
					if (!tmpValue.equals(""))
						acctNoFound = 1;
					
					for (int i = 0; i < tmpValue.length(); i++) {
						if ((Character.isDigit(tmpValue.charAt(i))) || (tmpValue.charAt(i) == '*') || (tmpValue.charAt(i) == '-') ||
								(tmpValue.charAt(i) == '<') || (tmpValue.charAt(i) == ' '))
							acctNumber = acctNumber + tmpValue.charAt(i);
						else {
							acctNoFound = 0;
							acctName = acctName + tmpValue.charAt(i);
						}
					}
				}
				
				if ((acctNoFound == 1) && (RPTDCount == 0) && (rptdValue != null)) {
					RPTDCount = 1; // first row not read
					//System.out.println("Setting RPTDCount to 1");
				}
				
				rowDEL = "Y";
				if (rptdValue != null) {
					RPTDCount = RPTDCount + 1;
					//System.out.println("Good RPTDValue: " + rptdValue.getText() + ", new RPTDCount: " + RPTDCount);
				}
								
				if (RPTDCount == 2) 
				{
					rowDEL = "N";
					//System.out.println("Setting Data, prevAccountName: " + prevAccountName);
					if (acctNameValue != null) {
						if (acctNoValue != null)
							acctNoValue.setText(acctNameValue.getText());
						else if (acctNoValue == null)
							acctNoColumn.addContent(new Element("Value").setText(acctNameValue.getText()));
						//System.out.println("Setting Data, acctNameValue.getText(): " + acctNameValue.getText());
					}
					
					if (!prevAccountName.equals("")) {
						if (acctNameValue != null)
							acctNameValue.setText(prevAccountName);
						else
							acctNameColumn.addContent(new Element("Value").setText(prevAccountName));
					}
					
					if (!prevRptdDate.equals("")) {
						if (rptdValue != null)
							rptdValue.setText(prevRptdDate);
						else
							rptdColumn.addContent(new Element("Value").setText(prevRptdDate));
					}
					
					if (termsValue != null)
						termsValue.setText(MatchandGetOuterString(prevPaymentAmount, "X", "$"));
					else if (termsValue == null)
						termsColumn.addContent(new Element("Value").setText(MatchandGetOuterString(prevPaymentAmount, "X", "$")));
					
					prevPaymentAmount = "";
					prevAccountName = "";
					prevRptdDate = "";
				}
					
				if ((rptdValue != null) && (RPTDCount == 1)) { 
					// first row
					if (acctNameValue != null)
						prevAccountName = acctNameValue.getText();
					if (rptdValue != null)
						prevRptdDate = rptdValue.getText();
					if (typeValue != null)
						prevPaymentAmount = typeValue.getText();
					//System.out.println("Store Values, prevAccountName: " + prevAccountName);
					//System.out.println("Store Values, prevPaymentAmount: " + prevPaymentAmount);
				}
				
				if (RPTDCount == 3) {
					//System.out.println("Resetting RPTDCount: " + RPTDCount);
					RPTDCount = 0;
				}
				
				if ((rowDEL.equals("Y")) || (zeroBalance.equals("0")) || (zeroBalance.equals("$0"))) {
					//System.out.println("Deleting Row, RPTDCount: " + RPTDCount);
					row.addContent(new Element("ToDelete").setText("DEL"));		
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageCRTextFormat Credit Report: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void manageCRTextTrendFormat(Element table) {
		try 
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null)
				return;
						
			String acctName = "";
			String acctNumber = "";
			String RptdDate = "";
			String PaymentAmount = "";
			String Balance = "";
			String terms = "";
			String rowDEL = "N";
			String tmpValue = "";
			String zeroBalance = "";
			int termsFound = 0;
			int rptdFound = 0;
			
			for(Element row : rowList)
			{	
				Element acctNameColumn = getDesiredColumn(row,"Account Name");
				Element acctNameValue = acctNameColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				Element rptdColumn = getDesiredColumn(row,"Reported Date");
				Element rptdValue = rptdColumn.getChild("Value");
				Element balColumn = getDesiredColumn(row,"Balance");
				Element balValue = balColumn.getChild("Value");
				Element paymentColumn = getDesiredColumn(row,"Payment Amount");
				Element paymentValue = paymentColumn.getChild("Value");
				Element termsColumn = getDesiredColumn(row,"Terms");
				Element termsValue = termsColumn.getChild("Value");
				
				rowDEL = "Y";			
				rptdFound = 0;
				termsFound = 0;
				
				/*System.out.println("Log all fields: Start");
					if (acctNameValue != null)
						System.out.println("acctNameValue: " + acctNameValue.getText());
					if (acctNoValue != null)
						System.out.println("acctNoValue: " + acctNoValue.getText());
					if (rptdValue != null)
						System.out.println("rptdValue: " + rptdValue.getText());
					if (balValue != null)
						System.out.println("balValue: " + balValue.getText());
					if (termsValue != null)
						System.out.println("termsValue: " + termsValue.getText());
					if (typeValue != null)
						System.out.println("typeValue: " + typeValue.getText());
				System.out.println("Log all fields: End");*/
				
				if ((rptdValue != null) && (RptdDate.equals(""))) {
					RptdDate = rptdValue.getTextTrim();
					rptdFound = 1;
				}
				
				// store account name and account number values
				if (RptdDate.equals("")) {
					if (acctNameValue != null)
						acctName = acctNameValue.getTextTrim();
				
					if (acctNoValue != null)
						acctNumber = acctNoValue.getTextTrim();
				
					// store payment and balance values
					if (paymentValue != null)
						PaymentAmount = paymentValue.getTextTrim();
				
					if (balValue != null)
						Balance = balValue.getTextTrim();
				
					if ((acctNumber.equals("")) && (RptdDate.equals("")))
						acctNumber = PaymentAmount + Balance;					
				}
				else {
					if (rptdFound == 1) {
						// store payment and balance values
						if (paymentValue != null)
							PaymentAmount = paymentValue.getTextTrim();
				
						if (balValue != null)
							Balance = balValue.getTextTrim();
					}
					
					if (acctNameValue != null) {
						tmpValue = acctNameValue.getTextTrim();
						if ((tmpValue.equals("AUTO")) || (tmpValue.equals("MORT")) || (tmpValue.equals("INST")) || (tmpValue.equals("REV"))) {
							terms = tmpValue;
							termsFound = 1;
						}
					}
					
					if (termsValue != null) {
						terms = termsValue.getTextTrim();
						termsFound = 1;
					}
				}
				
				if ((termsFound == 1) && (!RptdDate.equals("")))
				{
					rowDEL = "N";
					//System.out.println("Setting Data, prevAccountName: " + prevAccountName);
				
					if (acctNoValue != null)
						acctNoValue.setText(acctNumber);
					else
						acctNoColumn.addContent(new Element("Value").setText(acctNumber));
					
					if (acctNameValue != null)
						acctNameValue.setText(acctName);
					else
						acctNameColumn.addContent(new Element("Value").setText(acctName));
					
					if (rptdValue != null)
						rptdValue.setText(RptdDate);
					else
						rptdColumn.addContent(new Element("Value").setText(RptdDate));
					
					if (balValue != null)
						balValue.setText(Balance);
					else
						balColumn.addContent(new Element("Value").setText(Balance));
					
					if (paymentValue != null)
						paymentValue.setText(MatchandGetOuterString(PaymentAmount, "X", "$"));
					else
						paymentColumn.addContent(new Element("Value").setText(MatchandGetOuterString(PaymentAmount, "X", "$")));
					
					zeroBalance = Balance;
					RptdDate = "";
					acctName = "";
					acctNumber = "";
					PaymentAmount = "";
					Balance = "";
				}
				
				if ((rowDEL.equals("Y")) || (zeroBalance.equals("0")) || (zeroBalance.equals("$0"))) {					
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageCRTextTrendFormat Credit Report: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void manageCRCredcoFormat(Element table) {
		try
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null)
				return;
			
			String acctName = "";
			String acctName1 = "";
			String acctName2 = "";
			String acctTemp = "";
			String acctCurrent = "";
			String balAmount = "";
			String acctNumber = "";
			String rptdDate = "";
			String paymentAmount = "";
			String termsName = "";
			
			String rowDEL = "N";
			
			int balFound = 0;
			int startRow = 1;
			int lastRow = 0;
			
			for(Element row : rowList)
			{	
				Element acctNameColumn = getDesiredColumn(row,"Account Name");
				Element acctNameValue = acctNameColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				Element rptdColumn = getDesiredColumn(row,"Reported Date");
				Element rptdValue = rptdColumn.getChild("Value");
				Element balColumn = getDesiredColumn(row,"Balance");
				Element balValue = balColumn.getChild("Value");
				Element termsColumn = getDesiredColumn(row,"Payment Amount");
				Element termsValue = termsColumn.getChild("Value");
				Element typeColumn = getDesiredColumn(row,"Terms");
				Element typeValue = typeColumn.getChild("Value");
				rowDEL = "Y";
				
				/*System.out.println("CR Credo Data: Log all fields: Start");
				if (acctNameValue != null)
					System.out.println("acctNameValue: " + acctNameValue.getText());
				if (acctNoValue != null)
					System.out.println("acctNoValue: " + acctNoValue.getText());
				if (rptdValue != null)
					System.out.println("rptdValue: " + rptdValue.getText());
				if (balValue != null)
					System.out.println("balValue: " + balValue.getText());
				if (termsValue != null)
					System.out.println("termsValue: " + termsValue.getText());
				if (typeValue != null)
					System.out.println("typeValue: " + typeValue.getText());

				System.out.println("CR Credco Data: Log all fields: End");*/
				
				rowDEL = "Y";
				
				if ((balValue != null) && (startRow == 1) && (balFound == 0)) {
					balAmount = balValue.getTextTrim();
					balFound = 1;
				}
				
				if (lastRow == 1) {
					rowDEL = "N";					
					if (rptdValue != null)
						rptdDate = rptdValue.getText();	
				
					if (termsValue != null)
						paymentAmount = termsValue.getText();
					
					if (typeValue != null)
						termsName = typeValue.getText();
				}
				
				if ((balValue != null)) {
					//balAmount = balValue.getText();
					if (balValue.getText().contains("Balance"))
						startRow = 1;
					else if (balValue.getText().contains("Scheduled"))
						lastRow = 1;
				}
				
				if (startRow == 1) {
					if ((acctName.equals("")) && (acctNumber.equals(""))){
						if (acctNameValue != null){
							if (acctName1.equals("")) {
								acctName1 = acctNameValue.getText();
								acctTemp = acctName1;
							}
							else if (acctName2.equals("")) {
								acctName2 = acctNameValue.getText();
								acctCurrent = acctName2;
							}
							else {								
								if (acctNameValue.getText().contains("Decode")) {
									acctNumber = acctCurrent;
									if (acctTemp.trim().length() > 19)
										acctName = acctTemp.trim().substring(0,20);
									else
										acctName = acctTemp.trim();
								}
								else {
									acctCurrent = acctNameValue.getText();
									acctTemp = acctName1 + acctName2;
									//acctName2 = "";
								}
							}
						}
					}
				}
				
				if (rowDEL.equals("N")) {
					if ((balAmount.equals("0")) || (balAmount.equals("$0")) || (balAmount.equals("")))
						rowDEL = "Y";
					else
					{
						//rowDEL = "N";
					
						if (acctNameValue == null)
							acctNameColumn.addContent(new Element("Value").setText(acctName));
						else
							acctNameValue.setText(acctName);
						
						if (balValue == null)
							balColumn.addContent(new Element("Value").setText(balAmount));
						else
							balValue.setText(balAmount);
						
						if (acctNoValue == null)
							acctNoColumn.addContent(new Element("Value").setText(acctNumber));
						else
							acctNoValue.setText(acctNumber);
					}
					
					acctName = "";
					acctName1 = "";
					acctName2 = "";
					acctNumber = "";
					startRow = 0;
					balFound = 0;
					lastRow = 0;
				}
				
				if (rowDEL.equals("Y"))
				{
					//rowDEL = "N";
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageCRFactualDataFormat Credit Report: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public void manageCRCabrillo(Element table)
	{
		try
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null)
				return;
						
			String rowDEL = "N";
				
			String acctName = "";
			String acctNumber = "";
			String rptdDate = "";
			String balAmount = "";
			String paymentAmount = "";
			String termsName = "";
			String zeroBalance = "";
			int RPTDCount = 0;
			
			for(Element row : rowList)
			{	
				Element acctNameColumn = getDesiredColumn(row,"Account Name");
				Element acctNameValue = acctNameColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				Element rptdColumn = getDesiredColumn(row,"Reported Date");
				Element rptdValue = rptdColumn.getChild("Value");
				Element balColumn = getDesiredColumn(row,"Balance");
				Element balValue = balColumn.getChild("Value");
				Element termsColumn = getDesiredColumn(row,"Terms");
				Element termsValue = termsColumn.getChild("Value");
				Element typeColumn = getDesiredColumn(row,"Payment Amount");
				Element typeValue = typeColumn.getChild("Value");
				rowDEL = "Y";
				
				if (RPTDCount == 1)
				{
					RPTDCount = 2;
					rowDEL = "N";
					if (termsValue != null)
						termsName = termsValue.getTextTrim();
					
					if (acctNoValue != null)
						acctNumber = acctNoValue.getTextTrim();
				}
				else if ((rptdValue != null) && (RPTDCount == 0)) {
					RPTDCount++;
					
					if (acctNameValue != null)
						acctName = acctNameValue.getTextTrim();
					
					if (rptdValue != null)
						rptdDate = rptdValue.getTextTrim();
					
					if (balValue != null) {
						balAmount = balValue.getTextTrim();
						zeroBalance = balAmount; 
					}
					
					if (typeValue != null)
						paymentAmount = typeValue.getTextTrim();
				}
								
				/* System.out.println("CR Factual Data: Log all fields: Start");
				if (acctNameValue != null)
					System.out.println("acctNameValue: " + acctNameValue.getText());
				if (acctNoValue != null)
					System.out.println("acctNoValue: " + acctNoValue.getText());
				if (rptdValue != null)
					System.out.println("rptdValue: " + rptdValue.getText());
				if (balValue != null)
					System.out.println("balValue: " + balValue.getText());
				if (termsValue != null)
					System.out.println("termsValue: " + termsValue.getText());
				if (typeValue != null)
					System.out.println("typeValue: " + typeValue.getText());

				System.out.println("CR Factual Data: Log all fields: End"); */
				
				if ((rowDEL.equals("Y")) || ((RPTDCount == 2) && ((zeroBalance.equals("0")) || (zeroBalance.equals("$0")))))
				{					
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
				else //if ((rowDEL.equals("N")) && (acctNoCount > 0) && (acctNameCount > 0)) 
				{					
					if (acctNameValue == null)
						acctNameColumn.addContent(new Element("Value").setText(acctName));
					else
						acctNameValue.setText(acctName);
					
					if (rptdDate.length() > 6) // mm/dd mm/dd
						rptdDate = rptdDate.substring(6);
					if (rptdValue == null)
						rptdColumn.addContent(new Element("Value").setText(rptdDate));
					else
						rptdValue.setText(rptdDate);
					
					if (balValue == null)
						balColumn.addContent(new Element("Value").setText(balAmount));
					else
						balValue.setText(balAmount);
					
					if (termsValue == null)
						termsColumn.addContent(new Element("Value").setText(termsName));
					else
						termsValue.setText(termsName);
					
					if (typeValue == null)
						typeColumn.addContent(new Element("Value").setText(paymentAmount));
					else
						typeValue.setText(paymentAmount);
					
					if (acctNoValue == null)
						acctNoColumn.addContent(new Element("Value").setText(acctNumber));
					else
						acctNoValue.setText(acctNumber);
				}
				
				if (RPTDCount == 2) {
					acctName = "";
					acctNumber = "";
					rptdDate = "";
					balAmount = "";
					paymentAmount = "";
					termsName = "";
					RPTDCount = 0;
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageCRFactualDataFormat Credit Report: " + e.getMessage());
			e.printStackTrace();
		}		
	}
	
	public void manageCRChronosFormat(Element table)
	{
		try
		{
			List<Element> rowList = null;
			Element allRows = table.getChild("Rows");
			
			if (allRows != null)
				rowList = allRows.getChildren("Row");
			
			if (rowList == null) // ==
				return;
			
			String rowDEL = "N";
				
			String acctName = "";
			String acctNumber = "";
			String rptdDate = "";
			String balAmount = "";
			String paymentAmount = "";
			String termsName = "";
			
			String FoundRPTD = "N";
			String FoundOPND = "N";
			
			for(Element row : rowList)
			{	
				Element acctNameColumn = getDesiredColumn(row,"Account Name");
				Element acctNameValue = acctNameColumn.getChild("Value");
				Element acctNoColumn = getDesiredColumn(row,"Account Number");
				Element acctNoValue = acctNoColumn.getChild("Value");
				Element rptdColumn = getDesiredColumn(row,"Reported Date");
				Element rptdValue = rptdColumn.getChild("Value");
				Element balColumn = getDesiredColumn(row,"Balance");
				Element balValue = balColumn.getChild("Value");
				Element paymentColumn = getDesiredColumn(row,"Payment Amount");
				Element paymentValue = paymentColumn.getChild("Value");
				Element termsColumn = getDesiredColumn(row,"Terms");
				Element termsValue = termsColumn.getChild("Value");
				rowDEL = "Y";
				
				if ((FoundRPTD.equals("N")) && (FoundOPND.equals("N"))) {
					if ((rptdValue == null) && (acctNameValue != null))
						acctName = acctNameValue.getTextTrim();
					
					if (paymentValue != null)
						paymentAmount = paymentValue.getTextTrim();
					
					if (termsValue != null)
						termsName = termsValue.getTextTrim();
				}
				
				if (rptdValue != null) {
					if ((FoundRPTD.equals("N")) && (FoundOPND.equals("N"))) {
						FoundRPTD = "Y";
						rptdDate = rptdValue.getTextTrim();
					}
					else if ((FoundRPTD.equals("Y")) && (FoundOPND.equals("N"))) {
						FoundOPND = "Y";
					}
				}
				
				// safety net if RPTD is not read, if PAYMENT is read, beginning of row
				/*if ((FoundRPTD.equals("N")) && (FoundOPND.equals("N"))) {
					if (paymentValue != null) {
						if (paymentValue.equals("PAYMENT")) {
							FoundRPTD = "Y";
						}
					}
				}*/
				
				if ((FoundRPTD.equals("Y")) && (FoundOPND.equals("Y"))) {
					if (balValue != null) {
						balAmount = balValue.getTextTrim();
					}
					rowDEL = "N";
				}
				
				// safety net if the RPTD date is not read, if BALANCE is read, end of row
				//if (FoundRPTD.equals("N")) {
					if (balValue != null) { 
						if (balValue.getTextTrim().equals("BALANCE")) {
							FoundRPTD = "Y";
							FoundOPND = "Y";
						}
					}
				//}
				
				//if (FoundRPTD.equals("Y")) {
					if (paymentValue != null) {
						if ((paymentValue.getTextTrim().equals("PASTEUE")) || (paymentValue.getTextTrim().equals("PASTDIE")) || (paymentValue.getTextTrim().equals("PASTDUE"))
								|| (paymentValue.getTextTrim().equals("PAST DUE")) || (paymentValue.getTextTrim().equals("WUE")) ||
									(paymentValue.getTextTrim().equals("PAST IUE"))) {
							FoundRPTD = "Y";
							FoundOPND = "Y";
						}
					}
				//}
				
				if ((FoundRPTD.equals("Y")) && (FoundOPND.equals("N"))) {
					// if acct No is in the same or subsequent row from the reported date
					if (acctNameValue != null)
						acctNumber = acctNameValue.getTextTrim();
				}
								
				/*System.out.println("CR Chronos: Log all fields: Start");
				if (acctNameValue != null)
					System.out.println("acctNameValue: " + acctNameValue.getText());
				if (acctNoValue != null)
					System.out.println("acctNoValue: " + acctNoValue.getText());
				if (rptdValue != null)
					System.out.println("rptdValue: " + rptdValue.getText());
				if (balValue != null)
					System.out.println("balValue: " + balValue.getText());
				if (termsValue != null)
					System.out.println("termsValue: " + termsValue.getText());
				if (paymentValue != null)
					System.out.println("paymentValue: " + paymentValue.getText());

				System.out.println("CR Chronos: Log all fields: End");
				
				System.out.println("CR Chronos: Assigned Fields: Start");
				if (!(acctName.equals("")))
					System.out.println("acctName: " + acctName);
				if (!(acctNumber.equals("")))
					System.out.println("acctNumber: " + acctNumber);
				if (!(rptdDate.equals("")))
					System.out.println("rptdDate: " + rptdDate);
				if (!(balAmount.equals("")))
					System.out.println("balAmount: " + balAmount);
				if (!(termsName.equals("")))
					System.out.println("termsName: " + termsName);
				if (!(paymentAmount.equals("")))
					System.out.println("paymentAmount: " + paymentAmount);

				System.out.println("CR Chronos: Assigned Fields: End");*/
				
				if ((rowDEL.equals("Y")) || (balAmount.trim().equals("$0")) || (balAmount.trim().equals("-0-")) || (balAmount.trim().equals("SO")) ||
						(balAmount.trim().contains("CRED")) || (balAmount.trim().contains("HI")))
				{					
					row.addContent(new Element("ToDelete").setText("DEL"));
				}
				else //if ((rowDEL.equals("N")) && (acctNoCount > 0) && (acctNameCount > 0)) 
				{
					//System.out.println("Writing Results");
					if (acctNameValue == null)
						acctNameColumn.addContent(new Element("Value").setText(acctName));
					else
						acctNameValue.setText(acctName);
					
					if (rptdDate.length() > 10) // mm/dd mm/dd
						rptdDate = rptdDate.substring(10);
						
					if (rptdValue == null)
						rptdColumn.addContent(new Element("Value").setText(rptdDate));
					else
						rptdValue.setText(rptdDate);
					
					if (balValue == null)
						balColumn.addContent(new Element("Value").setText(balAmount));
					else
						balValue.setText(balAmount);
					
					if (termsValue == null)
						termsColumn.addContent(new Element("Value").setText(termsName));
					else
						termsValue.setText(termsName);
					
					if (paymentValue == null)
						paymentColumn.addContent(new Element("Value").setText(paymentAmount));
					else
						paymentValue.setText(paymentAmount);
					
					if (acctNoValue == null)
						acctNoColumn.addContent(new Element("Value").setText(acctNumber));
					else
						acctNoValue.setText(acctNumber);
				
					acctName = "";
					acctNumber = "";
					rptdDate = "";
					balAmount = "";
					paymentAmount = "";
					termsName = "";
					FoundRPTD = "N";
					FoundOPND = "N";
				}
			}
		}
		catch (Exception e) {
			System.out.println("*************  Error occurred in manageCRChronosFormat Credit Report: " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	public String GetOuterString(String mainString, String searchString, boolean isLeft)
	{  
		String returnString = mainString;
		if (searchString != ""){
			String mainStrinArry[]= mainString.split(searchString); 
			if (isLeft == true){  
				returnString = mainStrinArry[0];
			}
			else {
				if (mainStrinArry.length > 1){
					returnString = mainStrinArry[1];
				}
				else {
					returnString = mainStrinArry[0];
				}
			}
		}
		return returnString;
	}
	
	public String MatchandGetOuterString(String mainString, String searchString, String matchString)
	{  		
		String returnString = mainString;
		if (searchString != ""){
			String mainStrinArry[] = mainString.split(searchString);
			for (String oneString : mainStrinArry)
			{
				if (oneString.contains(matchString))
					return oneString;
			}
			if (mainStrinArry.length > 0)
				return mainStrinArry[0];
		}
		return returnString;
	}
	
	public String GetTypeAndTerms(String inputValue)
	{
		int AddedComma = 0;
		String outputValue = "";
		
		if (inputValue.length() <= 5) //TERMS
			return inputValue;
			
		for (int i = 5; i < inputValue.length(); i++) {
			if (Character.isDigit(inputValue.charAt(i)))
				outputValue = outputValue + inputValue.charAt(i);	
			else if ((AddedComma == 0) && ((Character.isAlphabetic(inputValue.charAt(i))) || (inputValue.charAt(i) == ' '))) {
				outputValue = outputValue + ",";
				AddedComma = 1;
			}
		}
		return outputValue;
	}
	
	public String ReplaceParenthesisWithZero(String inputValue)
	{
		// if 0 reads as () convert to 0
		String outputValue = "";
		int LeftBracketPosition = 0;
		
		for (int i = 0; i < inputValue.length(); i++) {
			if (inputValue.charAt(i) == ')') {											
				if (LeftBracketPosition == i - 1) {
					outputValue = outputValue + '0';
				}
				LeftBracketPosition = 0;
			}
			if (inputValue.charAt(i) == '(')
				LeftBracketPosition = i;										
			
			if ((inputValue.charAt(i) != '(') && (inputValue.charAt(i) != ')')) {
				outputValue = outputValue + inputValue.charAt(i);
			}
		}
		return outputValue;
	}

	public void iterateDel(Document document) { 		
		Element root = document.getRootElement();		
		List<Element> docList = root.getChild("Documents").getChildren("Document");
		
		for (Element doc : docList) 
		{			
			List<Element> tables = null;
			Element allTables = doc.getChild("DataTables");
			
			if (allTables != null)
				tables = allTables.getChildren("DataTable");
			
			if (tables != null)
			{
				for (Element table : tables)
				{
					Element nameElement = table.getChild("Name");
					String tableName = nameElement.getText();
					//System.out.println(tableName);
					if ((tableName.equals("Trade Info")) || (tableName.equals("Liability Table")) || 
						(tableName.equals("Assets Table")) || (tableName.equals("Monthly Income")) || 
						(tableName.equals("Housing Expense Information")) || (tableName.equals("Fraud Alert")) || (tableName.equals("CRFactualData")))
					{
						List<Element> rowList = table.getChild("Rows").getChildren("Row");
						
						//System.out.println(rowList.size());
						
						for (int i = rowList.size() - 1; i >= 0; i--)
						{
							Element row2 = rowList.get(i); 
							Element td = row2.getChild("ToDelete");
							if (td != null) {
								String td1 = td.getText();
								//System.out.println(td1);
								row2.detach();
							}
						}
					}
				}
			}
		}
	}
				
	//		Iterator itr = rowList.iterator();
	//		while (itr.hasNext()) {
	//		  Element child = (Element) itr.next();
	//		  String td = child.getAttributeValue("ToDelete"); 
	//		  System.out.println(td);
	//		  if( td.equals("DEL")){
	//		    itr.remove();
	//		  }
			
	//		  for (int i = nodes.getLength() - 1; i >= 0; i--) {
	//			  Element e = (Element)nodes.item(i);
	//			   if (certain criteria involving Element e) {
	//			    e.getParentNode().removeChild(e);
	//			  }
	//			}		
	
	private Element getDesiredColumn(Element row, String desiredColumn) {
		// TODO Auto-generated method stub

		List<Element> columnList = row.getChild("Columns").getChildren("Column");
		for(Element column : columnList)
		{
			String name = column.getChildText("Name");
			if(name.equals(desiredColumn))
			{
				return column;
			}
		}

		return null;
	}
	 
	/**
	 * @param document
	 */
	private void initialize(Document document) {
		this.logUtil = new LogUtil(logLevel, logger);
		super.initialize(document, logLevel);
	}
}
