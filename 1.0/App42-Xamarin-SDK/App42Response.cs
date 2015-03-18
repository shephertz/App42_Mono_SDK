using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp
{
    public class App42Response
    {
        private bool responseSuccess;

        private String strResponse;

        private int totalRecords = -1;

        public int GetTotalRecords()
        {
            return totalRecords;
        }

        public void SetTotalRecords(int totalRecords)
        {
            this.totalRecords = totalRecords;
        }


        public String GetStrResponse()
        {
            return strResponse;
        }

        public void SetStrResponse(String strResponse)
        {
            this.strResponse = strResponse;
        }

        public bool IsResponseSuccess()
        {
            return responseSuccess;
        }

        public void SetResponseSuccess(bool IsResponseSuccess)
        {
            this.responseSuccess = IsResponseSuccess;
        }

        public override String ToString()
        {
            return strResponse;
        }
    }
}