using UnityEngine;
using UnityEngine.UI;

public class NotifPrefab : MonoBehaviour
{
    public Image image;
    public Text title;
    public Text content;

    public void Set(Sprite i, string t, string c)
    {
        image.sprite = i;
        title.text = t;
        content.text = c;
    }
}
