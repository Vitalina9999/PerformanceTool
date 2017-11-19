using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PerformanceTool.Models
{
    public class UrlModel
    {
        public string Url { get; set; }

        public double PageLoadSeconds { get; set; }

        public int ScoreByGoogle { get; set; }

        public string Error { get; set; }
    }
}