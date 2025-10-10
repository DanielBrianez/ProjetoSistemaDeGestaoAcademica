using System.Data;
using TransformeseApp2.BLL;
using TransformeseApp2.DAL;
using TransformeseApp2.DTO;

namespace TransformeseApp2.Desktop
{
    public partial class ucUnidades : UserControl
    {
        private readonly UnidadeBLL unidadeBLL = new();
        private int? unidadeSelecionadaId = null;
        public ucUnidades()
        {
            InitializeComponent();
        }

        private void ucUnidades_Load(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                var unidade = new UnidadeDTO
                {
                    Id = Database.Unidades.Count + 1,
                    Nome = txtNome.Text,
                    Endereco = txtEndereco.Text
                };

                unidadeBLL.CadastrarUnidade(unidade);
                MessageBox.Show($"Unidade {txtNome.Text} cadastrada com sucesso", "Cadastro de Unidades", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Clear();
                txtEndereco.Clear();
                AtualizarGrid();
            }
            catch (Exception erro)
            {
                MessageBox.Show($"Erro: {erro.Message}");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgUnidades.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione ao menos uma unidade para exclusão!");
                return;
            }

            List<int> ids = new List<int>();
            List<string> nomesDasUnidades = new List<string>();

            foreach (DataGridViewRow row in dgUnidades.SelectedRows)
            {
                if (row.Cells["Id"].Value != null)
                    ids.Add(Convert.ToInt32(row.Cells["Id"].Value));
                if (row.Cells["Nome"].Value != null)
                    nomesDasUnidades.Add(row.Cells["Nome"].Value.ToString());
            }

            string nomesFormatados = string.Join(", ", nomesDasUnidades);
            var confirmacao = MessageBox.Show(
                $"Confirma a exclusão da(s) unidade(s) selecionado(s)?\nUnidade(s) selecionadas: {nomesFormatados}",
                "Confirmação", MessageBoxButtons.YesNo);

            if (confirmacao == DialogResult.Yes)
            {
                foreach (int id in ids)
                {
                    unidadeBLL.RemoverUnidade(id);
                }
                MessageBox.Show($"Unidade(s) {nomesFormatados} removida(s) com sucesso!", "Exclusão de Unidades", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AtualizarGrid();
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (unidadeSelecionadaId != null)
            {
                btnAtualizar.Enabled = true;
                try
                {
                    var unidadeAtualizada = new UnidadeDTO
                    {
                        Id = unidadeSelecionadaId.Value,
                        Nome = txtNome.Text,
                        Endereco = txtEndereco.Text
                    };
                    unidadeBLL.AtualizarUnidade(unidadeAtualizada);
                    MessageBox.Show($"Curso {unidadeAtualizada.Nome} atualizado com sucesso!");
                    txtNome.Clear();
                    unidadeSelecionadaId = null;
                    AtualizarGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro: {ex.Message}");
                    throw;
                }
            }
        }
        private void AtualizarGrid()
        {
            var lista = unidadeBLL.ListarUnidades()
                                .Select(unidade => new
                                {
                                    unidade.Id,
                                    unidade.Nome,
                                    unidade.Endereco
                                }).ToList();

            dgUnidades.DataSource = lista;

        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            BuscarUnidade();
        }
        private void BuscarUnidade()
        {
            string termo = txtBusca.Text.Trim().ToLower();

            var filtrados = unidadeBLL.ListarUnidades()
                                         .Where(unidade => unidade.Nome.ToLower().Contains(termo))
                                         .Select(unidade => new
                                         {
                                             unidade.Id,
                                             unidade.Nome,
                                             unidade.Endereco
                                         }).ToList();
            dgUnidades.DataSource = filtrados;
        }

        private void dgUnidades_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgUnidades.Rows[e.RowIndex];

                unidadeSelecionadaId = Convert.ToInt32(row.Cells["Id"].Value);
                txtNome.Text = row.Cells["Nome"].Value.ToString();

                string nomeUnidade = row.Cells["Nome"].Value.ToString();

                txtNome.Text = Database.Unidades.First(c => c.Nome == nomeUnidade).Nome;
                txtEndereco.Text = Database.Unidades.First(u => u.Nome == nomeUnidade).Endereco.ToString();

                btnAtualizar.Enabled = true;
            }
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            BuscarUnidade();
        }
    }
}
