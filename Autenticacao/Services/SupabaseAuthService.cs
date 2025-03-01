using Supabase;
using System.Threading.Tasks;


namespace Autenticacao.Services
{
    public class SupabaseAuthService
    {
        private readonly Client _supabase;
    
        //Construtor para se conectar ao banco
        public SupabaseAuthService(IConfiguration config)
        {
            _supabase = new Client(config["Supabse:Url"], config["Supabase:AnonKey"]);
        }

        //Chama o supabase e cria um user, se tudo der certo o supabase manda um cracha digital (Token)
        public async Task<string> RegisterAsync(string email, string password)
        {
            var response = await _supabase.Auth.SignUp(email, password);
            return response?.AccessToken;
        }
    
        public async Task<string> LoginAsync(string email, string password)
        {
            var response = await _supabase.Auth.SignIn(email, password);
            return response.AccessToken;
        }
    }
    
}
