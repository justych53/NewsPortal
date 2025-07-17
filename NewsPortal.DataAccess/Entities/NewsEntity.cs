using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsPortal.Core.Models;

namespace NewsPortal.DataAccess.Entities
{
    public class NewsEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public CategoryEntity Category { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ShortPhrase { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }
}
