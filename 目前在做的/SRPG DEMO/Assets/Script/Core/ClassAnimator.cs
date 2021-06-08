using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceDirection { Up, Down, Left, Right }
[Serializable]
public class ClassAnimator : MonoBehaviour//角色自定义动画播放器
{
    public float moveX { get; set; }
    public float moveY { get; set; }

    public bool isWalked { get; set; }

    public bool prvIsWalked;

    //这里储存着不同种类的自定义动画
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkLeftAnim;
    SpriteAnimator walkRightAnim;

    SpriteAnimator currentAnim;

    SpriteRenderer spriteRenderer;//播放动画所需要的spriteRenderer组件，播放动画需要用它切换目标的sprite

    [SerializeField] FaceDirection defaultDirection = FaceDirection.Down;//默认朝向，默认为下

    public void InitAnimator(ClassInfo classInfo)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //初始化该角色所拥有的各个动画
        walkDownAnim = new SpriteAnimator(classInfo.walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(classInfo.walkUpSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(classInfo.walkLeftSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(classInfo.walkRightSprites, spriteRenderer);

        SetFaceDirection(defaultDirection);
        currentAnim = walkDownAnim;//初始默认的动画设置
    }

    private void Update()
    {
        var prvAnim = currentAnim;//把当前的动画是什么储存起来，用于下面检测动画是否有变动，有变动的话就播放动画

        if (moveX > 0)//moveX和moveY由PlayerController控制，输入Vertical和Horizontal上的值对应了moveX和moveY
            currentAnim = walkRightAnim;
        else if (moveX < 0)
            currentAnim = walkLeftAnim;
        else if (moveY > 0)
            currentAnim = walkUpAnim;
        else if (moveY < 0)
            currentAnim = walkDownAnim;

        if (prvAnim != currentAnim || prvIsWalked != isWalked)//检测当前动画/是否行动的变量是否改变，如果改变就初始化动画
        {
            currentAnim.Start();
        }


        if (isWalked)
        {
            currentAnim.HandleUpdate();
        }
        else
        {
            spriteRenderer.sprite = currentAnim.frames[0];
        }

        prvIsWalked = isWalked;
    }

    public void SetFaceDirection(FaceDirection faceDirection)//设置初始化的动画朝向
    {
        if (faceDirection == FaceDirection.Right)
            moveX = 1;
        else if (faceDirection == FaceDirection.Left)
            moveX = -1;
        else if (faceDirection == FaceDirection.Up)
            moveY = 1;
        else if (faceDirection == FaceDirection.Down)
            moveY = -1;
    }

    public FaceDirection DefaultFaceDirection//默认朝向
    {
        get => defaultDirection;
    }
}
