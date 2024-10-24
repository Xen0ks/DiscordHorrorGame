using UnityEngine;

public class Door : MonoBehaviour
{
    public bool right;        // Indique si la porte est à droite (true) ou à gauche (false)
    public bool isOpen = false;  // État d'ouverture de la porte (fermée par défaut)

    public bool aiOpenable = false;
    private float openAngle = 90f;   // Angle d'ouverture de la porte (100 degrés)
    private float rotationSpeed = 2f; // Vitesse de rotation de la porte

    private Quaternion targetRotation; // Rotation cible de la porte
    private Quaternion initialRotation; // Rotation initiale de la porte

    private void Start()
    {
        // Initialiser la rotation de base de la porte
        initialRotation = transform.rotation;
        targetRotation = initialRotation;  // La porte commence fermée
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
                // Si la porte est à droite, elle tourne de +100 degrés autour de Vector3.up
                targetRotation = Quaternion.AngleAxis(openAngle, Vector3.up) * initialRotation;
            }
            else
            {
                // Si la porte est à gauche, elle tourne de -100 degrés autour de Vector3.up
                targetRotation = Quaternion.AngleAxis(-openAngle, Vector3.up) * initialRotation;
            }
            isOpen = true;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false;

            // Ferme la porte, revient à la rotation initiale
            targetRotation = initialRotation;
            isOpen = false;
        }
    }

    
}
