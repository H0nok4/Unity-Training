using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaimonAnimator : MonoBehaviour
{
    //播放派蒙动画
    [SerializeField] List<Sprite> walkAnimations;
    [SerializeField] List<Sprite> jumpAnimations;
    [SerializeField] List<Sprite> smashAnimations;

    public bool isJumping;
    public bool isSmashing;

    SpriteRenderer paimonRenderer;

    SpriteAnimation curAnimation;

    SpriteAnimation walkAnimation;
    SpriteAnimation jumpAnimation;
    SpriteAnimation smashAnimation;
    private void Start()
    {
        paimonRenderer = GetComponent<SpriteRenderer>();
        //初始化Animation
        walkAnimation = new SpriteAnimation(walkAnimations, paimonRenderer,0.12f);
        jumpAnimation = new SpriteAnimation(jumpAnimations, paimonRenderer);
        smashAnimation = new SpriteAnimation(smashAnimations, paimonRenderer,0.15f);
        //初始化Animation结束
        curAnimation = walkAnimation;
        isJumping = false;
    }
    public void Update()
    {
        if (isSmashing)
        {
            curAnimation =  smashAnimation;
        }
        else if(isJumping)
        {
            curAnimation = jumpAnimation;
        }
        else
        {
            curAnimation = walkAnimation;
        }

        curAnimation.HandleUpdate();
    }

}
