using UnityEngine;

public class Coin : MonoBehaviour
{
    bool taken;

    void Update()
    {
        transform.Rotate(0f, 90f * Time.deltaTime, 0f, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (taken) return;
        if (!other.CompareTag("Player") && other.GetComponent<PlayerController>() == null)
            return;
        taken = true;
        var state = Object.FindObjectOfType<GameState>();
        if (state != null) state.AddCoin();
        Destroy(gameObject);
    }
}
