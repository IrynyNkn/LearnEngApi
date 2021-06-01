using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TgBotApi.Models;

namespace TgBotApi.Data
{
    public interface IDataHandleRepo
    {
        bool SaveChanges();
        Task<string> SmallTalk(string phrase);
        Task<Grammar> CheckGrammar(string expression);
        Task<ApiResponse> LoadWordInfo(string word);
        Task<Translation> Translate(string exp, string toLang);
        void DeleteVocabItem(VocabItem vocabItem, int id);
        Task<WordQuiz> PlayWordQuiz(string level);
        Task<NewWord> LearnNewWord();
        Task<IEnumerable<UserVocab>> GetAllUserVocabs();
        UserVocab GetById(int id);
        void CreateUserVocab(UserVocab userVocab);
        void AddVocabItem(VocabItem vocab, int id);
    }
}
