namespace TestProject1
{
    public class StringFlipper
    {
        //Given a string with N length that only contains A and B, and a start index for a piece that sits between N and N+1
        //Considering every time the piece jumps over a letter it flips it
        //Find the minimum amount of jumps so the string has the same number of A and B chars
        [Theory]
        [InlineData("aaaab", 0, -1)]              
        [InlineData("aababa", 2, 1)]              
        [InlineData("aaaaaa", 2, 3)]              
        [InlineData("ababab", 3, 0)]              
        [InlineData("aabbaabb", 0, 0)]            
        [InlineData("bbbbbbbb", 4, 4)]            
        [InlineData("ababaa", 2, 3)]              
        [InlineData("ab", 0, 0)]                  
        public void Test(string s, int start, int expected)
        {
            int minJumps = Solution(s, start);

            Assert.Equal(expected, minJumps);
        }

        private int Solution(string s, int start)
        {
            //If string is odd, will never balance
            if(s.Length % 2 != 0)
            {
                return -1;
            }

            int numberOfJumps = CheckCombinations(s, start);

            return numberOfJumps;

        }

        private int CheckCombinations(string s, int start)
        {
            //We want to do a Breadth-First Search to check all possible combinations
            //We use a HashSet to quickly check which combinations we already checked
            HashSet<(string,int)> visitedCombinations = new HashSet<(string,int)>();
            Queue<(string, int, int)> possibleCombinations = new Queue<(string, int, int)>();

            //The given string is the first possible combination
            int jumpsSoFar = 0;
            possibleCombinations.Enqueue((s, start, jumpsSoFar));
            visitedCombinations.Add((s, start));


            while (possibleCombinations.Count > 0)
            {
                (string combination, int currentIndex, int jumps) = possibleCombinations.Dequeue();

                char[] letters = combination.ToCharArray();

                int aCount = 0, bCount = 0;

                for (int i = 0; i < letters.Length; i++)
                {
                    if (letters[i] == 'a')
                        aCount++;
                    if (letters[i] == 'b')
                        bCount++;
                }

                if (aCount == bCount)
                    return jumps;

                if(currentIndex-1 >= 0)// if we can go left
                {
                    string leftCharFlipped = Flip(combination, currentIndex - 1);

                    if(!visitedCombinations.Contains((leftCharFlipped, currentIndex - 1)))
                    {
                        //Jump Left by enqueueing the new state and adding a jump
                        possibleCombinations.Enqueue((Flip(combination, currentIndex - 1), currentIndex - 1, jumps+1));
                        visitedCombinations.Add((leftCharFlipped, currentIndex - 1));
                    }
                }
                if(currentIndex+1 < combination.Length)// if we can go right
                {
                    string rightCharFlipped = Flip(combination, currentIndex + 1);

                    if(!visitedCombinations.Contains((rightCharFlipped, currentIndex + 1)))
                    {
                        //Jump right by enqueueing the new state and adding a jump
                        possibleCombinations.Enqueue((rightCharFlipped, currentIndex+1, jumps+1));
                        visitedCombinations.Add((rightCharFlipped, currentIndex+1));
                    }
                }
            }

            return -1;
        }
        

        private string Flip(string s, int index)
        {
            char[] letters = s.ToCharArray();

            if (letters[index] == 'a')
            {
                letters[index] = 'b';
            }
            else if (letters[index] == 'b')
            {
                letters[index] = 'a';
            }
            return new string(letters);
        }
    }
}