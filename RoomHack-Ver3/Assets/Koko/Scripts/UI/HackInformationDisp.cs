using UnityEngine;

public class HackInformationDisp : MonoBehaviour
{
    [SerializeField, Header("要アタッチ")]
    CameraPositionController cameraPositionController;

    [SerializeField, Header("要アタッチ")]
    GeneralUpdateText nameText;

    [SerializeField, Header("要アタッチ")]
    GeneralUpdateText hpText;

    [SerializeField, Header("要アタッチ")]
    GeneralUpdateText nowHackText;

    GameObject target;

    private void Update()
    {
        if (GameTimer.Instance.IsHackTime)
        {

            if (cameraPositionController.targetObject != null)
            {
                nameText.gameObject.SetActive(true);
                hpText.gameObject.SetActive(true);
                nowHackText.gameObject.SetActive(true);

                target = cameraPositionController.targetObject;


                if (target.TryGetComponent<IDamageable>(out var damageable))
                {
                    hpText.inputText = damageable.NowHitPoint.ToString() + " / " + damageable.MaxHitPoint.ToString();
                }
                else
                {
                    hpText.inputText = "UnBroken";
                }
                
                if (target.TryGetComponent<IHackObject>(out var hackObject))
                {
                    nameText.inputText = hackObject.HackObjectName;

                    nowHackText.inputText = null;
                    foreach (var item in hackObject.nowHackEvent)
                    {
                        nowHackText.inputText += item.thisToolTag.ToString();
                        nowHackText.inputText += "\n";
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
