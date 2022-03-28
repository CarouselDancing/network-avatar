using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TrackerTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3 rotationOffset;
    public bool initialized = false;

    public void SetOffset()
    {
        offset = Quaternion.Inverse(transform.rotation) * (target.position - transform.position);
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!initialized) SetOffset();
        target.position = transform.position + transform.rotation * offset;
        target.rotation = transform.rotation * Quaternion.Euler(rotationOffset);
    }

    public void SetConfig(TrackerConfig config)
    {

        var _offset = config.posOffset;
        offset = new Vector3(_offset[0], _offset[1], _offset[2]);
        var _rotOffset = config.rotOffset;
        rotationOffset = new Vector3(_rotOffset[0], _rotOffset[1], _rotOffset[2]);
        var t = GetComponent<SteamVR_TrackedObject>();
        t.SetDeviceIndex(config.deviceID);
        Debug.Log("set device id" + name + ": " + config.deviceID.ToString());
    }


}
