using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace SnapTalk.Domain.Entities;

public class SessionEntity
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid RefreshToken { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
    public DateTime CreatedAt { get; } = DateTime.Now;
    
    public bool IsExpired => DateTime.Now >= ExpiresAt;
    
    public UserEntity User { get; set; }
}