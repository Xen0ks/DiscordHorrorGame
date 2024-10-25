using UnityEngine;

public class Player : MonoBehaviour
{
    public MouseLook mL;
    public MovementBehaviour movementBehaviour;
    public InteractionBehaviour interactionBehaviour;
    [HideInInspector] public bool detectable = true;
}
