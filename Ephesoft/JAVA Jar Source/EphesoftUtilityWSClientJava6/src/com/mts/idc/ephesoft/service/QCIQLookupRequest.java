package com.mts.idc.ephesoft.service;

/**
 *         This class represents the request from the client. It contains the
 *         Ephesoft XML file, the Ephesoft module which could be REVIEW,
 *         VALIDATION and EXPORT.
 */
public class QCIQLookupRequest {

	public String inputXML;
	public Boolean isManual;
	
	
	public String getInputXML() {
		return inputXML;
	}
}
