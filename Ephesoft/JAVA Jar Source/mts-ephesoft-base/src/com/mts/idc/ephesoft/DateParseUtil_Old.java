package com.mts.idc.ephesoft;

import java.text.*;
import java.util.Date;

public class DateParseUtil_Old
{
	
	private static String INVALID_CHARS = "-;th;rd;st;nd;day;of;th,;rd,;st,;nd,;day,;of,;,;";

	/**
	 * Parse date string to MM/dd/yyyy format.
	 * 
	 * @param String Date
	 */
	
	public static String ParseDate(String Date)
	{
		String formatteddate = null;
		
		//removing invalid characters
		//Date = RemoveSuffixandSeparators(Date);
		Date = Date.replaceAll("(?<=\\d)(rd|st|nd|th|day|of|th,|rd,|st,|nd,|day,|of,|-)\\b", "").replaceAll(",", "").replaceAll("\\s+", " ");
		
		//Format 1  - 05/16/2014
		SimpleDateFormat format = new SimpleDateFormat("MM/dd/yyyy");
		
		try {
			format.setLenient(false);
			Date fmtdate = format.parse(Date);
			
			formatteddate = format.format(fmtdate);
			
			formatteddate = ConvertCentury(formatteddate);
			
			return formatteddate;
		} catch (ParseException e) {
		}		
		
		//Format 2  - 16 May 2014
		try
		{
			DateFormat format2  = new SimpleDateFormat("dd MMM yyyy");
			format2.setLenient(false);
			Date mdate = format2.parse(Date);
			formatteddate = format.format(mdate);
			formatteddate = ConvertCentury(formatteddate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}		
		
		//Format 3  - 19 June 2014
		try
		{
			DateFormat format3  = new SimpleDateFormat("dd MMMM yyyy");
			format3.setLenient(false);
			Date mdate = format3.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}		
		
		//Format 4  - 16 March 2014
		try
		{
			DateFormat format4  = new SimpleDateFormat("dd MMMMM yyyy");
			format4.setLenient(false);
			Date mdate = format4.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}		
				
		//Format 5  - 16 August 2014
		try
		{
			DateFormat format5  = new SimpleDateFormat("dd MMMMMM yyyy");
			format5.setLenient(false);
			Date mdate = format5.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}		
		
		//Format 6  - 16 October 2014
		try
		{
			DateFormat format6  = new SimpleDateFormat("dd MMMMMMM yyyy");
			format6.setLenient(false);
			Date mdate = format6.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 7  - 16 November 2014
		try
		{
			DateFormat format7  = new SimpleDateFormat("dd MMMMMMMM yyyy");
			format7.setLenient(false);
			Date mdate = format7.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
				
		}				
		
		//Format 8  - 16 September 2014
		try
		{
			DateFormat format8  = new SimpleDateFormat("dd MMMMMMMMM yyyy");
			format8.setLenient(false);
			Date mdate = format8.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
						
		}

		
		//Format 9  - 6 May 2014, 2 Sep 2014
		try
		{
			DateFormat format9  = new SimpleDateFormat("d MMM yyyy");
			format9.setLenient(false);
			Date mdate = format9.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}		
		
		//Format 10  - 9 June 2014
		try
		{
			DateFormat format10  = new SimpleDateFormat("d MMMM yyyy");
			format10.setLenient(false);
			Date mdate = format10.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}		
		
		//Format 11  - 6 March 2014
		try
		{
			DateFormat format11  = new SimpleDateFormat("d MMMMM yyyy");
			format11.setLenient(false);
			Date mdate = format11.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}		
				
		//Format 12  - 1 August 2014
		try
		{
			DateFormat format12  = new SimpleDateFormat("d MMMMMM yyyy");
			format12.setLenient(false);
			Date mdate = format12.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}		
		
		//Format 13  - 6 October 2014
		try
		{
			DateFormat format13  = new SimpleDateFormat("d MMMMMMM yyyy");
			format13.setLenient(false);
			Date mdate = format13.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 14  - 9 November 2014
		try
		{
			DateFormat format14  = new SimpleDateFormat("d MMMMMMMM yyyy");
			format14.setLenient(false);
			Date mdate = format14.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
				
		}				
		
		//Format 15  - 5 September 2014
		try
		{
			DateFormat format15  = new SimpleDateFormat("d MMMMMMMMM yyyy");
			format15.setLenient(false);
			Date mdate = format15.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
						
		}
		
		//Format 16  - 3/16/14
		try
		{
		DateFormat format16  = new SimpleDateFormat("d/MM/yy");
		format16.setLenient(false);
		Date mdate = format16.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		}
		
		//Format 17  - 3/6/14
		try
		{
		DateFormat format17  = new SimpleDateFormat("d/M/yy");
		format17.setLenient(false);
		Date mdate = format17.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		}
		
		//Format 18  - 3/16/2014
		try
		{
		DateFormat format18  = new SimpleDateFormat("d/MM/yyyy");
		format18.setLenient(false);
		Date mdate = format18.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		}
		
		//Format 19  - 3/6/2014
		try
		{
		DateFormat format19  = new SimpleDateFormat("d/M/yyyy");
		format19.setLenient(false);
		Date mdate = format19.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		}
		
		//Format 20  - 3 16 14
		try
		{
		DateFormat format20  = new SimpleDateFormat("M dd yy");
		format20.setLenient(false);
		Date mdate = format20.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		}
		
		//Format 21  - 3 6 14
		try
		{
		DateFormat format21  = new SimpleDateFormat("M d yy");
		format21.setLenient(false);
		Date mdate = format21.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		}
		
		//Format 22  - 3 16 2014
		try
		{
		DateFormat format22  = new SimpleDateFormat("M dd yyyy");
		format22.setLenient(false);
		Date mdate = format22.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		}
		
		//Format 23  - 3 6 2014
		try
		{
		DateFormat format23  = new SimpleDateFormat("d M yyyy");
		format23.setLenient(false);
		Date mdate = format23.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		}
		
		//Format 24  - May 01 2014, Sep 23 2013
		try
		{
		DateFormat format24  = new SimpleDateFormat("MMM dd yyyy");
		format24.setLenient(false);
		Date mdate = format24.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 25  - June 01 2014, July 23 2013
		try
		{
		DateFormat format25  = new SimpleDateFormat("MMMM dd yyyy");
		format25.setLenient(false);
		Date mdate = format25.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 26  - April 01 2014
		try
		{
		DateFormat format26  = new SimpleDateFormat("MMMMM dd yyyy");
		format26.setLenient(false);
		Date mdate = format26.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 27  - August 23 2013
		try
		{
		DateFormat format27  = new SimpleDateFormat("MMMMMM dd yyyy");
		format27.setLenient(false);
		Date mdate = format27.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 28  - October 01 2014
		try
		{
		DateFormat format28  = new SimpleDateFormat("MMMMMMM dd yyyy");
		format28.setLenient(false);
		Date mdate = format28.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 29  - November 01 2014
		try
		{
		DateFormat format29  = new SimpleDateFormat("MMMMMMMM dd yyyy");
		format29.setLenient(false);
		Date mdate = format29.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 30  - September 23 2013
		try
		{
		DateFormat format30  = new SimpleDateFormat("MMMMMMMMM dd yyyy");
		format30.setLenient(false);
		Date mdate = format30.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 31  - May 1 2014, Sep 3 2013
		try
		{
		DateFormat format31  = new SimpleDateFormat("MMM d yyyy");
		format31.setLenient(false);
		Date mdate = format31.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 32  - June 1 2014, July 3 2013
		try
		{
		DateFormat format32  = new SimpleDateFormat("MMMM d yyyy");
		format32.setLenient(false);
		Date mdate = format32.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 33  - April 1 2014
		try
		{
		DateFormat format33  = new SimpleDateFormat("MMMMM d yyyy");
		format33.setLenient(false);
		Date mdate = format33.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 34  - August 2 2013
		try
		{
		DateFormat format34  = new SimpleDateFormat("MMMMMM d yyyy");
		format34.setLenient(false);
		Date mdate = format34.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 35  - October 7 2014
		try
		{
		DateFormat format35  = new SimpleDateFormat("MMMMMMM d yyyy");
		format35.setLenient(false);
		Date mdate = format35.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 36  - November 6 2014
		try
		{
		DateFormat format36  = new SimpleDateFormat("MMMMMMMM d yyyy");
		format36.setLenient(false);
		Date mdate = format36.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 37  - September 2 2013
		try
		{
		DateFormat format37  = new SimpleDateFormat("MMMMMMMMM d yyyy");
		format37.setLenient(false);
		Date mdate = format37.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 38  - May 01 14, Sep 23 13
		try
		{
		DateFormat format38  = new SimpleDateFormat("MMM dd yy");
		format38.setLenient(false);
		Date mdate = format38.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 39  - June 01 14, July 23 13
		try
		{
		DateFormat format39  = new SimpleDateFormat("MMMM dd yy");
		format39.setLenient(false);
		Date mdate = format39.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 40  - April 01 14
		try
		{
		DateFormat format40  = new SimpleDateFormat("MMMMM dd yy");
		format40.setLenient(false);
		Date mdate = format40.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 41  - August 23 13
		try
		{
		DateFormat format41  = new SimpleDateFormat("MMMMMM dd yy");
		format41.setLenient(false);
		Date mdate = format41.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 42  - October 01 14
		try
		{
		DateFormat format42  = new SimpleDateFormat("MMMMMMM dd yy");
		format42.setLenient(false);
		Date mdate = format42.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 43  - November 01 14
		try
		{
		DateFormat format43  = new SimpleDateFormat("MMMMMMMM dd yy");
		format43.setLenient(false);
		Date mdate = format43.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 44  - September 23 13
		try
		{
		DateFormat format44  = new SimpleDateFormat("MMMMMMMMM dd yy");
		format44.setLenient(false);
		Date mdate = format44.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 45  - May 1 14, Sep 3 13
		try
		{
		DateFormat format45  = new SimpleDateFormat("MMM d yy");
		format45.setLenient(false);
		Date mdate = format45.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 46  - June 1 14, July 3 13
		try
		{
		DateFormat format46  = new SimpleDateFormat("MMMM d yy");
		format46.setLenient(false);
		Date mdate = format46.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 47  - April 1 14
		try
		{
		DateFormat format47  = new SimpleDateFormat("MMMMM d yy");
		format47.setLenient(false);
		Date mdate = format47.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 48  - August 2 13
		try
		{
		DateFormat format48  = new SimpleDateFormat("MMMMMM d yy");
		format48.setLenient(false);
		Date mdate = format48.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 49  - October 7 14
		try
		{
		DateFormat format49  = new SimpleDateFormat("MMMMMMM d yy");
		format49.setLenient(false);
		Date mdate = format49.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 50  - November 6 14
		try
		{
		DateFormat format50  = new SimpleDateFormat("MMMMMMMM d yy");
		format50.setLenient(false);
		Date mdate = format50.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 51  - September 2 13
		try
		{
		DateFormat format51  = new SimpleDateFormat("MMMMMMMMM d yy");
		format51.setLenient(false);
		Date mdate = format51.parse(Date);
		formatteddate = format.format(mdate);
		return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 52  - 03-29-2016
		try
		{
			DateFormat format52  = new SimpleDateFormat("MM-dd-yyyy");
			format52.setLenient(false);
			Date mdate = format52.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 53  - 03-29-16
		try
		{
			DateFormat format53  = new SimpleDateFormat("MM-dd-yy");
			format53.setLenient(false);
			Date mdate = format53.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 53  - 29-03-2016
		try
		{
			DateFormat format53  = new SimpleDateFormat("dd-MM-yyyy");
			format53.setLenient(false);
			Date mdate = format53.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 55  - 29-03-16
		try
		{
			DateFormat format54  = new SimpleDateFormat("dd-MM-yy");
			format54.setLenient(false);
			Date mdate = format54.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 56  - 20160329
		try
		{
			DateFormat format56  = new SimpleDateFormat("yyyyMMdd");
			format56.setLenient(false);
			Date mdate = format56.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		//Format 57  - 03292016
		try
		{
			DateFormat format57  = new SimpleDateFormat("MMddyyyy");
			format57.setLenient(false);
			Date mdate = format57.parse(Date);
			formatteddate = format.format(mdate);
			return formatteddate;
		}
		catch (ParseException e)
		{
		
		}
		
		return formatteddate;
	}
	
	private static String RemoveSuffixandSeparators(String Date)
	{
		
		String[] arr = INVALID_CHARS.toLowerCase().split(";");
		for(int i=0;i < arr.length;i++)
		{
			if (Date.contains(arr[i]))
			{
				Date = Date.replace(arr[i], " ");
			}
		}
		
		//For removing extra spaces
		 Date = DelteSpaces(Date); 
		
		return Date;
	}

	private static String DelteSpaces(String str){ 
        StringBuilder sb=new StringBuilder();
        for(String s: str.split(" ")){

            if(!s.equals(""))        // ignore space
             sb.append(s+" ");       // add word with 1 space

        }
        return new String(sb.toString());
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



