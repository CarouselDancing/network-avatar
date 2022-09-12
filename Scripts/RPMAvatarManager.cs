using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ReadyPlayerMe;

public class RPMAvatarManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnAvatarURLChanged))]
    public string syncAvatarURL;
    public string AvatarURL = "";
    public GameObject go;
    public RuntimeAnimatorController animationController;
    public bool activateFootRig = false;
    protected NetworkAvatar networkAvatar;
    public bool IsOwner => isLocalPlayer;
    public bool initiated = false;
    public bool offline;
    virtual public void Start()
    {
        networkAvatar = GetComponent<NetworkAvatar>();
        if (IsOwner)
        {
            var config = ClientConfig.GetInstance();
            var avatarIndex = config.userAvatar;
            AvatarURL = config.rpmAvatars[avatarIndex].url;
            if (AvatarURL != "")
            {

                SetupAvatarControllerFromRPM(AvatarURL);
                CmdSetURL(AvatarURL);
            }
            else
            {
                Debug.Log("Error: avatar url is emtpy");
            }
        }

    }


    
    [Command]
    public void CmdSetURL(string newValue)
    {
        Debug.Log("CmdSetURL");
        AvatarURL = newValue;
        syncAvatarURL = newValue;
        if(!initiated && newValue != "") {
        SetupAvatarControllerFromRPM(newValue);
    }
    }


    void OnAvatarURLChanged(string _, string newValue)
    {
        Debug.Log("OnAvatarURLChanged");
        if(!initiated && newValue != "") {
            SetupAvatarControllerFromRPM(newValue);
        }
    }


    public void SetupAvatarControllerFromRPM(string url)
    {
        initiated = true;
        AvatarLoader avatarLoader = new AvatarLoader();
        Debug.Log($"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]");
        avatarLoader.LoadAvatar(url, OnAvatarImported, OnRPMAvatarLoaded);
    }

    public void OnAvatarImported(GameObject avatar)
    {   
        go = avatar;
        avatar.transform.parent = transform;
        Debug.Log($"Avatar imported. [{Time.timeSinceLevelLoad:F2}]");
    }


    virtual public void OnRPMAvatarLoaded(GameObject avatar, AvatarMetaData metaData=null)
    {
        var config = ClientConfig.GetInstance();
        bool activateFootRig = config.activateFootTrackers;
        var ikRigBuilder = new RPMIKRigBuilder(animationController, activateFootRig);
        var rigConfig = ikRigBuilder.Build(avatar);
        SetupRig(rigConfig, avatar);
        Debug.Log($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n");
    }


    public void SetupRig(CharacterRigConfig rigConfig, GameObject avatar)
    {
        InitVRRig vrRig = avatar.AddComponent<InitVRRig>();
        vrRig.SetupAvatarController(rigConfig, avatar);
        var animator = avatar.GetComponent<Animator>();
        networkAvatar.Init(animator);
        
        if (IsOwner)
        {
            vrRig.ConnectTrackers();
            Debug.Log("init vr rig");
        }
        else
        {
            vrRig.Deactivate();
            Debug.Log("deactivate vr rig");
        }
    }

}
