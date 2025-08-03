using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace FPS.Enemy
{
    public class SnakeBossHealth : MonoBehaviour
    {
        public SnakeBoss boss;
        private List<SnakeSegment> _segments = new List<SnakeSegment>();

        [SerializeField] private GameObject[] destroyedSnakeSegment;

        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }

        private int _nextDetachThreshold;
        private int _detachedCount = 0;

        private void Awake()
        {
            _segments = GetComponentsInChildren<SnakeSegment>().ToList();

            MaxHealth = _segments.Sum(s => s.MaxHealth);
            CurrentHealth = MaxHealth;

            _nextDetachThreshold = CurrentHealth - 200;

            foreach (var seg in _segments)
            {
                seg.OnTakeDamage += HandleSegmentDamage;
            }
        }

        public void TakeDamage(int damage)
        {
            if (_segments.Count == 0) return;

            SnakeSegment lastSegment = _segments[_segments.Count - 1];
            lastSegment.ApplyDamage(damage);

            HandleSegmentDamage(damage);
        }


        private void HandleSegmentDamage(int damage)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

            while (CurrentHealth <= _nextDetachThreshold && _segments.Count > 0)
            {
                DetachLastSegment();
                _nextDetachThreshold -= 200;
            }

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void DetachLastSegment()
        {
            var segment = _segments[_segments.Count - 1];
            _segments.RemoveAt(_segments.Count - 1);

            segment.gameObject.SetActive(false);

            if (_detachedCount < destroyedSnakeSegment.Length)
            {
                GameObject destroyedSeg = destroyedSnakeSegment[_detachedCount];
                _detachedCount++;

                destroyedSeg.transform.position = segment.transform.position;
                destroyedSeg.transform.rotation = segment.transform.rotation;
                destroyedSeg.SetActive(true);

                Rigidbody rb = destroyedSeg.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = destroyedSeg.AddComponent<Rigidbody>();
                }

                rb.useGravity = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                rb.AddForce(Random.insideUnitSphere * 5f, ForceMode.Impulse);

               // Destroy(destroyedSeg, 5f);
            }
        }

        public void Die()
        {
            Debug.Log("SnakeBoss zginął!");
            Destroy(gameObject);
        }
    }
}
