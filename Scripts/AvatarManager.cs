using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Mirror;

public class AvatarManager : NetworkBehaviour
{
    public List<GameObject> avatars;
    public List<HumanBodyBones> trackedBones;
    public int avatarIndex = 0;
    public GameObject currentAvatar;

    void Start()
    {
        UpdateAvatar();
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
        controller.headset = Camera.main.transform;
        controller.head = config.Head;
        controller.hips = config.Root;
        controller.cameraTarget = config.CameraTarget;
        controller.root = config.RootTarget;
        controller.rootRig = config.RootRig;
        controller.anim = o.GetComponent<Animator>();
        vrRig.controller = controller;
        vrRig.scaler = scaler;

        var avatar = GetComponent<NetworkAvatar>();
        avatar.vrRigInitializer = vrRig;
        avatar.Init(o.GetComponent<Animator>());


        return o;

    }
    [Command]
    void UpdateServerAvatar(int _avatarIndex)
    {
        avatarIndex = _avatarIndex;
        ChangeClientAvatar(avatarIndex);
        UpdateAvatar();
    }

    [ClientRpc]
    void ChangeClientAvatar(int _avatarIndex)
    {
        avatarIndex = _avatarIndex;
        UpdateAvatar();
    }

    public void ToggleAvatar()
    {
        var i = avatarIndex + 1;
        i %= avatars.Count;
        UpdateServerAvatar(i);
    }
}
