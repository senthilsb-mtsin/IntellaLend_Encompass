package com.mts.idc.ephesoft;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.OutputStream;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import org.jdom.Document;
import org.jdom.Element;
import org.jdom.JDOMException;
import org.jdom.output.XMLOutputter;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;


/**
 * Date Modified: 06/29/2015
 * @author Atit Patel  
 */
public class DocumentUtil {
	private static final String ZIP_FILE_EXT = ".zip";	
	private static final String BATCH_CLASS_IDENTIFIER = "BatchClassIdentifier";
	private static final String BATCH_CREATION_DATE = "BatchCreationDate";
	private static final String BATCH_LOCAL_PATH = "BatchLocalPath";
	private static final String BATCH_INSTANCE_ID = "BatchInstanceIdentifier";
	private static final String BATCH_NAME = "BatchName";
	private static final String EXT_BATCH_XML_FILE = "_batch.xml";
	private static final String DOCUMENTS = "Documents";
	private static final String DOCUMENT = "Document";
	private static final String IDENTIFIER = "Identifier";
	private static final String DOCUMENTLEVELFIELDS = "DocumentLevelFields";
	private static final String DOCUMENTLEVELFIELD = "DocumentLevelField";
	private static final String COORDINATESLIST = "CoordinatesList";
	private static final String COORDINATES = "Coordinates";
	private static final String NAME = "Name";
	private static final String VALUE = "Value";
	private static final String PAGES = "Pages";
	private static final String PAGE = "Page";
	private static final String TYPE = "Type";
	private static final String FIELDVALUEOPTIONLIST = "FieldValueOptionList";
	
	private static final String DATE_FORMAT = "MM/dd/yyyy";
	private static final String SCRIPT_CONFIG = "script-config";
	
	private Logger logger = LoggerFactory.getLogger(DocumentUtil.class);
		
	private Document document;
	private LogUtil logUtil;
	
	/**
	 * @param document
	 * @param logLevel
	 */
	public DocumentUtil(Document document, LogLevel logLevel) {
		this.document = document;
		this.logUtil = new LogUtil(logLevel, logger);
	}

	/**
	 * Get list of document elements
	 * @return
	 */
	public List<Element> getDocumentElements() {
		List<Element> documentElements = new ArrayList<Element>();
		
		//Get Batch element
		Element batchElement = this.document.getRootElement();
		
		//Get Documents element
		Element documentsElement = batchElement.getChild(DOCUMENTS);
		
		//Get list of Document elements
		List<?> documentList = documentsElement.getChildren(DOCUMENT);
		
		//Iterate through every document 
		for(int i=0; i<documentList.size(); i++) {
			Element documentElement = (Element)documentList.get(i);
			documentElements.add(documentElement);
		}
		return documentElements;
	}
	
	/**
	 * @param documentId
	 * @return
	 */
	public Element getDocumentElement(String documentId) {
		List<Element> documentElements = this.getDocumentElements();
		
		for(Element documentElement : documentElements) {
			Element identifierElement = documentElement.getChild(IDENTIFIER);
			String identifier = identifierElement.getText();
			
			if(identifier.equals(documentId))
				return documentElement;
		}
		
		return null;
	}
	
	/**
	 * @param docType
	 * @return
	 */
	public Element getDocumentElementByType(String docType) {
		List<Element> documentElements = this.getDocumentElements();
		
		for(Element documentElement : documentElements) {
			Element typeElement = documentElement.getChild(TYPE);
			String type = typeElement.getText();
			
			if(type.equals(docType))
				return documentElement;
		}
		
		return null;
	}
	
	/**
	 * @param documentElement
	 * @return
	 */
	public List<Element> getDocumentLevelFieldElements(Element documentElement) {
		List<Element> documentLevelFieldElements = null;
		
		//Get DocumentLevelFields element
		Element documentLevelFieldsElement = documentElement.getChild(DOCUMENTLEVELFIELDS);
		
		if(null != documentLevelFieldsElement) {
			documentLevelFieldElements = new ArrayList<Element>();
			@SuppressWarnings("unchecked")
			List<Element> documentLevelFieldList = documentLevelFieldsElement.getChildren(DOCUMENTLEVELFIELD);
			
			for(Element documentLevelField : documentLevelFieldList) {
				documentLevelFieldElements.add(documentLevelField);
			}
		}
		
		return documentLevelFieldElements;
	}
	
	/**
	 * Gets the document level field element from the document
	 * @param documentElement
	 * @param fieldName
	 * @return
	 */
	public Element getDocumentLevelField(Element documentElement, String fieldName) {

		//Get DocumentLevelFields element
		Element documentLevelFieldsElement = documentElement.getChild(DOCUMENTLEVELFIELDS);
		
		if(null != documentLevelFieldsElement) {
			List<?> documentLevelFieldList = documentLevelFieldsElement.getChildren(DOCUMENTLEVELFIELD);
			
			for(int i=0; i<documentLevelFieldList.size(); i++) {
				Element documentLevelFieldElement = (Element)documentLevelFieldList.get(i);
				Element nameElement = documentLevelFieldElement.getChild(NAME);
				String elementName = nameElement.getText();
				
				if(elementName.equals(fieldName)) {
					return documentLevelFieldElement;
				}
			}
		}		
		return null;
	}
	
	/**
	 * @param documentElement
	 * @return
	 */
	public String getDocType(Element documentElement) {
		Element typeElement = documentElement.getChild(TYPE);
		return typeElement.getText();
	}

	/**
	 * @param documentElement
	 * @return
	 */
	public String getDocIdentifier(Element documentElement) {
		Element typeElement = documentElement.getChild(IDENTIFIER);
		return typeElement.getText();
	}
	/**
	 * Get batch creation date
	 * @return
	 */
	public String getBatchCreationDate() {
		String batchCreationDate = null;
		
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS");
		
		try {
			
			//Get batch creation date from document
			Element batchElement = this.document.getRootElement();
			Element batchCreationDateElement = batchElement.getChild(BATCH_CREATION_DATE);
			batchCreationDate = batchCreationDateElement.getText();
			
			//Format batch creation date
			Date tmp = sdf.parse(batchCreationDate);
			SimpleDateFormat myDateFormat = new SimpleDateFormat(DATE_FORMAT);
			batchCreationDate = myDateFormat.format(tmp);
			
		} catch (ParseException e) {
			e.printStackTrace();
		}
		
		return batchCreationDate;
	}
	
	/**
	 * Get batch local path
	 * @return
	 */
	public String getBatchLocalPath() {
		//Get batch local path
		Element batchElement = this.document.getRootElement();
		Element batchLocalPathElement = batchElement.getChild(BATCH_LOCAL_PATH);
		return batchLocalPathElement.getText();
	}
	
	/**
	 * Get batch class identifier
	 * @return
	 */
	public String getBatchClassIdentifier() {
		//Get batch class identifier
		Element batchElement = this.document.getRootElement();
		Element batchClassIdentifierElement = batchElement.getChild(BATCH_CLASS_IDENTIFIER);
		return batchClassIdentifierElement.getText();
	}
	
	/**
	 * Get batch instance identifier
	 * @return
	 */
	public String getBatchInstanceIdentifier() {
		//Get batch instance identifier
		Element batchElement = this.document.getRootElement();
		Element batchInstanceIdentifierElement = batchElement.getChild(BATCH_INSTANCE_ID);
		return batchInstanceIdentifierElement.getText();
	}
	
	/**
	 * Get batch Name
	 * @return
	 */
	public String getBatchName() {
		//Get batch instance identifier
		Element batchElement = this.document.getRootElement();
		Element batchNameElement = batchElement.getChild(BATCH_NAME);
		return batchNameElement.getText();
	}
	
	/**
	 * Get script config path
	 * @return
	 */
	public String getScriptConfigPath() {
		String batchLocalPath = this.getBatchLocalPath();
		String batchCalssIdentifier = this.getBatchClassIdentifier();
		
		StringBuilder builder = new StringBuilder();
		builder.append(batchLocalPath.substring(0, batchLocalPath.indexOf("ephesoft-system-folder")));
		builder.append(batchCalssIdentifier);
		builder.append(File.separator);
		builder.append(SCRIPT_CONFIG);
		
		return builder.toString();
	}
	
	/**
	 * Gets the document level field value
	 * @param documentLevelFieldElement
	 * @return
	 */
	public String getFieldValue(Element documentLevelFieldElement) {
		if(null != documentLevelFieldElement) {
			Element valueElement = documentLevelFieldElement.getChild(VALUE);
			return valueElement.getText();
		}
		return null;
	}
	
	/**
	 * Sets the document level field value option list
	 * @param documentLevelFieldElement
	 * @param valueoptionlist
	 */
	public void setFieldValueOptionList(Element documentLevelFieldElement, String valueoptionlist) {
		if(null != documentLevelFieldElement) {
			Element valueElement = documentLevelFieldElement.getChild(FIELDVALUEOPTIONLIST);
			if (valueElement == null)
			{
				valueElement = new Element(FIELDVALUEOPTIONLIST);
				documentLevelFieldElement.addContent(valueElement);
			}
			valueElement.setText(valueoptionlist);
		}
	}

	/**
	 * Gets the document level field value option list
	 * @param documentLevelFieldElement
	 * @param valueoptionlist
	 */
	public String getFieldValueOptionList(Element documentLevelFieldElement) {
		if(null != documentLevelFieldElement) {
			Element valueElement = documentLevelFieldElement.getChild(FIELDVALUEOPTIONLIST);
			if (valueElement == null)
				return "";
			return valueElement.getText();
		}
		return "";
	}

	
	/**
	 * Remove field value option list
	 * @param documentLevelFieldElement
	 * @return
	 */
	public void removeFieldValueOptionList(Element documentLevelFieldElement) {
		if(null != documentLevelFieldElement) {
			documentLevelFieldElement.removeChild(FIELDVALUEOPTIONLIST);
		}
	}
	
	/**
	 * Gets the child element
	 * @param parent
	 * @param childName
	 * @return
	 */
	public Element getChild(Element parent, String childName) {
		return parent.getChild(childName);
	}
	
	/**
	 * @param element
	 * @param name
	 * @return
	 */
	public int getCoordinate(Element element, String name) {
		Element coordinatesListElement = this.getChild(element, COORDINATESLIST);
		
		if(null != coordinatesListElement) {
			Element coordinatesElement = this.getChild(coordinatesListElement, COORDINATES);
			
			if(null != coordinatesElement) {
				Element coordinate = this.getChild(coordinatesElement, name);
				String value = coordinate.getText();
			
				if(value != null) {
					return Integer.parseInt(value);
				}
			}
		}
		return 0;
	}
	
	@SuppressWarnings("unchecked")
	public List<Element> getChildren(Element parent, String childName) {
		return parent.getChildren();
	}
	
	/**
	 * Sets the document field value 
	 * @param documentElement
	 * @param fieldName
	 * @param value
	 */
	public void setFieldValue(Element documentElement,String fieldName, String value) {
		if(null != documentElement) {
			Element valueElement = documentElement.getChild(fieldName);
			valueElement.setText(value);
		}
	}
	
	/**
	 * Sets the document level field value
	 * @param documentLevelFieldElement
	 * @param value
	 */
	public void setFieldValue(Element documentLevelFieldElement, String value) {
		if(null != documentLevelFieldElement) {
			Element valueElement = documentLevelFieldElement.getChild(VALUE);
			valueElement.setText(value);
		}
	}
	
	/**
	 * Add child element to parent
	 * @param sourceElement
	 * @param destElement
	 */
	public void copyDocumentLevelFieldContext(Element sourceElement, Element destElement) {
		//Page Element
		Element pageElement = sourceElement.getChild(PAGE);
		destElement.addContent((Element)pageElement.clone());
		
		//CoordinatesList Element
		Element coordinatesListElement = sourceElement.getChild(COORDINATESLIST);
		destElement.addContent((Element)coordinatesListElement.clone());
	}
	
	/**
	 * Add child element to parent
	 * @param sourceElement
	 * @param destElement
	 */
	public void copyElement(Element sourceElement, Element destElement, String elementName) {
		Element element = sourceElement.getChild(elementName);
		destElement.addContent((Element)element.clone());
	}
	
	/**
	 * Merge consecutive document types
	 * @param documentTypes
	 */
	public void mergeDocuments(HashMap<String, String> documentTypes) {
		List<Element> documentElements = this.getDocumentElements();
		
		int mergeDocIndex = 0;
		for (int i = 0; i < documentElements.size(); i++) { 
			Element documentElement = documentElements.get(mergeDocIndex);
			String docType = this.getDocType(documentElement);
			log("Document " + i + " - " + docType);
			
			if(documentElements.size() > (i + 1)) {
				Element nextDocumentElement = documentElements.get(i + 1);
				
				if(null != nextDocumentElement) {
					String nextDoxType = this.getDocType(nextDocumentElement);
					log("   Document " + (i+1) + " - " + nextDoxType);
					
					if(documentTypes.get(docType) != null  && documentTypes.get(docType).equals(nextDoxType)) {
						log("     merging...");
						mergeDocument(documentElement, nextDocumentElement);
					}
					else
						mergeDocIndex = (i+1);
				}
			}
		}
	}
	
	/**
	 * The <code>writeToXML</code> method will write the state document to the XML file.
	 * 
	 * @param document {@link DocumentUtil}.
	 */
	public void writeToXML(Document document) {

		String batchLocalPath = this.getBatchLocalPath();

		if (null == batchLocalPath) {
			log("Unable to find the local folder path in batch xml file.");
			return;
		}

		String batchInstanceID = this.getBatchInstanceIdentifier();

		if (null == batchInstanceID) {
			log("Unable to find the batch instance ID in batch xml file.");
			return;
		}
		String batchXMLPath = batchLocalPath.trim() + File.separator + batchInstanceID + File.separator + batchInstanceID
				+ EXT_BATCH_XML_FILE;

		String batchXMLZipPath = batchXMLPath + ZIP_FILE_EXT;

		OutputStream outputStream = null;
		File zipFile = new File(batchXMLZipPath);

		FileWriter writer = null;
		XMLOutputter out = new XMLOutputter();
		try {
			if (zipFile.exists()) {
				outputStream = getOutputStreamFromZip(batchXMLPath, batchInstanceID + EXT_BATCH_XML_FILE);
				out.output(this.document, outputStream);
			} else {
				writer = new java.io.FileWriter(batchXMLPath);
				out.output(this.document, writer);
				writer.flush();
				writer.close();
			}
		} catch (Exception e) {
			log(e.getMessage());
			e.printStackTrace();
		} finally {
			if (outputStream != null) {
				try {
					outputStream.close();
				} catch (IOException e) {
					log(e.getMessage());
					e.printStackTrace();
				}
			}
		}
	}

	/**
	 * Merge two document elements
	 * 
	 * @param document
	 * @param nextDocument
	 */
	public void mergeDocument(Element document, Element nextDocument) {
		
		//Merge pages
		Element pages = document.getChild(PAGES);
		Element nextDocumentPages = nextDocument.getChild(PAGES);
		List<?> pageList = nextDocumentPages.getChildren(PAGE);
		for (int i = 0; i < pageList.size(); i++) {
			Element duplicatePage = (Element) ((Element) pageList.get(i)).clone();
			pages.addContent(duplicatePage);
		}
		
		//Remove next document
		Element root = this.document.getRootElement();
		Element documents = this.getChild(root, DOCUMENTS);
		this.removeContent(documents, nextDocument);
	}
	
	/**
	 * Get First Page of the document
	 * 
	 * @param document
	 */
	public Element getFirstPageOfDocument(Element document) {
		
		//Merge pages
		Element pages = document.getChild(PAGES);
		List<?> pageList = pages.getChildren(PAGE);
		
		Element firstpage  = (Element) ((Element)pageList.get(0)); 
		return firstpage;
	}
	
	/**
	 * Get Previous Document of "documentId"
	 * @param documentId
	 * @return
	 */
	public Element getPreviousDocumentElement(String documentId) {
		List<Element> documentElements = this.getDocumentElements();
		Element PreviousElement = null;
		for(Element documentElement : documentElements) {
			
			Element identifierElement = documentElement.getChild(IDENTIFIER);
			String identifier = identifierElement.getText();
			
			if(identifier.equals(documentId))
				return PreviousElement;
			
			PreviousElement = documentElement;
		}
		
		return null;
	}
	
	/**
	 * Get Next Document of "documentId"
	 * @param documentId
	 * @return
	 */
	public Element getNextDocumentElement(String documentId) {
		List<Element> documentElements = this.getDocumentElements();
		boolean isdocid = false;
		for(Element documentElement : documentElements) {
			if(isdocid)
				return documentElement;
			Element identifierElement = documentElement.getChild(IDENTIFIER);
			String identifier = identifierElement.getText();
			
			if(identifier.equals(documentId))
				isdocid = true;
		}
		
		return null;
	}
	
	/**
	 * Remove content
	 * @param removeFromElement
	 * @param toBeRemovedElement
	 */
	private void removeContent(Element removeFromElement, Element toBeRemovedElement) {
		removeFromElement.removeContent(toBeRemovedElement);
	}
	
	/**
	 * Log message
	 * @param message
	 */
	private void log(String message) {
		this.logUtil.log(message);
	}
	
	/**
	 * @param zipName
	 * @param fileName
	 * @return
	 * @throws FileNotFoundException
	 * @throws IOException
	 */
	private static OutputStream getOutputStreamFromZip(final String zipName, final String fileName) throws FileNotFoundException,
			IOException {
		ZipOutputStream stream = null;
		stream = new ZipOutputStream(new FileOutputStream(new File(zipName + ZIP_FILE_EXT)));
		ZipEntry zipEntry = new ZipEntry(fileName);
		stream.putNextEntry(zipEntry);
		return stream;
	}
		
	/**
	 * Gets the element with a given name
	 * @param sourceElement
	 * @param elementName
	 * @return
	 */
	private Element getElement(Element sourceElement, String elementName) {
		return sourceElement.getChild(elementName);
	}
			
	/**
	 * Assign key field value from the given doctype to every other doctype in a batch. Field will be added
	 * to the document if its not present.
	 * @param docType
	 * @param fieldName
	 * @throws Exception 
	 */
	public void assignKeyField(String docType, String fieldName) throws Exception {
		
		if(null == docType)
			throw new Exception("Missing docType parameter");
		
		if(null == fieldName)
			throw new Exception("Missing fieldName parameter");
		
		// get document by type
		Element documentElement = this.getDocumentElementByType(docType);
		
		if(null == documentElement)
			throw new Exception("Missing document with type - " + docType);
		
		// get document field 
		Element documentLevelFieldElement = this.getDocumentLevelField(documentElement, fieldName);
		
		if(null == documentLevelFieldElement)
			throw new Exception("Missing field '" + fieldName + "' for docType " + docType);
		
		// get key field value
		String keyFieldValue = this.getFieldValue(documentLevelFieldElement);
		
		// populate document level field to every document
		List<Element> documentElements = this.getDocumentElements();
		
		for(Element docElement : documentElements) {
			Element existingElement = null;
			Element keyFieldElement = null;
			
			//docLevelFieldElement 
			Element documentLevelFieldsElement = this.getElement(docElement, DOCUMENTLEVELFIELDS);
			
			if(null == documentLevelFieldsElement)
				documentLevelFieldsElement = new Element(DOCUMENTLEVELFIELDS);
			else {
				existingElement = this.getDocumentLevelField(docElement, fieldName);
				documentLevelFieldsElement.detach();
			}
			
			// create key field element when missing
			if(null == existingElement) {
				keyFieldElement = new Element(DOCUMENTLEVELFIELD);
				keyFieldElement.addContent(new Element(NAME).setText(fieldName));
				keyFieldElement.addContent(new Element(VALUE).setText(keyFieldValue));
			}
			else {
				//check if the values are same
				String currentValue = this.getFieldValue(existingElement);
				String currentDocType = this.getDocType(docElement);
				
				if(currentValue == null || !currentValue.equals(keyFieldValue)) {					
					this.log("Key field value does not match. Key field doctype - " + docType + ", key field value - " + keyFieldValue + ", docType - " + currentDocType + ", value - " + currentValue);
				}
			}
			
			// attache document level fields
			if(null != keyFieldElement) {
				documentLevelFieldsElement.addContent(keyFieldElement);		
			}
			
			docElement.addContent(documentLevelFieldsElement);
		}
	}
	
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		File xmlFile = new File("c:\\Temp\\BI816D_batch_1.xml");
		try {
			Document document = (Document) new org.jdom.input.SAXBuilder().build(xmlFile);
			
			DocumentUtil documentUtil = new DocumentUtil(document, LogLevel.INFO);
			documentUtil.assignKeyField("Invoice", "Dealer FaxNumber");
			
			// new XMLOutputter().output(doc, System.out);
			XMLOutputter xmlOutput = new XMLOutputter();
	 
			// display nice nice
			xmlOutput.setFormat(org.jdom.output.Format.getPrettyFormat());
			xmlOutput.output(document, new FileWriter("c:\\Temp\\output.xml"));
			
		} catch (JDOMException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}  catch (Exception e) {
			e.printStackTrace();
		}
	}
}
