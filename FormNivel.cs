// FormNivel.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MemoryGame
{
    /// <summary>
    /// Formulário principal para a seleção do nível de dificuldade do jogo.
    /// Projetado para ser intuitivo para crianças de 5 a 7 anos.
    /// </summary>
    public partial class FormNivel : Form
    {
        // Variável para armazenar o nível de dificuldade selecionado.
        private string nivelSelecionado = "Fácil";

        public FormNivel()
        {
            InitializeComponent();
            this.Text = "Jogo da Memória - Seleção de Nível";
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Configurações visuais para a usabilidade infantil
            this.Size = new Size(800, 600);
            
            // Tenta carregar uma imagem de fundo (o usuário precisará adicioná-la)
            // Para fins de demonstração, o BackColor será usado se a imagem não existir.
            try
            {
                // **NOTA PARA O USUÁRIO:** Adicione um arquivo chamado "fundo_nivel.jpg" 
                // na pasta de recursos ou comente esta linha e use apenas o BackColor.
                // this.BackgroundImage = Image.FromFile("fundo_nivel.jpg");
                // this.BackgroundImageLayout = ImageLayout.Stretch;
                this.BackColor = Color.LightSkyBlue; // Cor de fundo amigável
            }
            catch { this.BackColor = Color.LightSkyBlue; }

            AdicionarControles();
        }

        /// <summary>
        /// Método para inicializar os componentes do formulário (simulação do Designer.cs).
        /// </summary>
        private void InitializeComponent()
        {
            // Este método normalmente seria gerado automaticamente pelo Visual Studio no FormNivel.Designer.cs
            // Para este ambiente, implementamos a criação dos controles aqui.
        }

        /// <summary>
        /// Adiciona os controles (Rádio Buttons para Nível e Botão Começar) ao formulário.
        /// </summary>
        private void AdicionarControles()
        {
            // 1. Label de Título
            Label lblTitulo = new Label
            {
                Text = "Escolha o Nível de Dificuldade",
                Font = new Font("Arial", 32, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(50, 50)
            };
            this.Controls.Add(lblTitulo);
            
            // 2. GroupBox para os Níveis
            GroupBox gbNiveis = new GroupBox
            {
                Text = "Níveis",
                Font = new Font("Arial", 18, FontStyle.Regular),
                ForeColor = Color.DarkGreen,
                Size = new Size(300, 200),
                Location = new Point(50, 150)
            };
            this.Controls.Add(gbNiveis);

            // 3. Radio Buttons para os Níveis
            RadioButton rbFacil = new RadioButton { Text = "Fácil (4x4)", Tag = "Fácil", Checked = true, Font = new Font("Arial", 16), AutoSize = true, Location = new Point(20, 40) };
            RadioButton rbNormal = new RadioButton { Text = "Normal (6x6)", Tag = "Normal", Font = new Font("Arial", 16), AutoSize = true, Location = new Point(20, 90) };
            RadioButton rbDificil = new RadioButton { Text = "Difícil (8x8)", Tag = "Difícil", Font = new Font("Arial", 16), AutoSize = true, Location = new Point(20, 140) };

            rbFacil.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            rbNormal.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            rbDificil.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);

            gbNiveis.Controls.Add(rbFacil);
            gbNiveis.Controls.Add(rbNormal);
            gbNiveis.Controls.Add(rbDificil);

            // 4. Botão "Começar"
            Button btnComecar = new Button
            {
                Text = "Começar",
                Font = new Font("Comic Sans MS", 28, FontStyle.Bold),
                BackColor = Color.LightGreen,
                ForeColor = Color.DarkRed,
                Size = new Size(250, 100),
                Location = new Point(450, 200),
                FlatStyle = FlatStyle.Flat
            };
            btnComecar.FlatAppearance.BorderSize = 5;
            btnComecar.Click += new EventHandler(BtnComecar_Click);
            this.Controls.Add(btnComecar);
        }

        /// <summary>
        /// Evento acionado ao mudar a seleção de nível.
        /// </summary>
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                nivelSelecionado = rb.Tag.ToString();
            }
        }

        /// <summary>
        /// Evento acionado ao clicar no botão "Começar".
        /// Inicia o jogo no nível selecionado.
        /// </summary>
        private void BtnComecar_Click(object sender, EventArgs e)
        {
            // Esconde o formulário de seleção de nível.
            this.Hide();

            // Cria e exibe o formulário do jogo, passando o nível selecionado.
            FormJogo formJogo = new FormJogo(nivelSelecionado, this);
            


            // Inicia o jogo.
            formJogo.Show();
        }
    }
}
