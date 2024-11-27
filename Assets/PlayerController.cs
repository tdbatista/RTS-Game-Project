using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public LayerMask clickableLayer;
    public LayerMask groundLayer;
    public List<GameObject> selectedUnits = new List<GameObject>();
    public List<GameObject> playerUnits = new List<GameObject>();
    public GameObject viewUnit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject selectionBox;
    private Vector2 selectionBoxStartPosition;
    private Vector2 selectionBoxEndPosition;
    void Awake(){
        //singleton - também serve para acessar diretamente a instancia pela classe apenas
        if(Instance!=this){
            if(Instance!=null){
                Destroy(Instance);
            }
            Instance = this;
        }
    }
    
    void Start()
    {
        
    }

    public void ViewUnit(Unit unit){
        viewUnit = unit.gameObject;
        unit.playerSelector.SetActive(true);
    }

    public void UnView(){
        if(viewUnit){
            viewUnit.GetComponent<Unit>().playerSelector.SetActive(false);
            viewUnit = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //detect mouse move
     //mouse click  -> select
        
        if(Input.GetMouseButtonDown(0)){
            if (EventSystem.current.IsPointerOverGameObject()){

                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            playerUnits.ForEach(unit => unit.GetComponent<Unit>().Deselect());
            UnView(); //desceleciona unidades caso estiverem selecionados.
            if(Physics.Raycast(ray,out hit, Mathf.Infinity, clickableLayer)){
                if (hit.transform.CompareTag("Player")){
                    
                    hit.transform.gameObject.GetComponent<Unit>().Select();
                    Debug.Log("Selected " + hit.transform.gameObject.name);
                } else {
                    //apenas visualiza a unidade
                    ViewUnit(hit.transform.gameObject.GetComponent<Unit>());
                }
            }
        } 

    // create rect select
        if(Input.GetMouseButton(0)){
              if (EventSystem.current.IsPointerOverGameObject()){

                return;
            }
            RectTransform rect = selectionBox.transform.Find("SelectionBox").GetComponent<RectTransform>();
            
            if(!selectionBox.activeInHierarchy){
                selectionBox.SetActive(true);
                selectionBoxStartPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }else{
                selectionBoxEndPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 boxSize = selectionBoxEndPosition- selectionBoxStartPosition; 
                               
                rect.anchoredPosition = selectionBoxStartPosition + new Vector2(boxSize.x,boxSize.y)/2;
                rect.sizeDelta = new Vector2(Mathf.Abs(boxSize.x),Mathf.Abs(boxSize.y));
            }
            
        } 
    //release rect select
        if(Input.GetMouseButtonUp(0)){
            
            selectionBox.SetActive(false);
            RectTransform rect = selectionBox.transform.Find("SelectionBox").GetComponent<RectTransform>();
            Rect selectionRect = new Rect(
                                            Mathf.Min(selectionBoxStartPosition.x,selectionBoxEndPosition.x),
                                            Mathf.Min(selectionBoxStartPosition.y,selectionBoxEndPosition.y) ,
                                            Mathf.Abs(selectionBoxEndPosition.x -selectionBoxStartPosition.x),
                                            Mathf.Abs(selectionBoxEndPosition.y -selectionBoxStartPosition.y)
                                        );
            
            Debug.Log(selectionRect.ToString());
            foreach(var unit in playerUnits){
                
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(unit.transform.position);
                Debug.Log("Unit: "+unit.name + " position in screen " + screenPosition.ToString());
                if (selectionRect.Contains(screenPosition) && unit.CompareTag("Player")){
                    unit.GetComponent<Unit>().Select();
                }
            }
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;
        } 
     //mouse click direito  -> move
        if(Input.GetMouseButtonDown(1)){
            if (EventSystem.current.IsPointerOverGameObject()){

                return;
            }
            
            foreach (var unit in selectedUnits)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                var agent = unit.transform.GetComponent<NavMeshAgent>();
                if(Physics.Raycast(ray,out hit, Mathf.Infinity, groundLayer)){
                    unit.GetComponent<Unit>().Move(hit.point);                    
                }
                
            }
        }

    }
}
