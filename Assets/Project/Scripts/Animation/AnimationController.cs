using UnityEngine;

public class AnimationController
{
    private FacingDirection _currentDirection = FacingDirection.None;

    protected void ControlMoveAnimation(FlipController spriteController, Animator animator, BaseMove baseMove, Transform parent)
    {
        bool isMoving =
                baseMove.Direction != FacingDirection.None ||
                baseMove.IsMovingUp ||
                baseMove.IsMovingDown;

        if (isMoving)
        {
            PlayMoveAnim(animator);
        }
        else
        {
            StopMoveAnim(animator);
        }

        if (baseMove.Direction != FacingDirection.None)
        {
            FlipSprite(FlipDirection.X, baseMove.Direction, spriteController, parent);
        }
    }

    protected void ControlDeadAnimation(Animator animator)
    {
        animator.SetTrigger("Dead");
    }

    private void PlayMoveAnim(Animator animator)
    {
        animator.SetFloat("Speed", 1);
    }

    private void StopMoveAnim(Animator animator)
    {
        animator.SetFloat("Speed", 0);
    }

    private void FlipSprite(FlipDirection flipDirection, FacingDirection direction, FlipController flipController, Transform parent)
    {
        if (direction == FacingDirection.Right &&
            (_currentDirection == FacingDirection.Left ||
            _currentDirection == FacingDirection.None))
        {
            flipController.FlipAllSpriteParent(flipDirection, direction, parent);
        }
        else if (direction == FacingDirection.Left &&
            (_currentDirection == FacingDirection.Right ||
            _currentDirection == FacingDirection.None))
        {
            flipController.FlipAllSpriteParent(flipDirection, direction, parent);
        }
    }
}
