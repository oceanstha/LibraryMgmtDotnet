namespace LibraryMgmt.ViewModel
{
    public class BookIssueListViewModel
    {
        public Guid Guid { get; set; }
        public string BookTitle { get; set; }
        public string UserName {  get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

    }
}
