using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenges
{
    public class OpenLock
    {
        /*
 * You are given a 3-digit lock, represented as a string of 3 digits (e.g., "000").
 * The goal is to reach a target combination (e.g., "888") by rotating the dials. Each dial can be rotated independently by one step clockwise or counterclockwise.
 * Each rotation wraps around (e.g., from 0 to 9, or from 9 to 0).
 * You are also given a list of "deadends," which are combinations you cannot pass through.
 * 
 * The task is to return the minimum number of rotations (turns) needed to reach the target combination from the starting combination
 * If it's impossible to reach the target (due to deadends), return -1.
 * 
 * 
 * Constraints:
 * - The lock has 1,000 possible combinations ("000" to "999").
 * - Deadends can be up to 500 combinations.
 * - You must efficiently search through all possibilities.
 */

        [Theory]
        [InlineData("000", "999", new[] { "001", "010", "100" }, 3)] 
        [InlineData("888", "000", new string[] { }, 6)]                
        [InlineData("111", "000", new string[] { "111" }, -1)]          
        [InlineData("000", "001", new string[] { }, 1)]                
        [InlineData("005", "999", new string[] { "001", "002", "003", "004", "006", "007", "008", "009" }, 6)] 
        [InlineData("010", "010", new string[] { "009", "011", "110" }, 0)]
        [InlineData("080", "090", new string[] { "091" }, 1)] 
        [InlineData("000", "100", new string[] { "010", "001", "900" }, 1)] 
        [InlineData("543", "555", new string[] { "554", "556", "545", "565", "455", "655" }, -1)] 
        [InlineData("200", "888", new string[] { }, 8)]
        [InlineData("002", "001", new string[] { "001" }, -1)] 
        [InlineData("012", "123", new string[] { }, 3)] 
        [InlineData("505", "101", new string[] { "100", "102", "001" }, 8)] 
        public void Test(string startCombination, string targetCombination, string[] deadlocks, int expectedTurns)
        {
            var turns = Solution(startCombination, targetCombination, deadlocks);

            Assert.Equal(expectedTurns, turns);
        }

        private int Solution(string startingCombination, string targetCombination, string[] deadlock)
        {
            const int TOP_ROW = 0;
            const int MIDDLE_ROW = 1;
            const int BOTTOM_ROW = 2;


            if (deadlock.Contains(startingCombination) || deadlock.Contains(targetCombination))
                return -1;

            Queue<(string, int)> possibleCombinations = new Queue<(string, int)> ();
            HashSet<string> visitedCombinations = new HashSet<string>();

            int currentTurns = 0;
            possibleCombinations.Enqueue((startingCombination, currentTurns));
            visitedCombinations.Add(startingCombination);

            while(possibleCombinations.Count > 0)
            {
                (string currentCombination, int turns) = possibleCombinations.Dequeue();

                if(currentCombination == targetCombination)
                {
                    return turns;
                }

                turns = turns + 1;
                EnqueueNewCombination(TurnRight(currentCombination, TOP_ROW), turns, possibleCombinations, visitedCombinations, deadlock);
                EnqueueNewCombination(TurnRight(currentCombination, MIDDLE_ROW), turns, possibleCombinations, visitedCombinations, deadlock);
                EnqueueNewCombination(TurnRight(currentCombination, BOTTOM_ROW), turns, possibleCombinations, visitedCombinations, deadlock);

                EnqueueNewCombination(TurnLeft(currentCombination, TOP_ROW), turns, possibleCombinations, visitedCombinations, deadlock);
                EnqueueNewCombination(TurnLeft(currentCombination, MIDDLE_ROW), turns, possibleCombinations, visitedCombinations, deadlock);
                EnqueueNewCombination(TurnLeft(currentCombination, BOTTOM_ROW), turns, possibleCombinations, visitedCombinations, deadlock);
            }

            return -1;
        }

        private void EnqueueNewCombination(string combination, int turns, Queue<(string, int)> queue, HashSet<string> visited, string[] deadlocks)
        {
            if(!visited.Contains(combination) && !deadlocks.Contains(combination))
            {
                queue.Enqueue((combination, turns));
                visited.Add(combination);  
            }

        }

        private string TurnRight(string combination, int row)
        {
            char[] numbers = combination.ToCharArray();

            if(int.TryParse(numbers[row].ToString(), out int number))
            {
                if(number == 9)
                {
                    number = 0;
                }
                else
                {
                    number++;
                }

                numbers[row] = char.Parse(number.ToString());

                return new string(numbers);
            }

            throw new Exception("Char was not a number");
        }

        private string TurnLeft(string combination, int row)
        {
            char[] numbers = combination.ToCharArray();

            if (int.TryParse(numbers[row].ToString(), out int number))
            {
                if (number == 0)
                {
                    number = 9;
                }
                else
                {
                    number--;
                }

                numbers[row] = char.Parse(number.ToString());

                return new string(numbers);
            }

            throw new Exception("Char was not a number");
        }
    }
}
