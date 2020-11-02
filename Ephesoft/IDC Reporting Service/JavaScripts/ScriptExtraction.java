import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.OutputStream;
import java.util.List;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;
import org.json.simple.JSONObject;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.jdom.Document;
import org.jdom.Element;
import org.jdom.output.XMLOutputter;

import com.ephesoft.dcma.script.IJDomScript;
import com.ephesoft.dcma.util.logger.EphesoftLogger;
import com.ephesoft.dcma.util.logger.ScriptLoggerFactory;
import com.ephesoft.dcma.core.component.ICommonConstants;
import com.ephesoft.dcma.util.ApplicationConfigProperties;
import com.mts.idc.ephesoft.SQL_AUTH;
import com.mts.idc.ephesoft.service.EphesoftUtilityRequest;
import com.mts.idc.ephesoft.service.EphesoftModule;
import com.mts.idc.ephesoft.service.EphesoftUtilityWSClient;
import com.mts.idc.ephesoft.service.JsonResponseWSClient;
import com.mts.idc.ephesoft.service.QCIQLookupRequest;
import com.mts.idc.ephesoft.BaseScript;
import com.mts.idc.ephesoft.LogLevel;
import com.mts.idc.ephesoft.LogUtil;
import com.google.gson.Gson;


import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.io.FileInputStream;
import java.io.FilenameFilter;
import java.io.InputStream;
import org.fife.io.*; 

/**
 * The <code>ScriptExtraction</code> class represents the ScriptExtraction structure. Writer of scripts plug-in should implement this
 * IScript interface to execute it from the scripting plug-in. Via implementing this interface writer can change its java file at run
 * time. Before the actual call of the java Scripting plug-in will compile the java and run the new class file.
 * 
 * @author Ephesoft
 * @version 1.0
 */
public class ScriptExtraction extends BaseScript {
	private LogUtil logUtil;
	private LogLevel logLevel = LogLevel.INFO;
	private Logger logger = LoggerFactory.getLogger(ScriptExtraction.class);

	//private static EphesoftLogger LOGGER = ScriptLoggerFactory.getLogger(ScriptExtraction.class);
	private static String BATCH_LOCAL_PATH = "BatchLocalPath";
	private static String BATCH_INSTANCE_ID = "BatchInstanceIdentifier";
	private static String EXT_BATCH_XML_FILE = "_batch.xml";
	private static String ZIP_FILE_EXT = ".zip";
	private static final String targetReportFolder =  "D:\\Ephesoft\\SharedFolders\\mts-report-data\\";
	private static final String sourceFolder =  "D:\\Ephesoft\\SharedFolders\\ephesoft-system-folder\\";
	
	public Object execute(Document documentFile, String methodName, String documentIdentifier) {
		Exception exception = null;
		try {
			if (null == documentFile) {
				return new Exception("Input document is null");
			}
			// Initialize
			this.initialize(documentFile);
			
			String REST_SERVICE_URL = "http://34.224.49.101:8010/EphesoftUtilityAPI/Document/UpdateReviewedDate";			
			XMLOutputter xmlOut = new XMLOutputter();
			QCIQLookupRequest requestLoanJson=new QCIQLookupRequest();
			requestLoanJson.inputXML=xmlOut.outputString(documentFile);
			JSONObject responseJson=null;
			JsonResponseWSClient loanServiceClient=new JsonResponseWSClient();
			responseJson=loanServiceClient.invokeJsonResponseWS(requestLoanJson, REST_SERVICE_URL);				
			log("ScriptExtraction web service call made.");
			if(responseJson != null) {
				log("ScriptExtraction Response Not null.");	
				Gson _gson = new Gson();
				log("ScriptExtraction Result = " + _gson.toJson(responseJson));				
				//String success=(String) responseJson.get("Success");
				//log("ScriptExtraction web service call successful. Response = " + success);
			} else{
				log("ScriptExtraction Response NULL.");
			}

				String batchIdentifier=this.documentUtil.getBatchInstanceIdentifier();
				
				
				String targetFolder= targetReportFolder + batchIdentifier;
				File directory = new File(targetFolder);
				
				if (!directory.exists())
				{
					directory.mkdir();
				}
				
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
					 if(!fName.equals(tName)){
						  File fTarget = new File(new File(targetFolder), fSource.getName());
						  copyFileUsingStream(fSource,fTarget);
					 }
				  // fSource.delete(); // Uncomment this line if you want source file deleted
				}			
		
		} catch (Exception e) {
			System.out.println("Error occurred - " + e.getMessage());
			e.printStackTrace();
			exception = e;
		}
		return exception;
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
