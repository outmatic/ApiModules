using System.Collections.Concurrent;

namespace TestApi.Data;

public class BookList
{
    private static readonly List<BookEntity> Books =
    [
        new BookEntity(Guid.NewGuid(), "The Great Gatsby", "F. Scott Fitzgerald"),
        new BookEntity(Guid.NewGuid(), "Ulysses", "James Joyce"),
        new BookEntity(Guid.NewGuid(), "In Search Of Lost Time", "Marcel Proust"),
    ];

    public Guid Add(string title, string author)
    {
        var id = Guid.NewGuid();
        Books.Add(new BookEntity(id, title, author));

        return id;
    }

    public List<BookEntity> List() => Books;

    public bool Delete(Guid id) => Books.RemoveAll(x => x.Id == id) > 0;
}