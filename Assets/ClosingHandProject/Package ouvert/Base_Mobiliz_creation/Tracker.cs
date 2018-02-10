using UnityEngine;
using VRPNLightAPI_testCS;

[DisallowMultipleComponent]
public class Tracker : MonoBehaviour {

	[SerializeField] string vrpnName;
	[SerializeField] bool position, rotation;

	int _id;
	trackerData _trackerData = new trackerData();

	void Start() {
		_id = WrapperVrpnLightAPI.connectToTracker(vrpnName+"@localhost");
	}

	void Update() {
		WrapperVrpnLightAPI.getTrackerData(_id, ref _trackerData);
		if (_trackerData.wasUpdated == 1) {
			if (position) {
				transform.localPosition = new Vector3((float)-_trackerData.posX, (float)_trackerData.posY, (float)_trackerData.posZ);
			}
			if (rotation) {
				transform.localRotation = new Quaternion((float)_trackerData.quatX, (float)-_trackerData.quatY, (float)-_trackerData.quatZ, (float)_trackerData.quatW);
			}
		}
	}

	void Destroy() {
		WrapperVrpnLightAPI.closeTracker(_id);
	}
}
