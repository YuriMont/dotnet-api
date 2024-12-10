using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_dotnet.Data;
using api_dotnet.Interfaces;
using api_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext context;

        public CommentRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> DeleteAsync(int id)
        {
            var existingComment = await context.Comments.FirstOrDefaultAsync(item => item.Id == id);

            if(existingComment == null){
                throw new Exception("Comment does not exist");
            }

            context.Comments.Remove(existingComment);
            await context.SaveChangesAsync();

            return existingComment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await context.Comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(item => item.Id == id);

            if (comment == null)
            {
                throw new Exception("Comment not found");
            }

            return comment;
        }

        public async Task<Comment> UpdateAsync(int id, Comment comment)
        {
            var existingComment = await context.Comments.FirstOrDefaultAsync(item => item.Id == id);

            if(existingComment == null){
                throw new Exception("Comment does not exist");
            }

            existingComment.Title = comment.Title;
            existingComment.Content = comment.Content;

            await context.SaveChangesAsync();

            return existingComment;
        }
    }
}