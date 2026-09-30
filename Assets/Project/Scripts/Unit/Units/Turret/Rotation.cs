using UnityEngine;

public static class Rotation
{
    public static void RotateTowardsTarget(GameObject centerRotate, Transform target, float speed)
    {
        Vector3 direction = target.position - centerRotate.transform.position;
        centerRotate.transform.right =
            Vector2.MoveTowards(centerRotate.transform.right, direction, Time.deltaTime * speed);
    }

    public static void RotateTowardsTarget(GameObject centerRotate, Vector3 target, float speed)
    {
        Vector3 direction = target - centerRotate.transform.position;
        centerRotate.transform.right =
            Vector2.MoveTowards(centerRotate.transform.right, direction, Time.deltaTime * speed);
    }
}