Projeto de RTS 


Contém:

- Movimentação de câmera (pelo teclado).
- Seleção de Unidades
- Movimentação
- Detecção de unidades inimigas próximas e ataque
- Treinamento de unidades na construção.

A ser implementado:

- Névoa de visão (Fog of War)
- Sistema de building
- Unidades em terreno expecífico (agua por exemplo)
- Gestão de recursos
- Gestão de tipos de unidades usando configuração de dados
- Arvore tecnológica
- Sistema Multiplayer (dedicado ou host)


Scripts:


PlayerController:

Gerencia o sistema de seleção e ordem de movimentação das unidades.

Para funcionar precisa:
- Adicionar um layer que será detectado como terreno
- Adicioanr um layer na qual todos os itens clicáveis (unidades ou não) estarão inseridos

Unit:

Todo item clicável para que possa ser uma unidade e habilitar a seleção, movimentação e treinamento de unidades, precisa estar com esse componente.

Para funcionar precisa:
- Ter a tag player ou outra para saber se a unidade é controlável ou não.
- Configurar o gameobject para a layer das unidades clicáveis (a mesma layer configurado lá no player controller.
- Ter um navmesh agent / navmesh obstacle para construção ou item fixo.
- Ter um capsule collider (ou cube collider/sphere collider, vai depender da forma da unidade)
- Ter um animator para caso a unidade ter animação.
