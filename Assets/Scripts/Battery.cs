using Unity.VisualScripting;
using UnityEngine;

public class Battery : MonoBehaviour
{
    Inspectable i;
    public Phone phone;

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
            phone.TriggerBattery(true, gameObject);

        }
        else if(!phone.powerSwitchTrigger)
        {
            phone.TriggerBattery(false, gameObject);

        }
    }
}
