package com.mts.idc.ephesoft.service;

import java.io.IOException;
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

import com.google.gson.Gson;

public class QCIQLookupWSClient {
	
	public Document invokeQCIQLookupWS(QCIQLookupRequest requestMessage, String url)
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
