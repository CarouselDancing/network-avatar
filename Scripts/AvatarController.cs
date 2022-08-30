using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Linq;


public class AvatarController : MonoBehaviour
{

    public Transform headset;
    public Transform head;
    public Transform cameraTarget;
    public Transform hips;
    public Transform root;
    public Vector3 offset;
    public Vector3 cameraOffset;
    AvatarScaler scaler;
    public Rig rootRig;
    public Animator anim;
    public Transform leftController;
    public Transform rightController;
    public bool hideMesh = true;
    public bool active = true;
    public List<TrackerTarget> trackerTargets;
    // Start is called before the first frame update
    void Start()
    {
        scaler = GetComponent<AvatarScaler>();
        // assuming idle 
        cameraOffset = cameraTarget.position - head.position;
        var invQ = Quaternion.Inverse(Quaternion.Euler(0, root.rotation.eulerAngles.y, 0));
        cameraOffset = invQ * cameraOffset;
        offset = anim.GetBoneTransform(HumanBodyBones.Hips).position  - head.position; //root.position
        Debug.Log("AvatarController offset"+offset.ToString());
        offset = invQ * offset;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var t in trackerTargets){
            t.UpdateTarget();
        }
        if (active && headset != null) { 
            var rotation = Quaternion.Euler(0, root.rotation.eulerAngles.y, 0);
            var gcoffset = rotation * cameraOffset;
            var goffset = rotation * offset;
            root.position = headset.position - new Vector3(0, gcoffset.y, 0) + new Vector3(goffset.x, goffset.y, goffset.z);
            var srcrotation = headset.rotation.eulerAngles;
            root.rotation = Quaternion.Euler(0, srcrotation.y, 0);
        }
        
        /*
        rightFootDst.position = rightFootSrc.position;
        leftFootDst.position = leftFootSrc.position;
        leftFootDst.rotation = Quaternion.Inverse(leftRotOffset) * leftFootSrc.rotation;
        rightFootDst.rotation = Quaternion.Inverse(rightRotOffset) * rightFootSrc.rotation;
        */


    }

    public void AdaptModelToHuman()
    {
        rootRig.weight = 0;
        if(scaler != null)scaler.EstimateScale();
        // assuming idle animation
        cameraOffset = cameraTarget.position - head.position;
        var invQ = Quaternion.Inverse(Quaternion.Euler(0, root.rotation.eulerAngles.y, 0));
        cameraOffset = invQ * cameraOffset;
        offset = anim.GetBoneTransform(HumanBodyBones.Hips).position - head.position;
        offset = invQ * offset;
        //scaler.ScaleArms(anim, leftController.position, rightController.position);
        //offset = root.worldToLocalMatrix * offset;
        rootRig.weight = 1;
    }


    public void ToggleMeshRecursively(Transform t, bool enable)
    {
        var renderer = t.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Debug.Log("hide " + t.name);
            renderer.enabled = enable;
        }
        var srenderer = t.GetComponent<SkinnedMeshRenderer>();
        if (srenderer != null)
        {
            Debug.Log("hide " + t.name);
            srenderer.enabled = enable;
        }

        for (int i = 0; i < t.childCount; i++)
        {
            ToggleMeshRecursively(t.GetChild(i), enable);
        }
    }
}
