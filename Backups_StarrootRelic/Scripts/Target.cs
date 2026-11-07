using UnityEngine;

public class Target : MonoBehaviour
{
    public Color hitColor = Color.red;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            GetComponent<Renderer>().material.color = hitColor;
        }
    }
}