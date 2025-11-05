using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    private void Awake()
    {
        instance = this;
    }

    public int startingMana = 4, maxMana = 12;
    public int playerMana;
    private int currentPlayerMaxMana;

    public int startingCardsAmount = 5;
    public int cardsToDrawPerTurn = 2;

    public enum TurnOrder { playerActive, playerCardAttacks, enemyActive, enemyCardAttacks }
    public TurnOrder currentPhase;

    public Transform discardPoint;

    public int playerHealth, enemyHealth;

    private readonly Queue<int> playerPopupQueue = new Queue<int>();
    private readonly Queue<int> enemyPopupQueue = new Queue<int>();
    private bool playerPopupRunning, enemyPopupRunning;

    [SerializeField] private float damagePopupDelay = 0.5f;
    private WaitForSeconds popupGap;


    private void Start()
    {
        popupGap = new WaitForSeconds(damagePopupDelay);
        //playerMana = startingMana;
        //UIController.instance.SetPlayerManaText(playerMana);
        currentPlayerMaxMana = startingMana;
        FillPlayerMana();

        DeckController.instance.DrawMultipleCards(startingCardsAmount);

        UIController.instance.SetPlayerHealthText(playerHealth);
        UIController.instance.SetEnemyHealthText(enemyHealth);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            AdvanceTurn();
        }
    }

    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana = playerMana - amountToSpend;

        if (playerMana < 0)
        {
            playerMana = 0;
        }

        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void FillPlayerMana ()
    {
        //playerMana = startingMana;
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void AdvanceTurn ()
    {
        currentPhase++;

        if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
        { 
            currentPhase = 0; 
        }

        switch (currentPhase)
        {
            case TurnOrder.playerActive:

                UIController.instance.endTurnButton.SetActive(true);
                UIController.instance.drawCardButton.SetActive(true);

                if(currentPlayerMaxMana < maxMana)
                {
                    currentPlayerMaxMana++;
                }

                FillPlayerMana();

                DeckController.instance.DrawMultipleCards(cardsToDrawPerTurn);

                break;

            case TurnOrder.playerCardAttacks:

                //Debug.Log("skipping player card attacks");
                //AdvanceTurn();

                CardPointController.instance.PlayerAttack();

                break;

            case TurnOrder.enemyActive:

                Debug.Log("skipping enemy actions");
                AdvanceTurn();

                break;

            case TurnOrder.enemyCardAttacks:

                //Debug.Log("skipping enemy card attacks");
                //AdvanceTurn();

                CardPointController.instance.EnemyAttack();

                break;

        }
    }

    public void EndPlayerTurn ()
    {
        UIController.instance.endTurnButton.SetActive(false);
        UIController.instance.drawCardButton.SetActive(false);

        AdvanceTurn ();
    }

    public void DamagePlayer(int damageAmount)
    {
        if(playerHealth > 0)
        {
            playerHealth -= damageAmount;

            if(playerHealth <= 0)
            {
                playerHealth = 0;

                //End battle
            }

            UIController.instance.SetPlayerHealthText(playerHealth);

            playerPopupQueue.Enqueue(damageAmount);
            if (!playerPopupRunning) StartCoroutine(PlayerPopupWorker());
        }
    }

    public void DamageEnemy(int damageAmount)
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= damageAmount;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;

                //End battle
            }

            UIController.instance.SetEnemyHealthText(enemyHealth);

            enemyPopupQueue.Enqueue(damageAmount);
            if (!enemyPopupRunning) StartCoroutine(EnemyPopupWorker());
        }
    }

    private IEnumerator PlayerPopupWorker()
    {
        playerPopupRunning = true;
        while (playerPopupQueue.Count > 0)
        {
            int amount = playerPopupQueue.Dequeue();
            SpawnPlayerPopup(amount);
            yield return popupGap;
        }
        playerPopupRunning = false;
    }

    private IEnumerator EnemyPopupWorker()
    {
        enemyPopupRunning = true;
        while (enemyPopupQueue.Count > 0)
        {
            int amount = enemyPopupQueue.Dequeue();
            SpawnEnemyPopup(amount);
            yield return popupGap;
        }
        enemyPopupRunning = false;
    }

    private void SpawnPlayerPopup(int amount)
    {
        var damageClone = Instantiate(UIController.instance.playerDamage, UIController.instance.playerDamage.transform.parent);
        damageClone.damageText.text = amount.ToString();
        damageClone.gameObject.SetActive(true);
    }

    private void SpawnEnemyPopup(int amount)
    {
        var damageClone = Instantiate(UIController.instance.enemyDamage, UIController.instance.enemyDamage.transform.parent);
        damageClone.damageText.text = amount.ToString();
        damageClone.gameObject.SetActive(true);
    }

}
