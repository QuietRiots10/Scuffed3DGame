using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingEffectsScript : MonoBehaviour
{
    PostProcessVolume Volume;
    LensDistortion Distortion;
    Vignette Vignette;
    //Intensities
    float DistortIntensity = 0;
    float VignetteIntensity = 0;

    //Start
    void Start()
    {
        // Create an instance of a distorition
        Distortion = ScriptableObject.CreateInstance<LensDistortion>();
        Distortion.enabled.Override(true);
        Distortion.intensity.Override(DistortIntensity);
        Distortion.intensityX.Override(1);
        Distortion.intensityY.Override(1);

        Vignette = ScriptableObject.CreateInstance<Vignette>();
        Vignette.enabled.Override(false);
        Vignette.intensity.Override(0);
        Vignette.color.Override(new Color(0.172549f, 0.751017f, 0.945098f, 0.5607843f));

        // Use QuickVolume to create a volume with a priority of 100, and assign the distortion to this volume
        Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, Distortion, Vignette);
    }

    //Call Effect
    public void CallEffect(string EffectName, bool param1)
    {
        StartCoroutine(EffectName, param1);
    }
    
    //Parameter: Forward - Tells you how to play the effect (true = forward, false = backwards)
    IEnumerator TimeEffect(bool forward)
    {
        int count = 120;

        //Forward Distortion
        if (forward)
        {
            //Vingette enabled
            Vignette.enabled.Override(true);

            while (count > 0)
            {
                // Change distortion intensity
                DistortIntensity = DistortIntensity + 0.5f;
                Distortion.intensity.Override(DistortIntensity);

                //Change Vignette Intensity
                VignetteIntensity = VignetteIntensity + 0.003f;
                Vignette.intensity.Override(VignetteIntensity);

                count--;
                yield return new WaitForSecondsRealtime(0.0001f);
            }
        }

        //Reverse Distortion
        else if (!forward)
        {
            while (count > 0)
            {

                // Change distortion intensity
                DistortIntensity = DistortIntensity - 0.5f; ;
                Distortion.intensity.Override(DistortIntensity);

                //Change Vignette Intensity
                VignetteIntensity = VignetteIntensity - 0.003f;
                Vignette.intensity.Override(VignetteIntensity);

                count--;
                yield return new WaitForSecondsRealtime(0.0001f);
            }

            //Vignette Disabled
            Vignette.enabled.Override(false);
        }

        yield return null;
    }
}