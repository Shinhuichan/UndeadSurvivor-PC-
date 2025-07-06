using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTimeI;
    public float gameTimeII;
    public float maxGameTimeI = 180f;
    public float maxGameTimeII = 60f;
    [Header("# Player Info")]
    public int playerId;
    public float health;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = {5, 10, 17, 26, 37, 50, 65, 82, 91, 91};
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    void Start() { 
        Screen.SetResolution(720, 720, fullscreen: false);
    }

    void Awake() { // 게임을 시작하면 Static 변수를 따라서 자기 자신을 집어넣는다.
        instance = this;
        // 게임 내 프레임을 120프레임으로 고정시키기 위한 코드
        Application.targetFrameRate = 120;
    }

    public void GameStart(int id) {
        playerId = id;
        health = Character.Health;
        // 아이템 ID값
        int[] v = new int[] {0, 1, 5, 6, 7};

        // 캐릭터를 선택해야 Player 오브젝트를 활성화
        player.gameObject.SetActive(true);
        uiLevelUp.Select((playerId < 2) ? ((playerId == 0) ? 0 : 1) : ((playerId == 4) ? 7 : ((playerId == 2) ? 5 : ((playerId != 5) ? (playerId == 6 ? 8 : 6) : v[Random.Range(0, v.Length)]))));
        if (playerId == 5) {
            uiLevelUp.Select(v[Random.Range(0, v.Length)]);
            uiLevelUp.Select(v[Random.Range(0, v.Length)]);
        }
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameOver() {
        StartCoroutine(GameOverRoutine());  
    }

    IEnumerator GameOverRoutine() {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }


    public void GameVictory() {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine() {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    public void GameRetry() {
        SceneManager.LoadScene(0);
    }

    public void GameQuit() {
        Application.Quit();
    }

    void Update() {
        if (!isLive)
            return;

        gameTimeI += Time.deltaTime;

        if (gameTimeI > maxGameTimeI) {
            gameTimeI = maxGameTimeI;
            GameVictory();
        }
    }

    public void GetExp() {
        if (!isLive)
            return;
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop() {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume() {
        isLive = true ;
        Time.timeScale = 1;
    }
}
