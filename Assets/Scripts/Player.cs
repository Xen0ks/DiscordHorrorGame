using UnityEngine;

public class Player : MonoBehaviour
{
    public MouseLook mL;
    public MovementBehaviour movementBehaviour;
    public InteractionBehaviour interactionBehaviour;
    public bool detectable = true;

    public int detectIndex = 1;

    public static Player instance;

    private void Awake()
    {
        instance = this;
    }
    public void GetDetectable()
    {
        detectable = true;
        detectIndex++;

    }

    public void GetUnDetectable()
    {
        detectIndex--;
        if(detectIndex <= 0)
        {
            detectIndex = 0;
            detectable = false;
        }
    }
}
