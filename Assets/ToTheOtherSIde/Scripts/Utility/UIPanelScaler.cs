using UnityEngine;
using DG.Tweening;

public class UIPanelScaler : MonoBehaviour
{
    public bool inAnim = false;
    
    private async void OnEnable()
    {
        inAnim = true;
        //G.AudioManager.PlaySound(R.Audio.PanelIn, 0);
        Transform panel = GetComponent<RectTransform>().GetChild(0);
        panel.localScale = Vector3.zero;
        await panel.DOScale(Vector3.one, 0.8f)
            .SetEase(Ease.OutElastic, 1.1f, 0.5f)
            .AsyncWaitForCompletion();
        
        inAnim = false;
    }

    public void Close()
    {
        CloseAnim();
    }
    public async void CloseAnim()
    { 
        inAnim = true;
        Sequence mySequence = DOTween.Sequence();
        Transform panel = GetComponent<RectTransform>().GetChild(0);
        await panel.DOScale(Vector3.one * 0f, 0.5f)
            .SetEase(Ease.InBack, 0.7f)
            .AsyncWaitForCompletion();
        inAnim = false;
        gameObject.SetActive(false);
    }
    public void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }
}