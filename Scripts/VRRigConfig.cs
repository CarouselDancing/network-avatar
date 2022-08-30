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

    public Transform user;

    
    public void DisableAllTargets(){
        leftControllerTarget.initialized = false;
        leftControllerTarget.enabled = false;
        rightControllerTarget.initialized = false;
        rightControllerTarget.enabled = false;
        headTrackerTarget.initialized = false;
        headTrackerTarget.enabled = false;
        hipTrackerTarget.initialized = false;
        hipTrackerTarget.enabled = false;
        leftFootTarget.initialized = false;
        leftFootTarget.enabled = false;
        rightFootTarget.initialized = false;
        rightFootTarget.enabled = false;
    }
}
