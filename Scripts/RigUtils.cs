/*Assumes Ready Player Me avatar*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RPMIKRigBuilder
{
    bool _activateFootRig;
    bool _activateRig;
    RuntimeAnimatorController _animController;
    Dictionary<string, Transform> transformMap;
    string headLayerName = "head";
    List<string> headMeshClippingNames = new List<string> () {"Avatar_Renderer_EyeLeft", "Avatar_Renderer_EyeRight", "Avatar_Renderer_Hair","Avatar_Renderer_Head","Avatar_Renderer_Teeth"};
    int headLayerIndex;

    public RPMIKRigBuilder(RuntimeAnimatorController animController, bool activateFootRig = true, bool activateRig=true)
    {
        _animController = animController;
        _activateFootRig = activateFootRig;
        _activateRig = activateRig;
        headLayerIndex = LayerMask.NameToLayer(headLayerName);
    }

    public CharacterRigConfig BuildConfig(GameObject avatar){

        var config = avatar.AddComponent<CharacterRigConfig>();
        config.Meshes = new List<Transform>();
        for (int i = 0; i < avatar.transform.childCount; i++)
        {
            var t = avatar.transform.GetChild(i);
            if (t.name == "Armature") continue;
            config.Meshes.Add(t);
        }
        transformMap = new Dictionary<string, Transform>();
        UpdateTransformMap(avatar.transform);
        
        config.Root = transformMap["Hips"];
        config.Head = transformMap["Head"];
        config.ToeTip = transformMap["RightToe_End"];
        config.LeftHand = transformMap["LeftHand"];
        config.RightHand = transformMap["RightHand"];

        
        return config;
    }

    public void ClipHeadMeshes(){
        foreach(var name in headMeshClippingNames){
            if (transformMap.ContainsKey(name)) transformMap[name].gameObject.layer = headLayerIndex;
        }
    }

    public CharacterRigConfig Build(GameObject avatar, bool isOwner=false)
    {
        var config = BuildConfig(avatar);
        if (isOwner) ClipHeadMeshes();
        var rigBuilder = avatar.AddComponent<RigBuilder>();
        CreateCameraTarget(ref config);
        CreateRootRig(avatar.transform, rigBuilder, ref config);
        CreateHeadRig(avatar.transform, rigBuilder, ref config);
        CreateArmRig(avatar.transform, rigBuilder, ref config);
        CreateLegRig(avatar.transform, rigBuilder,  ref config);
        var anim = avatar.GetComponent<Animator>();
        anim.runtimeAnimatorController = _animController;
        rigBuilder.Build();
        return config;
    }

    public void UpdateTransformMap(Transform node)
    {
        transformMap[node.name] = node;
        for (int i = 0; i < node.childCount; i++)
        {
            UpdateTransformMap(node.GetChild(i));
        }
    }

    void CreateCameraTarget(ref CharacterRigConfig config)
    {
        var cameraTarget = new GameObject("CameraTarget");
        cameraTarget.transform.position = (transformMap["LeftEye"].position + transformMap["RightEye"].position) /2;
        cameraTarget.transform.parent = transformMap["Head"];
        
            
        config.CameraTarget = cameraTarget.transform;
    }


    void CreateRootRig(Transform root, RigBuilder rigBuilder, ref CharacterRigConfig config)
    {
        var rootRigObject = new GameObject("RootRig");
        rootRigObject.transform.parent = root;
        var rootRig = rootRigObject.AddComponent<Rig>();
        rootRig.weight = _activateRig ? 1 : 0;
        var rootTarget = new GameObject("RootTarget");
        rootTarget.transform.parent = rootRigObject.transform;
        rootTarget.transform.position = transformMap["Hips"].position;
        rootTarget.transform.rotation = transformMap["Hips"].rotation;

        var rootPosConstraint = new GameObject("RootPosition");
        rootPosConstraint.transform.parent = rootRigObject.transform;
        var posConstraint = rootPosConstraint.AddComponent<MultiPositionConstraint>();
        var posSrcArray = new WeightedTransformArray() { new WeightedTransform(rootTarget.transform, 1) };
        posConstraint.data.sourceObjects = posSrcArray;
        posConstraint.data.constrainedObject = config.Root;
        posConstraint.data.constrainedXAxis = true;
        posConstraint.data.constrainedYAxis = true;
        posConstraint.data.constrainedZAxis = true;

        var rootRotConstraint = new GameObject("RootRotation");
        rootRotConstraint.transform.parent = rootRigObject.transform;
        var rotConstraint = rootRotConstraint.AddComponent<MultiRotationConstraint>();
        var rotSrcArray = new WeightedTransformArray() { new WeightedTransform(rootTarget.transform, 1) };
        rotConstraint.data.sourceObjects = rotSrcArray;
        rotConstraint.data.constrainedObject = config.Root;
        rotConstraint.data.constrainedXAxis = true;
        rotConstraint.data.constrainedYAxis = true;
        rotConstraint.data.constrainedZAxis = true;
        config.RootRig = rootRig;
        config.RootTarget = rootTarget.transform;
        var layer = new RigLayer(rootRig);
        rigBuilder.layers.Add(layer);
    }

    Transform CreateTwoBoneIKConstraint(GameObject parent, string root, string mid, string tip, string targetName)
    {
        var o = new GameObject(targetName);
        o.transform.parent = parent.transform;
        o.transform.position = transformMap[tip].position;
        o.transform.rotation = transformMap[tip].rotation;
        var c = o.AddComponent<TwoBoneIKConstraint>();
        c.data.root = transformMap[root];
        c.data.mid = transformMap[mid];
        c.data.tip = transformMap[tip];
        c.data.target = o.transform;
        c.data.targetPositionWeight = 1;
        c.data.targetRotationWeight = 1;
        //c.data.hint = transformMap["RightHand"];
        c.data.target = o.transform;
        return o.transform;
    }


    void CreateArmRig(Transform root, RigBuilder rigBuilder, ref CharacterRigConfig config)
    {
        var armRigObject = new GameObject("ArmRig");
        armRigObject.transform.parent = root;
        var armRig = armRigObject.AddComponent<Rig>();
        armRig.weight = _activateRig ? 1 : 0;
        config.RightHandIKTarget = CreateTwoBoneIKConstraint(armRigObject, "RightArm", "RightForeArm", "RightHand", "RightHandTarget");

        config.LeftHandIKTarget = CreateTwoBoneIKConstraint(armRigObject, "LeftArm", "LeftForeArm", "LeftHand", "LeftHandTarget");

        var layer = new RigLayer(armRig);
        rigBuilder.layers.Add(layer);
    }

    void CreateHeadRig(Transform root, RigBuilder rigBuilder, ref CharacterRigConfig config)
    {
        var headRigObject = new GameObject("HeadRig");
        headRigObject.transform.parent = root;
        var headRig = headRigObject.AddComponent<Rig>();

        config.HeadIKTarget = CreateTwoBoneIKConstraint(headRigObject, "Spine", "Spine2", "Head", "HeadTarget");
        headRig.weight = _activateRig ? 1 : 0;
        var layer = new RigLayer(headRig);
        rigBuilder.layers.Add(layer);
    }
    void CreateLegRig(Transform root, RigBuilder rigBuilder, ref CharacterRigConfig config)
    {
        var legRigObject = new GameObject("LegRig");
        legRigObject.transform.parent = root;
        var legRig = legRigObject.AddComponent<Rig>();

        
        legRig.weight = ( _activateRig &&_activateFootRig) ? 1 : 0;
       
        config.RightFootIKTarget = CreateTwoBoneIKConstraint(legRigObject, "RightUpLeg", "RightLeg", "RightFoot", "RightFootTarget");

        config.LeftFootIKTarget = CreateTwoBoneIKConstraint(legRigObject, "LeftUpLeg", "LeftLeg", "LeftFoot", "LeftFootTarget");

        var layer = new RigLayer(legRig);
        rigBuilder.layers.Add(layer);
    }
}
