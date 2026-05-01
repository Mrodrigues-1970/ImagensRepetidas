using System;
using System.IO;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Data;

namespace ImagensRepetidas
{
    public partial class Form1 : Form
    {
        string gPath = string.Empty;


        public Form1()
        {
            InitializeComponent();
        }

        private void btProcurarPath_Click(object sender, EventArgs e)
        {
            //Busca o caminho da pasta onde estão as imagens
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    gPath = fbd.SelectedPath;
                    lblPath.Text = gPath;
                }
            }
        }


        static List<List<string>> EncontarSimilares(string path)
        {
            var hashDict = new Dictionary<string, List<string>>();
            var files = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                string hash = RetornaHashMedio(file);
                if (!hashDict.ContainsKey(hash))
                {
                    hashDict[hash] = new List<string>();
                }
                hashDict[hash].Add(file);
            }

            var duplicates = new List<List<string>>();
            foreach (var entry in hashDict)
            {
                if (entry.Value.Count > 1)
                {
                    duplicates.Add(entry.Value);
                }
            }

            return duplicates;
        }

        static string RetornaHashMedio(string filePath)
        {
            using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath))
            {
                // Reduz para 8x8 em tons de cinza
                image.Mutate(x => x.Resize(8, 8).Grayscale());

                double total = 0;
                double[] pixels = new double[64];
                int i = 0;

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        var pixel = image[x, y];
                        double value = pixel.R; // já está em grayscale
                        pixels[i++] = value;
                        total += value;
                    }
                }

                double avg = total / 64.0;
                char[] bits = new char[64];

                for (int j = 0; j < 64; j++)
                {
                    bits[j] = pixels[j] >= avg ? '1' : '0';
                }

                return new string(bits);
            }
        }

        private void btIniciar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            int contador = 0;

            DataTable tabela = new DataTable();
            tabela.Columns.Add("Imagem", typeof(string));

            List<List<string>> resultadoBusca = EncontarSimilares(gPath);
            foreach (List<string> iGrupo in resultadoBusca)
            {
                contador++;
                //listaFinal.Add($"Grupo {contador}:");
                tabela.Rows.Add($"Grupo {contador}:");
                foreach (string iPath in iGrupo)
                {
                    //listaFinal.Add(iPath);
                    tabela.Rows.Add(iPath);
                }
            }
            grdMain.DataSource = tabela;
            Cursor = Cursors.Default;
        }

        private void btExplorer_Click(object sender, EventArgs e)
        {
            //abre uma instância do windows explorer na pasta selecionada
            if (!string.IsNullOrEmpty(gPath) && Directory.Exists(gPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", gPath);
            }
        }
    }
}
