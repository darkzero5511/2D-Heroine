using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillToolTip;

    public void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
    }
}
