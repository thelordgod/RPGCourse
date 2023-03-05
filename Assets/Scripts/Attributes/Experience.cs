using Saving;
using UnityEngine;

namespace Attributes
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float) state;
        }

        public float GetExperience()
        {
            return experiencePoints;
        }
    }
}