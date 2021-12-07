/*
MonoBehavior to synchronize the root position and local rotations of a Humanoid Animator via the MirrorSDK
Based on NetworkPlayer from mirror-testbed
*/
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
    public bool visServer = false;
    public int updateFrameRate = 1;
    public int nextUpdateCounter;

    void Start()
    {
        // only the owner has the XR Rig
        nextUpdateCounter = updateFrameRate;
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

        
        if (nextUpdateCounter > 0)
        {
            nextUpdateCounter= nextUpdateCounter -1;
            return;
        }

        if (!visServer && !isClient) return;
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
        nextUpdateCounter = updateFrameRate;

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