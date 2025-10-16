using TransformeseApp2.BLL;

namespace TransformeseApp2.Desktop
{

    public partial class frmLogin : Form
    {
        private readonly UsuarioBLL usuarioBLL = new();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = usuarioBLL.Login(txtUsuario.Text, txtSenha.Text);
                AppSession.UsuarioLogado = usuario;
                mdEntrar.Show($"Seja bem vindo(a) {AppSession.UsuarioLogado.Nome}!");

                frmMain principal = new frmMain();
                principal.Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            frmCadastroUsuario frmCadastroUsuario = new frmCadastroUsuario();
            frmCadastroUsuario.Show();
        }

        private void btnKill_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblEsqueciSenha_Click(object sender, EventArgs e)
        {
            frmEsqueciSenha frmEsqueciSenha = new frmEsqueciSenha();
            frmEsqueciSenha.Show();
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtSenha.Text))
                {
                    txtSenha.Focus();
                }
                else
                {
                    btnEntrar.PerformClick();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }


        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEntrar.PerformClick();
            }
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


