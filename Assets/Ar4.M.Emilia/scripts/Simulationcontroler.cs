using UnityEngine;

public class SimulationMarkerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float rotateSpeed = 60f;
    [SerializeField] private bool useLocalSpace = false;

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float rotateY = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateY = -1f;
        if (Input.GetKey(KeyCode.E)) rotateY = 1f;

        Vector3 move = new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.deltaTime;
        float rotation = rotateY * rotateSpeed * Time.deltaTime;

        if (useLocalSpace)
        {
            transform.Translate(move, Space.Self);
            transform.Rotate(0f, rotation, 0f, Space.Self);
        }
        else
        {
            transform.Translate(move, Space.World);
            transform.Rotate(0f, rotation, 0f, Space.World);
        }
    }
}