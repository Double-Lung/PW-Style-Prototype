using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    PLAYERTURNS,
    ENEMYTURNS,
}

public class GameStateHandler : MonoBehaviour {
    public GameObject entityPrefab;
    public Vector2[] playerPositions;
    public Vector2[] enemyPositions;
    public CharacterStats enemyStats;
    private List<Entity> playerEntity;
    private List<Entity> enemyEntity;
    private Entity activeEntity;
    private CharacterLoader characterLoader;
    private GameState state;
    private bool started;
    private bool paused;
    
    private void Awake() {
        characterLoader = GetComponent<CharacterLoader>();
        playerEntity = new List<Entity>();
        enemyEntity = new List<Entity>();
    }
    void Start() {
        Setup();
        started = false;
        UIManager.instance.ChangeNotification("Press \"F\" to pay respect");
        UIManager.instance.ToggleCanvas(true);
    }

    private void Update() {
        if (started) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (paused) {
                    Time.timeScale = 1;
                    UIManager.instance.togglePauseMenu(false);
                } else {
                    Time.timeScale = 0;
                    UIManager.instance.togglePauseMenu(true);
                }
            }
            return;
        }
        if (!started && Input.GetKeyDown(KeyCode.F)) {
            started = true;
            StartCoroutine(BattleStartRoutine());
        }
    }

    void Setup() {
        List<CharacterStats> characters = characterLoader.GetActiveCharacters();
        for (int i = 0; i < characters.Count; i++) {
            Entity player = Instantiate(entityPrefab, playerPositions[i], Quaternion.identity).GetComponent<Entity>();
            player.EntitySetup(characters[i]);
            playerEntity.Add(player);
        }

        for (int i = 0; i < 6; i++) {
            Entity enemy = Instantiate(entityPrefab, enemyPositions[i], Quaternion.identity).GetComponent<Entity>();
            enemy.EntitySetup(enemyStats);
            enemyEntity.Add(enemy);
        }
    }

    IEnumerator BattleStartRoutine() {
        UIManager.instance.ChangeNotification("Start");
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = PlayerPrefs.GetInt("Speed");
        UIManager.instance.ToggleCanvas(false);
        UIManager.instance.toggleSpeedControl(true);
        state = GameState.PLAYERTURNS;
        
        Attack(playerEntity, enemyEntity, () => { ChooseNextSide(); });
    }
    void ChooseNextSide() {
        if (enemyEntity.Count < 1) {
            EndGame(true);
            return;
        }
        if (playerEntity.Count < 1) {
            EndGame(false);
            return;
        }
        if (state == GameState.PLAYERTURNS) {
            state = GameState.ENEMYTURNS;
            Attack(enemyEntity, playerEntity, () => { ChooseNextSide(); });
        } else {
            state = GameState.PLAYERTURNS;
            Attack(playerEntity, enemyEntity, () => { ChooseNextSide(); });
        }
    }
    void Attack(List<Entity> attacker, List<Entity> targets, Action onTurnComplete) {
        int[] attackOrder = getAttackOrder(attacker.Count);
        StartCoroutine(AttackRoutine(attackOrder, attacker, targets, onTurnComplete));
    }
    IEnumerator AttackRoutine(int[] attackOrder, List<Entity> attacker, List<Entity> targets, Action onTurnComplete) {
        for (int i = 0; i < attacker.Count; i++) {
            activeEntity = attacker[attackOrder[i]];
            int targetIndex = UnityEngine.Random.Range(0, targets.Count);
            Entity target = targets[targetIndex];
            bool attackComplete = false;
            activeEntity.Attack(target, () => { attackComplete = true; });
            while (!attackComplete) {
                yield return null;
            }
            if (target.IsDead()) {
                targets.RemoveAt(targetIndex);
            }
            if (targets.Count < 1) {
                break;
            }
        }
        onTurnComplete();
    }
    void EndGame(bool state) {
        StartCoroutine(EndGameRoutine(state));
    }
    IEnumerator EndGameRoutine(bool state) {
        string notif1 = "";
        string notif2 = "";
        if (state) {
            notif1 = "WON";
            notif2 = "Reward x 1";
            int p = PlayerPrefs.GetInt("Progress");
            int l = PlayerPrefs.GetInt("level");
            if (p==l&&p<50) {
                PlayerPrefs.SetInt("Progress", p+1);
            }
        } else {
            notif1 = "LOST";
            notif2 = "No reward for u";
        }
        UIManager.instance.toggleSpeedControl(false);
        PlayerPrefs.SetInt("Speed", (int)Time.timeScale);
        Time.timeScale = 1;
        UIManager.instance.ChangeNotification(notif1);
        UIManager.instance.ToggleCanvas(true);
        yield return new WaitForSeconds(1f);
        UIManager.instance.ChangeNotification(notif2);
        yield return new WaitForSeconds(1f);
        UIManager.instance.ToggleCanvas(false);
        if (state) {
            UIManager.instance.toggleWonCanvas(true);
        } else {
            SceneManager.LoadScene(0);
        }
    }
    int[] getAttackOrder(int size) {
        int[] attackOrder = new int[size];
        for (int i = 0; i < size; i++) {
            attackOrder[i] = i;
        }
        Shuffle(attackOrder);
        return attackOrder;
    }
    void Shuffle(int[] array) {
        System.Random random = new System.Random();
        for (int i = array.Length - 1; i > -1; i--) {
            int j = random.Next(0, i);
            int temp = array[j];
            array[j] = array[i];
            array[i] = temp;
        }
    }
}