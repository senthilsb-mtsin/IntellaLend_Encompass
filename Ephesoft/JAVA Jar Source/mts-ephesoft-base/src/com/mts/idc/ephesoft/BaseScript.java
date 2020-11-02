package com.mts.idc.ephesoft;

import org.jdom.Document;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.ephesoft.dcma.script.IJDomScript;

/**
 * Date Modified: 01/07/2015
 * @author Atit Patel   
 */
public abstract class BaseScript implements IJDomScript {
	
	public String logLevel;
	public DocumentUtil documentUtil;
	public Document document;
	private LogUtil logUtil;
	
	private Logger logger = LoggerFactory.getLogger(BaseScript.class);
	
	/**
	 * Log message
	 * @param message
	 */
	private void log(String message) {
		this.logUtil.log(message);
	}
	
	/**
	 * Initialize variables
	 * 
	 * @param document
	 * @param logLevel
	 */
	public void initialize(Document document, LogLevel logLevel) {
		this.document = document;
		this.documentUtil = new DocumentUtil(document, logLevel);
		this.logUtil = new LogUtil(logLevel, logger);
	}
}
