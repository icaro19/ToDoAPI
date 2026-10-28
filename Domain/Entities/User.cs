using System.Text.RegularExpressions;
using Domain.Validation;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public ICollection<TodoTask> Tasks { get; private set; }

        protected User()
        {
            Tasks = new List<TodoTask>();
        }

        public User(string name, string email)
        {
            Validate(name, email);

            Id = Guid.NewGuid();
            Name = name.Trim();
            Email = email.Trim().ToLower();
            Tasks = new List<TodoTask>();
        }

        public void Update(string name, string email)
        {
            Validate(name, email);

            Name = name.Trim();
            Email = email.Trim().ToLower();
        }

        private static void Validate(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainExceptionValidation("Nome é obrigatório.");

            if (name.Length < 3)
                throw new DomainExceptionValidation("Nome deve possuir pelo menos 3 caracteres.");

            if (name.Length > 150)
                throw new DomainExceptionValidation("Nome deve possuir no máximo 150 caracteres.");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainExceptionValidation("E-mail é obrigatório.");

            if (email.Length > 150)
                throw new DomainExceptionValidation("E-mail deve possuir no máximo 150 caracteres.");

            var emailRegex =
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(email, emailRegex))
                throw new DomainExceptionValidation("E-mail inválido.");
        }
    }
}
