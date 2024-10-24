using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    public Transform[] Targets;
    NavMeshAgent agent;
    Vector3 target;
    bool chasing = false;
    Animator anim;
    public Player player;

    public float detectionRadius = 2f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(NewTarget());
        anim = GetComponent<Animator>();
        player = GameObject.FindAnyObjectByType<Player>();
    }

    void Update()
    {
        Vector3 center = transform.position;
        center.y += 2f;

        if (Vector3.Distance(agent.transform.position, target) < 1f)
        {
            target = Targets[Random.Range(0, Targets.Length)].position;
            agent.SetDestination(target);
        }

        // Calcul de la distance entre le mob et le joueur
        float distanceToPlayer = Vector3.Distance(center, player.transform.position);
        Debug.Log(distanceToPlayer);

        if (distanceToPlayer < detectionRadius)
        {
            // Raycast pour vérifier la ligne de vue
            Ray ray = new Ray(transform.position + Vector3.up * 1.5f, (player.transform.position - (transform.position + Vector3.up * 1.5f)).normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.CompareTag("Player"))  // Le raycast a touché le joueur, donc il est visible
                {
                    agent.speed = 1.7f;
                    chasing = true;
                    target = player.transform.position;
                    agent.SetDestination(target);

                }
            }
        }
        else
        {
            agent.speed = 0.9f;
            chasing = false;
        }
    }


    public IEnumerator NewTarget()
    {
        while (true)
        {
            if (!chasing)
            {
                target = Targets[Random.Range(0, Targets.Length)].position;
                agent.SetDestination(target);
            }
            yield return new WaitForSeconds(50f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Door>(out Door d) && d.aiOpenable && !d.isOpen)
        {
            d.Switch();
        }
        if (other.CompareTag("Player"))
        {
            Screamer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.TryGetComponent<Door>(out Door d) && d.aiOpenable && d.isOpen))
        {
            d.Switch();
        }
    }

    public void Screamer()
    {
        anim.SetTrigger("Screamer");
        agent.SetDestination(transform.position);
        StartCoroutine(GameManager.instance.Screamer());
    }

    private void OnDrawGizmos()
    {
        // Dessiner la sphère de détection autour du mob
        Vector3 center = transform.position;
        center.y += 2f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, detectionRadius);

        // Dessiner le raycast vers le joueur pour visualiser la ligne de vue
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + Vector3.up * 1.5f, player.transform.position);
        }
    }

}
