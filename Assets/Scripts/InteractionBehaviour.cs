using UnityEngine;

public class InteractionBehaviour : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactMask;

    public Transform holder;
    public Transform inspectPos;
    public Phone phone;

    private void Start()
    {
        
    }


    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(holder.position, holder.forward, out hit, interactRange, interactMask) && Input.GetKeyDown(KeyCode.Mouse0) && !phone.inspected)
        {
            SoundManager.instance.InteractSfx();

            if (hit.transform.TryGetComponent<Door>(out Door d))
            {
                d.Switch();
            }
            if (hit.transform.TryGetComponent<Inspectable>(out Inspectable i))
            {
                i.Switch(inspectPos.position);
            }

            if(hit.transform.TryGetComponent<Phone_Item>(out Phone_Item p))
            {
                Destroy(p.gameObject);
                phone.gameObject.SetActive(true);
                phone.ToggleFlash();
                StartCoroutine(phone.BatteryDeload());
                StartCoroutine(phone.BatteryReload());

            }

            if (hit.transform.TryGetComponent<LightSwitch>(out LightSwitch s))
            {
                s.Switch();
            }

            if (hit.transform.TryGetComponent<Cardboard>(out Cardboard c) && !c.hidden)
            {
                c.Switch(GetComponent<Player>());
            }

            if (hit.transform.TryGetComponent<Key>(out Key k))
            {
                GameManager.instance.hasKey = true;
                Destroy(k.gameObject);
            }

            if (hit.transform.TryGetComponent<Uninstaller>(out Uninstaller u))
            {
                Debug.Log("sqdmlfkj");
                StartCoroutine(GameManager.instance.End());
            }

        }
    }
}
