namespace ImagensRepetidas
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            folderBrowserDialog1 = new FolderBrowserDialog();
            lblPath = new Label();
            btProcurarPath = new Button();
            btIniciar = new Button();
            picPreview = new PictureBox();
            btDeletar = new Button();
            lstGrupos = new ListBox();
            btReagrupar = new Button();
            btDeletarRepetidos = new Button();
            treeView1 = new TreeView();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            SuspendLayout();
            // 
            // lblPath
            // 
            lblPath.AutoSize = true;
            lblPath.Font = new Font("Segoe UI", 11F);
            lblPath.Location = new Point(12, 28);
            lblPath.Name = "lblPath";
            lblPath.Size = new Size(27, 20);
            lblPath.TabIndex = 0;
            lblPath.Text = "C:\\";
            // 
            // btProcurarPath
            // 
            btProcurarPath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btProcurarPath.Location = new Point(840, 28);
            btProcurarPath.Name = "btProcurarPath";
            btProcurarPath.Size = new Size(75, 23);
            btProcurarPath.TabIndex = 1;
            btProcurarPath.Text = "Procurar";
            btProcurarPath.UseVisualStyleBackColor = true;
            btProcurarPath.Click += btProcurarPath_Click;
            // 
            // btIniciar
            // 
            btIniciar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btIniciar.Location = new Point(932, 28);
            btIniciar.Name = "btIniciar";
            btIniciar.Size = new Size(75, 23);
            btIniciar.TabIndex = 2;
            btIniciar.Text = "Iniciar";
            btIniciar.UseVisualStyleBackColor = true;
            btIniciar.Click += btIniciar_Click;
            // 
            // picPreview
            // 
            picPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            picPreview.BorderStyle = BorderStyle.FixedSingle;
            picPreview.Location = new Point(840, 59);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(412, 582);
            picPreview.TabIndex = 5;
            picPreview.TabStop = false;
            picPreview.DoubleClick += picPreview_DoubleClick;
            // 
            // btDeletar
            // 
            btDeletar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btDeletar.Enabled = false;
            btDeletar.Location = new Point(1025, 28);
            btDeletar.Name = "btDeletar";
            btDeletar.Size = new Size(75, 23);
            btDeletar.TabIndex = 6;
            btDeletar.Text = "Deletar";
            btDeletar.UseVisualStyleBackColor = true;
            btDeletar.Click += btDeletar_Click;
            // 
            // lstGrupos
            // 
            lstGrupos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lstGrupos.Font = new Font("Segoe UI", 11F);
            lstGrupos.FormattingEnabled = true;
            lstGrupos.HorizontalScrollbar = true;
            lstGrupos.Location = new Point(12, 59);
            lstGrupos.Name = "lstGrupos";
            lstGrupos.Size = new Size(264, 504);
            lstGrupos.TabIndex = 8;
            // 
            // btReagrupar
            // 
            btReagrupar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btReagrupar.Enabled = false;
            btReagrupar.Location = new Point(12, 579);
            btReagrupar.Name = "btReagrupar";
            btReagrupar.Size = new Size(264, 23);
            btReagrupar.TabIndex = 9;
            btReagrupar.Text = "Reagrupar";
            btReagrupar.UseVisualStyleBackColor = true;
            btReagrupar.Click += btReagrupar_Click;
            // 
            // btDeletarRepetidos
            // 
            btDeletarRepetidos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btDeletarRepetidos.Enabled = false;
            btDeletarRepetidos.Location = new Point(12, 618);
            btDeletarRepetidos.Name = "btDeletarRepetidos";
            btDeletarRepetidos.Size = new Size(264, 23);
            btDeletarRepetidos.TabIndex = 10;
            btDeletarRepetidos.Text = "Deletar Repetidos";
            btDeletarRepetidos.UseVisualStyleBackColor = true;
            btDeletarRepetidos.Click += btDeletarRepetidos_Click;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(292, 59);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(542, 582);
            treeView1.TabIndex = 11;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 729);
            Controls.Add(treeView1);
            Controls.Add(btDeletarRepetidos);
            Controls.Add(btReagrupar);
            Controls.Add(lstGrupos);
            Controls.Add(btDeletar);
            Controls.Add(picPreview);
            Controls.Add(btIniciar);
            Controls.Add(btProcurarPath);
            Controls.Add(lblPath);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Imagens Repetidas";
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FolderBrowserDialog folderBrowserDialog1;
        private Label lblPath;
        private Button btProcurarPath;
        private Button btIniciar;
        private PictureBox picPreview;
        private Button btDeletar;
        private ListBox lstGrupos;
        private Button btReagrupar;
        private Button btDeletarRepetidos;
        private TreeView treeView1;
    }
}
