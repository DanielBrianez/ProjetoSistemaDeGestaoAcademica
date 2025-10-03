using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Logging;
using TransformeseApp2.BLL;
using TransformeseApp2.DAL;
using TransformeseApp2.DTO;

namespace TransformeseApp2.Desktop
{
    public partial class frmCadastroUsuario : Form
    {
        UsuarioBLL UsuarioBLL = new UsuarioBLL();

        string diretorio = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}\Tranformese";
        string diretorioImagens = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}";
        public frmCadastroUsuario()
        {
            InitializeComponent();
        }

        private void txtNome_Click(object sender, EventArgs e)
        {
            txtNome.Text = string.Empty;
        }

        private void txtUsuario_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = string.Empty;
        }

        private void txtSenha_Click(object sender, EventArgs e)
        {
            txtSenha.Text = string.Empty;
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
            string nomeImg = $"{Database.Usuarios.Count + 1} - {txtUsuario.Text}.jpg";

            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            string UrlImagem = Path.Combine(diretorio, nomeImg);

            Image imagem = pbFoto.Image;
            imagem.Save(UrlImagem);

            var usuario = new UsuarioDTO
            {
                Id = Database.Usuarios.Count + 1,
                Nome = txtNome.Text,
                Login = txtUsuario.Text,
                Senha = txtSenha.Text,
                UrlFoto = txtFotoCaminho.Text
            };

            UsuarioBLL.CadastrarUsuario(usuario);
            MessageBox.Show($"Usuário {usuario.Nome} cadastrado com sucesso!");
            txtUsuario.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtSenha.Text = string.Empty;
            pbFoto.Image = null;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
