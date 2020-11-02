package com.mts.idc.ephesoft;

import java.text.*;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

public class DateParseUtil
{
	/**
	 * Parse date string to MM/dd/yyyy format.
	 * 
	 * @param String Date
	 */
	
	private static String FINAL_FORMAT = "MM/dd/yyyy";
	private static List<String> formats = Arrays.asList(
			"MM/dd/yyyy", 
			"MM-dd-yyyy", 			
			"MM/dd/yy",
			"MM-dd-yy",			
			"MM.dd.yyyy",
			"MM.dd.yy",			
			"MM dd yyyy", 
			"MM dd yy",			
			"MMM dd yyyy", 
			"MMM dd yy",			
			"MMddyyyy", 
			"MMddyy",						
			"Mdyyyy",
			"Mdyyyy",			
			"MMdyyyy",
			"Mddyyyy",
			"MMdyy",
			"Mddyy",
			"Mdyy",
			"yyyyMMdd",
			"yyyyddMM",
			"yyyyMMd",
			"yyyydMM",
			"yyyyMdd",
			"yyyyMd",
			"yyyy/MM/dd",
			"yyyy/dd/MM",
			"yy/MM/dd",
			"yy/dd/MM",
			"yyyy/M/d",
			"yyyy/d/M",
			"yy/M/d",
			"yy/d/M",			
			"dd/MM/yyyy", 
			"dd-MM-yyyy",			
			"dd/MM/yyyy",
			"dd-MM-yy",	
			"dd-M-yy",	
			"dd MM yyyy", 
			"dd MM yy",			
			"dd MMM yyyy", 
			"dd MMMyyyy",
			"dd MMMyy",
			"dd MMM yy",
			"ddMMyyyy", 
			"ddMMyy",
			"ddMyyyy",			
			"MMM-dd",
			"dd-MMM",
			"MMM-d",
			"d-MMM",			
			"MM-d",
			"d-MM",
			"MM-dd",
			"dd-MM",					
			"MMM dd",
			"dd MMM",
			"MMM d",
			"d MMM",
			"dMMM",
			"MMMd",
			"ddMMM",
			"MMMdd",
			"MMdd",
			"ddMM",
			"MMd",
			"dMM",
			"MM dd",
			"dd MM",
			"MM d",
			"d MM");
	
	
	/**
	 * @param inputDate
	 * @return
	 */
	public static String ParseDate(String inputDate)
	{
		String formattedDate = null;
		
		//removing invalid characters
		inputDate = inputDate.replaceAll("(?<=\\d)(rd|st|nd|th|th,|rd,|st,|nd,|)\\b", "");
		inputDate = inputDate.replaceAll("day|of|day,|of,|-", "").replaceAll(",", " ").replaceAll("\\s+", " ");
				
		SimpleDateFormat outputFormat = new SimpleDateFormat(FINAL_FORMAT);
		
		//check every date format
		SimpleDateFormat inputFormat = null;
		
		for(String format : formats) {
			//System.out.print("          applying: " + format);
			
			try {
				
				inputFormat = new SimpleDateFormat(format);
				inputFormat.setLenient(false);
				
				//check input format
				Date parsedDate = inputFormat.parse(inputDate);
				
				//convert to output format
				formattedDate = outputFormat.format(parsedDate);
				
				//add century
				formattedDate = ConvertCentury(formattedDate);
				
				//System.out.println(" - Passed");
				
				int docYr = Integer.parseInt(formattedDate.substring(6).toString());
				
				Calendar now = Calendar.getInstance();
				int currYear = now.get(Calendar.YEAR);
				break;
//				if(docYr <= (currYear + 2) && docYr >= (currYear - 2) )
//				{
//					break;
//				}
			} catch (ParseException e) {
				//System.out.println(" - Failed");
			}
		}
		
		return formattedDate;
	}
	public static String ParseDate(String inputDate, String OutPutFormat){
		
		String formattedDate = inputDate;
		
		//removing invalid characters
		inputDate = inputDate.replaceAll("(?<=\\d)(rd|st|nd|th|th,|rd,|st,|nd,|)\\b", "");
		inputDate = inputDate.replaceAll("day|of|day,|of,|-", "").replaceAll(",", " ").replaceAll("\\s+", " ");
				
		SimpleDateFormat outputFormat = new SimpleDateFormat(OutPutFormat);
		
		//check every date format
		SimpleDateFormat inputFormat = null;
		
		for(String format : formats) {
			//System.out.print("          applying: " + format);
			
			try {
				
				inputFormat = new SimpleDateFormat(format);
				inputFormat.setLenient(false);
				
				//check input format
				Date parsedDate = inputFormat.parse(inputDate);
				
				//convert to output format
				formattedDate = outputFormat.format(parsedDate);			
	
				int docYr = Integer.parseInt(formattedDate.substring(6).toString());
				
				Calendar now = Calendar.getInstance();
				int currYear = now.get(Calendar.YEAR);
				
				if(docYr <= (currYear + 2) && docYr >= (currYear - 2) )
				{
					break;
				}				
			} catch (Exception e) {
				//System.out.println(" - Failed");
			}
		}
		
		return formattedDate;
		
	}
	public static String ParseDate(String inputDate, String InputFormat, String OutPutFormat)
	{
		String formattedDate = null;
		
		//removing invalid characters
		inputDate = inputDate.replaceAll("(?<=\\d)(rd|st|nd|th|th,|rd,|st,|nd,|)\\b", "");
		inputDate = inputDate.replaceAll("day|of|day,|of,|-", "").replaceAll(",", " ").replaceAll("\\s+", " ");
				
		SimpleDateFormat outputFormat = new SimpleDateFormat(OutPutFormat);
		
		//check every date format
		SimpleDateFormat inputFormat = null;
		
	
		try {
			
			inputFormat = new SimpleDateFormat(InputFormat);
			inputFormat.setLenient(false);
			
			//check input format
			Date parsedDate = inputFormat.parse(inputDate);
			
			//convert to output format
			formattedDate = outputFormat.format(parsedDate);
			
			//add century
			formattedDate = ConvertCentury(formattedDate);
			
			//System.out.println(" - Passed");
		} catch (ParseException e) {
			//System.out.println(" - Failed");
			formattedDate = inputDate;
		}
		
		return formattedDate;
	}
	
	private static String ConvertCentury(String formatteddate)
	{
		if(formatteddate.substring(6,8).equals("00"))
		{
			formatteddate = formatteddate.substring(0,6) + "20" +  formatteddate.substring(8,10);
		}
		return formatteddate;
	}
}



