package com.mts.idc.ephesoft.service;

//import java.io.File;
import java.io.IOException;
//import java.io.PrintWriter;
import java.io.StringReader;

import org.apache.http.HttpResponse;
import org.apache.http.HttpStatus;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpPut;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.util.EntityUtils;
import org.jdom.Document;
import org.jdom.JDOMException;
import org.jdom.input.SAXBuilder;
//import org.jdom.output.XMLOutputter;

import com.google.gson.Gson;

/**
 * @author Nisha
 * @version 1.0
 */
public class EphesoftUtilityWSClient {

//	private static String REST_SERVICE_URL = "http://localhost:57910/Document";
//
//	public static void main(String[] str) {
//		SAXBuilder builder = new SAXBuilder();
//		File xmlFile = new File("C:\\Users\\devadmin\\Documents\\Projects\\PeoplesUnitedDemo\\Ephesoft 4.0 CR\\Testing\\600Pages\\test2000pages.xml");
//				//"C:\\Users\\devadmin\\Desktop\\New folder (2)\\testbatch\\BI9_batch.xml");
//		Document document;
//		try {
//			document = (Document) builder.build(xmlFile);
//
//			EphesoftUtilityRequest requestContent = new EphesoftUtilityRequest();
//			requestContent.appendFlag = true;
//			requestContent.concatenateFlag = true;
//			requestContent.convertFlag = true;
//			requestContent.ephesoftModule = "EXPORT";
//			XMLOutputter xmlOut = new XMLOutputter();
//			requestContent.inputXML = xmlOut.outputString(document);
//
//			EphesoftUtilityWSClient client = new EphesoftUtilityWSClient();
//			client.invokeEphesoftUtilityWS(requestContent, REST_SERVICE_URL);
//		} catch (JDOMException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		} catch (IOException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
//
//	}

	public Document invokeEphesoftUtilityWS(EphesoftUtilityRequest requestMessage, String url)
			throws IOException, ClientProtocolException, JDOMException {

		// Create a Json message for the request class
		Gson gson = new Gson();
		String jsonMessage = gson.toJson(requestMessage);
		
		Document responseDoc = null;

		// Create the HTTP Put request for the Ephesoft Utility web service
		CloseableHttpClient httpClient = HttpClientBuilder.create().build();
		HttpPut request = new HttpPut(url);
		
		// Set the Json message in the request
		StringEntity params = new StringEntity(jsonMessage);
		request.addHeader("content-type", "application/json");
		request.setEntity(params);
		
		// Invoke the web service
		HttpResponse result = httpClient.execute(request);
		
		// Get the XML response from the web service
		String responseXML = EntityUtils.toString(result.getEntity(), "UTF-8");

		// Check if the response status code is 200 - OK
		// If OK, create a Document object for the response XML and return value. If the
		// web service call failed, a null object will be returned. 
		if (result.getStatusLine().getStatusCode() == HttpStatus.SC_OK) {
			SAXBuilder builder = new SAXBuilder();
			responseDoc = (Document) builder
					.build(new StringReader(responseXML));
		}
		return responseDoc;
	}
	
	
}