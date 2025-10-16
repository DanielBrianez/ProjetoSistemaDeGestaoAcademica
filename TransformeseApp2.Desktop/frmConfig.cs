using TransformeseApp2.BLL;
using TransformeseApp2.DTO;

namespace TransformeseApp2.Desktop
{
    public partial class frmConfig : Form
    {
        private readonly UsuarioBLL usuarioBLL = new();
        string diretorio = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\Transformese";
        string diretorioImagens = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}";
        public frmConfig()
        {
            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            txtNome.Text = AppSession.UsuarioLogado.Nome;
            txtUsuario.Text = AppSession.UsuarioLogado.Login;
            txtSenha.Text = AppSession.UsuarioLogado.Senha;

            if (!string.IsNullOrEmpty(AppSession.UsuarioLogado.UrlFoto) && File.Exists(AppSession.UsuarioLogado.UrlFoto))
            {
                pbFoto.Image = Image.FromFile(AppSession.UsuarioLogado.UrlFoto);
            }
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
                lblFotoCaminho.Text = nomeArquivoImagem;
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {

            try
            {
                var usuarioAtualizado = new UsuarioDTO
                {
                    Id = AppSession.UsuarioLogado.Id,
                    Nome = txtNome.Text,
                    Login = txtUsuario.Text,
                    Senha = txtSenha.Text,
                    UrlFoto = lblFotoCaminho.Text
                };

                usuarioBLL.AtualizarUsuario(usuarioAtualizado);

                // 🔁 Atualiza a sessão global
                AppSession.AtualizarUsuarioLogado(usuarioAtualizado);

                MessageBox.Show($"Usuário {usuarioAtualizado.Nome} atualizado com sucesso!");
                Close();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            var usuario = AppSession.UsuarioLogado;

            if (usuario == null)
            {
                MessageBox.Show("Nenhum usuário encontrado");
                    return;
            }

            var confirma = MessageBox.Show(
                $"{AppSession.UsuarioLogado.Nome}, deseja realmente excluir sua conta? Você será desconectado da sessão",
                "Confirmação", MessageBoxButtons.YesNo);

            if (confirma == DialogResult.Yes)
            {
                usuarioBLL.RemoverUsuario(AppSession.UsuarioLogado.Id);
            }

            MessageBox.Show($"Usuário(s) {AppSession.UsuarioLogado.Nome} removido(s) com sucesso!", "Exclusão de Usuários", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.OpenForms.OfType<frmMain>().FirstOrDefault()?.Close();
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
        }

        private void chkSenha_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSenha.Checked)
            {
                txtSenha.UseSystemPasswordChar = false;
                chkSenha.Text = "Ocultar";
            }
            else
            {
                txtSenha.UseSystemPasswordChar = true;
                chkSenha.Text = "Exibir";
            }
        }
    }
}
