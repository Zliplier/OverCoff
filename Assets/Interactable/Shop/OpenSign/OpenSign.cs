using Interactable.Object;
using Manager;
using Players;
using UnityEngine;
using Zlipacket.CoreZlipacket.Audio;
using Zlipacket.CoreZlipacket.Scene;

public class OpenSign : MonoBehaviour
{
    [SerializeField] private AudioClip signFX;
    public Shutter frontShutter;
    public Shutter shopShutter;
            
    public void Interact()
    {
        if (TimeManager.Instance.isRunning || (TimeManager.Instance.hour * 60) + TimeManager.Instance.minute - 480 >= TimeManager.Instance.endDayTime)
        {
            TimeManager.Instance.EndDay();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y - 180, gameObject.transform.rotation.eulerAngles.z);
            
            OvercoffManager.Instance.endMoney = Player.Instance.money;
            OvercoffManager.Instance.tax = GameplayManager.Instance.levelConfig.tax;
            
            SceneController.Instance.LoadScene("EndScene");
        }
        else
        {
            TimeManager.Instance.StartDay();
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y + 180, gameObject.transform.rotation.eulerAngles.z);
            
            frontShutter.Open();
            shopShutter.Open();
        }
        
        if (signFX != null)
            SoundFXManager.Instance.PlaySoundFX(signFX, transform, 1f);
    }
}
