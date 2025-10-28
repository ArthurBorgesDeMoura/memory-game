// Program.cs
using System;
using System.Windows.Forms;

namespace MemoryGame
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Para garantir a compatibilidade com versões anteriores e o funcionamento correto dos controles.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicia a aplicação com o formulário de Seleção de Nível.
            // O formulário principal (Form1) será usado como o formulário de Seleção de Nível.
            Application.Run(new FormNivel());
        }
    }
}
