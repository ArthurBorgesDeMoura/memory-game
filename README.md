# MemoryGame - Jogo da Memória em C# Windows Forms

Este projeto implementa um Jogo da Memória simples em C# utilizando o Windows Forms, com foco na usabilidade para crianças de 5 a 7 anos. O código é estruturado para ser modular, fácil de entender e permite fácil alteração da parte gráfica e dos ícones.

## Requisitos do Sistema

Para compilar e executar este projeto, você precisará de:

*   **Sistema Operacional:** Windows
*   **IDE:** Visual Studio (Recomendado)
*   **.NET SDK:** .NET 6.0 ou superior (com suporte ao Windows Desktop)

## Estrutura do Projeto

O projeto é composto por dois formulários principais e o ponto de entrada da aplicação:

| Arquivo | Descrição |
| :--- | :--- |
| `Program.cs` | Ponto de entrada da aplicação. Inicia o formulário de seleção de nível (`FormNivel`). |
| `FormNivel.cs` | Formulário de Seleção de Nível. Permite escolher entre Fácil (4x4), Normal (6x6) e Difícil (8x8). Contém o botão "Começar". |
| `FormJogo.cs` | Formulário principal do jogo. Contém toda a lógica do Jogo da Memória, incluindo a criação dinâmica do tabuleiro, o tratamento de cliques e a verificação de vitória. |
| `MemoryGame.csproj` | Arquivo de projeto C# configurado para uma aplicação Windows Forms (`net6.0-windows`). |

## Como Iniciar o Jogo

1.  **Abra o Projeto:** Abra o arquivo `MemoryGame.csproj` no Visual Studio.
2.  **Compilar e Executar:** Pressione `F5` ou clique em "Iniciar" no Visual Studio.
3.  **Seleção de Nível:** O jogo começará no formulário de seleção de nível.
4.  **Jogar:** Após selecionar o nível e clicar em "Começar", o tabuleiro será carregado.

## Especificações do Jogo

O jogo possui 3 níveis de dificuldade, conforme solicitado:

| Nível | Dimensões do Tabuleiro | Número de Pares | Total de Cartas |
| :--- | :--- | :--- | :--- |
| **Fácil** | 4x4 | 8 | 16 |
| **Normal** | 6x6 | 18 | 36 |
| **Difícil** | 8x8 | 32 | 64 |

## Explicação do Código e Pontos de Customização

### 1. `FormNivel.cs` (Seleção de Nível)

Este formulário é o primeiro a ser exibido.

*   **Usabilidade Infantil:** Utiliza cores claras (`LightSkyBlue`, `LightGreen`) e fontes grandes (`Arial`, `Comic Sans MS`) para uma experiência amigável.
*   **Personalização Gráfica:**
    *   A linha `this.BackColor = Color.LightSkyBlue;` define a cor de fundo. Você pode descomentar as linhas comentadas e adicionar um arquivo de imagem chamado `fundo_nivel.jpg` na pasta de recursos para usar uma imagem de fundo.
*   **Lógica de Navegação:** O método `BtnComecar_Click` esconde o formulário atual e inicia uma nova instância de `FormJogo`, passando o nível selecionado como parâmetro.

### 2. `FormJogo.cs` (Lógica do Jogo)

Este é o coração do jogo.

#### A. Configuração e Ícones

*   **Configuração do Nível:** O método `ConfigurarNivel()` define o tamanho do tabuleiro (`_linhas`, `_colunas`) e o número de pares (`_paresTotal`) com base no nível escolhido.
*   **Ícones:** A lista `_icones` é usada para armazenar os símbolos das cartas. Atualmente, ela usa caracteres da fonte "Webdings" para simular ícones.
    *   **CUSTOMIZAÇÃO DE ÍCONES:** Para usar imagens reais, você deve:
        1.  Adicionar as imagens ao projeto como recursos.
        2.  Mudar o tipo de controle de `Button` para um `PictureBox` ou um `Button` com `BackgroundImage`.
        3.  Modificar a lista `_icones` para armazenar os nomes dos recursos das imagens (ex: `"icone_sol"`, `"icone_lua"`).
        4.  No método `Carta_Click`, em vez de mudar `cartaClicada.Text`, você mudaria a `cartaClicada.BackgroundImage` para a imagem correspondente ao seu `Tag`.

#### B. Criação do Tabuleiro (`CriarTabuleiro()`)

*   O tabuleiro é criado dinamicamente usando um **`TableLayoutPanel`**, que é ideal para layouts de grade.
*   O método `GerarIconesDoJogo()` seleciona e embaralha os ícones, garantindo que haja pares.
*   Cada "carta" é um `Button` com as seguintes propriedades:
    *   `Text = "?"`: O texto visível quando a carta está virada.
    *   `Tag = icone`: O ícone real da carta, armazenado na propriedade `Tag` para não ser visível até ser clicado.

#### C. Lógica de Clique (`Carta_Click()`)

*   **Controle de Fluxo:** A variável `_timer.Enabled` impede cliques enquanto o jogo espera para virar as cartas de volta.
*   **Seleção de Par:** As variáveis `_primeiroClique` e `_segundoClique` armazenam as cartas selecionadas.
*   **Verificação:**
    *   Se `_primeiroClique.Tag == _segundoClique.Tag`, o par é encontrado, as cartas são desabilitadas (`Enabled = false`) e a cor de fundo muda para `LightGreen`.
    *   Se não for um par, o `_timer` é iniciado.

#### D. Timer e Fim de Jogo

*   **`Timer_Tick()`:** É chamado após 0.75 segundos (definido em `_timer.Interval`). Ele vira as cartas de volta (`Text = "?"`, `BackColor = LightYellow`) e reabilita os cliques.
*   **`VerificarFimDeJogo()`:** É chamado após cada par encontrado. Se `_paresEncontrados` for igual a `_paresTotal`, exibe a mensagem de "Parabéns" e oferece as opções de "Nova Partida" ou "Encerrar".

## Estrutura de Arquivos

O projeto foi gerado na pasta `MemoryGame/` com os seguintes arquivos:

```
MemoryGame/
├── MemoryGame.csproj
├── Program.cs
├── FormNivel.cs
├── FormNivel.Designer.cs
├── FormJogo.cs
├── FormJogo.Designer.cs
└── README.md
```

**Observação:** O código foi escrito para ser compatível com o .NET 6.0+ e utiliza recursos modernos do C#. Todos os comentários e a documentação estão em português, conforme solicitado.

https://uxwing.com/