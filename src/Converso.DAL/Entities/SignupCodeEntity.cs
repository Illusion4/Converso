using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnapTalk.Domain.Entities;

public class SignupCodeEntity
{
    public string Email { get; set; }
    
    public string Code { get; set; }
}