using TransformeseApp2.DTO;

namespace TransformeseApp2.BLL
{
    public static class AppSession
    {
        // ✅ A sessão deve ser estática e centralizada
        public static UsuarioDTO UsuarioLogado { get; set; }


        // 🔔 Evento global: notifica quando o usuário logado for alterado
        public static event Action<UsuarioDTO> OnUsuarioAtualizado;

        // ✅ Define o usuário inicial (ex: no login)
        public static void DefinirUsuario(UsuarioDTO usuario)
        {
            UsuarioLogado = usuario;
        }

        // ✅ Atualiza o usuário e dispara o evento global
        public static void AtualizarUsuarioLogado(UsuarioDTO novoUsuario)
        {
            UsuarioLogado = novoUsuario;
            OnUsuarioAtualizado?.Invoke(novoUsuario); // Notifica formulários/UCs ativos
        }

        // ✅ Método auxiliar (caso precise limpar a sessão ao deslogar
    }
}

