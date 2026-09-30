using UnityEngine;
public class FlipController
{
    private readonly LayerMask _layerMask;

    private FacingDirection _currentDirection;
    private int currentTransformFlip;
    private int currentScaleXFlip;
    private int currentScaleYFlip;

    public FlipController(LayerMask layerMask)
    {
        _layerMask = layerMask;
        _currentDirection = FacingDirection.None;
    }

    public void FlipSprite(Transform transform, FlipDirection flipDirection, FacingDirection currentDirection)
    {
        if (_currentDirection == currentDirection) return;

        _currentDirection = currentDirection;

        if (flipDirection == FlipDirection.X)
        {
            if (_currentDirection == FacingDirection.Right && currentScaleXFlip > 0)
            {
                FlipScale(transform, flipDirection);
            }
            else if (_currentDirection == FacingDirection.Left && currentScaleXFlip < 0)
            {
                FlipScale(transform, flipDirection);
            }
        }
        else if (flipDirection == FlipDirection.Y)
        {
            FlipScale(transform, flipDirection);
        }

        _currentDirection = currentDirection;
    }

    public void FlipAllSpriteParent(FlipDirection flipDirection, FacingDirection derection, Transform parent)
    {
        currentScaleXFlip = parent.localScale.x > 0 ? 1 : -1;
        currentTransformFlip = parent.localPosition.x > 0 ? 1 : -1;

        Transform[] children = parent.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if ((_layerMask & (1 << children[i].gameObject.layer)) != 0)
            {
                if (children[i].gameObject.GetComponent<Unit>() != null)
                {
                    FlipUnit(flipDirection, derection, parent);
                }
            }
        }
    }

    private void FlipScale(Transform transform, FlipDirection flipDirection)
    {
        if (flipDirection == FlipDirection.X)
        {
            transform.localScale = new Vector3(
                transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else if (flipDirection == FlipDirection.Y)
        {
            transform.localScale = new Vector3(
                transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
        }

        currentScaleXFlip = transform.localScale.x > 0 ? 1 : -1;
        currentScaleYFlip = transform.localScale.y > 0 ? 1 : -1;
    }

    private void FlipPosition(Transform transform, FlipDirection flipDirection)
    {
        if (flipDirection == FlipDirection.X)
        {
            transform.localPosition = new Vector3(
                    transform.localPosition.x * -1, transform.localPosition.y, transform.localPosition.z);
        }
        else if (flipDirection == FlipDirection.Y)
        {
            transform.localPosition = new Vector3(
                    transform.localPosition.x, transform.localPosition.y * -1, transform.localPosition.z);
        }

        currentTransformFlip = transform.localPosition.x > 0 ? 1 : -1;
    }

    private void Flip(SpriteRenderer sprite, FlipDirection flipDirection, FacingDirection direction)
    {
        if (flipDirection == FlipDirection.X)
        {
            if (direction == FacingDirection.Right)
            {
                sprite.flipX = false;
            }
            else if (direction == FacingDirection.Left)
            {
                sprite.flipX = true;
            }
        }
    }

    private void FlipUnit(FlipDirection flipDirection, FacingDirection direction, Transform parent)
    {
        if (direction == FacingDirection.Right && currentScaleXFlip < 0)
        {
            FlipScale(parent, flipDirection);
        }
        else if (direction == FacingDirection.Left && currentScaleXFlip > 0)
        {
            FlipScale(parent, flipDirection);
        }
    }

    private void FlipWeapon(Transform transform, FlipDirection flipDirection, FacingDirection direction)
    {
        if (_currentDirection == direction) return;

        if (currentTransformFlip == 0)
        {
            currentTransformFlip = transform.localPosition.x > 0 ? 1 : -1;
        }

        if (currentScaleXFlip == 0)
        {
            currentScaleXFlip = transform.localScale.x > 0 ? 1 : -1;
        }

        if (direction == FacingDirection.Right && currentTransformFlip < 0 && currentScaleXFlip < 0)
        {
            FlipPosition(transform, flipDirection);
            FlipScale(transform, flipDirection);
        }
        else if (direction == FacingDirection.Left && currentTransformFlip > 0 && currentScaleXFlip > 0)
        {
            FlipPosition(transform, flipDirection);
            FlipScale(transform, flipDirection);
        }
    }
}
