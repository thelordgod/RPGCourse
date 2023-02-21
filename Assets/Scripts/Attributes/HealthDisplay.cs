using System;
using TMPro;
using UnityEngine;

namespace Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;
        private TextMeshProUGUI _textBox;

        private void Start()
        {
            _textBox = GetComponent<TextMeshProUGUI>();
        }

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _textBox.text = $"{_health.GetPercentage():0.0}%";
        }
    }
}