namespace MiApi.Domain.Entities
{
  public class User(string id, string name, string email)
  {
    public string Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
  }
}