using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugAndFeature
{
    class Program
    {
        static int noPatch = 0, noBugSize = 0;
        static List<InputData> InputKeep = new List<InputData>();
        static List<int> PathTime = new List<int>();

        static void Main(string[] args)
        {
            char[] delimiter = { ' ' };
            bool isNumber = true;

            do
            {
                Console.WriteLine("Input Data");
                string input = Console.ReadLine();
                string[] splitInput = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                isNumber = int.TryParse(splitInput[0], out noPatch);
                if (isNumber)
                {
                    isNumber = int.TryParse(splitInput[1], out noBugSize);
                    if (isNumber)
                    {
                        if (noBugSize == 0 || noPatch == 0)
                        {
                            continue;
                        }
                        Console.WriteLine("Input: Time PatchCondition PatchDebug");

                        InputKeep = new List<InputData>();

                        //get patch condition and fixed patch
                        for (int i = 0; i < noPatch; i++)
                        {
                            string input2 = Console.ReadLine();
                            string[] splitInput2 = input2.Split(delimiter,StringSplitOptions.RemoveEmptyEntries);
                            //add value to global list for checking
                            if (splitInput2[1].Length != noBugSize && splitInput2[2].Length != noBugSize)
                            {
                                Console.WriteLine("Error All Patch Data will be erased, Try to Input Again.");
                                InputKeep.Clear();
                                break;
                            }
                            else
                            {
                                InputData myCurData = new InputData();
                                myCurData.Time = Convert.ToInt16(splitInput2[0]);
                                myCurData.PatchCond = splitInput2[1];
                                myCurData.Patched = splitInput2[2];
                                
                                InputKeep.Add(myCurData);
                            }
                        }

                        //Calculate Path / Data / Function
                        if (InputKeep.Count > 0)
                        {
                            string initial = string.Empty;
                            for (int i = 0; i < noBugSize; i++)
                            {
                                initial = initial + "+";
                            }
                            PathTime.Clear();
                            Caldata(initial, string.Empty, 0);
                            if (PathTime.Count > 0)
                                Console.WriteLine("Mininum Time For this bug is " + PathTime.Min() + " second.");
                            else
                                Console.WriteLine("Cannot Fix this bug.");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Error, There is not number. Try Again");
                    }
                }
                else
                {
                    Console.WriteLine("Error, There is not number. Try Again");
                }
            }while(noPatch != 0 && noBugSize !=0);

            Console.ReadKey();
        }

        static void Caldata(string input, string PrevPatch, int time)
        {
            for (int i = 0; i < InputKeep.Count; i++)
            {
                if (string.Compare(PrevPatch, InputKeep[i].PatchCond) != 0)
                {
                    if (CheckPreCondition(InputKeep[i].PatchCond, input))
                    {
                        string ResultDebug = FixedBug(input, InputKeep[i].Patched);
                        if (CheckIsAllFixed(ResultDebug))
                        {
                            //add value to PathTime for finding mininum
                            PathTime.Add(time + InputKeep[i].Time);
                        }
                        else if(time < 5000)
                        {
                            Caldata(ResultDebug, InputKeep[i].PatchCond, time + InputKeep[i].Time);
                        } //else is do nothing
                    }
                    //else do nothing
                }

            }
        }

        static bool CheckPreCondition(string Condition, string input)
        {
            int count = 0;
            for (int i = 0; i < noBugSize; i++)
            {
                if (Condition[i] == '0' || (Condition[i] == input[i]))
                {
                    count++;
                }
            }
            if (count == noBugSize)
                return true;
            else return false;
        }

        static string FixedBug(string input, string Patched)
        {
            string newString = string.Empty;
            for (int i = 0; i < noBugSize; i++)
            {
                if (Patched[i] == '0')
                {
                    newString = newString + input[i];
                }
                else
                {
                    newString = newString + Patched[i];
                }
            }
            return newString;
        }

        static bool CheckIsAllFixed(string input)
        {
            int count = 0;
            for (int i = 0; i < noBugSize; i++)
            {
                if (input[i] == '-')
                    count++;
            }
            if (count == noBugSize)
                return true;
            else return false;
        }
    }

    public class InputData
    {
        public int Time { get; set; }
        public string PatchCond { get; set; }
        public string Patched { get; set; }
    }
}
