using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
    public bool isLeft;
    public bool isBehind;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);
    Quaternion BehindRot = Quaternion.Euler(0, 0, 0);
    Quaternion BehindRotReverse = Quaternion.Euler(0, 0, 180);

    void Awake() {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate() {
        bool isReverse = player.flipX;
        if (isLeft) {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        } else if (isBehind) {
            transform.localRotation = isReverse ? BehindRotReverse : BehindRot;
        } else {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
