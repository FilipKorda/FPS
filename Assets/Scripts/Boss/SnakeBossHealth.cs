using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FPS.Enemy
{
    public class SnakeBossHealth : MonoBehaviour
    {
        [SerializeField] private SnakeBoss boss;
        [SerializeField] private Slider bossHealthSlider;
        [SerializeField] private GameObject bossHeealthHolder;
        [SerializeField] private GameObject skullImage;
        [SerializeField] private GameObject[] leftLines;
        [SerializeField] private GameObject[] rightLines;

        private List<SnakeSegment> _segments = new List<SnakeSegment>();

        [SerializeField] private GameObject[] destroyedSnakeSegment;

        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }

        private int _nextDetachThreshold;
        private int _detachedCount = 0;

        private Coroutine _healthBarRoutine;

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

            DisableBossImages();
        }

        public void TakeDamage(int damage)
        {
            if (_segments.Count == 0) return;

            SnakeSegment lastSegment = _segments[_segments.Count - 1];
            lastSegment.ApplyDamage(damage);

            HandleSegmentDamage(damage);
        }


        public void ShowAndSetupBossHealthSlider()
        {
            bossHeealthHolder.SetActive(true);

            bossHealthSlider.maxValue = MaxHealth;
            bossHealthSlider.value = CurrentHealth;

            RectTransform rect = bossHeealthHolder.GetComponent<RectTransform>();
            rect.localScale = new Vector3(0f, rect.localScale.y, rect.localScale.z);

            StartCoroutine(ScaleBossHealthBar(rect));
        }

        private IEnumerator ScaleBossHealthBar(RectTransform rect)
        {
            float duration = 2f;
            float elapsed = 0f;

            Vector3 startScale = new Vector3(0f, rect.localScale.y, rect.localScale.z);
            Vector3 endScale = new Vector3(1f, rect.localScale.y, rect.localScale.z);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                rect.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            rect.localScale = endScale;

            EnabledBossImages();
        }

        private void EnabledBossImages()
        {
            if (skullImage != null)
            {
                skullImage.SetActive(true);
                StartCoroutine(BumpSkullImage(skullImage.transform));
            }

            foreach (var line in leftLines)
                if (line != null)
                    line.SetActive(true);

            foreach (var line in rightLines)
                if (line != null)
                    line.SetActive(true);
        }

        private void DisableBossImages()
        {
            if (skullImage != null)
                skullImage.SetActive(false);

            foreach (var line in leftLines)
                if (line != null)
                    line.SetActive(false);

            foreach (var line in rightLines)
                if (line != null)
                    line.SetActive(false);
        }

        private IEnumerator BumpSkullImage(Transform target)
        {
            float duration = 0.3f;
            float elapsed = 0f;

            Vector3 originalScale = target.localScale;
            Vector3 peakScale = originalScale * 1.5f;

            while (elapsed < duration / 50f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration / 2f);
                target.localScale = Vector3.Lerp(originalScale, peakScale, t);
                yield return null;
            }

            elapsed = 0f;

            while (elapsed < duration / 2f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (duration / 2f);
                target.localScale = Vector3.Lerp(peakScale, originalScale, t);
                yield return null;
            }

            target.localScale = originalScale;
        }

        private void HandleSegmentDamage(int damage)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

            if (_healthBarRoutine != null)
                StopCoroutine(_healthBarRoutine);

            _healthBarRoutine = StartCoroutine(SmoothHealthBarChange());

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

        private IEnumerator SmoothHealthBarChange()
        {
            float startValue = bossHealthSlider.value;
            float endValue = CurrentHealth;
            float elapsed = 0f;
            float duration = 0.3f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                bossHealthSlider.value = Mathf.Lerp(startValue, endValue, elapsed / duration);
                yield return null;
            }

            bossHealthSlider.value = endValue;
        }

        public void Die()
        {
            Debug.Log("SnakeBoss zginął!");
            Destroy(gameObject);
        }
    }
}
