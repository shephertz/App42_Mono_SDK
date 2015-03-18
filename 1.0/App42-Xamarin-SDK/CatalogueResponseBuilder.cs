using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.shopping
{
    public class CatalogueResponseBuilder : App42ResponseBuilder
    {
        /// <summary>
        /// @throws Exception
        /// </summary>
        /// <param name="json">response</param>

        public Catalogue BuildResponse(String response)
        {
            JObject cataloguesJSONObject = GetServiceJSONObject("catalogues", response);
            JObject catalogueJSONObject = (JObject)cataloguesJSONObject["catalogue"];
            Catalogue catalogue = BuildCatalogueObject(catalogueJSONObject);
            catalogue.SetStrResponse(response);
            catalogue.SetResponseSuccess(IsResponseSuccess(response));
            return catalogue;
        }

        /// <summary>
        /// @throws Exception
        /// </summary>
        /// <param name="json">response</param>

        public IList<Catalogue> BuildArrayResponse(String response)
        {
            JObject cataloguesJSONObject = GetServiceJSONObject("catalogues", response);
            IList<Catalogue> catalogueList = new List<Catalogue>();

            if (cataloguesJSONObject["catalogue"] != null && cataloguesJSONObject["catalogue"] is JObject)
            {
                //Single Object
                JObject catalogueJSONObject = (JObject)cataloguesJSONObject["catalogue"];
                Catalogue catalogue = new Catalogue();
                catalogue.SetStrResponse(response);
                catalogue.SetResponseSuccess(IsResponseSuccess(response));
                BuildObjectFromJSONTree(catalogue, catalogueJSONObject);
                catalogueList.Add(catalogue);
            }

            else
            {
                //Get JSON Array for Catalogue
                JArray catalogueJSONArray = (JArray)cataloguesJSONObject["catalogue"];
                for (int i = 0; i < catalogueJSONArray.Count; i++)
                {
                    JObject catalogueJSONObj = (JObject)catalogueJSONArray[i];
                    Catalogue catalogue = BuildCatalogueObject(catalogueJSONObj);
                    catalogue.SetStrResponse(response);
                    catalogue.SetResponseSuccess(IsResponseSuccess(response));
                    catalogueList.Add(catalogue);
                }
            }
            return catalogueList;
        }


        /// <summary>
        /// @throws Exception
        /// </summary>
        /// <param name="json">catalogueJSONObj</param>

        private Catalogue BuildCatalogueObject(JObject catalogueJSONObj)
        {
            Catalogue catalogue = new Catalogue();
            BuildObjectFromJSONTree(catalogue, catalogueJSONObj);
            if (catalogueJSONObj["categories"] != null && catalogueJSONObj["categories"]["category"] != null)
            {
                // Fetch Category
                if (catalogueJSONObj["categories"]["category"] is JObject)
                {
                    //Single Category
                    JObject categoryJSONObj = (JObject)catalogueJSONObj["categories"]["category"];
                    Catalogue.Category catalogueCategory = new Catalogue.Category(catalogue);
                    BuildObjectFromJSONTree(catalogueCategory, categoryJSONObj);
                    // Check for Items Now
                    if (categoryJSONObj["items"] != null && categoryJSONObj["items"]["item"] != null)
                    {
                        if (categoryJSONObj["items"]["item"] is JObject)
                        {
                            //Single Item
                            JObject itemJSONObj = (JObject)categoryJSONObj["items"]["item"];
                            Catalogue.Category.Item item = new Catalogue.Category.Item(catalogueCategory);
                            BuildObjectFromJSONTree(item, itemJSONObj);
                        }
                        else
                        {
                            //Multiple Items
                            JArray categoryJSONArray = (JArray)categoryJSONObj["items"]["item"];
                            for (int j = 0; j < categoryJSONArray.Count; j++)
                            {
                                JObject itemJSONObj = (JObject)categoryJSONArray[j];
                                Catalogue.Category.Item item = new Catalogue.Category.Item(catalogueCategory);
                                BuildObjectFromJSONTree(item, itemJSONObj);
                            }
                        }
                    }

                }
                else
                {
                    //Multiple Categories
                    JArray itemJSONArray = (JArray)catalogueJSONObj["categories"]["category"];
                    for (int i = 0; i < itemJSONArray.Count; i++)
                    {
                        JObject categoryJSONObj = (JObject)itemJSONArray[i];
                        Catalogue.Category catalogueCategory = new Catalogue.Category(catalogue);
                        BuildObjectFromJSONTree(catalogueCategory, categoryJSONObj);
                        // Check for Items Now
                        if (categoryJSONObj["items"] != null && categoryJSONObj["items"]["item"] != null)
                        {
                            if (categoryJSONObj["items"]["item"] is JObject)
                            {
                                //Single Item
                                JObject itemJSONObj = (JObject)categoryJSONObj["items"]["item"];
                                Catalogue.Category.Item item = new Catalogue.Category.Item(catalogueCategory);
                                BuildObjectFromJSONTree(item, itemJSONObj);
                            }
                            else
                            {
                                //Multiple Items
                                JArray categoryJSONArray = (JArray)categoryJSONObj["items"]["item"];
                                for (int j = 0; j < categoryJSONArray.Count; j++)
                                {
                                    JObject itemJSONObj = (JObject)categoryJSONArray[j];
                                    Catalogue.Category.Item item = new Catalogue.Category.Item(catalogueCategory);
                                    BuildObjectFromJSONTree(item, itemJSONObj);
                                }
                            }
                        }
                    }

                }
            }
            return catalogue;
        }
    }
}
