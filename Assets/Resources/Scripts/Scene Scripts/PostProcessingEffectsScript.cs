using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingEffectsScript : MonoBehaviour
{
    //Volumes
    PostProcessVolume Volume;
    PostProcessVolume Volume2;
    PostProcessVolume Volume3;

    //Effects
    LensDistortion Distortion;
    Vignette TimeVignette;
    Vignette DamageVignette;
    ColorGrading DeathColorGrade;

    //Intensities and Parameters
    float TimeDistortIntensity = 0;
    float TimeVignetteIntensity = 0;
    float DamageVignetteIntensity = 0;
    Color DeathColorGradeColorFilter = new Color(1, 1 ,1);

    //Start
    void Start()
    {
        // Create an instance of a distorition
        Distortion = ScriptableObject.CreateInstance<LensDistortion>();
        Distortion.enabled.Override(true);
        Distortion.intensity.Override(TimeDistortIntensity);
        Distortion.intensityX.Override(1);
        Distortion.intensityY.Override(1);

        //Create an instance of a Vignette (Time Vignette)
        TimeVignette = ScriptableObject.CreateInstance<Vignette>();
        TimeVignette.enabled.Override(false);
        TimeVignette.intensity.Override(0);
        TimeVignette.color.Override(new Color(0.172549f, 0.751017f, 0.945098f, 0.5607843f));

        //Create an instance of a Vignette (Damage Vignette)
        DamageVignette = ScriptableObject.CreateInstance<Vignette>();
        DamageVignette.enabled.Override(false);
        DamageVignette.intensity.Override(0);
        DamageVignette.color.Override(Color.red);

        //Create an instance of ColorGrading (DeathColorGrade)
        DeathColorGrade = ScriptableObject.CreateInstance<ColorGrading>();
        DeathColorGrade.enabled.Override(false);
        DeathColorGrade.colorFilter.Override(new Color(1f, 0.2867924f, 0.2867924f));

        
    }
    
    //Parameter: Forward - Tells you how to play the effect (true = forward, false = backwards)
    public IEnumerator TimeEffect(bool forward)
    {
        int count = 120;

        //Forward Distortion
        if (forward)
        {
            //Use QuickVolume to create volumes
            Volume2 = PostProcessManager.instance.QuickVolume(gameObject.layer, 98f, TimeVignette, Distortion);

            //Vingette enabled
            TimeVignette.enabled.Override(true);
            Distortion.enabled.Override(true);

            while (count > 0)
            {
                //Change distortion intensity
                TimeDistortIntensity = TimeDistortIntensity + 0.5f;
                Distortion.intensity.Override(TimeDistortIntensity);

                //Change Vignette Intensity
                TimeVignetteIntensity = TimeVignetteIntensity + 0.003f;
                TimeVignette.intensity.Override(TimeVignetteIntensity);

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
                TimeDistortIntensity = TimeDistortIntensity - 0.5f; ;
                Distortion.intensity.Override(TimeDistortIntensity);

                //Change Vignette Intensity
                TimeVignetteIntensity = TimeVignetteIntensity - 0.003f;
                TimeVignette.intensity.Override(TimeVignetteIntensity);

                count--;
                yield return new WaitForSecondsRealtime(0.0001f);
            }

            //Vignette Disabled
            TimeVignette.enabled.Override(false);
            Distortion.enabled.Override(false);
            RuntimeUtilities.DestroyVolume(Volume2, false);
        }

        yield return null;
    }

    public IEnumerator DamageEffect()
    {
        Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 99f, DamageVignette);

        DamageVignette.enabled.Override(true);
        
        int count = 0;
        DamageVignetteIntensity = 0;

        //Increase intensity
        while (count <= 60)
        {
            //Change Vignette Intensity
            DamageVignetteIntensity = DamageVignetteIntensity + 0.0062f;
            DamageVignette.intensity.Override(DamageVignetteIntensity);
            count++;
            yield return new WaitForSecondsRealtime(0.0001f);
        }

        yield return new WaitForSecondsRealtime(0.05f);

        //Decrease intensity
        while (count >= 0)
        {
            //Change Vignette Intensity
            DamageVignetteIntensity = DamageVignetteIntensity - 0.0062f;
            DamageVignette.intensity.Override(DamageVignetteIntensity);
            count--;
            yield return new WaitForSecondsRealtime(0.0001f);
        }

        DamageVignette.enabled.Override(false);
        RuntimeUtilities.DestroyVolume(Volume, false);
        yield return null;
    }

    public IEnumerator DeathEffect()
    {
        //Use QuickVolume to create volumes
        Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 99f, DamageVignette, DeathColorGrade);

        DamageVignette.enabled.Override(true);
        DeathColorGrade.enabled.Override(true);

        int count = 0;
        DamageVignetteIntensity = 0;
        DeathColorGrade.colorFilter.Override(new Color(1f, 0.2867924f, 0.2867924f));

        //Increase intensity of Vignette and change color of filter
        while (count <= 40)
        {
            //Change Vignette Intensity
            DamageVignetteIntensity = DamageVignetteIntensity + 0.015f;
            DamageVignette.intensity.Override(DamageVignetteIntensity);
            count++;
            yield return new WaitForSecondsRealtime(0.0001f);
        }
        yield return null;
    }
}