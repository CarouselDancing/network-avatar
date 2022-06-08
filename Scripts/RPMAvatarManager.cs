using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ReadyPlayerMe;

public class RPMAvatarManager : NetworkBehaviour
{
    public string AvatarURL = "";
    public GameObject go;
    public RuntimeAnimatorController animationController;
    public bool activateFootRig = false;
    protected NetworkAvatar networkAvatar;
    public bool IsOwner => isLocalPlayer;
    public bool initiated = false;
    virtual public void Start()
    {
        networkAvatar = GetComponent<NetworkAvatar>();
        if (IsOwner)
        {
            AvatarURL = GlobalGameState.GetInstance().config.rpmURL;
            if (AvatarURL != "")
            {

                SetupAvatarControllerFromRPM(AvatarURL);
            }
            else
            {
                Debug.Log("Error: avatar url is emtpy");
            }
        }

    }

    private void Update()
    {

        if (!initiated && !IsOwner && AvatarURL != "")
        {
            SetupAvatarControllerFromRPM(AvatarURL);
        }
        else if (initiated && IsOwner && AvatarURL != "")
        { //TODO replace with server side avatarmanagement
            UpdateSyncvars(AvatarURL);
        }
    }


    [Command]
    void UpdateSyncvars(string newurl)
    {
        AvatarURL = newurl;
        ChangeClientAvatar(newurl);
    }

    [ClientRpc]
    void ChangeClientAvatar(string newurl)
    {
        AvatarURL = newurl;
    }


    void SetupAvatarControllerFromRPM(string url)
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
        bool activateFootRig = GlobalGameState.GetInstance().config.activateFootTrackers;
        var ikRigBuilder = new RPMIKRigBuilder(animationController, activateFootRig);
        var config = ikRigBuilder.Build(avatar);
        SetupRig(config, avatar);
        Debug.Log($"Avatar loaded. [{Time.timeSinceLevelLoad:F2}]\n\n");
    }


    public void SetupRig(CharacterRigConfig config, GameObject avatar)
    {
        InitVRRig vrRig = avatar.AddComponent<InitVRRig>();
        vrRig.SetupAvatarController(config, avatar);
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

}
