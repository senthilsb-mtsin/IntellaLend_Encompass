import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.OutputStream;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.List;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;
import org.apache.axis.utils.StringUtils;
import org.apache.http.client.ClientProtocolException;
import org.jdom.Document;
import org.jdom.Element;
import org.jdom.JDOMException;
import org.jdom.output.XMLOutputter;
import org.json.simple.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.ephesoft.dcma.core.component.ICommonConstants;
import com.ephesoft.dcma.script.IJDomScript;
import com.ephesoft.dcma.util.ApplicationConfigProperties;
import com.ephesoft.dcma.util.logger.EphesoftLogger;
import com.ephesoft.dcma.util.logger.ScriptLoggerFactory;
import com.mts.idc.ephesoft.BaseScript;
import com.mts.idc.ephesoft.LogLevel;
import com.mts.idc.ephesoft.LogUtil;
import com.mts.idc.ephesoft.SQL_AUTH;
import com.mts.idc.ephesoft.service.EphesoftUtilityRequest;
import com.mts.idc.ephesoft.service.EphesoftModule;
import com.mts.idc.ephesoft.service.EphesoftUtilityWSClient;
import com.mts.idc.ephesoft.service.JsonResponseWSClient;
import com.mts.idc.ephesoft.service.QCIQLookupRequest;

/**
 * The <code>ScriptDocumentAssembler</code> class represents the script execute structure. Writer of scripts plug-in should implement
 * this IScript interface to execute it from the scripting plug-in. Via implementing this interface writer can change its java file at
 * run time. Before the actual call of the java Scripting plug-in will compile the java and run the new class file.
 * 
 * @author Ephesoft
 * @version 1.0
 */
public class ScriptDocumentAssembler extends BaseScript {
	
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
	private static String VALUE = "Value";
	private static String BATCH_INSTANCE_PRIORITY = "50";
	private LogUtil logUtil;
	private LogLevel logLevel = LogLevel.INFO;
	private Logger logger = LoggerFactory.getLogger(ScriptDocumentAssembler.class);

	//private static EphesoftLogger LOGGER = ScriptLoggerFactory.getLogger(ScriptDocumentAssembler.class);
	private static String BATCH_LOCAL_PATH = "BatchLocalPath";
	private static String BATCH_INSTANCE_ID = "BatchInstanceIdentifier";
	private static String EXT_BATCH_XML_FILE = "_batch.xml";
	private static String ZIP_FILE_EXT = ".zip";

	/**
	 * The <code>execute</code> method will execute the script written by the writer at run time with new compilation of java file. It
	 * will execute the java file dynamically after new compilation.
	 * 
	 * @param document {@link Document}
	 */
	public Object execute(Document documentFile, String methodName, String documentIdentifier) {
	Exception exception = null;
		try {
		if (null == documentFile) {
			return new Exception("Input document is null");
		}
		
		// Initialize
		this.initialize(documentFile);
		// MTS AWS Env		
		String REST_SERVICE_URL = "http://10.0.2.229/RevampEphesoftUtilityAPI/Document/Execute";
		
		String REST_LOANSERVICE_URL="http://10.0.2.229/RevampEphesoftUtilityAPI/IntellaLendWrapper/GetLoanDetails";
		
		String REST_CHECKLOANPAGECOUNT_URL = "http://10.0.2.229/RevampEphesoftUtilityAPI/IntellaLendWrapper/CheckLoanPageCount";
		
		String MAS_DOCUMENT_INGESTION_URL = "http://10.0.2.229/RevampEphesoftUtilityAPI/IntellaLendWrapper/UpdateLOSExportFileStaging";

		EphesoftUtilityRequest requestContent = new EphesoftUtilityRequest();
		requestContent.orderOfExecution = "append|concatenate|convert|advmerge|pagesequence";
		requestContent.ephesoftModule = EphesoftModule.DOCUMENTASSEMBLY.ordinal();
		XMLOutputter xmlOut = new XMLOutputter();
		requestContent.inputXML = xmlOut.outputString(documentFile);
		//Here QCIQLookupRequest is used for loanDetails Request since both uses only xml as input
		QCIQLookupRequest requestLoanJson=new QCIQLookupRequest();
		requestLoanJson.inputXML=xmlOut.outputString(documentFile);
			JSONObject responseJson=null;
		log("Checking Loan Page Count");
		JsonResponseWSClient loanServiceClient=new JsonResponseWSClient();
		responseJson=loanServiceClient.invokeJsonResponseWS(requestLoanJson, REST_CHECKLOANPAGECOUNT_URL);
		if(responseJson != null)
		{
			//log(responseJson);
			boolean pageCountEq = Boolean.parseBoolean(responseJson.get("Equals").toString());
			//String pageCountEq = new String();
			//pageCountEq = responseJson.get("Equals").toString();
			//log("Response : " + pageCountEq.toString());
			if(pageCountEq == false){
				new Exception("Page Count not equals with Loan package PDF"); 
			}
			
			log("Page Count Equals PDF Page Count");
		}
			
			
		responseJson=null;
		//Request and response block starts
			Document responseDoc = null;
		
				EphesoftUtilityWSClient client = new EphesoftUtilityWSClient();
				responseDoc = client.invokeEphesoftUtilityWS(requestContent, REST_SERVICE_URL);
				log("Ephesoft Utility web service call successful.");
			 //} catch (Exception e) {

			if (responseDoc != null) {
				documentFile.detachRootElement();
				if(!documentFile.hasRootElement())
				{
					documentFile.setRootElement(responseDoc.detachRootElement());
				}
			} else {
				// TODO 
				log("ResponseDoc is Null.");
			}
			log("End execution of execute() method from "+ this.getClass().getName());
			//Request and response block ends	
			
			responseJson=loanServiceClient.invokeJsonResponseWS(requestLoanJson, REST_LOANSERVICE_URL);
			log("Ephesoft Loan web service call successful.");
			setCustomBatchFields(document,responseJson);
			
			
			
			log("End execution of execute() method from "
					+ this.getClass().getName());
					
			MASDocumentIngestion(documentFile);
		}
		catch (Exception e) {
			System.out.println("Error occurred - " + e.getMessage());
			e.printStackTrace();
			exception = e;
		}
		return exception;
	}
	
	private void MASDocumentIngestion(Document document){
		try {
		EphesoftUtilityRequest requestContent = new EphesoftUtilityRequest();
		requestContent.ephesoftModule = EphesoftModule.DOCUMENTASSEMBLY.ordinal();
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
	
	private void setCustomBatchFields(Document document,JSONObject loanDetails) {
		if(loanDetails != null){
			String customer = new String();
			String serviceType = new String();
			String auditMonthYear = new String();
			String priority  = new String();
			String batchName = this.documentUtil.getBatchName();

			customer=(String) loanDetails.get("Customer");
			serviceType=(String) loanDetails.get("ServiceType");
			auditMonthYear=(String) loanDetails.get("AuditMonthYear");
			priority=  loanDetails.get("Priority").toString();
			
			log("Customer: "+customer+" "+"ServiceType: "+serviceType+" "+"AuditMonthYear: "+auditMonthYear+" "+"Priority: "+priority);
			System.out.println("Customer: "+customer+" "+"ServiceType: "+serviceType+" "+"AuditMonthYear: "+auditMonthYear+" "+"Priority: "+priority);

			updateCustomValues("custom_column1", customer, batchName);
			updateCustomValues("custom_column2", serviceType, batchName);
			updateCustomValues("custom_column3", auditMonthYear, batchName);
			updateCustomValues("custom_column4", priority, batchName);
			
			if(priority != ""){		
				int reviewPriorityValue = 0;
				int newPriorityValue = 50;
				try{
					reviewPriorityValue = Integer.parseInt(priority);
					if(reviewPriorityValue > 0 && reviewPriorityValue < 5){
						newPriorityValue =  reviewPriorityValue * 10;						
					}
					BATCH_INSTANCE_PRIORITY = Integer.toString(newPriorityValue);
					updateCustomValues("batch_priority", BATCH_INSTANCE_PRIORITY, batchName);
				}catch (Exception e) {
					log("Error occurred - " + e.getMessage());
				}				
			}		
		}
	}
	
	/**
	 * This method updates a given column in the batch_instance table with the
	 * input value.
	 * */
	public void updateCustomValues(String columnName, String value,
			String batchName) {

		if (!StringUtils.isEmpty(columnName) && !StringUtils.isEmpty(value)
				&& !StringUtils.isEmpty(batchName)) {
			Connection connection = null;
			Statement statement = null;
			batchName = batchName.replace("'", "''");

			try {
				connection = this.getEphesoftConnection(SQL_AUTH.SQL);
				statement = connection.createStatement();
				String sql = "UPDATE batch_instance SET " + columnName + "= '"
						+ value + "' WHERE batch_name='" + batchName + "'";
				log("QUERY - " + sql);
				System.out.println("QUERY - " + sql);

				statement.executeUpdate(sql);
				log("Records succssfully updated into the table...");
				System.out.println("Records succssfully updated into the table...");
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
					if (statement != null)
						connection.close();
				} catch (SQLException se) {
				}// do nothing
				try {
					if (connection != null)
						connection.close();
				} catch (SQLException se) {
					se.printStackTrace();
				}// end finally try
			}// end try
		}
	}
	
	public String getFieldValue(Element doc, String fieldName) {
		Element documentLevelFieldElement = this.documentUtil.getDocumentLevelField(doc, fieldName);
		if (null != documentLevelFieldElement) {
			Element valueElement = documentLevelFieldElement.getChild(VALUE);
			if (valueElement != null) {
				return valueElement.getText();
			}
		}
		return null;
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

		// the sql server url
		StringBuilder builder = new StringBuilder(DB_DRIVER + "://");
		builder.append(DB_HOST);
		builder.append(":" + DB_PORT);
		//builder.append("/Ephesoft_DEV;"); //Ephisoft_ManiDev
		builder.append("/"+DB_NAME+";");
		
		if (sqlAuth == SQL_AUTH.WINDOWS) {
			builder.append(";useNTLMv2=true;");
			builder.append("domain=" + DB_DOMAIN + ";");
		} else if (sqlAuth == SQL_AUTH.SQL) {
			//builder.append("user=" + DB_USER + ";");
			//builder.append("password=" + DB_PASSWORD + ";");
		}

		if (null != DB_INSTANCE) {
			builder.append("instance=" + DB_INSTANCE + ";");
		}

		DB_URL = builder.toString();
		log("DB_URL: " + DB_URL);
		System.out.println("DB_URL: " + DB_URL);

		try {
			// get the sql server database connection
			connection = DriverManager.getConnection(DB_URL, DB_USER,
						DB_PASSWORD);
			//if (sqlAuth == SQL_AUTH.WINDOWS) {
			//	connection = DriverManager.getConnection(DB_URL, DB_USER,
			//			DB_PASSWORD);
			//} else if (sqlAuth == SQL_AUTH.SQL) {
			//	connection = DriverManager.getConnection(DB_URL);
			//}
		} catch (SQLException e) {
			log(e.getLocalizedMessage());
			e.printStackTrace();
		}
		return connection;
	}
	

	/**
	 * The <code>writeToXML</code> method will write the state document to the XML file.
	 * 
	 * @param document {@link Document}.
	 */
	private void writeToXML(Document document) {
		String batchLocalPath = null;
		List batchLocalPathList = document.getRootElement().getChildren(BATCH_LOCAL_PATH);
		if (null != batchLocalPathList) {
			batchLocalPath = ((Element) batchLocalPathList.get(0)).getText();
		}

		if (null == batchLocalPath) {
			log("Unable to find the local folder path in batch xml file.");
			return;
		}

		String batchInstanceID = null;
		List batchInstanceIDList = document.getRootElement().getChildren(BATCH_INSTANCE_ID);
		if (null != batchInstanceIDList) {
			batchInstanceID = ((Element) batchInstanceIDList.get(0)).getText();

		}

		if (null == batchInstanceID) {
			log("Unable to find the batch instance ID in batch xml file.");
			return;
		}

		String batchXMLPath = batchLocalPath.trim() + File.separator + batchInstanceID + File.separator + batchInstanceID
				+ EXT_BATCH_XML_FILE;
		boolean isZipSwitchOn = true;
		try {
			ApplicationConfigProperties prop = ApplicationConfigProperties.getApplicationConfigProperties();
			isZipSwitchOn = Boolean.parseBoolean(prop.getProperty(ICommonConstants.ZIP_SWITCH));
		} catch (IOException ioe) {
			log("Unable to read the zip switch value. Taking default value as true. Exception thrown is:" + ioe.getMessage()+ioe);
		}

		log("isZipSwitchOn************" + isZipSwitchOn);
		OutputStream outputStream = null;
		FileWriter writer = null;
		XMLOutputter out = new com.ephesoft.dcma.batch.encryption.util.BatchInstanceXmlOutputter(batchInstanceID);


		try {
			if (isZipSwitchOn) {
				log("Found the batch xml zip file.");
				outputStream = getOutputStreamFromZip(batchXMLPath, batchInstanceID + EXT_BATCH_XML_FILE);
				out.output(document, outputStream);
			} else {
				writer = new java.io.FileWriter(batchXMLPath);
				out.output(document, writer);
				writer.flush();
				writer.close();

			}
		} catch (Exception e) {
			log(e.getMessage());
		} finally {
			if (outputStream != null) {
				try {
					outputStream.close();
				} catch (IOException e) {
				}
			}
		}
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
