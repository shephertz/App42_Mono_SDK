App42_MONODROID_SDK
================

App42 BPaaS Cloud API Client SDK files for MONODROID

- Download the App42 BPaaS MONODROID SDK
- Unzip the file.
- Add the **App42_BPaaS_MONODROID_SDK_x.x.x.dll** to your project refrence. (**Add Reference -> Select Your dll path)**
- Initialize the library using
```
ServiceAPI sp = new ServiceAPI("<YOUR_API_KEY>","<YOUR_SECRET_KEY>");
sp.SetBaseURL("<YOUR_API_SERVER_URL>");
```
- Instantiate the service that one wants to use in the App, e.g. using User service one has to do the following
```
UserService userService = sp.BuildUserService();
```

- Now one can call associated method of that service e.g. user creation can be done with the following snippet

```
String userName = "Nick";
String pwd = "********";
String emailId = "nick@shephertz.co.in";    
User user = userService.CreateUser(userName, pwd, emailId); 
Console.WriteLine("userName is " + user.GetUserName());
Console.WriteLine("emailId is " + user.GetEmail());
```

- Build the project and run.
