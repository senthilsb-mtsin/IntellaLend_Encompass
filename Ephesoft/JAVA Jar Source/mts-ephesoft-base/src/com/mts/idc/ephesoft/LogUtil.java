package com.mts.idc.ephesoft;

import org.slf4j.Logger;

public class LogUtil {
	
	private LogLevel logLevel;
	private Logger logger;
	
	/**
	 * @param logLevel
	 * @param logger
	 */
	public LogUtil(LogLevel logLevel, Logger logger) {
		this.logLevel = logLevel;
		this.logger = logger;
	}
	
	/**
	 * Log message
	 * @param message
	 */
	public void log(String message) {
		if(this.logLevel == LogLevel.INFO) {
			if(logger.isErrorEnabled())
				logger.error(message);
			else
				logger.info(message);
		}
		else if(this.logLevel == LogLevel.ERROR)
			logger.error(message);
	}
}
