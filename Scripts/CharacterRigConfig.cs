using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterRigConfig : MonoBehaviour
{
    public List<Transform> Meshes;
    public Rig RootRig;
    //ik rig targets
    public Transform LeftHandIKTarget;
    public Transform RightHandIKTarget;
    public Transform LeftFootIKTarget;
    public Transform RightFootIKTarget;
    public Transform HeadIKTarget;
    public Transform CameraTarget;
    public Transform RootTarget;

    //bones
    public Transform Root;
    public Transform Head;
    public Transform ToeTip;
    public Transform LeftHand;
    public Transform RightHand;


}
