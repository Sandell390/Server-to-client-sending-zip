using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;

namespace Server
{
    public class directories
    {
        //Indholder info om Sange, deres path og id
        public List<infoClass> MainList { get; set; }
    }

    public class infoClass
    {
        public int Id = 0; //Id bliver givet til hvert bibliotek
        public string dirPath { get; set; } //Path for bibliotek
        public List<string> path { get; set; } //Sange navne i bibliotek
    }

    public static class JsonHandler
    {
        public static List<string> wavFiles = new List<string>(); //full path af sangene (Navnet giver ikke mening, ved det)
        public static List<string> wavNames = new List<string>(); //Navnet på sangene

        static string JsonFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Directories.json"; //Pathen til Json filen

        static int CountIds = 1;
        public static void FirstRead()
        {
            //Tjekker om der er blevet lavet en Json File
            try
            {
                var directories = ReadAndReturn();

                List<infoClass> CountPaths = directories.MainList;

                CountIds = CountPaths.Count;

            }
            catch (Exception)
            {
                //Laver Json Fil hvis der ikke er nogen 
                WriteFile();
            }
        }

        static void WriteFile()
        {
            //Laver en ny Json fil hvis der ikke er nogen
            try
            {
                var directories = GetDirectories();

                var jsonToWrite = JsonConvert.SerializeObject(directories, Formatting.Indented);

                using (var writer = new StreamWriter(JsonFilePath))
                {
                    writer.Write(jsonToWrite);
                }

                reload();
            }
            catch (Exception ex)
            {

                Console.WriteLine("Kan ikke lave Json Fil, Kontakt Jesper");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static directories GetDirectories()
        {
            //Sætter default variabler i Json Fil
            var directories = new directories
            {
                MainList = new List<infoClass> { new infoClass { Id = 1, dirPath = AppDomain.CurrentDomain.BaseDirectory + "Musik\\", path = new List<string>() } }
            };

            //Bliver nød til at lave et foreach loop for at få adgang til listen til sangene
            foreach (var item in directories.MainList)
            {
                item.path.AddRange(GetFileFormats(item.dirPath));
            }

            return directories;
        }

        public static directories ReadAndReturn()
        {
            string jsonFromFile;
            using (var reader = new StreamReader(JsonFilePath))
            {
                jsonFromFile = reader.ReadToEnd();
            }

            var pathh = JsonConvert.DeserializeObject<directories>(jsonFromFile);

            return pathh;
        }

        public static void reload()
        {
            //Opdater Musik filer


            try
            {
                var directories = ReadAndReturn();

                List<string> _temp = new List<string>();

                wavNames.Clear();
                wavFiles.Clear();


                //Sætter full path for sangene og deres navn 
                foreach (var item in directories.MainList)
                {
                    //Kan ikke fjerne sange fra json fil, 

                    _temp = GetFileFormats(item.dirPath);
                    int count = 0;

                    string torem = null; //Temp var
                    foreach (var songs in item.path)
                    {
                        if (songs != _temp[count])
                        {
                            torem = songs;

                            break;
                        }
                        count++;
                    }
                    if (torem != null)
                    {
                        item.path.Remove(torem);
                    }

                    //Console.WriteLine(item.dirPath);
                    foreach (var file in _temp)
                    {

                        wavFiles.Add(Path.Combine(item.dirPath, file));
                        wavNames.Add(file);

                    }

                }

                String encoded = JsonConvert.SerializeObject(directories);
                File.WriteAllText(JsonFilePath, encoded);
            }
            catch (Exception)
            {

                Console.WriteLine("Kan ikke læse " + JsonFilePath + " Kontakt Jesper");
            }

        }

        public static void UpdateFile(string UserDirectory)
        {
            //Adder bruger input til Json fil

            //Læser Json filen ind
            var directories = (dynamic)null;
            try
            {
                directories = ReadAndReturn();



                directories.MainList.Add(AddDirectories(directories, UserDirectory));

                //Skriver til filen
                var convertedJson = JsonConvert.SerializeObject(directories, Formatting.Indented);
                using (var writer = new StreamWriter(JsonFilePath))
                {
                    writer.Write(convertedJson);
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Kan ikke læse " + JsonFilePath + " Kontakt Jesper");
            }


        }

        private static infoClass AddDirectories(directories directories, string UserDirectory)
        {

            //Får bruger input så det kan bruges til at indsættes i Json filz
            var infoClass = new infoClass
            {
                dirPath = UserDirectory,
                Id = directories.MainList.Count + 1,
                path = new List<string>()

            };

            //Skriver sangene navn ind i filen som er i mappen
            infoClass.path.AddRange(GetFileFormats(infoClass.dirPath));

            return infoClass;
        }

        /* 
        public void UpdateDirList(ListBox listBox)
        {
            //Updater listen over bibliotekker
            listBox.SelectionMode = SelectionMode.One;
            listBox.Items.Clear();
            listBox.BeginUpdate();

            var directories = (dynamic)null;
            try
            {
                directories = ReadAndReturn(JsonFilePath);

                foreach (var item in directories.MainList)
                {
                    listBox.Items.Add(item.dirPath);
                }

                listBox.EndUpdate();
            }
            catch (Exception)
            {

                Console.WriteLine("Kan ikke læse " + JsonFilePath + " Kontakt Jesper");
            }


        }
        */

        public static void delectPath(int brugerTal)
        {
            //Sletter biblioteker
            try
            {
                var directories = ReadAndReturn();

                int tal = directories.MainList.Count;

                if (brugerTal == 0)
                {
                    Console.WriteLine("Du kan ikke fjerne dette bibliotek");
                }
                else if (brugerTal > 0 && brugerTal < tal)
                {
                    if (File.Exists(JsonFilePath))
                    {
                        infoClass torem = null; //Temp var
                        foreach (var item in directories.MainList)
                        {
                            if (brugerTal + 1 == item.Id)
                            {
                                torem = item;

                                break;
                            }
                        }
                        if (torem != null) //Fjerner bibliotekket hvis der er et
                        {
                            directories.MainList.Remove(torem);
                            foreach (var item in directories.MainList)
                            {
                                if (item.Id > brugerTal + 1)
                                {
                                    item.Id -= 1;
                                }
                            }
                        }
                        //Updater filen
                        String encoded = JsonConvert.SerializeObject(directories);
                        System.IO.File.WriteAllText(JsonFilePath, encoded);
                    }
                }
                reload();
            }
            catch (Exception)
            {

                Console.WriteLine("Kan ikke læse " + JsonFilePath + " Kontakt Jesper");
            }


        }

        private static List<string> GetFileFormats(string dirPath) //Får alle de supportede fil typer
        {
            List<string> fileFormats = new List<string>();

            fileFormats.Clear();

            fileFormats.AddRange(Directory.GetFiles(dirPath, "*.wav").Select(Path.GetFileName).ToList());
            fileFormats.AddRange(Directory.GetFiles(dirPath, "*.flac").Select(Path.GetFileName).ToList());
            fileFormats.AddRange(Directory.GetFiles(dirPath, "*.mp3").Select(Path.GetFileName).ToList());
            fileFormats.AddRange(Directory.GetFiles(dirPath, "*.aiff").Select(Path.GetFileName).ToList());
            fileFormats.AddRange(Directory.GetFiles(dirPath, "*.wma").Select(Path.GetFileName).ToList());

            return fileFormats;
        }
    }
}
