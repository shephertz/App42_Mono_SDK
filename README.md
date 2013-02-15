App42_Mono_SDK
==============

App42 Cloud API Client SDK files for Mono platform

[Download the latest App42 MonoTouch SDK] (https://github.com/shephertz/App42_Mono_SDK/raw/master/MonoTouch/0.8.5/App42_MONOTouch_SDK_0.8.5.zip)

[Download the latest App42 MonoDroid SDK] (https://github.com/shephertz/App42_Mono_SDK/raw/master/MonoDroid/0.8.5/App42_MonoDroid_SDK_0.8.5.zip)

[Documentation and API guide] (http://api.shephertz.com/cloudapidocs/index.php)

==============
Pre-requisites
==============

This assumes that you have setup your MonoDevelop IDE for the platform that you are trying to build for (iOS/Android etc). If you haven't done that, you can get started on that by visiting http://docs.xamarin.com/


#### Steps for integrating App42 APIs in to your MonoDroid project #####

The steps are quite straight forward

Download the zip file from this repo and unzip the contents.

Open your application's solution

Add references to the unzipped dlls i.e. App42_MonoDroid_API_0.x.x.dll/App42_MonoTouch_SDK_0.x.x.dll and Newtonsoft.Json.MonoDroid.dll/Newtonsoft.Json.MonoTouch.dll

You can now add references to App42 APIs and start using it in your application!

#### This Repository Also contains ######

======================
App42 MonoDroid Sample
======================

MonoDroid sample contains an activity which creates game and save user score in App42 Cloud.

======================
App42 MonoTouch Sample
======================
MonoTouch sample contains an activity which creates game and save user score in App42 Cloud.

======================================
Third Party binaries and licenses used
======================================

##### JSON.NET ######
(http://json.codeplex.com)
The MIT License (MIT)
Copyright (c) 2007 James Newton-King

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.