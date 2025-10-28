// FormJogo.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MemoryGame
{
    /// <summary>
    /// Formulário principal do jogo da memória.
    /// Contém a lógica para a criação do tabuleiro, seleção de cartas e verificação de pares.
    /// </summary>
    public partial class FormJogo : Form
    {
        // --- Variáveis de Configuração do Jogo ---
        private readonly string _nivel;
        private int _linhas;
        private int _colunas;
        private int _paresTotal;
        private int _paresEncontrados = 0;
        private FormNivel _formNivelPai;
        private readonly List<string> _icones = new List<string>
        {
            "girassol", "kiwi", "alho", "flor", "mirtilo", "uva", "banana", "milho", "toranja", "mato", "flor-roxa", "pepino", "espinafre", "pessego", "pomarola", "batata", "azeitona", "quiabo", "berinjela", "cenoura", "rabanete", "pimentao", "durian", "melancia", "pera", "coco", "maca", "manga", "folha", "arvore", "pimenta", "trevo"
        };
        
        // --- Variáveis de Estado do Jogo ---
        private Button _primeiroClique = null;
        private Button _segundoClique = null;
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        private readonly System.Diagnostics.Stopwatch _cronometro = new System.Diagnostics.Stopwatch();
        private readonly System.Windows.Forms.Timer _timerContador = new System.Windows.Forms.Timer(); // Timer para atualizar o display
        private Label _lblTempo;
        /// <summary>
        /// Construtor do formulário do jogo.
        /// </summary>
        /// <param name="nivel">O nível de dificuldade selecionado (Fácil, Normal, Difícil).</param>
        public FormJogo(string nivel, FormNivel formNivel)
        {
            InitializeComponent();
            this.Size = new Size(800, 600);
            _nivel = nivel;
            _formNivelPai = formNivel; // Guarda a referência do formulário pai
            this.Text = $"Jogo da Memória - Nível {_nivel}";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightBlue; // Fundo amigável

            ConfigurarNivel();
            CriarTabuleiro();
            
            // Configura o timer para virar as cartas de volta
            _timer.Interval = 750; // 0.75 segundos
            _timer.Tick += Timer_Tick;

            _lblTempo = new Label
            {
                Text = "Tempo: 00:00",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                AutoSize = true,
                // Posicione o Label em um local visível no formulário
                Location = new Point(10, 10)
            };
            this.Controls.Add(_lblTempo);

            _timerContador.Interval = 1000; // Atualiza a cada 1 segundo
            _timerContador.Tick += TimerContador_Tick;
            _timerContador.Start(); // Inicia o timer de atualização
            _cronometro.Start(); // Inicia a contagem do tempo de jogo
        }


        private void TimerContador_Tick(object sender, EventArgs e)
        {
            // Obtém o tempo decorrido do cronômetro
            TimeSpan ts = _cronometro.Elapsed;

            // Formata o tempo como MM:SS (Minutos:Segundos)
            string tempoFormatado = String.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds);

            // Atualiza o rótulo
            _lblTempo.Text = "Tempo: " + tempoFormatado;
        }
        /// <summary>
        /// Método para inicializar os componentes do formulário (simulação do Designer.cs).
        /// </summary>
        private void InitializeComponent()
        {
            // Este método normalmente seria gerado automaticamente pelo Visual Studio no FormJogo.Designer.cs
            // Para este ambiente, o layout será feito programaticamente.
        }

        /// <summary>
        /// Define as dimensões do tabuleiro e o número de pares com base no nível selecionado.
        /// </summary>
        private void ConfigurarNivel()
        {
            switch (_nivel)
            {
                case "Fácil":
                    _linhas = 4;
                    _colunas = 4;
                    _paresTotal = 8; // 4x4 = 16 cartas, 8 pares
                    break;
                case "Normal":
                    _linhas = 6;
                    _colunas = 6;
                    _paresTotal = 18; // 6x6 = 36 cartas, 18 pares
                    break;
                case "Difícil":
                    _linhas = 8;
                    _colunas = 8;
                    _paresTotal = 32; // 8x8 = 64 cartas, 32 pares
                    break;
                default:
                    // Padrão para Fácil se algo der errado
                    _linhas = 4;
                    _colunas = 4;
                    _paresTotal = 8;
                    break;
            }
        }

        /// <summary>
        /// Cria o layout do tabuleiro do jogo dinamicamente.
        /// </summary>
        private void CriarTabuleiro()
        {
            // O tabuleiro será um TableLayoutPanel
            TableLayoutPanel tlpTabuleiro = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.CornflowerBlue // Cor do tabuleiro
            };
            
            tlpTabuleiro.RowStyles.Clear();
            for (int i = 0; i < _linhas; i++)
            {
                tlpTabuleiro.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / _linhas));
            }
            
            tlpTabuleiro.ColumnStyles.Clear();
            for (int i = 0; i < _colunas; i++)
            {
                tlpTabuleiro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / _colunas));
            }

            // Gera a lista de ícones para o jogo
            List<string> iconesDoJogo = GerarIconesDoJogo();

            // Cria os botões (cartas) e os adiciona ao TableLayoutPanel
            for (int i = 0; i < _linhas; i++)
            {
                for (int j = 0; j < _colunas; j++)
                {
                    // O índice da carta na lista de ícones
                    int indiceIcone = i * _colunas + j;
                    
                    Button carta = new Button
                    {
                        // O Text (ícone) é armazenado no Tag para não ser revelado inicialmente
                        Tag = iconesDoJogo[indiceIcone], 
                        Text = " ", // Ícone de carta virada
                        Dock = DockStyle.Fill,
                        Font = new Font("Webdings", 48, FontStyle.Bold), // Fonte para ícones
                        BackColor = Color.LightYellow, // Cor da carta virada
                        ForeColor = Color.Black,
                        FlatStyle = FlatStyle.Flat,
                        BackgroundImageLayout = ImageLayout.Stretch
                    };
                    carta.FlatAppearance.BorderSize = 5;
                    carta.Click += Carta_Click;
                    
                    tlpTabuleiro.Controls.Add(carta, j, i); // j = coluna, i = linha
                }
            }



            this.Controls.Add(tlpTabuleiro);
        }

        /// <summary>
        /// Gera e embaralha a lista de ícones que serão usados no tabuleiro.
        /// </summary>
        /// <returns>Uma lista de strings com os ícones duplicados e embaralhados.</returns>
        private List<string> GerarIconesDoJogo()
        {
            // 1. Seleciona os ícones necessários (pares)
            List<string> iconesSelecionados = _icones.Take(_paresTotal).ToList();

            // 2. Duplica os ícones para formar os pares
            List<string> iconesDoJogo = iconesSelecionados.Concat(iconesSelecionados).ToList();

            // 3. Embaralha a lista (algoritmo Fisher-Yates)
            Random rng = new Random();
            int n = iconesDoJogo.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = iconesDoJogo[k];
                iconesDoJogo[k] = iconesDoJogo[n];
                iconesDoJogo[n] = value;
            }

            return iconesDoJogo;
        }

        /// <summary>
        /// Evento de clique em uma carta. Contém a lógica principal do jogo.
        /// </summary>
        private void Carta_Click(object sender, EventArgs e)
        {
            // Se o timer estiver rodando (esperando para virar cartas de volta), ignora o clique.
            if (_timer.Enabled)
                return;

            Button cartaClicada = (Button)sender;

            // Se a carta já estiver virada ou já tiver sido encontrada, ignora o clique.
            if (cartaClicada.BackgroundImage != null || cartaClicada.Enabled == false)
                return;

            string iconeCarta = cartaClicada.Tag.ToString();

            // Vira a carta, revelando o ícone que está no Tag.
           // cartaClicada.Text = cartaClicada.Tag.ToString();
            cartaClicada.BackColor = Color.White; // Cor da carta virada
            cartaClicada.BackgroundImage = MemoryGame.Properties.Resources.ResourceManager.GetObject(iconeCarta) as Image;
            cartaClicada.BackgroundImage = MemoryGame.Properties.Resources.ResourceManager.GetObject(iconeCarta) as Image;

            // --- Lógica de Seleção de Cartas ---

            if (_primeiroClique == null)
            {
                // É a primeira carta do par.
                _primeiroClique = cartaClicada;
                return;
            }

            // É a segunda carta do par.
            _segundoClique = cartaClicada;
            
            // Desabilita temporariamente todos os cliques enquanto verifica o par.
            DesabilitarCliques();

            // Verifica se os ícones são iguais.
            if (_primeiroClique.Tag.ToString() == _segundoClique.Tag.ToString())
            {
                // Par encontrado! Mantém as cartas viradas.
                _primeiroClique.Enabled = false; // Desabilita permanentemente
                _segundoClique.Enabled = false; // Desabilita permanentemente
                _primeiroClique.BackColor = Color.LightGreen; // Indica que o par foi encontrado
                _segundoClique.BackColor = Color.LightGreen;
                
                _paresEncontrados++;

                // Limpa as variáveis de clique e reabilita os cliques.
                _primeiroClique = null;
                _segundoClique = null;
                HabilitarCliques();

                // Verifica se o jogo terminou.
                VerificarFimDeJogo();
            }
            else
            {
                // Não é um par. Inicia o timer para virar as cartas de volta.
                _timer.Start();
            }
        }

        /// <summary>
        /// Evento do Timer: Vira as duas cartas de volta para a posição inicial.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Para o timer
            _timer.Stop();

            // Vira as cartas de volta
            _primeiroClique.BackgroundImage = null;
            _segundoClique.BackgroundImage = null;
            _primeiroClique.BackColor = Color.LightYellow;
            _segundoClique.BackColor = Color.LightYellow;

            // Limpa as variáveis de clique
            _primeiroClique = null;
            _segundoClique = null;

            // Reabilita os cliques
            HabilitarCliques();
        }

        /// <summary>
        /// Desabilita temporariamente os cliques em todas as cartas.
        /// </summary>
        private void DesabilitarCliques()
        {
            foreach (Control control in this.Controls[0].Controls)
            {
                if (control is Button button)
                {
                    // Apenas desabilita botões que ainda não foram encontrados
                    if (button.Enabled)
                    {
                        button.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Reabilita os cliques em todas as cartas que ainda não foram encontradas.
        /// </summary>
        private void HabilitarCliques()
        {
            foreach (Control control in this.Controls[0].Controls)
            {
                if (control is Button button)
                {
                    // Apenas reabilita botões que não foram permanentemente desabilitados
                    if (button.Tag != null && button.Enabled == false && button.BackColor != Color.LightGreen)
                    {
                        button.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se todos os pares foram encontrados e exibe a mensagem de vitória.
        /// </summary>
        private void VerificarFimDeJogo()
        {
            if (_paresEncontrados == _paresTotal)
            {
                _cronometro.Stop();
                _timerContador.Stop();

                TimeSpan tsFinal = _cronometro.Elapsed;
                string tempoFinalFormatado = String.Format("{0} minutos e {1} segundos",
                                                            tsFinal.Minutes,
                                                            tsFinal.Seconds);
                // Jogo concluído!
                string mensagem = $"Parabéns! Você encontrou todos os pares em {tempoFinalFormatado}!";
                string titulo = "Fim de Jogo";
                
                // Opções: Sim = Nova Partida, Não = Encerrar
                DialogResult resultado = MessageBox.Show(mensagem + "\n\nDeseja iniciar uma nova partida?", titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (resultado == DialogResult.Yes)
                {
                    // Inicia uma nova partida voltando para o FormNivel
                    _formNivelPai.Show(); // Reexibe o FormNivel que estava escondido
                    this.Close();
                }
                else
                {
                    // Encerrar o jogo
                    Application.Exit();
                }
            }
        }
    }
}
