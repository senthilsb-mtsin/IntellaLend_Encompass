import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.Formatter;
import java.util.Iterator;
import java.util.List;
import java.text.DateFormat;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.io.IOException;
import java.io.OutputStream;
import java.io.StringReader;
import java.math.BigDecimal;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;
import java.util.concurrent.TimeUnit;

import org.jdom.Document;
import org.jdom.Element;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.apache.commons.httpclient.HttpClient;
import org.apache.commons.httpclient.methods.GetMethod;
import org.apache.commons.httpclient.Credentials;
import org.apache.commons.httpclient.UsernamePasswordCredentials;
import org.apache.commons.httpclient.auth.AuthScope;

import com.mts.idc.ephesoft.BaseScript;
import com.mts.idc.ephesoft.DateParseUtil;
import com.mts.idc.ephesoft.LogLevel;
import com.mts.idc.ephesoft.LogUtil;

import org.jdom.output.XMLOutputter;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

import com.mts.idc.ephesoft.service.EphesoftUtilityRequest;
import com.mts.idc.ephesoft.service.EphesoftModule;
import com.mts.idc.ephesoft.service.EphesoftUtilityWSClient;
import com.mts.idc.ephesoft.service.QCIQLookupRequest;
import com.mts.idc.ephesoft.service.QCIQLookupWSClient;
import com.mts.idc.ephesoft.service.JsonResponseWSClient;
import com.google.gson.Gson;
import java.io.FileInputStream;
import java.io.FilenameFilter;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.BufferedReader;
import org.fife.io.*; 
/**
 * Date Modified: 01/07/2015
 * @author Atit Patel  
 */
public class ScriptExport extends BaseScript {

	// Property Document Update 
	private static List<String> DOCUMENTFIELDS = Arrays.asList("PropertyStreet","PropertyCity","PropertyState","PropertyZip");
    private static String  DocumentFieldParentName="Property Address";
    private static String DocumentFieldChildProperty ="FieldValueChangeScript";
    private static String PROPERTY_REQUEST_URL="http://10.1.0.41:8090/EphesoftUtilityTesting/IntellaLendWrapper/GetPropertyAddressDetails";
 	private static final String EXT_BATCH_XML_FILE = "_batch.xml";
	private static String ZIP_FILE_EXT = ".zip";
	//private static final String DateFieldType = "Date";
	private static final String mmddyyyyFieldType = "MMDDYYYY";
	private static final String mmddFieldType = "MMDD";
	private static final String SSNFieldType = "SSN";
	//private static final String AmountFieldType = "Amount";
	private static final String Decimal84FieldType = "DECIMAL84";
	private static final String Decimal74FieldType = "DECIMAL74";
	private static final String Decimal63FieldType = "DECIMAL63";
	private static final String Decimal64FieldType = "DECIMAL64";
	private static final String Int2FieldType = "INT2";
	private static final String IntUpto2FieldType = "INTUPTO2";
	private static final String IntUpto3FieldType = "INTUPTO3";
	private static final String IntUpto4FieldType = "INTUPTO4";
	private static final String StringToParseForColumnTotal="{\"Loan Application 1003 Continuation Sheet\": [{		\"TableName\": \"Assets Table\",		\"Columns\": [{			\"ColumnName\": \"Cash Value\",			\"DestinationField\": \"Total Assets\"		}]	},	{		\"TableName\": \"Liability Table\",		\"Columns\": [{			\"ColumnName\": \"Monthly Payment\",			\"DestinationField\": \"Total Liabilities\"		}]	}],	\"AUS Desktop UW Findings Report\": [{		\"TableName\": \"Assets Funds\",		\"Columns\": [{			\"ColumnName\": \"Amount\",			\"DestinationField\": \"Total Available Assets\"		}]	}]}";
	private static final String REST_SERVICE_URL = "http://10.1.0.41:8090/EphesoftUtilityTesting/FieldMapping/SetEncompassLoanType";
	private static final String DOCUMENT_SERVICE_URL = "http://10.1.0.41:8090/EphesoftUtilityTesting/Document/Execute";		
	
	// Borrower Name Update 
	 List<String> Document_BorrowerName_List =Arrays.asList("Borrower Name" , "Name","Name of Veteran","Applicant Name","Recipients Name","Name Insured","Employee Full Name","Printed Name","Employee Name");
 	 List<String> Document_Co_BorrowerName_List = Arrays.asList("Co-Borrower Name");
	 List<String> Document_VeteranName_List =Arrays.asList("Name of Veteran");
 	 String Document_Field_Name ="Name";
 	 String Document_Borrower_FirstName ="Borrower First Name";
 	 String Document_Borrower_LastName = "Borrower Last Name";
 	 String Document_Veteran_FirstName ="Veteran First Name";
 	 String Document_Veteran_LastName = "Veteran Last Name";
 	 String Document_Co_Borrower_FirstName ="Co-Borrower First Name";
	 String Document_Co_Borrower_LastName = "Co-Borrower Last Name";
	private static final String targetReportFolder =  "E:\\EpheSoft\\SharedFolders\\MTS-Report-Data\\";
	private static final String sourceFolder =  "E:\\EpheSoft\\SharedFolders\\ephesoft-system-folder\\";
	 
	private LogUtil logUtil;
	private LogLevel logLevel = LogLevel.INFO;
	private Logger logger = LoggerFactory.getLogger(ScriptExport.class);
	
		
	/* (non-Javadoc)
	 * @see com.ephesoft.dcma.script.IJDomScript#execute(org.jdom.Document, java.lang.String, java.lang.String)
	 */
	public Object execute(Document document, String methodName, String docIdentifier)  {
		//log("Execute function called...");
		Exception exception = null;
			if (null == document) {
				try {
					throw new Exception("Input document is null");
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
			XMLOutputter xmlOut = new XMLOutputter();
			this.initialize(document);
			//Getting Json Object for field formatting
			try {
				log("Start 1");
				//log(xmlOut.outputString(document));
					this.UpdateValidatorDate();
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			
			
			BufferedReader reader = null;
			JSONObject _jsonObject = null;
			String workingDir = System.getProperty("user.dir");
			try {
				log("Inside - File Reader");
				reader = new BufferedReader(new UnicodeReader(new FileInputStream(workingDir + "\\FormattingDocFields.txt"), "UTF-8"));
				_jsonObject = (JSONObject) new JSONParser().parse(reader);			
				log("Finished - File Reader");
			} catch (FileNotFoundException E) {
				E.printStackTrace();
			} catch (IOException E) {
				E.printStackTrace();
			} catch (ParseException e) {
				log("Inside - ParseException");
				log(e.toString());
				log(e.getLocalizedMessage());
				log(e.getMessage());
			} catch (Exception E) {
				E.printStackTrace();
			}	
			
			try {
					//Getting Json Object for ColumnTotal Calculation
					JSONParser columTotalParser = new JSONParser();
					JSONObject columnTotalJsonObject = (JSONObject) columTotalParser.parse(StringToParseForColumnTotal);
					//Initialize 
					log("columnTotalJsonObject - " + columnTotalJsonObject.toJSONString());
					//Get list of Document elements
					List<Element> documentList = this.documentUtil.getDocumentElements();
					
					//Iterate through every document 
					log("Start 2");
					//log(xmlOut.outputString(document));
					for(Element documentElement : documentList) {	
		
						String documentType = documentElement.getChildText("Type");			
									
						//function call for Date,Amount and SSN Field formatting
						this.ProcessDocument(_jsonObject, documentType,documentElement);
						
						JSONArray dtArray=(JSONArray) columnTotalJsonObject.get(documentType);
						if(dtArray != null)
						{
							this.calculateColumnTotal(documentElement, dtArray);
						}
					}
					log("Start 3");
					//log(xmlOut.outputString(document));
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				try{
				
				log("EphesoftUtilityRequest()");
					EphesoftUtilityRequest requestContent = new EphesoftUtilityRequest();
					requestContent.appendFlag = false; 
					requestContent.concatenateFlag = true; 
					requestContent.convertFlag = true;
					requestContent.pageSequenceFlag = false;
					requestContent.ephesoftModule = EphesoftModule.EXPORT.ordinal();
					
					requestContent.inputXML = xmlOut.outputString(document);
					
					//log(requestContent.inputXML);
					log("Start 4");
					//log(xmlOut.outputString(document));
					Document responseDoc = null;					 			
					EphesoftUtilityWSClient client = new EphesoftUtilityWSClient();
					responseDoc = client.invokeEphesoftUtilityWS(requestContent, DOCUMENT_SERVICE_URL);
					log("EphesoftUtilityRequest Ephesoft Utility web service call successful.");				
				

					if (responseDoc != null) {
						document.detachRootElement();
						if(!document.hasRootElement())
						{
							document.setRootElement(responseDoc.detachRootElement());
						}												
					} else {
						// TODO 
						log("ResponseDoc is Null.");
					}
					log("Start 5");
					//log(xmlOut.outputString(document));
				}
				 catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				//Write changes back to batch xml
				//this.documentUtil.writeToXML(this.document);
				//QCIQ Lookup Call goes here
				try {
					//this.QCIQLookupWSCall();
					this.UpdateEphesoftValidatorName();
					log("Start 6");
				//	log(xmlOut.outputString(document));
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				try
				{
					log("Ephesoft Service Request");
					this.DocPropertyFieldUpdate();
				}
				catch(Exception ex)
				{
					ex.printStackTrace();
				}
				this.documentUtil.writeToXML(this.document);

				
				String batchIdentifier=this.documentUtil.getBatchInstanceIdentifier();
				String batchClassIdentifier=this.documentUtil.getBatchClassIdentifier();
				
				String  batchXMLPath = sourceFolder + batchIdentifier + File.separator +  batchIdentifier + EXT_BATCH_XML_FILE;


				String targetFolder= targetReportFolder + batchIdentifier;
				File directory = new File(targetFolder);
				
				if (!directory.exists())
				{
					directory.mkdir();
				}

				try {
					String destZipFile = targetReportFolder + batchIdentifier + File.separator +  batchIdentifier + "_Export_" + batchClassIdentifier + "_batch_bak.xml.zip";
					String sourceZipFile = sourceFolder + batchIdentifier + File.separator +  batchIdentifier + "_Export_" + batchClassIdentifier + "_batch_bak.xml.zip";
					
					File xmlFile = new File(batchXMLPath);
					File sourceZipXMLFile = new File(sourceZipFile);
					log("sourceZipXMLFile " + sourceZipXMLFile);
					log("xmlFile " + xmlFile);
					log("batchXMLPath " + batchXMLPath);
					log("destZipFile " + destZipFile);
					if(!sourceZipXMLFile.exists()){
						log("sourceZipXMLFile : false");
						if(xmlFile.exists())
						{log("xmlFile : true");
							 writeToZipFile(batchXMLPath, destZipFile);
						}
					}
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

				/*log("Sleep for 2");
				TimeUnit time = TimeUnit.MINUTES;
				try {
					time.sleep(2);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}*/
				
				
				
				
				//String sourceFolder="F:\\WorkingSource\\Ephsoft\\Input";
				//String targetFolder="F:\\WorkingSource\\Ephsoft\\Output";
				String batchFolder = sourceFolder + batchIdentifier;
	  
				File sFile = new File(batchFolder);
		
				//File sFile = new File(sourceFolder);
				log("batchIdentifier Name : " + batchIdentifier);
				// Find files with specified extension
				File[] sourceFiles = sFile.listFiles(new FilenameFilter() {
					String regex = "^"+batchIdentifier;
					Pattern pattern = Pattern.compile(regex, Pattern.CASE_INSENSITIVE);
					
					public boolean accept(File dir, String name) {
						Matcher matcher = pattern.matcher(name);
						 if(name.endsWith(".zip") && matcher.find()) {// change this to your extension
							  return true;
							}else {
							  return false;
							}
					}
				});
				
			 // let us copy each file to the target folder
			 
			 log("zip file Count : " + Integer.toString(sourceFiles.length));
				for(File fSource:sourceFiles) {
					String fName = fSource.getName();
					 String tName = batchIdentifier + "_batch.xml.zip";
					 if(fName.equals(tName)){
						 tName = batchIdentifier + "_Export_" + batchClassIdentifier + "_batch_bak.xml.zip";
						 File fTarget = new File(new File(targetFolder), tName);
						 
						 if(fTarget.exists()){
							fTarget.delete();
						 }
						 
						copyFileUsingStream(fSource,fTarget);
					 }else{
						 File fTarget = new File(new File(targetFolder), fSource.getName());
						copyFileUsingStream(fSource,fTarget);
					 }
				  // fSource.delete(); // Uncomment this line if you want source file deleted
				}
		
		
				
	        //log("End execution of execute() method from " + this.getClass().getName());
				//log(" End execution of Execute function ...");
				
		return exception;
	}
	
	public static void writeToZipFile(String sourceFile, String zipFileName) throws IOException {

        FileOutputStream fos = new FileOutputStream(zipFileName);
        ZipOutputStream zipOut = new ZipOutputStream(fos);
        File fileToZip = new File(sourceFile);
        FileInputStream fis = new FileInputStream(fileToZip);
        ZipEntry zipEntry = new ZipEntry(fileToZip.getName());
        zipOut.putNextEntry(zipEntry);
        byte[] bytes = new byte[1024];
        int length;
        while((length = fis.read(bytes)) >= 0) {
            zipOut.write(bytes, 0, length);
        }
        zipOut.close();
        fis.close();
        fos.close();
    }
	
	/**
	   * Copies a file using the File streams
	   * @param source
	   * @param dest
	   */
	  private static void copyFileUsingStream(File source, File dest)  {
	      InputStream is = null;
	      OutputStream os = null;
	      try {
	          is = new FileInputStream(source);
	          os = new FileOutputStream(dest);
	          byte[] buffer = new byte[1024];
	          int length;
	          while ((length = is.read(buffer)) > 0) {
	              os.write(buffer, 0, length);
	          }
	      }catch(Exception ex) {
	        System.out.println("Unable to copy file:"+ex.getMessage());
	      }  
	      finally {
	        try {
	          is.close();
	          os.close();
	        }catch(Exception ex) {}
	      }
	  }
	
	private void UpdateValidatorDate()throws Exception {
		log("Begin UpdateValidatorDate()");
		
		String REST_URL = "http://10.1.0.41:8090/EphesoftUtilityTesting/Document/UpdateValidatorDate";			
		XMLOutputter xmlOut = new XMLOutputter();
		QCIQLookupRequest requestLoanJson=new QCIQLookupRequest();
		requestLoanJson.inputXML=xmlOut.outputString(document);
		JSONObject responseJson=null;
		JsonResponseWSClient loanServiceClient=new JsonResponseWSClient();
		responseJson=loanServiceClient.invokeJsonResponseWS(requestLoanJson, REST_URL);				
		log("UpdateValidatorDate web service call made.");
		if(responseJson != null) {
			log("UpdateValidatorDate Response Not null.");			
			Gson _gson = new Gson();
			log("ScriptExtraction Result = " + _gson.toJson(responseJson));	
		} else{
			log("UpdateValidatorDate Response NULL.");
		}			
		log("End UpdateValidatorDate()");
	}
	
	/**
	 * This method invokes the WS to Validate the extracted document based on the QCIQ DB Lookup. 
	 * 
	 * @throws Exception
	 */
	
	private void QCIQLookupWSCall()throws Exception {
		log("Begin QCIQLookupWSCall()");
		
		QCIQLookupRequest requestContent=new QCIQLookupRequest();
		XMLOutputter xmlOut = new XMLOutputter();
		requestContent.inputXML=xmlOut.outputString(document);
		requestContent.isManual = false;
		//log(requestContent.inputXML);
		Document responseDoc = null;
		QCIQLookupWSClient client=new QCIQLookupWSClient();
		responseDoc=client.invokeQCIQLookupWS(requestContent, REST_SERVICE_URL);
		
		if (responseDoc != null) {
			log("Ephesoft Utility Web service call Successfull...");
		} else {
			log("Response is Null. Ephesoft Utility Web service call failed.");
			//throw new Exception();
		}
		log("End QCIQLookupWSCall()");
	}
	
	private void UpdateEphesoftValidatorName()throws Exception {
		
		String UPDATE_EPHESOFT_VALIDATOR_NAME = "http://10.1.0.41:8090/EphesoftUtilityTesting/IntellaLendWrapper/UpdateEphesoftValidatorName";	
		
		log("Begin UpdateEphesoftValidatorName()");
		JSONObject responseJson=null;
		QCIQLookupRequest requestLoanJson=new QCIQLookupRequest();
		XMLOutputter xmlOut = new XMLOutputter();
		requestLoanJson.inputXML=xmlOut.outputString(document);
		JsonResponseWSClient loanServiceClient=new JsonResponseWSClient();
		responseJson=loanServiceClient.invokeJsonResponseWS(requestLoanJson, UPDATE_EPHESOFT_VALIDATOR_NAME);
		if (responseJson != null) {
			log("Ephesoft Utility Web service call Successfull...");
		} else {
			log("Response is Null. Ephesoft Utility Web service call failed.");
			//throw new Exception();
		}
		log("End UpdateEphesoftValidatorName()");
	}

	private void calculateColumnTotal(Element documentElement,JSONArray dtArray){
		@SuppressWarnings("unchecked")
		Iterator<JSONObject> dtIterator=dtArray.iterator();
		while(dtIterator.hasNext())
		{
			JSONObject dtObject=dtIterator.next();
			String dtName=(String) dtObject.get("TableName");
			log(dtName);
			JSONArray dtColumnArray=(JSONArray) dtObject.get("Columns");
			@SuppressWarnings("unchecked")
			Iterator<JSONObject> dtColumnIterator=dtColumnArray.iterator();
			while(dtColumnIterator.hasNext())
			{
				JSONObject dtColumnObject=dtColumnIterator.next();
				String dtColumnName=(String) dtColumnObject.get("ColumnName");
				log(dtColumnName);
				String dtDestinationField=(String) dtColumnObject.get("DestinationField");
				log(dtDestinationField);
				double totalAssets=this.getColumnTotal(documentElement,dtName,dtColumnName);
				this.assignColumnTotalValue(documentElement,totalAssets,dtDestinationField);
			}
		}
	}
	
	private void assignColumnTotalValue(Element documentElement,double totalAssets,String dtDestinationField){
		if(totalAssets !=-1)
		{
			Element documentLevelFieldElement = this.documentUtil.getDocumentLevelField(documentElement, dtDestinationField);
			if(null !=documentLevelFieldElement)
			{
				Element valueElement = documentLevelFieldElement.getChild("Value");
				if (null != valueElement){
					String totalAssetValue=valueElement.getText();
					log("before"+totalAssetValue);
				}
				
				valueElement.setText(String.valueOf(totalAssets));
				String changedAssetValue=valueElement.getText();
				log("after"+changedAssetValue);
			}
		}
	}
	
	private double getColumnTotal(Element documentElement,String dtName,String dtColumnName){
		double totalAssets=0;
		Element dataTablesElement=documentElement.getChild("DataTables");
		if(null != dataTablesElement) {
			List<Element> dataTableList=dataTablesElement.getChildren("DataTable");
			for (Element dataTableElement : dataTableList) {
				String dataTableName=dataTableElement.getChildText("Name");
				if(dataTableName.equals(dtName)){
					Element headerRowElement= dataTableElement.getChild("HeaderRow");
					if(null != headerRowElement)
					{
						Element hdrColumnsElement=headerRowElement.getChild("Columns");
						if(null != hdrColumnsElement)
						{
							@SuppressWarnings("unchecked")
							List<Element> hdrColumnList=hdrColumnsElement.getChildren("Column");
							int amountColIndex=getAmountColumnIndex(hdrColumnList,dtColumnName);
							
							if(amountColIndex !=-1)
							{
								Element rowsElement=dataTableElement.getChild("Rows");
								if(null != rowsElement){
									List<Element> rowsList=rowsElement.getChildren("Row");
									if(rowsList.size()==0){
										totalAssets=-1; 
										log("No Rows in"+dtName);
									}
									for (int i = 0; i < rowsList.size(); i++) {
										Element rowElement=(Element)rowsList.get(i);
										Element dtlColumnsElement=rowElement.getChild("Columns");
										if(null != dtlColumnsElement){
											
											Double parsedValue=0.0;
											List<Element> dtlColumnList=dtlColumnsElement.getChildren("Column");
											String amountValue=dtlColumnList.get(amountColIndex).getChildText("Value");
											amountValue=formatAmount(amountValue);
											try {
												parsedValue=Double.parseDouble(amountValue);
											} catch (Exception e) {
												// TODO: handle exception
												parsedValue=0.0;
											}
											totalAssets=totalAssets+parsedValue;
											log("Total Asset Value: "+totalAssets);
										}
									}
								}
							}
							
						}
					}
				}
			}
		}
		return totalAssets;
	}
	
	private int getAmountColumnIndex(List<Element> hrColumnList,String dtColumnName){
		int amountColIndex=-1;
		for (int i = 0; i < hrColumnList.size(); i++) {
			Element hrColElement=(Element)hrColumnList.get(i);
			String columnName=hrColElement.getChildText("Name");
			if(columnName.equals(dtColumnName)){
				amountColIndex=i;
				return amountColIndex;
			}
		}
		return amountColIndex;
	}
	
	private void ProcessDocument(JSONObject commonCollectionObj,String documentType,Element documentElement){
		
		if(commonCollectionObj == null){
			log("commonCollectionObj - NULL");
		}
		
				log("ProcessDocument() - " + documentType);
				
				log("commonCollectionObj - " + commonCollectionObj.toJSONString());
			JSONObject document=(JSONObject) commonCollectionObj.get(documentType);
			if(document == null){
			log("document - NULL");
		}
	        if(document !=null){
				log(document.toJSONString());
	        	//Formatting mmddyyyy fields if any
		        JSONArray dateFieldNames =(JSONArray) document.get(mmddyyyyFieldType);
		        Iterator<String> dateFieldIterator=dateFieldNames.iterator();
		        while(dateFieldIterator.hasNext()){
		        	//Date fieldvalue formatting fns goes here...
		        	String dateFieldName=dateFieldIterator.next();
					log("dateFieldName - " + dateFieldName);
		        	this.formatAndSetFieldsValue(documentElement, dateFieldName,mmddyyyyFieldType);
		        	//log(dateFieldName+" Field is Formatted");
		        }
		        
		      //Formatting mmdd fields if any
		        JSONArray mmddFieldNames =(JSONArray) document.get(mmddFieldType);
		        Iterator<String> mmddFieldIterator=mmddFieldNames.iterator();
		        while(mmddFieldIterator.hasNext()){
		        	//Date fieldvalue formatting fns goes here...
		        	String mmddFieldName=mmddFieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, mmddFieldName,mmddFieldType);
		        	//log(dateFieldName+" Field is Formatted");
		        }
		        
//		        //Formatting Amount fields if any
//	        	JSONArray amountFieldNames =(JSONArray) document.get(AmountFieldType);
//		        Iterator<String> amountFieldIterator=amountFieldNames.iterator();
//		        while(amountFieldIterator.hasNext()){
//		        	//Amount fieldvalue formatting fns goes here...
//		        	String amountFieldName=amountFieldIterator.next();
//		        	this.formatAndSetFieldsValue(documentElement, amountFieldName, AmountFieldType);
//		        	//log(amountFieldIterator.next() +" Field is Formatted");
//		        }
//		        
		      //Formatting DECIMAL84 fields if any
		        JSONArray decimal84FieldNames=(JSONArray) document.get(Decimal84FieldType);
		        Iterator<String> decimal84FieldIterator=decimal84FieldNames.iterator();
		        while(decimal84FieldIterator.hasNext()){
		        	String decimal84FieldName=decimal84FieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, decimal84FieldName, Decimal84FieldType);
		        }
		        
		      //Formatting DECIMAL74 fields if any
	        	JSONArray decimal74FieldNames =(JSONArray) document.get(Decimal74FieldType);
		        Iterator<String> decimal74FieldIterator=decimal74FieldNames.iterator();
		        while(decimal74FieldIterator.hasNext()){
		        	//Amount fieldvalue formatting fns goes here...
		        	String decimal74FieldName=decimal74FieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, decimal74FieldName, Decimal74FieldType);
		        	//log(amountFieldIterator.next() +" Field is Formatted");
		        }
		        
		      //Formatting DECIMAL63 fields if any
	        	JSONArray decimal63FieldNames =(JSONArray) document.get(Decimal63FieldType);
		        Iterator<String> decimal63FieldIterator=decimal63FieldNames.iterator();
		        while(decimal63FieldIterator.hasNext()){
		        	//Amount fieldvalue formatting fns goes here...
		        	String decimal63FieldName=decimal63FieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, decimal63FieldName, Decimal63FieldType);
		        	//log(amountFieldIterator.next() +" Field is Formatted");
		        }
		        
		      //Formatting DECIMAL64 fields if any
	        	JSONArray decimal64FieldNames =(JSONArray) document.get(Decimal64FieldType);
		        Iterator<String> decimal64FieldIterator=decimal64FieldNames.iterator();
		        while(decimal64FieldIterator.hasNext()){
		        	//Amount fieldvalue formatting fns goes here...
		        	String decimal64FieldName=decimal64FieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, decimal64FieldName, Decimal64FieldType);
		        	//log(amountFieldIterator.next() +" Field is Formatted");
		        }
		        
		      //Formatting INT2 fields if any
	        	JSONArray int2FieldNames =(JSONArray) document.get(Int2FieldType);
		        Iterator<String> int2FieldIterator=int2FieldNames.iterator();
		        while(int2FieldIterator.hasNext()){
		        	//Amount fieldvalue formatting fns goes here...
		        	String int2FieldName=int2FieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, int2FieldName, Int2FieldType);
		        	//log(amountFieldIterator.next() +" Field is Formatted");
		        } 
		        
		      //Formatting INTUPTO2 fields if any
	        	JSONArray intUpto2FieldNames =(JSONArray) document.get(IntUpto2FieldType);
		        Iterator<String> intUpto2FieldIterator=intUpto2FieldNames.iterator();
		        while(intUpto2FieldIterator.hasNext()){
		        	//Amount fieldvalue formatting fns goes here...
		        	String intUpto2FieldName=intUpto2FieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, intUpto2FieldName, IntUpto2FieldType);
		        	//log(amountFieldIterator.next() +" Field is Formatted");
		        }
		        
		      //Formatting INTUPTO3 fields if any
	        	JSONArray intUpto3FieldNames =(JSONArray) document.get(IntUpto3FieldType);
		        Iterator<String> intUpto3FieldIterator=intUpto3FieldNames.iterator();
		        while(intUpto3FieldIterator.hasNext()){
		        	//Amount fieldvalue formatting fns goes here...
		        	String intUpto3FieldName=intUpto3FieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, intUpto3FieldName, IntUpto3FieldType);
		        	//log(amountFieldIterator.next() +" Field is Formatted");
		        }
		        
		      //Formatting INTUPTO4 fields if any
	        	JSONArray intUpto4FieldNames =(JSONArray) document.get(IntUpto4FieldType);
		        Iterator<String> intUpto4FieldIterator=intUpto4FieldNames.iterator();
		        while(intUpto4FieldIterator.hasNext()){
		        	//Amount fieldvalue formatting fns goes here...
		        	String intUpto4FieldName=intUpto4FieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, intUpto4FieldName, IntUpto4FieldType);
		        	//log(amountFieldIterator.next() +" Field is Formatted");
		        }
		        
		        //Formatting SSN fields if any
		        JSONArray ssnFieldNames =(JSONArray) document.get(SSNFieldType);
		        Iterator<String> ssnFieldIterator=ssnFieldNames.iterator();
		        while(ssnFieldIterator.hasNext()){
		        	//SSN fieldvalue formatting fns goes here...
		        	String ssnFieldName=ssnFieldIterator.next();
		        	this.formatAndSetFieldsValue(documentElement, ssnFieldName,SSNFieldType);
		        	//log(ssnFieldIterator.next()+" Field is Formatted");
		        }
	        }
	}
	
	public String formatAmount(String inputAmount){
		inputAmount = inputAmount.replace("$", "").replace(",", "").replace("%", "").replaceAll("\\s+","");;
		String regex = "\\.(?=.*\\.)";
		inputAmount = inputAmount.replaceAll(regex, "");
		return inputAmount;
 	}
	
	private String formatSSN(String inputSSN){
		//String regex = "^[A-Za-z0-8][A-Za-z0-9]{2}-[A-Za-z0-9]{2}-[A-Za-z0-9]{4}$";
		//Pattern pattern = Pattern.compile(regex);
		//Matcher matcher = pattern.matcher(inputSSN);
	    //if(matcher.matches()){
	    //	return inputSSN;
	    //}else{
	    	inputSSN=inputSSN.replace("-", "").replaceAll("\\s+","");
	    	//if(inputSSN.length()>0 && inputSSN.length()>=9){
	    	//	return String.format("%s%s%s", inputSSN.substring(0, 3), inputSSN.substring(3,5), inputSSN.substring(5,9));
	    	//}else{
	    	//	return inputSSN;
	    	//}
			return inputSSN;
	    //}
	}
	
	private String formatDecimal(String inputAmount,int maxInteger,int maxFraction)
	{
		inputAmount = inputAmount.replace("$", "").replace(",", "").replace("%", "").replaceAll("\\s+","");;
		String regex = "\\.(?=.*\\.)";
		inputAmount = inputAmount.replaceAll(regex, "");
		try {
		BigDecimal bd = new BigDecimal(inputAmount);
		DecimalFormat myFormatter = new DecimalFormat();
		myFormatter.setMaximumIntegerDigits(maxInteger);
		myFormatter.setMaximumFractionDigits(maxFraction);
		inputAmount = myFormatter.format(bd);
		}catch(NumberFormatException e){
			log(e.getMessage());
		}
		//System.out.println(inputAmount);
		return inputAmount;
		
	}
	
	private String formatInt(String inputAmount,int maxInteger){
		inputAmount = inputAmount.replace("$", "").replace(",", "").replace("%", "").replaceAll("\\s+","");;
		String regex = "\\.(?=.*\\.)";
		inputAmount = inputAmount.replaceAll(regex, "");
		try {
		int inputValue=Integer.parseInt(inputAmount);
		Formatter fmt = new Formatter();
		inputAmount=fmt.format("%02d", inputValue).toString();
		
		inputAmount = inputAmount == null || inputAmount.length() < maxInteger ? inputAmount : inputAmount.substring(inputAmount.length() - maxInteger);
		}catch(NumberFormatException e){
			log(e.getMessage());
		}
		return inputAmount;
	}
	
	private void formatAndSetFieldsValue(Element documentElement,String fieldName,String fieldType){
		Element dateElement = this.documentUtil.getDocumentLevelField(documentElement, fieldName);
		if(null != dateElement) {
			String formattedFieldValue = null;
			String fieldValue = documentUtil.getFieldValue(dateElement);
			if(!fieldValue.isEmpty()) {
				if(fieldType == mmddyyyyFieldType){
					//log("Date Field value from document - " + fieldValue);
					String INVALID_CHARS = "in;the;year;d";
					String[] arr = INVALID_CHARS.toLowerCase().split(";");
					for(int i=0;i < arr.length;i++)
					{
						if (fieldValue.contains(arr[i]))
						{
							fieldValue = fieldValue.replace(arr[i], " ");
						}
					}
					formattedFieldValue = DateParseUtil.ParseDate(fieldValue);
				}else if(fieldType == mmddFieldType){
					//log("Date Field value from document - " + fieldValue);
					//String INVALID_CHARS = "in;the;year;d";
					//String[] arr = INVALID_CHARS.toLowerCase().split(";");
					//for(int i=0;i < arr.length;i++)
					//{
					//	if (fieldValue.contains(arr[i]))
					//	{
					//		fieldValue = fieldValue.replace(arr[i], " ");
					//	}
					//}
					formattedFieldValue = DateParseUtil.ParseDate(fieldValue, "MM/dd");
					//formattedFieldValue = formattedFieldValue == null || formattedFieldValue.length() < 5 ? formattedFieldValue : formattedFieldValue.substring(0,5);
					//log("Parsed date value - " + formattedFieldValue);
				}else if(fieldType == SSNFieldType){
					formattedFieldValue=this.formatSSN(fieldValue);
				}
//				else if(fieldType == AmountFieldType){
//					formattedFieldValue=this.formatAmount(fieldValue);
//				}
				else if(fieldType==Decimal84FieldType){
					formattedFieldValue=this.formatDecimal(fieldValue, 8, 4);
				}else if(fieldType==Decimal74FieldType){
					formattedFieldValue=this.formatDecimal(fieldValue, 7, 4);
				}else if(fieldType==Decimal63FieldType){
					formattedFieldValue=this.formatDecimal(fieldValue, 6, 3);
				}else if(fieldType==Decimal64FieldType){
					formattedFieldValue=this.formatDecimal(fieldValue, 6, 4);
				}else if(fieldType==Int2FieldType){
					formattedFieldValue=this.formatInt(fieldValue, 2);
				}else if(fieldType==IntUpto2FieldType){
					formattedFieldValue=this.formatInt(fieldValue, 2);
				}else if(fieldType==IntUpto3FieldType){
					formattedFieldValue=this.formatInt(fieldValue, 3);
				}else if(fieldType==IntUpto4FieldType){
					formattedFieldValue=this.formatInt(fieldValue, 4);
				}
				documentUtil.setFieldValue(dateElement, formattedFieldValue);
			}
		}
	}
	
	// Property Address Field update
	
	public void DocPropertyFieldUpdate()
	{
		List<Element> documentList = this.documentUtil.getDocumentElements();
		for(Element documentElement : documentList) 
		{
			Element _propertydocument =this.documentUtil.getDocumentLevelField(documentElement,DocumentFieldParentName);	
             if( _propertydocument !=null)
			  {
				Element dlFieldName=this.documentUtil.getChild(_propertydocument, DocumentFieldChildProperty);			
				if(dlFieldName != null)
					{
					Boolean _Fieldval = false;
					Element _porpertyvalueElement = _propertydocument.getChild("Value");
					
			      	String _PropertyDocFieldValue =	_porpertyvalueElement.getText();
					   String IsFieldValue =  dlFieldName.getText();				
					   if( Boolean.parseBoolean(IsFieldValue) == true)
					   {
						   String DocFieldValue =  this.GetDocumentLevelFieldValue(documentElement);
						   Element validElement = _propertydocument.getChild("Value");
						   validElement.setText(DocFieldValue);				
					   }
					  else if( Boolean.parseBoolean(IsFieldValue) == false )
					  {
						 this.GetDocFieldListValue(_PropertyDocFieldValue,documentElement);						
					  }
					}
			  }
             
             // Update Borrower Name  & Co-Borrower Name
             this.BorrowerNameUpdate(documentElement);
          }
	}
	public void GetDocFieldListValue(String _DocFieldValue,Element documentElement)
	{
		QCIQLookupRequest requestLoanJson=new QCIQLookupRequest();
		requestLoanJson.inputXML=_DocFieldValue;
		JSONObject responseJson=null;
		JsonResponseWSClient loanServiceClient=new JsonResponseWSClient();
		 responseJson=loanServiceClient.invokeJsonResponseWS(requestLoanJson, PROPERTY_REQUEST_URL);
		log("Ephesoft Loan web service call successful.");
		this.SetDocPropertyValues(responseJson,documentElement);
	}
	public void SetDocPropertyValues(JSONObject jsondata ,Element documentElement)
	{
		for(int i=0;i< DOCUMENTFIELDS.size();i++)
		{
			Element _propertydocField =this.documentUtil.getDocumentLevelField(documentElement,DOCUMENTFIELDS.get(i));	
			if(_propertydocField != null)
			{
				  Element validElement = _propertydocField.getChild("Value");
				  String _value = (String) jsondata.get(DOCUMENTFIELDS.get(i));
				  validElement.setText(_value);	
			}
		}		
	}
	public String GetDocumentLevelFieldValue(Element documentElement)
	{
		ArrayList<String> _propertyDocFieldValue=new ArrayList<String>();   
		for(int i=0;i< DOCUMENTFIELDS.size();i++)
		{
			Element _propertydocField =this.documentUtil.getDocumentLevelField(documentElement,DOCUMENTFIELDS.get(i));	
			if(_propertydocField != null)
			{
				 Element validElement = _propertydocField.getChild("Value");
				_propertyDocFieldValue.add(validElement.getValue());
			}
		}
		String _FieldValue = String.join(",",_propertyDocFieldValue);
		return _FieldValue;
	}	
	
	// Borrower Name Update 
	public void BorrowerNameUpdate(Element documentElement)
	{
			// Update Borrower FirstName And Last Name
			for (int i=0;i<Document_BorrowerName_List.size();i++ )
			{		
			 Element _BorrowerNamedocument =this.documentUtil.getDocumentLevelField(documentElement,Document_BorrowerName_List.get(i));
	    	  if( _BorrowerNamedocument !=null)
			   {
				Element BrFieldName=this.documentUtil.getChild(_BorrowerNamedocument, "Value");
				
				if(BrFieldName != null)
					{
			      	String _BorrowerName =	BrFieldName.getText(); 
			        this.SplitandUpdateBorrwoerName(_BorrowerName,documentElement,Document_Borrower_FirstName,Document_Borrower_LastName);  
			        }  
			   }
			} 
			//Update Borrower FirstName And Last Name
			for(int i=0;i<Document_Co_BorrowerName_List.size();i++)
			{
				Element _CO_BorrowerNamedocument =this.documentUtil.getDocumentLevelField(documentElement,Document_Co_BorrowerName_List.get(i));
             if( _CO_BorrowerNamedocument !=null)
			  {
				Element Co_BrFieldName=this.documentUtil.getChild(_CO_BorrowerNamedocument, "Value");
				
				if(Co_BrFieldName != null)
					{
			      	String _Co_BorrowerName =	Co_BrFieldName.getText(); 
			        this.SplitandUpdateBorrwoerName(_Co_BorrowerName,documentElement,Document_Co_Borrower_FirstName,Document_Co_Borrower_LastName);  
			      }
			  }
			}
			
			//Update Veteran FirstName And Last Name
			for(int i=0;i< Document_VeteranName_List.size();i++)
			{
				Element _VeteranNamedocument =this.documentUtil.getDocumentLevelField(documentElement,Document_VeteranName_List.get(i));
             if( _VeteranNamedocument !=null)
			  {
				Element VeteranFieldName=this.documentUtil.getChild(_VeteranNamedocument, "Value");
				
				if(VeteranFieldName != null)
					{
			      	String _VeteranName =	VeteranFieldName.getText(); 
			        this.SplitandUpdateBorrwoerName(_VeteranName,documentElement,Document_Veteran_FirstName,Document_Veteran_LastName);  
			      }
			  }
			}
	}
	public void SplitandUpdateBorrwoerName(String Name, Element documentElement,String FirstName,String LastName)
		{
			String[] names = Name.split(" ");
			Element _firstNamedocument =this.documentUtil.getDocumentLevelField(documentElement,FirstName);
			Element _lastNamedocument =this.documentUtil.getDocumentLevelField(documentElement,LastName);			
			if(names.length > 1){
				if( _firstNamedocument !=null)
				  {
					Element BrFieldName=_firstNamedocument.getChild("Value");
					BrFieldName.setText(names[0]);
			      }
			}
			if(names.length > 2){
				if(_lastNamedocument !=null)
				{
					Element BrFieldName=_lastNamedocument.getChild("Value");
					BrFieldName.setText(names[1] + " " +  names[2]);
				}
			}
			if(names.length == 2){
				if(_lastNamedocument !=null)
				{
					Element BrFieldName=_lastNamedocument.getChild("Value");
					BrFieldName.setText(names[1]);
				}
			}
		}
		
	/**
	 * @param message
	 */
	private void log(String message) {
		this.logUtil.log(message);
	}
	
	/**
	 * @param document
	 */
	private void initialize(Document document) {
		this.logUtil = new LogUtil(logLevel, logger);
		super.initialize(document, logLevel);
	}

}
