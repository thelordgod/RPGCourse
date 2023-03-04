using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace SceneManagement
{
    // fix for bugged lighting on scene load
    public class UpdateSky : MonoBehaviour
    {
        private ReflectionProbe _baker;

        private void Start()
        {
            _baker = gameObject.AddComponent<ReflectionProbe>();
            _baker.cullingMask = 0;
            _baker.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            _baker.mode = ReflectionProbeMode.Realtime;
            _baker.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
 
            RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
            StartCoroutine(UpdateEnvironment());
        }

        private IEnumerator UpdateEnvironment()
        {
            DynamicGI.UpdateEnvironment();
            _baker.RenderProbe();
            yield return new WaitForEndOfFrame();
            RenderSettings.customReflection = _baker.texture;
        }
    }
}