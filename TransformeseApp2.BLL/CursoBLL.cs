using TransformeseApp2.DAL;
using TransformeseApp2.DTO;

namespace TransformeseApp2.BLL
{
    public class CursoBLL
    {
        private List<CursoDTO> _cursos = Database.Cursos;
        public void CadastrarCurso(CursoDTO curso)
        {
            var cursos = Database.Cursos;
            //Validação antes de salvar o curso
            if (string.IsNullOrWhiteSpace(curso.Nome))
                throw new Exception("Nome do curso é obrigatório.");
            if (int.IsNegative(curso.CargaHoraria))
                throw new Exception("Carga horaria precisa ser positiva, tente novamente, por favor!");
            Database.Cursos.Add(curso);

            cursos.Add(curso);
            Database.Cursos = cursos;
        }
        public List<CursoDTO> ListarCursos() => Database.Cursos;

        public void AtualizarCurso(CursoDTO curso)
        {
            var cursoExistente = Database.Cursos.FirstOrDefault(curso => curso.Id == curso.Id);
            if (cursoExistente == null)
                throw new Exception("Curso não encontrado.");

            if (string.IsNullOrWhiteSpace(curso.Nome))
                throw new Exception("Nome é obrigatório.");


            // Atualiza os campos do aluno
            cursoExistente.Nome = curso.Nome;
            cursoExistente.Id = curso.Id;
            cursoExistente.CargaHoraria = curso.CargaHoraria;
        }

        public void RemoverCurso(int id)
        {
            var curso = Database.Cursos.FirstOrDefault(curso => curso.Id == id);
            if (curso == null)
            {
                throw new Exception("Curso não encontrado.");
            }

            Database.Cursos.Remove(curso);
        }
        public CursoDTO GetById(int id)
        {
            return _cursos.FirstOrDefault(curso => curso.Id == id);
        }
    }
}
