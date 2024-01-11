using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] private GameObject PlayerOne;
    [SerializeField] private GameObject PlayerTwo;

    [SerializeField] private LayerMask playerLayers;

    private float minZoom = 5f;
    private float maxZoom = 10f;
    private float zoomSpeed = 5f;

    private float minX = -10f;
    private float maxX = 10f;
    private float minY = -5f;
    private float maxY = 5f;

    private void Update()
    {
        Vector3 middlePos = new Vector3((PlayerOne.transform.position.x + PlayerTwo.transform.position.x) / 2, (PlayerOne.transform.position.y + PlayerTwo.transform.position.y) / 2, -10);
        middlePos.x = Mathf.Clamp(middlePos.x, minX, maxX);
        middlePos.y = Mathf.Clamp(middlePos.y, minY, maxY);
        this.gameObject.transform.position = middlePos;
        float distance = Vector2.Distance(PlayerOne.transform.position, PlayerTwo.transform.position);
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, distance / maxZoom);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
        
    }
}
