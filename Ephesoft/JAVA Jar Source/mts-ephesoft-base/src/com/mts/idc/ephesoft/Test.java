package com.mts.idc.ephesoft;

import java.util.LinkedHashMap;
import java.util.Map;

public class Test {

	public static void main(String[] args) {
		//sample dates used are 11/29/2015, 1/9/2015
		
		Map<String, String> values = new LinkedHashMap<String, String>();
		values.put("MM/dd/yyyy", "11/29/2015");
		values.put("MM/dd/yy", "11/29/15");
		values.put("M/dd/yyyy", "1/09/2015");
		values.put("M/d/yyyy", "1/9/2015");
		values.put("M/d/yy", "1/9/15");
		
		values.put("MM.dd.yyyy", "11.29.2015");
		values.put("MM.dd.yy", "11.29.15");
		
		values.put("dd/MM/yyyy", "29/11/2015");
		values.put("dd/MM/yy", "29/11/15");
		values.put("dd/M/yyyy", "09/1/2015");
		values.put("dd/M/yyyy", "29/11/2015");
		values.put("d/M/yyyy", "9/1/2015");
		values.put("d/M/yyyy", "29/1/2015");
		values.put("d/M/yy", "9/1/15");
		values.put("d/M/yy", "29/1/15");
		
		values.put("MM-dd-yyyy", "11-29-2015");
		values.put("MM-dd-yy", "11-29-15");
		values.put("M-dd-yyyy", "01-09-2015");
		values.put("M-d-yyyy", "1-9-2015");
		values.put("M-d-yy", "1-9-15");
		
		values.put("dd-MM-yyyy", "09-01-2015");
		values.put("dd-MM-yy", "09-01-15");
		values.put("dd-M-yyyy", "09-1-2015");
		values.put("dd-MM-yyyy", "09-01-2015");
		values.put("d-M-yy", "9-1-15");	
		
		values.put("MM dd yyyy", "11 29 2015");
		values.put("MM dd yy", "11 29 15");
		values.put("M dd yyyy", "1 09 2015");
		values.put("M d yyyy", "1 9 2015");
		values.put("M d yy", "1 9 15");
		
		values.put("dd MM yyyy", "09 01 2015");
		values.put("dd MM yy", "09 01 15");
		values.put("d MM yyyy", "9 01 2015");
		values.put("d M yyyy", "9 1 2015");
		values.put("d M yy", "9 1 15");
		
		values.put("MMM dd yyyy", "November 29 2015");
		values.put("MMM dd yy", "November 29 15");
		
		values.put("dd MMM yyyy", "29 November 2015");
		values.put("dd MMM yy", "29 November 15");
		
		values.put("yyyyMMdd", "20151129");
		values.put("yyyyMMd", "2015019");
		values.put("yyyyMdd", "2015109");
		values.put("yyyyMd", "201519");
		
		values.put("MMddyyyy", "11292015");
		values.put("MMddyy", "112915");
		
		values.put("ddMMyyyy", "29112015");
		values.put("ddMMyy", "291115");
		
		//values.put("dd MMM yyyy", "29th November,2015");
		//values.put("dd MMM yyyy", "29th November, 2015");
		//values.put("dd MMM yyyy", "29th   November,  2015");
		//values.put("dd MMM yyyy", "29th day of November,2015");
		//values.put("dd MMM yyyy", "29th day of November, 2015");
		//values.put("MMM dd yyyy", "Nov 29, 2015");
		//values.put("MMM dd yyyy", "Nov 29,2015");
		//values.put("MMM dd yyyy", "November 29th, 2015");
		//values.put("MMM dd yyyy", "November 29th,2015");
		
		for(String key : values.keySet()) {
			String value = values.get(key);
			
			System.out.print(key + " \n    Input:" + value + "\n");
			System.out.println("    Output: "+ DateParseUtil.ParseDate(value));
			System.out.println("\n\n");
		}
	}
}
