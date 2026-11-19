using UnityEngine;

namespace DefaultNamespace
{
    public class Gates : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                Destroy(other.gameObject);
                Debug.Log("Гол!");
            }
        }
    }
}