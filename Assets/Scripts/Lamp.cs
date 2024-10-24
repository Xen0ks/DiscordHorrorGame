using UnityEngine;

public class Lamp : MonoBehaviour
{
    public Light light;

    public void Switch()
    {
        light.gameObject.SetActive(!light.gameObject.activeSelf);
    }
}
