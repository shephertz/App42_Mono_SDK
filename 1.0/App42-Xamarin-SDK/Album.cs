using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.gallery
{
    /// <summary>
    ///  This Album object is the value object which contains the properties of Album along with the setter & getter for those properties.
    /// </summary>
    public class Album : App42Response
    {
        public String userName;
        public String name;
        public String description;
        public IList<Photo> photoList = new List<Photo>();
        /// <summary>
        /// Returns the user name of the album.
        /// </summary>
        /// <returns>The user name of the album.</returns>
        public String GetUserName()
        {
            return userName;
        }
        /// <summary>
        /// Sets the user name of the Album.
        /// </summary>
        /// <param name="userName">User Name of the Album.</param>
        public void SetUserName(String userName)
        {
            this.userName = userName;
        }
        /// <summary>
        /// Returns the name of the Album.
        /// </summary>
        /// <returns>The name of the Album.</returns>
        public String GetName()
        {
            return name;
        }
        /// <summary>
        /// Sets the name of the Album which has to be created.
        /// </summary>
        /// <param name="name">Name of the Album that has to be created.</param>
        public void SetName(String name)
        {
            this.name = name;
        }
        /// <summary>
        /// Returns the description of the Album.
        /// </summary>
        /// <returns>The description of the Album.</returns>
        public String GetDescription()
        {
            return description;
        }
        /// <summary>
        /// Sets the description of the Album.
        /// </summary>
        /// <param name="description">Description of the Album.</param>
        public void SetDescription(String description)
        {
            this.description = description;
        }
        /// <summary>
        /// Returns the list of all the photos for the Album.
        /// </summary>
        /// <returns>The list of all the photos for the Album.</returns>
        public IList<Photo> GetPhotoList()
        {
            return photoList;
        }
        /// <summary>
        /// Sets the list of all the photos for the Album.
        /// </summary>
        /// <param name="photoList">List of all the photos for the Album.</param>
        public void SetPhotoList(IList<Photo> photoList)
        {
            this.photoList = photoList;
        }
        /// <summary>
        /// An inner class that contains the remaining properties of the Album.
        /// </summary>
        public class Photo
        {
            /// <summary>
            /// This is a constructor.
            /// </summary>
            /// <param name="album"></param>
            public Photo(Album album)
            {
                album.photoList.Add(this);
            }
            public String name;
            public String description;
            public String url;
            public String tinyUrl;
            public String thumbNailUrl;
            public String thumbNailTinyUrl;
            public IList<String> tagList = new List<String>();
            /// <summary>
            /// Returns the name of the photo.
            /// </summary>
            /// <returns>The name of the photo.</returns>
            public String GetName()
            {
                return name;
            }
            /// <summary>
            /// Sets the name of the photo.
            /// </summary>
            /// <param name="name">Name of the photo.</param>
            public void SetName(String name)
            {
                this.name = name;
            }
            /// <summary>
            /// Returns the description of the photo.
            /// </summary>
            /// <returns>The description of the photo.</returns>
            public String GetDescription()
            {
                return description;
            }
            /// <summary>
            /// Sets the description of the photo.
            /// </summary>
            /// <param name="description">Description of the photo.</param>
            public void SetDescription(String description)
            {
                this.description = description;
            }
            /// <summary>
            /// Returns the url of the photo.
            /// </summary>
            /// <returns>The url of the photo.</returns>
            public String GetUrl()
            {
                return url;
            }
            /// <summary>
            /// Sets the url of the photo.
            /// </summary>
            /// <param name="url">Url of the photo.</param>
            public void SetUrl(String url)
            {
                this.url = url;
            }
            /// <summary>
            /// Returns the list of all the tags in the photo.
            /// </summary>
            /// <returns>The list of all the tags in the photo.</returns>
            public IList<String> GetTagList()
            {
                return tagList;
            }
            /// <summary>
            /// Sets the list of all the tags in the photo.
            /// </summary>
            /// <param name="tagList">List of all the tags in the photo.</param>
            public void SetTagList(IList<String> tagList)
            {
                this.tagList = tagList;
            }
            /// <summary>
            /// Returns the tiny url of the photo.
            /// </summary>
            /// <returns>The tiny url of the photo.</returns>
            public String GetTinyUrl()
            {
                return tinyUrl;
            }
            /// <summary>
            /// Sets the tiny url of the photo.
            /// </summary>
            /// <param name="tinyUrl">Tinyurl of the photo.</param>
            public void SetTinyUrl(String tinyUrl)
            {
                this.tinyUrl = tinyUrl;
            }
            /// <summary>
            /// Returns the thumbnail url of the photo.
            /// </summary>
            /// <returns>The thumbnail url of the photo.</returns>
            public String GetThumbNailUrl()
            {
                return thumbNailUrl;
            }
            /// <summary>
            /// Sets the thumbnail url of the photo.
            /// </summary>
            /// <param name="thumbNailUrl">Thumbnail url of the photo.</param>
            public void SetThumbNailUrl(String thumbNailUrl)
            {
                this.thumbNailUrl = thumbNailUrl;
            }
            /// <summary>
            /// Returns the thumbnail tiny url of the photo.
            /// </summary>
            /// <returns>The thumbnail tiny url of the photo.</returns>
            public String GetThumbNailTinyUrl()
            {
                return thumbNailTinyUrl;
            }
            /// <summary>
            /// Sets the thumbnail tiny url of the photo.
            /// </summary>
            /// <param name="thumbNailTinyUrl">Thumbnail tiny url of the photo.</param>
            public void SetThumbNailTinyUrl(String thumbNailTinyUrl)
            {
                this.thumbNailTinyUrl = thumbNailTinyUrl;
            }
            /// <summary>
            /// Returns the Album Response in JSON format.
            /// </summary>
            /// <returns>The response in JSON format.</returns>
            public String ToString()
            {
                return " name : " + name + " : description : " + description + " : url : " + url + " : tinyUrl : " + tinyUrl + " : thumbNailUrl : " + thumbNailUrl + " :  thumbNailTinyUrl  : " + thumbNailTinyUrl + " : tagList : " + tagList;
            }
        }
    }
}