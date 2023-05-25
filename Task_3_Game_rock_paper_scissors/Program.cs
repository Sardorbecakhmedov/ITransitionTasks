namespace Task_3_Game_rock_paper_scissors;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();

        if (args.Length < 3 || args.Length % 2 == 0 || Validations.HasDuplicates(args))
        {
            Console.WriteLine("\nInvalid input. Please provide an odd number of unique moves.");
            Console.WriteLine("Example: rock paper scissors lizard Spock\n");
            return;
        }

        string hmacKey = Hmac.GenerateKey();
        string computerMove = Hmac.GetRandomMove(args);
        string hmac = Hmac.CalculateHMAC(computerMove, hmacKey);

        Console.WriteLine("HMAC: " + hmac);
        ViewTables.DisplayMenu(args);

        string userMove;

        do
        {
            Console.Write("Enter your move: ");
            userMove = Console.ReadLine();

            if (userMove == "0")
                return;

            if (int.TryParse(userMove, out int moveNumber))
            {
                if (moveNumber >= 1 && moveNumber <= args.Length)
                {
                    userMove = args[moveNumber - 1];
                }
            }

        } while (!Validations.IsValidMove(userMove, args));

        Console.WriteLine("Your move: " + userMove);
        Console.WriteLine("Computer move: " + computerMove);

        int result = Validations.DetermineWinner(args.Length, Array.IndexOf(args, userMove), Array.IndexOf(args, computerMove));

        if (result == 0)
        {
            Console.WriteLine("It's a draw!");
        }
        else if (result > 0)
        {
            Console.WriteLine("You win!");
        }
        else
        {
            Console.WriteLine("Computer wins!");
        }

        Console.WriteLine("Hmac key: " + hmacKey);
    }

}