using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    // 여러 애니메이터 컨트롤러를 저장할 배열 변수 선언
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        // 인자값 true를 넣으면 비활성화 된 오브젝트도 된다.
        hands = GetComponentsInChildren<Hand>(true);
    }

    // 애니메이터 변경 로직 추가
    void OnEnable() {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    // Update is called once per frame
    void Update() {
        if (!GameManager.instance.isLive)
            return;

        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        if(!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position+nextVec);
    }

    void OnMove(InputValue value) {
        // Get<T> : 프로픽에서 설정한 컨트롤 타입 T값을 가져오는 함수
        inputVec = value.Get<Vector2>();
    }
    void LateUpdate() {
        if(!GameManager.instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        // 플레이어가 죽어있으면 안됨.
        if (!GameManager.instance.isLive)
            return;
        // 진행 시간에 따라 체력 피해를 받음.
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health <= 0) {
            // index가 2인 이유 : 플레이어가 죽으면 Player 오브젝트의 무기와 Spawner는 필요없으니
            //                      비활성화하기 위해서 index를 지정함.
            // childCount : 자식 오브젝트의 개수
            for (int index = 2; index < transform.childCount; index++) {
                // GetChild : 주어진 인덱스의 자식 오브젝트를 반환하는 함수
                transform.GetChild(index).gameObject.SetActive(false);
            }

            // 애니메이터 SetTrigger 함수로 죽음 애니메이션을 직접 실행시킴.
            anim.SetTrigger("Dead");
            // 게임오버 함수를 플레이어 스크립트의 사망 부분에서 호출하도록 작성
            GameManager.instance.GameOver();
        }
    }
}
