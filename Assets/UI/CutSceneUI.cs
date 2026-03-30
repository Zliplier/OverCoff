using System;
using Manager;
using UnityEngine;
using UnityEngine.Video;
using Zlipacket.CoreZlipacket.Scene;

namespace UI.CutSceneUi
{
    public class CutSceneUI : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;

        private void Start()
        {
            Invoke(nameof(EndCutScene), (float)videoPlayer.length);
        }

        public void EndCutScene()
        {
            SceneController.Instance.LoadScene("TitleScene");
        }

        public void SkipCutScene()
        {
            CancelInvoke(nameof(EndCutScene));
            EndCutScene();
        }
    }
}