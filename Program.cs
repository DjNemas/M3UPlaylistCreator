using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace M3UPlaylistCreator
{
    class Program
    {
        private static bool samsung = false;

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
                Console.ResetColor();
                return;
            }

            FileInfo[] mp3Files = null;
            try
            {
                DirectoryInfo di = new DirectoryInfo(args[0]);
                mp3Files = di.GetFiles("*.mp3");
            }
            catch (Exception e)
            {
                Console.WriteLine("Ordner konnte nicht gelesen werden.\n" + e);
            }

            List<List<object>> List2D = new List<List<object>>();
            // Get all file informations and store them
            foreach (var item in mp3Files)
            {
                List<object> objectList = new List<object>();

                objectList.Add(TagLib.File.Create(item.FullName));
                objectList.Add(item.Name);

                List2D.Add(objectList);
            }

            Console.WriteLine("Playlist wird gebaut. Bitte warten!");

            // Create Header
            if (!samsung)
                File.AppendAllText(m3uData, "#EXTM3U\n");
            // Create M3U Infortmation per Music File
            int countRounds = 1;
            foreach (var item in List2D)
            {
                if (samsung)
                    File.AppendAllText(m3uData, "/storage/emulated/0/Music/Vocaloid Best Songs for me! Car/" + ((string)item[1]) + "\n");
                else
                    File.AppendAllText(m3uData, "#EXTINF:" + ((TagLib.File)item[0]).Properties.Duration.TotalSeconds.ToString().Split(',')[0] + "," + ((TagLib.File)item[0]).Tag.Title + "\n" + ((string)item[1]) + "\n");
                int percent = (int)((float)100 / (float)List2D.Count * (float)countRounds);
                countRounds++;
                Console.WriteLine(percent + "% fertig gestellt!");
            }
            Console.WriteLine("Playlist erstellt!");
            Thread.Sleep(5000);
        }
    }
}
