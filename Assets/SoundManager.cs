using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource ambiance;
    public AudioSource music;
    public AudioSource sfx;

    [Header("SFX")]
    public AudioClip repere1;
    public AudioClip repere2;

    public AudioClip uiSfx;

    public AudioClip doorSfx;

    public AudioClip interactSfx;

    public AudioClip notifSfx;

    public AudioClip screamerSfx;


    public static SoundManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Repere()
    {
        Debug.Log("Repere Sfx");
        int i = Random.Range(0, 2);

        if(i == 0)
        {
            sfx.PlayOneShot(repere1);
        }
        else
        {
            sfx.PlayOneShot(repere2);
        }
    }

    public void DoorSfx()
    {
        sfx.PlayOneShot(doorSfx);
    }

    public void InteractSfx()
    {
        sfx.PlayOneShot(interactSfx);
    }

    public void UiSfx()
    {
        sfx.PlayOneShot(uiSfx);
    }

    public void NotifSfx()
    {
        sfx.PlayOneShot(notifSfx);
    }

    public void ScreamerSfx()
    {
        ambiance.Stop();
        music.Stop();
        sfx.Stop();
        sfx.PlayOneShot(screamerSfx);
    }


}

