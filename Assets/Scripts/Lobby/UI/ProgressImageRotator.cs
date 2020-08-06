using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NBPChess.Lobby.UI
{
    public class ProgressImageRotator : MonoBehaviour
    {
        public Vector3 axisSpeed;

        void Update()
        {
            transform.Rotate(axisSpeed * Time.deltaTime);
        }
    }
}

