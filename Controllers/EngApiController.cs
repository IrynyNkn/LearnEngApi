using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TgBotApi.Data;
using TgBotApi.Models;

namespace TgBotApi.Controllers
{
    public class EngApiController : Controller
    {
        private readonly IDataHandleRepo _repo;

        public EngApiController(IDataHandleRepo repo)
        {
            ApiHelper.InitializeClient();
            _repo = repo;
        }

        [Route("api/engapi/word")]
        [HttpGet]
        public ActionResult<Expression> GetWordInfo([FromQuery]string name)
        {
            var wordInfo = _repo.LoadWordInfo(name);

            if(wordInfo == null)
            {
                return NotFound(wordInfo.Result);
            }

            return Ok(wordInfo.Result);
        }

        [Route("api/engapi/quiz")]
        [HttpGet]
        public ActionResult<Expression> PlayWordGame([FromQuery] string level)
        {
            var quiz = _repo.PlayWordQuiz(level);

            if (quiz == null)
            {
                return NotFound(quiz.Result);
            }

            return Ok(quiz.Result);
        }

        [Route("api/engapi/translate")]
        [HttpGet]
        public ActionResult<Expression> GetTranslate([FromQuery] Phrase phrase)
        {
            var translation = _repo.Translate(phrase.PhraseItself, phrase.ToLang);
            return Ok(translation.Result);
        }

        [Route("api/engapi/grammar")]
        [HttpGet]
        public ActionResult<Grammar> GrammarCheck([FromQuery] string exp)
        {
            var checkedForMistakes = _repo.CheckGrammar(exp);

            return Ok(checkedForMistakes.Result);
        }

        [Route("api/engapi/talk")]
        [HttpGet]
        public ActionResult<string> SmallTalk([FromQuery] string phrase)
        {
            var response = _repo.SmallTalk(phrase);

            return Ok(response.Result);
        }


        [Route("api/engapi")]
        [HttpGet]
        public async Task<IEnumerable<UserVocab>> GetAllUserVocabs()
        {
            // async Task<IEnumerable<UserVocab>>
            var userVocabs = await _repo.GetAllUserVocabs();

            return userVocabs;
        }

        [Route("api/engapi/new")]
        [HttpGet]
        public async Task<NewWord> LearnNewWord()
        {
            var word = await _repo.LearnNewWord();

            return word;
        }

        [Route("api/engapi/{id}")]
        [HttpGet]
        public UserVocab GetById(int id)
        {
            var vocab =  _repo.GetById(id);
            return vocab;
        }

        [Route("api/engapi")]
        [HttpPost]
        public ActionResult<UserVocab> CreateUserVocab([FromBody]UserVocab crUserVocab)
        {
            if(crUserVocab.ChatId == 0)
            {
                throw new ArgumentNullException(nameof(crUserVocab));
            }
            _repo.CreateUserVocab(crUserVocab);
            _repo.SaveChanges();

            return Ok(crUserVocab);
        }

        [Route("api/engapi/{id}")]
        [HttpPost]
        public ActionResult AddVocabItem(int id, [FromBody]VocabItem vocabItem)
        {
            _repo.AddVocabItem(vocabItem, id);
            _repo.SaveChanges();
            return NoContent();
        }

        [Route("api/engapi/{id}")]
        [HttpDelete]
        public ActionResult DeleteVocabItem(int id, [FromBody] VocabItem vocabItem)
        {
            var commandModelFind = _repo.GetById(id);
            if(commandModelFind == null)
            {
                return NotFound();
            }

            foreach(var vocItem in commandModelFind.VocabItems)
            {
                if (vocabItem.EnglishWord == vocItem.EnglishWord)
                {
                    _repo.DeleteVocabItem(vocItem, id);
                    _repo.SaveChanges();

                    return NoContent();
                }
            }

            return NotFound();
        }

        [Route("api/engapi/{id}")]
        [HttpPatch]
        public ActionResult PartialInfoUpdate(int id,[FromBody] JsonPatchDocument<UserVocab> patchDoc)
        {
            var userVocabModel = _repo.GetById(id);
            userVocabModel.VocabItems = userVocabModel.VocabItems.OrderBy(ent => ent.VocabItemId).ToList();
            if (userVocabModel == null)
            {
                return NotFound();
            }
            patchDoc.ApplyTo(userVocabModel, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.SaveChanges();
            return NoContent();
        }
    }
}
