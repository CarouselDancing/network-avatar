using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Wolf3D.ReadyPlayerMe.AvatarSDK;


public enum AvatarType {
    LOCAL, 
    RMB
}
[Serializable]
public class AvatarInfo
{
    public AvatarType avatarType;
    public GameObject prefab;
    public string AvatarURL;
    public GameObject gameObject;

    public void Load(AvatarLoader avatarLoader, Action<GameObject, AvatarMetaData> OnAvatarLoaded)
    {
        avatarLoader.LoadAvatar(AvatarURL, OnAvatarImported, OnAvatarLoaded);
    }
    public void OnAvatarImported(GameObject avatar)
    {
        gameObject = avatar;
        Debug.Log($"Avatar imported. [{Time.timeSinceLevelLoad:F2}]");
    }


}

public class AvatarManager : NetworkBehaviour
{
    public List<AvatarInfo> avatars;
    public List<HumanBodyBones> trackedBones;
    public int avatarIndex = 0;
    int prevAvatarIndex = 0;
    NetworkAvatar networkAvatar;
    public RuntimeAnimatorController animController;
    void Start()
    {
        networkAvatar = GetComponent<NetworkAvatar>();
        UpdateAvatar();
    }

    public void UpdateAvatar()
    {
        var avatar = GetComponent<NetworkAvatar>();
        avatar.initialized = false;
        if (avatars[prevAvatarIndex].gameObject != null) Destroy(avatars[prevAvatarIndex].gameObject);
        if (avatars[avatarIndex].avatarType == AvatarType.RMB) {
            SetupAvatarControllerFromRMB(avatarIndex);
        }
        else
        {
            SetupAvatarControllerFromPrefab(avatarIndex);
        }
    }

    void SetupAvatarControllerFromRMB(int avatarIndex)
    {
        GameObject avatarPrefab = avatars[avatarIndex].prefab;
        AvatarLoader avatarLoader = new AvatarLoader();
        avatars[avatarIndex].Load(avatarLoader, OnRMBAvatarLoaded);
    }

    void SetupAvatarControllerFromPrefab(int avatarIndex)
    {
        GameObject avatarPrefab = avatars[avatarIndex].prefab;
        var o = GameObject.Instantiate(avatarPrefab);
        o.transform.parent = transform;
        var config = o.GetComponent<CharacterRigConfig>();
        avatars[avatarIndex].gameObject = o;
        var vrRig = SetupVRRig(o, config);
        RegisterVRRig(o.GetComponent<Animator>(), vrRig);
    }

    public void OnRMBAvatarLoaded(GameObject avatar, AvatarMetaData metaData=null)
    {
        avatar.transform.parent = transform;
        var ikRigBuilder = new RPMIKRigBuilder(animController);
        var config = ikRigBuilder.Build(avatar);
        var vrRig = SetupVRRig(avatar, config);
        RegisterVRRig(avatar.GetComponent<Animator>(), vrRig);
        Debug.Log($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
    }

    public InitVRRig SetupVRRig(GameObject o, CharacterRigConfig config)
    {
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
        return vrRig;
    }


    void RegisterVRRig(Animator animator, InitVRRig vrRig)
    {
        networkAvatar.vrRigInitializer = vrRig;
        networkAvatar.Init(animator);
    }

    [Command]
    void UpdateServerAvatar(int _avatarIndex)
    {
        prevAvatarIndex = avatarIndex;
        avatarIndex = _avatarIndex;
       
        ChangeClientAvatar(avatarIndex);
        UpdateAvatar();
    }

    [ClientRpc]
    void ChangeClientAvatar(int _avatarIndex)
    {
        prevAvatarIndex = avatarIndex;
        avatarIndex = _avatarIndex;
        UpdateAvatar();
    }

    public void ToggleAvatar()
    {
        prevAvatarIndex = avatarIndex;
        var i = avatarIndex + 1;
        i %= avatars.Count;
        UpdateServerAvatar(i);
    }
}
