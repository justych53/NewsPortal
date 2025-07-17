using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Core.Models
{
    public class Image
    {

        private Image(Guid id, string url)
        {
            Id = id; Url = url ;
        }
        public Guid Id { get; }
        public string Url { get; } = string.Empty;

        public static Image Create(Guid id, string url)
        {
            var image = new Image(id, url);
            return (image);
        }
    }
}
