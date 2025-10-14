using UnityEngine;
using UnityEngine.UI;

public class HackInformationDisp : MonoBehaviour
{
    [SerializeField]
    CameraPositionController cameraPositionController;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text hpText;

    [SerializeField]
    Text nowHackText;

    [SerializeField]
    GameObject target;

    private void Update()
    {
        if (Player.Instance.stateType == Player.StateType.Hack)
        {

            if (cameraPositionController.targetObject != null)
            {
                nameText.gameObject.SetActive(true);
                hpText.gameObject.SetActive(true);
                nowHackText.gameObject.SetActive(true);

                target = cameraPositionController.targetObject;

                nameText.text = target.name;

                if (target.TryGetComponent<IDamageable>(out var damageable))
                {
                    hpText.text = damageable.nowHitPoint.ToString() + " / " + damageable.maxHitPoint.ToString();
                }
                else
                {
                    hpText.text = "UnBroken";
                }
                
                if (target.TryGetComponent<IHackObject>(out var hackObject))
                {
                    nowHackText.text = null;
                    foreach (var item in hackObject.nowHackEvent)
                    {
                        nowHackText.text += item.name;
                        nowHackText.text += "\n";
                    }
                }

            }
            else
            {
                nameText.gameObject.SetActive(false);
                hpText.gameObject.SetActive(false);
                nowHackText.gameObject.SetActive(false);
            }
        }
        else
        {
            nameText.gameObject.SetActive(false);
            hpText.gameObject.SetActive(false);
            nowHackText.gameObject.SetActive(false);
        }
    }
}
