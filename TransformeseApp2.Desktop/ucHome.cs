using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            lblBemVindo.Text = $"Bem vindo(a). {Session.UsuarioLogado.Nome}!";
            contarAlunos();
            contarCursos();
            contarUnidades();
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
