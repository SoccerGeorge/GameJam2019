using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class GamePlayInit : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake () {
        PlayerManager.Instance.CreatePlayerForLevel(0, transform.position, transform.rotation);
        UIManager.Instance.SwitchToMenuByIndex(0);
    }
}
