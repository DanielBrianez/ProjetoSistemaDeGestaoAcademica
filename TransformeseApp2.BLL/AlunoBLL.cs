using TransformeseApp2.DAL;
using TransformeseApp2.DTO;

namespace TransformeseApp2.BLL
{
    public class AlunoBLL
    {
        private List<AlunoDTO> _alunos = Database.Alunos;
        public void CadastrarAluno(AlunoDTO aluno)
        {
            var alunos = Database.Alunos;
            //Validação antes de salvar o aluno
            if (string.IsNullOrWhiteSpace(aluno.Nome))
                throw new Exception("Nome do aluno é obrigatório.");
            Database.Alunos.Add(aluno);

            alunos.Add(aluno);
            Database.Alunos = alunos;
        }
        public List<AlunoDTO> ListarAlunos() => Database.Alunos;

        public void AtualizarAluno(AlunoDTO aluno)
        {
            var alunoExistente = Database.Alunos.FirstOrDefault(a => a.Id == aluno.Id);
            if (alunoExistente == null)
                throw new Exception("Aluno não encontrado.");

            if (string.IsNullOrWhiteSpace(aluno.Nome))
                throw new Exception("Nome é obrigatório.");
            

            // Atualiza os campos do aluno
            alunoExistente.Nome = aluno.Nome;
            alunoExistente.CursoId = aluno.CursoId;
            alunoExistente.UnidadeId = aluno.UnidadeId;
        }

        public void RemoverAluno(int id)
        {
            var aluno = Database.Alunos.FirstOrDefault(aluno => aluno.Id == id);
            if (aluno == null)
            {
                throw new Exception("Aluno não encontrado.");
            }

            Database.Alunos.Remove(aluno);
        }
    }
}
