﻿using BookRef.Api.Models;
using BookRef.Api.Persistence;

namespace BookRef.Api.Extensions
{
    public class SampleDataSeeder
    {
        private readonly BookRefDbContext _context;

        public SampleDataSeeder(
            BookRefDbContext context) =>
            _context = context;

        public void SeedAll()
        {
            // var story = new UserStory(
            //     "My first story",
            //     Priority.Create(1).Value,
            //     "This is my content",
            //     "This must be fulfilled",
            //     BusinessValue.BV900,
            //     UserStory.Relevance.ShouldHave);

            // story.AddTask(new StoryTask("Do this for me now!"));
            // story.AddTask(new StoryTask("And now this!"));

            // _context.Stories.Add(story);

            // _context.Stories.Add(
            //     new UserStory(
            //         "Second Story",
            //         Priority.Create(2).Value,
            //         "As administrator i want ...",
            //         "Working CI/CD pipeline",
            //         BusinessValue.BV1000,
            //         UserStory.Relevance.MustHave
            //     ));
            // _context.SaveChanges();
        }
    }
}
