using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransformeseApp2.BLL;
using TransformeseApp2.DAL;

namespace TransformeseApp2.Desktop
{
    public partial class ucHome : UserControl
    {
        public ucHome()
        {
            InitializeComponent();
        }

        private void ucHome_Load(object sender, EventArgs e)
        {
            lblBemVindo.Text = $"Bem vindo(a). {AppSession.UsuarioLogado.Nome}!";
            contarAlunos();
            contarCursos();
            contarUnidades();
            circleProgress();
        }
        public void circleProgress()
        {
            timer.Interval = 50;
            timer.Start();

            cpbAlunos.Minimum = 0;
            cpbAlunos.Maximum = 100;
            cpbAlunos.Value = Database.Alunos.Count;

            cpbCursos.Minimum = 0;
            cpbCursos.Maximum = 100;
            cpbCursos.Value = Database.Cursos.Count;

            cpbUnidades.Minimum = 0;
            cpbUnidades.Maximum = 100;
            cpbUnidades.Value = Database.Unidades.Count;
        }
        
        private void contarAlunos()
        {
            lblAlunos.Text = Database.Alunos.Count.ToString();
        }
        private void contarCursos()
        {
            lblCursos.Text = Database.Cursos.Count.ToString();
        }
        private void contarUnidades()
        {
            lblUnidades.Text = Database.Unidades.Count.ToString();
        }
    }
}
