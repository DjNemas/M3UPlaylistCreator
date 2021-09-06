using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace M3UPlaylistCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bitte <Ordner> angeben.");
            args = new string[2];
            args[0] = Console.ReadLine();
            Console.WriteLine("Bitte <Playlistname> angeben.");
            args[1] = Console.ReadLine();
            Console.WriteLine(args[0] + " | " + args[1]);
            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Der Ordner Existiert nicht! Bitte mit <OrdnerPfad> angeben!");
                Thread.Sleep(5000);
                return;
            }
            if (args[1] == null)
            {
                Console.WriteLine("Bitte einen Playlistnamen <Playlistname> angeben!");
                Thread.Sleep(5000);
                return;
            }

            // FilePath
            string m3uData = args[0] + "\\" + args[1] + ".m3u";

            // Create file
            try
            {
                File.Create(m3uData).Close();
                Console.WriteLine("Datei erstellt");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Beim erstellen der Playlist Datei ist ein Fehler aufgetretten.\n" + e);
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            string[] mp3Files = null;
            try
            {
                mp3Files = Directory.GetFiles(args[0], "*mp3");
            }
            catch (Exception e)
            {
                Console.WriteLine("Ordner konnte nicht gelesen werden.\n" + e);
            }

            List<TagLib.File> tlList = new List<TagLib.File>();
            // Get all file informations and store them
            foreach (var item in mp3Files)
            {
                tlList.Add(TagLib.File.Create(item));
            }

            Console.WriteLine("Playlist wird gebaut. Bitte warten!");
            int countRounds = 1;
            // Create Header
            File.AppendAllText(m3uData, "#EXTM3U\n");

            // Create M3U Infortmation per Music File
            foreach (var item in tlList)
            {

                string fileName = removeUntilChar(item.Name, '\\');
                File.AppendAllText(m3uData, "#EXTINF:" + item.Properties.Duration.TotalSeconds.ToString().Split(',')[0] + "," + item.Tag.Title + "\n" + fileName + "\n");
                int percent = (int)((float)100 / (float)tlList.Count * (float)countRounds);
                countRounds++;
                Console.WriteLine(percent + "% fertig gestellt!");
            }
            Console.WriteLine("Playlist erstellt!");
            Thread.Sleep(5000);
        }

        public static string removeUntilChar(string str, char character)
        {
            string reverseString = "";
            for (int i = str.Length - 1; i > -1; i--)
            {
                reverseString += str[i];
            }

            string shortString = "";
            for (int i = 0; i < reverseString.Length; i++)
            {
                if (reverseString[i].Equals(character))
                {
                    break;
                }
                shortString += reverseString[i];
            }

            string newString = "";
            for(int i = shortString.Length - 1 ; i > -1; i--)
            {
                newString += shortString[i];
            }
            return newString;
        }
    }
}
