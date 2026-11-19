using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Vector3 _start;
        [SerializeField] private Vector3 _end;
        [SerializeField] private float _speed;
        [SerializeField] private float _delay;
        private IEnumerator Start()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();

            while (true)
            {
                yield return Move(rigidbody, _start, _end);
                yield return new WaitForSeconds(_delay);
                yield return Move(rigidbody, _end, _start);
                yield return new WaitForSeconds(_delay);
            }
        }

        private IEnumerator Move(Rigidbody rigidbody, Vector3 start, Vector3 end)
        {
            while (Vector3.Distance(rigidbody.position, end) > 0.01f)
            {
                rigidbody.MovePosition(
                    Vector3.MoveTowards(
                        rigidbody.position,
                        end,
                        _speed * Time.fixedDeltaTime));
                yield return new WaitForFixedUpdate();
            }
        }
    }
}