using Newtonsoft.Json;
using DP;
using Root = DP.ImaggaModel.Root;
using static DP.USDAmodel;

namespace BL
{
    public class ImaggaLogic
    {
        //Check if the URL is correct,if it's food, kosher food, and maches the tag/title
        public string IsGoodPic(ImaggaParamsDTO data)
        {
            List<string> words = new List<string>();
            foreach (var word in data.Title.Split(' '))
            {
                words.Add(word);
            }
 
            //to know what massage to return
            List<string> tags = new List<string>();

            //get the values from the imagga
            DAL.ImaggaAdapter dal = new DAL.ImaggaAdapter();
            Root myPicture = null;
            if (data.ImageByte == null)
                return "Error: wrong Imege";
            string myJson = dal.GetImageInformation(data.ImageByte);
            if (myJson != null)
                myPicture = JsonConvert.DeserializeObject<Root>(myJson);
            bool degel = false;

            if (myPicture != null && myPicture.status.type == "success")
            {
                foreach (var tag in myPicture.result.tags)
                {
                    if (tag.tag.en != "Food")
                    {
                        return "The provided picture doesn't contain food";
                    }

                    foreach (var word in words)
                    {

                        if (word == tag.tag.en && tag.confidence >= 50)
                            degel = true;
                        break;

                    }
                }
                return "Error: no match title and picture";


            }
            else
            {
                if (degel == false)
                {
                    return "Error: somthing got wrong, can't read the image";

                }
                else
                { return "The image is good"; }
            }

        }


    }
}
