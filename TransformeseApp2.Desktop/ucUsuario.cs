using System.Data;
using TransformeseApp2.BLL;
using TransformeseApp2.DAL;
using TransformeseApp2.DTO;

namespace TransformeseApp2.Desktop
{
    public partial class ucUsuario : UserControl
    {
        private int? usuarioSelecionadoId = null;
        private readonly UsuarioBLL usuarioBLL = new();

        string diretorio = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\Transformese";
        string diretorioImagens = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}";

        public ucUsuario()
        {
            InitializeComponent();
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

                    // Exibe a imagem de forma segura
                    using (var temp = Image.FromFile(nomeArquivoImagem))
                    {
                        pbFoto.Image = new Bitmap(temp);
                    }

                    // Salva o caminho da foto
                    txtFotoCaminho.Text = nomeArquivoImagem;

                    // Atualiza o usuário selecionado, se houver
                    if (usuarioSelecionadoId.HasValue)
                    {
                        var usuario = Database.Usuarios.FirstOrDefault(u => u.Id == usuarioSelecionadoId.Value);
                        if (usuario != null)
                        {
                            usuario.FotoCaminho = nomeArquivoImagem;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar imagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = new UsuarioDTO
                {
                    Id = Database.Usuarios.Count + 1,
                    Nome = txtNome.Text,
                    Login = txtUsuario.Text,
                    Senha = txtSenha.Text,
                    FotoCaminho = txtFotoCaminho.Text
                };

                usuarioBLL.CadastrarUsuario(usuario);
                MessageBox.Show($"Usuário {txtNome.Text} cadastrado com sucesso", "Cadastro de Usuário", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimparCampos();
                AtualizarGrid();
            }
            catch (Exception erro)
            {
                MessageBox.Show($"Erro: {erro.Message}");
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (!usuarioSelecionadoId.HasValue) return;

            try
            {
                var usuarioAtualizado = new UsuarioDTO
                {
                    Id = usuarioSelecionadoId.Value,
                    Nome = txtNome.Text,
                    Login = txtUsuario.Text,
                    Senha = txtSenha.Text,
                    FotoCaminho = txtFotoCaminho.Text
                };

                usuarioBLL.AtualizarUsuario(usuarioAtualizado);
                MessageBox.Show($"Usuário {usuarioAtualizado.Nome} atualizado com sucesso!");

                LimparCampos();
                usuarioSelecionadoId = null;
                AtualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione ao menos um usuário para exclusão!");
                return;
            }

            List<int> ids = new List<int>();
            List<string> nomes = new List<string>();

            foreach (DataGridViewRow row in dgUsuarios.SelectedRows)
            {
                if (row.Cells["Id"].Value != null)
                    ids.Add(Convert.ToInt32(row.Cells["Id"].Value));
                if (row.Cells["Nome"].Value != null)
                    nomes.Add(row.Cells["Nome"].Value.ToString());
            }

            string nomesFormatados = string.Join(", ", nomes);
            var confirmacao = MessageBox.Show(
                $"Confirma a exclusão do(s) usuário(s) selecionado(s)?\nUsuários selecionados: {nomesFormatados}",
                "Confirmação", MessageBoxButtons.YesNo);

            if (confirmacao == DialogResult.Yes)
            {
                foreach (int id in ids)
                {
                    usuarioBLL.RemoverUsuario(id);
                }
                MessageBox.Show($"Usuário(s) {nomesFormatados} removido(s) com sucesso!", "Exclusão de Usuários", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AtualizarGrid();
                LimparCampos();
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            BuscarUsuario();
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            BuscarUsuario();
        }

        private void dgUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgUsuarios.Rows[e.RowIndex];

            usuarioSelecionadoId = Convert.ToInt32(row.Cells["Id"].Value);
            txtNome.Text = row.Cells["Nome"].Value.ToString();
            txtUsuario.Text = row.Cells["Login"].Value.ToString();
            txtSenha.Text = row.Cells["Senha"].Value.ToString();

            // Exibe foto, se houver
            var usuario = Database.Usuarios.FirstOrDefault(u => u.Id == usuarioSelecionadoId.Value);
            if (usuario != null && !string.IsNullOrEmpty(usuario.FotoCaminho) && File.Exists(usuario.FotoCaminho))
            {
                using (var temp = Image.FromFile(usuario.FotoCaminho))
                {
                    pbFoto.Image = new Bitmap(temp);
                }
                txtFotoCaminho.Text = usuario.FotoCaminho;
            }
            else
            {
                pbFoto.Image = null;
                txtFotoCaminho.Text = string.Empty;
            }

            btnAtualizar.Enabled = true;
        }

        private void ucUsuario_Load(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void chkSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !chkSenha.Checked;
            chkSenha.Text = chkSenha.Checked ? "Ocultar" : "Exibir";
        }

        private void AtualizarGrid()
        {
            dgUsuarios.Columns.Clear();
            dgUsuarios.AutoGenerateColumns = false;
            dgUsuarios.RowTemplate.Height = 60;
            dgUsuarios.AllowUserToAddRows = false;

            var colFoto = new DataGridViewImageColumn
            {
                HeaderText = "Foto",
                Name = "Foto",
                DataPropertyName = "Foto",
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };

            dgUsuarios.Columns.Add(colFoto);

            dgUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Name = "Id" });
            dgUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nome", HeaderText = "Nome", Name = "Nome" });
            dgUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "User", HeaderText = "User", Name = "User" });
            dgUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Senha", HeaderText = "Senha", Name = "Senha" });
            dgUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UrlFoto", HeaderText = "UrlFoto", Name = "UrlFoto" });

            var usuarios = usuarioBLL.ListarUsuarios();

            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nome", typeof(string));
            dt.Columns.Add("Login", typeof(string));
            dt.Columns.Add("Senha", typeof(string));
            dt.Columns.Add("Foto", typeof(Image));
            dt.Columns.Add("URLFoto", typeof(string));

            foreach (var u in usuarios)
            {
                Image? img = null;
                if(string.IsNullOrEmpty(u.UrlFoto) && File.Exists(u.UrlFoto))
                {
                    try
                    {
                        using (var fs = new FileStream(u.UrlFoto, FileMode.Open, FileAccess.Read))
                        {
                            img = Image.FromStream(fs);
                        }
                    }
                    catch (Exception)
                    {

                        img = null;
                    }
                }
                dt.Rows.Add(img,u.Id,u.Login,u.Senha,u.UrlFoto);
            }
            dgUsuarios.DataSource = dt;
        }

        private void BuscarUsuario()
        {
            string termo = txtBusca.Text.Trim().ToLower();

            var filtrados = usuarioBLL.ListarUsuarios()
                .Where(u => u.Nome.ToLower().Contains(termo))
                .Select(u => new
                {
                    u.Id,
                    u.Nome,
                    u.Login,
                    u.Senha
                })
                .ToList();

            dgUsuarios.DataSource = filtrados;
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtUsuario.Clear();
            txtSenha.Clear();
            pbFoto.Image = null;
        }
    }
}
