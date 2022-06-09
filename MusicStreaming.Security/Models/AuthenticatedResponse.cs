using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreaming.Security.Models
{
    public class AuthenticatedResponse
    {
        public string? Token { get; set; }

        public string? RefreshToken { get; set; }
    }
}
