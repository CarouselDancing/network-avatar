using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarScaler : MonoBehaviour
{

    public Transform cameraTarget;
    public Transform root;
    public Transform head;
    public Transform floor;
    public List<Transform> scaleTargets;
    public float scaleFactor;

    public void EstimateScale()
    {
        float targetHeight = cameraTarget.position.y;
        float figureHeight = head.position.y - floor.position.y;
        float rootHeight = root.position.y;
        scaleFactor = targetHeight / figureHeight;
        float targetRootHeight = rootHeight * scaleFactor;
        foreach (var t in scaleTargets)
        {
            t.localScale *= scaleFactor;
        }
        float delta = targetRootHeight - root.position.y;
        var p = new Vector3(transform.position.x, delta, transform.position.z);
        transform.position = p;
    }

    public void ScaleArms(Animator anim, Vector3 leftHandTarget, Vector3 rightHandTarget)
    {
        Transform leftShoulder = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        Transform leftElbow = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        Transform leftHand = anim.GetBoneTransform(HumanBodyBones.LeftHand);
        scaleArm(leftShoulder, leftElbow, leftHand.position, leftHandTarget);
        Transform rightShoulder = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        Transform rightElbow = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
        Transform rightHand = anim.GetBoneTransform(HumanBodyBones.RightHand);
        scaleArm(rightShoulder, rightElbow, rightHand.position, rightHandTarget);
    }

    void scaleArm(Transform shoulder, Transform elbow, Vector3 endEffector, Vector3 target)
    {
        var armDelta = endEffector - elbow.position;
        var targetDelta = target - elbow.position;

        var scaleFactor = targetDelta.magnitude / armDelta.magnitude;
        var newScale =  new Vector3(elbow.localScale.x, elbow.localScale.y* scaleFactor, elbow.localScale.z);
        elbow.localScale = newScale;
        //elbow.localScale += new Vector3(1, hScale, 1);

    }
}
