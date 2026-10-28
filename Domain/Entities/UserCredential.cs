using System.Text.RegularExpressions;
using Domain.Validation;

namespace Domain.Entities
{
    public class UserCredential
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public string Username { get; private set; }

        public string PasswordHash { get; private set; }

        public User User { get; private set; }

        protected UserCredential()
        {
        }

        public UserCredential(
        Guid userId,
        string username,
        string passwordHash)
        {
            Validate(userId, username, passwordHash);

            Id = Guid.NewGuid();
            UserId = userId;
            Username = username.Trim().ToLower();
            PasswordHash = passwordHash;
        }

        public void ChangePassword(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainExceptionValidation("Hash da senha é obrigatório.");

            PasswordHash = passwordHash;
        }

        private static void Validate(
        Guid userId,
        string username,
        string passwordHash)
        {
            if (userId == Guid.Empty)
                throw new DomainExceptionValidation("Usuário inválido.");

            if (string.IsNullOrWhiteSpace(username))
                throw new DomainExceptionValidation("Username é obrigatório.");

            if (username.Length < 4)
                throw new DomainExceptionValidation("Username deve possuir pelo menos 4 caracteres.");

            if (username.Length > 50)
                throw new DomainExceptionValidation("Username deve possuir no máximo 50 caracteres.");

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9._]+$"))
                throw new DomainExceptionValidation(
                "Username pode conter apenas letras, números, ponto e underscore.");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainExceptionValidation("Senha inválida.");
        }
    }
}
