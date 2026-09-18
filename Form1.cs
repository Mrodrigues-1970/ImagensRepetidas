using System;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace ImagensRepetidas
{
    public partial class Form1 : Form
    {
        string gPath = string.Empty;
        List<List<string>> listasInternasRepetidos;
        DataTable tabelaPrincipal;
        List<ImagemAnalizada> listaDeletaveis;

        public Form1()
        {
            InitializeComponent();
        }


        #region Botoes

        private void btProcurarPath_Click(object sender, EventArgs e)
        {
            LimparControles();
            SelecionarPath();
        }

        private void btIniciar_Click(object sender, EventArgs e)
        {
            Escanear();
        }

        private void btDeletar_Click(object sender, EventArgs e)
        {
            DeletarArquivo();
        }

        private void btReagrupar_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            //mover os arquivos dos diretórios agrupados para o primeiro diretório do grupo
            foreach (List<string> iGrupo in listasInternasRepetidos)
            {
                bool deletarDiretorio = false;
                if (iGrupo.Count > 1)
                {
                    string destino = Path.Combine(gPath, iGrupo[0]);
                    for (int i = 1; i < iGrupo.Count; i++)
                    {
                        string origem = Path.Combine(gPath, iGrupo[i]);
                        try
                        {
                            //Se o diretório de origem for diferente do diretório de destino, marca para deletar o diretório de origem após mover os arquivos
                            deletarDiretorio = iGrupo[0] != iGrupo[i];
                            //move todos os arquivos do diretório de origem para o diretório de destino
                            foreach (string file in Directory.GetFiles(origem))
                            {
                                string fileName = Path.GetFileName(file);
                                string destFile = Path.Combine(destino, fileName);
                                //se existir um arquivo com o mesmo nome no destino, adiciona um sufixo para evitar sobrescrever
                                if (File.Exists(destFile))
                                {
                                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                                    string ext = Path.GetExtension(fileName);
                                    destFile = Path.Combine(destino, $"{fileNameWithoutExt}_{iGrupo[i]}{ext}");
                                }
                                File.Move(file, destFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao mover o arquivo: {ex.Message}");
                        }

                        try {
                            if (deletarDiretorio)
                            {
                                //deleta o diretório de origem após mover os arquivos
                                Directory.Delete(origem);
                            }
                        }
                        catch ( Exception ex)
                        {
                            MessageBox.Show($"Erro ao deletar o diretório: {ex.Message}");
                        }
                    }
                }
            }
            Escanear();
            btReagrupar.Enabled = false;
        }

        private void btDeletarRepetidos_Click(object sender, EventArgs e)
        {
            btDeletarRepetidos.Enabled = false;
            //Confirmar com usuário sobre apagar todos as imagens repetidas
            DialogResult confirmacao = MessageBox.Show("Deletar todas as " + listaDeletaveis.Count().ToString() + " imagens repetidas?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacao == DialogResult.Yes)
            {
                foreach (ImagemAnalizada iImagem in listaDeletaveis)
                {
                    File.Delete(iImagem.PathCompleto);
                }
                MessageBox.Show("Todas as " + listaDeletaveis.Count().ToString() + " imagens repetidas foram deletadas.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            LimparControles();
        }

        #endregion


        #region IO

        private void SelecionarPath()
        {
            //Busca o caminho da pasta onde estão as imagens
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                string caminhoAnterior = fbd.SelectedPath;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    gPath = fbd.SelectedPath;
                    lblPath.Text = gPath;
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

        private void DeletarArquivo() {
            //foreach (DataGridViewRow iRow in grdMain.SelectedRows)
            //{
            //    string selectedPath = iRow.Cells[0].Value.ToString();
            //    try
            //    {
            //        //libera o arquivo que está sendo exibido no picturebox para permitir a exclusão
            //        picPreview.Image = null;

            //        File.Delete(selectedPath);
            //        //MessageBox.Show("Imagem deletada com sucesso.");
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Erro ao deletar a imagem: {ex.Message}");
            //    }
            //}
        }

        #endregion



        static List<List<ImagemAnalizada>> EncontarSimilares(string path)
        {
            var DicionarioHash = new Dictionary<string, List<string>>();
            var listaArquivos = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);
            var listaDuplicados = new List<List<string>>();
            List<List<ImagemAnalizada>> listaObjetosDuplicados = new List<List<ImagemAnalizada>>();
            List<ImagemAnalizada> listaImagensAnalizadas = new List<ImagemAnalizada>();

            try {
                foreach (var iArquivo in listaArquivos)
                {
                    ImagemAnalizada oIA = ProcessamentoImagem.RetornaHashMedio(iArquivo);
                    listaImagensAnalizadas.Add(oIA);
                    if (!DicionarioHash.ContainsKey(oIA.Hash))
                    {
                        DicionarioHash[oIA.Hash] = new List<string>();
                    }
                    DicionarioHash[oIA.Hash].Add(iArquivo);
                }

                foreach (var iItem in DicionarioHash)
                {
                    if (iItem.Value.Count > 1)
                    {
                        listaDuplicados.Add(iItem.Value);
                        listaObjetosDuplicados.Add(listaImagensAnalizadas.Where(x => x.Hash == iItem.Key).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao processar o arquivos:{path}\r {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<List<ImagemAnalizada>>();
            }
            return listaObjetosDuplicados;
        }

        //static string RetornaHashMedio(string filePath)
        //{
        //    using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath))
        //    {
        //        // Reduz para 8x8 em tons de cinza
        //        image.Mutate(x => x.Resize(8, 8).Grayscale());

        //        double total = 0;
        //        double[] pixels = new double[64];
        //        int i = 0;

        //        for (int y = 0; y < 8; y++)
        //        {
        //            for (int x = 0; x < 8; x++)
        //            {
        //                var pixel = image[x, y];
        //                double value = pixel.R; // já está em grayscale
        //                pixels[i++] = value;
        //                total += value;
        //            }
        //        }

        //        double avg = total / 64.0;
        //        char[] bits = new char[64];

        //        for (int j = 0; j < 64; j++)
        //        {
        //            bits[j] = pixels[j] >= avg ? '1' : '0';
        //        }

        //        return new string(bits);
        //    }
        //}

        private void picPreview_DoubleClick(object sender, EventArgs e)
        {
            picPreview.Image = null;
        }

        private void grdMain_DoubleClick(object sender, EventArgs e)
        {
            //string selectedPath = grdMain.CurrentCell.Value.ToString();
            //if (File.Exists(selectedPath))
            //{
            //    //usa o path da imagem selecionada para carregar o picturebox
            //    try
            //    {
            //        System.Drawing.Image imagemDoPreview = System.Drawing.Image.FromFile(selectedPath);
            //        if(imagemDoPreview.Width > imagemDoPreview.Height) {
            //            picPreview.Height = 260;
            //        }
            //        else
            //        {
            //            picPreview.Height = 582;
            //        }
            //        picPreview.Image = imagemDoPreview;
            //        //ajusta o tamanho da imagem para caber no picturebox
            //        picPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"Erro ao carregar a imagem: {ex.Message}");
            //    }
            //}
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
            if(listasInternasRepetidos.Count > 0)
            {
                btReagrupar.Enabled = true;
                btReagrupar.Text = "Reagrupar em " + listasInternasRepetidos.Count().ToString() + " folders";
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
            btReagrupar.Text = "Reagrupar em " + listasInternasRepetidos.Count().ToString() + " folders";
        }

        private void Escanear()
        {
            Cursor = Cursors.WaitCursor;
            listasInternasRepetidos = new List<List<string>>();
            int contador = 0;
            listaDeletaveis = new List<ImagemAnalizada>();
            //tabelaPrincipal = new DataTable();
            //tabelaPrincipal.Columns.Add("Imagem", typeof(string));

            List<List<ImagemAnalizada>> resultadoBusca = EncontarSimilares(gPath);
            foreach (List<ImagemAnalizada> iGrupo in resultadoBusca)
            {
                contador++;
                bool primeiraImagem = true;
                List<string> listaDoGrupo = new List<string>();
                //tabelaPrincipal.Rows.Add($"Imagem {contador}:");
                foreach (ImagemAnalizada iObjeto in iGrupo)
                {
                    if (!primeiraImagem)
                    {
                        listaDeletaveis.Add(iObjeto);
                    }
                    else
                    {
                        primeiraImagem = false;
                    }
                    //tabelaPrincipal.Rows.Add(iObjeto);
                    listaDoGrupo.Add(RecuperaDiretorioEnsaio(iObjeto.PathCompleto));
                }
                AdicionarOuAgrupar(listaDoGrupo);
            }
            PreencherTreeView(resultadoBusca);
            MostrarGrupos();
            if(listaDeletaveis.Count > 0)
            {
                btDeletarRepetidos.Enabled = true;
                btDeletarRepetidos.Text = "Deletar " + listaDeletaveis.Count().ToString() + " Imagens Repetidas";
                btDeletar.Enabled = true;
            }            
            Cursor = Cursors.Default;

            if (resultadoBusca.Count == 0)
            {
                MessageBox.Show("Nenhuma imagem similar encontrada.");
            }
            btReagrupar.Enabled = contador > 0;
        }

        private void LimparControles()
        {
            //grdMain.DataSource = null;            
            lstGrupos.DataSource = null;
            picPreview.Image = null;
            if(tabelaPrincipal != null)
            {
                tabelaPrincipal.Clear();
            }
            if (listasInternasRepetidos != null)
            {
                listasInternasRepetidos.Clear();
            }                
            if(listaDeletaveis != null)
            {
                listaDeletaveis.Clear();
            }
        }


        private void PreencherTreeView(List<List<ImagemAnalizada>> listaImagens)
        {
            treeView1.Nodes.Clear();
            for (int i = 0; i < listaImagens.Count; i++)
            {
                // Nó principal para cada lista
                TreeNode nodePrincipal = new TreeNode($"Lista {i + 1}");

                foreach (ImagemAnalizada iImagem in listaImagens[i])
                {
                    // Nó filho para cada imagem
                    TreeNode nodeImagem = new TreeNode(iImagem.PathCompleto);
                    TreeNode nodeLargura = new TreeNode($"Largura: {iImagem.Largura}"); 
                    nodeImagem.Nodes.Add(nodeLargura);
                    TreeNode nodeAltura = new TreeNode($"Altura: {iImagem.Altura}");
                    nodeImagem.Nodes.Add(nodeAltura);

                    nodePrincipal.Nodes.Add(nodeImagem);
                }
                treeView1.Nodes.Add(nodePrincipal);
            }
            treeView1.ExpandAll();
        }

    }
}
