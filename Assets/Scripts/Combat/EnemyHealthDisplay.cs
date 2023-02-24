using Attributes;
using TMPro;
using UnityEngine;

namespace Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _player;
        private TextMeshProUGUI _textBox;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            _textBox = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            var target = _player.GetTarget();
            _textBox.text = target ? $"{target.GetPercentage():0.0}%" : "NA";
        }
    }
}