using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam
{
    internal class Program
    {
        static List<User> users = new List<User>();
        static User currentUser = null;

        static void Main(string[] args)
        {
            users = LoadUserList();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Quiz Application!");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an action: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        Register();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

            }
        }

        static void Login()
        {
            Console.Clear();
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();

            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            User user = users.Find(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                currentUser = user;
                MainMenu();
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.");
                Console.ReadKey();
            }
        }

        static void Register()
        {
            Console.Clear();
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();

            if (users.Exists(u => u.Username == username))
            {
                Console.WriteLine("User with this username already exists.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            Console.Write("Enter your date of birth (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
            {
                User newUser = new User
                {
                    Username = username,
                    Password = password,
                    DateOfBirth = dateOfBirth
                };

                users.Add(newUser);
                SaveUserList(users);
                Console.WriteLine("Registration completed successfully. You can now log in.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid date format. Registration not completed.");
                Console.ReadKey();
            }
        }

        static void SaveQuizResults(List<QuizResult> results)
        {
            string filePath = "quiz_results.csv";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var result in results)
                {
                    writer.WriteLine(result.ToString());
                }
            }
        }

        static List<QuizResult> LoadQuizResults()
        {
            string filePath = "quiz_results.csv";

            if (File.Exists(filePath))
            {
                List<QuizResult> results = new List<QuizResult>();
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        QuizResult result = new QuizResult
                        {
                            Username = parts[0],
                            Section = parts[1],
                            CorrectAnswers = int.Parse(parts[2]),
                            QuizDate = DateTime.ParseExact(parts[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                        };
                        results.Add(result);
                    }
                }

                return results;
            }

            return new List<QuizResult>();
        }

        static void SaveUserList(List<User> userList)
        {
            string filePath = "users.csv";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var user in userList)
                {
                    writer.WriteLine($"{user.Username},{user.Password},{user.DateOfBirth:yyyy-MM-dd}");
                }
            }
        }

        static List<User> LoadUserList()
        {
            string filePath = "users.csv";

            if (File.Exists(filePath))
            {
                List<User> userList = new List<User>();
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        User user = new User
                        {
                            Username = parts[0],
                            Password = parts[1],
                            DateOfBirth = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        };
                        userList.Add(user);
                    }
                }

                return userList;
            }

            return new List<User>();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome, {currentUser.Username}!");
                Console.WriteLine("1. Start a New Quiz");
                Console.WriteLine("2. View Past Quiz Results");
                Console.WriteLine("3. Top 20 in the Quiz");
                Console.WriteLine("4. Change Settings");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an action: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StartQuiz();
                        break;
                    case "2":
                        ViewQuizResults();
                        break;
                    case "3":
                        ViewTop20();
                        break;
                    case "4":
                        ChangeSettings();
                        break;
                    case "5":
                        currentUser = null;
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void StartQuiz()
        {
            Console.Clear();
            Console.WriteLine("Choose a quiz section:");
            Console.WriteLine("1. History");
            Console.WriteLine("2. Geography");
            Console.WriteLine("3. Biology");
            Console.WriteLine("4. Mixed");
            Console.Write("Enter the section number: ");

            string sectionChoice = Console.ReadLine();

            int correctAnswers = 0;

            switch (sectionChoice)
            {
                case "1":
                    correctAnswers = StartHistoryQuiz();
                    break;
                case "2":
                    correctAnswers = StartGeographyQuiz();
                    break;
                case "3":
                    correctAnswers = StartBiologyQuiz();
                    break;
                case "4":
                    correctAnswers = StartMixedQuiz();
                    break;
                default:
                    Console.WriteLine("Invalid section choice.");
                    Console.ReadKey();
                    return;
            }

            Console.Clear();
            Console.WriteLine($"Number of correct answers: {correctAnswers}/20");



            Console.WriteLine("Thank you for participating in the quiz!");
            Console.ReadKey();
        }

        static int StartHistoryQuiz()
        {
            Console.Clear();
            Console.WriteLine("History Quiz:");

            List<string> questions = new List<string>();
            List<string> correctAnswers = new List<string>();

            questions.AddRange(GetHistoryQuestions());
            correctAnswers.AddRange(GetHistoryCorrectAnswers());

            int correctAnswersCount = 0;
            int questionNumber = 1;

            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine($"{questionNumber}. {questions[i]}");
                Console.Write("Your answer: ");
                string userAnswer = Console.ReadLine();

                if (userAnswer.ToLower() == correctAnswers[i].Substring(0, 1).ToLower())
                {
                    Console.WriteLine("Correct!");
                    Console.WriteLine();
                    correctAnswersCount++;
                }
                else
                {
                    Console.WriteLine("Incorrect.");
                    Console.WriteLine();
                }
                questionNumber++;
            }

            QuizResult result = new QuizResult
            {
                Username = currentUser.Username,
                Section = "History",
                CorrectAnswers = correctAnswersCount,
                QuizDate = DateTime.Now
            };

            List<QuizResult> results = LoadQuizResults();
            results.Add(result);

            SaveQuizResults(results);

            return correctAnswersCount;
        }

        static int StartGeographyQuiz()
        {
            Console.Clear();
            Console.WriteLine("Geography Quiz:");

            List<string> questions = new List<string>();
            List<string> correctAnswers = new List<string>();

            questions.AddRange(GetGeographyQuestions());
            correctAnswers.AddRange(GetGeographyCorrectAnswers());

            int correctAnswersCount = 0;
            int questionNumber = 1;

            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine($"{questionNumber}. {questions[i]}");
                Console.Write("Your answer: ");
                string userAnswer = Console.ReadLine();

                if (userAnswer.ToLower() == correctAnswers[i].Substring(0, 1).ToLower())
                {
                    Console.WriteLine("Correct!");
                    Console.WriteLine();
                    correctAnswersCount++;
                }
                else
                {
                    Console.WriteLine("Incorrect.");
                    Console.WriteLine();
                }
                questionNumber++;
            }

            QuizResult result = new QuizResult
            {
                Username = currentUser.Username,
                Section = "Geography",
                CorrectAnswers = correctAnswersCount,
                QuizDate = DateTime.Now
            };

            List<QuizResult> results = LoadQuizResults();
            results.Add(result);

            SaveQuizResults(results);

            return correctAnswersCount;
        }

        static int StartBiologyQuiz()
        {
            Console.Clear();
            Console.WriteLine("Biology Quiz:");

            List<string> questions = new List<string>();
            List<string> correctAnswers = new List<string>();

            questions.AddRange(GetBiologyQuestions());
            correctAnswers.AddRange(GetBiologyCorrectAnswers());

            int correctAnswersCount = 0;
            int questionNumber = 1;

            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine($"{questionNumber}. {questions[i]}");
                Console.Write("Your answer: ");
                string userAnswer = Console.ReadLine();

                if (userAnswer.ToLower() == correctAnswers[i].Substring(0, 1).ToLower())
                {
                    Console.WriteLine("Correct!");
                    Console.WriteLine();
                    correctAnswersCount++;
                }
                else
                {
                    Console.WriteLine("Incorrect.");
                    Console.WriteLine();
                }
                questionNumber++;
            }

            QuizResult result = new QuizResult
            {
                Username = currentUser.Username,
                Section = "Biology",
                CorrectAnswers = correctAnswersCount,
                QuizDate = DateTime.Now
            };

            List<QuizResult> results = LoadQuizResults();
            results.Add(result);

            SaveQuizResults(results);

            return correctAnswersCount;
        }

        static int StartMixedQuiz()
        {
            Console.Clear();
            Console.WriteLine("Mixed Quiz:");

            List<string> questions = new List<string>();
            List<string> correctAnswers = new List<string>();

            List<string> historyQuestions = GetHistoryQuestions();
            List<string> historyCorrectAnswers = GetHistoryCorrectAnswers();

            List<string> geographyQuestions = GetGeographyQuestions();
            List<string> geographyCorrectAnswers = GetGeographyCorrectAnswers();

            List<string> biologyQuestions = GetBiologyQuestions();
            List<string> biologyCorrectAnswers = GetBiologyCorrectAnswers();

            int maxQuestionsPerCategory = 7;
            int totalQuestions = 20;

            AddRandomQuestions(questions, correctAnswers, historyQuestions, historyCorrectAnswers, maxQuestionsPerCategory);
            AddRandomQuestions(questions, correctAnswers, geographyQuestions, geographyCorrectAnswers, maxQuestionsPerCategory);
            AddRandomQuestions(questions, correctAnswers, biologyQuestions, biologyCorrectAnswers, maxQuestionsPerCategory);

            Random random = new Random();
            List<string> shuffledQuestions = new List<string>();
            List<string> shuffledAnswers = new List<string>();

            for (int i = 0; i < totalQuestions; i++)
            {
                int categoryIndex = i % 3; // 0 - history, 1 - geography, 2 - biology

                List<string> sourceQuestions;
                List<string> sourceCorrectAnswers;

                if (categoryIndex == 0)
                {
                    sourceQuestions = historyQuestions;
                    sourceCorrectAnswers = historyCorrectAnswers;
                }
                else if (categoryIndex == 1)
                {
                    sourceQuestions = geographyQuestions;
                    sourceCorrectAnswers = geographyCorrectAnswers;
                }
                else
                {
                    sourceQuestions = biologyQuestions;
                    sourceCorrectAnswers = biologyCorrectAnswers;
                }

                if (sourceQuestions.Count > 0)
                {
                    int randomIndex = random.Next(0, sourceQuestions.Count);
                    shuffledQuestions.Add(sourceQuestions[randomIndex]);
                    shuffledAnswers.Add(sourceCorrectAnswers[randomIndex]);
                    sourceQuestions.RemoveAt(randomIndex);
                    sourceCorrectAnswers.RemoveAt(randomIndex);
                }
            }

            int correctAnswersCount = 0;
            int questionNumber = 1;

            for (int i = 0; i < shuffledQuestions.Count; i++)
            {
                Console.WriteLine($"{questionNumber}. {shuffledQuestions[i]}");
                Console.Write("Your answer: ");
                string userAnswer = Console.ReadLine();

                if (userAnswer.ToLower() == shuffledAnswers[i].Substring(0, 1).ToLower())
                {
                    Console.WriteLine("Correct!");
                    Console.WriteLine();
                    correctAnswersCount++;
                }
                else
                {
                    Console.WriteLine("Incorrect.");
                    Console.WriteLine();
                }
                questionNumber++;
            }

            QuizResult result = new QuizResult
            {
                Username = currentUser.Username,
                Section = "Mixed",
                CorrectAnswers = correctAnswersCount,
                QuizDate = DateTime.Now
            };

            List<QuizResult> results = LoadQuizResults();
            results.Add(result);

            SaveQuizResults(results);

            return correctAnswersCount;
        }

        static void AddRandomQuestions(List<string> mixedQuestions, List<string> mixedCorrectAnswers, List<string> questions, List<string> correctAnswers, int maxQuestions)
        {
            Random random = new Random();

            while (mixedQuestions.Count < maxQuestions && questions.Count > 0)
            {
                int randomIndex = random.Next(0, questions.Count);
                mixedQuestions.Add(questions[randomIndex]);
                mixedCorrectAnswers.Add(correctAnswers[randomIndex]);
                questions.RemoveAt(randomIndex);
                correctAnswers.RemoveAt(randomIndex);
            }
        }

        static List<string> GetHistoryQuestions()
        {
            return new List<string>
    {
        "In what year did the French Revolution begin?" +
        "\r\n   a) 1789\r\n   b) 1799\r\n   c) 1801",
        "Which Roman emperor declared Christianity the official religion of the Roman Empire?" +
        "\r\n   a) Augustus\r\n   b) Constantine the Great\r\n   c) Nero",
        "What event in the Middle Ages is often considered the beginning of a new era and the Renaissance?" +
        "\r\n   a) Fall of the Roman Empire\r\n   b) Crusades\r\n   c) Fall of the Byzantine Empire",
        "Which two countries were involved in the Cold War?" +
        "\r\n   a) USA and France\r\n   b) USSR and China\r\n   c) USSR and USA",
        "What was the main factor that triggered World War I?" +
        "\r\n   a) Assassination of Archduke Franz Ferdinand\r\n   b) Dissolution of the Ottoman Empire\r\n   c) Expansion of colonial empires",
        "Which period in European history followed the Middle Ages and preceded the Renaissance?" +
        "\r\n   a) Antiquity\r\n   b) Great Depression\r\n   c) Renaissance",
        "Who founded the Maurya dynasty in India?" +
        "\r\n   a) Ashoka the Great\r\n   b) Chandragupta Maurya\r\n   c) Gandhi",
        "In which year was the Great Migration of Peoples proclaimed?" +
        "\r\n   a) 410 AD\r\n   b) 476 AD\r\n   c) 395 AD",
        "Who was the first President of the United States?" +
        "\r\n   a) George Washington\r\n   b) John Adams\r\n   c) Thomas Jefferson",
        "Which country participated in the War of the Triple Alliance in the 17th century?" +
        "\r\n   a) Germany\r\n   b) Russia\r\n   c) Sweden",
        "In which year was the Magna Carta signed?" +
        "\r\n   a) 1215 AD\r\n   b) 1492 AD\r\n   c) 1776 AD",
        "Who was the first emperor of the Roman Empire?" +
        "\r\n   a) Julius Caesar\r\n   b) Augustus\r\n   c) Tiberius",
        "Which dynasty ruled China in the 3rd century BC?" +
        "\r\n   a) Qin\r\n   b) Han\r\n   c) Tang",
        "What event is considered the beginning of World War I?" +
        "\r\n   a) Attack on Poland\r\n   b) Assassination of Archduke Franz Ferdinand\r\n   c) War in Afghanistan",
        "Which Greek philosopher is considered the father of Western philosophy?" +
        "\r\n   a) Aristotle\r\n   b) Plato\r\n   c) Socrates",
        "Which great leader led the Indian people to independence from the British Empire?" +
        "\r\n   a) Mahatma Gandhi\r\n   b) Jawaharlal Nehru\r\n   c) Vallabhbhai Patel",
        "What event marked the end of the Middle Ages in Europe and the beginning of the Modern Era?" +
        "\r\n   a) Columbus's discovery of America\r\n   b) Fall of the Roman Empire\r\n   c) French Revolution",
        "Who founded the Carolingian dynasty in the Frankish state?" +
        "\r\n   a) Charlemagne\r\n   b) Charles Martel\r\n   c) Charlemagne the Younger",
        "Which Greek philosopher was the teacher of Alexander the Great?" +
        "\r\n   a) Aristotle\r\n   b) Socrates\r\n   c) Plato",
        "Which dynasty ruled Russia before the Romanovs?" +
        "\r\n   a) Rurik Dynasty\r\n   b) Godunovs\r\n   c) Petrovichs"
    };
        }

        static List<string> GetHistoryCorrectAnswers()
        {
            return new List<string>
    {
        "a",
        "b",
        "b",
        "c",
        "a",
        "c",
        "b",
        "a",
        "a",
        "c",
        "a",
        "b",
        "b",
        "b",
        "c",
        "a",
        "c",
        "b",
        "a",
        "b"
    };
        }

        static List<string> GetGeographyQuestions()
        {
            return new List<string>
    {
        "What year did the French Revolution begin?" +
        "\r\n   a) 1789\r\n   b) 1799\r\n   c) 1801",
        "Which Roman emperor declared Christianity the official religion of the Roman Empire?" +
        "\r\n   a) Augustus\r\n   b) Constantine the Great\r\n   c) Nero",
        "What event in the Middle Ages is often considered the beginning of a new era and the medieval renaissance?" +
        "\r\n   a) Fall of the Roman Empire\r\n   b) Crusades\r\n   c) Fall of the Byzantine Empire",
        "Which two countries were involved in the Cold War?" +
        "\r\n   a) USA and France\r\n   b) USSR and China\r\n   c) USSR and USA",
        "What was the main factor that triggered World War I?" +
        "\r\n   a) Assassination of Archduke Franz Ferdinand\r\n   b) Dissolution of the Ottoman Empire\r\n   c) Expansion of colonial empires",
        "What period in European history followed the Middle Ages and preceded the Renaissance?" +
        "\r\n   a) Antiquity\r\n   b) The Great Depression\r\n   c) The Renaissance",
        "Who was the founder of the Maurya dynasty in India?" +
        "\r\n   a) Ashoka the Great\r\n   b) Chandragupta Maurya\r\n   c) Gandhi",
        "In what year was the Great Migration of Peoples proclaimed?" +
        "\r\n   a) 410 AD\r\n   b) 476 AD\r\n   c) 395 AD",
        "Who was the first President of the United States?" +
        "\r\n   a) George Washington\r\n   b) John Adams\r\n   c) Thomas Jefferson",
        "Which country participated in the War of the Triple Alliance in the 17th century?" +
        "\r\n   a) Germany\r\n    b) Russia\r\n    c) Sweden",
        "In what year was the Magna Carta signed?" +
        "\r\n   a) 1215 AD\r\n    b) 1492 AD\r\n    c) 1776 AD",
        "Who was the first emperor of the Roman Empire?" +
        "\r\n   a) Julius Caesar\r\n    b) Augustus\r\n    c) Tiberius",
        "Which dynasty ruled China in the 3rd century BC?" +
        "\r\n   a) Qin\r\n    b) Han\r\n    c) Tang",
        "What event is considered the beginning of World War I?" +
        "\r\n   a) Attack on Poland\r\n    b) Assassination of Archduke Franz Ferdinand\r\n    c) War in Afghanistan",
        "Which Greek philosopher is considered the father of Western philosophy?" +
        "\r\n   a) Aristotle\r\n    b) Plato\r\n    c) Socrates",
        "Which great leader led the Indian people to independence from the British Empire?" +
        "\r\n   a) Mahatma Gandhi\r\n    b) Jawaharlal Nehru\r\n    c) Vallabhbhai Patel",
        "What event marked the end of the Middle Ages in Europe and the beginning of the Modern Age?" +
        "\r\n   a) Columbus's discovery of America\r\n    b) Fall of the Roman Empire\r\n    c) French Revolution",
        "Who was the founder of the Carolingian dynasty in the Frankish state?" +
        "\r\n   a) Charlemagne\r\n    b) Charles Martel\r\n    c) Charles the Younger",
        "Which Greek philosopher was the teacher of Alexander the Great?" +
        "\r\n   a) Aristotle\r\n    b) Socrates\r\n    c) Plato",
        "Which dynasty ruled Russia before the Romanovs?" +
        "\r\n   a) Rurikid\r\n    b) Godunov\r\n    c) Petrovich"
    };
        }

        static List<string> GetGeographyCorrectAnswers()
        {
            return new List<string>
    {
        "b",
        "a",
        "b",
        "b",
        "b",
        "c",
        "c",
        "c",
        "b",
        "c",
        "b",
        "a",
        "c",
        "b",
        "a",
        "a",
        "c",
        "b",
        "c",
        "c"
    };
        }

        static List<string> GetBiologyQuestions()
        {
            return new List<string>
    {
        "What is the powerhouse of the cell?" +
        "\r\n   a) Nucleus\r\n   b) Mitochondria\r\n   c) Ribosome",
        "Which gas do plants absorb from the atmosphere?" +
        "\r\n   a) Oxygen\r\n   b) Carbon dioxide\r\n   c) Nitrogen",
        "What is the largest organ in the human body?" +
        "\r\n   a) Liver\r\n   b) Skin\r\n   c) Heart",
        "What is the process by which green plants make their own food?" +
        "\r\n   a) Respiration\r\n   b) Photosynthesis\r\n   c) Digestion",
        "Which gas is responsible for the Earth's ozone layer?" +
        "\r\n   a) Oxygen\r\n   b) Carbon dioxide\r\n   c) Ozone",
        "What is the chemical symbol for water?" +
        "\r\n   a) O2\r\n   b) CO2\r\n   c) H2O",
        "Which gas do humans exhale when they breathe out?" +
        "\r\n   a) Oxygen\r\n   b) Carbon dioxide\r\n   c) Nitrogen",
        "What is the smallest unit of life?" +
        "\r\n   a) Molecule\r\n   b) Cell\r\n   c) Organism",
        "What is the process by which organisms produce offspring?" +
        "\r\n   a) Respiration\r\n   b) Reproduction\r\n   c) Digestion",
        "Which gas do animals inhale from the atmosphere?" +
        "\r\n   a) Oxygen\r\n   b) Carbon dioxide\r\n   c) Nitrogen",
        "What is the study of living organisms called?" +
        "\r\n   a) Astronomy\r\n   b) Geology\r\n   c) Biology",
        "What is the chemical process by which food is broken down into energy?" +
        "\r\n   a) Photosynthesis\r\n   b) Respiration\r\n   c) Digestion",
        "What is the body's first line of defense against infections?" +
        "\r\n   a) Skin\r\n   b) Muscles\r\n   c) Bones",
        "Which gas do humans inhale when they breathe in?" +
        "\r\n   a) Oxygen\r\n   b) Carbon dioxide\r\n   c) Nitrogen",
        "What is the process by which organisms convert food into energy?" +
        "\r\n   a) Respiration\r\n   b) Reproduction\r\n   c) Digestion",
        "What is the chemical building block of life?" +
        "\r\n   a) Cell\r\n   b) DNA\r\n   c) Protein",
        "What is the control center of the cell that contains genetic information?" +
        "\r\n   a) Mitochondria\r\n   b) Nucleus\r\n   c) Ribosome",
        "Which part of the plant is responsible for photosynthesis?" +
        "\r\n   a) Roots\r\n   b) Leaves\r\n   c) Flowers",
        "What is the process by which plants release water vapor into the atmosphere?" +
        "\r\n   a) Transpiration\r\n   b) Respiration\r\n   c) Condensation",
        "What is the process by which organisms produce offspring with traits from both parents?" +
        "\r\n   a) Asexual reproduction\r\n   b) Sexual reproduction\r\n   c) Budding"
    };
        }

        static List<string> GetBiologyCorrectAnswers()
        {
            return new List<string>
    {
        "b",
        "b",
        "b",
        "c",
        "b",
        "b",
        "b",
        "b",
        "a",
        "b",
        "c",
        "c",
        "a",
        "b",
        "a",
        "c",
        "a",
        "b",
        "b",
        "c"
    };
        }

        static void ViewQuizResults()
        {
            Console.Clear();
            List<QuizResult> results = LoadQuizResults();

            foreach (var result in results)
            {
                Console.WriteLine($"Name: {result.Username}");
                Console.WriteLine($"Section: {result.Section}");
                Console.WriteLine($"Correct Answers: {result.CorrectAnswers}");
                Console.WriteLine($"Date: {result.QuizDate}");
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        static void ViewTop20()
        {
            Console.Clear();
            List<QuizResult> results = LoadQuizResults();

            results.Sort((result1, result2) => result2.CorrectAnswers.CompareTo(result1.CorrectAnswers));

            Console.WriteLine("Top 20 quiz results:");

            for (int i = 0; i < Math.Min(20, results.Count); i++)
            {
                var result = results[i];
                Console.WriteLine($"Rank {i + 1}: {result.Username}, Section: {result.Section}, Correct Answers: {result.CorrectAnswers}, Date: {result.QuizDate}");
            }

            Console.ReadKey();
        }

        static void ChangeSettings()
        {
            Console.Clear();
            Console.WriteLine($"Settings for user {currentUser.Username}:");

            while (true)
            {
                Console.WriteLine("1. Change Password");
                Console.WriteLine("2. Change Date of Birth");
                Console.WriteLine("3. Return to Main Menu");
                Console.Write("Select an action: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ChangePassword();
                        break;
                    case "2":
                        ChangeDateOfBirth();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void ChangePassword()
        {
            Console.Clear();
            Console.Write("Enter a new password: ");
            string newPassword = Console.ReadLine();

            currentUser.Password = newPassword;
            Console.WriteLine("Password successfully changed.");
            Console.ReadKey();
        }

        static void ChangeDateOfBirth()
        {
            Console.Clear();
            Console.Write("Enter a new date of birth (yyyy-mm-dd): ");

            if (DateTime.TryParse(Console.ReadLine(), out DateTime newDateOfBirth))
            {
                currentUser.DateOfBirth = newDateOfBirth;
                Console.WriteLine("Date of birth successfully changed.");
            }
            else
            {
                Console.WriteLine("Invalid date format. Change not applied.");
            }

            Console.ReadKey();
        }
    }

    class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }

        public User() { }

        public User(string username, string password, DateTime dateOfBirth)
        {
            Username = username;
            Password = password;
            DateOfBirth = dateOfBirth;
        }
    }

    class QuizResult
    {
        public string Username { get; set; }
        public string Section { get; set; }
        public int CorrectAnswers { get; set; }
        public DateTime QuizDate { get; set; }

        public override string ToString()
        {
            return $"{Username},{Section},{CorrectAnswers},{QuizDate:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
