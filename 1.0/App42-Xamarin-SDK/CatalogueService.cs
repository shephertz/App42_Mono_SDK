
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using Newtonsoft.Json;
using System.IO;
using com.shephertz.app42.paas.sdk.csharp.connection;
using com.shephertz.app42.paas.sdk.csharp;


namespace com.shephertz.app42.paas.sdk.csharp.shopping
{
    /// <summary>
    /// This Service provides a complete cloud based catalogue management. An app can keep
    /// all its items based on category on the Cloud. This service provides several utility
    /// methods to manage catalogue on the cloud.
    /// 
    /// One can add items with its related information in a particular category. And there can 
    /// be several categories in a catalogue. The App developer can create several catalogues if needed.
    /// 
    /// The Cart service can be used along with Catalogue service to create an end to end Shopping feature for
    /// a Mobile and Web App
    ///  <see cref="CartService">Cart</see>
    ///  <see cref="CartService">ItemData</see>
    /// </summary>

    public class CatalogueService
    {
        Config config;
        private String version = "1.0";
        private String resource = "catalogue";
        private String apiKey;
        private String secretKey;
        String baseURL;

        /// <summary>
        /// Constructor that takes
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="baseURL"></param>
        public CatalogueService(String apiKey, String secretKey, String baseURL)
        {
            config = Config.GetInstance();
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;
        }
        /// <summary>
        /// Creates a Catalogue for a particular App. Categories can be added to the Catalogue
        /// </summary>
        /// <param name="catalogueName"> Name of the Catalogue to be created.</param>
        /// <param name="catalogueDescription"> Description of the catalogue to be created.</param>
        /// <returns>Catalogue object</returns>
        /// <exception>App42Exception</exception>

        public Catalogue CreateCatalogue(String catalogueName, String catalogueDescription)
        {

            String response = null;
            Catalogue catalogue = null;
            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Util.ThrowExceptionIfNullOrBlank(catalogueDescription, "CatalogueDescription");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            // Put these params for signing
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            // Construct a json body for Create catalogue
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(catalogueName);
            jsonWriter.WritePropertyName("description");
            jsonWriter.WriteValue(catalogueDescription);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"catalogue\":").Append(sbJson.ToString()).Append("}}");

            paramsDics.Add("body", sb.ToString());

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            catalogue = new CatalogueResponseBuilder().BuildResponse(response);
            return catalogue;

        }
        /// <summary>
        /// Creates a Category for a particular Catalogue e.g. Books, Music etc.
        /// </summary>
        /// <param name="catalogueName"> Name of the Catalogue for which Category has to be created.</param>
        /// <param name="categoryName"> Name of the Category that has to be created.</param>
        /// <param name="categoryDescription"> Description of the category to be created.</param>
        /// <returns>Catalogue object containing created category information.</returns>
        /// <exception>App42Exception</exception>
        public Catalogue CreateCategory(String catalogueName, String categoryName,
                String categoryDescription)
        {
            String response = null;
            Catalogue catalogue = null;

            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Util.ThrowExceptionIfNullOrBlank(categoryName, "CategoryName");
            Util.ThrowExceptionIfNullOrBlank(categoryDescription, "CategoryDescription");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());

            // Put these params for signing
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("catalogueName", catalogueName);

            // Construct a json body for Create catagory

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(categoryName);
            jsonWriter.WritePropertyName("description");
            jsonWriter.WriteValue(categoryDescription);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"catalogue\":{\"categories\":{\"category\":").Append(sbJson).Append("}}}}");

            paramsDics.Add("body", sb.ToString());

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + catalogueName + "/category";

            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            catalogue = new CatalogueResponseBuilder().BuildResponse(response);
            return catalogue;

        }
        /// <summary>
        /// Creates a Item in a Category for a particular Catelogue
        /// @see ItemData
        /// </summary>
        /// <param name="catalogueName"> Name of the Catalogue to which item has to be added.</param>
        /// <param name="categoryName"> Name of the Category to which item has to be added.</param>
        /// <param name="itemData"> Item Information that has to be added.</param>
        /// <returns>Catalogue object containing added item.</returns>
        /// <exception>App42Exception</exception>

        public Catalogue AddItem(String catalogueName, String categoryName, ItemData itemData)
        {
            String response = null;
            Catalogue catalogue = null;

            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Util.ThrowExceptionIfNullOrBlank(categoryName, "CategoryName");
            Util.ThrowExceptionIfNullOrBlank(itemData, "ItemData");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            paramsDics.Add("catalogueName", catalogueName);
            paramsDics.Add("categoryName", categoryName);
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("itemId", itemData.GetItemId());
            paramsDics.Add("name", itemData.GetName());
            paramsDics.Add("description", itemData.GetDescription());
            paramsDics.Add("price", "" + itemData.GetPrice());
            Dictionary<String, String> postParams = new Dictionary<String, String>(paramsDics);

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);

            String resourceUrl = Config.GetInstance().GetBaseURL() + version + "/" + resource + "/" + catalogueName + "/" + categoryName
                    + "/item";
            Stream stream = null;
            stream = itemData.GetImageStream();
            String file = itemData.GetImage();

            if (stream != null)
            {
                response = Util.MultiPartRequest("imageFile", stream, "", queryParams,
                        postParams, resourceUrl, Config.GetInstance().GetAccept());
            }
            else
            {
                response = Util.MultiPartRequest("imageFile", file, queryParams,
                        postParams, resourceUrl, Config.GetInstance().GetAccept());
            }
            catalogue = new CatalogueResponseBuilder().BuildResponse(response);
            return catalogue;
        }
        /// <summary>
        /// Fetches all items for a Catalogue.
        /// </summary>
        /// <param name="catalogueName"> Name of the Catalogue from which item has to be fetched.</param>
        /// <returns>Catalogue object containing all Items.</returns>
        /// <exception>App42Exception</exception>
        public Catalogue GetItems(String catalogueName)
        {

            String response = null;
            Catalogue catalogue = null;
            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("catalogueName", catalogueName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + catalogueName;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            catalogue = new CatalogueResponseBuilder().BuildResponse(response);
            return catalogue;
        }
        /// <summary>
        /// Fetches all items for a Catalogue and Category.
        /// </summary>
        /// <param name="catalogueName"> Name of the Catalogue from which item has to be fetched.</param>
        /// <param name="categoryName"> Name of the Category from which item has to be fetched.</param>
        /// <returns>Catalogue object</returns>
        /// <exception>App42Exception</exception>
        public Catalogue GetItemsByCategory(String catalogueName, String categoryName)
        {
            String response = null;
            Catalogue catalogue = null;

            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Util.ThrowExceptionIfNullOrBlank(categoryName, "CategoryName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("catalogueName", catalogueName);
            paramsDics.Add("categoryName", categoryName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + catalogueName + "/" + categoryName;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            catalogue = new CatalogueResponseBuilder().BuildResponse(response);
            return catalogue;
        }
        /// <summary>
        /// Fetches Item by id for a Catalogue and Category
        /// </summary>
        /// <param name="catalogueName"> Name of the Catalogue from which item has to be fetched.</param>
        /// <param name="categoryName"> Name of the Category from which item has to be fetched.</param>
        /// <param name="itemId"> Item id for which information has to be fetched.</param>
        /// <returns>Catalogue object</returns>
        /// <exception>App42Exception</exception>
        public Catalogue GetItemById(String catalogueName, String categoryName,
                String itemId)
        {
            String response = null;
            Catalogue catalogue = null;

            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Util.ThrowExceptionIfNullOrBlank(categoryName, "CategoryName");
            Util.ThrowExceptionIfNullOrBlank(itemId, "itemId");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("catalogueName", catalogueName);
            paramsDics.Add("categoryName", categoryName);
            paramsDics.Add("itemId", itemId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + catalogueName + "/" + categoryName + "/" + itemId;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            catalogue = new CatalogueResponseBuilder().BuildResponse(response);
            return catalogue;
        }
        /// <summary>
        /// Removes all Items in a Catalogue
        /// </summary>
        /// <param name="catalogueName"> Name of the Catalogue from which item has to be removed.</param>
        /// <returns>Catalogue object containing removed items.</returns>
        /// <exception>App42Exception</exception>
        public App42Response RemoveAllItems(String catalogueName)
        {
            String response = null;
            App42Response responseObj = new App42Response();

            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("catalogueName", catalogueName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + catalogueName;

            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Removes all Items from a Catalogue and Category
        /// </summary>
        /// <param name="catalogueName"> Name of the Catalogue from which item has to be removed.</param>
        /// <param name="categoryName"> Name of the Category from which item has to be removed.</param>
        /// <returns>Catalogue object containing removed item.</returns>
        /// <exception>App42Exception</exception>
        public App42Response RemoveItemsByCategory(String catalogueName,
                String categoryName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Util.ThrowExceptionIfNullOrBlank(categoryName, "CategoryName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("catalogueName", catalogueName);
            paramsDics.Add("categoryName", categoryName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + catalogueName + "/" + categoryName;

            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Removes Item by Id
        /// </summary>
        /// <param name="catalogueName"> Name of the Catalogue from which item has to be removed</param>
        /// <param name="categoryName"> Name of the Category from which item has to be removed</param>
        /// <param name="itemId"> Item id which has to be removed</param>
        /// <returns>Catalogue object containing removed item.</returns>
        /// <exception>App42Exception</exception>
        public App42Response RemoveItemById(String catalogueName, String categoryName,
                String itemId)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Util.ThrowExceptionIfNullOrBlank(categoryName, "CategoryName");
            Util.ThrowExceptionIfNullOrBlank(itemId, "ItemId");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("catalogueName", catalogueName);
            paramsDics.Add("categoryName", categoryName);
            paramsDics.Add("itemId", itemId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + catalogueName + "/" + categoryName + "/" + itemId;

            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }

        /// <summary>
        /// Fetches all items for a Catalogue and Category by paging.
        /// </summary>
        /// <param name="catalogueName">Name of the Catalogue from which item has to be fetched</param>
        /// <param name="categoryName">Name of the Category from which item has to be fetched</param> 
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>

        /// <returns>Catalogue object</returns>
        /// <exception>App42Exception</exception>
        public Catalogue GetItemsByCategory(String catalogueName, String categoryName, int max, int offset)
        {
            String response = null;
            Catalogue catalogue = null;
            Util.ValidateMax(max);
            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Util.ThrowExceptionIfNullOrBlank(categoryName, "CategoryName");
            Util.ThrowExceptionIfNullOrBlank(max, "Max");
            Util.ThrowExceptionIfNullOrBlank(offset, "Offset");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("catalogueName", catalogueName);
            paramsDics.Add("categoryName", categoryName);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "paging" + "/" + catalogueName + "/" + categoryName + "/"
                    + max + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            catalogue = new CatalogueResponseBuilder().BuildResponse(response);
            return catalogue;
        }
        /// <summary>
        /// Fetches count of all items for a Catalogue and Category
        /// </summary>
        /// <param name="catalogueName">Name of the Catalogue from which count of item has to be fetched</param>
        /// <param name="categoryName">Name of the Category from which count of item has to be fetched</param>
        /// <returns>App42Response object</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetItemsCountByCategory(String catalogueName,
                String categoryName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(catalogueName, "CatalogueName");
            Util.ThrowExceptionIfNullOrBlank(categoryName, "CategoryName");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("catalogueName", catalogueName);
            paramsDics.Add("categoryName", categoryName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + catalogueName + "/" + categoryName + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new CatalogueResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
    }
}