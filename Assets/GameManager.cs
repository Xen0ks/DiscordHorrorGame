using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool screamered;

    public GameObject jumpScareCamera;
    public GameObject crosshair;
    public GameObject player;
    public Animator fade;

    public bool switchOn;
    public Transform redSwitch;



    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (screamered && Input.anyKeyDown)
        {
            SceneManager.LoadScene(0);
        }
    }

    public IEnumerator Screamer()
    {
        jumpScareCamera.SetActive(true);
        crosshair.SetActive(true);
        player.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        fade.SetTrigger("Fade");
        screamered = true;
    }

    public void SwitchOn()
    {
        switchOn = true;
        redSwitch.localRotation = Quaternion.Euler(-52, 0, 0);
    }
}

