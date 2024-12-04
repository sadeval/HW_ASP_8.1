using HW_ASP_8._1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HW_ASP_8._1.Controllers
{
    public class SecretSantaController : Controller
    {
        private static List<Participant> _participants = new();

        [HttpGet]
        public IActionResult Index()
        {
            return View(_participants);
        }

        [HttpPost]
        public IActionResult AddParticipant(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _participants.Add(new Participant { Name = name });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Shuffle()
        {
            if (_participants.All(p => !string.IsNullOrEmpty(p.AssignedTo)))
            {
                return RedirectToAction("Result");
            }

            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public IActionResult ShuffleParticipants()
        {
            var names = _participants.Select(p => p.Name).ToList();
            var random = new Random();

            foreach (var participant in _participants)
            {
                var availableNames = names.Where(name => name != participant.Name).ToList();
                if (!availableNames.Any())
                {
                    return RedirectToAction("ShuffleParticipants"); 
                }

                var assignedTo = availableNames[random.Next(availableNames.Count)];
                participant.AssignedTo = assignedTo;
                names.Remove(assignedTo);
            }

            return RedirectToAction("Result");
        }

        public IActionResult Result()
        {
            return View(_participants);
        }
    }
}
