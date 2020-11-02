import java.io.IOException;
import java.lang.reflect.Method;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.Iterator;
import java.util.List;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

import org.apache.axis.utils.StringUtils;
import org.apache.http.client.ClientProtocolException;
import org.jdom.Document;
import org.jdom.Element;
import org.jdom.JDOMException;
import org.jdom.output.XMLOutputter;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;
import com.mts.idc.ephesoft.BaseScript;
import com.mts.idc.ephesoft.DateParseUtil;
import com.mts.idc.ephesoft.LogLevel;
import com.mts.idc.ephesoft.LogUtil;
import com.mts.idc.ephesoft.SQL_AUTH;
import com.mts.idc.ephesoft.service.QCIQLookupRequest;
import com.mts.idc.ephesoft.service.QCIQLookupWSClient;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


/**
 * Date Modified: 01/07/2015
 * 
 * @author Atit Patel
 */
public class ScriptFunctionKey extends BaseScript {

	/*** MTS Settings ***/

	private static List<String> LIMITED_FIELDS = Arrays.asList("Borrower Name","Property Address","Loan ID","Loan Number","Loan No","LoanID","LoanNumber","LoanNo","File No/Loan No","Lender Loan No","Lender Loan Number","New Loan No","Lender No","New Loan Number","Loan Application Number","Lender Loan No");
	private static List<String> COMMON_FIELDS = Arrays.asList("Borrower Name","Property Address","Loan ID","Loan Number","Loan No","LoanID","LoanNumber","LoanNo","File No/Loan No","Lender Loan No","Lender Loan Number","New Loan No","Lender No","New Loan Number","Loan Application Number","Lender Loan No");
	
	private static List<String> COBORROWER_FIELDS = Arrays.asList("Co-Borrower Address","Co-Borrower Business Phone","Co-Borrower Dependents","Co-Borrower DOB","Co-Borrower Employed Since","Co-Borrower Employer Address","Co-Borrower Formerly Self-Employed 3","Co-Borrower Formerly Self-Employed 4","Co-Borrower Home Phone No","Co-Borrower Name","Co-Borrower Old Employed 3 From","Co-Borrower Old Employed 3 To","Co-Borrower Old Employed 4 From","Co-Borrower Old Employed 4 To","Co-Borrower Old Employer 3 Address","Co-Borrower Old Employer 3 Name","Co-Borrower Old Employer 4 Address","Co-Borrower Old Employer 4 Name","Co-Borrower Old Employment 3 Income","Co-Borrower Old Employment 3 Phone","Co-Borrower Old Employment 3 Title","Co-Borrower Old Employment 4 Income","Co-Borrower Old Employment 4 Phone","Co-Borrower Old Employment 4 Title","Co-Borrower Profession Duration","Co-Borrower signed Date","Co-Borrower SSN","Co-Borrower Title","Co-BorrowerEmployerName","CoBorrowerAddressType","CoBorrowerEthnicity","CoBorrowerFormerAddress","CoBorrowerFormerAddressType","CoBorrowerFormerAddressYearsResided","CoBorrowerFormerlySelfEmployed1","CoBorrowerFormerlySelfEmployed2","CoBorrowerMaritalStatus","CoBorrowerOldEmployed1From","CoBorrowerOldEmployed1To","CoBorrowerOldEmployed2From","CoBorrowerOldEmployed2To","CoBorrowerOldEmployer1Address","CoBorrowerOldEmployer1Name","CoBorrowerOldEmployer2Address","CoBorrowerOldEmployer2Name","CoBorrowerOldEmployment1Income","CoBorrowerOldEmployment1Phone","CoBorrowerOldEmployment1Title","CoBorrowerOldEmployment2Income","CoBorrowerOldEmployment2Phone","CoBorrowerOldEmployment2Title","CoBorrowerRace","CoBorrowerRefusal","CoBorrowerSelfEmployed","CoBorrowerSex","CoBorrowerYearResided","CoBorrowerFormerlySelfEmployed3","CoBorrowerFormerlySelfEmployed4","CoBorrowerOldEmployed3From","CoBorrowerOldEmployed3To","CoBorrowerOldEmployed4From","CoBorrowerOldEmployed4To","CoBorrowerOldEmployer3Address","CoBorrowerOldEmployer3Name","CoBorrowerOldEmployer4Address","CoBorrowerOldEmployer4Name","CoBorrowerOldEmployment3Income","CoBorrowerOldEmployment3Phone","CoBorrowerOldEmployment3Title","CoBorrowerOldEmployment4Income","CoBorrowerOldEmployment4Phone","CoBorrowerOldEmployment4Title");

	private static final String REST_SERVICE_URL = "http://10.0.2.229/RevampEphesoftUtilityAPI/FieldMapping/Execute";
	private static String JSON_FIELDUPDATES="[{\"DocumentType\": \"Closing Disclosure\",\"FieldName\": \"Date Issued\",\"FieldType\": \"Date\",\"SortBy\": \"Desc\"},{\"DocumentType\": \"Loan Application 1003 Format 1\",\"FieldName\": \"Borrower signed Date\",\"FieldType\": \"Date\",\"SortBy\": \"Desc\"},{\"DocumentType\": \"Loan Application 1003 Format 2\",\"FieldName\": \"Borrower signed Date\",\"FieldType\": \"Date\",\"SortBy\": \"Desc\"}]";
		
	//fields for ephesoft DBQUERY
		private static final String DB_HOST = "10.0.2.137";
		private static final String DB_PORT = "1433";
		private static final String DB_NAME = "Ephesoft";
		private static final String DB_DOMAIN = null;
		private static final String DB_USER = "sa";
		private static final String DB_PASSWORD = "sadmin";
		private static final String DB_INSTANCE = null;
		private static final String DB_CLASS = "net.sourceforge.jtds.jdbc.Driver";
		private static final String DB_DRIVER = "jdbc:jtds:sqlserver";
		private static String DB_URL;
	/*** End of MTS Settings ***/	

	private LogUtil logUtil;
	private LogLevel logLevel = LogLevel.INFO;
	private Logger logger = LoggerFactory.getLogger(ScriptFunctionKey.class);

	/*
	 * (non-Javadoc)
	 * 
	 * @see com.ephesoft.dcma.script.IJDomScript#execute(org.jdom.Document,
	 * java.lang.String, java.lang.String)
	 */
	public Object execute(Document document, String methodName,
			String docIdentifier) {
		//log("Inside execute function....");
		Exception exception = null;
		try {
			if (null == document) {
				return new Exception("Input document is null");
			}

			// Initialize
			this.initialize(document);

			// Invoke method
			Method method = this.getClass().getMethod(methodName, String.class);
			method.invoke(this, docIdentifier);

			log("End execution of execute() method from "
					+ this.getClass().getName());

		} catch (Exception e) {
			log("Error occured - " + e.getMessage());
			e.printStackTrace();
			exception = e;
		}
		return exception;
	}
	
	/**
	 * This method invokes the WS to Validate the extracted document based on the QCIQ DB Lookup. 
	 * 
	 * @throws Exception
	 */
	
	public void QCIQLookupWSCall(String docIdentifier)  {
		log("Function Key Inside the Ephesoft QCIQ Lookup web service call .");
		
		QCIQLookupRequest requestContent=new QCIQLookupRequest();
		XMLOutputter xmlOut = new XMLOutputter();
		requestContent.inputXML=xmlOut.outputString(document);
		//log(requestContent.inputXML);
		requestContent.isManual = true;
		Document responseDoc = null;		
		QCIQLookupWSClient client=new QCIQLookupWSClient();
		
		try {
			responseDoc=client.invokeQCIQLookupWS(requestContent, REST_SERVICE_URL);
		} catch (ClientProtocolException e) {
			// TODO Auto-generated catch block
			log("Error occured - " + e.getMessage());
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			log("Error occured - " + e.getMessage());
			e.printStackTrace();
		} catch (JDOMException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			log("Error occured - " + e.getMessage());
		}
			if (responseDoc != null) {
			document.detachRootElement();
			if (!document.hasRootElement()) {
				document.setRootElement(responseDoc.detachRootElement());
				
				this.documentUtil.writeToXML(this.document);
			}
		} else {
			log("Response is Null. Ephesoft Utility Web service call failed.");
			//throw new Exception();
		}
		log("QCIQLookupWSCall().");
		
	}
	
	/**
	 * Finding the number of documents available in the name of configured docType
	 * @param docType
	 * @return docTypeCount as an integer
	 */
	
	private int getDocCountByDocType(String docType){
		int docTypeCount=0;
		List<Element> documentList = this.documentUtil.getDocumentElements();
		for(Element documentElement : documentList) 
		{
			String documentType = documentElement.getChildText("Type");	
			if(documentType.equalsIgnoreCase(docType)){
				docTypeCount++;
			}
		}
		return docTypeCount;
	}
	/**
	 * Get the list of documents available in the name of configured docType
	 * @param docType
	 * @return selectedDocumentList as a list of elements
	 */
	private List<Element> getDocumentElementsByDocType(String docType){
		List<Element> selectedDocumentList=new ArrayList<Element>();
		List<Element> documentList = this.documentUtil.getDocumentElements();
		for(Element documentElement : documentList) 
		{
			String documentType = documentElement.getChildText("Type");	
			if(documentType.equalsIgnoreCase(docType))
			{
				selectedDocumentList.add(documentElement);
			}
		}
		return selectedDocumentList;
	}
	
	/**
	 * Getting the dictionary of document fields from the selected document element
	 * @param documentElement to get the list of fields
	 * @return dlFieldsDictionary as a list of document fields in key,value pairs format
	 */
	private Map<String, String> getFieldDictionaryOfDocument(Element documentElement){
		Map<String, String> dlFieldsDictionary = new HashMap<String, String>();
		List<Element> documentLevelFieldsList =this.documentUtil.getDocumentLevelFieldElements(documentElement);
        
        for (Element element : documentLevelFieldsList) {
        	if (element !=null) {
        		Element nameElement = element.getChild("Name");
        		if(nameElement !=null){
        			String fieldKey = nameElement.getText();
    				String fieldValue = this.documentUtil.getFieldValue(element);
    				dlFieldsDictionary.put(fieldKey, fieldValue);
    				log(fieldKey+" : "+fieldValue);
        		}
			}
		}
		return dlFieldsDictionary;
	}
	
	/**
	 * Updating document level fields using the dictionary key values
	 * @param docIdentifier to get the document element to be updated
	 * @param dlFieldsDictionary to get the field values to update 
	 * @return nothing
	 */
	private void updateDocumentFieldsUsingDictionary(String docIdentifier,Map<String, String> dlFieldsDictionary){
		Element documentElement = this.documentUtil.getDocumentElement(docIdentifier);
		List<Element> documentLevelFieldsList =this.documentUtil.getDocumentLevelFieldElements(documentElement);
		for (Element documentField : documentLevelFieldsList) {
			if (documentField !=null) {
				Element nameElement = documentField.getChild("Name");
				if(nameElement !=null)
				{
					String fieldKey = nameElement.getText();
					if (dlFieldsDictionary.containsKey(fieldKey)) {
						Element fieldElement = this.documentUtil.getDocumentLevelField(documentElement, fieldKey);
						if (fieldElement !=null) {
							log("current "+fieldKey+": "+this.documentUtil.getFieldValue(fieldElement));
							this.documentUtil.setFieldValue(fieldElement,dlFieldsDictionary.get(fieldKey));
							log("Changed value of "+ fieldKey+": "+this.documentUtil.getFieldValue(fieldElement));
						}
					}
				}
				
			}
		}
	}
	/**
	 * Copying all the document level fields from the particular document selected based on the configuration and
	 * Updating the copied fields to the current document level fields
	 * @param docIdentifier to get the document element to be updated
	 * @return nothing
	 */
	public void copyAllFieldsFromSelectedDocToCurrentDocument(String docIdentifier)
	{
		log("Inside copyAllFieldsFromSelectedDocToCurrentDocument()...for Batch: " + docIdentifier);
		
		JSONParser fieldUpdatesParser = new JSONParser();
		try {
			JSONArray configuredArray=(JSONArray) fieldUpdatesParser.parse(JSON_FIELDUPDATES);
			if(configuredArray != null)
			{
				@SuppressWarnings("unchecked")
				Iterator<JSONObject> jsonObjIterator=configuredArray.iterator();
				while(jsonObjIterator.hasNext())
				{
					JSONObject jsonObject=jsonObjIterator.next();
					String docType=(String) jsonObject.get("DocumentType");
					String fieldName=(String) jsonObject.get("FieldName");
					String fieldType=(String) jsonObject.get("FieldType");
					String sortBy=(String) jsonObject.get("SortBy");
					//Finding the number of documents available in the name of configured docType
					int docTypeCount=this.getDocCountByDocType(docType);
					if(docTypeCount >1){
						log("Document Name:"+docType+" and Available Count :"+docTypeCount);
						//Get the list of documents available in the name of configured docType
						List<Element> selectedDocumentList=this.getDocumentElementsByDocType(docType);
						
						if(fieldType.equalsIgnoreCase("date")){
							ArrayList<Date> unsortedArray=new ArrayList<Date>();
							ArrayList<Date> sortedArray =null;
							SimpleDateFormat formatter = new SimpleDateFormat("MM/dd/yyyy");
							for (Element documentElement : selectedDocumentList) {
								Element fieldElement = this.documentUtil.getDocumentLevelField(documentElement,fieldName);
								if (fieldElement !=null) {
									String fieldValue = this.documentUtil.getFieldValue(fieldElement);
									Date date=null;
									try {
										fieldValue=DateParseUtil.ParseDate(fieldValue);
										if(fieldValue !=null){
										date= formatter.parse(fieldValue);
										}
									} catch (java.text.ParseException e) {
										// TODO Auto-generated catch block
										e.printStackTrace();
									}
									if(fieldValue !=null){
									unsortedArray.add(date);
									}
									log(fieldName+" : "+fieldValue+ ": "+date);
								}
							}
							
							sortedArray=new ArrayList<Date>(unsortedArray);
							if(sortBy.equalsIgnoreCase("Asc"))
							{
								Collections.sort(sortedArray);
					            log("Dates in Assending Order: "+sortedArray);
							}else{
								Collections.sort(sortedArray, Collections.reverseOrder());
					            log("Dates in Descending Order: "+sortedArray);
							}
							int docIndexToBeSelected=unsortedArray.indexOf(sortedArray.get(0));
							log("Document index to be selected : "+docIndexToBeSelected);
							if(docIndexToBeSelected !=-1){
								Element selecteddocumentElement=selectedDocumentList.get(docIndexToBeSelected);
								//Getting the dictionary of document fields from the selected document element
								Map<String, String> dlFieldsDictionary=this.getFieldDictionaryOfDocument(selecteddocumentElement);
								//Updating document level fields using the dictionary key values
						        this.updateDocumentFieldsUsingDictionary(docIdentifier, dlFieldsDictionary);
							}
							
						}else{
							ArrayList<String> unsortedArray=new ArrayList<String>();
							ArrayList<String> sortedArray =null;
							for (Element documentElement : selectedDocumentList) {
								Element fieldElement = this.documentUtil.getDocumentLevelField(documentElement,fieldName);
								if (fieldElement !=null) {
									String fieldValue = this.documentUtil.getFieldValue(fieldElement);
									if(!fieldValue.isEmpty()){
									unsortedArray.add(fieldValue);
									log(fieldName+" : "+fieldValue);
									}
								}
							}
							sortedArray=new ArrayList<String>(unsortedArray);
							if(sortBy.equalsIgnoreCase("Asc")){
								Collections.sort(sortedArray);
								log("sortedList in ascending order: " + sortedArray);
							}else {
								Collections.sort(sortedArray, Collections.reverseOrder()); 
						        log("sortedList in descending order: " + sortedArray);
							}
							int docIndexToBeSelected=unsortedArray.indexOf(sortedArray.get(0));
							log("Document index to be selected : "+docIndexToBeSelected);
							if(docIndexToBeSelected !=-1){
								Element selecteddocumentElement=selectedDocumentList.get(docIndexToBeSelected);
								//Getting the dictionary of document fields from the selected document element
						        Map<String, String> dlFieldsDictionary=this.getFieldDictionaryOfDocument(selecteddocumentElement);
						        //Updating document level fields using the dictionary key values
						        this.updateDocumentFieldsUsingDictionary(docIdentifier, dlFieldsDictionary);
							}
					}
					// Write changes back to batch xml
					this.documentUtil.writeToXML(this.document);
					log("Completed the execution of copyAllFieldsFromSelectedDocToCurrentDocument()...");
					}
				}
			}
			
		} catch (ParseException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	/**
	 * Copy list of common fields from the previous document during a function button
	 * key press
	 * 
	 * @param docIdentifier
	 * @return
	 */
	public void copyAllFieldsFromPreviousDoc(String docIdentifier){
		log("Inside copyAllFieldsFromPreviousDoc()...for batch id: " + docIdentifier);
		//this.copyFieldsFromPreviousDoc(docIdentifier, COMMON_FIELDS);
		this.copyCommonFieldsFromPreviousDocToCurrentDoc(docIdentifier);

		// Write changes back to batch xml
		this.documentUtil.writeToXML(this.document);
		log("completed the execution of copyAllFieldsFromPreviousDoc()...");
	}
	
	/**
	 * Copy list of common fields from the Current document during a function button
	 * key press and paste it to all the documents
	 * 
	 * @param docIdentifier
	 * @return
	 */
	public void copyToAllDocsFromCurrentDoc(String docIdentifier)
	{
		log("Inside copyToAllDocsFromCurrentDoc()...for batch id: " + docIdentifier);
		this.copyFieldsFromCurrentDoc(docIdentifier, COMMON_FIELDS);
		
		// Write changes back to batch xml
		this.documentUtil.writeToXML(this.document);
		log("completed the execution of copyToAllDocsFromCurrentDoc()...");
	}
	
	/**
	 * Copy Loan No,Borrower Name,Property Address from the previous document during a function button
	 * key press
	 * 
	 * @param docIdentifier
	 * @return
	 */
	public void copyLimitedFieldsFromPreviousDoc(String docIdentifier){
		log("Inside copyLimitedFieldsFromPreviousDoc()...for batch id: " + docIdentifier);
		this.copyFieldsFromPreviousDoc(docIdentifier, LIMITED_FIELDS);
		
		// Write changes back to batch xml
		this.documentUtil.writeToXML(this.document);
		log("completed the execution of copyLimitedFieldsFromPreviousDoc()...");
	}
	/**
	 * Copy the list of fields from the previous document 
	 * @param docIdentifier
	 * @param fieldList contains the list of fields to be copied from previous document
	 * @return
	 */
	public void copyFieldsFromPreviousDoc(String docIdentifier,List<String> fieldList){
		log("Inside copyFieldsFromPreviousDoc()...for batch id: " + docIdentifier);
		Map<String, String> dlFieldsDictionary = new HashMap<String, String>();
	
		// Get Previous Document
		Element documenPrevElement = this.documentUtil.getPreviousDocumentElement(docIdentifier);
		if (documenPrevElement !=null) {
			log("Previous document level field details...");
			for (String field : fieldList) {
				Element fieldElement = this.documentUtil.getDocumentLevelField(documenPrevElement,field);
				if (fieldElement !=null) {
					String fieldValue = this.documentUtil.getFieldValue(fieldElement);
					dlFieldsDictionary.put(field, fieldValue);
					log(field+" : "+fieldValue);
				}
				
			}
		}

		// Get current document element
		Element documentElement = this.documentUtil.getDocumentElement(docIdentifier);
		for (String field : fieldList) {
			if (dlFieldsDictionary.containsKey(field)) {
				Element fieldElement = this.documentUtil.getDocumentLevelField(documentElement, field);
				if (fieldElement !=null) {
					log("current "+field+": "+this.documentUtil.getFieldValue(fieldElement));
					this.documentUtil.setFieldValue(fieldElement,dlFieldsDictionary.get(field));
					log("Changed value of "+ field+": "+this.documentUtil.getFieldValue(fieldElement));
				}
			}
		}
		log("completed the execution of copyFieldsFromPreviousDoc()...");
	}
	
	/**
	 * Copy the list of common fields from the previous document 
	 * @param docIdentifier
	 * @return
	 */
	public void copyCommonFieldsFromPreviousDocToCurrentDoc(String docIdentifier)
	{
		log("Inside copyFieldsFromPreviousDoc()...for batch id: " + docIdentifier);
		Map<String, String> dlFieldsDictionary = new HashMap<String, String>();
		
		// Get all the documentlevel fields from Previous Document
				Element documenPrevElement = this.documentUtil.getPreviousDocumentElement(docIdentifier);
				if (documenPrevElement !=null) {
					log("Previous document level field details...");
					List<Element> documentLevelFieldsList =this.documentUtil.getDocumentLevelFieldElements(documenPrevElement);
			        
			        for (Element element : documentLevelFieldsList) 
			        {
			        	if (element !=null) 
			        	{
			        		Element nameElement = element.getChild("Name");
			        		if(nameElement !=null){
			        			String fieldKey = nameElement.getText();
			    				String fieldValue = this.documentUtil.getFieldValue(element);
			    				dlFieldsDictionary.put(fieldKey, fieldValue);
			    				log(fieldKey+" : "+fieldValue);
			        		}
						}
					}
				}
	
				// Get update the common fields from previous doc to  current document element
				this.updateDocumentFieldsUsingDictionary(docIdentifier, dlFieldsDictionary);
				log("completed the execution of copyFieldsFromPreviousDoc()...");
	}
	
	/**
	 * Copy the list of fields from the Current document 
	 * @param docIdentifier
	 * @param fieldList contains the list of fields to be copied from Current document
	 * @return
	 */
	public void copyFieldsFromCurrentDoc(String docIdentifier,List<String> fieldList){
		log("Inside copyFieldsFromCurrentDoc()...for batch id: " + docIdentifier);
		Map<String, String> dlFieldsDictionary = new HashMap<String, String>();
	
		// Get Configured Fields from current document element and prepare a FieldsDictionary
		Element currentDocumenElement = this.documentUtil.getDocumentElement(docIdentifier);
		if (currentDocumenElement !=null) {
			log("Current document level field details...");
			for (String field : fieldList) {
				Element fieldElement = this.documentUtil.getDocumentLevelField(currentDocumenElement,field);
				if (fieldElement !=null) {
					String fieldValue = this.documentUtil.getFieldValue(fieldElement);
					dlFieldsDictionary.put(field, fieldValue);
					log(field+" : "+fieldValue);
				}
				
			}
		}

		// Copy fieldDictionary values to all the document
		List<Element> documentElements = this.documentUtil.getDocumentElements();
		for(Element documentElement : documentElements) 
		{
			for (String field : fieldList) {
				if (dlFieldsDictionary.containsKey(field)) {
					Element fieldElement = this.documentUtil.getDocumentLevelField(documentElement, field);
					if (fieldElement !=null) {
						log("current "+field+": "+this.documentUtil.getFieldValue(fieldElement));
						this.documentUtil.setFieldValue(fieldElement,dlFieldsDictionary.get(field));
						log("Changed value of "+ field+": "+this.documentUtil.getFieldValue(fieldElement));
					}
				}
			}
		}
		log("completed the execution of copyFieldsFromCurrentDoc()...");
	}
	/**
	 * Gets the regexpattern details for validation as dictionary.
	 * Changing all ForceReview value of current documentfields to false if the regex pattern matches the fieldvalue
	 * Changing Valid and Reviewed tag value of current document to true if all the forceReviewElement value  of the current documentLevel fields are false
	 * @param docIdentifier : Current  Document Identifier
	 **/
	public void ValidateDocumentUsingRegex(String docIdentifier)
	{
		// Get current document element
		Element documentElement = this.documentUtil.getDocumentElement(docIdentifier);
		String doctypeName=this.documentUtil.getDocType(documentElement);
		String batchIdentifier=this.documentUtil.getBatchClassIdentifier();
		Map<String, String> regexFieldDictionary=getRegexValidationDetails(batchIdentifier, doctypeName);
		Element documentLevelFieldsElement = documentElement.getChild("DocumentLevelFields");
		
		if(null != documentLevelFieldsElement) {
			List<?> documentLevelFieldList = documentLevelFieldsElement.getChildren("DocumentLevelField");
			
			for(int i=0; i<documentLevelFieldList.size(); i++) 
			{
				Element documentLevelFieldElement = (Element)documentLevelFieldList.get(i);
				
				Element fieldNameElement=documentLevelFieldElement.getChild("Name");
				if(null != fieldNameElement)
				{
					String dlFieldName=fieldNameElement.getText();
					if(regexFieldDictionary.containsKey(dlFieldName))
					{
						String dlFieldValue=this.documentUtil.getFieldValue(documentLevelFieldElement);
						if(dlFieldValue.equals(""))
						{
							Element forceReviewElement = documentLevelFieldElement.getChild("ForceReview");
							if (null != forceReviewElement)
							{
								forceReviewElement.setText("false");
							}
						}else if(Pattern.matches(regexFieldDictionary.get(dlFieldName), dlFieldValue))
						{
							Element forceReviewElement = documentLevelFieldElement.getChild("ForceReview");
							if (null != forceReviewElement)
							{
								forceReviewElement.setText("false");
							}
						}else
						{
							Element forceReviewElement = documentLevelFieldElement.getChild("ForceReview");
							if (null != forceReviewElement)
							{
								forceReviewElement.setText("true");
							}
						}
					}else
					{
						Element forceReviewElement = documentLevelFieldElement.getChild("ForceReview");
						if (null != forceReviewElement)
						{
							forceReviewElement.setText("false");
						}
					}
				}	
			}
			log("Changed All ForceReview to false.");
		}	
		
		Boolean isInvalidDocument=isForceReviewElementsInvalid(documentLevelFieldsElement);
		
		if(!isInvalidDocument) 
		{
			Element validElement = documentElement.getChild("Valid");
			if (null != validElement) {
				validElement.setText("true");
			}
			log("Changed the value of Valid tag to true.");
			Element reviewElement = documentElement.getChild("Reviewed");
			if (null != reviewElement) {
				reviewElement.setText("true");
			}
			
			log("Changed the value of Reviewed tag to true.");
		}
		this.documentUtil.writeToXML(this.document);
		log("completed the execution of markAllFieldsAsValid()...");
		
	}
	
	public void markAllDocsAsValid(String docIdentifier){
		log("Inside markAllDocsAsValid...for batch id: " + docIdentifier);
		
		
		List<Element> documentList = this.documentUtil.getDocumentElements();
		for(Element documentElement : documentList) 
		{
				Element documentLevelFieldsElement = documentElement.getChild("DocumentLevelFields");
				
				if(null != documentLevelFieldsElement) {
					List<?> documentLevelFieldList = documentLevelFieldsElement.getChildren("DocumentLevelField");
					
					for(int i=0; i<documentLevelFieldList.size(); i++) 
					{
						Element documentLevelFieldElement = (Element)documentLevelFieldList.get(i);
						Element forceReviewElement = documentLevelFieldElement.getChild("ForceReview");
						if (null != forceReviewElement)
						{
							forceReviewElement.setText("false");
						}
					}
					//log("Changed All ForceReview to false...for batch id: " + docIdentifier);
				}	
				
				
				//documentElement.getChild("Valid").setText("true");
				Element validElement = documentElement.getChild("Valid");
				if (validElement != null) {
					validElement.setText("true");
				}
				//log("Changed the value of Valid tag to true.");
				
				//documentElement.getChild("Reviewed").setText("true");
				Element reviewElement = documentElement.getChild("Reviewed");
				if (reviewElement != null) {
					reviewElement.setText("true");
				}
				
				//log("Changed the value of Reviewed tag to true.");
				
				// Write changes back to batch xml
				this.documentUtil.writeToXML(this.document);
		}
	}
	
	
	
	
	/**
	 * Gets the regexpattern details for validation as dictionary.
	 * Changing all ForceReview value of current documentfields to false if the regex pattern matches the fieldvalue
	 * Changing Valid and Reviewed tag value of current document to true if all the forceReviewElement value  of the current documentLevel fields are false
	 * Make document valid if no documentLevelFields element found.
	 * @param docIdentifier : Current  Document Identifier
	 **/
	public void markAllFieldsAsValid(String docIdentifier){
		// Get current document element
		log("Inside markAllFieldsAsValid...for batch id: " + docIdentifier);
				Element documentElement = this.documentUtil.getDocumentElement(docIdentifier);

				Element documentLevelFieldsElement = documentElement.getChild("DocumentLevelFields");
				
				if(null != documentLevelFieldsElement) {
					List<?> documentLevelFieldList = documentLevelFieldsElement.getChildren("DocumentLevelField");
					
					for(int i=0; i<documentLevelFieldList.size(); i++) 
					{
						Element documentLevelFieldElement = (Element)documentLevelFieldList.get(i);
						Element forceReviewElement = documentLevelFieldElement.getChild("ForceReview");
						if (null != forceReviewElement)
						{
							forceReviewElement.setText("false");
							//String elementValue = forceReviewElement.getText();
							//if(elementValue.equals("true"))
							//{
							//	forceReviewElement.setText("false");
							//}
						}						
		
					}
					//log("Changed All ForceReview to false.");
				}	
				
				
				//documentElement.getChild("Valid").setText("true");
				Element validElement = documentElement.getChild("Valid");
				if (validElement != null) {
					validElement.setText("true");
				}
				//log("Changed the value of Valid tag to true.");
				
				//documentElement.getChild("Reviewed").setText("true");
				Element reviewElement = documentElement.getChild("Reviewed");
				if (reviewElement != null) {
					reviewElement.setText("true");
				}
				
				//log("Changed the value of Reviewed tag to true.");
				
				// Write changes back to batch xml
				this.documentUtil.writeToXML(this.document);
				//log("completed the execution of markAllFieldsAsValid()...");
	}
	
	/**
	 * Clear CoBorrower Fields if available in current document
	 * 
	 * @param docIdentifier
	 * @return
	 */
	public void clearCoBorrowerFields(String docIdentifier)
	{
		log("Inside clearCoBorrowerFields()...");
		this.clearCoBorrowerFieldsFromCurrentDoc(docIdentifier, COBORROWER_FIELDS);
		
		// Write changes back to batch xml
		this.documentUtil.writeToXML(this.document);
		log("completed the execution of clearCoBorrowerFields()...");
	}
	
	/**
	 * Copy the list of fields from the Current document 
	 * @param docIdentifier
	 * @param fieldList contains the list of fields to be copied from Current document
	 * @return
	 */
	public void clearCoBorrowerFieldsFromCurrentDoc(String docIdentifier,List<String> fieldList){
		log("Inside clearCoBorrowerFieldsFromCurrentDoc()...");
		//Map<String, String> dlFieldsDictionary = new HashMap<String, String>();
	
		// Get Configured Fields from current document element and prepare a FieldsDictionary
		Element currentDocumenElement = this.documentUtil.getDocumentElement(docIdentifier);
		if (currentDocumenElement !=null) {
			log("Current document level field details...");
			for (String field : fieldList) {
				Element fieldElement = this.documentUtil.getDocumentLevelField(currentDocumenElement,field);
				if (fieldElement !=null) {
					this.documentUtil.setFieldValue(fieldElement,"");
				}
			}
		}
		log("completed the execution of copyFieldsFromCurrentDoc()...");
	}
	
	private Boolean isForceReviewElementsInvalid(Element documentLevelFieldsElement)
	{
		Boolean isInvalidDocument=false;
		if(null != documentLevelFieldsElement) 
		{
			List<?> documentLevelFieldList = documentLevelFieldsElement.getChildren("DocumentLevelField");
			for(int i=0; i<documentLevelFieldList.size(); i++) 
			{
				Element documentLevelFieldElement = (Element)documentLevelFieldList.get(i);
				if(null != documentLevelFieldElement)
				{
					Element forceReviewElement = documentLevelFieldElement.getChild("ForceReview");
					if (null != forceReviewElement)
					{
						String forceReviewValue=forceReviewElement.getText();
						if("true".equalsIgnoreCase(forceReviewValue))
						{
							isInvalidDocument=true;
							break;
							
						}
					}
				}
			}
			
		}
		return isInvalidDocument;
	}
	
	/**
	 * This method retrieves the available regex pattern list from the ephesoft DB and creates a dictionary contains fieldname as key and regex_pattern as value
	 * returns regexpattern details for validation as dictionary.
	 * @param batchIdentifier :batchclass identifier
	 * @param DoctypeName : DocumentTypeName
	 * */
	public Map<String, String> getRegexValidationDetails(String batchIdentifier, String DoctypeName) {
		Map<String, String> regexFieldDictionary = new HashMap<String, String>();
		if (!StringUtils.isEmpty(batchIdentifier) && !StringUtils.isEmpty(DoctypeName)) {
			Connection connection = null;
			Statement statement = null;

			try {
				connection = this.getEphesoftConnection(SQL_AUTH.SQL);
				statement = connection.createStatement();
				String sql="select f.field_type_name, r.pattern, r.field_type_id from regex_validation r inner join field_type f on f.id = r.field_type_id inner join document_type d on d.id = f.document_type_id inner join batch_class_document_type bd on  d.id = bd.document_type_id inner join batch_class b on bd.batch_class_id = b.id where b.identifier ='" 
							+ batchIdentifier + "' and d.document_type_name ='" + DoctypeName + "' ";
				log("QUERY - " + sql);
				//System.out.println("QUERY - " + sql);

				ResultSet rs= statement.executeQuery(sql);
	
				while (rs.next()) {
					String fieldName = rs.getString("field_type_name");
					String pattern = rs.getString("pattern");
					log(fieldName+" of pattern "+pattern);
					//System.out.println(fieldName+" of pattern "+pattern);
					if(!regexFieldDictionary.containsKey(fieldName))
					{
						regexFieldDictionary.put(fieldName, pattern);
					}
	
				}
				log("regexPatternDictionary created successfully...");
				
				
		
				
			} catch (Exception exception) {

				try {
					statement.close();
					connection.close();
				} catch (SQLException e) {
					log(e.getLocalizedMessage());
				}
				log(exception.getLocalizedMessage());
				exception.printStackTrace();
			} finally {
				try {
					if (null != statement)
						connection.close();
				} catch (SQLException se) {
				}// do nothing
				try {
					if (null != connection)
						connection.close();
				} catch (SQLException se) {
					se.printStackTrace();
				}// end finally try
			}// end try
			
		}
		return regexFieldDictionary;
	}
	
	/**
	 * Get SQL connection to the ephesoft database
	 * 
	 * @param sqlAuth
	 * @return
	 */
	private Connection getEphesoftConnection(SQL_AUTH sqlAuth) {
		Connection connection = null;
		// the sql server driver string
		try {
			log("DB_CLASS - " + DB_CLASS);
			System.out.println("DB_CLASS - " + DB_CLASS);
			Class.forName(DB_CLASS);
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		}
		StringBuilder builder = new StringBuilder(DB_DRIVER + "://");
		builder.append(DB_HOST);
		builder.append(":" + DB_PORT);
		builder.append("/"+DB_NAME+";");
		if (sqlAuth == SQL_AUTH.WINDOWS) {
			builder.append(";useNTLMv2=true;");
			builder.append("domain=" + DB_DOMAIN + ";");
		} else if (sqlAuth == SQL_AUTH.SQL) {
		}
		if (null != DB_INSTANCE) {
			builder.append("instance=" + DB_INSTANCE + ";");
		}

		DB_URL = builder.toString();
		log("DB_URL: " + DB_URL);
		try {
			connection = DriverManager.getConnection(DB_URL, DB_USER,
						DB_PASSWORD);
		} catch (SQLException e) {
			log(e.getLocalizedMessage());
			e.printStackTrace();
		}
		return connection;
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
