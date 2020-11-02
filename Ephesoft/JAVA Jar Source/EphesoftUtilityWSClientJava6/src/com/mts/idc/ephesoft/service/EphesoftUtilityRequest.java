package com.mts.idc.ephesoft.service;

/**
 * @author Nisha
 * 
 *         This class represents the request from the client. It contains the
 *         Ephesoft XML file, the Ephesoft module which could be REVIEW,
 *         VALIDATION and EXPORT. The three flags indicate if the append,
 *         concatenate and convert operations should be performed for the input
 *         XML.
 */
public class EphesoftUtilityRequest {

	public String inputXML;
	public Boolean appendFlag;
	public Boolean concatenateFlag;
	public Boolean convertFlag;
	public Boolean pageSequenceFlag;
	public int ephesoftModule;
	public String orderOfExecution;

	public String getInputXML() {
		return inputXML;
	}

	public String getOrderOfExecution() {
		return orderOfExecution;
	}

	public void setInputXML(String inputXML) {
		this.inputXML = inputXML;
	}

	public Boolean getAppendFlag() {
		return appendFlag;
	}

	public void setAppendFlag(Boolean appendFlag) {
		this.appendFlag = appendFlag;
	}

	public Boolean getConcatenateFlag() {
		return concatenateFlag;
	}

	public void setConcatenateFlag(Boolean concatenateFlag) {
		this.concatenateFlag = concatenateFlag;
	}

	public Boolean getConvertFlag() {
		return convertFlag;
	}

	public void setConvertFlag(Boolean convertFlag) {
		this.convertFlag = convertFlag;
	}

	public int getEphesoftModule() {
		return ephesoftModule;
	}

}
