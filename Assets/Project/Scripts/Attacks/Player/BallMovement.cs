using UnityEngine;

public class BallMovement
{
    public void Move(Transform currentPosition, int direction, float speed)
    {
        var offset = new Vector3(
            currentPosition.position.x + direction * speed * Time.deltaTime,
            currentPosition.position.y,
            currentPosition.position.z);

        currentPosition.position = offset;
    }
}
