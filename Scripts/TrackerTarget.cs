using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


}
