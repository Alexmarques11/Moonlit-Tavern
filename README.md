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
### Modo Construção ativado
- ***Colocar Construção:*** Mouse 1 (na posição desejada)
- ***Alterar Construção Selecionada:*** Scroll do Mouse

## Técnicas de AI utilizadas
Para complementar e enriquecer a experiência e o desenvolvimento do jogo, implementamos várias técnicas de Inteligência Artificial (IA), permitindo comportamentos dinâmicos e desafiadores para os inimigos e sistemas do jogo. As principais técnicas utilizadas são State Machine (Máquina de Estados), Behaviour Tree (Árvore de Comportamentos) e Pathfinding A*.

## State Machine (Máquina de Estados):
Uma State Machine (Máquina de Estados) é um modelo computacional usado para descrever e controlar o comportamento de um sistema em diferentes estados. Ela é composta por estados, que representam condições ou situações específicas do sistema, como por exemplo, em um jogo, os estados de um inimigo podem ser "Parar", "Correr" ou "Atacar". Além disso, possui transições, que são conexões entre estados e definem como e quando o sistema deve mudar de um estado para outro, geralmente desencadeadas por eventos ou condições, como a proximidade do jogador. A máquina começa em um estado inicial e, a partir disso, executa ações relacionadas a esse estado. Quando uma condição de transição é satisfeita, ela muda para outro estado e começa a executar as ações associadas ao novo estado.

  
  
