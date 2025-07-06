using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    // ���� �ִϸ����� ��Ʈ�ѷ��� ������ �迭 ���� ����
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        // ���ڰ� true�� ������ ��Ȱ��ȭ �� ������Ʈ�� �ȴ�.
        hands = GetComponentsInChildren<Hand>(true);
    }

    // �ִϸ����� ���� ���� �߰�
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
        // Get<T> : �����ȿ��� ������ ��Ʈ�� Ÿ�� T���� �������� �Լ�
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
        // �÷��̾ �׾������� �ȵ�.
        if (!GameManager.instance.isLive)
            return;
        // ���� �ð��� ���� ü�� ���ظ� ����.
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health <= 0) {
            // index�� 2�� ���� : �÷��̾ ������ Player ������Ʈ�� ����� Spawner�� �ʿ������
            //                      ��Ȱ��ȭ�ϱ� ���ؼ� index�� ������.
            // childCount : �ڽ� ������Ʈ�� ����
            for (int index = 2; index < transform.childCount; index++) {
                // GetChild : �־��� �ε����� �ڽ� ������Ʈ�� ��ȯ�ϴ� �Լ�
                transform.GetChild(index).gameObject.SetActive(false);
            }

            // �ִϸ����� SetTrigger �Լ��� ���� �ִϸ��̼��� ���� �����Ŵ.
            anim.SetTrigger("Dead");
            // ���ӿ��� �Լ��� �÷��̾� ��ũ��Ʈ�� ��� �κп��� ȣ���ϵ��� �ۼ�
            GameManager.instance.GameOver();
        }
    }
}
