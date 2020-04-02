using UnityEngine;

namespace Space
{
    public class World : MonoBehaviour
    {
        public float gameSpeed = 1f;

        private void Update()
        {
            Time.timeScale = gameSpeed;
        }

    }
}
