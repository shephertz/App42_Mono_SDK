using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.storage
{
    /// <summary>
    /// This Storage object is the value object which contains the properties of Storage along with the setter & getter for those properties.
    /// </summary>
    public class Storage : App42Response
    {

        public String dbName;
        public String collectionName;
        public IList<JSONDocument> jsonDocList = new List<JSONDocument>();

        /// <summary>
        /// Returns the name of the database.
        /// </summary>
        /// <returns>The name of the database.</returns>
        public String GetDbName()
        {
            return dbName;
        }
        /// <summary>
        /// Sets the name of the database.
        /// </summary>
        /// <param name="dbName">Database name for storage json document.</param>
        public void SetDbName(String dbName)
        {
            this.dbName = dbName;
        }
        /// <summary>
        /// Returns the collection name of the storage.
        /// </summary>
        /// <returns>Collection name of storage.</returns>
        public String GetCollectionName()
        {
            return collectionName;
        }
        /// <summary>
        /// Sets the collection name of storage.
        /// </summary>
        /// <param name="collectionName">Collection name of storage.</param>
        public void SetCollectionName(String collectionName)
        {
            this.collectionName = collectionName;
        }
        /// <summary>
        /// Returns the json document list of storage.
        /// </summary>
        /// <returns>Json document list of storage.</returns>
        public IList<JSONDocument> GetJsonDocList()
        {
            return jsonDocList;
        }
        /// <summary>
        /// Sets the Json doc list.
        /// </summary>
        /// <param name="jsonDocList">Json document list of the storage.</param>
        public void SetJsonDocList(IList<JSONDocument> jsonDocList)
        {
            this.jsonDocList = jsonDocList;
        }
        /// <summary>
        /// An inner class that contains the remaining properties of the Storage.
        /// </summary>
        public class JSONDocument
        {

            public String jsonDoc;
            public String docId;
            /// <summary>
            /// This create the constructor.
            /// </summary>
            /// <param name="storage"></param>
            public JSONDocument(Storage storage)
            {
                storage.jsonDocList.Add(this);
            }
            /// <summary>
            /// Returns the json doc for Storage.
            /// </summary>
            /// <returns>Json doc for storage.</returns>
            public String GetJsonDoc()
            {
                return jsonDoc;
            }
            /// <summary>
            /// Sets the json document for Storage
            /// </summary>
            /// <param name="jsonDoc">Json document for storage.</param>
            public void SetJsonDoc(String jsonDoc)
            {
                this.jsonDoc = jsonDoc;
            }
            /// <summary>
            /// Returns the document Id.
            /// </summary>
            /// <returns>DocId.</returns>
            public String GetDocId()
            {
                return docId;
            }
            /// <summary>
            /// Sets the document Id for the Storage
            /// </summary>
            /// <param name="docId">Document Id for the storage.</param>
            public void SetDocId(String docId)
            {
                this.docId = docId;
            }
            /// <summary>
            /// Returns the Storage Response in JSON format.
            /// </summary>
            /// <returns>The response in JSON format.</returns>
            public override String ToString()
            {
                if (this.docId != null && this.jsonDoc != null)
                    return this.docId + " : " + this.jsonDoc;
                else
                    return base.ToString();
            }
        }
    }
}