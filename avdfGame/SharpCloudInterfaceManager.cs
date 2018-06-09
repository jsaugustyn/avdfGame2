using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC.API.ComInterop;
using SC.API.ComInterop.Models;


namespace avdfGame
{
    public class SharpCloudInterfaceManager
    {
        SharpCloudApi scClient;
        Story scStory;

        private string instance_url = "https://my.sharpcloud.com/html/#/story/b3bac1eb-f0bb-46d8-b1d4-867e0e7edddd/view/1905a354-cf22-498e-9d6d-e82b0ae40434";
        private string username = "jsaugustyn";
        private string password = "gB!o6X1VKz&@Y3e";
        private string story_id = "b3bac1eb-f0bb-46d8-b1d4-867e0e7edddd";

        private string mtaname = "Major Technology Area";
        private string testname = "Applicability Score";
        private string tmp = "Human Performance Advanced Technology for Mobility & Lethality";

        public void CreateClient()
        {
            // first create an instanceof SharpCloudApi
            try
            {
                scClient = new SharpCloudApi(username, password);
                scStory = scClient.LoadStory(story_id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void LoadStory()
        {
            // load the story you are interested in
            Console.WriteLine("");
        }

        public void UpdateStory(string attname)
        {
            if (scStory != null)
            {
                var attribute1 = scStory.Attribute_FindByName(attname);
                var attribute2 = scStory.Attribute_FindByName(testname);

                // go through all the items to find the ones matching the filters
                var filteredItems = new List<Item>();
                foreach (var item in scStory.Items)
                {
                    // check the attribute value of the item                   
                    if (item.GetAttributeIsAssigned(attribute1))
                    {
                        filteredItems.Add(item);
                        
                    }
                }

                foreach (var item in filteredItems)
                {
                    if(item.GetAttributeValueAsText(attribute1) == tmp)
                    {
                        double score = item.GetAttributeValueAsDouble(attribute2) + 10;
                        item.SetAttributeValue(attribute2, score);

                    }
                }



                Console.WriteLine("");

                // save changes to the story
                scStory.Save();
            }
        }

    }
}
