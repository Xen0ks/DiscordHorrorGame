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
    public Transform replacePoint;

    public bool switchOn;
    public Transform redSwitch;

    public bool hasKey;

    public GameObject[] enableOnPower;



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
        if (player.GetComponent<CharacterController>().enabled)
        {
            replacePoint.position = player.transform.position;
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

    public IEnumerator End()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(0.6f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(1);
    }

    public void SwitchOn()
    {
        switchOn = true;
        redSwitch.localRotation = Quaternion.Euler(-52, 0, 0);

        foreach (var c in enableOnPower)
        {
            c.SetActive(true);
        }
    }
}

