using PhinaMart.ViewModels;

namespace PhinaMart.Services
{
    public interface CommentService
    {
        public bool CreateComment(CreateComment createComment,CreateRating createRating,int id);
        public bool CreateCompare(int id);

        public bool DeleteCompare(int id);

    }
}
