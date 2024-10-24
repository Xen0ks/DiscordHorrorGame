using UnityEngine;

public class Inspectable : MonoBehaviour
{
    public bool inspected = false;
    Vector3 originalPos = Vector3.zero;
    Quaternion originalRot = Quaternion.identity;
    Quaternion targetRot = Quaternion.identity;

    public Vector3 rotationOffset;  // Offset de rotation ajouté

    Vector3 target;
    Vector3 vel;

    private void Start()
    {
        Invoke("SavePos", 2f);
    }

    void SavePos()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
        target = originalPos;
        targetRot = originalRot;
    }

    private void Update()
    {
        if (originalPos == Vector3.zero) return;

        if(inspected && Vector3.Distance(transform.position, Camera.main.transform.position) > 3f)
        {
            Switch(Vector3.zero);
        }

        if(inspected && Vector3.Distance(transform.position, Camera.main.transform.position) < 0.5f)
        {
            Switch(Vector3.zero);
        }

        // Mouvement lissé vers la cible
        transform.position = Vector3.SmoothDamp(transform.position, target, ref vel, 0.3f);
        // Interpolation de la rotation vers la rotation cible
        transform.rotation = targetRot;
    }

    public void Switch(Vector3 newPos)
    {
        if (inspected)
        {
            // Revenir à la position et rotation initiale
            target = originalPos;
            targetRot = originalRot;
            inspected = false;
            Invoke("EnableCollider", 0.4f);


        }
        else
        {
            // Passer en mode inspecté (vers la nouvelle position)
            target = newPos;
            inspected = true;
            GetComponent<Collider>().isTrigger = true;
            // Regarde la Main Camera
            Vector3 directionToCamera = Camera.main.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);

            // Ajouter l'offset de rotation spécifié
            targetRot = lookRotation * Quaternion.Euler(rotationOffset);
        }
    }

    void EnableCollider()
    {
        GetComponent<Collider>().isTrigger = false;

    }
}
