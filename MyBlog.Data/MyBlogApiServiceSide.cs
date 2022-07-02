using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Interfaces;
using MyBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data
{
    public class MyBlogApiServiceSide: IMyBlogApi
    {
        IDbContextFactory<MyBlogDBContext> factory;
        public MyBlogApiServiceSide(IDbContextFactory<MyBlogDBContext> factory)
        {
            this.factory = factory;
        }

        public async Task DeleteBlogPostAsync(BlogPost item)
        {
            await DeleteItem(item);
        }

        public async Task DeleteCategoryAsync(Category item)
        {
            await DeleteItem(item);
        }

        public async Task DeleteTagAsync(Tag item)
        {
            await DeleteItem(item);
        }

        public async Task<BlogPost> GetBlogPostAsync(int id)
        {
            using var context = factory.CreateDbContext();
            return await context.BlogPosts.Include(p => p.Category).Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<int> GetBlogPostCountAsync()
        {
            using var context = factory.CreateDbContext();
            return context.BlogPosts.CountAsync();
        }

        public async Task<List<BlogPost>> GetBlogPostsAsync(int numberofposts, int startindex)
        {
            using var context = factory.CreateDbContext();
            return await context.BlogPosts.OrderByDescending(p => p.PublishDate).Skip(startindex).Take(numberofposts).ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            using var context = factory.CreateDbContext();
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            using var context = factory.CreateDbContext();
            return await context.Categories.Include(p => p.BlogPosts).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Tag> GetTagAsync(int id)
        {
            using var context = factory.CreateDbContext();
            return await context.Tags.Include(p => p.BlogPosts).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            using var context = factory.CreateDbContext();
            return await context.Tags.ToListAsync();
        }

        public Task<BlogPost> SaveBlogPostAsync(BlogPost item)
        {
            throw new NotImplementedException();
        }

        public Task<Category> SaveCategoryAsync(Category item)
        {
            throw new NotImplementedException();
        }

        public Task<Tag> SaveTagAsync(Tag item)
        {
            throw new NotImplementedException();
        }

        #region Helper Function
        private async Task DeleteItem(IMyBlogItem item)
        {
            using var context = factory.CreateDbContext();
            context.Remove(item);
            await context.SaveChangesAsync();
        }
        #endregion
    }
}
