using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Core.Models
{
    public class Category
    {
        public const int MAX_LENGTH = 100;

        public Category(Guid id, string name)
        {
            Id = id; Name = name;
        }
        public Guid Id { get; }
        public string Name { get; } = string.Empty;

        public static (Category category, string Error) Create(Guid id, string name)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(name) || name.Length > MAX_LENGTH)
            {
                error = "Название категории не может быть пустым или превышать 100 символов";
            }
            var category = new Category(id, name);
            return (category, error);
        }
    }
}
