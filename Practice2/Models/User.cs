namespace Practice2.Models
{
    public enum Role
    {
        Guest = 0,
        Operator = 1,
        Admin = 2,
    }

    public record User(long Id, string Name, Role Role);
}