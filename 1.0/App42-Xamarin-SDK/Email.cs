using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.email
{
    public class Email : App42Response
    {
        public String from;
        public String to;
        public String subject;
        public String body;

        public IList<Email.Configuration> configList = new List<Email.Configuration>();

        public String GetFrom()
        {
            return from;
        }
        public void SetFrom(String from)
        {
            this.from = from;
        }
        public String GetTo()
        {
            return to;
        }
        public void SetTo(String to)
        {
            this.to = to;
        }
        public String GetSubject()
        {
            return subject;
        }
        public void SetSubject(String subject)
        {
            this.subject = subject;
        }
        public String GetBody()
        {
            return body;
        }
        public void SetBody(String body)
        {
            this.body = body;
        }
        public IList<Email.Configuration> GetConfigList()
        {
            return configList;
        }
        public void SetConfigList(IList<Email.Configuration> configList)
        {
            this.configList = configList;
        }

        public class Configuration
        {

            public String emailId;
            public String host;
            public Int32  port;
            public Boolean ssl;

            public Configuration(Email email)
            {
                email.configList.Add(this);
            }

            public String GetEmailId()
            {
                return emailId;
            }

            public void SetEmailId(String emailId)
            {
                this.emailId = emailId;
            }

            public String GetHost()
            {
                return host;
            }

            public void SetHost(String host)
            {
                this.host = host;
            }

            public Int32 GetPort()
            {
                return port;
            }

            public void SetPort(Int32 port)
            {
                this.port = port;
            }

            public Boolean GetSsl()
            {
                return ssl;
            }

            public void SetSsl(Boolean ssl)
            {
                this.ssl = ssl;
            }

            public override String ToString()
            {
                return "Email : " + emailId + " : Host  : " + host + " : port : " + port + " : ssl : " + ssl;
            }
        }
    }
}