using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Mirror;

public class AvatarManager : NetworkBehaviour
{
    public List<GameObject> avatars;
    public List<HumanBodyBones> trackedBones;
    void Start()
    {
        SetupAvatarController(avatars[0]);
    }

    void SetupAvatarController(GameObject avatarPrefab)
    {
        var o = GameObject.Instantiate(avatarPrefab);
        o.transform.parent = transform;
        var avatar = GetComponent<NetworkAvatar>();
        avatar.Init(o.GetComponent<Animator>());
        var config = o.GetComponent<CharacterRigConfig>();
        
        var vrRig = o.AddComponent<InitVRRig>();
        vrRig.headIKTarget = config.HeadIKTarget;
        vrRig.leftIKTarget = config.LeftHandIKTarget;
        vrRig.rightIKTarget = config.RightHandIKTarget;
        
        var scaler = o.AddComponent<AvatarScaler>();
        scaler.root = config.Root;
        scaler.head = config.Head;
        scaler.floor = config.ToeTip;
        scaler.scaleTargets = new List<Transform>();
        scaler.scaleTargets.Add(config.Root);
        foreach (var m in config.Meshes)
            scaler.scaleTargets.Add(m);

        var controller = o.AddComponent<AvatarController>();
        controller.head = config.Head;
        controller.cameraTarget = config.CameraTarget;
        controller.root = config.Root;
        controller.rootRig = config.RootRig;
        controller.anim = o.GetComponent<Animator>();
        vrRig.controller = controller;
        vrRig.scaler = scaler;

    }
}
