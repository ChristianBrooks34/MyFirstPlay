using UnityEngine;


public abstract class BaseProjectileMovement : MonoBehaviour
{
    public Vector3 Direction { get; private set; }

    public abstract void MoveBall();

    public virtual void SetDirection(Vector3 direction)
    {
        if (direction == null) Debug.LogError("direction == null");

        Direction = direction;
    }
}

