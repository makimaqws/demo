using System;

namespace AuthApp.Models
{
  public class User
  {
    public int UserID { get; set; }
    public string Login { get; set; }
    public char Role { get; set; }
    public bool IsBlocked { get; set; }
    public int FailedAttempts { get; set; }
    public DateTime? LastLogin { get; set; }
  }
}