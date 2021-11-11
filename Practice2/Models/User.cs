namespace Practice2.Models
{
    /// <summary>
    /// Application role
    /// </summary>
    public enum Role
    {
        Guest = 0,
        Operator = 1,
        Admin = 2,
    }

    /// <summary>
    /// User Model
    /// </summary>
    public record User
    {
        /// <summary>
        /// User Model
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="name">User name</param>
        /// <param name="role">User role</param>
        public User(long id, string name, Role role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        /// <summary>User id</summary>
        public long Id { get; init; }

        /// <summary>User name</summary>
        public string Name { get; init; }

        /// <summary>User role</summary>
        public Role Role { get; init; }
    }
}