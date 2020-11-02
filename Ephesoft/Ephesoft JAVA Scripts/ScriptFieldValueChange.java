import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.OutputStream;
import java.util.List;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import org.jdom.Document;
import org.jdom.Element;
import org.jdom.output.XMLOutputter;
import com.ephesoft.dcma.util.logger.EphesoftLogger;
import com.ephesoft.dcma.util.logger.ScriptLoggerFactory;
import com.ephesoft.dcma.core.component.ICommonConstants;
import com.ephesoft.dcma.util.ApplicationConfigProperties;

import com.ephesoft.dcma.script.IJDomScript;

public class ScriptFieldValueChange implements IJDomScript {

	private static EphesoftLogger LOGGER = ScriptLoggerFactory.getLogger(ScriptFieldValueChange.class);
	private static String BATCH_LOCAL_PATH = "BatchLocalPath";
	private static String BATCH_INSTANCE_ID = "BatchInstanceIdentifier";
	private static String ZIP_FILE_EXT = ".zip";
	private static String EXT_BATCH_XML_FILE = "_batch.xml";

	public Object execute(Document document, String fieldName, String docIdentifier) {
		Exception exception = null;
		try {
			changeField(document, fieldName, docIdentifier);
		} catch (Exception e) {
			LOGGER.error("*************  Error occurred in scripts." + e.getMessage());
			exception = e;
		}
		return exception;
	}

	public void changeField(Document document, String fieldName, String docIdentifier) {
		LOGGER.info("*************  Inside field value change script.");

		LOGGER.info("*************  Start execution of field value change  script.");

		if (null == document) {
			LOGGER.error("Input document is null.");
		}
		writeToXML(document);
		LOGGER.info("*************  End execution of the Field Value Change script.");
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

	public static OutputStream getOutputStreamFromZip(final String zipName, final String fileName) throws FileNotFoundException,
			IOException {
		ZipOutputStream stream = null;
		stream = new ZipOutputStream(new FileOutputStream(new File(zipName + ZIP_FILE_EXT)));
		ZipEntry zipEntry = new ZipEntry(fileName);
		stream.putNextEntry(zipEntry);
		return stream;
	}

}
