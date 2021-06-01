using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TgBotApi.Models;

namespace TgBotApi.Data
{
    public class DataHandleRepo : IDataHandleRepo
    {
        private readonly EngDbContext _engDbContext;

        private readonly string ApiKey = "5f64485dccmsh3d8946bd8aade82p17cd4ajsnbbef9aa68c8e";

        public DataHandleRepo(EngDbContext engDbContext)
        {
            _engDbContext = engDbContext;
        }

        public void AddVocabItem(VocabItem vocab, int id)
        {
            _engDbContext.UserVocabs.Include(t => t.VocabItems).FirstOrDefault(t => t.ChatId == id).VocabItems.Add(vocab);
        }

        public async Task<Grammar> CheckGrammar(string expression)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://grammarbot.p.rapidapi.com/check"),
                Headers =
                {
                    { "x-rapidapi-key", $"{ApiKey}" },
                    { "x-rapidapi-host", "grammarbot.p.rapidapi.com" },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "text", $"{expression}" },
                    { "language", "en-US" },
                }),
            };
            using (var response = await ApiHelper.ApiClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Grammar>(body);
                return result;
            }

        }

        public void CreateUserVocab(UserVocab userVocab)
        {
            
            if (userVocab == null)
            {
                throw new ArgumentNullException(nameof(userVocab));
            }

            _engDbContext.UserVocabs.Add(userVocab);
        }

        public void DeleteVocabItem(VocabItem vocabItem, int id)
        {
            if(vocabItem == null)
            {
                throw new ArgumentException(nameof(vocabItem));
            }


            _engDbContext.UserVocabs.Include(t => t.VocabItems).FirstOrDefault(t => t.ChatId == id).VocabItems.Remove(vocabItem);
        }

        public async Task<IEnumerable<UserVocab>> GetAllUserVocabs()
        {
            return await _engDbContext.UserVocabs.Include(t => t.VocabItems).ToListAsync();
        }

        public UserVocab GetById(int id)
        {
            return _engDbContext.UserVocabs.Include(t => t.VocabItems).FirstOrDefault(t => t.ChatId == id);
        }

        public async Task<NewWord> LearnNewWord()
        {
            var quizWords = await PlayWordQuiz("4");

            string exp = quizWords.Quizlist[0].Option[0];

            var translate = (await Translate(exp, "uk"));

            var transcription = LoadWordInfo(exp)?.Result?.Explanation ?? "not specified";

            

            var selectWord = new NewWord
            {
                EnglishWord = exp,
                Translation = translate.data.translation,
                Explanation = transcription
            };

            return selectWord;
        }

        public async Task<ApiResponse> LoadWordInfo(string word)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://lingua-robot.p.rapidapi.com/language/v1/entries/en/" + word.ToLower()),
                Headers =
                {
                    { "x-rapidapi-key", "5f64485dccmsh3d8946bd8aade82p17cd4ajsnbbef9aa68c8e" },
                    { "x-rapidapi-host", "lingua-robot.p.rapidapi.com" },
                },
            };
            using (var response = await ApiHelper.ApiClient.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    Expression info = await response.Content.ReadAsAsync<Expression>();

                    var tgResponse = new ApiResponse
                    {
                        Definition = new List<string>(),
                        Usage = new List<string>(),
                        Synonyms = new List<string>(),
                        Antonyms = new List<string>()
                    };

                    if (info?.entries != null && info.entries.Count != 0)
                    {
                        tgResponse.Word = info.entries.FirstOrDefault().entry;
                    }
                    else
                    {
                        return null;
                    }

                    if(info?.entries?.FirstOrDefault()?.pronunciations?.FirstOrDefault()?.transcriptions?.FirstOrDefault()?.transcription != null)
                    {
                        tgResponse.Explanation = info?.entries?.FirstOrDefault()?.pronunciations?.FirstOrDefault()?.transcriptions?.FirstOrDefault()?.transcription;
                    }

                    foreach (var el in info.entries)
                    {
                        if (el.lexemes.Count != 0)
                        {
                            foreach (var el1 in el.lexemes)
                            {
                                if (el1.senses.Count != 0)
                                {
                                    foreach (var el2 in el1.senses)
                                    {
                                        if (el2.definition != null)
                                        {
                                            tgResponse.Definition.Add(el2.definition);
                                        }
                                        if (el2.usageExamples != null)
                                        {
                                            tgResponse.Usage.AddRange(el2.usageExamples);
                                        }
                                    }
                                }

                                if (el1?.antonymSets != null && el1.antonymSets.Count != 0)
                                {
                                    foreach (var ant in el1.antonymSets)
                                    {
                                        if (ant?.antonyms != null && ant.antonyms.Count!=0)
                                        {
                                            tgResponse.Antonyms.AddRange(ant.antonyms);
                                        }
                                    }
                                }

                                if (el1.synonymSets != null && el1.synonymSets.Count != 0)
                                {
                                    foreach (var syn in el1.synonymSets)
                                    {
                                        if (syn.synonyms != null && syn.synonyms.Count != 0)
                                        {
                                            tgResponse.Synonyms.AddRange(syn.synonyms);
                                        }
                                    }
                                }

                            }
                        }
                    };

                    return tgResponse;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<WordQuiz> PlayWordQuiz(string level)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://twinword-word-association-quiz.p.rapidapi.com/type1/?level={level}"),
                Headers =
                {
                    { "x-rapidapi-key", "5f64485dccmsh3d8946bd8aade82p17cd4ajsnbbef9aa68c8e" },
                    { "x-rapidapi-host", "twinword-word-association-quiz.p.rapidapi.com" },
                },
            };

            using (var response = await ApiHelper.ApiClient.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    WordQuiz info = await response.Content.ReadAsAsync<WordQuiz>();

                    return info;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public bool SaveChanges()
        {
            return (_engDbContext.SaveChanges() >= 0);
        }

        public async Task<string> SmallTalk(string phrase)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://smalltalk-nlu.p.rapidapi.com/api/v1/smalltalk"),
                Headers =
                {
                    { "x-rapidapi-key", $"{ApiKey}" },
                    { "x-rapidapi-host", "smalltalk-nlu.p.rapidapi.com" },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "query", $"{phrase}" },
                    { "languageCode", "en-US" },
                }),
            };
            using (var response = await ApiHelper.ApiClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsAsync<Talk>();
                return body.fulfillmentMessages.text.FirstOrDefault();
            }
        }

        public async Task<Translation> Translate(string exp, string toLang)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://google-translate20.p.rapidapi.com/translate?text={exp}&tl={toLang}"),
                Headers =
                {
                    { "x-rapidapi-key", "5f64485dccmsh3d8946bd8aade82p17cd4ajsnbbef9aa68c8e" },
                    { "x-rapidapi-host", "google-translate20.p.rapidapi.com" },
                },
            };
            using (var response = await ApiHelper.ApiClient.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    Translation info = await response.Content.ReadAsAsync<Translation>();
                    
                    return info;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
