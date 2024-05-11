namespace IAGE.Shared;

public class TokenGenerator
{
    
    private static char[] letters = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' };
    private static char[] numbers = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
    private static char[] special = new[] { '!', 'à', '&', '$' };
    
    public static string GenerateToken(int length)
    {
        string generatedToken = "";
        
        char[] [] arrays = new[] { letters, numbers, special };

        Random randomArray = new Random();
      
        for (int i = 0; i < length; i++)
        {
           var selectedArray= arrays[randomArray.Next(0, 3)];
           char targetChar = selectedArray[randomArray.Next(0, selectedArray.Length - 1)];
           generatedToken += targetChar;
        }

        return generatedToken;
    }
}