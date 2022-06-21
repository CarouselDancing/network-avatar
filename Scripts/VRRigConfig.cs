using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRigConfig : MonoBehaviour
{
    public Transform leftController;
    public Transform rightController;
    public TrackerTarget leftControllerTarget;
    public TrackerTarget rightControllerTarget;


    public TrackerTarget hipTrackerTarget;
    public TrackerTarget leftFootTarget;
    public TrackerTarget rightFootTarget;


    public TrackerTarget headTrackerTarget;
    public Transform origin;
}
