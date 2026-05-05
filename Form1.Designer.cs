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
            folderBrowserDialog1 = new FolderBrowserDialog();
            lblPath = new Label();
            btProcurarPath = new Button();
            btIniciar = new Button();
            grdMain = new DataGridView();
            Imagem = new DataGridViewTextBoxColumn();
            btExplorer = new Button();
            picPreview = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)grdMain).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            SuspendLayout();
            // 
            // lblPath
            // 
            lblPath.AutoSize = true;
            lblPath.Font = new Font("Segoe UI", 11F);
            lblPath.Location = new Point(34, 31);
            lblPath.Name = "lblPath";
            lblPath.Size = new Size(27, 20);
            lblPath.TabIndex = 0;
            lblPath.Text = "C:\\";
            // 
            // btProcurarPath
            // 
            btProcurarPath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btProcurarPath.Location = new Point(823, 28);
            btProcurarPath.Name = "btProcurarPath";
            btProcurarPath.Size = new Size(75, 23);
            btProcurarPath.TabIndex = 1;
            btProcurarPath.Text = "Procurar";
            btProcurarPath.UseVisualStyleBackColor = true;
            btProcurarPath.Click += btProcurarPath_Click;
            // 
            // btIniciar
            // 
            btIniciar.Location = new Point(911, 28);
            btIniciar.Name = "btIniciar";
            btIniciar.Size = new Size(75, 23);
            btIniciar.TabIndex = 2;
            btIniciar.Text = "Iniciar";
            btIniciar.UseVisualStyleBackColor = true;
            btIniciar.Click += btIniciar_Click;
            // 
            // grdMain
            // 
            grdMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            grdMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdMain.Columns.AddRange(new DataGridViewColumn[] { Imagem });
            grdMain.Location = new Point(34, 108);
            grdMain.Name = "grdMain";
            grdMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdMain.Size = new Size(574, 501);
            grdMain.TabIndex = 3;
            grdMain.Click += grdMain_Click;
            // 
            // Imagem
            // 
            Imagem.DataPropertyName = "Imagem";
            Imagem.HeaderText = "Imagem";
            Imagem.Name = "Imagem";
            Imagem.Width = 500;
            // 
            // btExplorer
            // 
            btExplorer.Location = new Point(996, 31);
            btExplorer.Name = "btExplorer";
            btExplorer.Size = new Size(75, 23);
            btExplorer.TabIndex = 4;
            btExplorer.Text = "Explorer";
            btExplorer.UseVisualStyleBackColor = true;
            btExplorer.Click += btExplorer_Click;
            // 
            // picPreview
            // 
            picPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            picPreview.BorderStyle = BorderStyle.FixedSingle;
            picPreview.Location = new Point(659, 108);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(412, 501);
            picPreview.TabIndex = 5;
            picPreview.TabStop = false;
            picPreview.DoubleClick += picPreview_DoubleClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1108, 697);
            Controls.Add(picPreview);
            Controls.Add(btExplorer);
            Controls.Add(grdMain);
            Controls.Add(btIniciar);
            Controls.Add(btProcurarPath);
            Controls.Add(lblPath);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Imagens Repetidas";
            ((System.ComponentModel.ISupportInitialize)grdMain).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FolderBrowserDialog folderBrowserDialog1;
        private Label lblPath;
        private Button btProcurarPath;
        private Button btIniciar;
        private DataGridView grdMain;
        private Button btExplorer;
        private DataGridViewTextBoxColumn Imagem;
        private PictureBox picPreview;
    }
}
