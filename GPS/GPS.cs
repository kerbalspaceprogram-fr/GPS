using System;
using UnityEngine;
using System.Text;

namespace GPS
{
	public class GPS : PartModule
	{
		private Rect windowPosition;

		public override void OnStart (StartState state)
		{
			base.OnStart (state);

			if (this.windowPosition.x == 0 && this.windowPosition.y == 0)
				this.windowPosition = new Rect (Screen.width / 2, Screen.height / 2, 10, 10);
		}

		[KSPEvent(guiActive = true, guiName = "GPS")]
		public void Configuration ()
		{
			RenderingManager.AddToPostDrawQueue (3, new Callback (GpsGui));
		}

		private void GpsGui ()
		{
			GUI.skin = HighLogic.Skin;
			this.windowPosition = GUILayout.Window (1, this.windowPosition, DrawGui, "GPS", GUILayout.ExpandWidth (true), GUILayout.MinWidth (200));
		}

		private void DrawGui (int windowId)
		{
			GUILayout.BeginVertical ();


			string latitude = AngleToString(this.vessel.latitude) + ((this.vessel.latitude > 0) ? " N" : " S");
			string longitude = AngleToString(this.vessel.longitude) + ((this.vessel.longitude > 0) ? " E" : " W");

			GUILayout.Label("Latitude: " + latitude, GUILayout.ExpandWidth (true));
			GUILayout.Label("Longitude: " + longitude, GUILayout.ExpandWidth (true));
			GUILayout.Label("Speed: " + Math.Floor(this.vessel.srf_velocity.magnitude * 3.6) + "km/h" , GUILayout.ExpandWidth (true));

			if (GUILayout.Button ("Close", GUILayout.ExpandWidth (true))) RenderingManager.RemoveFromPostDrawQueue (3, new Callback (GpsGui));

			GUILayout.EndVertical ();

			GUI.DragWindow (new Rect (0, 0, 10000, 20));
		}

		private string AngleToString(double angle) {
			double abs = Math.Abs (angle);

			StringBuilder str = new StringBuilder ();
			str.Append (Math.Floor (abs));
			str.Append ("° ");
			str.Append (Math.Floor ((abs * 60) % 60));
			str.Append ("' ");
			str.Append (Math.Floor ((abs * 3600) % 60));
			str.Append ("\"");


			return str.ToString ();
		}

	}
}

