namespace FIAP.TechChallenge.Domain.Helpers.Constants
{
    public class Values
    {
        public static class Message
        {
            public const string Unauthorized = "Você não tem permissão para executar esta ação";
            public const string DefaultError = "Nossos servidores estão indisponíveis no momento. Por favor, tente mais tarde.";
            public const string InvalidGuid = "O parâmetro informado não é válido, por favor informe um valor de padrão UUID";
            public const string UserRequestNotFound = "O usuário informado na requisição não foi encontrado, realize o login novamente";
            public const string CompanyRequestNotFound = "A corretora informada na requisição não foi encontrado, realize o login novamente";
            public const string ApplicationRequestNotFound = "A aplicação informada na requisição não foi encontrado, realize o login novamente";
        }
    }
}
