using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.gallery
{
    /// <summary>
    /// AlbumResponseBuilder class converts the JSON response retrieved from the server to the value object i.e Album
    /// </summary>
    public class AlbumResponseBuilder : App42ResponseBuilder
    {
        /// <summary>
        /// Converts the response in JSON format to the value object i.e Album
        /// </summary>
        /// <param name="json">Response in JSON format</param>
        /// <returns>Album object filled with json data</returns>
        public Album BuildResponse(String json)
        {
            JObject albumsJSONObj = GetServiceJSONObject("albums", json);
            JObject albumJSONObj = (JObject)albumsJSONObj["album"];
            Album albumObj = new Album();
            albumObj.SetStrResponse(json);
            albumObj.SetResponseSuccess(IsResponseSuccess(json));
            albumObj.SetTotalRecords(GetTotalRecords(json));
            BuildObjectFromJSONTree(albumObj, albumJSONObj);
            Object obj = albumJSONObj["photos"];
            if (obj != null)
            {
                if (albumJSONObj["photos"]["photo"] != null && albumJSONObj["photos"]["photo"] is JObject)
                {
                    Album.Photo photoObj = new Album.Photo(albumObj);
                    JObject photoJsonObj = (JObject)albumJSONObj["photos"]["photo"];
                    BuildObjectFromJSONTree(photoObj, photoJsonObj);
                    photoObj = SetTagList(photoObj, photoJsonObj);
                }
                else if (albumJSONObj["photos"]["photo"] != null && albumJSONObj["photos"]["photo"] is JArray)
                {
                    JArray photoJSONArray = (JArray)albumJSONObj["photos"]["photo"];
                    for (int i = 0; i < photoJSONArray.Count(); i++)
                    {
                        JObject photoJSONObj = (JObject)photoJSONArray[i];
                        Album.Photo photoObj = new Album.Photo(albumObj);
                        BuildObjectFromJSONTree(photoObj, photoJSONObj);
                        photoObj = SetTagList(photoObj, photoJSONObj);
                    }
                }
            }
            else
            {
                return albumObj;
            }
            return albumObj;
        }
        /// <summary>
        /// Converts the Album JSON object to the value object i.e Album
        /// </summary>
        /// <param name="albumsJSONObj">Album data as JSONObject.</param>
        /// <returns>Album object filled with json data.</returns>
        private Album BuildAlbumObject(JObject albumsJSONObj)
        {
            Album albumObj = new Album();
            BuildObjectFromJSONTree(albumObj, albumsJSONObj);
            if (albumsJSONObj["photos"] != null
                 && albumsJSONObj["photos"]["photo"] != null)
            {
                if (albumsJSONObj["photos"]["photo"] is JObject)
                {
                    JObject obj1 = (JObject)albumsJSONObj["photos"]["photo"];
                    Album.Photo photoObj = new Album.Photo(albumObj);
                    BuildObjectFromJSONTree(photoObj, obj1);
                    photoObj = SetTagList(photoObj, obj1);
                }
                else
                {
                    JArray photoJSONArray = (JArray)albumsJSONObj["photos"]["photo"];
                    for (int j = 0; j < photoJSONArray.Count; j++)
                    {
                        JObject photoJSONObj = (JObject)photoJSONArray[j];

                        Album.Photo photoObj = new Album.Photo(albumObj);
                        BuildObjectFromJSONTree(photoObj, photoJSONObj);
                        photoObj = SetTagList(photoObj, photoJSONObj);
                    }
                }
            }
            return albumObj;
        }
        /// <summary>
        /// Converts the response in JSON format to the list of value objects i.e Album
        /// </summary>
        /// <param name="json">Response in JSON format.</param>
        /// <returns>List of Album object filled with json data.</returns>
        public IList<Album> BuildArrayResponse(String json)
        {
            JObject albumsJSONObj = GetServiceJSONObject("albums", json);
            IList<Album> albumList = new List<Album>();
            if (albumsJSONObj["album"] is JArray)
            {
                JArray albumJSONArray = (JArray)albumsJSONObj["album"];
                for (int i = 0; i < albumJSONArray.Count(); i++)
                {
                    JObject albumJSONObj = (JObject)albumJSONArray[i];
                    Album albumObj = BuildAlbumObject(albumJSONObj);
                    albumObj.SetStrResponse(json);
                    albumObj.SetResponseSuccess(IsResponseSuccess(json));

                    albumObj.SetTotalRecords(GetTotalRecords(json));
                    albumList.Add(albumObj);
                }
            }
            else
            {
                JObject albumJSONObj = (JObject)albumsJSONObj["album"];
                Album album = BuildAlbumObject(albumJSONObj);
                album.SetStrResponse(json);
                album.SetResponseSuccess(IsResponseSuccess(json));
                album.SetTotalRecords(GetTotalRecords(json));
                albumList.Add(album);
            }
            return albumList;
        }
        /// <summary>
        /// set tags to the list
        /// </summary>
        /// <param name="photoObj"></param>
        /// <param name="photoJsonObj"></param>
        /// <returns>Photo object</returns>
        /// <exception>Exception</exception>
        private Album.Photo SetTagList(Album.Photo photoObj, JObject photoJsonObj)
        {
            if (photoJsonObj["tags"] != null)
            {
                IList<String> tagList = new List<String>();
                if (photoJsonObj["tags"] is JArray)
                {
                    JArray tagArr = (JArray)photoJsonObj["tags"];
                    for (int i = 0; i < tagArr.Count(); i++)
                    {
                        tagList.Add(tagArr[i].ToString());
                    }
                }
                else
                {
                    tagList.Add(photoJsonObj["tags"].ToString());
                }
                photoObj.SetTagList(tagList);
            }
            return photoObj;
        }
    }
}