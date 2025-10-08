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

        string diretorio = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\Tranformese";
        string diretorioImagens = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}";
        public ucUsuario()
        {
            InitializeComponent();
        }

        private void pbFoto_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory("diretorio");
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = diretorioImagens;
            openFileDialog.Filter = "Arquivos de imagens | *.jpg;*.jpeg;*.png;*.gif";
            openFileDialog.Title = "Selecione a imagem de perfil";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string nomeArquivoImagem = openFileDialog.FileName;

                //Exibe a imagem escolhida no PictureBox
                pbFoto.Image = Image.FromFile(nomeArquivoImagem);
                //Salva o Caminho da foto
                txtFotoCaminho.Text = nomeArquivoImagem;
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
                };

                usuarioBLL.CadastrarUsuario(usuario);
                MessageBox.Show($"Usuário {txtNome.Text} cadastrado com sucesso", "Cadastro de Usuário", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Clear();
                txtUsuario.Clear();
                txtSenha.Clear();
                AtualizarGrid();
            }
            catch (Exception erro)
            {

                MessageBox.Show($"Erro: {erro.Message}");
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
            List<string> nomesDosUsuarios = new List<string>();

            foreach (DataGridViewRow row in dgUsuarios.SelectedRows)
            {
                if (row.Cells["Id"].Value != null)
                    ids.Add(Convert.ToInt32(row.Cells["Id"].Value));
                if (row.Cells["Nome"].Value != null)
                    nomesDosUsuarios.Add(row.Cells["Nome"].Value.ToString());
            }

            string nomesFormatados = string.Join(", ", nomesDosUsuarios);
            var confirmacao = MessageBox.Show(
                $"Confirma a exclusão do(s) usuário(s) selecionado(s)?\nUsuários selecionados: {nomesFormatados}",
                "Confirmação", MessageBoxButtons.YesNo);

            if (confirmacao == DialogResult.Yes)
            {
                foreach (int id in ids)
                {
                    usuarioBLL.RemoverUsuario(id);
                }
                MessageBox.Show($"Usuários(s) {nomesFormatados} removido(s) com sucesso!", "Exclusão de Usuários", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AtualizarGrid();
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            BuscarUsuario();
        }
        private void AtualizarGrid()
        {
            var lista = usuarioBLL.ListarUsuarios()
                                .Select(usuario => new
                                {
                                    usuario.Id,
                                    usuario.Nome,
                                    usuario.Login
                                }).ToList();

            dgUsuarios.DataSource = lista;

        }

        private void BuscarUsuario()
        {
            string termo = txtBusca.Text.Trim().ToLower();

            var filtrados = usuarioBLL.ListarUsuarios()
                                         .Where(usuario => usuario.Nome.ToLower().Contains(termo))
                                         .Select(usuario => new
                                         {
                                             usuario.Id,
                                             usuario.Nome,
                                             usuario.Login,
                                         }).ToList();
            dgUsuarios.DataSource = filtrados;
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            BuscarUsuario();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (usuarioSelecionadoId != null)
            {
                btnAtualizar.Enabled = true;
                try
                {
                    var usuarioAtualizado = new UsuarioDTO
                    {
                        Id = usuarioSelecionadoId.Value,
                        Nome = txtNome.Text,
                        Login = txtUsuario.Text
                    };

                    usuarioBLL.AtualizarUsuario(usuarioAtualizado);
                    MessageBox.Show($"Usuário {usuarioAtualizado.Nome} atualizado com sucesso!");
                    txtNome.Clear();
                    usuarioAtualizado = null;
                    AtualizarGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro: {ex.Message}");

                }

            }
        }

        private void dgUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                DataGridViewRow row = dgUsuarios.Rows[e.RowIndex];

                usuarioSelecionadoId = Convert.ToInt32(row.Cells["Id"].Value);
                txtNome.Text = row.Cells["Nome"].Value.ToString();

                string nomeUsuario = row.Cells["Nome"].Value.ToString();
                string nomeLogin = row.Cells["Login"].Value.ToString();

                btnAtualizar.Enabled = true;
            }
        }

        private void ucUsuario_Load(object sender, EventArgs e)
        {
            AtualizarGrid();
        }
    }
}

