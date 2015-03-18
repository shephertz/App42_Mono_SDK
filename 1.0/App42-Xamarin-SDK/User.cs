using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace com.shephertz.app42.paas.sdk.csharp.user
{
    /// <summary>
    /// This User object is the value object which contains the properties of User
    /// along with the setter & getter for those properties.
    /// </summary>
    public class User : App42Response
    {
        public String userName;
        public String password;
        public String email;
        private Profile profile;
        public Boolean accountLocked;
        public IList<String> roleList = new List<String>();
        /// <summary>
        /// Returns the roles assigned to the User
        /// </summary>
        /// <returns>List of the roles assigned to the User</returns>
        public IList<String> GetRoleList()
        {
            return roleList;
        }
        /// <summary>
        /// Assigns the list of roles to the User
        /// </summary>
        /// <param name="roleList">List of roles to be assigned to User</param>
        public void SetRoleList(IList<String> roleList)
        {
            this.roleList = roleList;
        }
        /// <summary>
        /// Returns the User.Profile object for the User.
        /// </summary>
        /// <returns>Profile of the User</returns>
        public Profile GetProfile()
        {
            return profile;
        }
        /// <summary>
        /// Sets the User. Profile object for the User.
        /// </summary>
        /// <param name="profile">Profile of the User</param>

        public void SetProfile(User.Profile profile)
        {
            this.profile = profile;
        }
        /// <summary>
        /// Returns the User's account status.
        /// </summary>
        /// <returns>true if account is locked, false if account is unlocked.</returns>
        public Boolean IsAccountLocked()
        {
            return accountLocked;
        }
        /// <summary>
        /// Sets the value of account to either true or false.
        /// </summary>
        /// <param name="accountLocked">True or false</param>
        public void SetAccountLocked(Boolean accountLocked)
        {
            this.accountLocked = accountLocked;
        }
        /// <summary>
        /// Returns the name of the User.
        /// </summary>
        /// <returns>The name of the User.</returns>
        public String GetUserName()
        {
            return userName;
        }
        /// <summary>
        /// Sets the name of the User.
        /// </summary>
        /// <param name="userName">Name of the User</param>
        public void SetUserName(String userName)
        {
            this.userName = userName;
        }
        /// <summary>
        /// Returns the password of the User.
        /// </summary>
        /// <returns>The password of the User.</returns>
        public String GetPassword()
        {
            return password;
        }
        /// <summary>
        /// Sets the password for the User.
        /// </summary>
        /// <param name="password">Password for the User</param>
        public void SetPassword(String password)
        {
            this.password = password;
        }
        /// <summary>
        /// Returns the email of the User.
        /// </summary>
        /// <returns>The email of the User.</returns>
        public String GetEmail()
        {
            return email;
        }
        /// <summary>
        /// Sets the Email of the User.
        /// </summary>
        /// <param name="email">Email of the User</param>
        public void SetEmail(String email)
        {
            this.email = email;
        }


        public class Profile
        {
            public Profile(User user)
            {
                user.profile = this;
            }

            public String firstName;
            public String lastName;
            public String sex;
            public String mobile;
            public String line1;
            public String line2;
            public String city;
            public String state;
            public String country;
            public String pincode;
            public String homeLandLine;
            public String officeLandLine;
            public DateTime dateOfBirth;
            /// <summary>
            /// Returns the gender of the User.
            /// </summary>
            /// <returns>The gender of the User.</returns>
            public String GetSex()
            {
                return sex;
            }
            /// <summary>
            /// Sets the gender of the User.
            /// </summary>
            /// <param name="sex">Gender of the User</param>
            public void SetSex(String sex)
            {
                this.sex = sex;
            }
            /// <summary>
            /// Returns the first name of the User.
            /// </summary>
            /// <returns>The first name of the User.</returns>          
            public String GetFirstName()
            {
                return firstName;
            }
            /// <summary>
            /// Sets the first name of the User.
            /// </summary>
            /// <param name="sex">FirstName of the User</param>
            public void SetFirstName(String firstName)
            {
                this.firstName = firstName;
            }
            /// <summary>
            /// Returns the last name of the User.
            /// </summary>
            /// <returns>The last name of the User.</returns>
            public String GetLastName()
            {
                return lastName;
            }
            /// <summary>
            /// Sets the last name of the User.
            /// </summary>
            /// <param name="lastName">LastName of the User</param>
            public void SetLastName(String lastName)
            {
                this.lastName = lastName;
            }

            /// <summary>
            /// Returns the mobile number of the User.
            /// </summary>
            /// <returns>The mobile of the User.</returns>
            public String GetMobile()
            {
                return mobile;
            }
            /// <summary>
            /// Sets the mobile number for the User. 
            /// </summary>
            /// <param name="mobile">Mobile of the User</param>
            public void SetMobile(String mobile)
            {
                this.mobile = mobile;
            }
            /// <summary>
            /// Returns the address line1 of the User.
            /// </summary>
            /// <returns>The address line1 of the User.</returns>
            public String GetLine1()
            {
                return line1;
            }
            /// <summary>
            /// Sets the address line1 of the User.
            /// </summary>
            /// <param name="line1">Address line1 of the User</param>
            public void SetLine1(String line1)
            {
                this.line1 = line1;
            }
            /// <summary>
            ///  Returns the address line2 of the User.
            /// </summary>
            /// <returns>The address line2 of the User.</returns>
            public String GetLine2()
            {
                return line2;
            }
            /// <summary>
            /// Sets the address line2 of the User.
            /// </summary>
            /// <param name="line2">Address line2 of the User</param>
            public void SetLine2(String line2)
            {
                this.line2 = line2;
            }
            /// <summary>
            /// Returns the city of the User.
            /// </summary>
            /// <returns>The city of the User.</returns>
            public String GetCity()
            {
                return city;
            }
            /// <summary>
            /// Sets the city of the User.
            /// </summary>
            /// <param name="city">City of the User</param>
            public void SetCity(String city)
            {
                this.city = city;
            }
            /// <summary>
            /// Returns the state of the User.
            /// </summary>
            /// <returns>The state of the User.</returns>
            public String GetState()
            {
                return state;
            }
            /// <summary>
            /// Sets the state of the User.
            /// </summary>
            /// <param name="state">State of the User</param>
            public void SetState(String state)
            {
                this.state = state;
            }
            /// <summary>
            /// Returns the country of the User.
            /// </summary>
            /// <returns>The country of the User.</returns>
            public String GetCountry()
            {
                return country;
            }
            /// <summary>
            /// Sets the country of the User.
            /// </summary>
            /// <param name="country">Country of the User</param>
            public void SetCountry(String country)
            {
                this.country = country;
            }
            /// <summary>
            /// Returns the pincode of the User.
            /// </summary>
            /// <returns>The pincode of the User.</returns>
            public String GetPincode()
            {
                return pincode;
            }
            /// <summary>
            /// Sets the pincode of the User.
            /// </summary>
            /// <param name="pincode">Pincode of the User</param>
            public void SetPincode(String pincode)
            {
                this.pincode = pincode;
            }
            /// <summary>
            /// Returns the home land line of the User.
            /// </summary>
            /// <returns>The home land line of the User.</returns>
            public String GetHomeLandLine()
            {
                return homeLandLine;
            }
            /// <summary>
            /// Sets the home land line of the User.
            /// </summary>
            /// <param name="homeLandLine">Home land line of the User</param>
            public void SetHomeLandLine(String homeLandLine)
            {
                this.homeLandLine = homeLandLine;
            }
            /// <summary>
            /// Returns the office land line of the User.
            /// </summary>
            /// <returns>The office land line of the User.</returns>
            public String GetOfficeLandLine()
            {
                return officeLandLine;
            }
            /// <summary>
            /// Sets the office land line of the User.
            /// </summary>
            /// <param name="officeLandLine">Office land line of the User</param>
            public void SetOfficeLandLine(String officeLandLine)
            {
                this.officeLandLine = officeLandLine;
            }
            /// <summary>
            /// Returns the date of birth of the User.
            /// </summary>
            /// <returns>The data of birth of the User.</returns>
            public DateTime GetDateOfBirth()
            {
                return dateOfBirth;
            }
            /// <summary>
            /// Sets the data of birth of the User.
            /// </summary>
            /// <param name="dateOfBirth">Date of birth of the User</param>
            public void SetDateOfBirth(DateTime dateOfBirth)
            {
                this.dateOfBirth = dateOfBirth;
            }
        }
    }
}