using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carousel{
   

public class HandAnimationController : MonoBehaviour
{
    Animator animator;
    static string LEFT_HAND_PARAM ="LeftHandButtonPressed";
    static string RIGHT_HAND_PARAM ="RightHandButtonPressed";
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void OpenRightHand(){
        animator.SetBool(RIGHT_HAND_PARAM, false);
    }

    public void CloseRightHand(){
        animator.SetBool(RIGHT_HAND_PARAM, true);
    }
    public void OpenLeftHand(){
        animator.SetBool(LEFT_HAND_PARAM, false);
    }
    public void CloseLeftHand(){
        animator.SetBool(LEFT_HAND_PARAM, true);
    }
    }
}