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

    public Transform leftFootIKTarget;

    public Transform rightFootIKTarget;



    public void Setup(CharacterRigConfig config, GameObject avatar)
    {
        var globalConfig = GlobalGameState.GetInstance().config;
        //TODO store config in VRRig initalizer
        headIKTarget = config.HeadIKTarget;
        leftIKTarget = config.LeftHandIKTarget;
        rightIKTarget = config.RightHandIKTarget;
        if (globalConfig.activateHipTracker)
        {
            hipTrackerTarget = config.RootTarget;
        }


        if (globalConfig.activateFootTrackers)
        {
            leftFootIKTarget = config.LeftFootIKTarget;

            rightFootIKTarget = config.RightFootIKTarget;
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
        var trackerConfig = Camera.main.GetComponent<VRRigConfig>();
        if (trackerConfig == null)
        {
            scaler.enabled = false;
            controller.enabled = false;
            Debug.Log("disable VR");
            return;
        }
        scaler.cameraTarget = Camera.main.transform;
        controller.headset = Camera.main.transform;
        controller.rightController = trackerConfig.rightController;
        controller.leftController = trackerConfig.leftController;

        trackerConfig.leftControllerTarget.target = leftIKTarget;
        trackerConfig.leftControllerTarget.offset = Vector3.zero;
        trackerConfig.leftControllerTarget.initialized = true;
        trackerConfig.leftControllerTarget.enabled = true;

        trackerConfig.rightControllerTarget.target = rightIKTarget;
        trackerConfig.rightControllerTarget.offset = Vector3.zero;
        trackerConfig.rightControllerTarget.initialized = true;
        trackerConfig.rightControllerTarget.enabled = true;

        if (headIKTarget != null && trackerConfig.headTrackerTarget != null)
        {
            trackerConfig.headTrackerTarget.target = headIKTarget;
            trackerConfig.headTrackerTarget.offset = Vector3.zero;
            trackerConfig.headTrackerTarget.initialized = true;
            trackerConfig.headTrackerTarget.enabled = true;
            Debug.Log("activate head ik");
        }
        else
        {
            Debug.Log("deactivate head ik");
        }

        if (hipTrackerTarget != null && trackerConfig.hipTrackerTarget != null)
        {
            trackerConfig.hipTrackerTarget.target = hipTrackerTarget;
            trackerConfig.hipTrackerTarget.initialized = true;
            trackerConfig.hipTrackerTarget.enabled = true;
        }

        if (leftFootIKTarget != null && trackerConfig.leftFootTarget != null && rightFootIKTarget != null && trackerConfig.rightFootTarget != null)
        {
            trackerConfig.leftFootTarget.target = leftFootIKTarget;
            trackerConfig.leftFootTarget.initialized = true;
            trackerConfig.leftFootTarget.enabled = true;
     
            trackerConfig.rightFootTarget.target = rightFootIKTarget;
            trackerConfig.rightFootTarget.initialized = true;
            trackerConfig.rightFootTarget.enabled = true;
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
