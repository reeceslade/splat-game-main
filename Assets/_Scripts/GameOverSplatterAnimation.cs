using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSplatterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void playAnimation()
    {
        animator.SetTrigger("GameOverAnim");
    }
}
