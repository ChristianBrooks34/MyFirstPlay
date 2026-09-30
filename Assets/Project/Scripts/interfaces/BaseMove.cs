using UnityEngine;

public abstract class BaseMove : MonoBehaviour
{
    public Vector2 Offset { get; set; }
    public FacingDirection Direction { get; set; }
    public bool IsMovingUp { get; private set; }
    public bool IsMovingDown { get; private set; }

    public abstract void Move();

    public void ChargeDirection(Vector2 input)
    {
        if (input.x > 0)
        {
            Direction = FacingDirection.Right;
        }
        else if (input.x < 0)
        {
            Direction = FacingDirection.Left;
        }
        else
        {
            Direction = FacingDirection.None;
        }

        if (input.y > 0)
        {
            IsMovingUp = true;
            IsMovingDown = false;
        }
        else if (input.y < 0)
        {
            IsMovingUp = false;
            IsMovingDown = true;
        }
        else
        {
            IsMovingUp = false;
            IsMovingDown = false;
        }
    }
}
