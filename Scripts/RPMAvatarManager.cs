using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Wolf3D.ReadyPlayerMe.AvatarSDK;


public class RPMAvatarManager : NetworkBehaviour
{
    [SyncVar]
    public string AvatarURL = "";
    public GameObject go;
    public RuntimeAnimatorController animationController;
    public bool activateFootRig = false;
    NetworkAvatar networkAvatar;
    public bool IsOwner => isLocalPlayer;
    private void Start()
    {
        if (IsOwner)
        {
            AvatarURL = GlobalGameManager.GetInstance().config.rpmURL;
        }
        networkAvatar = GetComponent<NetworkAvatar>();
        if (AvatarURL != "")
        {
            SetupAvatarControllerFromRPM();
        }
        else
        {
            Debug.Log("Error: avatar url is emtpy");
        }
    }

    void SetupAvatarControllerFromRPM()
    {
        AvatarLoader avatarLoader = new AvatarLoader();
        Debug.Log($"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]");
        avatarLoader.LoadAvatar(AvatarURL, OnAvatarImported, OnRPMAvatarLoaded);
    }

    public void OnAvatarImported(GameObject avatar)
    {
        go = avatar;
        avatar.transform.parent = transform;
        Debug.Log($"Avatar imported. [{Time.timeSinceLevelLoad:F2}]");
    }


    public void OnRPMAvatarLoaded(GameObject avatar, AvatarMetaData metaData = null)
    {
        var ikRigBuilder = new RPMIKRigBuilder(animationController, activateFootRig);
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
        bool activatedVRRig = networkAvatar.Init(animator);

        if (activatedVRRig) return;

        for (int i = 0; i < animator.transform.childCount; i++)
        {
            var t = animator.transform.GetChild(i);
            if (t.name == "Armature")
            {
                t.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                break;
            }
        }
    }

}
