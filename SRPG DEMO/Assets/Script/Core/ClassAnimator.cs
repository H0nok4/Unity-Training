using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceDirection { Up, Down, Left, Right }
public class ClassAnimator : MonoBehaviour//角色自定义动画播放器
{
    //这里用来储存所需要的sprite List，对应不同的自定义动画。
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;

    //
    public float moveX { get; set; }
    public float moveY { get; set; }

    public bool isWalked { get; set; }

    public bool prvIsWalked;

    //这里储存着不同种类的自定义动画
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkLeftAnim;
    SpriteAnimator walkRightAnim;

    //
    SpriteAnimator currentAnim;//这个变量用来调用目前播放的是哪个动画

    SpriteRenderer spriteRenderer;//播放动画所需要的spriteRenderer组件，播放动画需要用它切换目标的sprite

    [SerializeField] FaceDirection defaultDirection = FaceDirection.Down;//NPC的默认朝向，默认为下

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();//获取当前物体的sprteRenderer组件。
        //初始化该角色所拥有的各个动画
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);

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

    public void SetFaceDirection(FaceDirection faceDirection)//设置NPC初始化的动画朝向
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

    public FaceDirection DefaultFaceDirection//向外暴露NPC实例的默认朝向
    {
        get => defaultDirection;
    }
}
