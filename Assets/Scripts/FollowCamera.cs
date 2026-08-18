using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 6f, -10f);
    public float follow = 8f;

    void LateUpdate()
    {
        if (target == null) return;
        var desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, follow * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}
