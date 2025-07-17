using NewsPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.DataAccess.Entities
{
    public class CategoryEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public static CategoryEntity MapCategoryToEntity(Core.Models.Category category)
        {
            if (category == null)
            {
                return null;
            }
            return new CategoryEntity { Id = category.Id, Name = category.Name };
        }
        public static Category FromEntity(CategoryEntity entity)
        {
            return new Category(
                entity.Id,
                entity.Name
            );
        }

    }

}
