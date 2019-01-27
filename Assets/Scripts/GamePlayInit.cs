using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class GamePlayInit : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    private void Awake () {
        UIManager.Instance.SwitchToMenuByIndex(0);
        CanvasGroup canvasGroup = UIManager.Instance.GetActiveMenu();
        StartCoroutine(FadeOutMenu(canvasGroup.transform.GetChild(1).GetComponent<CanvasGroup>()));

        DrunkPlayer player = PlayerManager.Instance.CreatePlayerForLevel(0, transform.position, transform.rotation) as DrunkPlayer;
        player.SetTarget(target);
    }

    private IEnumerator FadeOutMenu (CanvasGroup canvasGroup) {
        canvasGroup.alpha = 1f;
        canvasGroup.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);

        while (canvasGroup.alpha > 0f) {
            canvasGroup.alpha -= 0.2f;
            yield return null;
        }

        canvasGroup.gameObject.SetActive(false);
    }
}
