﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Models
{
    public class Post
    {
        public int postID {  get; set; }
        public int userID { get; set; }
        public string userName { get; set; }
        public string postTitle {  get; set; }
        public string postContent {  get; set; }
        public string postCategory {  get; set; }
        public DateTime postTime { get; set; }
        public int likesCount {  get; set; }
        public int forwardCount {  get; set; }
        public int commentsCount {  get; set; }
        public int refrencePostID { get; set; }
    }
}