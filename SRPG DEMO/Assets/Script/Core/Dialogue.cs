using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Dialogue
{
    public Sprite dialogueCharacterSprite;
    public string CharacterName;
    public int dialoguePosition;
    [TextArea]
    public string dialogueText;

}
