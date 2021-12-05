using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
public class NetworkAvatar : NetworkBehaviour
{



    [SyncVar]
    public List<Quaternion> rotations;
    [SyncVar]
    public Vector3 rootPos;

    // bones of the avatar
    public List<HumanBodyBones> trackedBones;
    Animator anim;
    public List<Transform> bones;
    public Transform root;


    // https://mirror-networking.gitbook.io/docs/guides/communications/networkbehaviour-callbacks

    public override void OnStartAuthority()
    {
        Debug.Log("Object has authority");
    }

    void Start()
    {
        // only the owner has the XR Rig

        anim = GetComponent<Animator>();
        bones = new List<Transform>();
        root = anim.GetBoneTransform(HumanBodyBones.Hips);
        foreach (var b in trackedBones)
        {
            var t = anim.GetBoneTransform(b);
            if (t!= null) bones.Add(t);
        }
        anim.enabled = IsOwner;

        InitVRRig vrRig = GetComponent<InitVRRig>();
        if (vrRig != null)
        {
            if (IsOwner)
            {
                vrRig.Init();
            }
            else
            {
                vrRig.Deactivate();
            }
        }
    }


    public bool IsOwner => isLocalPlayer;

    [Command]
    void UpdateSyncvars(Vector3 _rootPos, List<Quaternion> _rotations)
    {
        rootPos = _rootPos;
        rotations = _rotations;
    }

    void Update()
    {

        if (IsOwner)
        {

            var rots = new List<Quaternion>();
            //write vars 
            foreach (var b in bones)
            {
                rots.Add(b.localRotation);
            }
            UpdateSyncvars(root.position, rots);
        }
        else // if we're the instance of a remote player, get the synced variable and set it
        {
            if (bones.Count == rotations.Count)
            {
                for (int i = 0; i < rotations.Count; i++)
                {
                    bones[i].localRotation = rotations[i];
                }
                root.position = rootPos;
            }
        }

       
    }

    private void OnGUI()
    {
        if (IsOwner)
        {
            GUILayout.BeginArea(new Rect(200, 10, 300, 300));
            GUILayout.Label($"Network ID: {this.netId}");
            GUILayout.EndArea();
        }
    }
}