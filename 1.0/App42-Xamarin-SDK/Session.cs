using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.session
{
    /// <summary>
    /// This Session object is the value object which contains the properties of
    /// Session along with the setter & getter for those properties.
    /// </summary>
    public class Session : App42Response
    {

        public String userName;
        public String sessionId;
        public DateTime createdOn;
        public DateTime invalidatedOn;
        public IList<Attribute> attributeList = new List<Session.Attribute>();
        /// <summary>
        /// Returns the userName for the session.
        /// </summary>
        /// <returns>The userName.</returns>
        public String GetUserName()
        {
            return userName;
        }
        /// <summary>
        /// Sets the user name for the session.
        /// </summary>
        /// <param name="userName">UserName of the session.</param>
        public void SetUserName(String userName)
        {
            this.userName = userName;
        }
        /// <summary>
        /// Returns the sessionId for the session.
        /// </summary>
        /// <returns>The sessionId information.</returns>
        public String GetSessionId()
        {
            return sessionId;
        }
        /// <summary>
        /// Sets the session Id for the session.
        /// </summary>
        /// <param name="sessionId">SessionId of the session.</param>
        public void SetSessionId(String sessionId)
        {
            this.sessionId = sessionId;
        }
        /// <summary>
        /// Returns the time, date and day the session was created on.
        /// </summary>
        /// <returns>The createdOn information.</returns>
        public DateTime GetCreatedOn()
        {
            return createdOn;
        }
        /// <summary>
        /// Sets the createdOn for the session.
        /// </summary>
        /// <param name="createdOn">Session information on when it was created</param>
        public void SetCreatedOn(DateTime createdOn)
        {
            this.createdOn = createdOn;
        }
        /// <summary>
        /// Returns the invalidatedOn information for the session.
        /// </summary>
        /// <returns>The invalidatedOn information.</returns>
        public DateTime GetInvalidatedOn()
        {
            return invalidatedOn;
        }
        /// <summary>
        /// Sets the invalidatedOn for the session.
        /// </summary>
        /// <param name="invalidatedOn">InvalidatedOn of the session.</param>
        public void SetInvalidatedOn(DateTime invalidatedOn)
        {
            this.invalidatedOn = invalidatedOn;
        }
        /// <summary>
        /// Returns the List of the Attributed for the Session.
        /// </summary>
        /// <returns>The attributeList information.</returns>
        public IList<Attribute> GetAttributeList()
        {
            return attributeList;
        }
        /// <summary>
        /// Sets the user name for the Session.
        /// </summary>
        /// <param name="attributeList">AttributeList of the Session.</param>
        public void SetAttributeList(IList<Attribute> attributeList)
        {
            this.attributeList = attributeList;
        }
        /// <summary>
        /// An inner class that contains the remaining properties of the Session.
        /// </summary>
        public class Attribute
        {
            /// <summary>
            /// This is a constructor.
            /// </summary>
            /// <param name="session"></param>
            public Attribute(Session session)
            {
                session.attributeList.Add(this);
            }
            public String name;
            public String value;

            /// <summary>
            /// Returns the name of the attribute.
            /// </summary>
            /// <returns>The name of the attribute.</returns>
            public String GetName()
            {
                return name;
            }

            /// <summary>
            /// Sets the name for the attribute.
            /// </summary>
            /// <param name="name">Name of the attribute.</param>
            public void SetName(String name)
            {
                this.name = name;
            }

            /// <summary>
            /// Returns the value of the session.
            /// </summary>
            /// <returns>The value of the session.</returns>
            public String GetValue()
            {
                return value;
            }

            /// <summary>
            /// Sets the value for the session.
            /// </summary>
            /// <param name="value">Value of the session.</param>
            public void SetValue(String value)
            {
                this.value = value;
            }
        }
    }
}