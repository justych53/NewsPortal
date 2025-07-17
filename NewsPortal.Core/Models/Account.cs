namespace NewsPortal.Models
{
    public class Account
    {
        public const int MAX_LENGTH = 100;
        public Account(Guid id, string userName, string firstName, string passwordHash)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            PasswordHash = passwordHash;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string PasswordHash { get; set; }

        public static (Account Account, string Error) Create(Guid id, string userName, string firstName, string passwordHash)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passwordHash)) {
                error = "Имеются пустые поля";
            }
            if (userName.Length > MAX_LENGTH || firstName.Length > MAX_LENGTH || passwordHash.Length > MAX_LENGTH) {
                error = "Ограничение 250 символов";
            }
            var account = new Account(id, userName, firstName, passwordHash);
            return (account, error);
        }
    }
}
