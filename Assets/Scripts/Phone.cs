using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{

    public Text timeText;
    public Light flash;
    public MouseLook mL;

    public bool inspected = false;
    public Transform originalPos;
    public Transform inspectPos;
    public GameObject crosshair;

    public GameObject SettingsPanel;
    public GameObject InteractPanel;

    public bool batteryTrigger = false;
    GameObject battery;
    bool hasBattery;
    public bool powerSwitchTrigger;

    public Transform batteryUI;
    public Transform barPrefab;

    public int batteryLevel = 4;

    bool flashOn = true;

    Vector3 vel = Vector3.zero;

    private void Start()
    {
        batteryLevel = 4;
        UpdateBattery();
    }

    void Update()
    {
        // Récupérer l'heure actuelle du système
        DateTime currentTime = DateTime.Now;

        // Afficher l'heure dans la console
        timeText.text= currentTime.ToString("HH:mm");

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            CloseSets();
            inspected = !inspected;
            mL.enabled = !inspected;
            if (inspected)
            {
                Cursor.lockState = CursorLockMode.None;
                crosshair.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                crosshair.SetActive(true);

            }
        }

        if (inspected)
        {
            transform.position = Vector3.SmoothDamp(transform.position, inspectPos.position, ref vel, 0.4f);
            transform.rotation = inspectPos.rotation;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, originalPos.position, ref vel, 0.4f);
            transform.rotation = originalPos.rotation;
        }

    }


    public IEnumerator BatteryDeload()
    {
        while(batteryLevel > 0)
        {
            if (hasBattery || batteryLevel <= 0) yield break;
            yield return new WaitForSeconds(30);
            if(flashOn) batteryLevel--;
            if (batteryLevel <= 0)
            {
                batteryLevel = 0;
                if (flashOn) ToggleFlash();
            }
            UpdateBattery();

        }
    }

    public IEnumerator BatteryReload()
    {
        while (batteryLevel > 4 && hasBattery)
        {
            yield return new WaitForSeconds(45);
            if(batteryLevel < 4)
            {
                batteryLevel++;
            }
            UpdateBattery();
        }
    }

    void UpdateBattery()
    {
        foreach(Transform b in batteryUI)
        {
            Destroy(b.gameObject);
        }
        for(int i = 0; i < batteryLevel; i++)
        {
            Instantiate(barPrefab, batteryUI);
        }

    }



    public void InteractButton()
    {
        InteractPanel.SetActive(false);
        GatherBattery();
        SwitchSwitch();
    }


    public void TriggerSwitch(bool b)
    {
        InteractPanel.SetActive(b);
        powerSwitchTrigger = b;
    }

    public void SwitchSwitch()
    {
        if (!powerSwitchTrigger || GameManager.instance.switchOn) return;

        GameManager.instance.SwitchOn();
    }



    public void TriggerBattery(bool b, GameObject g)
    {
        batteryTrigger = b;
        battery = g;
        InteractPanel.SetActive(b);
    }

    public void GatherBattery()
    {
        if (!batteryTrigger) return;
        Destroy(battery);
        hasBattery = true;
        StartCoroutine(BatteryReload());
        StopCoroutine(BatteryDeload());
        StartCoroutine(BatteryDeload());
    }



    public void ToggleFlash()
    {
        if (!flashOn && batteryLevel <= 0) return;

        flash.gameObject.SetActive(!flash.gameObject.activeSelf);
        flashOn = flash.gameObject.activeSelf;
    }

    public void OpenSets()
    {
        SettingsPanel.SetActive(true);
    }

    public void CloseSets()
    {
        SettingsPanel.SetActive(false);
    }
}
