using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour {
    Collider2D coll;
    void Awake() {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision) { // 인플라이저에서 나갔을 때 발생하는 함수
        if (!collision.CompareTag("Area")) // Area 태그가 아니면 return 함수를 만나게 되고 프로그램을  실행하지 않고 탈출(필터링)
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        

        switch (transform.tag) { // tag가 Ground일 경우?
            case "Ground":
                float diffX = playerPos.x - myPos.x; // x좌표의 차이의 절댓값을 찾아 거리 측정
                float diffY = playerPos.y - myPos.y; // y좌표의 차이의 절댓값을 찾아 거리 측정
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
