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
        List<List<string>> listasInternasRepetidos;
        DataTable tabelaPrincipal;
        List<string> listaDeletaveis = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btProcurarPath_Click(object sender, EventArgs e)
        {
            grdMain.DataSource = null;

            //Busca o caminho da pasta onde estão as imagens
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (!Path.Exists(fbd.InitialDirectory))
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
            Escanear();
        }

        private void picPreview_DoubleClick(object sender, EventArgs e)
        {
            picPreview.Image = null;
        }

        private void btDeletar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow iRow in grdMain.SelectedRows)
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

        private string RecuperaDiretorioEnsaio(string fullPath)
        {
            int indiceSeparador = fullPath.LastIndexOf("\\");
            string somentePath = fullPath.Substring(0, indiceSeparador);
            indiceSeparador = somentePath.LastIndexOf("\\");
            string diretorio = somentePath.Substring(indiceSeparador + 1);
            return diretorio;
        }

        public void AdicionarOuAgrupar(List<string> parametros)
        {
            if (parametros == null || parametros.Count == 0)
                return;

            // Verifica se algum elemento já existe em alguma lista interna
            var listaEncontrada = listasInternasRepetidos.FirstOrDefault(
                interna => interna.Any(item => parametros.Contains(item))
            );

            if (listaEncontrada != null)
            {
                // Adiciona todos os elementos do parâmetro à lista encontrada
                foreach (var elemento in parametros)
                {
                    if (!listaEncontrada.Contains(elemento))
                    {
                        listaEncontrada.Add(elemento);
                    }
                }
            }
            else
            {
                // Cria uma nova lista com todos os elementos
                listasInternasRepetidos.Add(new List<string>(parametros));
            }
        }

        private List<string> AgruparElementosListas()
        {
            List<string> resultado = new List<string>();
            foreach (List<string> iItem in listasInternasRepetidos)
            {
                string grupo = string.Empty;
                foreach (string iFolder in iItem)
                {
                    grupo += iFolder + " - ";
                }
                resultado.AddRange(grupo);
            }
            return resultado;
        }

        private void MostrarGrupos()
        {
            List<string> grupos = AgruparElementosListas();
            lstGrupos.DataSource = grupos;
        }

        private void Escanear()
        {
            Cursor = Cursors.WaitCursor;
            listasInternasRepetidos = new List<List<string>>();
            int contador = 0;            

            tabelaPrincipal = new DataTable();
            tabelaPrincipal.Columns.Add("Imagem", typeof(string));

            List<List<string>> resultadoBusca = EncontarSimilares(gPath);
            foreach (List<string> iGrupo in resultadoBusca)
            {
                contador++;
                bool primeiraImagem = true;
                List<string> listaDoGrupo = new List<string>();
                tabelaPrincipal.Rows.Add($"Imagem {contador}:");
                foreach (string iPath in iGrupo)
                {
                    if (!primeiraImagem)
                    {
                        listaDeletaveis.Add(iPath);
                    }
                    else
                    {
                        primeiraImagem = false;
                    }
                    tabelaPrincipal.Rows.Add(iPath);
                    listaDoGrupo.Add(RecuperaDiretorioEnsaio(iPath));
                }
                AdicionarOuAgrupar(listaDoGrupo);
            }
            grdMain.DataSource = tabelaPrincipal;
            MostrarGrupos();
            Cursor = Cursors.Default;

            if (tabelaPrincipal.Rows.Count == 0)
            {
                MessageBox.Show("Nenhuma imagem similar encontrada.");
            }
        }

        private void btReagrupar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            //mover os arquivos dos diretórios agrupados para o primeiro diretório do grupo
            foreach (List<string> iGrupo in listasInternasRepetidos)
            {
                if (iGrupo.Count > 1)
                {
                    string destino = Path.Combine(gPath, iGrupo[0]);
                    for (int i = 1; i < iGrupo.Count; i++)
                    {
                        string origem = Path.Combine(gPath, iGrupo[i]);
                        try
                        {
                            //move todos os arquivos do diretório de origem para o diretório de destino
                            foreach (string file in Directory.GetFiles(origem))
                            {
                                string fileName = Path.GetFileName(file);
                                string destFile = Path.Combine(destino, fileName);
                                File.Move(file, destFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao mover o arquivo: {ex.Message}");
                        }
                    }
                }
            }
            Escanear();
        }

        private void btDeletarRepetidos_Click(object sender, EventArgs e)
        {
            //Confirmar com usuário sobre apagar todos as imagens repetidas
            DialogResult confirmacao = MessageBox.Show("Deletar todas as imagens repetidas", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(confirmacao == DialogResult.Yes)
            {
                foreach(string iImagem in listaDeletaveis)
                {
                    File.Delete(iImagem);
                }
                MessageBox.Show("Todas as " + listaDeletaveis.Count().ToString() + " imagens repetidas foram deletadas.","Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
