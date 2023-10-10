using DP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using System.Threading;

namespace BL
{
 //HebCalLogic is checking if there is a holiday in the next week
    public class HebCalLogic
    {
        string[] theHolidays = {"ראש השנה", "פורים", "שבועות", "פסח", "חנוכה", "סוכות" };

        //return a list of the holidays in the comming week
        public List<string> IsHolidyWeek(string today,string SevenDaysFromNow)
        {
            //gets the values of the comming week
            DAL.HebCalAdapter dal = new DAL.HebCalAdapter();
            Root myHolyday = null;
            string myJson = dal.GetComingWeek(today, SevenDaysFromNow);
            if (myJson != null)
                myHolyday = JsonConvert.DeserializeObject<Root>(myJson);

            //will contain the relevant holidays
            List<string> holidays = new List<string>();
          
            foreach (var item in myHolyday.items)
                if (item.category == "holiday")
                    if (item.hebrew!=null)
                        foreach(string holiday in theHolidays)
                            if (item.hebrew.Contains(holiday) && !holidays.Contains(holiday)&& item.hebrew != "פסח שני")
                            {
                                holidays.Add(holiday);
                                break;
                            }
            
            if(holidays.Count == 0)
                holidays.Add("No holiday");
            return holidays;
        }
    }
}


