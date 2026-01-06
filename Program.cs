using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("LibraryDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Swagger Stuff
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "LibraryAPI";
    config.Title = "LibraryAPI";
    config.Version = "v1";
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//More Swagger Stuff
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "LibraryAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}
//Route Groups
RouteGroupBuilder books = app.MapGroup("/books");
RouteGroupBuilder authors = app.MapGroup("/authors");

//Endpoints
books.MapGet("/", GetAllBooks);
books.MapGet("/{id}", GetBook);
books.MapPost("/", CreateBook);
books.MapPut("/{id}", UpdateBook);
books.MapDelete("/{id}", DeleteBook);
authors.MapGet("/", GetAllAuthors);
authors.MapGet("/{id}", GetAuthors);
authors.MapPost("/", CreateAuthor);
authors.MapPut("/{id}", UpdateAuthors);
authors.MapDelete("/{id}", DeleteAuthors);

app.Run();
// Endpoint handlers
static async Task<IResult> GetAllBooks(ApplicationDbContext db)
{
    var books = await db.Book.Include(b => b.author).ToListAsync();
    return Results.Ok(books);
}

static async Task<IResult> GetBook(string id, ApplicationDbContext db)
{
    if (int.TryParse(id, out int intId) && intId > int.MaxValue) return Results.NotFound();
    var book = await db.Book.Include(b => b.author).FirstOrDefaultAsync(b => b.id == intId);
    return book is null ? Results.NotFound() : Results.Ok(book);
}

static async Task<IResult> CreateBook(BookDTO bookDTO, ApplicationDbContext db)
{
    if (bookDTO.title is "" or null || bookDTO.year < 0 || bookDTO.authorId < 0) return Results.BadRequest();
    var authorSearch = await db.Author.FindAsync(bookDTO.authorId);
    if (authorSearch is null) return Results.BadRequest("Author not found");

    var book = new Book
    {
        title = bookDTO.title,
        year = bookDTO.year,
        authorId = bookDTO.authorId,
        author = authorSearch
    };

    db.Book.Add(book);
    await db.SaveChangesAsync();

    return Results.Created($"/books/{book.id}", book);
}

static async Task<IResult> UpdateBook(string id, UpdateBookDTO updatedBookDTO, ApplicationDbContext db)
{
    if (int.TryParse(id, out int intId) && intId > int.MaxValue) return Results.NotFound();
    if (updatedBookDTO.title is null || updatedBookDTO.title == "" || 
    updatedBookDTO.year is null || updatedBookDTO.year < 0 || 
    updatedBookDTO.authorId is null || updatedBookDTO.authorId < 0) 
    { 
        return Results.BadRequest("Invalid data"); 
    }
    var existing = await db.Book.FindAsync(intId);
    var authorSearch = await db.Author.FindAsync(updatedBookDTO.authorId);
    if (authorSearch is null) return Results.BadRequest("Author not found");
    if (existing is null) return Results.NotFound();

    existing.title = updatedBookDTO.title;
    existing.year = updatedBookDTO.year.Value;
    existing.authorId = updatedBookDTO.authorId.Value;

    await db.SaveChangesAsync();
    return Results.NoContent();
}



static async Task<IResult> DeleteBook(string id, ApplicationDbContext db)
{
    if (int.TryParse(id, out int intId) && intId > int.MaxValue) return Results.NotFound();
    var existing = await db.Book.FindAsync(intId);
    if (existing is null) return Results.NotFound();

    db.Book.Remove(existing);
    await db.SaveChangesAsync();
    return Results.NoContent();
}

static async Task<IResult> GetAllAuthors(ApplicationDbContext db)
{
    var authors = await db.Author
        .ToListAsync();
    return Results.Ok(authors);
}

static async Task<IResult> GetAuthors(int id, ApplicationDbContext db)
{
    var author = await db.Author
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.id == id);

    return author is null ? Results.NotFound() : Results.Ok(author);
}

static async Task<IResult> CreateAuthor(AuthorDTO authorDTO, ApplicationDbContext db)
{
    if (authorDTO.first_name is "" or null || authorDTO.last_name is "" or null) return Results.BadRequest();

    var author = new Author
    {
        first_name = authorDTO.first_name,
        last_name = authorDTO.last_name,
    };

    db.Author.Add(author);
    await db.SaveChangesAsync();

        return TypedResults.Created($"/authors/{author.id}", author);
}

static async Task<IResult> UpdateAuthors(int id, AuthorDTO updatedAuthorDTO, ApplicationDbContext db)
{
    if (updatedAuthorDTO.first_name is "" or null || updatedAuthorDTO.last_name is "" or null) return Results.BadRequest();

    var existing = await db.Author.FindAsync(id);
    if (existing is null) return Results.NotFound();

    existing.first_name = updatedAuthorDTO.first_name;
    existing.last_name = updatedAuthorDTO.last_name;

    await db.SaveChangesAsync();
    return Results.NoContent();
}


static async Task<IResult> DeleteAuthors(int id, ApplicationDbContext db)
{
    var existing = await db.Author.FindAsync(id);
    if (existing is null) return Results.NotFound();

    db.Author.Remove(existing);
    await db.SaveChangesAsync();
    return Results.NoContent();
}
