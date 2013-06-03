namespace MetroLepra.Model
{
    public class UserModel
    {
        public string Id { get; set; }
        public UserGender Gender { get; set; }
        public string CustomRank { get; set; }
        public string Username { get; set; }
        public string Userpic { get; set; }
        public string Number { get; set; }
        public string RegistrationDate { get; set; }
        public string FullName { get; set; }
        public string Location { get; set; }
        public string Karma { get; set; }
        public string UserStat { get; set; }
        public string VoteStat { get; set; }
        public string[] Contacts { get; set; }
        public string Description { get; set; }
    }

    public enum UserGender
    {
        Male,
        Female
    }
}