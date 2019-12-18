﻿using GigHub.Controllers;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [ValidDate(ErrorMessage = "La fecha no es válida!")]
        public string Date { get; set; }

        [Required]
        [ValidTime(ErrorMessage = "La hora no es válida!")]
        public string Time { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public string Heading { get; set; }

        public string Action
        {
            get
            {
                Expression<Func<GigsController, ActionResult>> update =
                    (c => c.Update(this));
                Expression<Func<GigsController, ActionResult>> create =
                    (c => c.Create(this));

                var action = Id != 0 ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }

        //public GigFormViewModel()
        //{
        //    Genres = new List<Genre>();
        //}

        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}