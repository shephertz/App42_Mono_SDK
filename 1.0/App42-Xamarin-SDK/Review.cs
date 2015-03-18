using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.review
{
    public class Review : App42Response
    {

        public String userId;
        public String itemId;
        public String status;
        public String reviewId;
        public String comment;
        public Double rating;
        public DateTime createdOn;

        public String GetUserId()
        {
            return userId;
        }
        public void SetUserId(String userId)
        {
            this.userId = userId;
        }
        public String GetItemId()
        {
            return itemId;
        }
        public void SetItemId(String itemId)
        {
            this.itemId = itemId;
        }
        public String GetStatus()
        {
            return status;
        }
        public void SetStatus(String status)
        {
            this.status = status;
        }
        public String GetReviewId()
        {
            return reviewId;
        }
        public void SetReviewId(String reviewId)
        {
            this.reviewId = reviewId;
        }
        public String GetComment()
        {
            return comment;
        }
        public void SetComment(String comment)
        {
            this.comment = comment;
        }
        public Double GetRating()
        {
            return rating;
        }
        public void SetRating(Double rating)
        {
            this.rating = rating;
        }
        public DateTime GetCreatedOn()
        {
            return createdOn;
        }
        public void SetCreatedOn(DateTime createdOn)
        {
            this.createdOn = createdOn;
        }

        public String GetStringView()
        {
            return "UserId :" + userId + " : ItemId : " + itemId + " : Status : " + status + " : ReviewId : " + reviewId + " : Comment : " + comment + " : Rating : " + rating + " : CreatedOn : " + createdOn;
        }
    }
}