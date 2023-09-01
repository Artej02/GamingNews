using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YourApiNamespace.Controllers.News
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly List<NewsArticle> _newsArticles = new List<NewsArticle>
        {
            new NewsArticle { Id = 1, Title = "Breaking News 1", Content = "This is the content of breaking news 1." },
            new NewsArticle { Id = 2, Title = "Breaking News 2", Content = "This is the content of breaking news 2." }
        };

        // GET: api/news
        [HttpGet]
        public ActionResult<IEnumerable<NewsArticle>> Get()
        {
            return Ok(_newsArticles);
        }

        // GET: api/news/1
        [HttpGet("{id}")]
        public ActionResult<NewsArticle> Get(int id)
        {
            var article = _newsArticles.FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        // POST: api/news
        [HttpPost]
        public ActionResult<NewsArticle> Post([FromBody] NewsArticle article)
        {
            if (article == null)
            {
                return BadRequest();
            }

            article.Id = _newsArticles.Max(a => a.Id) + 1;
            _newsArticles.Add(article);
            return CreatedAtAction(nameof(Get), new { id = article.Id }, article);
        }

        // PUT: api/news/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] NewsArticle article)
        {
            var existingArticle = _newsArticles.FirstOrDefault(a => a.Id == id);
            if (existingArticle == null)
            {
                return NotFound();
            }

            existingArticle.Title = article.Title;
            existingArticle.Content = article.Content;
            return NoContent();
        }

        // DELETE: api/news/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var article = _newsArticles.FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            _newsArticles.Remove(article);
            return NoContent();
        }
    }

    public class NewsArticle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
