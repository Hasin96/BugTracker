﻿//using BugTracker.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Task = BugTracker.Models.Task;

//namespace BugTracker.Controllers
//{
//    [Route("api/projects/{projectId}/tasks")]
//    [ApiController]
//    public class TasksApiController : ControllerBase
//    {
//        private BugTrackerContext _context;

//        public TasksApiController(BugTrackerContext context)
//        {
//            _context = context;
//        }

//        //[HttpPut("{taskId:int}")]
//        //public async Task<IActionResult> Put(int projectId, int taskId, BugTracker.Domain.Project.Task newTask)
//        //{
//        //    try
//        //    {
//        //        var project = await _context.Projects.Include(proj => proj.Tasks).SingleOrDefaultAsync(proj => proj.Id == projectId);
//        //        if (project is null) return NotFound($"Couldn't find the project with the project id of : ${projectId}");

//        //        if (taskId != newTask.Id)
//        //            return BadRequest("task id does not match task object in request");

//        //        project.UpdateTaskStatus(newTask);

//        //        var task = project.Tasks.SingleOrDefault(t => t.Id == taskId);
//        //        if (task is null) return NotFound($"Couldn't find the talk with the task id of : ${taskId} with the project id of : ${projectId}");

//        //        await _context.SaveChangesAsync();

//        //        return Ok(task);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure, failed to change tasks");
//        //    }
//        //}
//    }
//}
