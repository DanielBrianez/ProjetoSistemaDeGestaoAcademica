using System.Runtime.CompilerServices;
using TransformeseApp2.DTO;

namespace TransformeseApp2.DAL
{
    //Simulação do banco de dados em memória
    public static class Database
    {
        public static List<AlunoDTO> Alunos
        {
            get => JsonDatabase.Ler<AlunoDTO>("Alunos.json");
            set => JsonDatabase.Salvar("Alunos.json", value);
        }

        /*new AlunoDTO{Id = 1, Nome = "Daniel", CursoId = 1, UnidadeId = 1},
        new AlunoDTO{Id = 2, Nome = "Gabriel", CursoId = 2, UnidadeId = 2},
        new AlunoDTO{Id = 3, Nome = "Paulo", CursoId = 1, UnidadeId = 1},
        new AlunoDTO{Id = 4, Nome = "Marcos", CursoId = 2, UnidadeId = 2},
        new AlunoDTO{Id = 5, Nome = "Felipe", CursoId = 1, UnidadeId = 1 }*/

        public static List<CursoDTO> Cursos
        {
            get => JsonDatabase.Ler<CursoDTO>("Cursos.json");
            set => JsonDatabase.Salvar("Cursos.json", value);
        }

        /* new CursoDTO{Id = 1, Nome = "Programador de Sistemas", CargaHoraria = 200},
        new CursoDTO{Id = 2, Nome = "Banco de dados", CargaHoraria = 180},
        new CursoDTO{Id = 1, Nome = "Qualidade de Software", CargaHoraria = 400}*/

        public static List<UnidadeDTO> Unidades
        {
            get => JsonDatabase.Ler<UnidadeDTO>("Unidades.json");
            set => JsonDatabase.Salvar("Unidades.json", value);
        }
        /* new UnidadeDTO{Id = 1, Nome = "SMP - São Miguel Paulista", Endereco = "Avenida Marechal Tito n°1500" },
        new UnidadeDTO{Id = 2, Nome = "ITQ - Itaquera", Endereco = "Avenida Itaquera n°8266" },
        new UnidadeDTO{Id = 3, Nome = "ACL - Aclimação", Endereco = "Avenida Aclimação n°990" }*/
    
        public static List<UsuarioDTO> Usuarios
        {
            get => JsonDatabase.Ler<UsuarioDTO>("Usuarios.json");
            set => JsonDatabase.Salvar("Usuarios.json", value);
        }

        /* new UsuarioDTO { Id = 1, Nome = "Admin", Login = "admin", Senha = "admin123"},
           new UsuarioDTO { Id = 2, Nome = "Usuario", Login = "user", Senha = "user123" },
           new UsuarioDTO { Id = 3, Nome = "Convidado", Login = "guest", Senha = "guest123"} */
    }
}





