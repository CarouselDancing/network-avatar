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
    public Transform HeadIKTarget;
    public Transform CameraTarget;

    //bones
    public Transform Root;
    public Transform Head;
    public Transform ToeTip;


}
