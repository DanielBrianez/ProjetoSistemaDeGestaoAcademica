using TransformeseApp2.DAL;
using TransformeseApp2.DTO;

namespace TransformeseApp2.BLL
{
    public class UnidadeBLL
    {
        private List<UnidadeDTO> _unidades = Database.Unidades;
        public void CadastrarUnidade(UnidadeDTO unidadeDTO)
        {
            //Validação antes de salvar a unidade
            if (string.IsNullOrWhiteSpace(unidadeDTO.Nome))
                throw new Exception("Nome da unidade é obrigatório.");
                Database.Unidades.Add(unidadeDTO);
        }
        public List<UnidadeDTO> ListarUnidades() => Database.Unidades;

        public UnidadeDTO GetById(int id)
        {
            return _unidades.FirstOrDefault(unidade => unidade.Id == id);
        }
        public void RemoverUnidade(int id)
        {
            var unidade = Database.Unidades.FirstOrDefault(unidade => unidade.Id == id);
            if (unidade == null)
            {
                throw new Exception("Unidade não encontrada.");
            }

            Database.Unidades.Remove(unidade);
        }

        public void AtualizarUnidade(UnidadeDTO unidadeDTO)
        {
            var unidadeExistente = Database.Unidades.FirstOrDefault(unidade => unidade.Id == unidadeDTO.Id);
            if (unidadeExistente == null)
                throw new Exception("Unidade não encontrada.");

            if (string.IsNullOrWhiteSpace(unidadeDTO.Nome))
                throw new Exception("Nome é obrigatório.");

            unidadeExistente.Nome = unidadeDTO.Nome;
            unidadeExistente.Id = unidadeDTO.Id;
            unidadeExistente.Endereco = unidadeDTO.Endereco;
        }
    }
}
