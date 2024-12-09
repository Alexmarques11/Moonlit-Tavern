# Moonlit Tavern
Este projeto consiste em um jogo 2D de Tower Defense em visão top-down, desenvolvido na Unity. O jogador assume o papel de um anão, o personagem principal, que pode explorar um pequeno mapa, construir estruturas estratégicas e aprimorar suas armas e habilidades. Essas construções desempenham um papel crucial ao auxiliar o jogador na defesa de sua taverna contra hordas de inimigos. A combinação de estratégia e ação garante uma experiência envolvente e desafiadora.

## Grupo

- a25968 [Alexandre Marques](https://github.com/Alexmarques11)
- a25977 [Miguel Sousa](https://github.com/MiguelVS2004)
- a25959 [Rui Costa](https://github.com/Rui2117)

## Controlos
### Modo Construção desativado
- ***Movimentação:*** W/A/S/D ou setas do teclado
- ***Atacar:*** Mouse 1 (na direção do cursor do mouse)
- ***Ativar Modo Construção:*** C
- ***Iniciar a Noite:*** Enter
- ***Abrir Inventário:*** I
- ***Interagir com Edifícios:*** F
- ***Trocar de Arma:*** Scroll do Mouse
- ***Usar Poção de Cura:*** Q
- ***Usar Poção Ofensiva:*** E
- ***Menu de Pausa:*** Esc
### Modo Construção ativo
- ***Colocar Construção:*** Mouse 1 (na posição desejada)
- ***Alterar Construção Selecionada:*** Scroll do Mouse

## Técnicas de AI utilizadas
Para complementar e enriquecer a experiência e o desenvolvimento do jogo, implementamos várias técnicas de Inteligência Artificial (IA), permitindo comportamentos dinâmicos e desafiadores para os inimigos e sistemas do jogo. As principais técnicas utilizadas são State Machine (Máquina de Estados), Behaviour Tree (Árvore de Comportamentos) e Pathfinding A*.

## State Machine (Máquina de Estados):
Uma State Machine (Máquina de Estados) é um modelo computacional usado para descrever e controlar o comportamento de um sistema em diferentes estados. Ela é composta por estados, que representam condições ou situações específicas do sistema, como por exemplo, em um jogo, os estados de um inimigo podem ser "Parar", "Correr" ou "Atacar". Além disso, possui transições, que são conexões entre estados e definem como e quando o sistema deve mudar de um estado para outro, geralmente desencadeadas por eventos ou condições, como a proximidade do jogador. A máquina começa em um estado inicial e, a partir disso, executa ações relacionadas a esse estado. Quando uma condição de transição é satisfeita, ela muda para outro estado e começa a executar as ações associadas ao novo estado.

### Implementação de State Machine no Nosso Jogo
No nosso jogo, existem diversos tipos de inimigos, cada um equipado com diferentes inteligências artificiais para controlar os seus movimentos e ataques de acordo com o ambiente ao seu redor. Para otimizar o comportamento desses inimigos, implementámos uma State Machine, permitindo uma gestão mais eficiente das funções a serem executadas com base no estado atual de cada inimigo. Essa abordagem garante transições fluidas entre estados e melhora a reatividade e a lógica de tomada de decisão dos inimigos, tornando o jogo mais dinâmico e desafiador.

A implementação da State Machine está no script "EnemyPlayerAI", que é responsável por controlar o movimento dos inimigos. Esses inimigos priorizam os seus alvos na seguinte ordem: jogador > edifícios > taverna, garantindo que suas ações sejam orientadas pela lógica de maior ameaça ou objetivo estratégico.

```cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerAI : MonoBehaviour
{
    public Rigidbody2D theRigidbody;
    public float moveSpeed;
    public SpriteRenderer enemyRenderer;
    public float playerDetectionRadius = 7f;
    public float buildingDetectionRadius = 5f;

    private Transform target;
    private Vector3 targetPositionOffset;
    private bool isFlipped = false;
    private Damage damageComponent;

    public Animator animator;

    private enum State
    {
        MoveToTavern,
        MoveToBuilding,
        MoveToPlayer,
        Idle
    }

    private State currentState;

    void Start()
    {
        damageComponent = GetComponent<Damage>();
        ChangeState(State.Idle);
    }

    void Update()
    {
        if (damageComponent != null && damageComponent.isAttacking)
        {
            theRigidbody.velocity = Vector2.zero;
            return;
        }

        UpdateState();
        ExecuteState();
        MoveToTarget(target);
    }

    private void UpdateState()
    {
        if (DetectPlayer())
        {
            ChangeState(State.MoveToPlayer);
            return;
        }

        if (DetectBuildings())
        {
            ChangeState(State.MoveToBuilding);
            return;
        }

        FindTavern();
        if (target == null)
        {
            ChangeState(State.Idle);
        }
        else
        {
            ChangeState(State.MoveToTavern);
        }
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case State.MoveToTavern:
                animator.SetBool("isMoving", true);
                MoveToTarget(target);
                break;
            case State.MoveToBuilding:
                animator.SetBool("isMoving", true);
                MoveToTarget(target);
                break;
            case State.MoveToPlayer:
                animator.SetBool("isMoving", true);
                MoveToTarget(target);
                break;
            case State.Idle:
                animator.SetBool("isMoving", false);
                break;
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    private void MoveToTarget(Transform target)
    {
        if (target == null) return;

        Vector3 direction = (target.position + targetPositionOffset - transform.position).normalized;
        theRigidbody.velocity = direction * moveSpeed;

        if (direction.x > 0 && isFlipped) Flip();
        else if (direction.x < 0 && !isFlipped) Flip();
    }

    private void Flip()
    {
        isFlipped = !isFlipped;
        Vector3 localScale = enemyRenderer.transform.localScale;
        localScale.x *= -1;
        enemyRenderer.transform.localScale = localScale;
    }

    private bool DetectPlayer()
    {
        Collider2D[] playersInRange = Physics2D.OverlapCircleAll(transform.position, playerDetectionRadius);
        foreach (Collider2D obj in playersInRange)
        {
            if (obj.gameObject.CompareTag("Player"))
            {
                if (Vector2.Distance(transform.position, obj.transform.position) <= playerDetectionRadius)
                {
                    target = obj.transform;
                    targetPositionOffset = Vector3.zero;
                    return true;
                }
            }
        }
        return false;
    }

    private bool DetectBuildings()
    {
        Collider2D[] buildingsInRange = Physics2D.OverlapCircleAll(transform.position, buildingDetectionRadius);
        foreach (Collider2D obj in buildingsInRange)
        {
            if (obj.gameObject.CompareTag("Building") && obj.gameObject.name != "Tavern")
            {
                Collider2D buildingCollider = obj.GetComponent<Collider2D>();
                if (buildingCollider != null)
                {
                    target = obj.transform;
                    targetPositionOffset = buildingCollider.bounds.center - obj.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    private void FindTavern()
    {
        if (target == null || (target.gameObject.name != "Tavern" && target.gameObject.name != "Tavern(Clone)"))
        {
            GameObject tavernObject = GameObject.Find("Tavern");
            if (tavernObject == null)
            {
                tavernObject = GameObject.Find("Tavern(Clone)");
            }

            if (tavernObject != null)
            {
                target = tavernObject.transform;
                Collider2D tavernCollider = tavernObject.GetComponent<Collider2D>();
                if (tavernCollider != null)
                {
                    targetPositionOffset = tavernCollider.bounds.center - tavernObject.transform.position;
                }
                else
                {
                    targetPositionOffset = Vector3.zero;
                }
            }
            else
            {
                target = null;
                Debug.LogWarning("Neither 'Tavern' nor 'Tavern(Clone)' found in the scene!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, buildingDetectionRadius);
    }
}

```
### Análise
*Inicialmente temos um Enum que representa a possibilidade dos estados no inimigo*

```cs
private enum State
    {
        MoveToTavern,
        MoveToBuilding,
        MoveToPlayer,
        Idle
    }
```
*MoveToTavern:* O inimigo desloca-se em direção ao edifício principal (a taverna).
*MoveToBuilding:* Caso exista um edifício que não seja a taverna, o inimigo direciona-se para ele.
*MoveToPLayer:* O inimigo movimenta-se em direção ao jogador, priorizando-o acima de qualquer outro alvo ou edifício.
*Idle:* Na ausência do jogador e de construções no alcance, o inimigo permanece imóvel.

### Estados e Transições

```cs
private void UpdateState()
    {
        if (DetectPlayer())
        {
            ChangeState(State.MoveToPlayer);
            return;
        }

        if (DetectBuildings())
        {
            ChangeState(State.MoveToBuilding);
            return;
        }

        FindTavern();
        if (target == null)
        {
            ChangeState(State.Idle);
        }
        else
        {
            ChangeState(State.MoveToTavern);
        }
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case State.MoveToTavern:
                animator.SetBool("isMoving", true);
                MoveToTarget(target);
                break;
            case State.MoveToBuilding:
                animator.SetBool("isMoving", true);
                MoveToTarget(target);
                break;
            case State.MoveToPlayer:
                animator.SetBool("isMoving", true);
                MoveToTarget(target);
                break;
            case State.Idle:
                animator.SetBool("isMoving", false);
                break;
        }
    }

    private void ChangeState(State newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
```

***MoveToTavern:***
-Neste estado, o inimigo foca-se em atacar o edifício principal (a taverna).

-A lógica do estado procura o objeto "Tavern" na cena e move o inimigo na sua direção, ajustando o movimento com base na posição atual da taverna.

-Este estado é acionado quando não há jogadores ou outros edifícios no alcance.

***MoveToBuilding:***
-Caso o inimigo detecte um edifício que não seja a taverna, ele muda para este estado.

-Um edifício no alcance é identificado através de um raio de deteção, e o inimigo move-se em direção a ele para atacá-lo.

-Este comportamento é útil para priorizar objetivos estratégicos antes da taverna.

***MoveToPlayer:***
-Este estado é acionado quando o jogador entra no raio de deteção.

-O inimigo considera o jogador como a maior prioridade, movendo-se diretamente para atacá-lo.

-O script verifica continuamente a posição do jogador e ajusta a direção do movimento.

***Idle:***
-No estado Idle, o inimigo não realiza nenhuma ação ativa, ficando imóvel.

-Este estado é usado como fallback, quando não há jogadores nem construções no alcance.

### Transições
As transições entre estados são definidas na função UpdateState(). Essa função avalia as condições do ambiente (presença de jogador, edifícios ou taverna) para determinar o próximo estado.

***Por exemplo:***
Se um jogador é detectado, o estado muda para MoveToPlayer. Se não há jogadores, mas existem edifícios no alcance, o estado muda para MoveToBuilding. Se nenhum dos dois é encontrado, a prioridade é a taverna, mudando o estado para MoveToTavern. Caso contrário, o inimigo fica em Idle.

### Execução do Estado
A função ExecuteState() aplica as ações associadas ao estado atual:
-Inicia animações apropriadas (animator.SetBool("isMoving", true) ou false).

-Ajusta o movimento usando MoveToTarget(), que calcula a direção e velocidade baseada no alvo.



  
  
