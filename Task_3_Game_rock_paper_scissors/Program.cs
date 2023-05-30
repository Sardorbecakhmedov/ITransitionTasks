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

        var (hmacKey, userMove, computerMove, isExit) = Startup.UserMove(args);

        if(isExit)
            return;

        Startup.WinnerCheck(hmacKey, args, userMove, computerMove);

    }

}