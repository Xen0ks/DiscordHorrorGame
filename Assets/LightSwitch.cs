using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject light;

    public void Switch()
    {

        Vector3 currentRotation = transform.GetChild(0).transform.rotation.eulerAngles;  // Obtenir la rotation actuelle en angles d'Euler
        currentRotation.y *= -1;  // Multiplier la composante Y par -1
        transform.GetChild(0).transform.rotation = Quaternion.Euler(currentRotation);  // Appliquer la nouvelle rotation
        if (!GameManager.instance.switchOn) return;
        light.SetActive(!light.activeSelf);
    }
}
