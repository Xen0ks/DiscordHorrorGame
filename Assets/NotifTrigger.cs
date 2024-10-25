using UnityEngine;

public class NotifTrigger : MonoBehaviour
{
    public Sprite image;
    public string title;
    public string content;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Phone.instance.Notif(image, title, content);
            Destroy(gameObject);
        }
    }
}
