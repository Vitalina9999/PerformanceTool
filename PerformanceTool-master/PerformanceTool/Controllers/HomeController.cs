using PerformanceTool.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using HippoValidator.GooglePageSpeedClient;

namespace PerformanceTool.Controllers
{
    public class HomeController : Controller
    {
        private static List<UrlModel> _urlModelList = new List<UrlModel>();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendUrl(UrlModel model)
        {
            if (ModelState.IsValid)
            {
                Stopwatch timer = new Stopwatch();
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(model.Url);

                    timer.Start();
                    var validationResult = new GooglePageSpeedValidator("AIzaSyB9pxTpZhWL3ReyqnEMyAKJGsVtItR3Lp4").Validate(new Uri(model.Url));
                    var score = validationResult.Score;
                    model.ScoreByGoogle = score;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Close();
                    timer.Stop();

                    TimeSpan timeTaken = timer.Elapsed;
                    model.PageLoadSeconds = timeTaken.TotalSeconds;
                }
                catch (Exception)
                {
                    timer.Stop();
                    model.Error = "url is not valid";
                }
            }
            return RedirectToAction("SaveUrlPerformance", model);
        }

        [HttpGet]
        public ActionResult MyChart()
        {
            List<string> xValueList = new List<string>();
            List<double> yValueList = new List<double>();
            _urlModelList.Reverse();

            ViewData["UrlList"] = xValueList;

            foreach (var urlModel in _urlModelList.Take(10))
            {
                xValueList.Add(urlModel.Url);
                yValueList.Add(urlModel.PageLoadSeconds);
            }
            xValueList.Reverse();
            yValueList.Reverse();
            var bytes = new Chart(600, 300)
                .SetXAxis("Url(s)")
                .SetYAxis("Time (secs)")
                .AddSeries(
                    chartType: "Column", legend: "Page load time",
                     xValue: xValueList,
                    yValues: yValueList)
                .GetBytes("png");
            return File(bytes, "image/png");
        }

        public ActionResult SaveUrlPerformance(UrlModel model)
        {
            _urlModelList.Add(model);
            List<UrlModel> urlListCount = _urlModelList.Take(10).ToList();
            ModelState.Clear();
            return View("Index", urlListCount);
        }
    }
}