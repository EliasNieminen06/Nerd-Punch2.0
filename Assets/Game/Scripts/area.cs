using UnityEngine;

public class area : MonoBehaviour
{
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<player>().Die();
        }
    }
}
