using System;

namespace AlbaDL
{
    public class Note
    {
        public string Subject { get; set; }

        public DateTime Created { get; set; }

        public string Category { get; set; }

        public Guid Id { get; set; }

        public Note(string subject, string category)
        {
            Subject = subject;
            Created = DateTime.Now;
            Category = category;
            Id = Guid.NewGuid();
        }
    }
}