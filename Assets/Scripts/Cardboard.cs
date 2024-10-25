using UnityEngine;

public class Cardboard : MonoBehaviour
{
    public bool hidden;
    Player player;
    Vector3 lastPos = Vector3.zero;

    Vector3 vel = Vector3.zero;
    public void Switch(Player p)
    {
        lastPos = p.transform.position;
        player = p;
        hidden = !hidden;
        p.mL.enabled = !hidden;
        p.movementBehaviour.enabled = !hidden;
        p.interactionBehaviour.enabled = !hidden;
        p.GetComponent<CharacterController>().enabled = !hidden;
        p.GetComponent<HeadBobController>().enabled = !hidden;
        p.detectable = !hidden;

        if (!hidden)
        {
            Player.instance.GetDetectable();
        }
        else
        {
            Player.instance.GetUnDetectable();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && hidden)
        {
            player.transform.position = GameManager.instance.replacePoint.position;

            Switch(GameObject.FindAnyObjectByType<Player>());

        }

        if (hidden)
        {
            player.transform.position = Vector3.SmoothDamp(player.transform.position, transform.position, ref vel, 0.2f);
        }

    }
}
