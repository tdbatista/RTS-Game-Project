using System;
using System.Threading;
using UnityEngine;

public class Ability 
{
    public string name;
    public int time;
    
    public Action action;
    private int count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Ability(string name, int time , Action action){
        this.name = name;
        this.time = time;
        this.action = action;
    }

    public void Start(){
        
    }

    public void Update(){

    }

    // Update is called once per frame
    
}
