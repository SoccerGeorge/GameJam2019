using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

/// <summary>
/// Fuck Unity's collision detection
/// </summary>
public class CollisionDetection : MonoBehaviour
{
    private DrunkPlayer _player = null;

    private void Start () {
        _player = GetComponentInParent<DrunkPlayer>();
    }

    private void OnCollisionEnter (Collision collision) {
        _player.CheckCollisionEnter(collision);
    }
}
