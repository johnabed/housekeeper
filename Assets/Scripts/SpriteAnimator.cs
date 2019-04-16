using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class SpriteAnimator : Editor
{
    [UnityEditor.MenuItem("TestAnim/Create Test")]
    public static void Animate() {
        
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites"); // load all sprites in "assets/Resources/sprite" folder

        float frameRate = 12f;
        int startIndex = sprites[0].name.Length - 4;
        string spriteName = sprites[0].name.Substring(0, startIndex - 1);
        
        if(!AssetDatabase.IsValidFolder("assets/Resources/Sprites/" + spriteName))
        {
            AssetDatabase.CreateFolder("assets/Resources/Sprites", spriteName);
        }
       
     
        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";

        Dictionary<string, List<Sprite>> animList = new Dictionary<string, List<Sprite>>();
        animList["hurt"] = new List<Sprite>();
        animList["spellcast_down"] = new List<Sprite>();
        animList["spellcast_left"] = new List<Sprite>();
        animList["spellcast_right"] = new List<Sprite>();
        animList["spellcast_up"] = new List<Sprite>();
        animList["shoot_down"] = new List<Sprite>();
        animList["shoot_left"] = new List<Sprite>();
        animList["shoot_right"] = new List<Sprite>();
        animList["shoot_up"] = new List<Sprite>();
        animList["slash_down"] = new List<Sprite>();
        animList["slash_left"] = new List<Sprite>();
        animList["slash_right"] = new List<Sprite>();
        animList["slash_up"] = new List<Sprite>();
        animList["thrust_down"] = new List<Sprite>();
        animList["thrust_left"] = new List<Sprite>();
        animList["thrust_right"] = new List<Sprite>();
        animList["thrust_up"] = new List<Sprite>();
        animList["walk_down"] = new List<Sprite>();
        animList["walk_left"] = new List<Sprite>();
        animList["walk_right"] = new List<Sprite>();
        animList["walk_up"] = new List<Sprite>();
        animList["idle_down"] = new List<Sprite>();
        animList["idle_left"] = new List<Sprite>();
        animList["idle_right"] = new List<Sprite>();
        animList["idle_up"] = new List<Sprite>();
        
        foreach (var spr in sprites)
        {
            if (spr.name.Substring(startIndex, 2) == "hu")
            {
                animList["hurt"].Add(spr);
            }
            else if (spr.name.Substring(startIndex, 2) == "sc")
            {
                if (spr.name.Substring(startIndex+3, 1) == "d") animList["spellcast_down"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "l") animList["spellcast_left"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "r") animList["spellcast_right"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "t") animList["spellcast_up"].Add(spr);
            }
            else if (spr.name.Substring(startIndex, 2) == "sh")
            {
                if (spr.name.Substring(startIndex+3, 1) == "d") animList["shoot_down"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "l") animList["shoot_left"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "r") animList["shoot_right"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "t") animList["shoot_up"].Add(spr);
            }
            else if (spr.name.Substring(startIndex, 2) == "sl")
            {
                if (spr.name.Substring(startIndex+3, 1) == "d") animList["slash_down"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "l") animList["slash_left"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "r") animList["slash_right"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "t") animList["slash_up"].Add(spr);
            }
            else if (spr.name.Substring(startIndex, 2) == "th")
            {
                if (spr.name.Substring(startIndex+3, 1) == "d") animList["thrust_down"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "l") animList["thrust_left"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "r") animList["thrust_right"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "t") animList["thrust_up"].Add(spr);
            }
            else if(spr.name.Substring(startIndex, 2) == "wc" && spr.name.Substring(startIndex+5, 1) == "0")
            {
                if (spr.name.Substring(startIndex+3, 1) == "d") animList["idle_down"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "l") animList["idle_left"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "r") animList["idle_right"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "t") animList["idle_up"].Add(spr);
            }
            else if (spr.name.Substring(startIndex, 2) == "wc")
            {
                if (spr.name.Substring(startIndex+3, 1) == "d") animList["walk_down"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "l") animList["walk_left"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "r") animList["walk_right"].Add(spr);
                if (spr.name.Substring(startIndex+3, 1) == "t") animList["walk_up"].Add(spr);
            }
            else
            {
                Debug.Log("ERROR IF HERE");
            }
        }

        foreach (var animTrack in animList)
        {
            Debug.Log(animTrack.Key + ": " + animTrack.Value.Count);
            AnimationClip animClip = new AnimationClip();
            animClip.frameRate = frameRate;   // FPS (set @ 12)
            
            AnimationClipSettings animClipSett = new AnimationClipSettings();
            animClipSett.loopTime = true;
            animClipSett.stopTime = animTrack.Value.Count / frameRate; //todo: this is not fixing the anim freeze
            animClipSett.keepOriginalPositionY = true;
            AnimationUtility.SetAnimationClipSettings(animClip, animClipSett);
            
            ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[animTrack.Value.Count];
            for(int i = 0; i < (animTrack.Value.Count); i++) {
                spriteKeyFrames[i] = new ObjectReferenceKeyframe();
                spriteKeyFrames[i].time = (float)i / frameRate;
                spriteKeyFrames[i].value = animTrack.Value.ElementAt(i);
            }
            AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);
            
            AssetDatabase.CreateAsset(animClip, "assets/Resources/Sprites/" + spriteName + "/" + spriteName + "_" + animTrack.Key + ".anim");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}