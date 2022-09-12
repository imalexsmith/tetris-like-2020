//  ==================================================================================================================
//    <description>NestAnimClips.cs - Nesting AnimationClips inside an AnimationContoller.</description>
//  <author>ZombieGorilla for Unity Forums</author>
//     <version>1.0</version>
//    <date>2016-02-14</date>
//  ==================================================================================================================
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;


public class NestAnimClips : MonoBehaviour
{
    [MenuItem("Assets/Nest AnimClips in Controller")]
    static public void nestAnimClips()
    {
        UnityEditor.Animations.AnimatorController anim_controller = null;
        List<AnimationClip> clips = new List<AnimationClip>();
        var objs = Selection.objects;

        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].GetType() == typeof(UnityEditor.Animations.AnimatorController))
            {
                if (anim_controller != null)
                {
                    Debug.Log("<color=red>Please Select only ONE controller.</color>");
                    return;
                }
                else
                {
                    anim_controller = (UnityEditor.Animations.AnimatorController)objs[i];
                }
            }

            if (objs[i].GetType() == typeof(AnimationClip))
            {
                clips.Add((AnimationClip)objs[i]);
            }
        }

        if (anim_controller != null && clips.Count > 0)
        {
            foreach (AnimationClip ac in clips)
            {
                var new_ac = Object.Instantiate(ac) as AnimationClip;
                new_ac.name = ac.name;

                AssetDatabase.AddObjectToAsset(new_ac, anim_controller);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(new_ac));
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(ac));
            }
            Debug.Log("<color=orange>Added " + clips.Count.ToString() + " clips to controller: </color><color=yellow>" + anim_controller.name + "</color>");
        }
        else
        {
            Debug.Log("<color=red>Nothing done. Select a controller and anim clips to nest.</color>");
        }
    }
}
