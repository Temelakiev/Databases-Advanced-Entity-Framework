namespace BookShopSystem.Migrations
{
    using Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.IO;
    using System.Linq;


    internal sealed class Configuration : DbMigrationsConfiguration<Data.BookShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        protected override void Seed(Data.BookShopContext context)
        {
            SeedAuthors(context);

            SeedBooks(context);

            SeedCategories(context);
            base.Seed(context);
        }
        private static void SeedCategories(Data.BookShopContext context)
        {
            int booksCount = context.Books.Local.Count;


            string[] categories = File.ReadAllLines(@"././BookShopSystem/Import/categories.csv");
            Console.WriteLine();
            for (int i = 1; i < categories.Length; i++)
            {
                string[] data = categories[i]
                  .Split(',')
                  .Select(arg => arg.Replace("\"", string.Empty))
                  .ToArray();

                string categoryName = data[0].ToString();
                Category category = new Category() { Name = categoryName };

                int bookIndex = (i * 30) % booksCount;
                for (int j = 0; j < bookIndex; j++)
                {
                    Book book = context.Books.Local[j];
                    category.Books.Add(book);
                }
                context.Categories.AddOrUpdate(c => c.Name, category);
            }
        }


        private static void SeedBooks(Data.BookShopContext context)
        {
            int authorsCount = context.Authors.Local.Count;
            string[] books = File.ReadAllLines(@"././BookShopSystem/Import/books.csv");

            for (int i = 1; i < books.Length; i++)
            {
                string[] data = books[i]
                    .Split(',')
                    .Select(arg => arg.Replace("\"", string.Empty))
                    .ToArray();

                int authorIndex = i % authorsCount;
                Author author = context.Authors.Local[authorIndex];
                EditionType edition = (EditionType)int.Parse(data[0]);
                DateTime releaseDate = DateTime.ParseExact(data[1], "d/M/yyyy", CultureInfo.InvariantCulture);
                int copies = int.Parse(data[2]);
                decimal price = decimal.Parse(data[3]);
                AgeRestriction ageRestriction = (AgeRestriction)int.Parse(data[4]);
                string title = data[5];
                Book book = new Book
                {
                    Author = author,
                    AuthorId = author.Id,
                    EditionType = edition,
                    ReleaseDate = releaseDate,
                    Copies = copies,
                    Price = price,
                    AgeRestriction = ageRestriction,
                    Title = title
                };

                context.Books.AddOrUpdate(b => new { b.Title, b.AuthorId }, book);
            }
        }

        private void SeedAuthors(Data.BookShopContext context)
        {
            string[] authors = File.ReadAllLines(@"././BookShopSystem/Import/authors.csv");
      
            // Foreach all authors and add them to database.
            // Skip first row because it is only describing column names.
            for (int i = 1; i < authors.Length; i++)
            {
                string[] data = authors[i].Split(',');
                string firstName = data[0].Replace("\"", string.Empty);
                string lastName = data[1].Replace("\"", string.Empty);
                Author author = new Author()
                {
                    FirstName = firstName,
                    LastName = lastName
                };

                // When adding we may want to check if author with same first name and last name is existing.
                context.Authors.AddOrUpdate(a => new { a.FirstName, a.LastName }, author);

            }

        }


    }
}
