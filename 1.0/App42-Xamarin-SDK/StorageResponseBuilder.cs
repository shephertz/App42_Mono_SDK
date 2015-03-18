using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.storage
{
    /// <summary>
    /// StorageResponseBuilder class converts the JSON response retrieved from the server to the value object i.e Storage
    /// </summary>
    public class StorageResponseBuilder : App42ResponseBuilder
    {
        /// <summary>
        /// Converts the response in JSON format to the value object i.e Storage.
        /// </summary>
        /// <param name="json">Response in JSON format.</param>
        /// <returns>Storage object filled with json data.</returns>
        public Storage BuildResponse(String json)
        {
            Storage storageObj = new Storage();
            IList<Storage.JSONDocument> jsonDocList = new List<Storage.JSONDocument>();
            storageObj.SetJsonDocList(jsonDocList);
            storageObj.SetStrResponse(json);
            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjApp42 = (JObject)jsonObj["app42"];
            JObject jsonObjResponse = (JObject)jsonObjApp42["response"];
            storageObj.SetResponseSuccess((Boolean)jsonObjResponse["success"]);
            JObject jsonObjStorage = (JObject)jsonObjResponse["storage"];

            BuildObjectFromJSONTree(storageObj, jsonObjStorage);

            if (jsonObjStorage["jsonDoc"] == null)
                return storageObj;

            if (jsonObjStorage["jsonDoc"] != null && jsonObjStorage["jsonDoc"] is JObject)
            {
                JObject jsonObjDoc = (JObject)jsonObjStorage["jsonDoc"];
                Storage.JSONDocument document = new Storage.JSONDocument(storageObj);
                BuildJsonDocument(document, jsonObjDoc);
            }
            else
            {
                JArray jsonObjDocArray = (JArray)jsonObjStorage["jsonDoc"];
                for (int i = 0; i < jsonObjDocArray.Count(); i++)
                {
                    App42Log.Debug("jsonObjDocArray" + jsonObjDocArray);
                    JObject jsonObjDoc = (JObject)jsonObjDocArray[i];
                    Storage.JSONDocument document = new Storage.JSONDocument(storageObj);
                    BuildJsonDocument(document, jsonObjDoc);
                }
            }
            App42Log.Debug(storageObj.GetDbName() + " : " + storageObj.GetCollectionName() + " : " + storageObj.GetJsonDocList());

            return storageObj;
        }
        /// <summary>
        /// Builds the Json Document for the storage w.r.t their docId.
        /// </summary>
        /// <param name="document">Document for storage.</param>
        /// <param name="jsonObjDoc">JsonDoc object for storage.</param>
        private void BuildJsonDocument(com.shephertz.app42.paas.sdk.csharp.storage.Storage.JSONDocument document, JObject jsonObjDoc)
        {
            if (jsonObjDoc["_id"] != null)
            {

                JObject idObj = (JObject)jsonObjDoc["_id"];
                String oIdObj = "" + idObj["$oid"];
                document.SetDocId(oIdObj);

                jsonObjDoc.Remove("_id");
                document.SetJsonDoc(jsonObjDoc.ToString());
            }

        }
        /// <summary>
        /// Main method creating a new Storage and JSONDocument object resulting int a response that has to be displayed.
        /// </summary>
        /// <param name="args"></param>
        public static void main(String[] args)
        {
            Storage storage = new Storage();
            Storage.JSONDocument doc = new Storage.JSONDocument(storage);
            new StorageResponseBuilder().BuildResponse("{\"app42\":{\"response\":{\"success\":true,\"storage\":{\"dbName\":\"db\",\"collectionName\":\"col\",\"jsonDoc\":[{\"_id\":{\"$oid\":\"4f6af8b56cba3551222b5db8\"},\"nae\":\"test\"},{\"_id\":{\"$oid\":\"4f6af8fb6cba3551222b5db9\"},\"nae\":\"test\"},{\"_id\":{\"$oid\":\"4f6af9b66cba3551222b5dba\"},\"nae\":\"test\"}]}}}}");
        }
    }
}