using System;
using System.Data.Entity;
using System.Collections.Generic;

public class User
{
	public User()
	{
	}
    public int ID { get; set; } 
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateTime LastLogin { get; set; }
    public virtual ICollection<Search> Searches { get; set; }
}

public class Search
{
    public Search()
    {
    }
    public int ID { get; set; }
    public string Name { get; set; }
    public DateTime Executed { get; set; }

    public List<string> TopTenWords { get; set; }
    public virtual ICollection<SearchResult> Results { get; set; }
}

public class SearchResult
{
    public SearchResult()
    {
    }
    public int ID { get; set; }
    public string WebSite { get; set; }
    public List<string> TopTenWords { get; set; }
}


public class UserDBEntities : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Search> Searches { get; set; }
    public virtual DbSet<SearchResult> SearchResults { get; set; }
}