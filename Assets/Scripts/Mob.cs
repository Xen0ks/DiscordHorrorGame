using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    // Configuration
    public Transform[] patrolTargets;   // Les points de patrouille
    public float detectionRadius = 2f;  // Rayon de détection du joueur
    public float chaseSpeed = 1.7f;     // Vitesse pendant la chasse
    public float patrolSpeed = 0.9f;    // Vitesse pendant la patrouille
    public float soundCooldown = 10f;   // Temps minimum entre deux répétitions de sons
    public LayerMask layerMask;

    // Composants
    private NavMeshAgent agent;         // Composant de navigation
    private Animator anim;              // Composant Animator pour les animations
    private Player player;              // Référence au joueur

    // État du mob
    public bool isChasing = false;     // Si le mob poursuit le joueur
    private Vector3 currentTarget;      // Cible actuelle de la patrouille
    private float lastSoundTime;        // Dernière fois où le son a été joué

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<Player>();

        StartCoroutine(PatrolRoutine());
    }

    private void Update()
    {
        DetectPlayer();
        if(Vector3.Distance(transform.position, currentTarget) < 0.5f)
        {
            NewDestination();
        }
    }

    // Routine de patrouille
    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (!isChasing) // Si le mob ne poursuit pas, il patrouille
            {
                NewDestination();
                yield return new WaitForSeconds(30f); // Temps entre chaque changement de cible
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void NewDestination()
    {
        int randomTargetIndex = Random.Range(0, patrolTargets.Length);

        if(randomTargetIndex == patrolTargets.Length - 1 && !GameManager.instance.hasKey) 
        {
            randomTargetIndex--;
        }

        currentTarget = patrolTargets[randomTargetIndex].position;
        agent.speed = patrolSpeed;
        agent.SetDestination(currentTarget);
    }

    // Détection du joueur
    private void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < detectionRadius)
        {
            Ray ray = new Ray(transform.position + Vector3.up * 1.5f, (player.transform.position - (transform.position + Vector3.up * 1.5f)).normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                if (hit.collider.CompareTag("Player") && player.detectable)
                {
                    Debug.Log("True");

                    ChasePlayer();
                }
                else if (isChasing) // Si le joueur est caché ou ne peut pas être détecté
                {
                    Debug.Log("False");

                    StopChasingAndReturnToPatrol();
                }

                Debug.Log(hit.transform.tag);
            }
        }
        else if (isChasing)
        {

            StopChasingAndReturnToPatrol();
        }
    }

    // Commencer la poursuite du joueur
    private void ChasePlayer()
    {
        Debug.Log("Chase");
        if (!isChasing)
        {
            isChasing = true;
            agent.speed = chaseSpeed;

            // Jouer les sons si le cooldown est terminé
            if (Time.time - lastSoundTime > soundCooldown)
            {
                SoundManager.instance.Repere();
                SoundManager.instance.music.Play();
                lastSoundTime = Time.time;
            }
        }

        // Mettre à jour la destination du mob vers le joueur
        agent.SetDestination(player.transform.position);
    }

    // Arrêter la poursuite et retourner à la patrouille
    private void StopChasingAndReturnToPatrol()
    {
        Debug.Log("Stop Chasing");
        isChasing = false;
        SoundManager.instance.music.Stop();
        SoundManager.instance.ambiance.Play();

        // Choisir un nouveau point de patrouille
        int randomTargetIndex = Random.Range(0, patrolTargets.Length);
        currentTarget = patrolTargets[randomTargetIndex].position;
        agent.SetDestination(currentTarget);
        agent.speed = patrolSpeed;
    }

    // Interaction avec les portes
    private void OnTriggerEnter(Collider other)
    {
        // Interaction avec les portes
        if (other.TryGetComponent<Door>(out Door door))
        {
            if (door.aiOpenable)
            {
                if (!door.isOpen)
                {
                    // Ouvrir la porte si elle est fermée
                    door.Switch();
                }
                else if (door.isOpen)
                {
                    // Refermer la porte si elle est ouverte
                    door.Switch();
                }
            }
        }

        // Interaction avec le joueur (déclenche le screamer)
        if (other.CompareTag("Player"))
        {
            Screamer();
        }
    }

    // Interaction lors de la sortie du trigger (pour les portes)
    private void OnTriggerExit(Collider other)
    {
        // Si le mob quitte une porte ouverte, il la referme
        if (other.TryGetComponent<Door>(out Door door))
        {
            if (door.aiOpenable && door.isOpen)
            {
                door.Switch();
            }
        }
    }

    // Lancer l'animation et le son du screamer
    private void Screamer()
    {
        anim.SetTrigger("Screamer");
        agent.SetDestination(transform.position); // Arrêter le mouvement pendant le screamer
        StartCoroutine(GameManager.instance.Screamer());
        SoundManager.instance.ScreamerSfx();
    }

    // Visualiser la sphère de détection dans l'éditeur
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
