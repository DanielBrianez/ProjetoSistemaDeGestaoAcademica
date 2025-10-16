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
            lblUsuario.Text = AppSession.UsuarioLogado.Nome ?? "Usuário";
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
            try
            {
                var confirmacao = mdSair.Show("Deseja realmente sair?");

                if (confirmacao == DialogResult.Yes)
                {
                    frmLogin principal = new frmLogin();
                    principal.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                mdNotifica.Show($"Erro no sistema: {ex.Message}");
            }
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
            lblUsuario.Text = AppSession.UsuarioLogado.Nome ?? "Usuário";

            if (!string.IsNullOrEmpty(AppSession.UsuarioLogado.UrlFoto) && File.Exists((AppSession.UsuarioLogado.UrlFoto)))
            {
                pbFoto.Image = Image.FromFile(AppSession.UsuarioLogado.UrlFoto);
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
            panelConteudo.Controls.Clear();
            AbrirUserControl(new ucAlunos());
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmacao = mdSair.Show("Deseja realmente sair?");

                if (confirmacao == DialogResult.Yes)
                {
                    frmLogin principal = new frmLogin();
                    principal.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                mdNotifica.Show($"Erro no sistema: {ex.Message}");
            }

        }

        private void btnCursos_Click(object sender, EventArgs e)
        {
            panelConteudo.Controls.Clear();
            AbrirUserControl(new ucCursos());
        }

        private void btnUnidades_Click(object sender, EventArgs e)
        {
            panelConteudo.Controls.Clear();
            AbrirUserControl(new ucUnidades());
        }

        private void pbColorMode_Click(object sender, EventArgs e)
        {
            bool isDarkMode = this.BackColor == Color.FromArgb(32, 32, 32);

            if (isDarkMode)
            {
                // 🌞 MODO CLARO
                Color lightFormColor = SystemColors.Control;
                Color lightPanelColor = Color.WhiteSmoke;
                Color lightTextColor = Color.Black;

                // Cores básicas
                this.BackColor = lightFormColor;
                this.ForeColor = lightTextColor;
                panelConteudo.BackColor = lightPanelColor;

                // Ajusta todos os controles
                AplicarTema(this.Controls, lightPanelColor, lightTextColor, false);

                pbColorMode.Image = Properties.Resources.darkmode; // ícone escuro
            }
            else
            {
                // 🌚 MODO ESCURO
                Color darkFormColor = Color.FromArgb(32, 32, 32);
                Color darkPanelColor = Color.FromArgb(45, 45, 45);
                Color darkTextColor = Color.WhiteSmoke;

                // Cores básicas
                this.BackColor = darkFormColor;
                this.ForeColor = darkTextColor;
                panelConteudo.BackColor = darkPanelColor;

                // Ajusta todos os controles
                AplicarTema(this.Controls, darkPanelColor, darkTextColor, true);

                pbColorMode.Image = Properties.Resources.lightmode; // ícone claro
            }
        }

        // 🔧 Função auxiliar para aplicar o tema a todos os controles recursivamente
        private void AplicarTema(Control.ControlCollection controls, Color backColor, Color foreColor, bool isDark)
        {
            foreach (Control ctrl in controls)
            {
                switch (ctrl)
                {
                    case Panel pnl:
                        pnl.BackColor = backColor;
                        AplicarTema(pnl.Controls, backColor, foreColor, isDark);
                        break;

                    case Label lbl:
                        lbl.ForeColor = foreColor;
                        break;

                    case Button btn:
                        btn.BackColor = isDark ? Color.FromArgb(60, 60, 60) : Color.WhiteSmoke;
                        btn.ForeColor = foreColor;
                        break;

                    case PictureBox pb:
                        pb.BackColor = Color.Transparent; // evita blocos coloridos indesejados
                        break;

                    default:
                        ctrl.ForeColor = foreColor;
                        break;
                }
            }
        }

    }
}

