using AudioStreaming.Domain.Entities;
using AudioStreaming.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStreaming.Application.DTOs.Playlists
{
    public class SharePlaylistPermissionsDto
    {
        public string UserId { get; set; }

        public PermissionType PermissionType { get; set; }
    }
}
