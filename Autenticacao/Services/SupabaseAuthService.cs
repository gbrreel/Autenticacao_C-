using Supabase;
using Supabase.Gotrue;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Autenticacao.Services
{
    public class SupabaseAuthService
    {
        private readonly Supabase.Client _supabase;

        public SupabaseAuthService(IConfiguration config)
        {
            var supabaseUrl = config["Supabase:Url"];
            var supabaseKey = config["Supabase:AnonKey"];

            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
            {
                throw new Exception("ERRO: Supabase URL ou AnonKey estão ausentes no appsettings.json!");
            }

            var options = new SupabaseOptions
            {
                AutoConnectRealtime = false
            };

            _supabase = new Supabase.Client(supabaseUrl, supabaseKey, options);
        }
        public async Task<string> RegisterAsync(string email, string password)
        {
            try
            {
                Console.WriteLine($"🔹 Tentando registrar usuário: {email}");

                // Faz a chamada ao Supabase
                var response = await _supabase.Auth.SignUp(email, password, new SignUpOptions());

                // Verifica se a resposta do Supabase está correta
                if (response?.User?.Id == null)
                {
                    Console.WriteLine("⚠️ ERRO AO REGISTRAR USUÁRIO!");
                    Console.WriteLine($"🔹 Resposta JSON do Supabase: {System.Text.Json.JsonSerializer.Serialize(response)}");
                    return null;
                }

                Console.WriteLine($"✅ Usuário registrado com sucesso! ID: {response.User.Id}");
                return response.User.Id;
            }
            catch (Supabase.Gotrue.Exceptions.GotrueException gotrueEx)
            {
                Console.WriteLine("❌ ERRO NO SUPABASE:");
                Console.WriteLine($"🔴 Código: {gotrueEx.GetType().Name}");
                Console.WriteLine($"🔴 Mensagem: {gotrueEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERRO GERAL NO REGISTRO:");
                Console.WriteLine($"🔴 Mensagem: {ex.Message}");
                Console.WriteLine($"🔴 StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                Console.WriteLine($"Tentando login para: {email}");

                var response = await _supabase.Auth.SignIn(email, password);

                if (response == null || string.IsNullOrEmpty(response.AccessToken))
                {
                    Console.WriteLine("Erro no login: Credenciais inválidas ou resposta nula.");
                    return null;
                }

                Console.WriteLine($"Login bem-sucedido! Token JWT gerado.");
                return response.AccessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao realizar login no Supabase: {ex.Message}");
                return null;
            }
        }
    }
}
