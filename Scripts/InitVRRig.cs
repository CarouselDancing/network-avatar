using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class InitVRRig : MonoBehaviour
{

    public AvatarScaler scaler;
    public AvatarController controller;
    public Transform headIKTarget;
    public Transform leftIKTarget;
    public Transform rightIKTarget;
    public Transform hipTrackerTarget;
    public Vector3 hipTrackerOffset;
    public Transform leftFootIKTarget;
    public Vector3 leftFootTrackerOffset;
    public Transform rightFootIKTarget;
    public Vector3 rightFootTrackerOffset;
    public void Init()
    {
        var config = Camera.main.GetComponent<VRRigConfig>();
        scaler.cameraTarget = Camera.main.transform;
        controller.headset = Camera.main.transform;
        controller.rightController = config.rightController;
        controller.leftController = config.leftController;

        config.leftControllerTarget.target = leftIKTarget;
        config.leftControllerTarget.offset = Vector3.zero;
        config.leftControllerTarget.initialized = true;
        config.leftControllerTarget.enabled = true;

        config.rightControllerTarget.target = rightIKTarget;
        config.rightControllerTarget.offset = Vector3.zero;
        config.rightControllerTarget.initialized = true;
        config.rightControllerTarget.enabled = true;

        if (headIKTarget != null && config.headTrackerTarget != null)
        {
            config.headTrackerTarget.target = headIKTarget;
            config.headTrackerTarget.offset = Vector3.zero;
            config.headTrackerTarget.initialized = true;
            config.headTrackerTarget.enabled = true;
        }

        if (hipTrackerTarget != null && config.hipTrackerTarget != null)
        {
            config.hipTrackerTarget.target = hipTrackerTarget;
            config.hipTrackerTarget.offset = hipTrackerOffset;
            config.hipTrackerTarget.initialized = true;
            config.hipTrackerTarget.enabled = true;
        }

        if (leftFootIKTarget != null && config.leftFootTarget != null)
        {
            config.leftFootTarget.target = leftFootIKTarget;
            config.leftFootTarget.offset = leftFootTrackerOffset;
            config.leftFootTarget.initialized = true;
            config.leftFootTarget.enabled = true;
        }
        if (rightFootIKTarget != null && config.rightFootTarget != null)
        {
            config.rightFootTarget.target = rightFootIKTarget;
            config.rightFootTarget.offset = rightFootTrackerOffset;
            config.rightFootTarget.initialized = true;
            config.rightFootTarget.enabled = true;
        }
    }

    public void Deactivate()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }
        RigBuilder rig = GetComponent<RigBuilder>();
        if (rig != null)
        {
            rig.enabled = false;
        }
        scaler.enabled = false;
        controller.enabled = false;
    }


}
