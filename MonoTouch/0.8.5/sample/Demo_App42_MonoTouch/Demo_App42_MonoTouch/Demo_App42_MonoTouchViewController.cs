using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.game;

namespace Demo_App42_MonoTouch
{
	public partial class Demo_App42_MonoTouchViewController : UIViewController
	{
		public Demo_App42_MonoTouchViewController () : base ("Demo_App42_MonoTouchViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			//Here we register for the TouchUpInside event using an outlet to
			//the UIButton created in Interface Builder. Also see the target-action
			//approach for accomplishing the same thing below.
			aButton.TouchUpInside += (o,s) => {
				Console.WriteLine ("button touched using a TouchUpInside event"); 
			}; 
			
			//You could also use a C# 2.0 style anonymous function
			//            aButton.TouchUpInside += delegate {
			//                Console.WriteLine ("button touched");         
			//            };
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			base.ViewDidUnload ();
			
			aButton = null;
			//ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		//This is an action method connected to the TouchUpInside event
		//of a UIButton. The action is connected via Interface Builder
		//The same thing can be accomplished with a .NET event registered
		//to the UIButton in code, as we do in the ViewDidLoad method above.
		
		partial void HandleButtonTouch (NSObject sender)
		{
			Console.WriteLine ("button touched using the action method");
			//aButton = "";
			String gameName = "MonoTouchGame";
			String gameDescription = "description";
			String userName = "John";
			double userScore = 100;

			//Initialize ServiceAPI with YOUR API_KEY and Secret Key
			ServiceAPI sp = new ServiceAPI("<API_KEY>", "<SECRET_KEY>");
			GameService gameService = sp.BuildGameService();
			ScoreBoardService scoreBoardService = sp.BuildScoreBoardService();

			//Create Game (One time Activity. Will Throw an Exception if already created)
			Game game = gameService.CreateGame(gameName, gameDescription);

			//Save user game score
			Game scoreObj = scoreBoardService.SaveUserScore(gameName, userName, userScore);

			Console.WriteLine (" Score Saved : " + scoreObj);


		}
	}
}

