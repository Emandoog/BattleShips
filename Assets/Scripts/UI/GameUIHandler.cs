using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIHandler : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI buildingLog;
    [SerializeField] private GameObject buildTipsText;
    [SerializeField] private GameObject combatTipsText;
    [SerializeField] private Button showPlayerBoardButton;
    [SerializeField] private Button showEnemyBoardButton;
    [SerializeField] private GameObject buttonPlayer;
    [SerializeField] private GameObject buttonEnemy;
    [SerializeField] private GameObject turnDisplay;
    [SerializeField] private TextMeshProUGUI turnDisplayText;
    [SerializeField] private TextMeshProUGUI gameEndText;
    [SerializeField] private GameObject gameEndScreen;
	

	private bool playerTurn = true;

	private void Start()
	{
        showPlayerBoardButton.onClick.AddListener(ChangeTableToPlayer);
		showEnemyBoardButton.onClick.AddListener(ChangeTableToEnemy);
	}

	public void SetBuildingLog(string textToSet)
    { 
    buildingLog.text = textToSet;
    
    }
    public void EndBuildPhaseUI() 
    {
		buildTipsText.SetActive(false);

        combatTipsText.SetActive(true);
		buttonPlayer.SetActive(true);
		buttonEnemy.SetActive(true);
        turnDisplay.SetActive(true);

	}
	public void OnGameEnd(string endText)
	{
		gameEndScreen.SetActive(true);
        gameEndText.text = endText;
	}
	public void ChangeTableToPlayer()
    {

        CombatHandler.instance.GetComponent<CombatHandler>().SetTableToPlayer();
    
    }
    public void ChangeTableToEnemy()
    {

		CombatHandler.instance.GetComponent<CombatHandler>().SetTableToEnemy();

	}
    public void ChangeTurn(string turn) 
    {


        turnDisplayText.text = turn;

}
}
