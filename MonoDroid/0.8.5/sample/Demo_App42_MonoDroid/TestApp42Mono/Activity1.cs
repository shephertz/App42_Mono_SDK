using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.game;
namespace TestApp42Mono
{
	[Activity (Label = "TestApp42Mono", MainLauncher = true)]
	public class Activity1 : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				String gameName = "TestGame";
				String description = "Game Description";
				
				String userName = "John";
				double userScore = 100;
				
				//Your API_KEY and SECRET_KEY msut be given here
				ServiceAPI sp = new ServiceAPI("<API_KEY>","<SECRET_KEY>");
				GameService gameService = sp.BuildGameService();
				ScoreBoardService scoreBoardService = sp.BuildScoreBoardService();
				
				//Create Game (Only One Time Activity). Will throw an exception if already created
				Game game = gameService.CreateGame(gameName, description);
				
				//Save user score in App42 Cloud for created Game
				Game  score = scoreBoardService.SaveUserScore(gameName, userName, userScore);
				
				Console.WriteLine(" Response :"  + score);
				button.Text = string.Format ("Score Saved in App42 Cloud");

			};

		}
	}
}


