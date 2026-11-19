
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotate;
        private IEnumerator Start()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            while (true)
            {
                rigidbody.rotation *= Quaternion.Euler(_rotate * Time.deltaTime);
                yield return null;
            }
        }
    }
}
