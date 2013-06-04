namespace MetroLepra.Model
{
    public class CommentModel
    {
        public string Id { get; set; }

        public bool IsNew { get; set; }

        public int Indent { get; set; }

        public string Text { get; set; }

        public string Rating { get; set; }

        public UserModel Author { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public int Vote { get; set; }
    }
}