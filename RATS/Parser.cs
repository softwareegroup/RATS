using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace RATS
{
    class Parser
    {

        public static int pi = 928;
        public static int sigma = 963;
        public static int join = 10781;
        public static int left_semi_join = 8905;
        public static int right_semi_join = 8906;
        public static int anti_Join = 9655;
        public static int union = 8746;
        public static int intersection = 8745;
        
        
       
        public readonly static String CONN = "User ID=rats;Password=softwareeng2;Data Source=//picard2.radford.edu:1521/itec2.radford.edu;";

        public int wordIndex = 0;
        public string[] words;
        public string currentWord = "";
        public string nextWord = "";
        public string whereClause = "";
        public int parenCount = 0;
       

        //Set delimiters 
        char[] delimiterChars = { ' ', '\t', '\r' };

        public Parser(String _algebra)
        {
            try
            {

                String parsed = GetRtfUnicodeEscapedString(CleanInput(_algebra));
                words = parsed.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

               //   MessageBox.Show(parsed);

                wordIndex = words.Length - CountWords(parsed);

                //  MessageBox.Show("Number of words:" + CountWords(parsed) + "current index");

                currentWord = words[wordIndex];
            }
            catch (IndexOutOfRangeException e)
            {
                //do nothing for now, catching the user submitting empty input
            }

        }

        public string GetRtfUnicodeEscapedString(string s)
        {
            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (c == 0x28) // "("
                    sb.Append(" " + c + " ");
                else if (c == 0x29) // ")"
                    sb.Append(" " + c + " ");                      

                else if (c <= 0x7f)
                    sb.Append(c);
                else
                    sb.Append(Convert.ToUInt32(c) + " ");
                
            }
            return sb.ToString();
        }



        public static DataTable getGenericData(string SQL)
        {
            DataSet ds = new DataSet();

            OracleConnection dbConnection = new OracleConnection();
            dbConnection.ConnectionString = CONN;
                //AppFunctions.getConnectionString();
           

            dbConnection.Open();
          

            OracleCommand cmd = new OracleCommand(SQL, dbConnection);
            cmd.CommandType = CommandType.Text;

            OracleDataAdapter da = new OracleDataAdapter(cmd);


            OracleCommandBuilder cb = new OracleCommandBuilder(da);
        

            da.Fill(ds);

            dbConnection.Close();

            DataTable dt = ds.Tables["GenericData"];

            return dt;
        }


       


        public string parseSQL(String currentWord)
        {
            String returnString = "";
            Boolean prevExpr = false;
            
            
            //foreach (string word in words)
            //{
            //    MessageBox.Show("The word is : " + word);
            // }

            //  MessageBox.Show("The first word is :" + words[wordIndex]);

            try
            {


                if (Char.IsLetter(currentWord[0]))
                {
                    returnString += "SELECT * FROM " + currentWord;

                }
                else if (Char.IsNumber(currentWord[0]))
                {
                    if (Int32.Parse(currentWord) == pi && words.Length > (wordIndex + 1))
                    {
                        returnString += "SELECT " + words[wordIndex + 1];
                        wordIndex += 1;
                    }
                    else if (Int32.Parse(currentWord) == pi)
                    {
                        returnString += "SELECT ";
                    }
                    else
                    {
                        returnString += Int32.Parse(currentWord);  //for testing
                    }
                }
                else if (currentWord[0] == '(' && words.Length > (wordIndex + 1))
                {
                    nextWord = words[wordIndex + 1];

                    if (Char.IsNumber(nextWord[0]))
                    {
                        if (Int32.Parse(nextWord) == sigma && words.Length > (wordIndex + 2))
                        {

                            //  returnString += " WHERE " + words[wordIndex + 2];
                            whereClause = " \nWHERE " + words[wordIndex + 2];   //store clause to be placed after FROM statement
                            wordIndex += 2;
                        }
                        else if (Int32.Parse(nextWord) == sigma)
                        {
                            returnString += " \nWHERE ";
                            wordIndex += 1;
                        }
                        else if (Int32.Parse(nextWord) == pi && words.Length > (wordIndex + 2))
                        {
                            returnString += " \nFROM (SELECT " + words[wordIndex + 2]; ;
                            prevExpr = true;
                            parenCount += 1;
                            wordIndex += 2;
                        }
                        else if (Int32.Parse(nextWord) == pi)
                        {
                            returnString += " \nFROM (SELECT ";
                            wordIndex += 1;
                        }

                    }

                    else if (Char.IsLetter(nextWord[0]))
                    {

                        returnString += " \nFROM " + words[wordIndex + 1] + whereClause;
                        wordIndex += 1;
                    }

                }
                else if (currentWord[0] == '(')
                {
                    returnString += " \nFROM ";
                }
                else if (currentWord[0] == ')')
                {
                    if (prevExpr = true && parenCount > 0)
                    {
                        returnString += ")";
                        prevExpr = false;
                        parenCount -= 1;
                            }
                    wordIndex += 1;
                }
                else
                {
                    returnString += currentWord;
                    wordIndex += 1;
                }

                if (words.Length > (wordIndex + 1))
                {
                    wordIndex += 1;
                    currentWord = words[wordIndex];
                    return returnString + parseSQL(currentWord);
                }

            }
            catch (IndexOutOfRangeException e)
            {
                //do nothing for now, catching if user tries to submit empty string
            }

            return returnString;
        }

        static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return Regex.Replace(strIn, @"[^\w\.\,\(\)\=\'\>\<\!-]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return String.Empty;
            }
        }

        public static int CountWords(string s)
        {
            MatchCollection collection = Regex.Matches(s, @"[\S]+");
            return collection.Count;
        }



        //currently not using toSQL due to some parsing issues
        //parseSQL is the replacement but this could be useful in the future
        public string toSQL(String _algebra)
        {
            String returnString = "";
            String dummy = "";
            Scanner scan = new Scanner(_algebra);

            dummy = scan.next();


            if (Char.IsLetter(dummy[0]))
            {
                returnString += "SELECT * FROM " + dummy;
            }
            else if (Char.IsNumber(dummy[0]))
            {
                if (Int32.Parse(dummy) == pi)
                {
                    returnString += "SELECT " + scan.next();
                }
            }
            else if (dummy[0] == '(')
            {
                returnString += " FROM " + scan.next();

            }
            else
            {
                returnString += dummy;
            }

            if (scan.hasNext())
            {
                return returnString + toSQL(scan.next());
            }

            return returnString;
        }
    }
}
