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
                if(!Path.Exists(fbd.InitialDirectory))
                {
                    fbd.InitialDirectory = "D:\\";
                }

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
                tabela.Rows.Add($"Grupo {contador}:");
                foreach (string iPath in iGrupo)
                {
                    tabela.Rows.Add(iPath);
                }
            }
            grdMain.DataSource = tabela;
            Cursor = Cursors.Default;

            if (tabela.Rows.Count == 0)
            {
                MessageBox.Show("Nenhuma imagem similar encontrada.");
            }

        }

        private void btExplorer_Click(object sender, EventArgs e)
        {
            //abre uma instância do windows explorer na pasta selecionada
            if (!string.IsNullOrEmpty(gPath) && Directory.Exists(gPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", gPath);
            }
        }

        private void picPreview_DoubleClick(object sender, EventArgs e)
        {
            picPreview.Image = null;
        }

        private void btDeletar_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;
            if (!chkAutorizarDelete.Checked) {
                //Pede confirmação para deletar a imagem selecionada
                result = MessageBox.Show("Tem certeza que deseja deletar a imagem selecionada?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }


            foreach (DataGridViewRow iRow in grdMain.SelectedRows)
            {


                if (result == DialogResult.Yes)
                {
                    string selectedPath = iRow.Cells[0].Value.ToString();
                    try
                    {
                        //libera o arquivo que está sendo exibido no picturebox para permitir a exclusão
                        picPreview.Image = null;

                        File.Delete(selectedPath);
                        //MessageBox.Show("Imagem deletada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao deletar a imagem: {ex.Message}");
                    }
                }
            }

        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {
            string selectedPath = grdMain.CurrentCell.Value.ToString();
            if (File.Exists(selectedPath))
            {
                //usa o path da imagem selecionada para carregar o picturebox
                try
                {
                    picPreview.Image = System.Drawing.Image.FromFile(selectedPath);
                    //ajusta o tamanho da imagem para caber no picturebox
                    picPreview.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar a imagem: {ex.Message}");
                }
            }
        }
    }
}
