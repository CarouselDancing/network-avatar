/*
MonoBehavior to synchronize the root position and local rotations of a Humanoid Animator via the MirrorSDK
Based on NetworkPlayer from mirror-testbed
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using System.Linq;


public class NetworkAgent : NetworkBehaviour
{


    protected List<Quaternion> rotations = new List<Quaternion>();
    protected Vector3 _rootPos;
    Quaternion _rootRot;

    // bones of the avatar
    public List<HumanBodyBones> trackedBones;
    protected Animator anim;
    public List<Transform> bones;
    public Transform root;
    public int updateFrameRate = 1;
    public int nextUpdateCounter;
    public bool initOnStart = true;
    public bool initialized = false;
    public string armatureName = "Armature";
    public bool useRootRotation = true;

    void Start()
    {
        if (initOnStart)
        {
            Init(GetComponent<Animator>());
        }
    }
    virtual public bool Init(Animator _anim)
    {
        anim = _anim;
        // only the owner has the XR Rig
        nextUpdateCounter = updateFrameRate;
        bones = new List<Transform>();
        root = anim.GetBoneTransform(HumanBodyBones.Hips);
        foreach (var b in trackedBones)
        {
            var t = anim.GetBoneTransform(b);
            if (t != null) bones.Add(t);
        }
        if (armatureName != ""){
            var armature = GetComponentsInChildren<Transform>().First(x => x.name == armatureName);
            if (armature != null) bones.Add(armature);
        }
        anim.enabled = false;
        Debug.Log("Init Network Agent");
        initialized = true;
        return initialized;
    }

    void Update()
    {

        if (!initialized) return;
        if (nextUpdateCounter > 0)
        {
            nextUpdateCounter= nextUpdateCounter -1;
            return;
        }

        if ( isServer)
        {

            var rots = new List<Quaternion>();
            //write vars 
            foreach (var b in bones)
            {
                rots.Add(b.localRotation);
            }
            UpdateSyncvars(root.position, root.rotation, rots);
        }
        else // if we're the instance of a remote player, get the synced variable and set it
        {
            if (bones.Count == rotations.Count)
            {
                for (int i = 0; i < rotations.Count; i++)
                {
                    bones[i].localRotation = rotations[i];
                }
                root.position = _rootPos;
                if(useRootRotation)root.rotation = _rootRot;
            }
        }
        nextUpdateCounter = updateFrameRate;

    }


    [ClientRpc]
    void UpdateSyncvars(Vector3 rootPos, Quaternion rootRot, List<Quaternion> _rotations)
    {
        _rootPos = rootPos;
        _rootRot = rootRot;
        rotations.Clear();
        foreach (var q in _rotations) {
            rotations.Add(q);
        }
    }

}