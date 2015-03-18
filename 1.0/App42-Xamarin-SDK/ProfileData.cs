using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.user
{
    public class ProfileData
    {
        private String firstName;
        private String lastName;
        private String sex;
        private String mobile;
        private String line1;
        private String line2;
        private String city;
        private String state;
        private String country;
        private String pincode;
        private String homeLandLine;
        private String officeLandLine;
        private DateTime dateOfBirth;
        public String GetFirstName()
        {
            return firstName;
        }
        public void SetFirstName(String firstName)
        {
            this.firstName = firstName;
        }
        public String GetLastName()
        {
            return lastName;
        }
        public void SetLastName(String lastName)
        {
            this.lastName = lastName;
        }
        public String GetSex()
        {
            return sex;
        }
        public void SetSex(String userGender)
        {
            if(!UserGender.MALE.ToString().Equals(userGender) && !UserGender.FEMALE.ToString().Equals(userGender)) {
                throw new App42Exception("Invalid User Gender. Could be either Male or Female");
            }
            this.sex = userGender;
        }
        public String GetMobile()
        {
            return mobile;
        }
        public void SetMobile(String mobile)
        {
            this.mobile = mobile;
        }
        public String GetLine1()
        {
            return line1;
        }
        public void SetLine1(String line1)
        {
            this.line1 = line1;
        }
        public String GetLine2()
        {
            return line2;
        }
        public void SetLine2(String line2)
        {
            this.line2 = line2;
        }
        public String GetCity()
        {
            return city;
        }
        public void SetCity(String city)
        {
            this.city = city;
        }
        public String GetState()
        {
            return state;
        }
        public void SetState(String state)
        {
            this.state = state;
        }
        public String GetCountry()
        {
            return country;
        }
        public void SetCountry(String country)
        {
            this.country = country;
        }
        public String GetPincode()
        {
            return pincode;
        }
        public void SetPincode(String pincode)
        {
            this.pincode = pincode;
        }
        public String GetHomeLandLine()
        {
            return homeLandLine;
        }
        public void SetHomeLandLine(String homeLandLine)
        {
            this.homeLandLine = homeLandLine;
        }
        public String GetOfficeLandLine()
        {
            return officeLandLine;
        }
        public void SetOfficeLandLine(String officeLandLine)
        {
            this.officeLandLine = officeLandLine;
        }
        public DateTime GetDateOfBirth()
        {
            return dateOfBirth;
        }
        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            this.dateOfBirth = dateOfBirth;
        }
    }
}
