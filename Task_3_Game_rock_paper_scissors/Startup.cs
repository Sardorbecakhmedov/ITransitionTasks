namespace Task_3_Game_rock_paper_scissors;

public static class Startup
{
    public static (string hmacKey, string userMove, string computerMove, bool IsExit) UserMove(string[] args)
    {
        string hmacKey = Hmac.GenerateKey();
        string computerMove = Hmac.GetRandomMove(args);
        string hmac = Hmac.CalculateHMAC(computerMove, hmacKey);
        bool isExit = false;

        Console.WriteLine("HMAC: " + hmac);
        ViewTables.DisplayMenu(args);

        string userMove;

        do
        {
            Console.Write("Enter your move: ");
            userMove = Console.ReadLine();

            if(userMove == "0")
                isExit = true;

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

        return (hmacKey, userMove, computerMove, isExit)!;
    }

    public static void WinnerCheck(string hmacKey, string[] args, string userMove, string computerMove)
    {
        var result = Validations.DetermineWinner(
            args.Length, Array.IndexOf(args, userMove), Array.IndexOf(args, computerMove));

        switch (result)
        {
            case 0:
                Console.WriteLine("It's a draw!");
                break;
            case > 0:
                Console.WriteLine("You win!");
                break;
            default:
                Console.WriteLine("Computer wins!");
                break;
        }

        Console.WriteLine("Hmac key: " + hmacKey);
    }

}