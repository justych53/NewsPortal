public class News
{
    public const int MAX_LENGTH = 250;

    private News(Guid id, string title, Guid categoryId, DateTime createdAt, string shortPhrase, string description)
    {
        Id = id;
        Title = title;
        CategoryId = categoryId;
        CreatedAt = createdAt;
        ShortPhrase = shortPhrase;
        Description = description;
    }

    public Guid Id { get; }
    public string Title { get; } = string.Empty;
    public Guid CategoryId { get; }
    public DateTime CreatedAt { get; }
    public string ShortPhrase { get; } = string.Empty;
    public string Description { get; } = string.Empty;

    public static (News news, string Error) Create(
        Guid id,
        string title,
        Guid categoryId,
        DateTime createdAt,
        string shortPhrase,
        string description)
    {
        var error = string.Empty;
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(shortPhrase) || string.IsNullOrEmpty(description))
        {
            error = "Некоторые строки пустые";
        }
        if (title.Length > MAX_LENGTH || shortPhrase.Length > MAX_LENGTH)
        {
            error = "Название и фраза не должны быть больше 250 символов";
        }

        var news = new News(id, title, categoryId, createdAt, shortPhrase, description);
        return (news, error);
    }
}