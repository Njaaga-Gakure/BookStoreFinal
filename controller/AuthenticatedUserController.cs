using BookStoreRemastered.models;
using BookStoreRemastered.services;


namespace BookStoreRemastered.controller
{
    public class AuthenticatedUserController
    {
        BooksService bookService = new BooksService();
        public async Task Home(User authenticatedUser) {
            int count = 0;
            do {
                if (count == 0) Console.Clear();
                Console.WriteLine($"Welcome, {authenticatedUser.UserName} - ({authenticatedUser.Role})\n");
                Console.WriteLine("List All Books");
                Console.WriteLine("--------------\n");
                await PrintAllBooks();
                Console.WriteLine("\n");
                string prompt = authenticatedUser.Role == "admin" ? "Press `c` if you wish to create a book" : "Press `b` if you want to buy a book"; 
                Console.WriteLine(prompt);
                string? option = Console.ReadLine();
                var validOptions = new List<string>() { "c", "b"};
                bool isValidOption = ValidateOptions(option, validOptions);
                if (!isValidOption) {
                    Console.Clear();
                    Console.WriteLine("-------- Invalid choice. Please Try Again. -------");
                    count++;
                    continue;
                }
                if (option == "b" && authenticatedUser.Role == "admin")
                {
                    Console.Clear();
                    Console.WriteLine("---------- Admin cannot buy a book. Enter a diffent option. ---------\n");
                    count++;
                    continue;
                }
                else if (option == "c" && authenticatedUser.Role == "user") {
                    Console.Clear();
                    Console.WriteLine("---------- User is not authorized to perform this task. Enter a diffent option. --------\n");
                    count++;
                    continue;
                }
                await redirectUser(option);
            } while (true);
        }

        public async Task PrintAllBooks() { 
            var books = await bookService.GetAllBooks();
            foreach (var book in books) {
                Console.WriteLine($"{book.Id}): {book.BookName} -> {book.Description.Substring(0, 30)}...");
            }
        }
        public bool ValidateOptions(string option, List<string> validOptions) { 
            return validOptions.Contains(option);
        }
        public async Task redirectUser(string option) {
            switch (option) {
                case "b":
                    Console.WriteLine("buy");
                    break;
                case "c":
                    Console.WriteLine("create");
                    break;
            }
        }
    }
}
