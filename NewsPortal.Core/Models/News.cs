using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Core.Models
{
    public class News
    {
        public const int MAX_LENGTH = 250;
        private News(Guid id, string title, Category category, Guid categoryId, DateTime createdAt, string shortPhrase, string description)
        {
            Id=id; 
            Title=title; 
            Category=category;
            CategoryId=categoryId;
            CreatedAt=createdAt;
            ShortPhrase=shortPhrase; 
            Description=description;
        }
        
        public Guid Id { get; }
        public string Title { get; } = string.Empty;
        public Category Category { get; }
        public Guid CategoryId { get; }
        public DateTime CreatedAt { get; }
        public string ShortPhrase { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public static (News news, string Error) Create(Guid id,string title, Category category, Guid categoryId ,DateTime createdAt, string shortPhrase, string description)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(shortPhrase) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(category.Name))
            {
                error = "Некоторые строки пустые";
            }
            if (title.Length > MAX_LENGTH || shortPhrase.Length>MAX_LENGTH) 
            {
                error = "Название и фраза не должны быть больше 250 символов";
            }
            var news = new News(id, title, category,categoryId ,createdAt, shortPhrase, description);
            return (news, error);
        }
    }
}
