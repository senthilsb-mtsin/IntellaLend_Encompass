package com.mts.idc.ephesoft.service;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import org.apache.http.HttpResponse;
import org.apache.http.ParseException;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpPut;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.util.EntityUtils;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import com.google.gson.Gson;

public class JsonResponseWSClient {

	public JSONObject invokeJsonResponseWS(QCIQLookupRequest requestMessage, String url){
		JSONObject responseObj=null;
		
		Gson gson = new Gson();
		String jsonMessage = gson.toJson(requestMessage);
		
		// Create the HTTP Put request for the Ephesoft Utility web service
		CloseableHttpClient httpClient = HttpClientBuilder.create().build();
		HttpPut request = new HttpPut(url);
		
		// Set the Json message in the request
		StringEntity params = null;
		try {
			params = new StringEntity(jsonMessage);
		} catch (UnsupportedEncodingException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		request.addHeader("content-type", "application/json");
		request.setEntity(params);
		
		// Invoke the web service
		HttpResponse result = null;
		try {
			result = httpClient.execute(request);
		} catch (ClientProtocolException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		// Get the XML response from the web service
		try {
			String responseStringJson = EntityUtils.toString(result.getEntity(), "UTF-8");
			JSONParser parser = new JSONParser(); 
			try {
				responseObj = (JSONObject) parser.parse(responseStringJson);
			} catch (org.json.simple.parser.ParseException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		} catch (ParseException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		
		return responseObj;
	}
}
