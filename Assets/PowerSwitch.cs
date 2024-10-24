using UnityEngine;

public class PowerSwitch : MonoBehaviour
{

    Inspectable i;
    Phone phone;

    void Start()
    {
        i = GetComponent<Inspectable>();

    }

    private void Update()
    {
        phone = GameObject.FindAnyObjectByType<Phone>();
        if (phone == null) return;
        if (i.inspected)
        {
            phone.TriggerSwitch(true);
        }
        else if(!phone.batteryTrigger)
        {
            phone.TriggerSwitch(false);
        }
    }
}
