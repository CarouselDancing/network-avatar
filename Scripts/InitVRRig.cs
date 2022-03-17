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



    public void Setup(CharacterRigConfig config, GameObject avatar)
    {
        //TODO store config in VRRig initalizer
        headIKTarget = config.HeadIKTarget;
        leftIKTarget = config.LeftHandIKTarget;
        rightIKTarget = config.RightHandIKTarget;

        bool activateTrackers = GlobalGameManager.GetInstance().config.activateHipTracker;
        if (activateTrackers)
        {
            hipTrackerTarget = config.RootTarget;
            hipTrackerOffset = new Vector3();
        }


        bool activateFootTrackers = GlobalGameManager.GetInstance().config.activateFootTrackers;
        if (activateFootTrackers)
        {
            leftFootIKTarget = config.LeftFootIKTarget;
            leftFootTrackerOffset = new Vector3();
            rightFootIKTarget = config.RightFootIKTarget;
            rightFootTrackerOffset = new Vector3();
        }

        scaler = avatar.AddComponent<AvatarScaler>();
        scaler.root = config.Root;
        scaler.head = config.Head;
        scaler.floor = config.ToeTip;
        scaler.scaleTargets = new List<Transform>();
        scaler.scaleTargets.Add(config.Root);
        foreach (var m in config.Meshes)
            scaler.scaleTargets.Add(m);

        controller = avatar.AddComponent<AvatarController>();
        controller.headset = Camera.main.transform;
        controller.head = config.Head;
        controller.hips = config.Root;
        controller.cameraTarget = config.CameraTarget;
        controller.root = config.RootTarget;
        controller.rootRig = config.RootRig;
        controller.anim = avatar.GetComponent<Animator>();

    }


    public void Init()
    {
        var config = Camera.main.GetComponent<VRRigConfig>();
        if (config == null)
        {
            scaler.enabled = false;
            controller.enabled = false;
            Debug.Log("disable VR");
            return;
        }
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
            Debug.Log("activate head ik");
        }
        else
        {
            Debug.Log("deactivate head ik");
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
