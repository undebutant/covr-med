// Decompiled with JetBrains decompiler
// Type: TrackingManager
// Assembly: Base_Realyz, Version=1.0.6172.26195, Culture=neutral, PublicKeyToken=null
// MVID: 828CA3DB-EE4F-4033-BCA6-E7EC497C16AA
// Assembly location: C:\Users\Devadmin\Documents\Formation IBISC 31-01-17\Projet SDB - Scene à compléter - IBISC\Projet à compléter Unity 5.1.1f1\Assets\Plugins\Base_Realyz.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using VRPNLightAPI_testCS;

public class TrackingManager : MonoBehaviour
{
	private static bool UpdateIsDoneOneTimeDuringTheActualFrame = false;
	private static bool StartIsAlreadyDone = false;
	private static List<TrackingManager.Tracker> trackers;
	private trackerData trackerDataTemp;
	public GameObject g_Base_Mobilyz;
	public List<string> Object_List;
	public List<string> VRPN_List;

	public TrackingManager ()
	{
		//base.\u002Ector ();
	}

	private void Awake ()
	{
		//this.Object_List = new List<string> ();
		//this.VRPN_List = new List<string> ();
	}

	private void Start ()
	{
		if (TrackingManager.StartIsAlreadyDone)
			return;
		//TrackingSwapCalculations.init_matrix_trackingSoftware2Unity3D ();
		this.trackerDataTemp = new trackerData ();
		TrackingManager.trackers = new List<TrackingManager.Tracker> ();
		this.readAndInitTrackersDataFromConfigFiles ();
		TrackingManager.StartIsAlreadyDone = true;
	}

	private void Update ()
	{
		if (TrackingManager.UpdateIsDoneOneTimeDuringTheActualFrame)
			return;

		this.AssignTrackingPose ();
		TrackingManager.UpdateIsDoneOneTimeDuringTheActualFrame = true;
	}

	private void LateUpdate ()
	{
		TrackingManager.UpdateIsDoneOneTimeDuringTheActualFrame = false;
	}

	public void Close ()
	{
		foreach (TrackingManager.Tracker tracker in TrackingManager.trackers)
			WrapperVrpnLightAPI.closeTracker (tracker.handle);
	}

	private void OnDestroy ()
	{
		this.Close ();
	}

	private void AssignTrackingPose ()
	{
		foreach (TrackingManager.Tracker tracker in TrackingManager.trackers) {
			if (tracker.active) {
				WrapperVrpnLightAPI.getTrackerData (tracker.handle, ref this.trackerDataTemp);
				if (this.trackerDataTemp.wasUpdated == 1) {
					if (!float.IsNaN ((float)this.trackerDataTemp.quatX) && !float.IsNaN ((float)this.trackerDataTemp.quatY) && (!float.IsNaN ((float)this.trackerDataTemp.quatZ) && !float.IsNaN ((float)this.trackerDataTemp.quatW)))
						tracker.gameObject.transform.rotation = (this.g_Base_Mobilyz.transform.rotation * TrackingSwapCalculations.GetRotationFromTrackingSpaceToBaseMobilyzSpace ((float)this.trackerDataTemp.quatX, (float)this.trackerDataTemp.quatY, (float)this.trackerDataTemp.quatZ, (float)this.trackerDataTemp.quatW));
					if (!float.IsNaN ((float)this.trackerDataTemp.posX) && !float.IsNaN ((float)this.trackerDataTemp.posY) && !float.IsNaN ((float)this.trackerDataTemp.posZ))
						tracker.gameObject.transform.position = (this.g_Base_Mobilyz.transform.TransformPoint (TrackingSwapCalculations.GetPositionFromTrackingSpaceToBaseMobilyzSpace ((float)this.trackerDataTemp.posX, (float)this.trackerDataTemp.posY, (float)this.trackerDataTemp.posZ)));
				}
			}
		}
	}

	private void readAndInitTrackersDataFromConfigFiles ()
	{
		for (int index = 0; index < this.Object_List.Count; ++index)
			this.createTracker (GameObject.Find (this.Object_List [index]), this.VRPN_List [index], true);
	}

	public void printListOfTrackersCurrentlyUsed ()
	{
		string str = "List of Trackers currently used ( = contained in the variable 'trackers' in TrackingManager.cs) :\n";
		for (int index = 0; index < TrackingManager.trackers.Count; ++index)
			str = str + (object)'\n' + "gameObject : " + (object)TrackingManager.trackers [index].gameObject + (object)'\n' + "name : " + TrackingManager.trackers [index].name + (object)'\n' + "handle : " + (object)TrackingManager.trackers [index].handle + (object)'\n' + "active : " + (object)TrackingManager.trackers [index].active + (object)'\n';
		MonoBehaviour.print ((object)str);
	}

	public TrackingManager.Tracker createTracker (GameObject _gameObject, string _name, bool active = true)
	{
		foreach (TrackingManager.Tracker tracker in TrackingManager.trackers) {
			if (tracker.gameObject == _gameObject)
				throw new UnityException ("A tracker with the same gameObject already exist! Use it instead of creating a new one.");
		}
		TrackingManager.Tracker tracker1;
		tracker1.gameObject = _gameObject;
		tracker1.name = _name;
		tracker1.handle = WrapperVrpnLightAPI.connectToTracker (_name);
		tracker1.active = active;
		TrackingManager.trackers.Add (tracker1);
		return tracker1;
	}

	public void deleteTracker (TrackingManager.Tracker tracker)
	{
		if (!TrackingManager.trackers.Contains (tracker))
			throw new TrackingManager.TrackerNotFoundException ("Tracker of " + (object)tracker.gameObject + " is not found in the list of trackers currently used.\nThis tracker has the following fields:\ngameObject : " + (object)tracker.gameObject + (object)'\n' + "name : " + tracker.name + (object)'\n' + "handle : " + (object)tracker.handle + (object)'\n' + "active : " + (object)tracker.active);
		WrapperVrpnLightAPI.closeTracker (tracker.handle);
		TrackingManager.trackers.Remove (tracker);
	}

	public void deleteAllTrackers ()
	{
		foreach (TrackingManager.Tracker tracker in TrackingManager.trackers)
			WrapperVrpnLightAPI.closeTracker (tracker.handle);
		TrackingManager.trackers.Clear ();
	}

	public TrackingManager.Tracker setActiveTracker (TrackingManager.Tracker tracker, bool isActive)
	{
		int trackerIndex = this.getTrackerIndex (tracker);
		if (trackerIndex >= 0 && trackerIndex < TrackingManager.trackers.Count) {
			TrackingManager.Tracker tracker1 = TrackingManager.trackers [trackerIndex];
			tracker1.active = isActive;
			TrackingManager.trackers [trackerIndex] = tracker1;
			return tracker1;
		}
		throw new TrackingManager.TrackerNotFoundException ("Tracker of " + (object)tracker.gameObject + " is not found in the list of trackers currently used.\nThis tracker has the following fields:\nname : " + tracker.name + (object)'\n' + "gameObject : " + (object)tracker.gameObject + (object)'\n' + "handle : " + (object)tracker.handle + (object)'\n' + "active : " + (object)tracker.active);
	}

	public void setAllActiveTrackers (bool isActive)
	{
		for (int index = 0; index < TrackingManager.trackers.Count; ++index) {
			TrackingManager.Tracker tracker = TrackingManager.trackers [index];
			tracker.active = isActive;
			TrackingManager.trackers [index] = tracker;
		}
	}

	public TrackingManager.Tracker setGameObjectTracker (TrackingManager.Tracker tracker, GameObject _gameObject)
	{
		int trackerIndex = this.getTrackerIndex (tracker);
		if (trackerIndex >= 0 && trackerIndex < TrackingManager.trackers.Count) {
			TrackingManager.Tracker tracker1 = TrackingManager.trackers [trackerIndex];
			tracker1.gameObject = _gameObject;
			TrackingManager.trackers [trackerIndex] = tracker1;
			return tracker1;
		}
		throw new TrackingManager.TrackerNotFoundException ("Tracker of " + (object)tracker.gameObject + " is not found in the list of trackers currently used.\nThis tracker has the following fields:\ngameObject : " + (object)tracker.gameObject + (object)'\n' + "name : " + tracker.name + (object)'\n' + "handle : " + (object)tracker.handle + (object)'\n' + "active : " + (object)tracker.active);
	}

	public int getTrackerIndex (TrackingManager.Tracker tracker)
	{
		return TrackingManager.trackers.FindIndex ((Predicate<TrackingManager.Tracker>)(x =>
		{
			if (x.gameObject == tracker.gameObject && x.name == tracker.name && x.handle == tracker.handle)
				return x.active == tracker.active;
			return false;
		}));
	}

	public List<TrackingManager.Tracker> getTrackers (string _name)
	{
		return TrackingManager.trackers.FindAll ((Predicate<TrackingManager.Tracker>)(x => x.name == _name));
	}

	public TrackingManager.Tracker getTracker (GameObject _gameObject)
	{
		return TrackingManager.trackers.Find ((Predicate<TrackingManager.Tracker>)(x => x.gameObject==_gameObject));
	}

	public Vector3 getTrackerPositionTrackingSoftwareSpace (TrackingManager.Tracker tracker)
	{
		WrapperVrpnLightAPI.getTrackerData (tracker.handle, ref this.trackerDataTemp);
		return new Vector3 ((float)this.trackerDataTemp.posX, (float)this.trackerDataTemp.posY, (float)this.trackerDataTemp.posZ);
	}

	public Quaternion getTrackerRotationTrackingSoftwareSpace (TrackingManager.Tracker tracker)
	{
		WrapperVrpnLightAPI.getTrackerData (tracker.handle, ref this.trackerDataTemp);
		return new Quaternion ((float)this.trackerDataTemp.quatX, (float)this.trackerDataTemp.quatY, (float)this.trackerDataTemp.quatZ, (float)this.trackerDataTemp.quatW);
	}

	public Vector3 getTrackerPositionBaseMobilyzSpace (TrackingManager.Tracker tracker)
	{
		WrapperVrpnLightAPI.getTrackerData (tracker.handle, ref this.trackerDataTemp);
		return this.g_Base_Mobilyz.transform.TransformPoint (TrackingSwapCalculations.GetPositionFromTrackingSpaceToBaseMobilyzSpace ((float)this.trackerDataTemp.posX, (float)this.trackerDataTemp.posY, (float)this.trackerDataTemp.posZ));
	}

	public Quaternion getTrackerRotationBaseMobilyzSpace (TrackingManager.Tracker tracker)
	{
		WrapperVrpnLightAPI.getTrackerData (tracker.handle, ref this.trackerDataTemp);
		return TrackingSwapCalculations.GetRotationFromTrackingSpaceToBaseMobilyzSpace ((float)this.trackerDataTemp.quatX, (float)this.trackerDataTemp.quatY, (float)this.trackerDataTemp.quatZ, (float)this.trackerDataTemp.quatW);
	}

	public struct Tracker
	{
		public GameObject gameObject;
		public string name;
		public int handle;
		public bool active;
	}

	public class TrackerNotFoundException : UnityException
	{
		public TrackerNotFoundException (string message = "Tracker Not Found")
		{

		}
	}
}
