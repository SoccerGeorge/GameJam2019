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
        DrunkPlayer player = PlayerManager.Instance.CreatePlayerForLevel(0, transform.position, transform.rotation) as DrunkPlayer;
        player.SetTarget(target);
    }
}
