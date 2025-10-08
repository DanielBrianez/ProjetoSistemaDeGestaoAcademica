using System.Net.Quic;
using TransformeseApp2.BLL;

namespace TransformeseApp2.Desktop
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            AbrirUserControl(new ucHome());
            lblUsuario.Text = Session.UsuarioLogado.Nome ?? "Usuário";
            AtualizarUsuarioLogado();
        }
        private void AbrirUserControl(UserControl uc)
        {
            panelConteudo.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelConteudo.Controls.Add(uc);

        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            fecharMain();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            panelConteudo.Controls.Clear();
            AbrirUserControl(new ucHome());
        }

        private void fecharMain()
        {
            Close();
            frmLogin telaLogin = new();
            telaLogin.ShowDialog();
        }

        private void AtualizarUsuarioLogado()
        {
            lblUsuario.Left = pbFoto.Left + (pbFoto.Width - lblUsuario.Width) / 2;
            lblUsuario.Top = pbFoto.Bottom + 4;
            lblUsuario.Text = Session.UsuarioLogado.Nome ?? "Usuário";

            if (!string.IsNullOrEmpty(Session.UsuarioLogado.UrlFoto)&& File.Exists((Session.UsuarioLogado.UrlFoto)))
            {
                pbFoto.Image = Image.FromFile(Session.UsuarioLogado.UrlFoto);
            }
        }

        private void pbAlert_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.TryParse(npNotifica.Text, out int qdeNotification))
                {
                    if (qdeNotification > 0)
                    {
                        qdeNotification--;
                        npNotifica.Text = qdeNotification > 0 ? qdeNotification.ToString() : string.Empty;
                        npNotifica.FillColor = qdeNotification > 0 ? npNotifica.FillColor : Color.Transparent;
                        string mensagem = qdeNotification > 0 ? $"Você tem {qdeNotification} notificações" : "Você não tem mais notificações!";
                        mdNotifica.Show(mensagem);

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            panelConteudo.Controls.Clear();
            AbrirUserControl(new ucUsuario());
        }

        private void btnAlunos_Click(object sender, EventArgs e)
        {

        }
    }
}
