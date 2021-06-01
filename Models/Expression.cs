using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TgBotApi.Models
{
    
    public class Expression
    {
        public List<Entry> entries { get; set; }   
    }

    public class Transcription
    {
        public string transcription { get; set; }
    }

    public class Audio
    {
        public string url { get; set; }
    }

    public class Pronunciation
    {
        public List<Transcription> transcriptions { get; set; }
        public Audio audio { get; set; }
    }

    public class Sens
    {
        public string definition { get; set; }
        public List<string> usageExamples { get; set; }
    }

    public class SynonymSet
    {
        public List<string> synonyms { get; set; }
    }

    public class AntonymSet
    {
        public List<string> antonyms { get; set; }
    }

    public class Lexeme
    {
        public string partOfSpeech { get; set; }
        public List<Sens> senses { get; set; }
        public List<SynonymSet> synonymSets { get; set; }
        public List<AntonymSet> antonymSets { get; set; }
    }

    public class Entry
    {
        public string entry { get; set; }
        public List<Pronunciation> pronunciations { get; set; }
        public List<Lexeme> lexemes { get; set; }
    }

}
