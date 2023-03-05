using TMPro;
using UnityEngine;

namespace Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience _experience;
        private TextMeshProUGUI _textBox;

        private void Start()
        {
            _textBox = GetComponent<TextMeshProUGUI>();
        }

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            _textBox.text = $"{_experience.GetExperience()}";
        }
    }
}