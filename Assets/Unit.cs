using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    //Estados personalizados
    public enum State {   idle,  //parado
                          move,  // movimentando a mando do player
                          follow, // perseguindo alvo a mando do player
                          attack, // ataque automático, persegue o inimigo se estiver no campo de visão
                          combat //  está atacando 
                      }

    public List<Ability> abilities = new List<Ability>();
    private PlayerController playerController; //controlador da unidade
    public GameObject playerSelector; //indicador que a unidade está selecionada
    public float LOS = 5f;  //linha de visão
    public float atackRange = 1f; //range do ataque
    public LayerMask detectionLayer; //layer que as unidades  estão setadas 
    public GameObject target; //alvo da unidade
    private State state;  //estado da unidade
    private NavMeshAgent agent;    //agent de pathfind
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = PlayerController.Instance;
        playerController.playerUnits.Add(this.gameObject);
        agent = transform.GetComponent<NavMeshAgent>();
        state = State.idle;   //estado padrão
        if(agent)
            agent.stoppingDistance = 2; //para evitar das unidades ficarem se batendo.
    }

    // Update is called once per frame
    void Update()
    {
        //gerencia estados logica simples
        switch(state){
            case State.move:
                state = IsMoving(state);
            break;
            case State.combat:
                state = IsCombating(state);
            break;
            case State.attack:
                state = IsAttacking(state);
            break;
            default:
                state = IsIdle(state);
            break;
        }
        
    }
    private State IsCombating(State state){
        if(target==null){
            return State.idle;
        }
        else{
            //detecta as arestas mais próximas da unidade (serve para construções e unidades grandes)
            Vector3 closestPoint = target.GetComponent<Collider>().ClosestPoint(transform.position);
            //atualiza a distancia dele para o alvo
            float distanceToTarget = Vector3.Distance(closestPoint, gameObject.transform.position);
            if (distanceToTarget > atackRange){ //se estiver fora do atack range
                //volta para atacar (esse sistema de estado não grava estado anterior, logo não posso simplesmente
                //desabilitar o estado combate)
                return State.attack;
            }else {
                return State.combat;
            }
        }
    }
    private State IsAttacking(State state){
        //verifica se o objeto ainda está no range, e muda para combate se tiver dentro da area de combate
        if(target==null){
            return State.idle; //caso a unidade target não exista mais
        }else{
            //detecta as arestas mais próximas da unidade (serve para construções e unidades grandes)
            Vector3 closestPoint = target.GetComponent<Collider>().ClosestPoint(transform.position);
            //atualiza a distancia dele para o alvo
            float distanceToTarget = Vector3.Distance(closestPoint, gameObject.transform.position);
            if (distanceToTarget <= atackRange){ //se estiver dentro do attackrange
                if(agent)
                    agent.ResetPath(); //para de perseguir
                return State.combat;
            }else
            if (distanceToTarget > LOS){  //se estiver fora do campo de visão
                
                target = null; //retira o alvo
                return State.idle; //volta para state de ocioso
            }else{
                //se estiver dentro do campo de visão e ainda não está dentro do atack range,
                //mantém perseguindo
                //nao persegue se for fixo
                if(agent){
                    agent.SetDestination(closestPoint);
                    return State.attack;
                }else{
                    return State.idle;
                }
                
            }

        }        
    }
    private State IsIdle(State state){
        state = State.idle; //padrão
        //verifica se tem objetos próximos com a função que retorna colliders dentro da esfera
        Collider[] unitsColliders = Physics.OverlapSphere(transform.position, LOS, detectionLayer);
        foreach(var unitCollider in unitsColliders){
            //primeiramente verifica se o colidido naõ é o proprio objetos
            //depois compara a tag para ver se é inimigo
            if (unitCollider.gameObject != gameObject && gameObject.tag != unitCollider.gameObject.tag) 
            {
                if (!target) //se não há objeto como target
                {
                    target = unitCollider.gameObject; 
                }
                else //se tiver mais de um objeto, ele caça o mais próximo
                {
                    //detecta as arestas mais próximas da unidade (serve para construções e unidades grandes)
                    Vector3 closestPoint = target.GetComponent<Collider>().ClosestPoint(transform.position);
                    //atualiza a distancia dele para o alvo
                    float distanceToTarget = Vector3.Distance(closestPoint, transform.position);
                    //verifica o objeto mais perto
                    Vector3 closestPointCollider = unitCollider.ClosestPoint(transform.position);
                    //atualiza a distancia dele para o alvo
                    float distanceToCollider = Vector3.Distance(closestPointCollider, transform.position);

                    if (distanceToTarget > distanceToCollider)
                    {
                        //troca de target pelo objeto mais próximo
                        target = unitCollider.gameObject;
                    }
                }
                //se entrou nesse if, é porque ele está atacando alguém                
                state = State.attack;
            }  
        }                 
        return state;
    }
    private State IsMoving(State state){
        state = State.move;
        target = null;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Se o agente está parado ou sem caminho, altere a animação para "parado"
            if (agent.velocity.sqrMagnitude == 0f)
            {
                 // Troque o parâmetro conforme o seu Animator
                return State.idle;
            }
        }
        Debug.Log("Estado: "+ state.ToString());
        return state;
    }

    public void Move(Vector3 point){
        target = null;
        if(agent){ // se a unidade for fixa 
            agent.ResetPath();
            agent.SetDestination(point);  
            state = IsMoving(state);
        }else{
            state = IsIdle(state);
        }
        
      
    }
    public void Select(){
        Debug.Log(playerController);
        if(!playerController.selectedUnits.Contains(this.gameObject)){
            playerController.selectedUnits.Add(this.gameObject);
            playerSelector.SetActive(true);
        }
    }

    public void Deselect(){
        if(playerController.selectedUnits.Contains(this.gameObject)){
            playerController.selectedUnits.Remove(this.gameObject);
            playerSelector.SetActive(false);
        }
    }
}
