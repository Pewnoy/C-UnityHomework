using UnityEngine;

namespace DefaultNamespace
{
    public class BallShooter : MonoBehaviour
    {
        [SerializeField] private GameObject _ballPrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _force = 10f;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject ball = Instantiate(
                    _ballPrefab,
                    _shootPoint.position,
                    _shootPoint.rotation);

                Rigidbody rigidbody = ball.GetComponent<Rigidbody>();

                rigidbody.AddForce(_shootPoint.forward * _force, ForceMode.Impulse);
            }
        }
    }
}