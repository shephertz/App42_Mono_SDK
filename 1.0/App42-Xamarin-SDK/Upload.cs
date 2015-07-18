using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.upload
{
    /// <summary>
    /// This Upload object is the value object which contains the properties of
    /// Upload along with the setter & getter for those properties.
    /// </summary>
    public class Upload : App42Response
    {

        IList<File> fileList = new List<File>();
        /// <summary>
        /// Returns the list of all the files.
        /// </summary>
        /// <returns>The list of files.</returns>
        public IList<File> GetFileList()
        {
            return fileList;
        }
        /// <summary>
        /// Sets the list of files.
        /// </summary>
        /// <param name="fileList">List of all the files</param>
        public void SetFileList(IList<File> fileList)
        {
            this.fileList = fileList;
        }
        /// <summary>
        /// An inner class that contains the remaining properties of Upload.
        /// </summary>
        public class File
        {

            public File(Upload upload)
            {
                upload.fileList.Add(this);
            }

            public String name;
            public String type;
            public String url;
            public String tinyUrl;
            public String userName;
            public String description;
            /// <summary>
            /// Returns the name of the File.
            /// </summary>
            /// <returns>The name of the file.</returns>
            public String GetName()
            {
                return name;
            }
            /// <summary>
            /// Sets the name of the file.
            /// </summary>
            /// <param name="name">Name of the file.</param>
            public void SetName(String name)
            {
                this.name = name;
            }
            /// <summary>
            /// Returns the name of the User.
            /// </summary>
            /// <returns>The name of the user.</returns>
            public String GetUserName()
            {
                return userName;
            }
            /// <summary>
            /// Sets the name of the User for upload file.
            /// </summary>
            /// <param name="userName">Name of the User for which file has to be saved.</param>
            public void SetUserName(String userName)
            {
                this.userName = userName;
            }

            #warning this one hides .net default method .GetType()!
            /// <summary>
            /// Returns the type of the Upload File.
            /// </summary>
            /// <returns>The type of the Upload File.</returns>
            public String GetType()
            {
                return type;
            }
            /// <summary>
            /// Sets the type of the file to be uploaded.

            /// </summary>
            /// <param name="type">The type of the file. File can be either Audio, Video,
            /// Image, csv or other Use the static constants e.g.
            /// Upload.AUDIO, Upload.XML etc.
            /// </param>
            public void SetType(String type)
            {
                this.type = type;
            }
            /// <summary>
            /// Returns the url of the Upload File.
            /// </summary>
            /// <returns>The url of the Upload File.</returns>
            public String GetUrl()
            {
                return url;
            }
            /// <summary>
            /// Url of the upload file.
            /// </summary>
            /// <param name="url">Url of the file which has to be uploaded.</param>
            public void SetUrl(String url)
            {
                this.url = url;
            }
            /// <summary>
            /// Returns the tinyUrl of the Upload File.
            /// </summary>
            /// <returns>The tinyUrl of the Upload File.</returns>
            public String GetTinyUrl()
            {
                return tinyUrl;
            }
            /// <summary>
            /// TinyUrl of the upload file.
            /// </summary>
            /// <param name="tinyUrl">TinyUrl of the file which has to be uploaded.</param>
            public void SetTinyUrl(String tinyUrl)
            {
                this.tinyUrl = tinyUrl;
            }
            /// <summary>
            /// Returns the description of the Upload File.
            /// </summary>
            /// <returns>The description of the Upload File.</returns>
            public String GetDescription()
            {
                return description;
            }
            /// <summary>
            /// Sets the description of the upload file.
            /// </summary>
            /// <param name="description">Description of the file to be uploaded.</param>
            public void SetDescription(String description)
            {
                this.description = description;
            }
        }
    }
}