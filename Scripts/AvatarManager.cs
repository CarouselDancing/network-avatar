using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Mirror;

public class AvatarManager : NetworkBehaviour
{
    public List<GameObject> avatars;
    public List<HumanBodyBones> trackedBones;
    [SyncVar]
    public int avatarIndex = 0;
    [SyncVar]
    public bool initialized = false;
    public GameObject currentAvatar;

    void Update()
    {
        if (!initialized) UpdateAvatar();
    }
    public void UpdateAvatar()
    {
        var avatar = GetComponent<NetworkAvatar>();
        avatar.initialized = false;
        if (currentAvatar != null) Destroy(currentAvatar);
        currentAvatar = SetupAvatarController(avatars[avatarIndex]);
    }

    GameObject SetupAvatarController(GameObject avatarPrefab)
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
        initialized = true;
        return o;

    }
    [Command]
    void UpdateAvatar(int _avatarIndex)
    {
        avatarIndex = _avatarIndex;
        initialized = false;
    }

    public void ToggleAvatar()
    {
        avatarIndex += 1;
        avatarIndex %= avatars.Count;
        UpdateAvatar(avatarIndex);
    }


  
}
