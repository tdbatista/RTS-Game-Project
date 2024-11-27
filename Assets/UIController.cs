using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Text unitName;
    private Text unitOwner;
    private Text unitRange;
    private Text unitLOS;
    private Button train;
    private static Color defaultColor = new Color(56f, 56f, 56f, 255f);
    PlayerController playerController;

    public static Color DefaultColor { get => defaultColor; set => defaultColor = value; }

    void Start()
    {
        unitName = transform.Find("UnitName").GetComponent<Text>();
        unitOwner = transform.Find("UnitOwner").GetComponent<Text>();
        unitRange = transform.Find("AttackRange").GetComponent<Text>();
        unitLOS = transform.Find("LOS").GetComponent<Text>();
        train = transform.Find("TrainUnit").GetComponent<Button>();
        playerController = PlayerController.Instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        //mostra dados da unidade
        
        GameObject lastUnit = null;
        if(playerController.selectedUnits.Count >0 ) {
            lastUnit  = playerController.selectedUnits.Last<GameObject>();
        }
        else {
            //pega a unidade que está sendo visivel, caso estiver (unidades que não são do player)
            lastUnit = playerController.viewUnit;
        }
        var showUI = false;
        var showButtons = false;
        if(lastUnit){
            if(lastUnit.GetComponent<Unit>().abilities.Count > 0){
                showButtons = true;
            }
            showUI = true;
            unitName.text = lastUnit.name;
            
            if(lastUnit.tag =="Player"){
                unitOwner.text = "Player";
                unitOwner.fontStyle = FontStyle.Normal;                
                unitOwner.color = defaultColor;
            }else{
                unitOwner.text = "Enemy";
                unitOwner.fontStyle = FontStyle.Bold;
                unitOwner.color = Color.red;                
            }
            unitRange.text = lastUnit.GetComponent<Unit>().atackRange.ToString("F0");
            unitLOS.text =   lastUnit.GetComponent<Unit>().LOS.ToString("F0");
        }
        unitName.gameObject.SetActive(showUI);
        unitOwner.gameObject.SetActive(showUI);
        unitRange.gameObject.SetActive(showUI);
        unitLOS.gameObject.SetActive(showUI);
        train.gameObject.SetActive(showButtons);
        
    }
}
