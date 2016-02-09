namespace lib.repos
{
    internal static class SQL
    {
        public const string Read_User_By_UserName = "SELECT u.id, u.username, u.password FROM user u where u.username = @UserName";
        public const string Read_All_Users = "SELECT u.id, u.username, u.password FROM user u";
        public const string Read_Claims_For_User = "SELECT claim FROM userclaim WHERE username = @UserName";
        public const string Create_User = "INSERT INTO user(username, password) VALUES (@UserName, @Password);";
        public const string Create_Claim = "INSERT INTO userclaim(username, claim) VALUES (@UserName, @Claim);";
        public const string Delete_User = "DELETE FROM user WHERE username = @UserName";
        public const string Delete_Claim = "DELETE FROM userclaim WHERE userName = @UserName";
        public const string Update_User = "UPDATE user SET username = @UserName, password = @Password WHERE id = @id;";
        public const string Delete_All_User_Claims = "DELETE FROM userclaim where username = @UserName";
    }
}
