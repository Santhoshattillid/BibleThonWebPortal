using System;

public class Note
{
	private string _subject;

	public string Subject
	{
		get { return _subject; }
		set { _subject = value; }
	}

	private DateTime _created;

	public DateTime Created
	{
		get { return _created; }
		set { _created = value; }
	}

	private string _category;
	public string Category
	{
		get { return _category; }
		set { _category = value; }
	}

	private Guid _id;
	public Guid Id
	{
		get { return _id; }
	}

	public Note(string subject, string category)
	{
		_subject = subject;
		_created = DateTime.Now;
		_category = category;
		_id = Guid.NewGuid();
	}
}
