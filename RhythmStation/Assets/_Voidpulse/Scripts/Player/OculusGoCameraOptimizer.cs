using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusGoCameraOptimizer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.Off;
		//Off/LMSLow/LMSMEdium/LMSHigh

		// Set to 72hz max for oGo
		OVRManager.display.displayFrequency = 72.0f;
	}

}
