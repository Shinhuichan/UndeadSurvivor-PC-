using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour {
    Collider2D coll;
    void Awake() {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision) { // ���ö��������� ������ �� �߻��ϴ� �Լ�
        if (!collision.CompareTag("Area")) // Area �±װ� �ƴϸ� return �Լ��� ������ �ǰ� ���α׷���  �������� �ʰ� Ż��(���͸�)
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        

        switch (transform.tag) { // tag�� Ground�� ���?
            case "Ground":
                float diffX = playerPos.x - myPos.x; // x��ǥ�� ������ ������ ã�� �Ÿ� ����
                float diffY = playerPos.y - myPos.y; // y��ǥ�� ������ ������ ã�� �Ÿ� ����
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY) {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY) {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":
                if (coll.enabled) {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
