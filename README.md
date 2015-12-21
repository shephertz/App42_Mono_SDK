App42_CSHARP_SDK_Xamarin
================

1. [Register] (https://apphq.shephertz.com/register) with App42 platform.
2. Create an app once you are on Quick start page after registration.
3. If you are already registered, login to [AppHQ] (http://apphq.shephertz.com/register/app42Login) console and create an app from App Manager Tab.

__Download And Set Up SDK :-__

1). [Download] (https://github.com/shephertz/App42_Mono_SDK/archive/master.zip) Xamarin SDK (Ignore if you have already downloaded our SDK)

2). Unzip the downloaded Zip file. Unzipped folder contains version folders of dll and a sample folder.

3). Version folder (i.e __0.0__, __0.1__ etc) will contain __App42_CSHARP_SDK_x.x.x.dll__.

4). Then add __App42_CSHARP_SDK_x.x.x.dll__ in References of your Xamarin project by following below steps.
  
    (1) Right Click on References folder in your project.
	(2) Click on Edit References.
	(3) Select .Net Assembly from above tabs.
	(4) Click on browse button below, and select __App42_CSHARP_SDK_x.x.x.dll__ from your file system.
	(5) Then click OK and rebuild your project.
	

__Initializing SDK :-__
You have to instantiate App42API by putting your ApiKey/SecretKey to initialize the SDK.

```
App42API.Initialize("YOUR_API_KEY","YOUR_SECRET_KEY"); 
```

__Using App42 Services :-__
 You have to build target service that you want to use in your app. For example, User Service can be build with following snippet. Similarly you can build other service also with same notation.
 
```
UserService userService = App42API.BuildUserService();
//Similarly you can build other services like App42API.BuildXXXXService()
```

[Documentation and API guide] (http://api.shephertz.com/app42-dev/csharp-backend-apis.php)
