using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

//https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite.sqliteparametercollection.contains?view=msdata-sqlite-7.0.0
//https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/user-defined-functions
//https://learn.microsoft.com/en-us/answers/questions/1160856/createfunction-like
Console.WriteLine("----------------------------");

using (BloggingContext context = new BloggingContext())
{
    if (!context.Posts.Any())
    {
        context.Posts.Add(new Post { Title = "j" });
        context.Posts.Add(new Post { Title = "J" });
        context.Posts.Add(new Post { Title = "K" });
        context.Posts.Add(new Post { Title = "k" });
        context.Posts.Add(new Post { Title = "A" });
        context.Posts.Add(new Post { Title = "b" });
        context.Posts.Add(new Post { Title = "C" });
        context.Posts.Add(new Post { Title = "c" });
        context.Posts.Add(new Post { Title = "Ç" });
        context.Posts.Add(new Post { Title = "Ç" });
        context.Posts.Add(new Post { Title = "D" });
        context.Posts.Add(new Post { Title = "d" });
        context.Posts.Add(new Post { Title = "E" });
        context.Posts.Add(new Post { Title = "e" });
        context.Posts.Add(new Post { Title = "F" });
        context.Posts.Add(new Post { Title = "f" });
        context.Posts.Add(new Post { Title = "G" });
        context.Posts.Add(new Post { Title = "g" });
        context.Posts.Add(new Post { Title = "Ğ" });
        context.Posts.Add(new Post { Title = "ğ" });
        context.Posts.Add(new Post { Title = "H" });
        context.Posts.Add(new Post { Title = "h" });
        context.Posts.Add(new Post { Title = "I" });
        context.Posts.Add(new Post { Title = "ı" });
        context.Posts.Add(new Post { Title = "İ" });
        context.Posts.Add(new Post { Title = "i" });
 

        context.Posts.Add(new Post { Title = "İlk İçerik" });
        context.Posts.Add(new Post { Title = "ilk olmayan" });
    }      
    context.SaveChanges();
    string value = "ilk";

    foreach (var item in context.Posts.OrderBy(i => i.Title))
    {
        Console.WriteLine(item.Title);
    }
    Console.WriteLine("-----------search-----------");
    foreach (var item in context.Posts.Where(i => i.Title.ToUpper().Contains(value.ToUpper())))
    {
        Console.WriteLine(item.Title);
    }
    Console.WriteLine("----------------------------");

    Console.ReadLine();
}

public class Post 
{
    public int PostId { get; set; }
 
    public string Title { get; set; }
}

public class BloggingContext : DbContext
{
    SqliteConnection _connection = new SqliteConnection("Data Source=BlogDB.db");
    public DbSet<Post> Posts { get; set; }
    public BloggingContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
       // _connection = new SqliteConnection("Data Source=BlogDB.db");

        _connection = (SqliteConnection)Database.GetDbConnection();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().Property(t => t.Title).HasColumnType("TEXT COLLATE NOCASE");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _connection.CreateCollation("NOCASE", (x, y) => string.Compare(x, y, ignoreCase: true));
        _connection.CreateFunction("upper", (string x) => x.ToUpper(), isDeterministic: true);
        _connection.CreateFunction("lower", (string x) => x.ToLower(), isDeterministic: true);

        //_connection.CreateFunction("like", 

        optionsBuilder.UseSqlite(_connection);
    }
 
     
}

 