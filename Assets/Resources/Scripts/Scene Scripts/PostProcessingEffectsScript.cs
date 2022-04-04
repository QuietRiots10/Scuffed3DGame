using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingEffectsScript : MonoBehaviour
{
    //Post Process Volumes
    PostProcessVolume Volume1;

    //Effects
    LensDistortion TimeDistortion;
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
        // Create an instance of a distorition (Time distortion)
        TimeDistortion = ScriptableObject.CreateInstance<LensDistortion>();
        TimeDistortion.enabled.Override(false);
        TimeDistortion.intensity.Override(TimeDistortIntensity);
        TimeDistortion.intensityX.Override(1);
        TimeDistortion.intensityY.Override(1);

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
        
        float count = 0;

        //Forward Distortion
        if (forward)
        {
            //Use QuickVolume to create volumes
            Volume1 = PostProcessManager.instance.QuickVolume(gameObject.layer, 98f, TimeVignette , TimeDistortion);

            //Vingette enabled
            TimeVignette.enabled.Override(true);
            TimeDistortion.enabled.Override(true);

            count = 0;
            while (count < 0.25f)
            {
                //Change distortion intensity
                TimeDistortIntensity = 240 * count;
                TimeDistortion.intensity.Override(TimeDistortIntensity);

                //Change Vignette Intensity
                TimeVignetteIntensity = 1.44f * count;
                TimeVignette.intensity.Override(TimeVignetteIntensity);

                count = count + 5 * Time.deltaTime;
                yield return new WaitForSecondsRealtime(0.0000001f);
            }
        }

        //Reverse Distortion
        else if (!forward)
        {
            count = 0.25f;
            while (count > 0)
            {

                // Change distortion intensity
                TimeDistortIntensity = 240 * count;
                TimeDistortion.intensity.Override(TimeDistortIntensity);

                //Change Vignette Intensity
                TimeVignetteIntensity = 1.44f * count;
                TimeVignette.intensity.Override(TimeVignetteIntensity);

                count = count - Time.deltaTime;
                yield return new WaitForSecondsRealtime(0.0000001f);
            }

            //Vignette Disabled
            TimeVignette.enabled.Override(false);
            TimeVignetteIntensity = 0;
            TimeVignette.intensity.Override(0);
            

            //Distortion disabled
            TimeDistortion.enabled.Override(false);
            

            //RuntimeUtilities.DestroyVolume(Volume1, true, false);
        }

        yield return null;
    }

    public IEnumerator DamageEffect()
    {
        PostProcessVolume Volume2 = PostProcessManager.instance.QuickVolume(gameObject.layer, 99f, DamageVignette);

        DamageVignette.enabled.Override(true);
        
        float count = 0;
        DamageVignetteIntensity = 0;

        //Increase intensity
        while (count <= 0.2)
        {
            //Change Vignette Intensity
            DamageVignetteIntensity = 1.86f * count;
            DamageVignette.intensity.Override(DamageVignetteIntensity);
            count = count + Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(0.05f);

        //Decrease intensity
        while (count >= 0)
        {
            //Change Vignette Intensity
            DamageVignetteIntensity = 1.86f * count ;
            DamageVignette.intensity.Override(DamageVignetteIntensity);
            count = count - Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        DamageVignette.enabled.Override(false);
        //RuntimeUtilities.DestroyVolume(Volume2, true, false);
        yield return null;
    }

    public IEnumerator DeathEffect()
    {
        //Use QuickVolume to create volumes
        PostProcessVolume Volume3 = PostProcessManager.instance.QuickVolume(gameObject.layer, 99f, DamageVignette, DeathColorGrade);

        DamageVignette.enabled.Override(true);
        DeathColorGrade.enabled.Override(true);

        float count = 0;
        DamageVignetteIntensity = 0;
        DeathColorGrade.colorFilter.Override(new Color(1f, 0.2867924f, 0.2867924f));

        //Increase intensity of Vignette and change color of filter
        while (count <= 0.5f)
        {
            //Change Vignette Intensity
            DamageVignetteIntensity = 1.2f * count;
            DamageVignette.intensity.Override(DamageVignetteIntensity);
            count = count + Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}