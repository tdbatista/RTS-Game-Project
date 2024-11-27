using System.Collections.Generic;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int count;
    public int trainingTime = 60;
    private bool training;
    public GameObject unitGeneric;
    
    void Start()
    {
        //para ser acessível no UI
        transform.GetComponent<Unit>().abilities.Add(new Ability("Train Unit",trainingTime,TrainUnit));
    }

    public void TrainUnit(){
        if(!training){
            count = 0;
            training = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(training){
            count++;
            if(count>trainingTime){
                Instantiate(unitGeneric,transform.position,Quaternion.identity);
                training = false;
                Debug.Log("Unidade criada!");
            }   
        }
    }
}
