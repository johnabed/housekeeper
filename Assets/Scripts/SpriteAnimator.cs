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
        string spriteName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(sprites[0]));
        
        if(!AssetDatabase.IsValidFolder("assets/Resources/Sprites/" + spriteName))
        {
            AssetDatabase.CreateFolder("assets/Resources/Sprites", spriteName);
        }
       
     
        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";

        Dictionary<string, List<Sprite>> animList = new Dictionary<string, List<Sprite>>();
        animList["hu"] = new List<Sprite>();
        animList["sc_d"] = new List<Sprite>();
        animList["sc_l"] = new List<Sprite>();
        animList["sc_r"] = new List<Sprite>();
        animList["sc_t"] = new List<Sprite>();
        animList["sh_d"] = new List<Sprite>();
        animList["sh_l"] = new List<Sprite>();
        animList["sh_r"] = new List<Sprite>();
        animList["sh_t"] = new List<Sprite>();
        animList["sl_d"] = new List<Sprite>();
        animList["sl_l"] = new List<Sprite>();
        animList["sl_r"] = new List<Sprite>();
        animList["sl_t"] = new List<Sprite>();
        animList["th_d"] = new List<Sprite>();
        animList["th_l"] = new List<Sprite>();
        animList["th_r"] = new List<Sprite>();
        animList["th_t"] = new List<Sprite>();
        animList["wc_d"] = new List<Sprite>();
        animList["wc_l"] = new List<Sprite>();
        animList["wc_r"] = new List<Sprite>();
        animList["wc_t"] = new List<Sprite>();
        
        foreach (var spr in sprites)
        {
            if (spr.name.Substring(0, 2) == "hu")
            {
                animList["hu"].Add(spr);
            }
            else
            {
                animList[spr.name.Substring(0, 4)].Add(spr);
            }
        }

        foreach (var animTrack in animList)
        {
            AnimationClip animClip = new AnimationClip();
            animClip.frameRate = 12;   // FPS
            ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[animTrack.Value.Count];
            for(int i = 0; i < (animTrack.Value.Count); i++) {
                spriteKeyFrames[i] = new ObjectReferenceKeyframe();
                spriteKeyFrames[i].time = i;
                spriteKeyFrames[i].value = animTrack.Value.ElementAt(i);
            }
            AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);
        
            AnimationClipSettings animClipSett = new AnimationClipSettings();
            animClipSett.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(animClip, animClipSett);

            AssetDatabase.CreateAsset(animClip, "assets/Resources/Sprites/" + spriteName + "/" + spriteName + "_" + animTrack.Key + ".anim");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}