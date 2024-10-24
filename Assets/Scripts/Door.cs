using UnityEngine;

public class Door : MonoBehaviour
{
    public bool right;        // Indique si la porte est � droite (true) ou � gauche (false)
    public bool isOpen = false;  // �tat d'ouverture de la porte (ferm�e par d�faut)

    public bool aiOpenable = false;
    private float openAngle = 90f;   // Angle d'ouverture de la porte (100 degr�s)
    private float rotationSpeed = 2f; // Vitesse de rotation de la porte

    private Quaternion targetRotation; // Rotation cible de la porte
    private Quaternion initialRotation; // Rotation initiale de la porte

    private void Start()
    {
        // Initialiser la rotation de base de la porte
        initialRotation = transform.rotation;
        targetRotation = initialRotation;  // La porte commence ferm�e
    }

    private void Update()
    {
        // Interpolation vers la rotation cible pour une transition fluide autour de l'axe Y global (Vector3.up)
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void Switch()
    {
        if (!isOpen)
        {
            GetComponent<Collider>().isTrigger = true;
            // Ouvre la porte autour de l'axe Y global (Vector3.up)
            if (right)
            {
                // Si la porte est � droite, elle tourne de +100 degr�s autour de Vector3.up
                targetRotation = Quaternion.AngleAxis(openAngle, Vector3.up) * initialRotation;
            }
            else
            {
                // Si la porte est � gauche, elle tourne de -100 degr�s autour de Vector3.up
                targetRotation = Quaternion.AngleAxis(-openAngle, Vector3.up) * initialRotation;
            }
            isOpen = true;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false;

            // Ferme la porte, revient � la rotation initiale
            targetRotation = initialRotation;
            isOpen = false;
        }
    }

    
}
