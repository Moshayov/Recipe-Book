using Newtonsoft.Json;
using DP;
using static DP.ImaggaModel;
using System;
using System.Collections.Generic;
using System.Text;
using Root = DP.ImaggaModel.Root;
using static DP.USDAmodel;

namespace BL
{
    public class ImaggaLogic
    {
        //Check if the URL is correct,if it's food, kosher food, and maches the tag/title
        public string IsGoodPic(ImaggaParamsDTO data)
        {
            //for the kosher part
            string dairyFood = "milk, butter, cheese";
            string meatyFood = "meat, beef, chicken";
            string notKosherFood = "lobster,shrimps,horseshoe crab";

            //to know what massage to return
            List<string> tags = new List<string>();

            //get the values from the imagga
            DAL.ImaggaAdapter dal = new DAL.ImaggaAdapter();
            Root myPicture = null;
            if (data.ImageUrl == null)
                return "Error: wrong URL";
            string myJson = dal.GetImageInformation(data.ImageUrl);
            if (myJson != null)
                myPicture = JsonConvert.DeserializeObject<Root>(myJson);

            //put in the tag list the interesting values from the immaga
            if (myPicture != null && myPicture.status.type == "success")
            {
                foreach (var tag in myPicture.result.tags)
                {
                    if (notKosherFood.Contains(tag.tag.en) && tag.confidence > 50)
                        return "Error: the food is not kosher";
                    if (tag.tag.en == "food")
                        tags.Add("food");
                    if (data.Title.ToLower().Contains(tag.tag.en.ToLower()) || data.Tag.ToLower()==tag.tag.en)
                        tags.Add("title");
                    if (dairyFood.Contains(tag.tag.en.ToLower()) && tag.confidence > 50 && !tags.Contains("dairy"))
                        tags.Add("dairy");
                    if (meatyFood.Contains(tag.tag.en.ToLower()) && tag.confidence > 50 && !tags.Contains("meaty"))
                        tags.Add("meaty");
                }
            }
            else 
                return "Error: somthing got wrong, can't read the image";

            //return the message
            if(!tags.Contains("food"))
                return "Error: the food in not eaten";
            if (!tags.Contains("title"))
                return "Error: no match title and picture";
            if (tags.Contains("dairy") && tags.Contains("meaty"))
                return "Error: the food is not kosher";
            return "good image";
        }

    }
}
