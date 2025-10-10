using System.Data;
using TransformeseApp2.BLL;
using TransformeseApp2.DAL;
using TransformeseApp2.DTO;

namespace TransformeseApp2.Desktop
{
    public partial class ucAlunos : UserControl
    {
        string diretorio = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\Tranformese";
        string diretorioImagens = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}";
        public ucAlunos()
        {
            InitializeComponent();
        }

        private readonly AlunoBLL alunoBLL = new();
        private int? alunoSelecionadoId = null;

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                var aluno = new AlunoDTO
                {
                    Id = Database.Alunos.Count + 1,
                    Nome = txtNome.Text,
                    CursoId = (int)cboCursos.SelectedValue,
                    UnidadeId = (int)cboUnidade.SelectedValue,
                    FotoCaminho = txtFotoCaminho.Text
                };

                alunoBLL.CadastrarAluno(aluno);
                MessageBox.Show($"Aluno {txtNome.Text} cadastrado com sucesso", "Cadastro de Aluno", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Clear();
                AtualizarGrid();
            }
            catch (Exception erro)
            {

                MessageBox.Show($"Erro: {erro.Message}");
            }
        }

        private void AtualizarGrid()
        {
            dgAlunos.Columns.Clear();
            dgAlunos.AutoGenerateColumns = false;
            dgAlunos.RowTemplate.Height = 60;
            dgAlunos.AllowUserToAddRows = false;

            var colFoto = new DataGridViewImageColumn
            {
                HeaderText = "Foto",
                Name = "Foto",
                DataPropertyName = "Foto",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };

            dgAlunos.Columns.Add(colFoto);

            dgAlunos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Name = "Id" });
            dgAlunos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nome", HeaderText = "Nome", Name = "Nome" });
            dgAlunos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Curso", HeaderText = "Curso", Name = "Curso" });
            dgAlunos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Unidade", HeaderText = "Unidade", Name = "Unidade" });
            dgAlunos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UrlFoto", HeaderText = "UrlFoto", Name = "UrlFoto" });

            var alunos = alunoBLL.ListarAlunos();

            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nome", typeof(string));
            dt.Columns.Add("Curso", typeof(string));
            dt.Columns.Add("Unidade", typeof(string));
            dt.Columns.Add("Foto", typeof(Image));
            dt.Columns.Add("URLFoto", typeof(string));

            foreach (var a in alunos)
            {
                Image? img = null;
                if (string.IsNullOrEmpty(a.UrlFoto) && File.Exists(a.UrlFoto))
                {
                    try
                    {
                        using (var fs = new FileStream(a.UrlFoto, FileMode.Open, FileAccess.Read))
                        {
                            img = Image.FromStream(fs);
                        }
                    }
                    catch (Exception)
                    {

                        img = null;
                    }
                }
                dt.Rows.Add(img, a.Id, a.Nome, a.Curso, a.Unidade, a.UrlFoto);
            }
            dgAlunos.DataSource = dt;
        }

        private void BuscarAluno()
        {
            string termo = txtBusca.Text.Trim().ToLower();

            var filtrados = alunoBLL.ListarAlunos()
                                         .Where(aluno => aluno.Nome.ToLower().Contains(termo))
                                         .Select(aluno => new
                                         {
                                             aluno.Id,
                                             aluno.Nome,
                                             Curso = Database.Cursos.First(curso => curso.Id == aluno.CursoId).Nome,
                                             Unidade = Database.Unidades.First(unidade => unidade.Id == aluno.UnidadeId).Nome
                                         }).ToList();
            dgAlunos.DataSource = filtrados;
        }

        private void ucAlunos_Load(object sender, EventArgs e)
        {
            if (!Database.Cursos.Any())
            {
                Database.Cursos.Add(new CursoDTO { Id = 1, Nome = "Programador de Sistemas" });
                Database.Cursos.Add(new CursoDTO { Id = 2, Nome = "Banco de Dados" });
            }

            if (!Database.Unidades.Any())
            {
                Database.Unidades.Add(new UnidadeDTO { Id = 1, Nome = "SMP - São Miguel Paulista" });
                Database.Unidades.Add(new UnidadeDTO { Id = 2, Nome = "ITQ - Itaquera" });
            }

            if (!Database.Alunos.Any())
            {
                Database.Alunos.Add(new AlunoDTO { Id = 1, Nome = "Ana Silva", CursoId = 1, UnidadeId = 1 });
                Database.Alunos.Add(new AlunoDTO { Id = 2, Nome = "Jorge von Estranho", CursoId = 2, UnidadeId = 1 });
                Database.Alunos.Add(new AlunoDTO { Id = 3, Nome = "Glebenson", CursoId = 2, UnidadeId = 2 });
            }

            //Populando ComboBox de Cursos
            cboCursos.DataSource = Database.Cursos; //Obtendo lista completa de cursos
            cboCursos.DisplayMember = "Nome"; // Atributo que será exibido no Combo Box
            cboCursos.ValueMember = "Id"; // Atributo que será atrelado ao valor do item exibido
            cboCursos.DropDownStyle = ComboBoxStyle.DropDownList;

            // Populando ComboBox de Unidades
            cboUnidade.DataSource = Database.Unidades; // Obtendo lista completa de unidades
            cboUnidade.DisplayMember = "Nome"; // Atributo que será exibido no Combo Box
            cboUnidade.ValueMember = "Id"; // Atributo que será atrelado ao valor do item exibido
            cboUnidade.DropDownStyle = ComboBoxStyle.DropDownList;

            AtualizarGrid();
        }

        private void btnAtualizar_Click_1(object sender, EventArgs e)
        {
            if (alunoSelecionadoId != null)
            {
                btnAtualizar.Enabled = true;
                try
                {
                    var alunoAtualizado = new AlunoDTO
                    {
                        Id = alunoSelecionadoId.Value,
                        Nome = txtNome.Text,
                        CursoId = (int)cboCursos.SelectedValue,
                        UnidadeId = (int)cboUnidade.SelectedValue,
                        FotoCaminho = txtFotoCaminho.Text
                    };
                    alunoBLL.AtualizarAluno(alunoAtualizado);
                    MessageBox.Show($"Aluno {alunoAtualizado.Nome} atualizado com sucesso!");
                    txtNome.Clear();
                    alunoSelecionadoId = null;
                    AtualizarGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro: {ex.Message}");

                }
            }
        }

        private void dgAlunos_CellClick_2(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgAlunos.Rows[e.RowIndex];

                alunoSelecionadoId = Convert.ToInt32(row.Cells["Id"].Value);
                txtNome.Text = row.Cells["Nome"].Value.ToString();

                string nomeCurso = row.Cells["Curso"].Value.ToString();
                string nomeUnidade = row.Cells["Unidade"].Value.ToString();

                cboCursos.SelectedValue = Database.Cursos.First(c => c.Nome == nomeCurso).Id;
                cboUnidade.SelectedValue = Database.Unidades.First(u => u.Nome == nomeUnidade).Id;

                // Busca o aluno completo na base
                var aluno = Database.Alunos.FirstOrDefault(a => a.Id == alunoSelecionadoId);

                if (aluno != null && !string.IsNullOrEmpty(aluno.FotoCaminho) && File.Exists(aluno.FotoCaminho))
                {
                    pbFoto.Image = null; // <-- força a limpeza antes de carregar
                    using (var temp = Image.FromFile(aluno.FotoCaminho))
                    {
                        pbFoto.Image = new Bitmap(temp);
                    }

                    txtFotoCaminho.Text = aluno.FotoCaminho;
                }
                else
                {
                    pbFoto.Image = null;
                    txtFotoCaminho.Text = string.Empty;
                }

                btnAtualizar.Enabled = true;
            }
        }

        private void txtBusca_TextChanged_1(object sender, EventArgs e)
        {
            BuscarAluno();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            BuscarAluno();
        }

        private void btnExcluir_Click_1(object sender, EventArgs e)
        {
            if (dgAlunos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione ao menos um aluno para exclusão!");
                return;
            }

            List<int> ids = new List<int>();
            List<string> nomesDosAlunos = new List<string>();

            foreach (DataGridViewRow row in dgAlunos.SelectedRows)
            {
                if (row.Cells["Id"].Value != null)
                    ids.Add(Convert.ToInt32(row.Cells["Id"].Value));
                if (row.Cells["Nome"].Value != null)
                    nomesDosAlunos.Add(row.Cells["Nome"].Value.ToString());
            }

            string nomesFormatados = string.Join(", ", nomesDosAlunos);
            var confirmacao = MessageBox.Show(
                $"Confirma a exclusão do(s) aluno(s) selecionado(s)?\nAlunos selecionados: {nomesFormatados}",
                "Confirmação", MessageBoxButtons.YesNo);

            if (confirmacao == DialogResult.Yes)
            {
                foreach (int id in ids)
                {
                    alunoBLL.RemoverAluno(id);
                }
                MessageBox.Show($"Aluno(s) {nomesFormatados} removido(s) com sucesso!", "Exclusão de Aluno", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AtualizarGrid();
            }
        }

        private void pbFoto_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = diretorioImagens,
                    Filter = "Arquivos de imagens | *.jpg;*.jpeg;*.png;*.gif",
                    Title = "Selecione a imagem de perfil"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string nomeArquivoImagem = openFileDialog.FileName;

                    // Exibe a imagem de forma segura (sem travar o arquivo)
                    using (var temp = Image.FromFile(nomeArquivoImagem))
                    {
                        pbFoto.Image = new Bitmap(temp);
                    }

                    // Salva o caminho da foto
                    txtFotoCaminho.Text = nomeArquivoImagem;

                    // Atualiza o aluno selecionado, se houver
                    if (alunoSelecionadoId.HasValue)
                    {
                        var aluno = Database.Alunos.FirstOrDefault(a => a.Id == alunoSelecionadoId.Value);
                        if (aluno != null)
                        {
                            aluno.FotoCaminho = nomeArquivoImagem;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar imagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

