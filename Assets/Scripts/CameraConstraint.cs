using UnityEngine;

public class CameraConstraint : MonoBehaviour
{
    private Camera m_Camera;
    private GameObject player;
    private Transform playerTransform;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        m_Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        m_Camera.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y +7f, player.transform.position.z);
    }
}
