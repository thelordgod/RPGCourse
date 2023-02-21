using System;
using TMPro;
using UnityEngine;

namespace Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textBox;
        
        private Health _health;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            textBox.text = $"{_health.GetPercentage()}%";
        }
    }
}